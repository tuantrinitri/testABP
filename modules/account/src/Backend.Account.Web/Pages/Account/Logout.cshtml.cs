using System.Threading.Tasks;
using Backend.Account.Settings;
using Backend.Identity.Identity;
using Backend.Identity.IdentityUsers.IdentitySecurityLogs;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Settings;

namespace Backend.Account.Web.Pages.Account;

public class LogoutModel : AccountPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string ReturnUrl { get; set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string ReturnUrlHash { get; set; }

    public virtual async Task<IActionResult> OnGetAsync()
    {
        await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
        {
            Identity = IdentitySecurityLogIdentityConsts.Identity,
            Action = IdentitySecurityLogActionConsts.Logout
        });

        await SignInManager.SignOutAsync();
        if (ReturnUrl != null)
        {
            return RedirectSafely(ReturnUrl, ReturnUrlHash);
        }

        if (await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin))
        {
            return RedirectToPage("/Account/Login");
        }

        return RedirectToPage("/");
    }

    public virtual Task<IActionResult> OnPostAsync()
    {
        return Task.FromResult<IActionResult>(Page());
    }
}
