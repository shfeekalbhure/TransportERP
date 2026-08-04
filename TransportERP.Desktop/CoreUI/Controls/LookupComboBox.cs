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
        DrawItem += DrawArabicItem;
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

            SelectedIndex = selectFirstItem && Items.Count > 0 ? 0 : -1;
        }
        finally
        {
            EndUpdate();
            UpdateVisualState();
            Invalidate();
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

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        RightToLeft = RightToLeft.Yes;
        Invalidate();
    }

    private void ApplyDefaultStyle()
    {
        DropDownStyle = ComboBoxStyle.DropDownList;
        DrawMode = DrawMode.OwnerDrawFixed;
        ItemHeight = 28;
        FlatStyle = FlatStyle.Flat;
        Font = UiTheme.CreateRegularFont(10.5F);
        ForeColor = UiTheme.HeadingText;
        RightToLeft = RightToLeft.Yes;
        UpdateVisualState();
    }

    /// <summary>
    /// رسم العنصر المختار وعناصر القائمة بمحاذاة عربية إلى أقصى اليمين.
    /// </summary>
    private void DrawArabicItem(object? sender, DrawItemEventArgs e)
    {
        e.DrawBackground();

        if (e.Index < 0 || e.Index >= Items.Count)
        {
            return;
        }

        var text = GetItemText(Items[e.Index]);
        var textColor = (e.State & DrawItemState.Selected) == DrawItemState.Selected
            ? SystemColors.HighlightText
            : ForeColor;
        var textBounds = Rectangle.Inflate(e.Bounds, -8, 0);

        TextRenderer.DrawText(
            e.Graphics,
            text,
            Font,
            textBounds,
            textColor,
            TextFormatFlags.Right |
            TextFormatFlags.VerticalCenter |
            TextFormatFlags.RightToLeft |
            TextFormatFlags.EndEllipsis |
            TextFormatFlags.NoPrefix);

        e.DrawFocusRectangle();
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
