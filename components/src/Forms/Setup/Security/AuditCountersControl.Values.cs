using System.ComponentModel;

namespace TransportERP.Desktop.Forms.Setup.Security;

/// <summary>
/// Display values for the eight existing Designer fields. The caller owns formatting,
/// historical identity, unavailable-value wording, and counter event semantics.
/// No session identity, current timestamp, or zero is synthesized here.
/// </summary>
public sealed record AuditCounterDisplayValues(
    string? CreatedBy,
    string? ModifiedBy,
    string? CreatedAt,
    string? ModifiedAt,
    string? CreatedDevice,
    string? ModifiedDevice,
    string? PrintCount,
    string? ModificationCount);

public partial class AuditCountersControl
{
    [Category("Audit values"), Bindable(true), DefaultValue("")]
    [Description("Display text for the historical record creator; not the current session user.")]
    public string CreatedByText { get => textBox4.Text; set => textBox4.Text = value ?? string.Empty; }

    [Category("Audit values"), Bindable(true), DefaultValue("")]
    [Description("Display text for the historical last modifier.")]
    public string ModifiedByText { get => textBox3.Text; set => textBox3.Text = value ?? string.Empty; }

    [Category("Audit values"), Bindable(true), DefaultValue("")]
    [Description("Caller-formatted creation timestamp; no current timestamp is generated.")]
    public string CreatedAtText { get => textBox6.Text; set => textBox6.Text = value ?? string.Empty; }

    [Category("Audit values"), Bindable(true), DefaultValue("")]
    [Description("Caller-formatted last modification timestamp.")]
    public string ModifiedAtText { get => textBox5.Text; set => textBox5.Text = value ?? string.Empty; }

    [Category("Audit values"), Bindable(true), DefaultValue("")]
    [Description("Historical creation-device display value from the caller.")]
    public string CreatedDeviceText { get => textBox8.Text; set => textBox8.Text = value ?? string.Empty; }

    [Category("Audit values"), Bindable(true), DefaultValue("")]
    [Description("Historical modification-device display value from the caller.")]
    public string ModifiedDeviceText { get => textBox7.Text; set => textBox7.Text = value ?? string.Empty; }

    [Category("Counter values"), Bindable(true), DefaultValue("")]
    [Description("Caller-formatted print count; this control does not count events or infer zero.")]
    public string PrintCountText { get => textBox1.Text; set => textBox1.Text = value ?? string.Empty; }

    [Category("Counter values"), Bindable(true), DefaultValue("")]
    [Description("Caller-formatted modification count; event definition belongs to the screen contract.")]
    public string ModificationCountText { get => textBox2.Text; set => textBox2.Text = value ?? string.Empty; }

    /// <summary>Updates only text on controls already created by the Designer.</summary>
    public void SetDisplayValues(AuditCounterDisplayValues values)
    {
        ArgumentNullException.ThrowIfNull(values);
        textBox4.Text = values.CreatedBy ?? string.Empty;
        textBox3.Text = values.ModifiedBy ?? string.Empty;
        textBox6.Text = values.CreatedAt ?? string.Empty;
        textBox5.Text = values.ModifiedAt ?? string.Empty;
        textBox8.Text = values.CreatedDevice ?? string.Empty;
        textBox7.Text = values.ModifiedDevice ?? string.Empty;
        textBox1.Text = values.PrintCount ?? string.Empty;
        textBox2.Text = values.ModificationCount ?? string.Empty;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public AuditCounterDisplayValues DisplayValues => new(
        textBox4.Text, textBox3.Text, textBox6.Text, textBox5.Text,
        textBox8.Text, textBox7.Text, textBox1.Text, textBox2.Text);

    public void ClearDisplayValues() => SetDisplayValues(new(null, null, null, null, null, null, null, null));
}
