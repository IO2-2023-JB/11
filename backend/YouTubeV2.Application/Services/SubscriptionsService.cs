using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
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
        private readonly UserManager<User> _userManager;
        public SubscriptionsService(IBlobImageService blobImageService, YTContext context, UserManager<User> userManager)
        {
            _blobImageService = blobImageService;
            _context = context;
            _userManager = userManager;
        }


        public async Task<UserSubscriptionListDTO> GetSubscriptionsAsync(Guid Id, CancellationToken cancellationToken)
        {
            return new UserSubscriptionListDTO( await _context.Subscriptions.
                Where(s => s.SubscriberId == Id.ToString()).
                Select(s => new SubscriptionDTO(new Guid(s.SubscribeeId), _blobImageService.GetProfilePicture(s.Subscribee.Id), s.Subscribee.UserName)).
                ToListAsync(cancellationToken));
        }
        public async Task PostSubscriptionsAsync(Guid id, string token, CancellationToken cancellationToken)
        {
            Subscription subRequest = new Subscription();
            var subscribee = await _userManager.FindByIdAsync(id.ToString());
            if(subscribee == null)
            {
                throw new BadRequestException();
            }
            subRequest.SubscribeeId = subscribee.Id;
            subRequest.Subscribee = subscribee;

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwt = handler.ReadJwtToken(token);

            string userId = jwt.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            subRequest.SubscriberId = subscribee.Id;//???
            subRequest.Subscriber = subscribee;//???

            if(_context.Subscriptions.
                Where(s => s.SubscriberId == subRequest.SubscriberId && s.SubscribeeId == subRequest.SubscribeeId).
                ToArray().Length != 0)
            {
                throw new BadRequestException();
            }
            await _context.Subscriptions.AddAsync(subRequest, cancellationToken);
            
            _context.SaveChanges();
        }

        public async Task DeleteSubscriptionsAsync(Guid id, string token, CancellationToken cancellationToken)
        {
            var subs = await _context.Subscriptions.Where(s => s.SubscribeeId == id.ToString()).ToArrayAsync(cancellationToken);
            if(subs.Length == 0)
            {
                throw new BadRequestException();
            }

            _context.Subscriptions.Remove(subs[0]);
            _context.SaveChanges();
        }

    }
}