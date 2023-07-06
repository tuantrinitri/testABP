using System;
using Volo.Abp.Auditing;
using Volo.Abp.MultiTenancy;

namespace Backend.Identity.Etos;

[Serializable]
public class OrganizationUnitEto: IMultiTenant, IHasEntityVersion
{
    public Guid Id { get; set; }

    public Guid? TenantId { get; set; }

    public string Code { get; set; }

    public string DisplayName { get; set; }

    public int EntityVersion { get; set; }
}