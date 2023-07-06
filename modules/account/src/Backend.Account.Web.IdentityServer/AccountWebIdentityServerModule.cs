using Backend.IdentityServer;
using IdentityServer4.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Backend.Account.Web;

[DependsOn(
    typeof(AccountWebModule),
    typeof(IdentityServerDomainModule)
    )]
public class AccountWebIdentityServerModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<IdentityAspNetCoreOptions>(options =>
        {
            options.ConfigureAuthentication = false;
        });

        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AccountWebIdentityServerModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AccountWebIdentityServerModule>();
        });

        Configure<IdentityServerOptions>(options =>
        {
            options.UserInteraction.ConsentUrl = "/Consent";
            options.UserInteraction.ErrorUrl = "/Account/Error";
        });

        //Try to reuse from AbpIdentityAspNetCoreModule
        context.Services
            .AddAuthentication(o =>
            {
                o.DefaultScheme = IdentityConstants.ApplicationScheme;
                o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddIdentityCookies();
    }
}
