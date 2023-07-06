using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.MedicineService.DbMigration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Backend.MedicineService.EntityFrameworkCore;
using Backend.Shared.Hosting.AspNetCore;
using Backend.Shared.Hosting.Microservices;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace Backend.MedicineService;

[DependsOn(
    typeof(MedicineServiceHttpApiModule),
    typeof(MedicineServiceApplicationModule),
    typeof(MedicineServiceEntityFrameworkCoreModule),
    typeof(BackendSharedHostingMicroservicesModule)
)]
public class MedicineServiceHttpApiHostModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        JwtBearerConfigurationHelper.Configure(context, "MedicineService");
        
        SwaggerConfigurationHelper.ConfigureWithAuth(
            context: context,
            authority: configuration["AuthServer:Authority"],
            scopes: new
                Dictionary<string, string> /* Requested scopes for authorization code request and descriptions for swagger UI only */
                {
                    { "MedicineService", "Medicine Service API" }
                },
            apiTitle: "Medicine Service API"
        );
        
        context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins(
                        configuration["App:CorsOrigins"]
                            .Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(o => o.Trim().RemovePostFix("/"))
                            .ToArray()
                    )
                    .WithAbpExposedHeaders()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
        
        //tạo controller thay vì auto-controller config
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(typeof(MedicineServiceApplicationModule).Assembly, 
                opts =>
            {
                opts.RootPath = "medicine";
                opts.RemoteServiceName = "Medicine";
            });
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseCorrelationId();
        app.UseCors();
        app.UseAbpRequestLocalization();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAbpClaimsMap();
        app.UseAuthorization();
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Medicine Service API");
            options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
            options.OAuthClientSecret(configuration["AuthServer:SwaggerClientSecret"]);
        });

        app.UseAbpSerilogEnrichers();
        app.UseAuditing();
        app.UseUnitOfWork();
        app.UseConfiguredEndpoints();
    }
    
    public override async Task OnPostApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        await context.ServiceProvider
            .GetRequiredService<MedicineServiceDatabaseMigrationChecker>()
            .CheckAndApplyDatabaseMigrationsAsync();
    }
}
