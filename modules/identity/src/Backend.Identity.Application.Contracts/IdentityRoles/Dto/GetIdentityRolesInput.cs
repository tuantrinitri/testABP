using Volo.Abp.Application.Dtos;

namespace Backend.Identity.IdentityRoles.Dto;

public class GetIdentityRolesInput : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}