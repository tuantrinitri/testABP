using System;
using Backend.Identity.Ranks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Backend.Identity.EntityFrameworkCore.Implements;

public class EFCoreRankRepository: EfCoreRepository<IdentityDbContext, Rank, Guid>, IRankRepository
{
    public EFCoreRankRepository(IDbContextProvider<IdentityDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}