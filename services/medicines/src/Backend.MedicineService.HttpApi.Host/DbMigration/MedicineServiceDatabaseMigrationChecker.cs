using System;
using Backend.MedicineService.EntityFrameworkCore;
using Backend.Shared.Hosting.Microservices.DbMigrations.EfCore;
using Volo.Abp.DistributedLocking;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace Backend.MedicineService.DbMigration;

public class MedicineServiceDatabaseMigrationChecker
    : PendingEfCoreMigrationsChecker<MedicineServiceDbContext>
{
    public MedicineServiceDatabaseMigrationChecker(
        IUnitOfWorkManager unitOfWorkManager,
        IServiceProvider serviceProvider, 
        ICurrentTenant currentTenant, 
        IDistributedEventBus distributedEventBus,
        IAbpDistributedLock abpDistributedLock) : base(
            unitOfWorkManager, 
            serviceProvider,
            currentTenant, 
            distributedEventBus, 
            abpDistributedLock, 
            MedicineServiceDbProperties.ConnectionStringName)
    {
    }
}