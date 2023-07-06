using System.Security.Claims;
using System.Threading.Tasks;
using Backend.Identity.Identity;
using Backend.Identity.IdentityUsers.IdentitySecurityLogs;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;

namespace Backend.Account.Web.Pages.Account;

[ExposeServices(typeof(LogoutModel))]
public class IdentityServerSupportedLogoutModel : LogoutModel
{
    protected IIdentityServerInteractionService Interaction { get; }

    public IdentityServerSupportedLogoutModel(IIdentityServerInteractionService interaction)
    {
        Interaction = interaction;
    }

    public override async Task<IActionResult> OnGetAsync()
    {
        await SignInManager.SignOutAsync();

        var logoutId = Request.Query["logoutId"].ToString();

        if (!string.IsNullOrEmpty(logoutId))
        {
            var logoutContext = await Interaction.GetLogoutContextAsync(logoutId);

            await SaveSecurityLogAsync(logoutContext?.ClientId);

            await SignInManager.SignOutAsync();

            HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            var vm = new LoggedOutModel()
            {
                PostLogoutRedirectUri = logoutContext?.PostLogoutRedirectUri,
                ClientName = logoutContext?.ClientName,
                SignOutIframeUrl = logoutContext?.SignOutIFrameUrl
            };

            Logger.LogInformation($"Redirecting to LoggedOut Page...");

            return RedirectToPage("./LoggedOut", vm);
        }

        await SaveSecurityLogAsync();

        if (ReturnUrl != null)
        {
            return LocalRedirect(ReturnUrl);
        }

        Logger.LogInformation(
            $"IdentityServerSupportedLogoutModel couldn't find postLogoutUri... Redirecting to:/Account/Login..");
        return RedirectToPage("/Account/Login");
    }

    protected virtual async Task SaveSecurityLogAsync(string clientId = null)
    {
        if (CurrentUser.IsAuthenticated)
        {
            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
            {
                Identity = IdentitySecurityLogIdentityConsts.Identity,
                Action = IdentitySecurityLogActionConsts.Logout,
                ClientId = clientId
            });
        }
    }
}
