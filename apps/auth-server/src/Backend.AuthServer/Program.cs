using Backend.Shared.Hosting.AspNetCore;
using Serilog;

namespace Backend.AuthServer;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var assemblyName = typeof(Program).Assembly.GetName().Name;

        SerilogConfigurationHelper.Configure(assemblyName);

        try
        {
            Log.Information("Starting {AssemblyName}", assemblyName);
            var app = await ApplicationBuilderHelper.BuildApplicationAsync<BackendAuthServerModule>(args);
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