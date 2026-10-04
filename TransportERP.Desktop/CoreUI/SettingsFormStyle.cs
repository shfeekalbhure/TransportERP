namespace TransportERP.Desktop.CoreUI;

/// <summary>
/// Applies visual properties to controls that already exist in the WinForms Designer hierarchy.
/// This helper must never create controls or compose a control hierarchy.
/// </summary>
internal static class SettingsFormStyle
{
    public static void ApplyFormStyle(Form form, bool wide = false)
    {
        form.BackColor = UiDesignTokens.Surface;
        form.Font = UiDesignTokens.BodyFont();
        form.MinimumSize = wide ? UiDesignTokens.MinimumWideFormSize : UiDesignTokens.MinimumSettingsFormSize;
        form.RightToLeft = RightToLeft.Yes;
        form.RightToLeftLayout = true;
        form.StartPosition = FormStartPosition.CenterScreen;
    }

    public static void ApplyHeaderStyle(Panel header, Label title, Label subtitle)
    {
        header.BackColor = UiDesignTokens.PrimaryStrong;
        header.Height = UiDesignTokens.HeaderHeight;
        header.Padding = new Padding(UiDesignTokens.Space24, UiDesignTokens.Space12, UiDesignTokens.Space24, UiDesignTokens.Space12);
        title.Font = UiDesignTokens.HeaderFont();
        title.ForeColor = Color.White;
        title.TextAlign = ContentAlignment.MiddleRight;
        subtitle.Font = UiDesignTokens.BodyFont();
        subtitle.ForeColor = Color.FromArgb(225, 235, 248);
        subtitle.TextAlign = ContentAlignment.MiddleRight;
    }

    public static void ApplySectionStyle(GroupBox group)
    {
        group.Font = UiDesignTokens.SectionFont();
        group.ForeColor = UiDesignTokens.PrimaryStrong;
        group.Padding = UiDesignTokens.SectionPadding;
    }

    public static void ApplyLabelStyle(Label label, bool required = false)
    {
        label.Font = UiDesignTokens.LabelFont();
        label.ForeColor = required ? UiDesignTokens.Required : UiDesignTokens.TextPrimary;
        label.TextAlign = ContentAlignment.MiddleRight;
    }

    public static void ApplyInputStyle(Control input)
    {
        input.Font = UiDesignTokens.InputFont();
        input.Height = UiDesignTokens.ControlHeight;
        input.Margin = new Padding(UiDesignTokens.Space4, UiDesignTokens.FieldVerticalSpacing, UiDesignTokens.Space4, UiDesignTokens.FieldVerticalSpacing);
        input.RightToLeft = RightToLeft.Yes;
    }

    public static void ApplyPrimaryButtonStyle(Button button) => ApplyButtonStyle(button, UiDesignTokens.Primary, Color.White);

    public static void ApplySecondaryButtonStyle(Button button) => ApplyButtonStyle(button, Color.White, UiDesignTokens.Primary);

    public static void ApplyDangerButtonStyle(Button button) => ApplyButtonStyle(button, UiDesignTokens.Danger, Color.White);

    public static void ApplyNeutralButtonStyle(Button button) => ApplyButtonStyle(button, UiDesignTokens.ReadOnly, UiDesignTokens.TextPrimary);

    public static void ApplyGridStyle(DataGridView grid)
    {
        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.BackgroundColor = UiDesignTokens.PanelSurface;
        grid.BorderStyle = BorderStyle.FixedSingle;
        grid.ColumnHeadersHeight = 40;
        grid.EnableHeadersVisualStyles = false;
        grid.Font = UiDesignTokens.BodyFont();
        grid.MultiSelect = false;
        grid.ReadOnly = true;
        grid.RightToLeft = RightToLeft.Yes;
        grid.RowHeadersVisible = false;
        grid.RowTemplate.Height = UiDesignTokens.GridRowHeight;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
    }

    public static void ApplyRequiredLabelStyle(Label label) => ApplyLabelStyle(label, true);

    public static void ApplyInvalidStyle(Control control)
    {
        control.BackColor = Color.FromArgb(254, 243, 242);
        control.ForeColor = UiDesignTokens.Error;
    }

    public static void ApplyWarningStyle(Control control)
    {
        control.BackColor = Color.FromArgb(255, 250, 235);
        control.ForeColor = UiDesignTokens.Warning;
    }

    public static void ApplyReadOnlyStyle(Control control)
    {
        control.BackColor = UiDesignTokens.ReadOnly;
        control.ForeColor = UiDesignTokens.TextSecondary;
    }

    public static void ApplyDisabledStyle(Control control)
    {
        control.BackColor = UiDesignTokens.Disabled;
        control.ForeColor = UiDesignTokens.TextSecondary;
    }

    public static void ApplySuccessStyle(Control control)
    {
        control.BackColor = Color.FromArgb(236, 253, 243);
        control.ForeColor = UiDesignTokens.Success;
    }

    public static void ApplyInfoStyle(Control control)
    {
        control.BackColor = Color.FromArgb(239, 248, 255);
        control.ForeColor = UiDesignTokens.Info;
    }

    private static void ApplyButtonStyle(Button button, Color backColor, Color foreColor)
    {
        button.AutoSize = false;
        button.BackColor = backColor;
        button.FlatStyle = FlatStyle.Flat;
        button.Font = UiDesignTokens.LabelFont();
        button.ForeColor = foreColor;
        button.Height = UiDesignTokens.ButtonHeight;
        button.Margin = new Padding(UiDesignTokens.Space4);
        button.UseVisualStyleBackColor = false;
        button.Width = UiDesignTokens.ButtonWidth;
    }
}
