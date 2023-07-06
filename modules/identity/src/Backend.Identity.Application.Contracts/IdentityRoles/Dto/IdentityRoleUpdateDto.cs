using Volo.Abp.Domain.Entities;

namespace Backend.Identity.IdentityRoles.Dto;

public class IdentityRoleUpdateDto : IdentityRoleCreateOrUpdateDtoBase, IHasConcurrencyStamp
{
    public string ConcurrencyStamp { get; set; }
}