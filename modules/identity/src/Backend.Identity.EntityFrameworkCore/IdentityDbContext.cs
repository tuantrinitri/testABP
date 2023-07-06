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

/// <summary>
/// Base class for the Entity Framework database context used for identity.
/// </summary>
[ConnectionStringName(IdentityDbProperties.ConnectionStringName)]
public class IdentityDbContext : AbpDbContext<IdentityDbContext>, IIdentityDbContext
{
    public DbSet<IdentityUser> Users { get; set; }

    public DbSet<IdentityRole> Roles { get; set; }

    public DbSet<IdentityClaimType> ClaimTypes { get; set; }

    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }

    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }

    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    
    public DbSet<Position> Positions { get; set; }
    
    public DbSet<Rank> Ranks { get; set; }

    public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureIdentity();
    }
}
