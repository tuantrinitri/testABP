using System;
using Volo.Abp.Domain.Repositories;

namespace Backend.Identity.Ranks;

public interface IRankRepository: IBasicRepository<Rank, Guid>
{
    
}