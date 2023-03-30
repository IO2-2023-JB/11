using FluentValidation;
using Microsoft.AspNetCore.Identity;
using YouTubeV2.Application.DTO;
using YouTubeV2.Application.Model;
using YouTubeV2.Application.Constants;

namespace YouTubeV2.Application.Validator
{
    public class UserIDValidator : AbstractValidator<string>
    {
        public UserIDValidator(UserManager<User> userManager)
        {
            RuleFor(id => id).NotNull().NotEmpty()
                .MustAsync(async (id, cancellationToken) => await userManager.FindByIdAsync(id) != null)
                .WithMessage("User with provided ID does not exist");
        }
    }
}