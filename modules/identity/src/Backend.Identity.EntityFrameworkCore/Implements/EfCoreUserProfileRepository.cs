using System;
using Backend.Identity.IdentityUsers;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Backend.Identity.EntityFrameworkCore.Implements;

public class EfCoreUserProfileRepository : EfCoreRepository<IIdentityDbContext, UserProfile, Guid>,
    IUserProfileRepository
{
    public EfCoreUserProfileRepository(IDbContextProvider<IIdentityDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}