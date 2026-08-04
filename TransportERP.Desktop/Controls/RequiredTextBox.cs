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
    private bool _isApplyingArabicAlignment;

    /// <summary>
    /// إنشاء حقل نص بالهوية البصرية المعتمدة للنظام.
    /// </summary>
    public RequiredTextBox()
    {
        ApplyDefaultStyle();
        Enter += HandleEnter;
        Leave += HandleLeave;
        TextChanged += (_, _) =>
        {
            ApplyArabicAlignment();
            UpdateVisualState();
        };
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
    /// إعادة فرض الاتجاه والمحاذاة بعد إنشاء مقبض WinForms.
    /// </summary>
    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        ApplyArabicAlignment();

        if (IsHandleCreated)
        {
            BeginInvoke(ApplyArabicAlignment);
        }
    }

    /// <summary>
    /// تثبيت المحاذاة عند إضافة الحقل إلى أي حاوية.
    /// </summary>
    protected override void OnParentChanged(EventArgs e)
    {
        base.OnParentChanged(e);
        ApplyArabicAlignment();
    }

    /// <summary>
    /// تثبيت المحاذاة عند تغير الخط أو القياس.
    /// </summary>
    protected override void OnFontChanged(EventArgs e)
    {
        base.OnFontChanged(e);
        ApplyArabicAlignment();
    }

    /// <summary>
    /// منع المصمم أو الحاوية من إعادة النص إلى اليسار.
    /// </summary>
    protected override void OnTextAlignChanged(EventArgs e)
    {
        base.OnTextAlignChanged(e);

        if (!_isApplyingArabicAlignment && TextAlign != HorizontalAlignment.Right)
        {
            ApplyArabicAlignment();
        }
    }

    protected override void OnRightToLeftChanged(EventArgs e)
    {
        base.OnRightToLeftChanged(e);

        if (!_isApplyingArabicAlignment && RightToLeft != RightToLeft.Yes)
        {
            ApplyArabicAlignment();
        }
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
    /// فرض الكتابة العربية ومحاذاة النص إلى أقصى اليمين.
    /// </summary>
    private void ApplyArabicAlignment()
    {
        if (_isApplyingArabicAlignment)
        {
            return;
        }

        try
        {
            _isApplyingArabicAlignment = true;
            RightToLeft = RightToLeft.Yes;
            TextAlign = HorizontalAlignment.Right;
        }
        finally
        {
            _isApplyingArabicAlignment = false;
        }
    }

    private void HandleEnter(object? sender, EventArgs e)
    {
        ApplyArabicAlignment();
        BackColor = UiTheme.FocusedInputBackground;
    }

    private void HandleLeave(object? sender, EventArgs e)
    {
        ApplyArabicAlignment();
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
