﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Claims;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Guids;
using Volo.Abp.Users;

namespace Backend.Identity.IdentityUsers
{
    public class IdentityUser : FullAuditedAggregateRoot<Guid>, IUser, IHasEntityVersion
    {
        public virtual Guid? TenantId { get; protected set; }

        /// <summary>
        /// Get hoặc set tên user cho user này.
        /// </summary>
        public virtual string UserName { get; protected internal set; }

        /// <summary>
        /// Get hoặc set tên user được chuẩn hóa cho user này.
        /// </summary>
        [DisableAuditing]
        public virtual string NormalizedUserName { get; protected internal set; }

        /// <summary>
        /// Get hoặc set Name cho user này.
        /// </summary>
        [CanBeNull]
        public virtual string Name { get; set; }

        /// <summary>
        /// Get hoặc set Surname cho user này.
        /// </summary>
        [CanBeNull]
        public virtual string Surname { get; set; }

        public virtual string Email { get; protected internal set; }

        [DisableAuditing]
        public virtual string NormalizedEmail { get; protected internal set; }

        public virtual bool EmailConfirmed { get; protected internal set; }

        [DisableAuditing]
        public virtual string PasswordHash { get; protected internal set; }

        [DisableAuditing]
        public virtual string SecurityStamp { get; protected internal set; }

        public virtual bool IsExternal { get; set; }

        [CanBeNull]
        public virtual string PhoneNumber { get; protected internal set; }

        /// <summary>
        /// Get hoặc set cờ cho biết user đã xác nhận địa chỉ điện thoại của họ chưa.
        /// </summary>
        /// <value>True nếu số điện thoại đã được xác nhận, nếu không false.</value>
        public virtual bool PhoneNumberConfirmed { get; protected internal set; }

        /// <summary>
        /// Get hoặc set một lá cờ cho biết nếu user đang hoạt động.
        /// </summary>
        public virtual bool IsActive { get; protected internal set; }

        /// <summary>
        /// Get hoặc set cờ cho biết liệu xác thực hai yếu tố có được bật cho user này hay không.
        /// </summary>
        /// <value>True nếu 2fa được bật, nếu không false.</value>
        public virtual bool TwoFactorEnabled { get; protected internal set; }

        /// <summary>
        /// Get hoặc set ngày và giờ, tính bằng UTC, khi bất kỳ khóa user nào kết thúc.
        /// </summary>
        /// <remarks>
        /// Một giá trị trong quá khứ có nghĩa là user không bị khóa.
        /// </remarks>
        public virtual DateTimeOffset? LockoutEnd { get; set; }

        /// <summary>
        /// Get hoặc set một lá cờ cho biết liệu user có thể bị khóa hay không.
        /// </summary>
        /// <value>True nếu user có thể bị khóa, nếu không false.</value>
        public virtual bool LockoutEnabled { get; set; }

        /// <summary>
        /// Get hoặc set số lần đăng nhập không thành công cho user hiện tại.
        /// </summary>
        public virtual int AccessFailedCount { get; set; }

        /// <summary>
        /// Nên thay đổi mật khẩu vào lần đăng nhập tiếp theo.
        /// </summary>
        public virtual bool ShouldChangePasswordOnNextLogin { get; protected internal set; }

        /// <summary>
        /// Giá trị phiên bản được tăng lên bất cứ khi nào thực thể được thay đổi.
        /// </summary>
        public virtual int EntityVersion { get; protected set; }

        /// <summary>
        /// Get hoặc set thời gian thay đổi mật khẩu cuối cùng cho user.
        /// </summary>
        public virtual DateTimeOffset? LastPasswordChangeTime { get; protected set; }
        
        /// <summary>
        /// Thuộc tính điều hướng cho Role của user này.
        /// </summary>
        public virtual ICollection<IdentityUserRole> Roles { get; protected set; }

        /// <summary>
        /// Thuộc tính điều hướng cho các Claim mà user này sở hữu.
        /// </summary>
        public virtual ICollection<IdentityUserClaim> Claims { get; protected set; }

        /// <summary>
        /// Thuộc tính điều hướng cho tài khoản đăng nhập của user này.
        /// </summary>
        public virtual ICollection<IdentityUserLogin> Logins { get; protected set; }

        /// <summary>
        /// Thuộc tính điều hướng cho token của user này.
        /// </summary>
        public virtual ICollection<IdentityUserToken> Tokens { get; protected set; }

        /// <summary>
        /// Thuộc tính điều hướng cho các đơn vị tổ chức.
        /// </summary>
        public virtual ICollection<IdentityUserOrganizationUnit> OrganizationUnits { get; protected set; }

        [CanBeNull] 
        public virtual UserProfile UserProfile { get; set; }
        
        protected IdentityUser()
        {
        }

        public IdentityUser(
            Guid id,
            [NotNull] string userName,
            [NotNull] string email,
            Guid? tenantId = null)
            : base(id)
        {
            Check.NotNull(userName, nameof(userName));
            Check.NotNull(email, nameof(email));

            TenantId = tenantId;
            UserName = userName;
            NormalizedUserName = userName.ToUpperInvariant();
            Email = email;
            NormalizedEmail = email.ToUpperInvariant();
            ConcurrencyStamp = Guid.NewGuid().ToString("N");
            SecurityStamp = Guid.NewGuid().ToString();
            IsActive = true;

            Roles = new Collection<IdentityUserRole>();
            Claims = new Collection<IdentityUserClaim>();
            Logins = new Collection<IdentityUserLogin>();
            Tokens = new Collection<IdentityUserToken>();
            OrganizationUnits = new Collection<IdentityUserOrganizationUnit>();
        }

        public virtual void AddRole(Guid roleId)
        {
            Check.NotNull(roleId, nameof(roleId));

            if (IsInRole(roleId))
            {
                return;
            }

            Roles.Add(new IdentityUserRole(Id, roleId, TenantId));
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

        public virtual void AddClaim([NotNull] IGuidGenerator guidGenerator, [NotNull] Claim claim)
        {
            Check.NotNull(guidGenerator, nameof(guidGenerator));
            Check.NotNull(claim, nameof(claim));
        
            Claims.Add(new IdentityUserClaim(guidGenerator.Create(), Id, claim.Type, claim.Value ,TenantId));
        }

        public virtual void AddClaims([NotNull] IGuidGenerator guidGenerator, [NotNull] IEnumerable<Claim> claims)
        {
            Check.NotNull(guidGenerator, nameof(guidGenerator));
            Check.NotNull(claims, nameof(claims));
        
            foreach (var claim in claims)
            {
                AddClaim(guidGenerator, claim);
            }
        }

        public virtual IdentityUserClaim FindClaim([NotNull] Claim claim)
        {
            Check.NotNull(claim, nameof(claim));
        
            return Claims.FirstOrDefault(c => c.ClaimType == claim.Type && c.ClaimValue == claim.Value);
        }

        public virtual void ReplaceClaim([NotNull] Claim claim, [NotNull] Claim newClaim)
        {
            Check.NotNull(claim, nameof(claim));
            Check.NotNull(newClaim, nameof(newClaim));
        
            var userClaims = Claims.Where(uc => uc.ClaimValue == claim.Value && uc.ClaimType == claim.Type);
            foreach (var userClaim in userClaims)
            {
                userClaim.SetClaim(newClaim);
            }
        }

        public virtual void RemoveClaims([NotNull] IEnumerable<Claim> claims)
        {
            Check.NotNull(claims, nameof(claims));
        
            foreach (var claim in claims)
            {
                RemoveClaim(claim);
            }
        }

        public virtual void RemoveClaim([NotNull] Claim claim)
        {
            Check.NotNull(claim, nameof(claim));
        
            Claims.RemoveAll(c => c.ClaimValue == claim.Value && c.ClaimType == claim.Type);
        }

        public virtual void AddLogin([NotNull] UserLoginInfo login)
        {
            Check.NotNull(login, nameof(login));

            Logins.Add(new IdentityUserLogin(Id, login, TenantId));
        }

        public virtual void RemoveLogin([NotNull] string loginProvider, [NotNull] string providerKey)
        {
            Check.NotNull(loginProvider, nameof(loginProvider));
            Check.NotNull(providerKey, nameof(providerKey));

            Logins.RemoveAll(userLogin =>
                userLogin.LoginProvider == loginProvider && userLogin.ProviderKey == providerKey);
        }

        [CanBeNull]
        public virtual IdentityUserToken FindToken(string loginProvider, string name)
        {
            return Tokens.FirstOrDefault(t => t.LoginProvider == loginProvider && t.Name == name);
        }

        public virtual void SetToken(string loginProvider, string name, string value)
        {
            var token = FindToken(loginProvider, name);
            if (token == null)
            {
                Tokens.Add(new IdentityUserToken(Id, loginProvider, name, value, TenantId));
            }
            else
            {
                token.Value = value;
            }
        }

        public virtual void RemoveToken(string loginProvider, string name)
        {
            Tokens.RemoveAll(t => t.LoginProvider == loginProvider && t.Name == name);
        }

        public virtual void AddOrganizationUnit(Guid organizationUnitId)
        {
            if (IsInOrganizationUnit(organizationUnitId))
            {
                return;
            }
        
            OrganizationUnits.Add(
                new IdentityUserOrganizationUnit(
                    Id,
                    organizationUnitId,
                    TenantId
                )
            );
        }

        public virtual void RemoveOrganizationUnit(Guid organizationUnitId)
        {
            if (!IsInOrganizationUnit(organizationUnitId))
            {
                return;
            }
        
            OrganizationUnits.RemoveAll(
                ou => ou.OrganizationUnitId == organizationUnitId
            );
        }
        
        public virtual bool IsInOrganizationUnit(Guid organizationUnitId)
        {
            return OrganizationUnits.Any(
                ou => ou.OrganizationUnitId == organizationUnitId
            );
        }

        /// <summary>
        /// Dùng <see cref="IdentityUserManager.ConfirmEmailAsync"/> để xác nhận email thông thường.
        /// Sử dụng điều này bỏ qua quá trình xác nhận và trực tiếp thiết lập <see cref="EmailConfirmed"/>.
        /// </summary>
        public virtual void SetEmailConfirmed(bool confirmed)
        {
            EmailConfirmed = confirmed;
        }

        public virtual void SetPhoneNumberConfirmed(bool confirmed)
        {
            PhoneNumberConfirmed = confirmed;
        }

        /// <summary>
        /// Thông thường sử dụng <see cref="IdentityUserManager.ChangePhoneNumberAsync"/> để thay đổi số điện thoại
        /// trong application code.
        /// Phương thức này trực tiếp thiết lập nó với thông xin xác nhận.
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="confirmed"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void SetPhoneNumber(string phoneNumber, bool confirmed)
        {
            PhoneNumber = phoneNumber;
            PhoneNumberConfirmed = !phoneNumber.IsNullOrWhiteSpace() && confirmed;
        }

        public virtual void SetIsActive(bool isActive)
        {
            IsActive = isActive;
        }

        public virtual void SetShouldChangePasswordOnNextLogin(bool shouldChangePasswordOnNextLogin)
        {
            ShouldChangePasswordOnNextLogin = shouldChangePasswordOnNextLogin;
        }

        public virtual void SetLastPasswordChangeTime(DateTimeOffset? lastPasswordChangeTime)
        {
            LastPasswordChangeTime = lastPasswordChangeTime;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, UserName = {UserName}";
        }
    }
}
