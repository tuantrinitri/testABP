using System;
using System.Threading.Tasks;
using Serilog;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace Backend.Shared.Hosting.Microservices.DbMigrations;

public abstract class PendingMigrationsCheckerBase : ITransientDependency
{
    public async Task TryAsync(Func<Task> task, int retryCount = 3)
    {
        try
        {
            await task();
        }
        catch (Exception ex)
        {
            retryCount--;

            if (retryCount <= 0)
            {
                throw;
            }

            Log.Warning("{Name} has been thrown. The operation will be tried {RetryCount} times more. Exception:\n{ExMessage}", ex.GetType().Name, retryCount, ex.Message);

            await Task.Delay(RandomHelper.GetRandom(5000, 15000));

            await TryAsync(task, retryCount);
        }
    }
}