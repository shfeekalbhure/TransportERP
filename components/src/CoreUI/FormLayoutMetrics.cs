namespace TransportERP.Desktop.CoreUI;

internal enum FormDensity
{
    Standard,
    Compact
}

internal sealed record FormLayoutMetrics(
    int RowHeight,
    int InputHeight,
    int LabelWidth,
    Padding FieldMargin,
    Padding InputMargin)
{
    public static FormLayoutMetrics For(FormDensity density)
    {
        return density == FormDensity.Compact
            ? new FormLayoutMetrics(
                RowHeight: 38,
                InputHeight: 30,
                LabelWidth: 124,
                FieldMargin: new Padding(0, 3, 0, 3),
                InputMargin: new Padding(0, 4, 0, 0))
            : new FormLayoutMetrics(
                RowHeight: 42,
                InputHeight: 32,
                LabelWidth: 132,
                FieldMargin: new Padding(0, 4, 0, 4),
                InputMargin: new Padding(0, 6, 0, 0));
    }
}
