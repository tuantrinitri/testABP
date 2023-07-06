using Backend.Account.Localization;
using Backend.Account.Web.Pages.Account.Components.ProfileManagementGroup.PersonalInfo;
using Backend.Account.Web.ProfileManagement;
using Backend.Identity.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;
using Volo.Abp.AutoMapper;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Http.ProxyScripting.Generators.JQuery;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.Threading;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace Backend.Account.Web;

[DependsOn(
    typeof(AccountApplicationContractsModule),
    typeof(IdentityAspNetCoreModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpAspNetCoreMvcUiThemeSharedModule),
    typeof(AbpExceptionHandlingModule)
    )]
public class AccountWebModule : AbpModule
{
    private static readonly OneTimeRunner OneTimeRunner = new();
    
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(typeof(AccountResource), typeof(AccountWebModule).Assembly);
        });

        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AccountWebModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AccountWebModule>();
        });

        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new AccountUserMenuContributor());
        });

        Configure<AbpToolbarOptions>(options =>
        {
            options.Contributors.Add(new AccountModuleToolbarContributor());
        });

        ConfigureProfileManagementPage();

        context.Services.AddAutoMapperObjectMapper<AccountWebModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<AccountWebAutoMapperProfile>(validate: true);
        });

        Configure<DynamicJavaScriptProxyOptions>(options =>
        {
            options.DisableModule(AccountRemoteServiceConsts.ModuleName);
        });
    }

    private void ConfigureProfileManagementPage()
    {
        Configure<RazorPagesOptions>(options =>
        {
            options.Conventions.AuthorizePage("/Account/Manage");
        });

        Configure<ProfileManagementPageOptions>(options =>
        {
            options.Contributors.Add(new AccountProfileManagementPageContributor());
        });

        // Configure<AbpBundlingOptions>(options =>
        // {
        //     options.ScriptBundles
        //         .Configure(typeof(ManageModel).FullName,
        //             configuration =>
        //             {
        //                 configuration.AddFiles("/client-proxies/account-proxy.js");
        //                 configuration.AddFiles("/Pages/Account/Components/ProfileManagementGroup/Password/Default.js");
        //                 configuration.AddFiles("/Pages/Account/Components/ProfileManagementGroup/PersonalInfo/Default.js");
        //             });
        // });
    }
    
    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() =>
        {
            ModuleExtensionConfigurationHelper
                .ApplyEntityConfigurationToUi(
                    IdentityModuleExtensionConsts.ModuleName,
                    IdentityModuleExtensionConsts.EntityNames.User,
                    editFormTypes: new[] { typeof(AccountProfilePersonalInfoManagementGroupViewComponent.PersonalInfoModel) }
                );
        });
    }
}
