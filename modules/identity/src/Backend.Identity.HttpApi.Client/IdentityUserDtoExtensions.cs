﻿using Backend.Identity.IdentityUsers.Dto;
using Volo.Abp.Users;

namespace Backend.Identity.HttpApi.Client;

public static class IdentityUserDtoExtensions
{
    public static IUserData ToUserInfo(this IdentityUserDto user)
    {
        return new UserData(
            user.Id,
            user.UserName,
            user.Email,
            user.Name,
            user.Surname,
            user.EmailConfirmed,
            user.PhoneNumber,
            user.PhoneNumberConfirmed,
            user.TenantId
        );
    }
}
