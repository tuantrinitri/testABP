using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.IdentityService.DbMigrations;
using Backend.IdentityService.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Backend.Shared.Hosting.AspNetCore;
using Backend.Shared.Hosting.Gateways;
using Backend.Shared.Hosting.Microservices;
using Microsoft.AspNetCore.Identity;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace Backend.IdentityService;

[DependsOn(
    typeof(BackendSharedHostingMicroservicesModule),
    typeof(IdentityServiceHttpApiModule),
    typeof(IdentityServiceApplicationModule),
    typeof(IdentityServiceEntityFrameworkCoreModule),
    typeof(BackendSharedHostingGatewaysModule)
)]
public class IdentityServiceHttpApiHostModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        JwtBearerConfigurationHelper.Configure(context, "IdentityService");

        SwaggerConfigurationHelper.ConfigureWithAuth(
            context: context,
            authority: configuration["AuthServer:Authority"],
            scopes: new
                Dictionary<string, string> /* Requested scopes for authorization code request and descriptions for swagger UI only */
                {
                    { "IdentityService", "Identity Service API" }
                },
            apiTitle: "IdentityService API"
        );
        
        context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins(
                        configuration["App:CorsOrigins"]!
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
        
        Configure<IdentityOptions>(options =>
        {
            options.User.AllowedUserNameCharacters = "";
        });
        
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(typeof(IdentityServiceApplicationModule).Assembly, opts =>
            {
                opts.RootPath = "identity";
                opts.RemoteServiceName = "Identity";
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
        app.UseAbpSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "IdentityService API");

            var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
            c.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
            c.OAuthScopes("IdentityService");
        });
        app.UseAbpSerilogEnrichers();
        app.UseUnitOfWork();
        app.UseAuditing();
        app.UseConfiguredEndpoints();
    }
    
    public override async Task OnPostApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        await context.ServiceProvider
            .GetRequiredService<IdentityServiceDatabaseMigrationChecker>()
            .CheckAndApplyDatabaseMigrationsAsync();
    }
}
