using System.Threading.Tasks;
using Backend.IdentityServer.IdentityResources;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Modularity;
using Xunit;

namespace Backend.IdentityServer;

public abstract class IdentityResourceRepositoryTests<TStartupModule> : IdentityServerTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected IIdentityResourceRepository IdentityResourceRepository;

    public IdentityResourceRepositoryTests()
    {
        IdentityResourceRepository = ServiceProvider.GetRequiredService<IIdentityResourceRepository>();
    }

    [Fact]
    public async Task GetListByScopesAsync()
    {
        (await IdentityResourceRepository.GetListByScopeNameAsync(new[] { "", "NewIdentityResource2" })).Count.ShouldBe(1);
    }
}
