using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.Testing;

namespace Backend.IdentityServer;

public class IdentityServerTestBase<TStartupModule> : AbpIntegratedTest<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
