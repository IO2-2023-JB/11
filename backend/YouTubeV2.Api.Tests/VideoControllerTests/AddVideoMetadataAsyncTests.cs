using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YouTubeV2.Api.Enums;
using YouTubeV2.Application.DTO;
using YouTubeV2.Application.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;
using Moq;
using YouTubeV2.Application.Services;
using System.Net;
using FluentAssertions;

namespace YouTubeV2.Api.Tests.VideoControllerTests
{
    [TestClass]
    public class AddVideoMetadataAsyncTests
    {
        private WebApplicationFactory<Program> _webApplicationFactory = null!;
        private Mock<IUserService> _userServiceMock = new();
        private User _user = new();

        [TestInitialize]
        public async Task Initialize()
        {
            _userServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(_user);
            _webApplicationFactory = Setup.GetWebApplicationFactory(_userServiceMock);
            var config = _webApplicationFactory.Services.GetService<IConfiguration>();
            var connection = config!.GetConnectionString("Db");
            await Setup.ResetDatabaseAsync(connection!);
        }

        [TestMethod]
        public async Task AddVideoMetadataAsync_ValidInput_ReturnsOk()
        {
            // ARRANGE
            VideoMetadataPostDTO videoMetadata = new(
            "Test Video Title",
                "Test Video Description",
                "data:image/png;base64,iVBORw0KGg==",
                new[] { "test tag1", "test tag2" },
                Visibility.Public);

            string userId;
            await _webApplicationFactory.DoWithinScope<UserManager<User>>(
              async userManager =>
              {
                  await userManager.CreateAsync(_user);
                  userId = await userManager.GetUserIdAsync(_user);
              });

            using var httpClient = _webApplicationFactory.CreateClient();

            // ACT
            var httpResponseMessage = await httpClient.PostAsync("video-metadata", new StringContent(JsonConvert.SerializeObject(videoMetadata), Encoding.UTF8, "application/json"));

            // ASSERT
            httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
