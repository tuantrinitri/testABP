﻿using Backend.AdministrationService.EntityFrameworkCore;
using Backend.Shared.Hosting.AspNetCore;
using Medallion.Threading;
using Medallion.Threading.Redis;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.Auditing;
using Volo.Abp.BackgroundJobs.RabbitMQ;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.DistributedLocking;
using Volo.Abp.EventBus.RabbitMq;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace Backend.Shared.Hosting.Microservices;

[DependsOn(
    typeof(BackendSharedHostingAspNetCoreModule),
    typeof(AbpBackgroundJobsRabbitMqModule),
    typeof(AbpAspNetCoreMultiTenancyModule),
    typeof(AbpEventBusRabbitMqModule),
    typeof(AbpCachingStackExchangeRedisModule),
    typeof(AdministrationServiceEntityFrameworkCoreModule),
    typeof(AbpDistributedLockingModule)
)]
public class BackendSharedHostingMicroservicesModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
        var configuration = context.Services.GetConfiguration();

        Configure<AbpMultiTenancyOptions>(options =>
        {
            options.IsEnabled = true;
        });

        Configure<AbpDistributedCacheOptions>(options =>
        {
            options.KeyPrefix = "Backend:";
        });
        
        Configure<AbpAuditingOptions>(options =>
        {
            options.EntityHistorySelectors.AddAllEntities();
        });

        var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
        context.Services
            .AddDataProtection()
            .PersistKeysToStackExchangeRedis(redis, "Backend-Protection-Keys");
            
        context.Services.AddSingleton<IDistributedLockProvider>(sp =>
        {
            var connection = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
            return new RedisDistributedSynchronizationProvider(connection.GetDatabase());
        });
    }
}