using System.ComponentModel.DataAnnotations;
using Backend.Identity.Identity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;

namespace Backend.Identity.IdentityRoles.Dto;

public class IdentityRoleCreateOrUpdateDtoBase : ExtensibleObject
{
    [Required]
    [DynamicStringLength(typeof(IdentityRoleConsts), nameof(IdentityRoleConsts.MaxNameLength))]
    [Display(Name = "RoleName")]
    public string Name { get; set; }

    public bool IsDefault { get; set; }

    public bool IsPublic { get; set; }

    protected IdentityRoleCreateOrUpdateDtoBase() : base(false)
    {

    }
}