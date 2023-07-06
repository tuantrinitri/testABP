using System.Security.Cryptography.X509Certificates;
using Backend.Account;
using Backend.Account.Web;
using Backend.AdministrationService.EntityFrameworkCore;
using Backend.IdentityService;
using Backend.IdentityService.EntityFrameworkCore;
using Backend.Shared.Hosting.AspNetCore;
using IdentityServer4.Configuration;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;
using Volo.Abp;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.Auditing;
using Volo.Abp.BackgroundJobs.RabbitMQ;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.EventBus.RabbitMq;
using Volo.Abp.IdentityServer;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation.Urls;

namespace Backend.AuthServer;

[DependsOn(
    typeof(AbpCachingStackExchangeRedisModule),
    typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
    typeof(AbpEventBusRabbitMqModule),
    typeof(AbpBackgroundJobsRabbitMqModule),
    typeof(AbpAspNetCoreMvcUiBasicThemeModule),
    typeof(AccountWebIdentityServerModule),
    typeof(AccountHttpApiModule),
    typeof(AccountApplicationModule),
    typeof(BackendSharedHostingAspNetCoreModule),
    typeof(AdministrationServiceEntityFrameworkCoreModule),
    typeof(IdentityServiceEntityFrameworkCoreModule)
)]
public class BackendAuthServerModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
    
        if (hostingEnvironment.IsDevelopment()) return;
    
         PreConfigure<AbpIdentityServerBuilderOptions>(options =>
         {
             options.AddDeveloperSigningCredential = false;
         });
    
        // PreConfigure<IIdentityServerBuilder>(builder =>
        // {
        //     builder.AddSigningCredential(GetSigningCertificate(hostingEnvironment));
        // });
    }
    
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
        var configuration = context.Services.GetConfiguration();

        ConfigureSwagger(context, configuration);

        context.Services.AddAuthentication()
            .AddJwtBearer(options =>
            {
                options.Authority = configuration["AuthServer:Authority"];
                options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                options.Audience = "AccountService";
            });

        Configure<IdentityServerOptions>(options => { options.IssuerUri = configuration["App:SelfUrl"]; });

         Configure<AbpClaimsServiceOptions>(options =>
         {
             options.RequestedClaims.AddRange(new[]
             {
                 BackendConstants.AnonymousUserClaimName
             });
         });

        Configure<AbpAuditingOptions>(options => { options.ApplicationName = "AuthServer"; });

        Configure<AppUrlOptions>(options =>
        {
            options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            options.RedirectAllowedUrls.AddRange(configuration["App:RedirectAllowedUrls"]?.Split(',')!);
        });

        Configure<AbpDistributedCacheOptions>(options => { options.KeyPrefix = "Backend:"; });

        var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
        context.Services
            .AddDataProtection()
            .PersistKeysToStackExchangeRedis(redis, "Backend-Protection-Keys")
            .SetApplicationName("Backend-AuthServer");

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
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();

        app.Use(async (ctx, next) =>
        {
            if (ctx.Request.Headers.ContainsKey("from-ingress"))
            {
                ctx.SetIdentityServerOrigin(configuration["App:SelfUrl"]);
            }
        
            await next();
        });

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAbpRequestLocalization();

        if (!env.IsDevelopment())
        {
            app.UseErrorPage();
        }

        app.UseCorrelationId();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors();
        app.UseCookiePolicy();
        app.UseAuthentication();
        app.UseJwtTokenMiddleware();
        app.UseAbpSerilogEnrichers();
        app.UseUnitOfWork();
        app.UseIdentityServer();
        app.UseAuthorization();
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Account Service API");
            options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
            options.OAuthClientSecret(configuration["AuthServer:SwaggerClientSecret"]);
        });
        app.UseAuditing();
        app.UseConfiguredEndpoints();
    }

    private X509Certificate2 GetSigningCertificate(IWebHostEnvironment hostingEnv)
    {
        const string fileName = "backend-authserver.pfx";
        const string passPhrase = "780F3C11-0A96-40DE-B335-9848BE88C77D";
        var file = Path.Combine(hostingEnv.ContentRootPath, fileName);

        if (!File.Exists(file))
        {
            throw new FileNotFoundException($"Signing Certificate couldn't found: {file}");
        }

        return new X509Certificate2(file, passPhrase);
    }

    private void ConfigureSwagger(ServiceConfigurationContext context, IConfiguration configuration)
    {
        SwaggerConfigurationHelper.ConfigureWithAuth(
            context: context,
            authority: configuration["AuthServer:Authority"],
            scopes: new Dictionary<string, string>
            {
                /* Requested scopes for authorization code request and descriptions for swagger UI only */
                {
                    "AccountService",
                    "Account Service API"
                },
            },
            apiTitle: "Account Service API"
        );
    }
}