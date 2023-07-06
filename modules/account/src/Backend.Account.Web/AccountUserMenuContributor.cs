using System.Threading.Tasks;
using Backend.Account.Localization;
using Localization.Resources.AbpUi;
using Volo.Abp.UI.Navigation;

namespace Backend.Account.Web;

public class AccountUserMenuContributor : IMenuContributor
{
    public virtual Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name != StandardMenus.User)
        {
            return Task.CompletedTask;
        }

        var uiResource = context.GetLocalizer<AbpUiResource>();
        var accountResource = context.GetLocalizer<AccountResource>();

        context.Menu.AddItem(new ApplicationMenuItem("Account.Manage", accountResource["MyAccount"], url: "~/Account/Manage", icon: "fa fa-cog", order: 1000, null));
        context.Menu.AddItem(new ApplicationMenuItem("Account.Logout", uiResource["Logout"], url: "~/Account/Logout", icon: "fa fa-power-off", order: int.MaxValue - 1000));

        return Task.CompletedTask;
    }
}
