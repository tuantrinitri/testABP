using System.Threading.Tasks;
using Backend.Account.Dto;
using Backend.Account.Interfaces;
using Backend.Account.Localization;
using Backend.Account.Settings;
using Backend.Identity.Identity;
using Backend.Identity.IdentityRoles;
using Backend.Identity.IdentityUsers;
using Backend.Identity.IdentityUsers.Dto;
using Backend.Identity.IdentityUsers.IdentitySecurityLogs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Settings;

namespace Backend.Account;

public class AccountAppService : ApplicationService, IAccountAppService
{
    protected IIdentityRoleRepository RoleRepository { get; }
    protected IdentityUserManager UserManager { get; }
    protected IdentitySecurityLogManager IdentitySecurityLogManager { get; }
    protected IOptions<IdentityOptions> IdentityOptions { get; }

    public AccountAppService(
        IdentityUserManager userManager,
        IIdentityRoleRepository roleRepository,
        IdentitySecurityLogManager identitySecurityLogManager,
        IOptions<IdentityOptions> identityOptions)
    {
        RoleRepository = roleRepository;
        IdentitySecurityLogManager = identitySecurityLogManager;
        UserManager = userManager;
        IdentityOptions = identityOptions;

        LocalizationResource = typeof(AccountResource);
    }

    public virtual async Task<IdentityUserDto> RegisterAsync(RegisterDto input)
    {
        await CheckSelfRegistrationAsync();

        await IdentityOptions.SetAsync();

        var user = new IdentityUser(GuidGenerator.Create(), input.UserName, input.EmailAddress, CurrentTenant.Id);

        input.MapExtraPropertiesTo(user);

        (await UserManager.CreateAsync(user, input.Password)).CheckErrors();

        await UserManager.SetEmailAsync(user, input.EmailAddress);
        await UserManager.AddDefaultRolesAsync(user);

        return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
    }

    public virtual async Task<bool> VerifyPasswordResetTokenAsync(VerifyPasswordResetTokenInput input)
    {
        var user = await UserManager.GetByIdAsync(input.UserId);
        return await UserManager.VerifyUserTokenAsync(
            user,
            UserManager.Options.Tokens.PasswordResetTokenProvider,
            UserManager<IdentityUser>.ResetPasswordTokenPurpose,
            input.ResetToken);
    }

    public virtual async Task ResetPasswordAsync(ResetPasswordDto input)
    {
        await IdentityOptions.SetAsync();

        var user = await UserManager.GetByIdAsync(input.UserId);
        (await UserManager.ResetPasswordAsync(user, input.ResetToken, input.Password)).CheckErrors();

        await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
        {
            Identity = IdentitySecurityLogIdentityConsts.Identity,
            Action = IdentitySecurityLogActionConsts.ChangePassword
        });
    }

    protected virtual async Task<IdentityUser> GetUserByEmailAsync(string email)
    {
        var user = await UserManager.FindByEmailAsync(email);
        if (user == null)
        {
            throw new UserFriendlyException(L["Volo.Account:InvalidEmailAddress", email]);
        }

        return user;
    }

    protected virtual async Task CheckSelfRegistrationAsync()
    {
        if (!await SettingProvider.IsTrueAsync(AccountSettingNames.IsSelfRegistrationEnabled))
        {
            throw new UserFriendlyException(L["SelfRegistrationDisabledMessage"]);
        }
    }
}
