using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace Backend.Identity.Positions;

/// <summary>
/// Entity Chức danh
/// </summary>
public class Position: FullAuditedEntity<Guid>, IHasConcurrencyStamp
{
    /// <summary>
    /// Mã chức danh
    /// </summary>
    [StringLength(255)]
    [Description("Mã chức danh")]
    public string Code { get; set; }
    
    /// <summary>
    /// Tên chức danh
    /// </summary>
    [StringLength(255)]
    [Description("Tên chức danh")]
    public string Name { get; set; }
    
    public string ConcurrencyStamp { get; set; }
}