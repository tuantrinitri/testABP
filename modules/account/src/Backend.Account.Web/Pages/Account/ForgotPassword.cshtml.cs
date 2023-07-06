using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Backend.Account.Dto;
using Backend.Identity.Identity;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Validation;

namespace Backend.Account.Web.Pages.Account;

public class ForgotPasswordModel : AccountPageModel
{
    [Required]
    [EmailAddress]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
    [BindProperty]
    public string Email { get; set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string ReturnUrl { get; set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string ReturnUrlHash { get; set; }

    public virtual Task<IActionResult> OnGetAsync()
    {
        return Task.FromResult<IActionResult>(Page());
    }

    // public virtual async Task<IActionResult> OnPostAsync()
    // {
    //     try
    //     {
    //         await AccountAppService.SendPasswordResetCodeAsync(
    //             new SendPasswordResetCodeDto
    //             {
    //                 Email = Email,
    //                 AppName = "MVC", //TODO: Const!
    //                     ReturnUrl = ReturnUrl,
    //                 ReturnUrlHash = ReturnUrlHash
    //             }
    //         );
    //     }
    //     catch (UserFriendlyException e)
    //     {
    //         Alerts.Danger(GetLocalizeExceptionMessage(e));
    //         return Page();
    //     }
    //
    //
    //     return RedirectToPage(
    //         "./PasswordResetLinkSent",
    //         new {
    //             returnUrl = ReturnUrl,
    //             returnUrlHash = ReturnUrlHash
    //         });
    // }
}
