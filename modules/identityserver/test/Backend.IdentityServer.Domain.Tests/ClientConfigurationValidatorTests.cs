using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.IdentityServer.EntityFrameworkCore;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Backend.IdentityServer;

public class ClientConfigurationValidatorTests : IdentityServerTestBase
{
    private readonly IClientConfigurationValidator _abpClientConfigurationValidator;

    private readonly Client _testClient = new()
    {
        AllowedGrantTypes = GrantTypes.Code,

        ClientSecrets = new List<IdentityServer4.Models.Secret>
        {
                new("1q2w3e*")
            },

        RedirectUris = new List<string>
            {
                "https://{0}.api.abp.io:8080",
                "http://{0}.ng.abp.io",
                "http://ng.abp.io"
            }
    };

    public ClientConfigurationValidatorTests()
    {
        _abpClientConfigurationValidator = GetRequiredService<IClientConfigurationValidator>();
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.AddAbpClientConfigurationValidator();
    }

    [Fact]
    public async Task ValidateAsync()
    {
        var context = new ClientConfigurationValidationContext(_testClient);

        await _abpClientConfigurationValidator.ValidateAsync(context);

        context.IsValid.ShouldBeTrue();
    }
}
