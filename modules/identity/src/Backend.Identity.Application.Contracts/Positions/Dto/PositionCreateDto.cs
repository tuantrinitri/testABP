using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;
using Volo.Abp.ObjectExtending;

namespace Backend.Identity.Positions.Dto;

public class PositionCreateDto
{
    // Mã chức danh
    [Required]
    public string Code { get; set; }
    
    // Tên chức danh
    [Required]
    public string Name { get; set; }
}