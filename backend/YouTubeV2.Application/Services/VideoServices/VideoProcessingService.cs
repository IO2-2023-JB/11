using Microsoft.Extensions.Hosting;
using System.Threading.Channels;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;
using YouTubeV2.Application.Enums;
using YouTubeV2.Application.Jobs;
using YouTubeV2.Application.Model;
using YouTubeV2.Application.Services.BlobServices;

namespace YouTubeV2.Application.Services.VideoServices
{
    public class VideoProcessingService : BackgroundService, IVideoProcessingService
    {
        private readonly Channel<VideoProcessJob> _videoProcessingChannel = Channel.CreateUnbounded<VideoProcessJob>();
        private readonly IBlobVideoService _blobVideoService;
        private readonly IVideoService _videoService;
        private const string _mp4Extension = ".mp4";

        public VideoProcessingService(IBlobVideoService blobVideoService, IVideoService videoService)
        {
            _blobVideoService = blobVideoService;
            _videoService = videoService;
        }

        public async ValueTask EnqueVideoProcessingJobAsync(VideoProcessJob videoProcessJob) => await _videoProcessingChannel.Writer.WriteAsync(videoProcessJob);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await FFmpegDownloader.GetLatestVersion(FFmpegVersion.Official);
            await foreach (var videoProcessJob in _videoProcessingChannel.Reader.ReadAllAsync(stoppingToken))
            {
                await ConvertToMP4AndUploadVideoAsync(videoProcessJob, stoppingToken);
            }
        }

        private async Task ConvertToMP4AndUploadVideoAsync(VideoProcessJob videoProcessJob, CancellationToken cancellationToken)
        {
            Video? video = null;

            try
            {
                video = await _videoService.GetVideoByIdAsync(videoProcessJob.VideoId, cancellationToken);

                if (video == null) return;

                if (videoProcessJob.Extension == _mp4Extension)
                {

                    await _blobVideoService.UploadVideoAsync(videoProcessJob.VideoId.ToString(), videoProcessJob.VideoStream, cancellationToken);
                    await _videoService.SetVideoProcessingProgressAsync(video, ProcessingProgress.Ready, cancellationToken);
                    return;
                }

                await _videoService.SetVideoProcessingProgressAsync(video, ProcessingProgress.Processing, cancellationToken);

                var inputFilePath = Path.GetTempFileName() + videoProcessJob.Extension;
                var outputFilePath = Path.GetTempFileName() + _mp4Extension;

                await using var inputFileStream = File.Create(inputFilePath);
                await videoProcessJob.VideoStream.CopyToAsync(inputFileStream, cancellationToken);

                var conversion = await FFmpeg.Conversions.FromSnippet.Convert(inputFilePath, outputFilePath);
                await conversion.Start(cancellationToken);

                File.Delete(inputFilePath);

                using var outputFileStream = File.OpenRead(outputFilePath);
                await _blobVideoService.UploadVideoAsync(videoProcessJob.VideoId.ToString(), outputFileStream, cancellationToken);
                File.Delete(outputFilePath);
                await _videoService.SetVideoProcessingProgressAsync(video, ProcessingProgress.Ready, cancellationToken);
            }
            catch
            {
                if (video != null)
                    await _videoService.SetVideoProcessingProgressAsync(video, ProcessingProgress.FailedToUpload, cancellationToken);
            }
        }
    }
}
