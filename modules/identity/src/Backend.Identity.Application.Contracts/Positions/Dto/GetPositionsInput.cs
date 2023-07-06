using Volo.Abp.Application.Dtos;

namespace Backend.Identity.Positions.Dto;

public class GetPositionsInput: PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}