using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Web;
using YouTubeV2.Application.DTO;
using YouTubeV2.Application.Model;

namespace YouTubeV2.Api.Tests
{
    [TestClass]
    public class UserTests
    {
        private WebApplicationFactory<Program> _webApplicationFactory;

        [TestInitialize]
        public async Task Initialize()
        {
            _webApplicationFactory = Setup.GetWebApplicationFactory();
            var config = _webApplicationFactory.Services.GetService<IConfiguration>();
            var connection = config.GetConnectionString("Db");
            await Setup.ResetDatabaseAsync(connection);
        }

        [TestMethod]
        public async Task RegisterShouldAddToDB()
        {
            // Arrange
            var httpClient = _webApplicationFactory.CreateClient();

            var registerDto = new RegisterDto("mail@mail.com", "Senior", "Generator", "Frajdy", "asdf1243@#$GJH", Role.Simple, "");

            var stringContent = new StringContent(JsonConvert.SerializeObject(registerDto), Encoding.UTF8, "application/json");

            // Act
            HttpResponseMessage response = await httpClient.PostAsync("register", stringContent);

            // Assert
            await _webApplicationFactory.DoWithinScope<UserManager<User>>(
                async userManager =>
                {
                    User userResult = await userManager
                        .Users
                        .SingleOrDefaultAsync(x => x.Email == registerDto.email);

                    userResult.Should().NotBeNull();
                    userResult.Name.Should().Be(registerDto.name);
                    userResult.Surname.Should().Be(registerDto.surname);
                    userResult.UserName.Should().Be(registerDto.nickname);
                    userResult.NormalizedUserName.Should().Be(registerDto.nickname.ToUpper());
                    userResult.Email.Should().Be(registerDto.email);
                    userResult.NormalizedUserName.Should().Be(registerDto.nickname.ToUpper());
                    userManager.PasswordHasher.VerifyHashedPassword(userResult, userResult.PasswordHash, registerDto.password);

                    var roles = await userManager.GetRolesAsync(userResult);
                    roles.Should().Contain(Role.Simple);
                });

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [TestMethod]
        public async Task DeleteShouldRemoveFromDB()
        {
            // Arrange
            var httpClient = _webApplicationFactory.CreateClient();
            var registerDto = new RegisterDto("adres@adres.com", "Senior", "Generator", "Frajdy", "asdf1243@#$GJH", Role.Simple, "");
            var stringContent = new StringContent(JsonConvert.SerializeObject(registerDto), Encoding.UTF8, "application/json");
            await httpClient.PostAsync("register", stringContent);

            string userID = String.Empty;
            await _webApplicationFactory.DoWithinScope<UserManager<User>>(
                async userManager =>
                {
                    User user = await userManager.FindByEmailAsync(registerDto.email);
                    userID = user.Id;
                });

            // Act
            var path = QueryHelpers.AddQueryString("user", "userID", userID);
            HttpResponseMessage response = await httpClient.DeleteAsync(path);

            // Assert
            await _webApplicationFactory.DoWithinScope<UserManager<User>>(
                async userManager =>
                {
                    User user = await userManager.FindByEmailAsync(registerDto.email);
                    user.Should().BeNull();
                });

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [TestMethod]
        public async Task GetShouldReturnDataFromDB()
        {
            // Arrange
            var httpClient = _webApplicationFactory.CreateClient();
            var registerDto = new RegisterDto("adress@adress.com", "Seniorek", "Generatorik", "Frajdusi", "asdf1243@#$GJH", Role.Simple, "");
            var stringContent = new StringContent(JsonConvert.SerializeObject(registerDto), Encoding.UTF8, "application/json");
            await httpClient.PostAsync("register", stringContent);

            string userID = String.Empty;
            await _webApplicationFactory.DoWithinScope<UserManager<User>>(
                async userManager =>
                {
                    User user = await userManager.FindByEmailAsync(registerDto.email);
                    userID = user.Id;
                });

            // Act
            var path = QueryHelpers.AddQueryString("user", "userID", userID);
            HttpResponseMessage response = await httpClient.GetAsync(path);

            // Assert
            response.Content.Should().NotBeNull();
            string responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().NotBeNull().Should().NotBe(string.Empty);
            var userDTO = JsonConvert.DeserializeObject<UserDTO>(responseString);
            await _webApplicationFactory.DoWithinScope<UserManager<User>>(
                async userManager =>
                {
                    User userResult = await userManager.FindByEmailAsync(registerDto.email);

                    userResult.Name.Should().Be(userDTO.name);
                    userResult.Surname.Should().Be(userDTO.surname);
                    userResult.UserName.Should().Be(userDTO.nickname);
                    userResult.Email.Should().Be(userDTO.email);
                    userResult.AccountBalance.Should().Be(userDTO.accountBalance);
                    userResult.SubscriptionsCount.Should().Be(userDTO.subscriptionsCount);
                    userResult.Id.Should().Be(userDTO.id.ToString());

                    var roles = await userManager.GetRolesAsync(userResult);
                    roles.First().Should().Be(userDTO.userType);
                });

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [TestMethod]
        public async Task PutShouldEditDB()
        {
            // Arrange
            var httpClient = _webApplicationFactory.CreateClient();
            var registerDto = new RegisterDto("adreser@adresera.com", "Seniorita", "Generatorny", "Franki", "asdf1243@#$GJH", Role.Simple, "");
            var stringContent = new StringContent(JsonConvert.SerializeObject(registerDto), Encoding.UTF8, "application/json");
            await httpClient.PostAsync("register", stringContent);

            string userID = String.Empty;
            await _webApplicationFactory.DoWithinScope<UserManager<User>>(
                async userManager =>
                {
                    User user = await userManager.FindByEmailAsync(registerDto.email);
                    userID = user.Id;
                });

            UserDTO userDTO = new UserDTO(new Guid(userID), "koka@cola.com", "Robert", "Robert", "Kubica", 13, Role.Creator, "", 73);
            stringContent = new StringContent(JsonConvert.SerializeObject(userDTO), Encoding.UTF8, "application/json");

            // Act
            HttpResponseMessage response = await httpClient.PutAsync("/user", stringContent);

            // Assert
            await _webApplicationFactory.DoWithinScope<UserManager<User>>(
                async userManager =>
                {
                    User userResult = await userManager.FindByIdAsync(userID);

                    userResult.Name.Should().Be(userDTO.name);
                    userResult.Surname.Should().Be(userDTO.surname);
                    userResult.UserName.Should().Be(userDTO.nickname);
                    userResult.Email.Should().Be(userDTO.email);
                    userResult.AccountBalance.Should().Be(userDTO.accountBalance);
                    userResult.SubscriptionsCount.Should().Be(userDTO.subscriptionsCount);

                    var roles = await userManager.GetRolesAsync(userResult);
                    roles.Should().HaveCount(1);
                    roles.First().Should().Be(Role.Creator);
                });

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
