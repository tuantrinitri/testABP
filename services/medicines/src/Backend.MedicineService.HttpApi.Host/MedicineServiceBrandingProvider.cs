using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Backend.MedicineService;

[Dependency(ReplaceServices = true)]
public class MedicineServiceBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "MedicineService";
}
