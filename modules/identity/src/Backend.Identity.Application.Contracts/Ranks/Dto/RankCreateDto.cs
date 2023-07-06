using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;

namespace Backend.Identity.Ranks.Dto;

public class RankCreateDto
{
    // Mã chức danh
    [Required]
    public string Code { get; set; }
    
    // Tên chức danh
    [Required]
    public string Name { get; set; }
}