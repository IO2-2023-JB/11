using Microsoft.AspNetCore.Identity;
using YouTubeV2.Application.Exceptions;
using YouTubeV2.Application.Model;

namespace YouTubeV2.Application.Services
{
    public class DonationService : IDonationService
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        
        public DonationService(UserManager<User> userManager, IUserService userService) 
        {
            _userManager = userManager;
            _userService = userService;
        }

        public async Task SendDonationAsync(string senderId, string recipientId, decimal ammount)
        {
            if (senderId == recipientId)
                throw new BadRequestException("You cannot send money to yourself");

            if (ammount <= 0)
                throw new BadRequestException("Ammount has to be positive");

            User? recipient = await _userService.GetByIdAsync(recipientId)
                ?? throw new NotFoundException("Recipient does not exist");

            User? sender = await _userService.GetByIdAsync(senderId)
                ?? throw new BadRequestException();

            if (!await _userManager.IsInRoleAsync(recipient, Role.Creator))
                throw new BadRequestException("You can only support Creators");
            
            recipient.AccountBalance += ammount;

            await _userManager.UpdateAsync(sender);
            await _userManager.UpdateAsync(recipient);
        }

        public async Task WithdrawMoneyAsync(string withdrawerId, decimal ammount)
        {
            User? user = await _userManager.FindByIdAsync(withdrawerId) 
                ?? throw new BadRequestException();

            if (!await _userManager.IsInRoleAsync(user, Role.Creator))
                throw new ForbiddenException("Only Creator can withdraw");
            
            if (ammount <= 0)
                throw new BadRequestException("Ammount has to be positive");

            User? withdrawer = await _userService.GetByIdAsync(withdrawerId)
                ?? throw new BadRequestException();

            if (withdrawer.AccountBalance < ammount)
                throw new BadRequestException("Not enough ballance");

            withdrawer.AccountBalance -= ammount;
            
            await _userManager.UpdateAsync(withdrawer);
        }
    }
}
