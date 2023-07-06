using System;
using Backend.Identity.Ranks.Dto;
using Volo.Abp.Application.Services;

namespace Backend.Identity.Ranks;

public interface IRankAppService: ICrudAppService<
    RankDto, 
    Guid, 
    GetRanksInput, 
    RankCreateDto,
    RankUpdateDto>
{
}