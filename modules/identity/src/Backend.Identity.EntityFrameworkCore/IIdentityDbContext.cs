using Backend.Identity.IdentityClaims;
using Backend.Identity.IdentityRoles;
using Backend.Identity.IdentityUsers;
using Backend.Identity.IdentityUsers.IdentityLinkUsers;
using Backend.Identity.IdentityUsers.IdentitySecurityLogs;
using Backend.Identity.IdentityUsers.IdentityUserDelegations;
using Backend.Identity.OrganizationUnits;
using Backend.Identity.Positions;
using Backend.Identity.Ranks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Backend.Identity.EntityFrameworkCore;

[ConnectionStringName(IdentityDbProperties.ConnectionStringName)]
public interface IIdentityDbContext : IEfCoreDbContext
{
    DbSet<IdentityUser> Users { get; }

    DbSet<IdentityRole> Roles { get; }

    DbSet<IdentityClaimType> ClaimTypes { get; }

    DbSet<OrganizationUnit> OrganizationUnits { get; }

    DbSet<IdentitySecurityLog> SecurityLogs { get; }

    DbSet<IdentityLinkUser> LinkUsers { get; }
    
    DbSet<Position> Positions { get; }
    
    public DbSet<Rank> Ranks { get; }

    //DbSet<IdentityUserDelegation> UserDelegations { get; }
}
