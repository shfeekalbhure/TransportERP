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
    public void No_wave1_screen_is_silently_ready_while_authority_gates_remain()
    {
        Assert.True(Wave1ReadinessCatalog.HasMergeBlockers);
        Assert.DoesNotContain(Wave1ReadinessCatalog.All, x => string.IsNullOrWhiteSpace(x.Gate));
        Assert.DoesNotContain(Wave1ReadinessCatalog.All, x => string.IsNullOrWhiteSpace(x.EvidenceBasis));
    }

    [Theory]
    [InlineData("GEN-003", "W1_PHYSICAL_PROMOTION")]
    [InlineData("GEN-013", "W1_W3_FIELD_SEMANTICS")]
    [InlineData("ACC-036", "W1_W2_ENTITY_DTO_RECONCILIATION")]
    [InlineData("ACC-074", "OPEN_ITEM_SOURCE_RECONCILIATION")]
    [InlineData("ACC-075", "OPEN_ITEM_SOURCE_RECONCILIATION")]
    [InlineData("ACC-050", "OTS_W1_005_CASH_FLOW_CLASSIFICATION")]
    public void Current_governing_blockers_are_explicit(string screenId, string gate)
    {
        var entry = Wave1ReadinessCatalog.GetRequired(screenId);
        Assert.Equal(Wave1ReadinessState.Hold, entry.State);
        Assert.Equal(gate, entry.Gate);
    }

    [Fact]
    public void Wave1_contains_exactly_six_holds_and_seven_review_required_screens()
    {
        Assert.Equal(6, Wave1ReadinessCatalog.All.Count(x => x.State == Wave1ReadinessState.Hold));
        Assert.Equal(7, Wave1ReadinessCatalog.All.Count(x => x.State == Wave1ReadinessState.ImplementedReviewRequired));
    }

    [Theory]
    [InlineData("GEN-004")]
    [InlineData("GEN-005")]
    [InlineData("GEN-006")]
    [InlineData("GEN-007")]
    [InlineData("GEN-014")]
    [InlineData("ACC-049")]
    [InlineData("ACC-058")]
    public void Implemented_screens_remain_review_required_not_ready(string screenId)
    {
        var entry = Wave1ReadinessCatalog.GetRequired(screenId);
        Assert.Equal(Wave1ReadinessState.ImplementedReviewRequired, entry.State);
        Assert.False(string.IsNullOrWhiteSpace(entry.Gate));
    }
}
