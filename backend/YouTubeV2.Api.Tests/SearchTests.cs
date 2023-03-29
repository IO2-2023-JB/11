using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YouTubeV2.Application.DTO;
using YouTubeV2.Application.Model;

namespace YouTubeV2.Api.Tests
{
    [TestClass]
    public class SearchTests
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
        public async Task SearchShouldReturnUsersFromDB()
        {
            // Arrange
            var httpClient = _webApplicationFactory.CreateClient();
            var registerDtos = new[]
            {
                new RegisterDto("mail@mail.com", "Alaa", "Alaa", "Tomasz", "asdf1243@#$GJH", Role.Creator, ""),
                new RegisterDto("list@list.com", "Alab", "Alab", "Tomek", "asdf1243@#$GJH", Role.Simple, ""),
                new RegisterDto("wiad@wiad.com", "Alac", "Alac", "Tomus", "asdf1243@#$GJH", Role.Creator, ""),
                new RegisterDto("polska@polska.com", "Maikołaj", "Maikołaj", "Tomaszewski", "asdf1243@#$GJH", Role.Creator, "")
            };

            foreach (var regosterDTO in registerDtos)
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(regosterDTO), Encoding.UTF8, "application/json");
                await httpClient.PostAsync("register", stringContent);
            }

            // Act
            var querys = new Dictionary<string, string?>()
            {
                { "query", "ala" },
                { "sortingCriterion", "Alphabetical" },
                { "sortingType", "Descending" }
            };
            var path = QueryHelpers.AddQueryString("search", querys);
            HttpResponseMessage response = await httpClient.GetAsync(path);

            // Assert 
            response.Content.Should().NotBeNull();
            string responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().NotBeNull().Should().NotBe(string.Empty);
            var searchDTO = JsonConvert.DeserializeObject<SearchResultsDTO>(responseString);
            var foundUsers = searchDTO.users.ToList();

            foundUsers.Should().HaveCount(2);
            foundUsers[0].nickname.Should().Be("Alac");
            foundUsers[1].nickname.Should().Be("Alaa");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

        }
    }

}
