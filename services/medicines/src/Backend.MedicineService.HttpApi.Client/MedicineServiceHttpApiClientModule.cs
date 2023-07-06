﻿using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Backend.MedicineService;

[DependsOn(
    typeof(MedicineServiceApplicationContractsModule),
    typeof(AbpHttpClientModule)
)]
public class MedicineServiceHttpApiClientModule : AbpModule
{
    public const string RemoteServiceName = "Default";

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(MedicineServiceApplicationContractsModule).Assembly,
            RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<MedicineServiceHttpApiClientModule>();
        });
    }
}
