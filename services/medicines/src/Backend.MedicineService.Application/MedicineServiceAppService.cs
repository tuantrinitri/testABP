using System;
using System.Collections.Generic;
using System.Text;
using Backend.MedicineService.Localization;
using Volo.Abp.Application.Services;

namespace Backend.MedicineService;

/* Inherit your application services from this class.
 */
public abstract class MedicineServiceAppService : ApplicationService
{
    protected MedicineServiceAppService()
    {
        LocalizationResource = typeof(MedicineServiceResource);
    }
}
