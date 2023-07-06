using System.Threading.Tasks;
using Backend.Account.Dto;
using Backend.Identity.IdentityUsers.Dto;
using Volo.Abp.Application.Services;

namespace Backend.Account.Interfaces;

public interface IAccountAppService : IApplicationService
{
    Task<IdentityUserDto> RegisterAsync(RegisterDto input);

    //Task SendPasswordResetCodeAsync(SendPasswordResetCodeDto input);

    Task<bool> VerifyPasswordResetTokenAsync(VerifyPasswordResetTokenInput input);

    Task ResetPasswordAsync(ResetPasswordDto input);
}
