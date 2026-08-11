using System.ComponentModel;
using TransportERP.Desktop.CoreUI.Presentation;
using TransportERP.Desktop.Themes;

namespace TransportERP.Desktop.CoreUI.Controls;

/// <summary>
/// قائمة اختيار موحدة للبيانات المرجعية في نظام TransportERP.
/// تستخدم لاختيار الشركات والفروع والعملات والحسابات والعملاء والموردين وغيرها.
/// </summary>
[ToolboxItem(true)]
public sealed class LookupComboBox : ComboBox, ITransportPresentationAware
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
    /// العقد الحالي للعرض والاختيار فقط. لا يعرّف أو يستدعي مصدر البيانات.
    /// </summary>
    [Browsable(false)]
    public LookupPresentationContract? PresentationContract { get; private set; }

    [Browsable(false)]
    public LookupPresentationItem? SelectedPresentationItem => SelectedItem as LookupPresentationItem;

    [Browsable(false)]
    public LookupPresentationSelection? SelectedPresentation =>
        PresentationContract is not null && SelectedPresentationItem is not null
            ? new LookupPresentationSelection(PresentationContract, SelectedPresentationItem.Id)
            : null;

    public void BindPresentationItems(
        LookupPresentationContract contract,
        IEnumerable<LookupPresentationItem> items,
        bool selectFirstItem = true)
    {
        ArgumentNullException.ThrowIfNull(contract);
        ArgumentNullException.ThrowIfNull(items);

        PresentationContract = contract;
        BindItems(items, selectFirstItem);
    }

    public void ApplyPresentationContext(TransportPresentationContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        RightToLeft = context.RightToLeft;
    }

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

    public void ResetSelection()
    {
        SelectedIndex = -1;
        UpdateVisualState();
    }

    /// <summary>
    /// تطبيق تنسيق آمن ومتوافق مع مصمم WinForms دون رسم يدوي.
    /// </summary>
    private void ApplyDefaultStyle()
    {
        DropDownStyle = ComboBoxStyle.DropDownList;
        DrawMode = DrawMode.Normal;
        FlatStyle = FlatStyle.Flat;
        Font = UiTheme.CreateRegularFont(10.5F);
        ForeColor = UiTheme.HeadingText;
        RightToLeft = RightToLeft.Yes;
        UpdateVisualState();
    }

    private void HandleEnter(object? sender, EventArgs e)
    {
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
