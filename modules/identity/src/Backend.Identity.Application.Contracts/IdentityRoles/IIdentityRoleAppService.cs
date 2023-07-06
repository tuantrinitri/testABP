using System;
using System.Threading.Tasks;
using Backend.Identity.IdentityRoles.Dto;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Backend.Identity.IdentityRoles;

public interface IIdentityRoleAppService: ICrudAppService<
    IdentityRoleDto,
    Guid,
    GetIdentityRolesInput,
    IdentityRoleCreateDto,
    IdentityRoleUpdateDto>
{
    Task<ListResultDto<IdentityRoleDto>> GetAllListAsync();
}