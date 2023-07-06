using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Backend.Identity.Positions.Dto;

/// <summary>
/// Dto Chức danh
/// </summary>
public class PositionDto: FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    // Mã chức danh
    [Required]
    public string Code { get; set; }
    
    // Tên chức danh
    [Required]
    public string Name { get; set; }
    
    public string ConcurrencyStamp { get; set; }
}