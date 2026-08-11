using TransportERP.Desktop.CoreUI.Controls;

namespace TransportERP.Desktop.CoreUI.Presentation;

/// <summary>
/// رسالة تحقق للعرض. MessageKey مفتاح ترجمة فقط، ولا يحمل هذا العقد نصًا أو قاعدة عمل.
/// </summary>
public sealed record InputValidationPresentation(string FieldKey, TransportFieldVisualState VisualState, string? MessageKey = null)
{
    public void EnsureComplete()
    {
        if (string.IsNullOrWhiteSpace(FieldKey))
        {
            throw new ArgumentException("A field key is required for validation presentation.", nameof(FieldKey));
        }

        if (VisualState is not TransportFieldVisualState.Warning and not TransportFieldVisualState.Error)
        {
            throw new ArgumentException(
                "Validation presentation must use Warning or Error state.",
                nameof(VisualState));
        }
    }
}

/// <summary>
/// يربط نتيجة التحقق بالمظهر المركزي فقط؛ لا ينفذ validation ولا يعرض رسالة أعمال.
/// </summary>
public static class InputValidationPresenter
{
    public static void Apply(
        Control control,
        InputValidationPresentation presentation,
        TransportPresentationContext context)
    {
        ArgumentNullException.ThrowIfNull(control);
        ArgumentNullException.ThrowIfNull(presentation);
        ArgumentNullException.ThrowIfNull(context);
        presentation.EnsureComplete();

        TransportFieldState.Apply(control, presentation.VisualState);
        TransportPresentationPolicy.Apply(control, context);
    }
}
