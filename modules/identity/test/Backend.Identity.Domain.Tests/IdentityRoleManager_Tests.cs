using System.Threading.Tasks;
using Backend.Identity.IdentityRoles;
using Microsoft.AspNetCore.Identity;
using Shouldly;
using Xunit;

namespace Backend.Identity;

public class IdentityRoleManager_Tests : IdentityDomainTestBase
{
    private readonly IdentityRoleManager _identityRoleManager;
    private readonly IIdentityRoleRepository _identityRoleRepository;
    private readonly ILookupNormalizer _lookupNormalizer;
    private readonly IdentityTestData _testData;

    public IdentityRoleManager_Tests()
    {
        _identityRoleManager = GetRequiredService<IdentityRoleManager>();
        _identityRoleRepository = GetRequiredService<IIdentityRoleRepository>();
        _lookupNormalizer = GetRequiredService<ILookupNormalizer>();
        _testData = GetRequiredService<IdentityTestData>();
    }


    [Fact]
    public async Task GetByIdAsync()
    {
        var role = await _identityRoleManager.FindByIdAsync(_testData.RoleModeratorId.ToString());

        role.ShouldNotBeNull();
        role.Name.ShouldBe("moderator");
    }

    [Fact]
    public async Task SetRoleNameAsync()
    {
        var role = await _identityRoleRepository.FindByNormalizedNameAsync(_lookupNormalizer.NormalizeName("moderator"));
        role.ShouldNotBeNull();

        (await _identityRoleManager.SetRoleNameAsync(role, "teacher")).Succeeded.ShouldBeTrue();

        role.Name.ShouldBe("teacher");
    }

    [Fact]
    public async Task DeleteAsync()
    {
        var role = await _identityRoleRepository.FindByNormalizedNameAsync(_lookupNormalizer.NormalizeName("moderator"));
        role.ShouldNotBeNull();

        await _identityRoleManager.DeleteAsync(role);

        (await _identityRoleRepository.FindByNormalizedNameAsync(_lookupNormalizer.NormalizeName("moderator"))).ShouldBeNull();
    }

}
