using Backend.Identity.Identity;
using JetBrains.Annotations;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Validation;

namespace Backend.Identity.IdentityUsers.Dto;

public class IdentityUserUpdateDto : IdentityUserCreateOrUpdateDtoBase, IHasConcurrencyStamp
{
    [DisableAuditing]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
    public string Password { get; set; }
    
    [CanBeNull] 
    public ProfileDto Profile { get; set; }
    
    public string ConcurrencyStamp { get; set; }
}