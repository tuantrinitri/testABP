using Backend.IdentityService.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Backend.IdentityService.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class IdentityServiceController : AbpControllerBase
{
    protected IdentityServiceController()
    {
        LocalizationResource = typeof(IdentityServiceResource);
    }
}
