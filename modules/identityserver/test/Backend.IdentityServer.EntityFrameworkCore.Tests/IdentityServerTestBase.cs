using Volo.Abp;
using Volo.Abp.Testing;

namespace Backend.IdentityServer.EntityFrameworkCore;

public class IdentityServerTestBase : AbpIntegratedTest<IdentityServerTestEntityFrameworkCoreModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
