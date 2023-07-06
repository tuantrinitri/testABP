using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Backend.Identity.Identity;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Backend.Identity.OrganizationUnits;

/// <summary>
/// Đại diện cho một đơn vị tổ chức (OU).
/// </summary>
public class OrganizationUnit : FullAuditedAggregateRoot<Guid>, IMultiTenant, IHasEntityVersion
{
    public virtual Guid? TenantId { get; protected set; }

    /// <summary>
    /// Parent <see cref="OrganizationUnit"/> Id.
    /// Null, nếu OU này là root.
    /// </summary>
    public virtual Guid? ParentId { get; internal set; }

    /// <summary>
    /// Code phân cấp của đơn vị tổ chức này.
    /// Ví dụ: "00001.00042.00005".
    /// Đây là code duy nhất cho một OrganizationUnit.
    /// Có thể thay đổi nếu hệ thống phân cấp OU bị thay đổi.
    /// </summary>
    public virtual string Code { get; set; }

    /// <summary>
    /// Tên hiển thị của Đơn vị tổ chức này.
    /// </summary>
    public virtual string DisplayName { get; set; }
    
    /// <summary>
    /// Mã của Đơn vị tổ chức này.
    /// </summary>
    public virtual string CodeName { get; set; }

    /// <summary>
    /// Giá trị phiên bản được tăng lên bất cứ khi nào thực thể được thay đổi.
    /// </summary>
    public virtual int EntityVersion { get; set; }

    /// <summary>
    /// Danh sách Role của OU này.
    /// </summary>
    public virtual ICollection<OrganizationUnitRole> Roles { get; protected set; }

    /// <summary>
    /// Khởi tạo một instance mới của lớp <see cref="OrganizationUnit"/>.
    /// </summary>
    public OrganizationUnit()
    {
    }

    /// <summary>
    /// Khởi tạo một instance mới của lớp <see cref="OrganizationUnit"/>.
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="displayName">Display name.</param>
    /// <param name="codeName">Mã đơn vị</param>
    /// <param name="parentId">Id của parent hoặc null nếu OU là root.</param>
    /// <param name="tenantId">Id của đối tượng thuê hoặc null đối với host.</param>
    public OrganizationUnit(Guid id, string displayName, [CanBeNull] string codeName, Guid? parentId = null, Guid? tenantId = null)
        : base(id)
    {
        TenantId = tenantId;
        DisplayName = displayName;
        CodeName = codeName;
        ParentId = parentId;
        Roles = new Collection<OrganizationUnitRole>();
    }

    /// <summary>
    /// Tạo code cho các số đã cho.
    /// Ví dụ: nếu số là 4,2 thì trả về "00004.00002";
    /// </summary>
    /// <param name="numbers">Mảng cấc số</param>
    public static string CreateCode(params int[] numbers)
    {
        return numbers.IsNullOrEmpty()
            ? null
            : numbers.Select(number => number.ToString(new string('0', OrganizationUnitConsts.CodeUnitLength)))
                .JoinAsString(".");
    }

    /// <summary>
    /// Nối Child code vào Parent code.
    /// Ví dụ: nếu parentCode = "00001", childCode = "00042" thì trả về "00001.00042".
    /// </summary>
    /// <param name="parentCode">Parent code. Có thể rỗng hoặc trống nếu parent là root.</param>
    /// <param name="childCode">c.</param>
    public static string AppendCode(string parentCode, string childCode)
    {
        if (childCode.IsNullOrEmpty())
        {
            throw new ArgumentNullException(nameof(childCode), "childCode can not be null or empty.");
        }

        if (parentCode.IsNullOrEmpty())
        {
            return childCode;
        }

        return parentCode + "." + childCode;
    }

    /// <summary>
    /// Nhận code tương đối với parent.
    /// Ví dụ: nếu code = "00019.00055.00001" và parentCode = "00019" thì trả về "00055.00001".
    /// </summary>
    /// <param name="code">Code.</param>
    /// <param name="parentCode">Parent code.</param>
    public static string GetRelativeCode(string code, string parentCode)
    {
        if (code.IsNullOrEmpty())
        {
            throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
        }

        if (parentCode.IsNullOrEmpty())
        {
            return code;
        }

        return code.Length == parentCode.Length ? null : code[(parentCode.Length + 1)..];
    }

    /// <summary>
    /// Tính code tiếp theo cho code đã cho.
    /// Ví dụ: nếu code = "00019.00055.00001" trả về "00019.00055.00002".
    /// </summary>
    /// <param name="code">Code.</param>
    public static string CalculateNextCode(string code)
    {
        if (code.IsNullOrEmpty())
        {
            throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
        }

        var parentCode = GetParentCode(code);
        var lastUnitCode = GetLastUnitCode(code);

        return AppendCode(parentCode, CreateCode(Convert.ToInt32(lastUnitCode) + 1));
    }

    /// <summary>
    /// Lấy code của unit cuối cùng.
    /// Ví dụ: nếu code = "00019.00055.00001" trả về "00001".
    /// </summary>
    /// <param name="code">Code.</param>
    public static string GetLastUnitCode(string code)
    {
        if (code.IsNullOrEmpty())
        {
            throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
        }

        var splittedCode = code.Split('.');
        return splittedCode[^1];
    }

    /// <summary>
    /// Lấy parent code.
    /// Ví dụ: nếu code = "00019.00055.00001" trả về "00019.00055".
    /// </summary>
    /// <param name="code">Code.</param>
    public static string GetParentCode(string code)
    {
        if (code.IsNullOrEmpty())
        {
            throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
        }

        var splittedCode = code.Split('.');
        return splittedCode.Length == 1 ? null : splittedCode.Take(splittedCode.Length - 1).JoinAsString(".");
    }

    public virtual void AddRole(Guid roleId)
    {
        Check.NotNull(roleId, nameof(roleId));

        if (IsInRole(roleId))
        {
            return;
        }

        Roles.Add(new OrganizationUnitRole(roleId, Id, TenantId));
    }

    public virtual void RemoveRole(Guid roleId)
    {
        Check.NotNull(roleId, nameof(roleId));

        if (!IsInRole(roleId))
        {
            return;
        }

        Roles.RemoveAll(r => r.RoleId == roleId);
    }

    public virtual bool IsInRole(Guid roleId)
    {
        Check.NotNull(roleId, nameof(roleId));

        return Roles.Any(r => r.RoleId == roleId);
    }
}