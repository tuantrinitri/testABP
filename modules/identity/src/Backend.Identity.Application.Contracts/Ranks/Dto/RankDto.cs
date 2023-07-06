using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Backend.Identity.Ranks.Dto;

/// <summary>
/// Dto Cấp bậc
/// </summary>
public class RankDto: FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    // Mã cấp bậc
    [Required]
    public string Code { get; set; }
    
    // Tên cấp bậc
    [Required]
    public string Name { get; set; }
    
    public string ConcurrencyStamp { get; set; }
}