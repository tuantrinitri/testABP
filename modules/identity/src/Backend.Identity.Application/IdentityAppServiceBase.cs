using Backend.Identity.Identity;
using Backend.Identity.Localization;
using Volo.Abp.Application.Services;

namespace Backend.Identity;

public class IdentityAppServiceBase: ApplicationService
{
    protected IdentityAppServiceBase()
    {
        ObjectMapperContext = typeof(IdentityApplicationModule);
        LocalizationResource = typeof(IdentityResource);
    }
}