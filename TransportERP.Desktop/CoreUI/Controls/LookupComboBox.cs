using System.ComponentModel;
using TransportERP.Desktop.Themes;

namespace TransportERP.Desktop.CoreUI.Controls;

/// <summary>
/// قائمة اختيار موحدة للبيانات المرجعية في نظام TransportERP.
/// تستخدم لاختيار الشركات والفروع والعملات والحسابات والعملاء والموردين وغيرها.
/// </summary>
[ToolboxItem(true)]
public sealed class LookupComboBox : ComboBox
{
    private bool _isRequired = true;
    private string _requiredMessage = "يرجى اختيار قيمة من القائمة.";

    /// <summary>
    /// إنشاء قائمة اختيار بالهوية البصرية المعتمدة للنظام.
    /// </summary>
    public LookupComboBox()
    {
        ApplyDefaultStyle();
        Enter += HandleEnter;
        Leave += HandleLeave;
        SelectedIndexChanged += (_, _) => UpdateVisualState();
    }

    /// <summary>
    /// يحدد هل اختيار قيمة من القائمة إلزامي.
    /// </summary>
    [Category("TransportERP")]
    [Description("يحدد هل يجب اختيار قيمة من القائمة قبل الحفظ أو المتابعة.")]
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
    /// رسالة التحقق العربية التي تظهر عند عدم اختيار قيمة إلزامية.
    /// </summary>
    [Category("TransportERP")]
    [Description("رسالة التحقق المعروضة عندما لا يتم اختيار قيمة من القائمة الإلزامية.")]
    [DefaultValue("يرجى اختيار قيمة من القائمة.")]
    public string RequiredMessage
    {
        get => _requiredMessage;
        set => _requiredMessage = string.IsNullOrWhiteSpace(value)
            ? "يرجى اختيار قيمة من القائمة."
            : value.Trim();
    }

    /// <summary>
    /// تعبئة القائمة بمجموعة عناصر مع تحديد العنصر الأول اختياريًا.
    /// </summary>
    /// <typeparam name="TItem">نوع العناصر المضافة إلى القائمة.</typeparam>
    /// <param name="items">العناصر المطلوب عرضها.</param>
    /// <param name="selectFirstItem">يحدد هل يتم اختيار أول عنصر تلقائيًا.</param>
    public void BindItems<TItem>(IEnumerable<TItem> items, bool selectFirstItem = true)
    {
        ArgumentNullException.ThrowIfNull(items);

        BeginUpdate();
        try
        {
            DataSource = null;
            Items.Clear();

            foreach (var item in items)
            {
                Items.Add(item);
            }

            SelectedIndex = selectFirstItem && Items.Count > 0
                ? 0
                : -1;
        }
        finally
        {
            EndUpdate();
            UpdateVisualState();
        }
    }

    /// <summary>
    /// التحقق من اختيار قيمة عندما تكون القائمة إلزامية.
    /// </summary>
    /// <param name="showMessage">يحدد هل تعرض رسالة للمستخدم عند فشل التحقق.</param>
    /// <returns>صحيح إذا كانت القائمة صالحة؛ وإلا يعيد خطأ.</returns>
    public bool ValidateSelection(bool showMessage = true)
    {
        if (!_isRequired || SelectedIndex >= 0)
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
    /// إعادة القائمة إلى حالة عدم الاختيار.
    /// </summary>
    public void ResetSelection()
    {
        SelectedIndex = -1;
        UpdateVisualState();
    }

    /// <summary>
    /// تطبيق التنسيق الافتراضي للقائمة.
    /// </summary>
    private void ApplyDefaultStyle()
    {
        DropDownStyle = ComboBoxStyle.DropDownList;
        FlatStyle = FlatStyle.Flat;
        Font = UiTheme.CreateRegularFont(10.5F);
        ForeColor = UiTheme.HeadingText;
        RightToLeft = RightToLeft.Yes;
        UpdateVisualState();
    }

    /// <summary>
    /// تمييز القائمة عند حصولها على التركيز.
    /// </summary>
    private void HandleEnter(object? sender, EventArgs e)
    {
        BackColor = UiTheme.FocusedInputBackground;
    }

    /// <summary>
    /// إعادة لون القائمة بعد مغادرتها وفق حالة الإلزام والاختيار.
    /// </summary>
    private void HandleLeave(object? sender, EventArgs e)
    {
        UpdateVisualState();
    }

    /// <summary>
    /// تحديث لون القائمة لتمييز الحقول الإلزامية وحالة الإدخال.
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
