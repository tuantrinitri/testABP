using System.Threading.Tasks;
using Backend.IdentityServer.Devices;
using Backend.IdentityServer.EntityFrameworkCore;
using Backend.IdentityServer.Grants;
using Shouldly;
using Xunit;

namespace Backend.IdentityServer.Tokens;

public class TokenCleanupServiceTests : IdentityServerTestBase
{
    private readonly IPersistentGrantRepository _persistentGrantRepository;
    private readonly IDeviceFlowCodesRepository _deviceFlowCodesRepository;
    private readonly TokenCleanupService _tokenCleanupService;

    public TokenCleanupServiceTests()
    {
        _persistentGrantRepository = GetRequiredService<IPersistentGrantRepository>();
        _deviceFlowCodesRepository = GetRequiredService<IDeviceFlowCodesRepository>();
        _tokenCleanupService = GetRequiredService<TokenCleanupService>();
    }

    [Fact]
    public async Task Should_Clear_Expired_Tokens()
    {
        var persistentGrantCount = await _persistentGrantRepository.GetCountAsync();
        var deviceFlowCodesCount = await _deviceFlowCodesRepository.GetCountAsync();

        await _tokenCleanupService.CleanAsync();

        (await _persistentGrantRepository.GetCountAsync())
            .ShouldBe(persistentGrantCount - 1);

        (await _deviceFlowCodesRepository.GetCountAsync())
            .ShouldBe(deviceFlowCodesCount - 1);
    }
}
