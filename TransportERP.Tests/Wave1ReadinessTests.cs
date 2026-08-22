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
    [InlineData("GEN-014", "CONTRACT_CODE_PARITY")]
    [InlineData("ACC-036", "W1_PHYSICAL_FIELD_MAPPING")]
    [InlineData("ACC-050", "CASH_FLOW_SOURCE_RECONCILIATION")]
    public void Known_blockers_are_explicit(string screenId, string gate)
    {
        var entry = Wave1ReadinessCatalog.GetRequired(screenId);
        Assert.Equal(Wave1ReadinessState.Hold, entry.State);
        Assert.Equal(gate, entry.Gate);
    }
}
