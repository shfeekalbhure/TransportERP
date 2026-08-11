using System.Globalization;

namespace TransportERP.Desktop.CoreUI.Presentation;

/// <summary>
/// اتجاه العرض الذي يقرره سياق الواجهة، ولا يقرر أي قاعدة أعمال أو اسم شاشة.
/// </summary>
public enum TransportPresentationDirection
{
    RightToLeft,
    LeftToRight
}

/// <summary>
/// نقاط القياس المعتمدة لاختبارات واجهة Wave 1.
/// </summary>
public static class TransportPresentationDesignTokens
{
    public static IReadOnlyList<int> SupportedDpiPercentages { get; } = [100, 125, 150, 200];

    public const int MinimumGridLogicalWidth = 120;

    public static void EnsureSupportedDpiPercentage(int value)
    {
        if (!SupportedDpiPercentages.Contains(value))
        {
            throw new ArgumentOutOfRangeException(
                nameof(value),
                value,
                "Only the approved 100, 125, 150, and 200 DPI presentation scales are supported.");
        }
    }
}

/// <summary>
/// سياق عرض محلي مشترك. يبقى النص نفسه لدى المستهلك أو مورد الترجمة؛ لا يحمل هذا العقد تسميات أعمال.
/// </summary>
public sealed record TransportPresentationContext
{
    public TransportPresentationContext(CultureInfo culture, TransportPresentationDirection direction, int dpiPercentage)
    {
        Culture = culture ?? throw new ArgumentNullException(nameof(culture));
        Direction = direction;
        TransportPresentationDesignTokens.EnsureSupportedDpiPercentage(dpiPercentage);
        DpiPercentage = dpiPercentage;
    }

    public CultureInfo Culture { get; }

    public TransportPresentationDirection Direction { get; }

    public int DpiPercentage { get; }

    public RightToLeft RightToLeft => Direction == TransportPresentationDirection.RightToLeft
        ? RightToLeft.Yes
        : RightToLeft.No;

    public HorizontalAlignment TextAlignment => Direction == TransportPresentationDirection.RightToLeft
        ? HorizontalAlignment.Right
        : HorizontalAlignment.Left;

    public ContentAlignment ContentAlignment => Direction == TransportPresentationDirection.RightToLeft
        ? ContentAlignment.MiddleRight
        : ContentAlignment.MiddleLeft;

    public FlowDirection FlowDirection => Direction == TransportPresentationDirection.RightToLeft
        ? FlowDirection.RightToLeft
        : FlowDirection.LeftToRight;

    public static TransportPresentationContext Arabic(int dpiPercentage) =>
        new(new CultureInfo("ar"), TransportPresentationDirection.RightToLeft, dpiPercentage);

    public static TransportPresentationContext English(int dpiPercentage) =>
        new(new CultureInfo("en"), TransportPresentationDirection.LeftToRight, dpiPercentage);
}

/// <summary>
/// يطبق اتجاه العرض بصورة مركزية على المكونات المشتركة فقط.
/// </summary>
public static class TransportPresentationPolicy
{
    public static void Apply(Control control, TransportPresentationContext context)
    {
        ArgumentNullException.ThrowIfNull(control);
        ArgumentNullException.ThrowIfNull(context);

        control.RightToLeft = context.RightToLeft;

        if (control is ITransportPresentationAware presentationAware)
        {
            presentationAware.ApplyPresentationContext(context);
        }

        switch (control)
        {
            case TextBox textBox:
                textBox.TextAlign = context.TextAlignment;
                break;
            case DateTimePicker dateTimePicker:
                dateTimePicker.RightToLeftLayout = context.Direction == TransportPresentationDirection.RightToLeft;
                break;
            case FlowLayoutPanel flowLayoutPanel:
                flowLayoutPanel.FlowDirection = context.FlowDirection;
                break;
            case Label label:
                label.TextAlign = context.ContentAlignment;
                break;
        }

        foreach (Control child in control.Controls)
        {
            Apply(child, context);
        }
    }

    public static void SetDynamicVisibility(Control control, bool isVisible)
    {
        ArgumentNullException.ThrowIfNull(control);

        control.Visible = isVisible;
        control.Parent?.PerformLayout();
    }
}

/// <summary>
/// تتبناه المكونات المشتركة التي تحتاج الاحتفاظ بسياق العرض خلال تفاعلاتها الداخلية.
/// </summary>
public interface ITransportPresentationAware
{
    void ApplyPresentationContext(TransportPresentationContext context);
}
