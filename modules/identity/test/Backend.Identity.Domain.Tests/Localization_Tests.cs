using Backend.Identity.Identity;
using Backend.Identity.Localization;
using Microsoft.Extensions.Localization;
using Shouldly;
using Volo.Abp.Localization;
using Xunit;

namespace Backend.Identity;

public class Localization_Tests : IdentityDomainTestBase
{
    private readonly IStringLocalizer<IdentityResource> _stringLocalizer;

    public Localization_Tests()
    {
        _stringLocalizer = GetRequiredService<IStringLocalizer<IdentityResource>>();
    }

    [Fact]
    public void Test()
    {
        using (CultureHelper.Use("en"))
        {
            _stringLocalizer["PersonalSettingsSavedMessage"].Value
            .ShouldBe("Your personal settings has been saved successfully.");
        }
    }
}
