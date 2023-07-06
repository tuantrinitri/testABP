using System;
using System.Threading.Tasks;
using Backend.Identity.Positions.Dto;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Backend.Identity.Positions;

[RemoteService(Name = IdentityRemoteServiceConsts.RemoteServiceName)]
[Area(IdentityRemoteServiceConsts.ModuleName)]
[ControllerName("Position")]
[Route("api/identity/positions")]
public class PositionController: AbpControllerBase, IPositionAppService
{
    private IPositionAppService PositionAppService { get; }

    public PositionController(IPositionAppService positionAppService)
    {
        PositionAppService = positionAppService;
    }
    
    [HttpGet]
    [Route("{id}")]
    public Task<PositionDto> GetAsync(Guid id)
    {
        return PositionAppService.GetAsync(id);
    }

    [HttpGet]
    public Task<PagedResultDto<PositionDto>> GetListAsync(GetPositionsInput input)
    {
        return PositionAppService.GetListAsync(input);
    }

    [HttpPost]
    public Task<PositionDto> CreateAsync(PositionCreateDto input)
    {
        return PositionAppService.CreateAsync(input);
    }
    
    [HttpPut]
    [Route("{id}")]
    public Task<PositionDto> UpdateAsync(Guid id, PositionUpdateDto input)
    {
        return PositionAppService.UpdateAsync(id, input);
    }
    
    [HttpDelete]
    [Route("{id}")]
    public Task DeleteAsync(Guid id)
    {
        return PositionAppService.DeleteAsync(id);
    }
}