using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace Backend.Identity.Ranks;

/// <summary>
/// Entity Cấp bậc
/// </summary>
public class Rank: FullAuditedEntity<Guid>, IHasConcurrencyStamp
{
    /// <summary>
    /// Mã cấp bậc
    /// </summary>
    [StringLength(255)]
    [Description("Mã Cấp bậc")]
    public string Code { get; set; }
    
    /// <summary>
    /// Tên cấp bậc
    /// </summary>
    [StringLength(255)]
    [Description("Tên Cấp bậc")]
    public string Name { get; set; }
    
    public string ConcurrencyStamp { get; set; }
}