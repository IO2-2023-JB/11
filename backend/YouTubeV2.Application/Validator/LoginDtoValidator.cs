﻿using FluentValidation;
using Microsoft.AspNetCore.Identity;
using YouTubeV2.Application.Constants;
using YouTubeV2.Application.DTO;
using YouTubeV2.Application.Model;

namespace YouTubeV2.Application.Validator
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.email)
               .NotNull()
               .Length(1, UserConstants.MaxUserEmailLength)
               .EmailAddress()
               .MustAsync(async (email, cancellationToken) => await userManager.FindByEmailAsync(email) == null)
               .WithMessage(x => $"User with email {x.email} already exists");

            RuleFor(x => x.password)
                .NotNull()
                .MinimumLength(UserConstants.MinUserPasswordLength).WithMessage($"Your password length must be at least {UserConstants.MinUserPasswordLength}.")
                .MaximumLength(UserConstants.MaxUserPasswordLength).WithMessage($"Your password length must not exceed {UserConstants.MaxUserPasswordLength}.")
                .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
                .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
                .Matches(@"[!@#$%^&*(),.<>?/]+").WithMessage("Your password must contain at least one special character.");
        }
    }
}