using System;
using Volo.Abp.Domain.Repositories;

namespace Backend.Identity.Positions;

public interface IPositionRepository: IBasicRepository<Position, Guid>
{
    
}