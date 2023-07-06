using Backend.Identity;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;

namespace Backend.Account;

[DependsOn(
    typeof(AccountApplicationContractsModule),
    typeof(IdentityApplicationModule),
    typeof(AbpUiNavigationModule)
)]
public class AccountApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AccountApplicationModule>();
        });

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<AccountApplicationModuleAutoMapperProfile>(validate: true);
        });

        Configure<AppUrlOptions>(options =>
        {
            options.Applications["MVC"].Urls[AccountUrlNames.PasswordReset] = "Account/ResetPassword";
        });
    }
}
