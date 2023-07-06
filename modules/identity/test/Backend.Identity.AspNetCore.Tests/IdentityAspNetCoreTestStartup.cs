using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Backend.Identity.AspNetCore;

public class IdentityAspNetCoreTestStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplication<AbpIdentityAspNetCoreTestModule>();
    }

    public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
    {
        app.InitializeApplication();
    }
}
