using System;
using IdentityServer4;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Backend.IdentityServer;

public abstract class Secret : Entity
{
    public string Type { get; protected set; }

    public string Value { get; set; }

    public string Description { get; set; }

    public DateTime? Expiration { get; set; }

    protected Secret()
    {

    }

    protected Secret(
        [NotNull] string value,
        DateTime? expiration = null,
        string type = IdentityServerConstants.SecretTypes.SharedSecret,
        string description = null)
    {
        Check.NotNull(value, nameof(value));

        Value = value;
        Expiration = expiration;
        Type = type;
        Description = description;
    }
}
