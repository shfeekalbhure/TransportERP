using TransportERP.Desktop.Themes;

namespace TransportERP.Desktop.CoreUI.Controls;

/// <summary>Single source of shared CoreUI geometry and presentation defaults at 96 DPI.</summary>
internal static class CoreUIProperties
{
    internal const int ControlHeight = 32;
    internal const int FieldRowHeight = 40;
    internal const int RowGap = 8;
    internal const int ColumnGap = 16;
    internal const int ContainerPadding = 16;
    internal const int DefaultInputColumns = 2;
    internal const int MaximumInputColumns = 3;
    internal const int GridRowHeight = 32;
    internal const int GridHeaderHeight = 32;
    internal static readonly Font DefaultFont = UiTheme.CreateRegularFont(10F);
    internal static readonly Color WorkspaceColor = UiTheme.WorkspaceBackground;
    internal static readonly Color SurfaceColor = UiTheme.SurfaceBackground;
    internal const RightToLeft Rtl = RightToLeft.Yes;
    internal const AutoScaleMode DpiScale = AutoScaleMode.Dpi;
}
