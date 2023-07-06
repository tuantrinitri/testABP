using System;
using Backend.IdentityService.EntityFrameworkCore;
using Backend.Shared.Hosting.Microservices.DbMigrations.EfCore;
using Volo.Abp.Data;
using Volo.Abp.DistributedLocking;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace Backend.IdentityService.DbMigrations;

public class IdentityServiceDatabaseMigrationChecker : PendingEfCoreMigrationsChecker<IdentityServiceDbContext>
{
    public IdentityServiceDatabaseMigrationChecker(
        IUnitOfWorkManager unitOfWorkManager,
        IServiceProvider serviceProvider,
        ICurrentTenant currentTenant,
        IDistributedEventBus distributedEventBus,
        IAbpDistributedLock abpDistributedLock)
        : base(
            unitOfWorkManager,
            serviceProvider,
            currentTenant,
            distributedEventBus,
            abpDistributedLock,
            IdentityServiceDbProperties.ConnectionStringName)
    {
    }
}