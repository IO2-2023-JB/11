using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using YouTubeV2.Api.Enums;
using YouTubeV2.Api.Tests.Providers;
using YouTubeV2.Application;
using YouTubeV2.Application.Model;

namespace YouTubeV2.Api.Tests.CommentControllerTests
{
    [TestClass]
    public class RemoveCommentResponseAsyncTests
    {
        private WebApplicationFactory<Program> _webApplicationFactory = null!;
        private User _user = null!;
        private readonly User _notCommentOwner = new()
        {
            Email = "notOwner@mail.com",
            UserName = "notOwnerUsername",
            Name = "notOwnerName",
            Surname = "notOwnerSurname",
        };
        private readonly User _notCommentOwner = new()
        {
            Email = "notOwner@mail.com",
            UserName = "notOwnerUsername",
            Name = "notOwnerName",
            Surname = "notOwnerSurname",
        };
        private Video _video = null!;
        private Comment _comment = null!;
        private string _commentOwnerId = null!;
        private Guid _commentId;
        private CommentResponse _commentResponse1 = null!;
        private CommentResponse _commentResponse2 = null!;

        [TestInitialize]
        public async Task Initialize()
        {
            _user = new()
            {
                Email = "owner@mail.com",
                UserName = "ownerUsername",
                Name = "ownerName",
                Surname = "ownerSurname",
            };

            _commentResponse1 = new CommentResponse()
            {
                Content = "comment response1",
                CreateDate = DateTimeOffset.UtcNow,
                Author = _user,
            };

            _commentResponse2 = new CommentResponse()
            {
                Content = "comment response2",
                CreateDate = DateTimeOffset.UtcNow,
                Author = _user,
            };

            _comment = new()
            {
                Content = "comment content",
                CreateDate = DateTimeOffset.UtcNow,
                Author = _user,
                Responses = new List<CommentResponse>()
                {
                    _commentResponse1,
                    
                },
            };

            _video = new()
            {
                Title = "test title",
                Description = "test description",
                Visibility = Visibility.Public,
                UploadDate = DateTimeOffset.UtcNow,
                EditDate = DateTimeOffset.UtcNow,
                Author = _user,
                Comments = new[]
                {
                    _comment,
                }
            };

            _webApplicationFactory = Setup.GetWebApplicationFactory();
            var config = _webApplicationFactory.Services.GetService<IConfiguration>();
            var connection = config!.GetConnectionString("Db");
            await Setup.ResetDatabaseAsync(connection!);

            await _webApplicationFactory.DoWithinScope<UserManager<User>>(
              async userManager =>
              {
                  await userManager.CreateAsync(_user);
                  await userManager.CreateAsync(_notCommentOwner);
                  _commentOwnerId = await userManager.GetUserIdAsync(_user);
              });

            await _webApplicationFactory.DoWithinScope<YTContext>(
                async context =>
                {
                    context.Users.Attach(_user);
                    var video = await context.Videos.AddAsync(_video);
                    await context.SaveChangesAsync();
                    _commentId = video.Entity.Comments.Single(comment => comment.Content == _comment.Content).Id;
                });
        }

        [TestMethod]
        public async Task RemoveCommentAsync_WhenYouAreTheOwner_ShouldRemoveCommentAndItsResponsesFromDataBase()
        {
            // ARRANGE
            using HttpClient httpClient = _webApplicationFactory.WithAuthentication(ClaimsProvider.WithRoleAccessAndUserId(Role.Creator, _commentOwnerId)).CreateClient();

            // ACT
            var httpResponseMessage = await httpClient.DeleteAsync($"comment?id={_commentId}");

            // ASSERT
            httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);

            await _webApplicationFactory.DoWithinScope<YTContext>(
                async context =>
                {
                    Comment? deletedComment = await context.Comments.FindAsync(_commentId);
                    deletedComment.Should().BeNull();

                    var deletedCommentResponses = await context.CommentResponses.Where(commentResponse => commentResponse.RespondOn.Id == _commentId).ToListAsync();
                    deletedCommentResponses.Should().BeEmpty();

                    Comment? notDeletedComment = await context.Comments.Include(comment => comment.Responses).FirstOrDefaultAsync(comment => comment.Id == _comment2Id);
                    notDeletedComment.Should().NotBeNull();
                    notDeletedComment!.Responses.Should().HaveCount(1);
                });
        }

        [TestMethod]
        public async Task RemoveCommentAsync_WhenYouAreNotTheOwnerButAdministrator_ShouldRemoveCommentAndItsResponsesFromDataBase()
        {
            // ARRANGE
            using HttpClient httpClient = _webApplicationFactory.WithAuthentication(ClaimsProvider.WithRoleAccessAndUserId(Role.Administrator, _notCommentOwnerId)).CreateClient();

            // ACT
            var httpResponseMessage = await httpClient.DeleteAsync($"comment?id={_commentId}");

            // ASSERT
            httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);

            await _webApplicationFactory.DoWithinScope<YTContext>(
                async context =>
                {
                    Comment? deletedComment = await context.Comments.FindAsync(_commentId);
                    deletedComment.Should().BeNull();

                    var deletedCommentResponses = await context.CommentResponses.Where(commentResponse => commentResponse.RespondOn.Id == _commentId).ToListAsync();
                    deletedCommentResponses.Should().BeEmpty();

                    Comment? notDeletedComment = await context.Comments.Include(comment => comment.Responses).FirstOrDefaultAsync(comment => comment.Id == _comment2Id);
                    notDeletedComment.Should().NotBeNull();
                    notDeletedComment!.Responses.Should().HaveCount(1);
                });
        }

        [TestMethod]
        public async Task RemoveCommentAsync_WhenYouAreNotTheOwnerNeitherAdministrator_ShouldReturnForbidden()
        {
            // ARRANGE
            using HttpClient httpClient = _webApplicationFactory.WithAuthentication(ClaimsProvider.WithRoleAccessAndUserId(Role.Simple, _notCommentOwnerId)).CreateClient();

            // ACT
            var httpResponseMessage = await httpClient.DeleteAsync($"comment?id={_commentId}");

            // ASSERT
            httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}
