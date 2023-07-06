using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Backend.MedicineService.EntityFrameworkCore;

[ConnectionStringName(MedicineServiceDbProperties.ConnectionStringName)]
public class MedicineServiceDbContext : AbpDbContext<MedicineServiceDbContext>
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    public MedicineServiceDbContext(DbContextOptions<MedicineServiceDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Configure your own tables/entities inside here */

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(MedicineServiceConsts.DbTablePrefix + "YourEntities", MedicineServiceConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});
    }
}
