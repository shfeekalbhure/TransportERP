using System.ComponentModel;
using TransportERP.Desktop.Controls;
using TransportERP.Desktop.Themes;

namespace TransportERP.Desktop.CoreUI.Controls;

/// <summary>
/// شريط الأوامر الموحد لجميع شاشات TransportERP.
/// الأزرار تبدأ دائمًا من أقصى اليمين وبنفس الأسماء والألوان في كل شاشة.
/// </summary>
[ToolboxItem(true)]
public sealed class TransportToolbar : UserControl
{
    private readonly FlowLayoutPanel _buttonsPanel = new();

    public PrimaryButton NewButton { get; } = CreateButton("جديد", ToolbarAction.New);
    public PrimaryButton SaveButton { get; } = CreateButton("حفظ", ToolbarAction.Save);
    public PrimaryButton EditButton { get; } = CreateButton("تعديل", ToolbarAction.Edit);
    public PrimaryButton DisableButton { get; } = CreateButton("إيقاف", ToolbarAction.Disable);
    public PrimaryButton DeleteButton { get; } = CreateButton("حذف", ToolbarAction.Delete);
    public PrimaryButton PrintButton { get; } = CreateButton("طباعة", ToolbarAction.Print);
    public PrimaryButton CloseButton { get; } = CreateButton("إغلاق", ToolbarAction.Close);

    public TransportToolbar()
    {
        InitializeLayout();
        RegisterEvents();
    }

    public event EventHandler? NewRequested;
    public event EventHandler? SaveRequested;
    public event EventHandler? EditRequested;
    public event EventHandler? DisableRequested;
    public event EventHandler? DeleteRequested;
    public event EventHandler? PrintRequested;
    public event EventHandler? CloseRequested;

    public void SetActionVisible(ToolbarAction action, bool isVisible) => GetButton(action).Visible = isVisible;
    public void SetActionEnabled(ToolbarAction action, bool isEnabled) => GetButton(action).Enabled = isEnabled;

    public void ResetActions()
    {
        foreach (Control control in _buttonsPanel.Controls)
        {
            control.Visible = true;
            control.Enabled = true;
        }
    }

    private void InitializeLayout()
    {
        AutoSize = false;
        BackColor = Color.White;
        Dock = DockStyle.Fill;
        Height = TransportUiMetrics.ToolbarHeight;
        MinimumSize = new Size(0, TransportUiMetrics.ToolbarHeight);
        Padding = new Padding(6, 4, 6, 4);
        RightToLeft = RightToLeft.Yes;

        _buttonsPanel.AutoSize = true;
        _buttonsPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        _buttonsPanel.AutoScroll = false;
        _buttonsPanel.BackColor = Color.Transparent;
        _buttonsPanel.Dock = DockStyle.Right;
        _buttonsPanel.FlowDirection = FlowDirection.RightToLeft;
        _buttonsPanel.RightToLeft = RightToLeft.Yes;
        _buttonsPanel.WrapContents = false;
        _buttonsPanel.Padding = Padding.Empty;

        // بسبب اتجاه FlowLayoutPanel من اليمين إلى اليسار، نضيف الأزرار بترتيب عكسي
        // حتى يظهر الترتيب البصري الفعلي من أقصى اليمين كما هو معتمد:
        // جديد ← حفظ ← تعديل ← إيقاف ← حذف ← طباعة ← إغلاق
        _buttonsPanel.Controls.Add(CloseButton);
        _buttonsPanel.Controls.Add(PrintButton);
        _buttonsPanel.Controls.Add(DeleteButton);
        _buttonsPanel.Controls.Add(DisableButton);
        _buttonsPanel.Controls.Add(EditButton);
        _buttonsPanel.Controls.Add(SaveButton);
        _buttonsPanel.Controls.Add(NewButton);

        Controls.Add(_buttonsPanel);
    }

    private void RegisterEvents()
    {
        NewButton.Click += (_, _) => NewRequested?.Invoke(this, EventArgs.Empty);
        SaveButton.Click += (_, _) => SaveRequested?.Invoke(this, EventArgs.Empty);
        EditButton.Click += (_, _) => EditRequested?.Invoke(this, EventArgs.Empty);
        DisableButton.Click += (_, _) => DisableRequested?.Invoke(this, EventArgs.Empty);
        DeleteButton.Click += (_, _) => DeleteRequested?.Invoke(this, EventArgs.Empty);
        PrintButton.Click += (_, _) => PrintRequested?.Invoke(this, EventArgs.Empty);
        CloseButton.Click += (_, _) => CloseRequested?.Invoke(this, EventArgs.Empty);
    }

    private static PrimaryButton CreateButton(string text, ToolbarAction action)
    {
        var (normal, hover) = action switch
        {
            ToolbarAction.Save => (UiTheme.ActionSave, UiTheme.ActionSaveHover),
            ToolbarAction.Edit => (UiTheme.ActionEdit, UiTheme.ActionEditHover),
            ToolbarAction.Disable => (UiTheme.ActionDisable, UiTheme.ActionDisableHover),
            ToolbarAction.Delete => (UiTheme.ActionDelete, UiTheme.ActionDeleteHover),
            ToolbarAction.Close => (UiTheme.ActionClose, UiTheme.ActionCloseHover),
            _ => (UiTheme.PrimaryBlue, UiTheme.PrimaryBlueHover)
        };

        return new PrimaryButton
        {
            CornerRadius = 6,
            Height = TransportUiMetrics.ToolbarButtonHeight,
            MinimumSize = new Size(70, TransportUiMetrics.ToolbarButtonHeight),
            Width = 78,
            Margin = new Padding(2, 0, 2, 0),
            Text = text,
            NormalBackColor = normal,
            HoverBackColor = hover
        };
    }

    private PrimaryButton GetButton(ToolbarAction action) => action switch
    {
        ToolbarAction.New => NewButton,
        ToolbarAction.Save => SaveButton,
        ToolbarAction.Edit => EditButton,
        ToolbarAction.Disable => DisableButton,
        ToolbarAction.Delete => DeleteButton,
        ToolbarAction.Print => PrintButton,
        ToolbarAction.Close => CloseButton,
        _ => throw new ArgumentOutOfRangeException(nameof(action), action, "نوع العملية غير مدعوم.")
    };
}

public enum ToolbarAction
{
    New,
    Save,
    Edit,
    Disable,
    Delete,
    Print,
    Close
}
