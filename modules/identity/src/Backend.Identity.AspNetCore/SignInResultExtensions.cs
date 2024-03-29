﻿using Backend.Identity.Identity;
using Microsoft.AspNetCore.Identity;

public static class SignInResultExtensions
{
    public static string ToIdentitySecurityLogAction(this SignInResult result)
    {
        if (result.Succeeded)
        {
            return IdentitySecurityLogActionConsts.LoginSucceeded;
        }

        if (result.IsLockedOut)
        {
            return IdentitySecurityLogActionConsts.LoginLockedout;
        }

        if (result.RequiresTwoFactor)
        {
            return IdentitySecurityLogActionConsts.LoginRequiresTwoFactor;
        }

        if (result.IsNotAllowed)
        {
            return IdentitySecurityLogActionConsts.LoginNotAllowed;
        }

        if (!result.Succeeded)
        {
            return IdentitySecurityLogActionConsts.LoginFailed;
        }

        return IdentitySecurityLogActionConsts.LoginFailed;
    }
}