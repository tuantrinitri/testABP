﻿using System.Threading.Tasks;
using Backend.Identity.Identity;
using Shouldly;
using Volo.Abp.Identity;
using Xunit;

namespace Backend.Identity;

public class UserRoleFinder_Tests : IdentityDomainTestBase
{
    private readonly IUserRoleFinder _userRoleFinder;
    private readonly IdentityTestData _testData;

    public UserRoleFinder_Tests()
    {
        _userRoleFinder = GetRequiredService<IUserRoleFinder>();
        _testData = GetRequiredService<IdentityTestData>();
    }

    [Fact]
    public async Task GetRolesAsync()
    {
        var roleNames = await _userRoleFinder.GetRolesAsync(_testData.UserJohnId);
        roleNames.ShouldNotBeEmpty();
        roleNames.ShouldContain(x => x == "moderator");
        roleNames.ShouldContain(x => x == "supporter");
    }
}
