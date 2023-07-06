using Volo.Abp.Application.Dtos;

namespace Backend.Identity.IdentityUsers.Dto;

public class GetIdentityUsersInput : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}