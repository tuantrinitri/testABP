using System;
using System.Linq;
using System.Threading.Tasks;
using Backend.AdministrationService.EntityFrameworkCore;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DistributedLocking;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;
using Backend.Shared.Hosting.Microservices.DbMigrations.MongoDb;
using Volo.Abp.Data;

namespace Backend.AdministrationService.DbMigrations;

public class AdministrationServiceDatabaseMigrationChecker
    : PendingMongoDbMigrationsChecker<AdministrationServiceDbContext>
{
    private readonly IPermissionDefinitionManager _permissionDefinitionManager;
    private readonly IPermissionDataSeeder _permissionDataSeeder;

    public AdministrationServiceDatabaseMigrationChecker(
        IUnitOfWorkManager unitOfWorkManager,
        IServiceProvider serviceProvider,
        ICurrentTenant currentTenant,
        IDataSeeder dataSeeder,
        IAbpDistributedLock abpDistributedLock,
        IPermissionDefinitionManager permissionDefinitionManager,
        IPermissionDataSeeder permissionDataSeeder)
        : base(
            unitOfWorkManager,
            serviceProvider,
            currentTenant,
            dataSeeder,
            abpDistributedLock,
            AdministrationServiceDbProperties.ConnectionStringName)
    {
        _permissionDefinitionManager = permissionDefinitionManager;
        _permissionDataSeeder = permissionDataSeeder;
    }

    public override async Task CheckAndApplyDatabaseMigrationsAsync()
    {
        await base.CheckAndApplyDatabaseMigrationsAsync();

        await TryAsync(async () => await SeedDataAsync());
    }

    private async Task SeedDataAsync()
    {
        using var uow = UnitOfWorkManager.Begin(requiresNew: true, isTransactional: false);
        const MultiTenancySides multiTenancySide = MultiTenancySides.Host;

        var permissionNames = (await _permissionDefinitionManager
            .GetPermissionsAsync())
            .Where(p => p.MultiTenancySide.HasFlag(multiTenancySide))
            .Where(p => !p.Providers.Any() ||
                        p.Providers.Contains(RolePermissionValueProvider.ProviderName))
            .Select(p => p.Name)
            .ToArray();

        await _permissionDataSeeder.SeedAsync(
            RolePermissionValueProvider.ProviderName,
            "admin",
            permissionNames
        );

        await uow.CompleteAsync();
    }
}