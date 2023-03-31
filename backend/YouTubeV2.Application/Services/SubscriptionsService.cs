using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using YouTubeV2.Application.DTO;
using YouTubeV2.Application.Exceptions;
using YouTubeV2.Application.Model;
using YouTubeV2.Application.Services.AzureServices.BlobServices;
using YouTubeV2.Application.Validator;

namespace YouTubeV2.Application.Services
{
    public class SubscriptionsService
    {
        public SubscriptionsService()
        {

        }

        public async Task GetSubscriptionsAsync(CancellationToken cancellationToken)
        {
            
        }
    }
}