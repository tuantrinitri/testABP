using Backend.IdentityServer.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Backend.IdentityServer;

[DependsOn(typeof(IdentityServerTestEntityFrameworkCoreModule))]
public class IdentityServerDomainTestModule : AbpModule
{

}
