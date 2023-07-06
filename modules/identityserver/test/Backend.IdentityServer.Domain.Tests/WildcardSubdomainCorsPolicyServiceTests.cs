using System.Threading.Tasks;
using Backend.IdentityServer.EntityFrameworkCore;
using IdentityServer4.Services;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Backend.IdentityServer;

public class WildcardSubdomainCorsPolicyServiceTests : IdentityServerTestBase
{
    private readonly ICorsPolicyService _corsPolicyService;

    public WildcardSubdomainCorsPolicyServiceTests()
    {
        _corsPolicyService = GetRequiredService<ICorsPolicyService>();
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.AddAbpWildcardSubdomainCorsPolicyService();
    }

    [Fact]
    public void Should_Register_AbpWildcardSubdomainCorsPolicyService()
    {
        _corsPolicyService.GetType().ShouldBe(typeof(AbpWildcardSubdomainCorsPolicyService));
    }

    [Fact]
    public async Task IsOriginAllowedAsync()
    {
        (await _corsPolicyService.IsOriginAllowedAsync("https://client1-origin.com")).ShouldBeTrue();
        (await _corsPolicyService.IsOriginAllowedAsync("https://client2-origin.com")).ShouldBeFalse();

        (await _corsPolicyService.IsOriginAllowedAsync("https://abp.io")).ShouldBeTrue();
        (await _corsPolicyService.IsOriginAllowedAsync("https://t1.abp.io")).ShouldBeTrue();
        (await _corsPolicyService.IsOriginAllowedAsync("https://t1.ng.abp.io")).ShouldBeTrue();

        (await _corsPolicyService.IsOriginAllowedAsync("https://t1.abp.io.mydomain.com")).ShouldBeFalse();
    }
}
