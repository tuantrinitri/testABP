using System;
using System.Threading.Tasks;
using Backend.Identity.Ranks.Dto;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Backend.Identity.Ranks;

public class RankAppService: CrudAppService<Rank, RankDto, Guid, GetRanksInput, RankCreateDto, RankUpdateDto>, IRankAppService
{
    public RankAppService(IRepository<Rank, Guid> repository) : base(repository)
    {
    }
}