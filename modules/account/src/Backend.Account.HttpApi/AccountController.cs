using System.Threading.Tasks;
using Backend.Account.Dto;
using Backend.Account.Interfaces;
using Backend.Identity.IdentityUsers.Dto;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace Backend.Account;

[RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
[Area(AccountRemoteServiceConsts.ModuleName)]
[Route("api/account")]
public class AccountController : AbpControllerBase, IAccountAppService
{
    protected IAccountAppService AccountAppService { get; }

    public AccountController(IAccountAppService accountAppService)
    {
        AccountAppService = accountAppService;
    }

    [HttpPost]
    [Route("register")]
    public virtual Task<IdentityUserDto> RegisterAsync(RegisterDto input)
    {
        return AccountAppService.RegisterAsync(input);
    }

    [HttpPost]
    [Route("verify-password-reset-token")]
    public Task<bool> VerifyPasswordResetTokenAsync(VerifyPasswordResetTokenInput input)
    {
        return AccountAppService.VerifyPasswordResetTokenAsync(input);
    }

    [HttpPost]
    [Route("reset-password")]
    public virtual Task ResetPasswordAsync(ResetPasswordDto input)
    {
        return AccountAppService.ResetPasswordAsync(input);
    }
}
