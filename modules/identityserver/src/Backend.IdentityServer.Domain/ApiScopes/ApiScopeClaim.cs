﻿using System;
using JetBrains.Annotations;

namespace Backend.IdentityServer.ApiScopes;

public class ApiScopeClaim : UserClaim
{
    public Guid ApiScopeId { get; protected set; }

    protected ApiScopeClaim()
    {

    }

    public virtual bool Equals(Guid apiScopeId, [NotNull] string type)
    {
        return ApiScopeId == apiScopeId && Type == type;
    }

    protected internal ApiScopeClaim(Guid apiScopeId, [NotNull] string type)
        : base(type)
    {
        ApiScopeId = apiScopeId;
    }

    public override object[] GetKeys()
    {
        return new object[] { ApiScopeId, Type };
    }
}
