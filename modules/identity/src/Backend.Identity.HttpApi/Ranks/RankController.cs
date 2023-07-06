using System;
using System.Threading.Tasks;
using Backend.Identity.Ranks.Dto;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Backend.Identity.Ranks;

[RemoteService(Name = IdentityRemoteServiceConsts.RemoteServiceName)]
[Area(IdentityRemoteServiceConsts.ModuleName)]
[ControllerName("Rank")]
[Route("api/identity/ranks")]
public class RankController: AbpControllerBase, IRankAppService
{
    private IRankAppService RankAppService { get; }

    public RankController(IRankAppService rankAppService)
    {
        RankAppService = rankAppService;
    }

    [HttpGet]
    [Route("{id}")]
    public Task<RankDto> GetAsync(Guid id)
    {
        return RankAppService.GetAsync(id);
    }

    [HttpGet]
    public Task<PagedResultDto<RankDto>> GetListAsync(GetRanksInput input)
    {
        return RankAppService.GetListAsync(input);
    }
    
    [HttpPost]
    public Task<RankDto> CreateAsync(RankCreateDto input)
    {
        return RankAppService.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    public Task<RankDto> UpdateAsync(Guid id, RankUpdateDto input)
    {
        return RankAppService.UpdateAsync(id, input);
    }
    
    [HttpDelete]
    [Route("{id}")]
    public Task DeleteAsync(Guid id)
    {
        return RankAppService.DeleteAsync(id);
    }
}