using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.ObjectExtending;

namespace Backend.Identity.Positions.Dto;

public class PositionUpdateDto: IHasConcurrencyStamp
{
    // Mã chức danh
    [Required]
    public string Code { get; set; }
    
    // Tên chức danh
    [Required]
    public string Name { get; set; }
    
    public string ConcurrencyStamp { get; set; }
}