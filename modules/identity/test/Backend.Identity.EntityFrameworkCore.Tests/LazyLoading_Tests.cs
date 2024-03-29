﻿using Backend.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace Backend.Identity;

public class LazyLoading_Tests : LazyLoading_Tests<IdentityEntityFrameworkCoreTestModule>
{
    protected override void BeforeAddApplication(IServiceCollection services)
    {
        services.Configure<AbpDbContextOptions>(options =>
        {
            options.PreConfigure<IdentityDbContext>(context =>
            {
                context.DbContextOptions.UseLazyLoadingProxies();
            });
        });
    }
}
