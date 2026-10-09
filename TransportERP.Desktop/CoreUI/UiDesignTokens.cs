namespace TransportERP.Desktop.CoreUI;

/// <summary>Applied visual baseline for Arabic desktop forms.</summary>
internal static class UiDesignTokens
{
    // Noto Sans Arabic was not installed on the verification machine (2026-09-06).
    // Segoe UI is the documented Windows fallback until the governed font is deployed.
    public const string FontFamily = "Segoe UI";

    public const float FontSizeFinePrint = 9F;
    public const float FontSizeBody = 10F;
    public const float FontSizeInput = 11F;
    public const float FontSizeSection = 12F;
    public const float FontSizeHeader = 20F;

    public const int Space4 = 4;
    public const int Space8 = 8;
    public const int Space12 = 12;
    public const int Space16 = 16;
    public const int Space24 = 24;
    public const int Space32 = 32;

    public const int HeaderHeight = 88;
    public const int ActionBarHeight = 60;
    public const int ControlHeight = 34;
    public const int ButtonHeight = 38;
    public const int ButtonWidth = 112;
    public const int GridRowHeight = 36;
    public const int SectionSpacing = 12;
    public const int FieldVerticalSpacing = 8;

    public static readonly Size MinimumSettingsFormSize = new(980, 680);
    public static readonly Size MinimumWideFormSize = new(1120, 720);
    public static readonly Padding FormPadding = new(Space16);
    public static readonly Padding SectionPadding = new(Space16, Space24, Space16, Space16);

    public static readonly Color TextPrimary = Color.FromArgb(16, 24, 40);
    public static readonly Color TextSecondary = Color.FromArgb(71, 84, 103);
    public static readonly Color Primary = Color.FromArgb(21, 94, 239);
    public static readonly Color PrimaryStrong = Color.FromArgb(0, 53, 128);
    public static readonly Color Surface = Color.FromArgb(248, 250, 252);
    public static readonly Color PanelSurface = Color.White;
    public static readonly Color Border = Color.FromArgb(208, 213, 221);
    public static readonly Color ReadOnly = Color.FromArgb(242, 244, 247);
    public static readonly Color Disabled = Color.FromArgb(234, 236, 240);
    public static readonly Color Danger = Color.FromArgb(180, 35, 24);
    public static readonly Color Warning = Color.FromArgb(181, 71, 8);
    public static readonly Color Error = Color.FromArgb(180, 35, 24);
    public static readonly Color Success = Color.FromArgb(6, 118, 71);
    public static readonly Color Info = Color.FromArgb(23, 92, 211);
    public static readonly Color Required = Color.FromArgb(180, 35, 24);

    public static Font HeaderFont() => new(FontFamily, FontSizeHeader, FontStyle.Bold);
    public static Font SectionFont() => new(FontFamily, FontSizeSection, FontStyle.Bold);
    public static Font LabelFont() => new(FontFamily, FontSizeBody, FontStyle.Bold);
    public static Font InputFont() => new(FontFamily, FontSizeInput, FontStyle.Regular);
    public static Font BodyFont() => new(FontFamily, FontSizeBody, FontStyle.Regular);
    public static Font FinePrintFont() => new(FontFamily, FontSizeFinePrint, FontStyle.Regular);

    // Compatibility names retained for existing visual-only code outside this remediation scope.
    public static Font TitleFont() => HeaderFont();
    public static Font SectionTitleFont() => SectionFont();
    public static Font OptionFont() => LabelFont();
    public static Font SecondaryFont() => BodyFont();
    public static readonly Color Accent = Info;
    public static readonly Color FieldGroupSurface = Surface;
    public static readonly Color OptionsSurface = ReadOnly;
    public static readonly Color FieldBorder = Border;
}
