using System.Threading.Tasks;
using Backend.Identity;
using Backend.Identity.IdentityUsers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.Settings;
using IdentityUser = Backend.Identity.IdentityUsers.IdentityUser;

public class AbpSignInManager : SignInManager<IdentityUser>
{
    protected AbpIdentityOptions AbpOptions { get; }

    protected ISettingProvider SettingProvider { get; }

    private readonly IdentityUserManager _identityUserManager;

    public AbpSignInManager(
        IdentityUserManager userManager,
        IHttpContextAccessor contextAccessor,
        IUserClaimsPrincipalFactory<IdentityUser> claimsFactory,
        IOptions<IdentityOptions> optionsAccessor,
        ILogger<SignInManager<IdentityUser>> logger,
        IAuthenticationSchemeProvider schemes,
        IUserConfirmation<IdentityUser> confirmation,
        IOptions<AbpIdentityOptions> options,
        ISettingProvider settingProvider) : base(
        userManager,
        contextAccessor,
        claimsFactory,
        optionsAccessor,
        logger,
        schemes,
        confirmation)
    {
        SettingProvider = settingProvider;
        AbpOptions = options.Value;
        _identityUserManager = userManager;
    }

    public override async Task<SignInResult> PasswordSignInAsync(
        string userName,
        string password,
        bool isPersistent,
        bool lockoutOnFailure)
    {
        foreach (var externalLoginProviderInfo in AbpOptions.ExternalLoginProviders.Values)
        {
            var externalLoginProvider = (IExternalLoginProvider)Context.RequestServices
                .GetRequiredService(externalLoginProviderInfo.Type);

            if (await externalLoginProvider.TryAuthenticateAsync(userName, password))
            {
                var user = await UserManager.FindByNameAsync(userName);

                return await SignInOrTwoFactorAsync(user, isPersistent);
            }
        }

        return await base.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);
    }

    protected override async Task<SignInResult> PreSignInCheck(IdentityUser user)
    {
        if (!user.IsActive)
        {
            Logger.LogWarning($"The user is not active therefore cannot login! (username: \"{user.UserName}\", id:\"{user.Id}\")");
            return SignInResult.NotAllowed;
        }

        if (user.ShouldChangePasswordOnNextLogin)
        {
            Logger.LogWarning($"The user should change password! (username: \"{user.UserName}\", id:\"{user.Id}\")");
            return SignInResult.NotAllowed;
        }

        if (await _identityUserManager.ShouldPeriodicallyChangePasswordAsync(user))
        {
            return SignInResult.NotAllowed;
        }

        return await base.PreSignInCheck(user);
    }
}