﻿using System.ComponentModel.DataAnnotations;
using Backend.Identity.Identity;
using Volo.Abp.Auditing;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;

namespace Backend.Account.Dto;

public class RegisterDto : ExtensibleObject
{
    [Required]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxUserNameLength))]
    public string UserName { get; set; }

    [Required]
    [EmailAddress]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
    public string EmailAddress { get; set; }

    [Required]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
    [DataType(DataType.Password)]
    [DisableAuditing]
    public string Password { get; set; }

    [Required]
    public string AppName { get; set; }
}
