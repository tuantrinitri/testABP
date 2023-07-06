using System.ComponentModel.DataAnnotations;

namespace Backend.Identity.IdentityUsers.Dto;

public class IdentityUserUpdateRolesDto
{
    [Required]
    public string[] RoleNames { get; set; }
}