using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly IBlobImageService _blobImageService;
        private readonly YTContext _context;
        public SubscriptionsService(IBlobImageService blobImageService, YTContext context)
        {
            _blobImageService = blobImageService;
            _context = context;
        }

        public async Task<IReadOnlyList<SubscriptionDTO>> GetSubscriptionsAsync(string Id)
        {
            return await _context.Subscriptions.
                Where(s => s.SubscriberId == Id).
                Select(s => GetDTOForSubscription(s)).
                ToListAsync();
        }

        public SubscriptionDTO GetDTOForSubscription(Subscription subscription)
        {
            var imageUri = _blobImageService.GetProfilePicture(subscription.Subscribee.Id);
            return new SubscriptionDTO(subscription.SubscribeeId, imageUri.ToString(), subscription.Subscribee.UserName);
        }
    }
}