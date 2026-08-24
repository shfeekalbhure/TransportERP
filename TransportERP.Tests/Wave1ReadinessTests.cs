using TransportERP.Contracts.Wave1;

namespace TransportERP.Tests;

public sealed class Wave1ReadinessTests
{
    [Fact]
    public void Every_wave1_screen_has_a_readiness_gate()
    {
        var screens = Wave1ScreenCatalog.All.Select(x => x.ScreenId).OrderBy(x => x, StringComparer.Ordinal).ToArray();
        var readiness = Wave1ReadinessCatalog.All.Select(x => x.ScreenId).OrderBy(x => x, StringComparer.Ordinal).ToArray();
        Assert.Equal(screens, readiness);
    }

    [Fact]
    public void Owner_approved_authority_closure_leaves_no_wave1_hold()
    {
        Assert.False(Wave1ReadinessCatalog.HasMergeBlockers);
        Assert.Equal(13, Wave1ReadinessCatalog.All.Count);
        Assert.All(Wave1ReadinessCatalog.All, x =>
        {
            Assert.Equal(Wave1ReadinessState.ImplementedReviewRequired, x.State);
            Assert.Equal("EXACT_SHA_INDEPENDENT_REVIEW", x.Gate);
            Assert.False(string.IsNullOrWhiteSpace(x.EvidenceBasis));
        });
    }
}
