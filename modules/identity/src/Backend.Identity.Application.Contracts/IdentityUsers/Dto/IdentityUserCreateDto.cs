using System.ComponentModel.DataAnnotations;
using Backend.Identity.Identity;
using Volo.Abp.Auditing;
using Volo.Abp.Validation;

namespace Backend.Identity.IdentityUsers.Dto;

public class IdentityUserCreateDto : IdentityUserCreateOrUpdateDtoBase
{
    [DisableAuditing]
    [Required]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
    public string Password { get; set; }
}