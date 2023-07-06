using System;
using System.Linq;
using Backend.Identity;
using Backend.Identity.EntityFrameworkCore;
using Backend.Identity.IdentityUsers;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Testing;

public class AccountApplicationTestBase : AbpIntegratedTest<AccountApplicationTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }

    protected virtual IdentityUser GetUser(string userName)
    {
        var user = UsingDbContext(context => context.Users.FirstOrDefault(u => u.UserName == userName));
        if (user == null)
        {
            throw new EntityNotFoundException();
        }

        return user;
    }

    protected virtual T UsingDbContext<T>(Func<IIdentityDbContext, T> action)
    {
        using (var dbContext = GetRequiredService<IIdentityDbContext>())
        {
            return action.Invoke(dbContext);
        }
    }
}