using Volo.Abp;
using Volo.Abp.Testing;

namespace Backend.IdentityServer;

public class IdentityServerDomainTestBase : AbpIntegratedTest<IdentityServerDomainTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
