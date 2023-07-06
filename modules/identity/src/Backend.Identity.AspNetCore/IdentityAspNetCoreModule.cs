using Backend.Identity;
using Backend.Identity.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using IdentityUser = Backend.Identity.IdentityUsers.IdentityUser;

[DependsOn(
    typeof(IdentityDomainModule)
)]
public class IdentityAspNetCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IdentityBuilder>(builder =>
        {
            builder
                .AddDefaultTokenProviders()
                .AddTokenProvider<LinkUserTokenProvider>(LinkUserTokenProviderConsts.LinkUserTokenProviderName)
                .AddSignInManager<AbpSignInManager>();
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddScoped<AbpSecurityStampValidator>();
        context.Services.AddScoped(typeof(SecurityStampValidator<IdentityUser>), provider => provider.GetService(typeof(AbpSecurityStampValidator)));
        context.Services.AddScoped(typeof(ISecurityStampValidator), provider => provider.GetService(typeof(AbpSecurityStampValidator)));

        var options = context.Services.ExecutePreConfiguredActions(new IdentityAspNetCoreOptions());

        if (options.ConfigureAuthentication)
        {
            context.Services
                .AddAuthentication(o =>
                {
                    o.DefaultScheme = IdentityConstants.ApplicationScheme;
                    o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                })
                .AddIdentityCookies();
        }
    }
}