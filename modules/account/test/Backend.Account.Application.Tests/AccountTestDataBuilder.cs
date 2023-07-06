using System.Security.Claims;
using System.Threading.Tasks;
using Backend.Identity.IdentityUsers;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.Account;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using IdentityUser = Backend.Identity.IdentityUsers.IdentityUser;

public class AccountTestDataBuilder : ITransientDependency
{
    private readonly IGuidGenerator _guidGenerator;
    private readonly IIdentityUserRepository _userRepository;
    private readonly AccountTestData _testData;

    public AccountTestDataBuilder(
        AccountTestData testData,
        IGuidGenerator guidGenerator,
        IIdentityUserRepository userRepository)
    {
        _testData = testData;
        _guidGenerator = guidGenerator;
        _userRepository = userRepository;
    }

    public async Task Build()
    {
        await AddUsers();
    }

    private async Task AddUsers()
    {
        var john = new IdentityUser(_testData.UserJohnId, "john.nash", "john.nash@abp.io");
        john.AddLogin(new UserLoginInfo("github", "john", "John Nash"));
        john.AddLogin(new UserLoginInfo("twitter", "johnx", "John Nash"));
        john.AddClaim(_guidGenerator, new Claim("TestClaimType", "42"));
        john.SetToken("test-provider", "test-name", "test-value");
        await _userRepository.InsertAsync(john);
    }
}