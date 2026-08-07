namespace TransportERP.Desktop.CoreUI.Controls;

/// <summary>
/// حالات العرض الموحدة للحقول في جميع الشاشات.
/// تمنع هذه الدالة اختلاف ألوان Required وReadOnly وError وWarning من شاشة لأخرى.
/// </summary>
public static class TransportFieldState
{
    public static void Apply(Control control, TransportFieldVisualState state)
    {
        if (control is null)
        {
            return;
        }

        var normalBackColor = Color.White;
        var requiredBackColor = Color.FromArgb(255, 251, 220);
        var readOnlyBackColor = Color.FromArgb(245, 247, 250);
        var warningBackColor = Color.FromArgb(255, 247, 224);
        var errorBackColor = Color.FromArgb(253, 235, 235);

        control.BackColor = state switch
        {
            TransportFieldVisualState.Required => requiredBackColor,
            TransportFieldVisualState.ReadOnly => readOnlyBackColor,
            TransportFieldVisualState.Warning => warningBackColor,
            TransportFieldVisualState.Error => errorBackColor,
            TransportFieldVisualState.Disabled => readOnlyBackColor,
            _ => normalBackColor
        };

        control.Enabled = state != TransportFieldVisualState.Disabled;

        if (control is TextBox textBox)
        {
            textBox.ReadOnly = state == TransportFieldVisualState.ReadOnly;
            textBox.TextAlign = HorizontalAlignment.Right;
        }
    }
}

/// <summary>الحالات البصرية القياسية للحقول.</summary>
public enum TransportFieldVisualState
{
    Normal,
    Required,
    ReadOnly,
    Disabled,
    Warning,
    Error
}
