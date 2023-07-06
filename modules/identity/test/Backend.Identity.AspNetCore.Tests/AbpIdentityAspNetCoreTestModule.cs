using Backend.Identity.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Modularity;

namespace Backend.Identity;

[DependsOn(
    //typeof(IdentityAspNetCoreModule),
    typeof(IdentityDomainTestModule),
    typeof(AbpAspNetCoreTestBaseModule),
    typeof(AbpAspNetCoreMvcModule)
)]
public class AbpIdentityAspNetCoreTestModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<IMvcBuilder>(builder =>
        {
            builder.PartManager.ApplicationParts.Add(new AssemblyPart(typeof(AbpIdentityAspNetCoreTestModule).Assembly));
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {

    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseConfiguredEndpoints();
    }
}
