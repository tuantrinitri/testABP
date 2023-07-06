using System;
using System.Security.Claims;
using Backend.Identity.IdentityClaims;
using JetBrains.Annotations;

namespace Backend.Identity.IdentityUsers;

public class IdentityUserClaim: IdentityClaim
{
    /// <summary>
    /// Gets or sets the primary key of the user associated with this claim.
    /// </summary>
    public virtual Guid UserId { get; protected set; }

    protected IdentityUserClaim()
    {

    }

    protected internal IdentityUserClaim(Guid id, Guid userId, [NotNull] Claim claim, Guid? tenantId)
        : base(id, claim, tenantId)
    {
        UserId = userId;
    }

    public IdentityUserClaim(Guid id, Guid userId, [NotNull] string claimType, string claimValue, Guid? tenantId)
        : base(id, claimType, claimValue, tenantId)
    {
        UserId = userId;
    }
}