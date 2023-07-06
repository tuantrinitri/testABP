using System.Threading.Tasks;
using Backend.Account.Dto;
using Volo.Abp.Application.Services;

namespace Backend.Account.Interfaces;

public interface IProfileAppService : IApplicationService
{
    Task<ProfileDto> GetAsync();

    Task<ProfileDto> UpdateAsync(UpdateProfileDto input);

    Task ChangePasswordAsync(ChangePasswordInput input);
}
