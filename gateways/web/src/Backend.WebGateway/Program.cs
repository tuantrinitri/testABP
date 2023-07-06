using System;
using System.Threading.Tasks;
using Backend.Shared.Hosting.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Backend.WebGateway;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var assemblyName = typeof(Program).Assembly.GetName().Name;

        SerilogConfigurationHelper.Configure(assemblyName);

        try
        {
            Log.Information("Starting {AssemblyName}", assemblyName);
            var builder = WebApplication.CreateBuilder(args);
            builder.Host
                .AddAppSettingsSecretsJson()
                .AddYarpJson()
                .UseAutofac()
                .UseSerilog();

            await builder.AddApplicationAsync<BackendWebGatewayModule>();
            var app = builder.Build();
            await app.InitializeApplicationAsync();
            await app.RunAsync();
            
            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "{AssemblyName} terminated unexpectedly!", assemblyName);
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
