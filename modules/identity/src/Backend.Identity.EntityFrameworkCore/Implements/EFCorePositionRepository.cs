using System;
using Backend.Identity.Positions;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Backend.Identity.EntityFrameworkCore.Implements;

public class EFCorePositionRepository: EfCoreRepository<IIdentityDbContext, Position, Guid>, IPositionRepository
{
    public EFCorePositionRepository(IDbContextProvider<IIdentityDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}