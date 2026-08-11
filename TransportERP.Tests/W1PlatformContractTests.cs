using System.Globalization;
using System.Reflection;
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
    public void Presentation_policy_applies_direction_recursively_and_preserves_lookup_direction_after_interaction()
    {
        var root = new Panel();
        var textBox = new TextBox();
        var label = new Label { Text = new string('ط', 160) };
        var datePicker = new DateTimePicker();
        var flow = new FlowLayoutPanel();
        var lookup = new LookupComboBox();
        root.Controls.Add(textBox);
        root.Controls.Add(label);
        root.Controls.Add(datePicker);
        root.Controls.Add(flow);
        root.Controls.Add(lookup);

        TransportPresentationPolicy.Apply(root, TransportPresentationContext.English(125));
        InvokeLookupLifecycle(lookup, "HandleEnter");
        InvokeLookupLifecycle(lookup, "HandleLeave");

        Assert.Equal(RightToLeft.No, root.RightToLeft);
        Assert.Equal(HorizontalAlignment.Left, textBox.TextAlign);
        Assert.Equal(ContentAlignment.MiddleLeft, label.TextAlign);
        Assert.False(datePicker.RightToLeftLayout);
        Assert.Equal(FlowDirection.LeftToRight, flow.FlowDirection);
        Assert.Equal(RightToLeft.No, lookup.RightToLeft);

        TransportPresentationPolicy.Apply(root, TransportPresentationContext.Arabic(150));
        InvokeLookupLifecycle(lookup, "HandleEnter");
        InvokeLookupLifecycle(lookup, "HandleLeave");

        Assert.Equal(RightToLeft.Yes, root.RightToLeft);
        Assert.Equal(HorizontalAlignment.Right, textBox.TextAlign);
        Assert.Equal(ContentAlignment.MiddleRight, label.TextAlign);
        Assert.True(datePicker.RightToLeftLayout);
        Assert.Equal(FlowDirection.RightToLeft, flow.FlowDirection);
        Assert.Equal(RightToLeft.Yes, lookup.RightToLeft);
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
    public void Lookup_combo_binds_caller_supplied_presentation_items_and_exposes_only_the_selection()
    {
        var contract = new LookupPresentationContract("LKP-SHARED-001", "Reference", "Presentation", LookupSelectionMode.Single);
        var lookup = new LookupComboBox();

        lookup.BindPresentationItems(contract, [new LookupPresentationItem("item-1", "A1", "Display")]);

        Assert.Same(contract, lookup.PresentationContract);
        Assert.Equal("item-1", lookup.SelectedPresentation!.SelectedId);
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

    [Fact]
    public void Validation_presentation_preserves_the_active_presentation_direction()
    {
        var error = new InputValidationPresentation("SharedField", TransportFieldVisualState.Error);
        var textBox = new TextBox();

        InputValidationPresenter.Apply(textBox, error, TransportPresentationContext.English(100));
        Assert.Equal(HorizontalAlignment.Left, textBox.TextAlign);
        Assert.Equal(RightToLeft.No, textBox.RightToLeft);

        InputValidationPresenter.Apply(textBox, error, TransportPresentationContext.Arabic(200));
        Assert.Equal(HorizontalAlignment.Right, textBox.TextAlign);
        Assert.Equal(RightToLeft.Yes, textBox.RightToLeft);
    }

    [Fact]
    public void Dynamic_visibility_relayouts_the_parent_without_hiding_other_required_content()
    {
        var parent = new FlowLayoutPanel { AutoSize = true };
        var requiredContent = new TextBox { Text = new string('x', 160) };
        var optionalContent = new Label { Text = "Optional" };
        parent.Controls.Add(requiredContent);
        parent.Controls.Add(optionalContent);

        TransportPresentationPolicy.SetDynamicVisibility(optionalContent, false);

        Assert.False(optionalContent.Visible);
        Assert.True(requiredContent.Visible);
        Assert.Contains(requiredContent, parent.Controls.Cast<Control>());
    }

    private static void InvokeLookupLifecycle(LookupComboBox lookup, string methodName)
    {
        var method = typeof(LookupComboBox).GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.NotNull(method);
        method!.Invoke(lookup, new object?[] { null, EventArgs.Empty });
    }
}
