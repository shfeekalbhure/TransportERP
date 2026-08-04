namespace TransportERP.Desktop;

/// <summary>
/// تطبيق معيار الخط المعتمد على شاشة الدول فقط.
/// </summary>
public partial class FrmCountries
{
    private static readonly Font CountriesBodyFont = new("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
    private static readonly Font CountriesButtonFont = new("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
    private static readonly Font CountriesSectionFont = new("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
    private static readonly Font CountriesTitleFont = new("Segoe UI", 22F, FontStyle.Bold, GraphicsUnit.Point);
    private static readonly Font CountriesAuditFont = new("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        ApplyApprovedTypography();
    }

    /// <summary>
    /// توحيد الخط والمحاذاة حسب القرار البصري المعتمد.
    /// </summary>
    private void ApplyApprovedTypography()
    {
        Font = CountriesBodyFont;
        ApplyTypographyRecursively(this);

        dgvCountries.EnableHeadersVisualStyles = false;
        dgvCountries.ColumnHeadersDefaultCellStyle.Font = CountriesButtonFont;
        dgvCountries.DefaultCellStyle.Font = CountriesBodyFont;
        dgvCountries.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        dgvCountries.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
    }

    private void ApplyTypographyRecursively(Control parent)
    {
        foreach (Control control in parent.Controls)
        {
            control.RightToLeft = RightToLeft.Yes;

            switch (control)
            {
                case Button button:
                    button.Font = CountriesButtonFont;
                    button.TextAlign = ContentAlignment.MiddleCenter;
                    break;

                case TextBoxBase textBox:
                    textBox.Font = CountriesBodyFont;
                    textBox.TextAlign = HorizontalAlignment.Right;
                    break;

                case ComboBox comboBox:
                    comboBox.Font = CountriesBodyFont;
                    comboBox.RightToLeft = RightToLeft.Yes;
                    break;

                case Label label when string.Equals(label.Text.Trim(), "الدول", StringComparison.Ordinal):
                    label.Font = CountriesTitleFont;
                    label.TextAlign = ContentAlignment.MiddleRight;
                    break;

                case Label label when IsSectionHeading(label.Text):
                    label.Font = CountriesSectionFont;
                    label.TextAlign = ContentAlignment.MiddleRight;
                    break;

                case Label label when IsAuditOrCounterLabel(label):
                    label.Font = CountriesAuditFont;
                    label.TextAlign = ContentAlignment.MiddleRight;
                    break;

                case Label label:
                    label.Font = CountriesBodyFont;
                    label.TextAlign = ContentAlignment.MiddleRight;
                    break;
            }

            if (control.HasChildren)
            {
                ApplyTypographyRecursively(control);
            }
        }
    }

    private static bool IsSectionHeading(string text)
    {
        var value = text.Trim();
        return value is "البيانات الرئيسية"
            or "بحث وتصفية"
            or "قائمة الدول"
            or "بيانات الإنشاء والتعديل"
            or "العدادات والإحصاءات";
    }

    private bool IsAuditOrCounterLabel(Label label)
    {
        return pnlAudit.Contains(label) || pnlCounters.Contains(label);
    }
}
