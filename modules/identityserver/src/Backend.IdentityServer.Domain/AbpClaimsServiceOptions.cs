using System.Collections.Generic;

namespace Backend.IdentityServer;

public class AbpClaimsServiceOptions
{
    public List<string> RequestedClaims { get; }

    public AbpClaimsServiceOptions()
    {
        RequestedClaims = new List<string>();
    }
}
