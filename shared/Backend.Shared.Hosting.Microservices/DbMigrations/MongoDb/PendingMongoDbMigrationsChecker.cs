using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Serilog;
using Volo.Abp.Data;
using Volo.Abp.DistributedLocking;
using Volo.Abp.MongoDB;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace Backend.Shared.Hosting.Microservices.DbMigrations.MongoDb;

public class PendingMongoDbMigrationsChecker<TDbContext> : PendingMigrationsCheckerBase
{
    protected IUnitOfWorkManager UnitOfWorkManager { get; }
    protected IServiceProvider ServiceProvider { get; }
    protected ICurrentTenant CurrentTenant { get; }
    protected IDataSeeder DataSeeder { get; }
    protected IAbpDistributedLock DistributedLockProvider { get; }
    protected string DatabaseName { get; }

    protected PendingMongoDbMigrationsChecker(
        IUnitOfWorkManager unitOfWorkManager,
        IServiceProvider serviceProvider,
        ICurrentTenant currentTenant,
        IDataSeeder dataSeeder,
        IAbpDistributedLock distributedLockProvider,
        string databaseName)
    {
        UnitOfWorkManager = unitOfWorkManager;
        ServiceProvider = serviceProvider;
        CurrentTenant = currentTenant;
        DataSeeder = dataSeeder;
        DistributedLockProvider = distributedLockProvider;
        DatabaseName = databaseName;
    }

    public virtual async Task CheckAndApplyDatabaseMigrationsAsync()
    {
        await TryAsync(async () =>
        {
            using (CurrentTenant.Change(null))
            {
                // Create database tables if needed
                using (var uow = UnitOfWorkManager.Begin(requiresNew: true, isTransactional: false))
                {
                    await MigrateDatabaseSchemaAsync();

                    await DataSeeder.SeedAsync();

                    await uow.CompleteAsync();
                }
            }
        });
    }

    /// <summary>
    /// Apply scheme update for MongoDB Database.
    /// </summary>
    private async Task MigrateDatabaseSchemaAsync()
    {
        await using var handle = await DistributedLockProvider.TryAcquireAsync("Migration_" + DatabaseName);
        using var uow = UnitOfWorkManager.Begin(requiresNew: true, isTransactional: false);
        Log.Information("Lock is acquired for db migration and seeding on database named: {DatabaseName}...",
            DatabaseName);

        if (handle is null)
        {
            Log.Information("Handle is null because of the locking for : {DatabaseName}", DatabaseName);
            return;
        }

        async Task MigrateDatabaseSchemaWithDbContextAsync()
        {
            var dbContexts = ServiceProvider.GetServices<IAbpMongoDbContext>();
            var connectionStringResolver = ServiceProvider.GetRequiredService<IConnectionStringResolver>();

            foreach (var dbContext in dbContexts)
            {
                var connectionString =
                    await connectionStringResolver.ResolveAsync(
                        ConnectionStringNameAttribute.GetConnStringName(dbContext.GetType()));
                if (connectionString.IsNullOrWhiteSpace())
                {
                    continue;
                }

                var mongoUrl = new MongoUrl(connectionString);
                var databaseName = mongoUrl.DatabaseName;
                var client = new MongoClient(mongoUrl);

                if (databaseName.IsNullOrWhiteSpace())
                {
                    databaseName = ConnectionStringNameAttribute.GetConnStringName(dbContext.GetType());
                }

                (dbContext as AbpMongoDbContext)?.InitializeCollections(client.GetDatabase(databaseName));
            }
        }

        //Migrating the host database
        await MigrateDatabaseSchemaWithDbContextAsync();

        await uow.CompleteAsync();
    }
}