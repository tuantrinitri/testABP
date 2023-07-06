using AutoMapper;
using Backend.Identity.Etos;
using Backend.Identity.IdentityClaims;
using Backend.Identity.IdentityRoles;
using Backend.Identity.IdentityUsers;
using Backend.Identity.OrganizationUnits;
using Volo.Abp.Users;

namespace Backend.Identity;

public class IdentityDomainMappingProfile: Profile
{
    public IdentityDomainMappingProfile()
    {
        CreateMap<IdentityUser, UserEto>();
        CreateMap<IdentityClaimType, IdentityClaimTypeEto>();
        CreateMap<IdentityRole, IdentityRoleEto>();
        CreateMap<OrganizationUnit, OrganizationUnitEto>();
    }
}