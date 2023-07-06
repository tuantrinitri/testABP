using System;
using Backend.Identity.Identity;
using Volo.Abp.Domain.Entities.Auditing;

namespace Backend.Identity.IdentityUsers;

/// <summary>
/// Profile của người dùng.
/// </summary>
public class UserProfile : FullAuditedEntity<Guid>
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
    
    /// <summary>
    /// Ảnh đại diện.
    /// </summary>
    public string Avatar { get; set; }

    public virtual IdentityUser User { get; set; }
}