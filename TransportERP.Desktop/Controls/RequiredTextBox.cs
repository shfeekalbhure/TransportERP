using System.ComponentModel;
using TransportERP.Desktop.Themes;

namespace TransportERP.Desktop.Controls;

/// <summary>
/// حقل نص موحد للبيانات الإلزامية في نظام TransportERP.
/// يميز الحقل الإلزامي بلون أصفر فاتح، ويوفر تحققًا مبسطًا ورسالة عربية قابلة للتخصيص.
/// </summary>
[ToolboxItem(true)]
public sealed class RequiredTextBox : TextBox
{
    private bool _isRequired = true;
    private string _requiredMessage = "هذا الحقل إلزامي.";

    /// <summary>
    /// إنشاء حقل نص بالهوية البصرية المعتمدة للنظام.
    /// </summary>
    public RequiredTextBox()
    {
        ApplyDefaultStyle();
        Enter += HandleEnter;
        Leave += HandleLeave;
        TextChanged += (_, _) => UpdateVisualState();
    }

    [Category("TransportERP")]
    [Description("يحدد هل يجب إدخال قيمة في الحقل قبل الحفظ أو المتابعة.")]
    [DefaultValue(true)]
    public bool IsRequired
    {
        get => _isRequired;
        set
        {
            _isRequired = value;
            UpdateVisualState();
        }
    }

    [Category("TransportERP")]
    [Description("رسالة التحقق المعروضة عندما يكون الحقل الإلزامي فارغًا.")]
    [DefaultValue("هذا الحقل إلزامي.")]
    public string RequiredMessage
    {
        get => _requiredMessage;
        set => _requiredMessage = string.IsNullOrWhiteSpace(value)
            ? "هذا الحقل إلزامي."
            : value.Trim();
    }

    public bool ValidateRequired(bool showMessage = true)
    {
        if (!_isRequired || !string.IsNullOrWhiteSpace(Text))
        {
            UpdateVisualState();
            return true;
        }

        BackColor = Color.MistyRose;

        if (showMessage)
        {
            MessageBox.Show(
                _requiredMessage,
                "تنبيه",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        Focus();
        return false;
    }

    public void ResetField()
    {
        Clear();
        UpdateVisualState();
    }

    /// <summary>
    /// إعادة فرض اتجاه ومحاذاة النص بعد إنشاء مقبض WinForms.
    /// يمنع رجوع الكتابة إلى اليسار بسبب وراثة اتجاه الحاويات.
    /// </summary>
    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        ApplyArabicAlignment();
    }

    /// <summary>
    /// إعادة تطبيق المحاذاة عند تغير اتجاه العنصر أو الحاوية الأب.
    /// </summary>
    protected override void OnRightToLeftChanged(EventArgs e)
    {
        base.OnRightToLeftChanged(e);
        ApplyArabicAlignment();
    }

    private void ApplyDefaultStyle()
    {
        BorderStyle = BorderStyle.FixedSingle;
        Font = UiTheme.CreateRegularFont(10.5F);
        ForeColor = UiTheme.HeadingText;
        ApplyArabicAlignment();
        UpdateVisualState();
    }

    /// <summary>
    /// تثبيت الكتابة العربية من اليمين ومحاذاة النص إلى اليمين.
    /// </summary>
    private void ApplyArabicAlignment()
    {
        RightToLeft = RightToLeft.Yes;
        TextAlign = HorizontalAlignment.Right;
    }

    private void HandleEnter(object? sender, EventArgs e)
    {
        ApplyArabicAlignment();
        BackColor = UiTheme.FocusedInputBackground;
    }

    private void HandleLeave(object? sender, EventArgs e)
    {
        UpdateVisualState();
    }

    private void UpdateVisualState()
    {
        if (Focused)
        {
            BackColor = UiTheme.FocusedInputBackground;
            return;
        }

        BackColor = _isRequired
            ? Color.FromArgb(255, 250, 214)
            : Color.White;
    }
}
