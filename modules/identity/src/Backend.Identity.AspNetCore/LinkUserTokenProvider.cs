using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using IdentityUser = Backend.Identity.IdentityUsers.IdentityUser;

public class LinkUserTokenProvider : DataProtectorTokenProvider<IdentityUser>
{
    public LinkUserTokenProvider(
        IDataProtectionProvider dataProtectionProvider,
        IOptions<DataProtectionTokenProviderOptions> options,
        ILogger<DataProtectorTokenProvider<IdentityUser>> logger)
        : base(dataProtectionProvider, options, logger)
    {

    }
}