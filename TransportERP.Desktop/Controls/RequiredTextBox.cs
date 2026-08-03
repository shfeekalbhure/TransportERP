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

    /// <summary>
    /// يحدد هل الحقل إلزامي أم اختياري.
    /// </summary>
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

    /// <summary>
    /// رسالة التحقق العربية التي تظهر عند ترك الحقل الإلزامي فارغًا.
    /// </summary>
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

    /// <summary>
    /// التحقق من أن الحقل يحتوي على قيمة عندما يكون إلزاميًا.
    /// </summary>
    /// <param name="showMessage">يحدد هل تعرض رسالة للمستخدم عند فشل التحقق.</param>
    /// <returns>صحيح إذا كان الحقل صالحًا؛ وإلا يعيد خطأ.</returns>
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

    /// <summary>
    /// إعادة الحقل إلى حالته الطبيعية وحذف القيمة الحالية.
    /// </summary>
    public void ResetField()
    {
        Clear();
        UpdateVisualState();
    }

    /// <summary>
    /// تطبيق التنسيق الافتراضي للحقل.
    /// </summary>
    private void ApplyDefaultStyle()
    {
        BorderStyle = BorderStyle.FixedSingle;
        Font = UiTheme.CreateRegularFont(10.5F);
        ForeColor = UiTheme.HeadingText;
        RightToLeft = RightToLeft.Yes;
        TextAlign = HorizontalAlignment.Right;
        UpdateVisualState();
    }

    /// <summary>
    /// تمييز الحقل النشط أثناء إدخال المستخدم للبيانات.
    /// </summary>
    private void HandleEnter(object? sender, EventArgs e)
    {
        BackColor = UiTheme.FocusedInputBackground;
    }

    /// <summary>
    /// إعادة لون الحقل بعد مغادرته بحسب كونه إلزاميًا وحالته الحالية.
    /// </summary>
    private void HandleLeave(object? sender, EventArgs e)
    {
        UpdateVisualState();
    }

    /// <summary>
    /// تحديث لون الحقل لتمييز الإلزامية وحالة الإدخال.
    /// </summary>
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
