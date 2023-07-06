using System.Threading.Tasks;

namespace Backend.IdentityServer.IdentityResources;

public interface IIdentityResourceDataSeeder
{
    Task CreateStandardResourcesAsync();
}
