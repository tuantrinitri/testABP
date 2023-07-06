using System;
using Backend.Identity.Identity;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Validation;

namespace Backend.Identity.IdentityUsers.Dto;

public class ProfileDto : EntityDto<Guid>
{
    /// <summary>
    /// Id người dùng.
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Giới tính.
    /// </summary>
    public Gender Gender { get; set; }
    
    /// <summary>
    /// Mã quân nhân
    /// </summary>
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxUserNameLength))]
    public string SoldierCode { get; set; }
    
    /// <summary>
    /// Ngày sinh (lưu kiểu unix timestamp).
    /// </summary>
    public long DateOfBirth { get; set; }
    
    /// <summary>
    /// Ngày nhập ngũ (lưu kiểu unix timestamp).
    /// </summary>
    public long StartDateMilitary { get; set; }
    
    /// <summary>
    /// Ngày xuất ngũ (lưu kiểu unix timestamp).
    /// </summary>
    public long EndDateMilitary { get; set; }
}