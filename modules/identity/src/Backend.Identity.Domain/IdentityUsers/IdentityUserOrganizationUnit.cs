using System;
using Backend.Identity.OrganizationUnits;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Backend.Identity.IdentityUsers;

/// <summary>
/// Thể hiện tư cách thành viên của User đối với OU.
/// </summary>
public class IdentityUserOrganizationUnit : CreationAuditedEntity, IMultiTenant
{
    /// <summary>
    /// TenantId của thực thể này.
    /// </summary>
    public virtual Guid? TenantId { get; protected set; }

    /// <summary>
    /// Id của User.
    /// </summary>
    public virtual Guid UserId { get; protected set; }

    /// <summary>
    /// Id của liên quan <see cref="OrganizationUnit"/>.
    /// </summary>
    public virtual Guid OrganizationUnitId { get; protected set; }

    protected IdentityUserOrganizationUnit()
    {

    }

    public IdentityUserOrganizationUnit(Guid userId, Guid organizationUnitId, Guid? tenantId = null)
    {
        UserId = userId;
        OrganizationUnitId = organizationUnitId;
        TenantId = tenantId;
    }

    public override object[] GetKeys()
    {
        return new object[] { UserId, OrganizationUnitId };
    }
}