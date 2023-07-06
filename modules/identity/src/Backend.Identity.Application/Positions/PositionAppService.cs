using System;
using System.Threading.Tasks;
using Backend.Identity.Positions.Dto;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Backend.Identity.Positions;

public class PositionAppService: CrudAppService<Position,PositionDto, Guid, GetPositionsInput, PositionCreateDto, PositionUpdateDto>,
    IPositionAppService
{
    public PositionAppService(IRepository<Position, Guid> repository) : base(repository)
    {
    }
}