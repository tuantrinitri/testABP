using System.IO;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace Backend.Shared.Hosting.AspNetCore;

public static class SerilogConfigurationHelper
{
    public static void Configure(string applicationName)
    {
        // TODO: Uncomment following lines for ElasticSearch configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

        Log.Logger = new LoggerConfiguration()
#if DEBUG
            .MinimumLevel.Debug()
#else
                .MinimumLevel.Information()
#endif
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", $"{applicationName}")
            .WriteTo.Async(c => c.File("Logs/logs.txt"))
            // TODO: Uncomment following lines for ElasticSearch configuration
            // .WriteTo.Elasticsearch(
            //     new ElasticsearchSinkOptions(new Uri(configuration["ElasticSearch:Url"]))
            //     {
            //         AutoRegisterTemplate = true,
            //         AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6,
            //         IndexFormat = "Backend-log-{0:yyyy.MM}"
            //     })
            .WriteTo.Async(c => c.Console())
            .CreateLogger();
    }
}