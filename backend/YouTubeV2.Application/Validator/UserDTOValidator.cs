﻿using FluentValidation;
using Microsoft.AspNetCore.Identity;
using YouTubeV2.Application.DTO;
using YouTubeV2.Application.Model;
using YouTubeV2.Application.Constants;
using System;

namespace YouTubeV2.Application.Validator
{
    public class UserDTOValidator : AbstractValidator<UserDTO>
    {
        public UserDTOValidator(UserManager<User> userManager)
        {
            RuleFor(x => x.name).NotNull().Length(1, UserConstants.MaxUserNameLength);
            RuleFor(x => x.surname).NotNull().Length(1, UserConstants.MaxUserSurnameLength);
            RuleFor(x => x.userType).Must(userType => userType.Equals(Role.Simple, StringComparison.InvariantCultureIgnoreCase)
                || userType.Equals(Role.Creator, StringComparison.InvariantCultureIgnoreCase));

            RuleFor(x => x.nickname)
                .NotNull()
                .Length(1, UserConstants.MaxUserNicknameLength);

            RuleFor(x => x.email)
                .NotNull()
                .Length(1, UserConstants.MaxUserEmailLength)
                .EmailAddress();

            RuleFor(x => x.subscriptionsCount).NotNull()
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.accountBalance).NotNull();
        }
    }
}
