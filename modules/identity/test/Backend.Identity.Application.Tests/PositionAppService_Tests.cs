using System;
using System.Linq;
using System.Threading.Tasks;
using Backend.Identity.IdentityUsers;
using Backend.Identity.Positions;
using Backend.Identity.Positions.Dto;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace Backend.Identity.Application.Tests;

public class PositionAppService_Tests : IdentityApplicationTestBase
{
    private readonly IPositionAppService _positionAppService;
    private readonly IPositionRepository _positionRepository;

    public PositionAppService_Tests()
    {
        _positionAppService = GetRequiredService<IPositionAppService>();
        _positionRepository = GetRequiredService<IPositionRepository>();
    }

    #region Should_Get_List_Of_Positon

    /// <summary>
    /// Test case kiểm tra lấy danh sách chức vụ:<para />
    /// 1. Tạo mới một chức vụ<para />
    /// 2. Lấy danh sách chức vụ<para />
    /// 3. Kiểm tra: tổng số lượng và số lượng trả về nên lớn hơn 0
    /// </summary>
    [Fact]
    public async Task Should_Get_List_Of_Positon()
    {
        //Arrange
        var input = new PositionCreateDto()
        {
            Code = "QLB",
            Name = "Quản lý bếp"
        };

        await _positionAppService.CreateAsync(input);
        
        //Act
        var result = await _positionAppService.GetListAsync(new GetPositionsInput());

        //Assert
        result.TotalCount.ShouldBeGreaterThan(0);
        result.Items.Count.ShouldBeGreaterThan(0);
    }

    #endregion

    private async Task<Position> GetPositionAsync(string positionName)
    {
        return (await _positionRepository.GetListAsync()).First(u => u.Name == positionName);
    }
}