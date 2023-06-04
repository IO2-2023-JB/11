using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Channels;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;
using YouTubeV2.Application.Enums;
using YouTubeV2.Application.FileInspector;
using YouTubeV2.Application.Jobs;
using YouTubeV2.Application.Model;
using YouTubeV2.Application.Services.BlobServices;

namespace YouTubeV2.Application.Services.VideoServices
{
    public class VideoProcessingService : BackgroundService, IVideoProcessingService
    {
        private readonly Channel<VideoProcessJob> _videoProcessingChannel = Channel.CreateUnbounded<VideoProcessJob>();
        private readonly IServiceScopeFactory _serviceScopeFactory;
        IHostApplicationLifetime _hostApplicationLifetime;
        private IVideoService _videoService = null!;
        private IBlobVideoService _blobVideoService = null!;
        private IFileInspector _fileInspector = null!;
        private const string _mp4Extension = ".mp4";

        public VideoProcessingService(IServiceScopeFactory serviceScopeFactory, IHostApplicationLifetime hostApplicationLifetime)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _hostApplicationLifetime = hostApplicationLifetime;
        }

        public async ValueTask EnqueVideoProcessingJobAsync(VideoProcessJob videoProcessJob) => await _videoProcessingChannel.Writer.WriteAsync(videoProcessJob);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await FFmpegDownloader.GetLatestVersion(FFmpegVersion.Official, Directory.GetCurrentDirectory());
            FFmpeg.SetExecutablesPath(Directory.GetCurrentDirectory());

            await WaitForApplicationStarted();

            using var serviceScope = _serviceScopeFactory.CreateScope();
            _videoService = serviceScope.ServiceProvider.GetRequiredService<IVideoService>();
            _blobVideoService = serviceScope.ServiceProvider.GetRequiredService<IBlobVideoService>();
            _fileInspector = serviceScope.ServiceProvider.GetRequiredService<IFileInspector>();

            await foreach (var videoProcessJob in _videoProcessingChannel.Reader.ReadAllAsync(stoppingToken))
            {
                await ConvertToMP4AndUploadVideoAsync(videoProcessJob, stoppingToken);
            }
        }

        private Task WaitForApplicationStarted()
        {
            var completionSource = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            _hostApplicationLifetime.ApplicationStarted.Register(() => completionSource.TrySetResult());
            return completionSource.Task;
        }

        private async Task ConvertToMP4AndUploadVideoAsync(VideoProcessJob videoProcessJob, CancellationToken cancellationToken)
        {
            Video? video = null;

            try
            {
                video = await _videoService.GetVideoByIdAsync(videoProcessJob.VideoId, cancellationToken);

                if (video == null) return;

                if (video.ProcessingProgress != ProcessingProgress.Uploading) return;

                await SetVideoLengthAsync(video, videoProcessJob.Path, cancellationToken);

                if (videoProcessJob.Extension == _mp4Extension)
                {
                    using FileStream videoStream = _fileInspector.OpenRead(videoProcessJob.Path);
                    await _blobVideoService.UploadVideoAsync(videoProcessJob.VideoId.ToString(), videoStream, cancellationToken);
                    await _videoService.SetVideoProcessingProgressAsync(video, ProcessingProgress.Ready, cancellationToken);
                    videoStream.Close();
                    _fileInspector.Delete(videoProcessJob.Path);
                    return;
                }

                await _videoService.SetVideoProcessingProgressAsync(video, ProcessingProgress.Processing, cancellationToken);

                var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), $"{videoProcessJob.VideoId}{_mp4Extension}");

                var conversion = FFmpeg.Conversions.New()
                    .AddParameter($"-i \"{videoProcessJob.Path}\"")
                    .SetOutput(outputFilePath)
                    .SetOverwriteOutput(true);

                await conversion.Start(cancellationToken);

                _fileInspector.Delete(videoProcessJob.Path);

                await using var outputFileStream = _fileInspector.OpenRead(outputFilePath);
                await _blobVideoService.UploadVideoAsync(videoProcessJob.VideoId.ToString(), outputFileStream, cancellationToken);
                outputFileStream.Close();
                _fileInspector.Delete(outputFilePath);

                await _videoService.SetVideoProcessingProgressAsync(video, ProcessingProgress.Ready, cancellationToken);
            }
            catch
            {
                if (video != null)
                    await _videoService.SetVideoProcessingProgressAsync(video, ProcessingProgress.FailedToUpload, cancellationToken);
            }
        }

        private async Task SetVideoLengthAsync(Video video, string path, CancellationToken cancellationToken)
        {
            var mediaInfo = await FFmpeg.GetMediaInfo(path, cancellationToken);
            await _videoService.SetVideoLengthAsync(video, mediaInfo.Duration.TotalSeconds, cancellationToken);
        }
    }
}
