using System.Linq;
using System.Threading.Tasks;
using Backend.IdentityServer.Clients;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Modularity;
using Xunit;

namespace Backend.IdentityServer;

public abstract class ClientRepositoryTests<TStartupModule> : IdentityServerTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected IClientRepository clientRepository { get; }

    protected ClientRepositoryTests()
    {
        clientRepository = ServiceProvider.GetRequiredService<IClientRepository>();
    }

    [Fact]
    public async Task FindByClientIdAsync()
    {
        (await clientRepository.FindByClientIdAsync("ClientId2")).ShouldNotBeNull();
    }

    [Fact]
    public async Task GetAllDistinctAllowedCorsOriginsAsync()
    {
        var origins = await clientRepository.GetAllDistinctAllowedCorsOriginsAsync();
        origins.Any().ShouldBeTrue();
    }
}
