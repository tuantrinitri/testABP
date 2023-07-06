using System.Threading.Tasks;
using Backend.IdentityServer.ApiResources;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Modularity;
using Xunit;

namespace Backend.IdentityServer;

public abstract class ApiResourceRepositoryTests<TStartupModule> : IdentityServerTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected IApiResourceRepository ApiResourceRepository { get; }

    public ApiResourceRepositoryTests()
    {
        ApiResourceRepository = ServiceProvider.GetRequiredService<IApiResourceRepository>();
    }

    [Fact]
    public async Task FindByNormalizedNameAsync()
    {
        (await ApiResourceRepository.FindByNameAsync(new[] { "NewApiResource2" })).ShouldNotBeNull();
    }

    [Fact]
    public async Task GetListByScopesAsync()
    {
        (await ApiResourceRepository.GetListByScopesAsync(new[] { "NewApiResource2", "NewApiResource3" })).Count.ShouldBe(2);
    }
}
