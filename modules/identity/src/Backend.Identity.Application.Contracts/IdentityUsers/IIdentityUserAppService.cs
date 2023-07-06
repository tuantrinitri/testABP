using System;
using System.Threading.Tasks;
using Backend.Identity.IdentityRoles.Dto;
using Backend.Identity.IdentityUsers.Dto;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Backend.Identity.IdentityUsers;

public interface IIdentityUserAppService
    : ICrudAppService<
        IdentityUserDto,
        Guid,
        GetIdentityUsersInput,
        IdentityUserCreateDto,
        IdentityUserUpdateDto>
{
    Task<ListResultDto<IdentityRoleDto>> GetRolesAsync(Guid id);

    Task<ListResultDto<IdentityRoleDto>> GetAssignableRolesAsync();

    Task UpdateRolesAsync(Guid id, IdentityUserUpdateRolesDto input);
    
    Task<IdentityUserDto> FindByUsernameAsync(string userName);

    Task<IdentityUserDto> FindByEmailAsync(string email);
}