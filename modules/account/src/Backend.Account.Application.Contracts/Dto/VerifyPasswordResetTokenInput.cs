using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Account.Dto;

public class VerifyPasswordResetTokenInput
{
    public Guid UserId { get; set; }

    [Required]
    public string ResetToken { get; set; }
}
