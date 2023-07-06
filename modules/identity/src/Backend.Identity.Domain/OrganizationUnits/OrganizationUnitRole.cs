using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Backend.Identity.OrganizationUnits;

/// <summary>
/// Thể hiện tư cách thành viên của Role đối với OU.
/// </summary>
public class OrganizationUnitRole : CreationAuditedEntity, IMultiTenant
{
    /// <summary>
    /// TenantId.
    /// </summary>
    public virtual Guid? TenantId { get; protected set; }

    /// <summary>
    /// Id của Role.
    /// </summary>
    public virtual Guid RoleId { get; protected set; }

    /// <summary>
    /// Id của <see cref="OrganizationUnit"/>.
    /// </summary>
    public virtual Guid OrganizationUnitId { get; protected set; }

    /// <summary>
    /// Khởi tạo một instance mới của class <see cref="OrganizationUnitRole"/>.
    /// </summary>
    protected OrganizationUnitRole()
    {

    }

    /// <summary>
    /// Khởi tạo một instance mới của class <see cref="OrganizationUnitRole"/>.
    /// </summary>
    /// <param name="tenantId">TenantId</param>
    /// <param name="roleId">Id của Role.</param>
    /// <param name="organizationUnitId">Id của <see cref="OrganizationUnit"/>.</param>
    public OrganizationUnitRole(Guid roleId, Guid organizationUnitId, Guid? tenantId = null)
    {
        RoleId = roleId;
        OrganizationUnitId = organizationUnitId;
        TenantId = tenantId;
    }

    public override object[] GetKeys()
    {
        return new object[] { OrganizationUnitId, RoleId };
    }
}