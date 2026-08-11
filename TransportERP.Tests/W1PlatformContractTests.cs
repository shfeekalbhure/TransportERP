using System.Globalization;
using TransportERP.Desktop.CoreUI.Controls;
using TransportERP.Desktop.CoreUI.Presentation;

namespace TransportERP.Tests;

public sealed class W1PlatformContractTests
{
    [Fact]
    public void Presentation_context_supports_Arabic_Rtl_and_English_Ltr_at_each_approved_scale()
    {
        foreach (var dpiPercentage in TransportPresentationDesignTokens.SupportedDpiPercentages)
        {
            var arabic = TransportPresentationContext.Arabic(dpiPercentage);
            var english = TransportPresentationContext.English(dpiPercentage);

            Assert.Equal(RightToLeft.Yes, arabic.RightToLeft);
            Assert.Equal(HorizontalAlignment.Right, arabic.TextAlignment);
            Assert.Equal(FlowDirection.RightToLeft, arabic.FlowDirection);
            Assert.Equal(RightToLeft.No, english.RightToLeft);
            Assert.Equal(HorizontalAlignment.Left, english.TextAlignment);
            Assert.Equal(FlowDirection.LeftToRight, english.FlowDirection);
        }
    }

    [Fact]
    public void Presentation_context_rejects_an_unapproved_dpi_scale()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new TransportPresentationContext(
                CultureInfo.InvariantCulture,
                TransportPresentationDirection.LeftToRight,
                110));
    }

    [Fact]
    public void Lookup_contract_describes_selection_without_a_data_source_or_authority()
    {
        var contract = new LookupPresentationContract(
            "LKP-SHARED-001",
            "Reference",
            "Presentation",
            LookupSelectionMode.Single,
            ["CompanyContext", "BranchContext"]);

        Assert.Equal("LKP-SHARED-001", contract.LookupId);
        Assert.Equal(LookupSelectionMode.Single, contract.SelectionMode);
        Assert.Equal(new[] { "CompanyContext", "BranchContext" }, contract.AllowedFilters);
        Assert.DoesNotContain(
            typeof(LookupPresentationContract).GetProperties(),
            property => property.Name.Contains("Endpoint", StringComparison.OrdinalIgnoreCase)
                || property.Name.Contains("Permission", StringComparison.OrdinalIgnoreCase)
                || property.Name.Contains("Cache", StringComparison.OrdinalIgnoreCase)
                || property.Name.Contains("Entity", StringComparison.OrdinalIgnoreCase));

        var selection = new LookupPresentationSelection(contract, "item-1");
        selection.EnsureComplete();
        Assert.Equal("item-1", selection.SelectedId);
    }

    [Fact]
    public void Lookup_contract_rejects_missing_presentation_identity()
    {
        Assert.Throws<ArgumentException>(
            () => new LookupPresentationContract(" ", "Reference", "Presentation", LookupSelectionMode.Single));
    }

    [Fact]
    public void Validation_presentation_requires_a_field_and_a_validation_visual_state()
    {
        var error = new InputValidationPresentation("SharedField", TransportFieldVisualState.Error, "validation.required");

        error.EnsureComplete();

        Assert.Throws<ArgumentException>(
            () => new InputValidationPresentation("SharedField", TransportFieldVisualState.Normal).EnsureComplete());
        Assert.Throws<ArgumentException>(
            () => new InputValidationPresentation(" ", TransportFieldVisualState.Warning).EnsureComplete());
    }
}
