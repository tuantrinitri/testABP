using Volo.Abp.Application.Dtos;

namespace Backend.Identity.Ranks.Dto;

public class GetRanksInput: PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}