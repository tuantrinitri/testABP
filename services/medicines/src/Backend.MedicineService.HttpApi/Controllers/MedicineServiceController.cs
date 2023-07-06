using Backend.MedicineService.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Backend.MedicineService.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class MedicineServiceController : AbpControllerBase
{
    protected MedicineServiceController()
    {
        LocalizationResource = typeof(MedicineServiceResource);
    }
}
