using System;
using System.Threading.Tasks;
using Backend.Shared.Hosting.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Serilog;

namespace Backend.IdentityService;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var assemblyName = typeof(Program).Assembly.GetName().Name;

        SerilogConfigurationHelper.Configure(assemblyName);

        try
        {
            Log.Information("Starting {AssemblyName}", assemblyName);
            var app = await ApplicationBuilderHelper
                .BuildApplicationAsync<IdentityServiceHttpApiHostModule>(args);
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
            await Log.CloseAndFlushAsync();
        }
    }
}