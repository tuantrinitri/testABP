using System;
using Backend.Identity.Positions.Dto;
using Volo.Abp.Application.Services;

namespace Backend.Identity.Positions;

public interface IPositionAppService
    : ICrudAppService<
        PositionDto,
        Guid,
        GetPositionsInput,
        PositionCreateDto,
        PositionUpdateDto>
{
}