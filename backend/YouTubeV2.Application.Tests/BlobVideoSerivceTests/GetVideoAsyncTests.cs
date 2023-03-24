using Azure;
using Azure.Storage.Blobs;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Text;
using YouTubeV2.Application.Configurations.BlobStorage;
using YouTubeV2.Application.Services.AzureServices.BlobServices;

namespace YouTubeV2.Application.Tests.BlobVideoSerivceTests
{
    [TestClass]
    public class GetVideoAsyncTests
    {
        private Mock<BlobServiceClient> _blobServiceClientMock = new Mock<BlobServiceClient>();
        private Mock<BlobContainerClient> _blobContainerClientMock = new Mock<BlobContainerClient>();
        private Mock<BlobClient> _blobClientMock = new Mock<BlobClient>();
        private BlobVideoService _blobVideoService = null!;
        private byte[] _entireStreamBuffer = null!;


        [TestInitialize]
        public void Initialize()
        {
            _entireStreamBuffer = Encoding.UTF8.GetBytes("testStreamContent");
            _blobClientMock
                .Setup(x => x.DownloadToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Callback<Stream, CancellationToken>(async (stream, cancellationToken) =>
                {
                    await stream.WriteAsync(_entireStreamBuffer, 0, _entireStreamBuffer.Length, cancellationToken);
                    await stream.FlushAsync(cancellationToken);
                });
            _blobClientMock
                .Setup(x => x.ExistsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Response.FromValue(true, new Mock<Response>().Object));
            _blobContainerClientMock.Setup(x => x.GetBlobClient(It.IsAny<string>())).Returns(_blobClientMock.Object);
            _blobServiceClientMock
                .Setup(x => x.GetBlobContainerClient(It.IsAny<string>()))
                .Returns(_blobContainerClientMock.Object);
            _blobVideoService = new BlobVideoService(
                _blobServiceClientMock.Object,
                Options.Create(new BlobStorageVideosConfig { VideosContainerName = It.IsAny<string>() }));
        }

        [TestMethod]
        public async Task GetVideoTryingToGetBytesStartingAndEndingInRangeOfStreamShouldReturnFullBuffer()
        {
            // ARRANGE
            System.Range range = new System.Range(1, 4);

            // ACT
            byte[] buffer = await _blobVideoService.GetVideoAsync(It.IsAny<string>(), range);

            // ASSERT
            buffer.Length.Should().Be(range.End.Value - range.Start.Value + 1);
            for (int i = range.Start.Value; i <= range.End.Value; i++)
                buffer[i - range.Start.Value].Should().Be(_entireStreamBuffer[i]);
        }
    }
}
