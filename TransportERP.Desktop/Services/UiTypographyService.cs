namespace TransportERP.Desktop.Services;

/// <summary>
/// خدمة مركزية لتطبيق إعدادات الخط والمحاذاة على شاشات النظام.
/// الإعداد العام هو الأصل، ويمكن إضافة تخصيص حسب رمز الشاشة لاحقًا.
/// </summary>
public static class UiTypographyService
{
    private const string DefaultFontFamily = "Segoe UI";

    public static void Apply(Form form, string screenCode)
    {
        ArgumentNullException.ThrowIfNull(form);

        var settings = GetSettings(screenCode);
        form.Font = CreateFont(settings.BodySize, FontStyle.Regular);
        ApplyRecursively(form, settings);
    }

    private static TypographySettings GetSettings(string screenCode)
    {
        // الإعداد العام المعتمد حاليًا. عند إنشاء شاشة إعدادات الخطوط
        // سيُستبدل هذا المصدر بقراءة الإعدادات العامة وتخصيص الشاشة.
        return new TypographySettings(
            BodySize: 10F,
            ButtonSize: 10F,
            SectionSize: 11F,
            TitleSize: 22F,
            AuditSize: 9.5F);
    }

    private static void ApplyRecursively(Control parent, TypographySettings settings)
    {
        foreach (Control control in parent.Controls)
        {
            control.RightToLeft = RightToLeft.Yes;

            switch (control)
            {
                case DataGridView grid:
                    ApplyGridTypography(grid, settings);
                    break;

                case Button button:
                    button.Font = CreateFont(settings.ButtonSize, FontStyle.Bold);
                    button.TextAlign = ContentAlignment.MiddleCenter;
                    break;

                case TextBox textBox:
                    textBox.Font = CreateFont(settings.BodySize, FontStyle.Regular);
                    textBox.TextAlign = HorizontalAlignment.Right;
                    break;

                case MaskedTextBox maskedTextBox:
                    maskedTextBox.Font = CreateFont(settings.BodySize, FontStyle.Regular);
                    maskedTextBox.TextAlign = HorizontalAlignment.Right;
                    break;

                case RichTextBox richTextBox:
                    richTextBox.Font = CreateFont(settings.BodySize, FontStyle.Regular);
                    richTextBox.SelectionAlignment = HorizontalAlignment.Right;
                    break;

                case ComboBox comboBox:
                    comboBox.Font = CreateFont(settings.BodySize, FontStyle.Regular);
                    comboBox.RightToLeft = RightToLeft.Yes;
                    break;

                case Label label:
                    ApplyLabelTypography(label, settings);
                    break;

                default:
                    control.Font = CreateFont(settings.BodySize, FontStyle.Regular);
                    break;
            }

            if (control.HasChildren)
            {
                ApplyRecursively(control, settings);
            }
        }
    }

    private static void ApplyGridTypography(DataGridView grid, TypographySettings settings)
    {
        grid.EnableHeadersVisualStyles = false;
        grid.Font = CreateFont(settings.BodySize, FontStyle.Regular);
        grid.DefaultCellStyle.Font = CreateFont(settings.BodySize, FontStyle.Regular);
        grid.ColumnHeadersDefaultCellStyle.Font = CreateFont(settings.ButtonSize, FontStyle.Bold);
        grid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        grid.RightToLeft = RightToLeft.Yes;
    }

    private static void ApplyLabelTypography(Label label, TypographySettings settings)
    {
        var text = label.Text.Trim();

        if (IsScreenTitle(text))
        {
            label.Font = CreateFont(settings.TitleSize, FontStyle.Bold);
        }
        else if (IsSectionHeading(text))
        {
            label.Font = CreateFont(settings.SectionSize, FontStyle.Bold);
        }
        else if (IsAuditOrCounterText(text))
        {
            label.Font = CreateFont(settings.AuditSize, FontStyle.Regular);
        }
        else
        {
            label.Font = CreateFont(settings.BodySize, FontStyle.Regular);
        }

        label.TextAlign = ContentAlignment.MiddleRight;
    }

    private static bool IsScreenTitle(string text) => text is "الدول";

    private static bool IsSectionHeading(string text) => text is
        "البيانات الرئيسية" or
        "بحث وتصفية" or
        "قائمة الدول" or
        "بيانات الإنشاء والتعديل" or
        "العدادات والإحصاءات";

    private static bool IsAuditOrCounterText(string text) => text.Contains("تاريخ", StringComparison.Ordinal)
        || text.Contains("بواسطة", StringComparison.Ordinal)
        || text.Contains("عدد", StringComparison.Ordinal)
        || text.Contains("آخر طباعة", StringComparison.Ordinal)
        || text.Contains("آخر تعديل", StringComparison.Ordinal);

    private static Font CreateFont(float size, FontStyle style) =>
        new(DefaultFontFamily, size, style, GraphicsUnit.Point);

    private sealed record TypographySettings(
        float BodySize,
        float ButtonSize,
        float SectionSize,
        float TitleSize,
        float AuditSize);
}
