﻿using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;
using Volo.Abp.ObjectExtending;

namespace Backend.Identity.Ranks.Dto;

public class RankUpdateDto: IHasConcurrencyStamp
{
    // Mã chức danh
    [Required]
    public string Code { get; set; }
    
    // Tên chức danh
    [Required]
    public string Name { get; set; }
    
    public string ConcurrencyStamp { get; set; }
}