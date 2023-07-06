using System.Threading.Tasks;

namespace Backend.Account.Web.ProfileManagement;

public interface IProfileManagementPageContributor
{
    Task ConfigureAsync(ProfileManagementPageCreationContext context);
}
