using System.ComponentModel;
using TransportERP.Desktop.Controls;
using TransportERP.Desktop.Themes;

namespace TransportERP.Desktop.CoreUI.Controls;

/// <summary>
/// شريط الأدوات الموحد لشاشات TransportERP.
/// يرتب أزرار العمليات من اليمين إلى اليسار وفق القرار المعتمد،
/// ويوفر أحداثًا مستقلة لكل عملية رئيسية في الشاشة.
/// </summary>
[ToolboxItem(true)]
public sealed class TransportToolbar : UserControl
{
    // الحاوية الداخلية التي تجمع الأزرار في صف واحد من اليمين إلى اليسار.
    private readonly FlowLayoutPanel _buttonsPanel = new();

    /// <summary>زر إنشاء سجل جديد.</summary>
    public PrimaryButton NewButton { get; } = CreateButton("جديد");

    /// <summary>زر حفظ السجل الحالي.</summary>
    public PrimaryButton SaveButton { get; } = CreateButton("حفظ");

    /// <summary>زر تعديل السجل الحالي.</summary>
    public PrimaryButton EditButton { get; } = CreateButton("تعديل");

    /// <summary>زر إيقاف السجل الحالي.</summary>
    public PrimaryButton DisableButton { get; } = CreateButton("إيقاف");

    /// <summary>زر حذف السجل الحالي.</summary>
    public PrimaryButton DeleteButton { get; } = CreateButton("حذف");

    /// <summary>زر طباعة بيانات الشاشة.</summary>
    public PrimaryButton PrintButton { get; } = CreateButton("طباعة");

    /// <summary>زر إغلاق الشاشة.</summary>
    public PrimaryButton CloseButton { get; } = CreateButton("إغلاق");

    /// <summary>
    /// إنشاء شريط الأدوات وترتيب الأزرار وفق التسلسل المعتمد.
    /// </summary>
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

    /// <summary>إظهار أو إخفاء زر محدد داخل شريط الأدوات.</summary>
    public void SetActionVisible(ToolbarAction action, bool isVisible) =>
        GetButton(action).Visible = isVisible;

    /// <summary>تمكين أو تعطيل زر محدد داخل شريط الأدوات.</summary>
    public void SetActionEnabled(ToolbarAction action, bool isEnabled) =>
        GetButton(action).Enabled = isEnabled;

    /// <summary>إعادة جميع الأزرار إلى الحالة المرئية والمفعلة.</summary>
    public void ResetActions()
    {
        foreach (Control control in _buttonsPanel.Controls)
        {
            control.Visible = true;
            control.Enabled = true;
        }
    }

    /// <summary>
    /// تهيئة الحاوية بارتفاع 12 مم تقريبًا، بينما ارتفاع كل زر 9 مم تقريبًا.
    /// بهذه الطريقة تبقى الأزرار بنفس المقاس في جميع الشاشات.
    /// </summary>
    private void InitializeLayout()
    {
        AutoSize = false;
        BackColor = Color.White;
        Dock = DockStyle.Fill;
        Height = TransportUiMetrics.Container12Mm;
        MinimumSize = new Size(0, TransportUiMetrics.Container12Mm);
        Padding = new Padding(8, 5, 8, 5);
        RightToLeft = RightToLeft.Yes;

        _buttonsPanel.AutoScroll = true;
        _buttonsPanel.BackColor = Color.Transparent;
        _buttonsPanel.Dock = DockStyle.Fill;
        _buttonsPanel.FlowDirection = FlowDirection.RightToLeft;
        _buttonsPanel.WrapContents = false;
        _buttonsPanel.Padding = Padding.Empty;

        // الترتيب المعتمد من أقصى اليمين: جديد ← حفظ ← تعديل ← إيقاف ← حذف ← طباعة ← إغلاق.
        _buttonsPanel.Controls.Add(NewButton);
        _buttonsPanel.Controls.Add(SaveButton);
        _buttonsPanel.Controls.Add(EditButton);
        _buttonsPanel.Controls.Add(DisableButton);
        _buttonsPanel.Controls.Add(DeleteButton);
        _buttonsPanel.Controls.Add(PrintButton);
        _buttonsPanel.Controls.Add(CloseButton);

        Controls.Add(_buttonsPanel);
    }

    /// <summary>
    /// ربط نقر كل زر بحدث عام، حتى لا نكرر نفس التوصيلات داخل كل شاشة.
    /// </summary>
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

    /// <summary>
    /// إنشاء زر موحد بارتفاع 9 مم تقريبًا.
    /// العرض يبقى كافيًا للنص العربي مع الحفاظ على شكل مضغوط.
    /// </summary>
    private static PrimaryButton CreateButton(string text)
    {
        return new PrimaryButton
        {
            CornerRadius = 9,
            Height = TransportUiMetrics.Control9Mm,
            Margin = new Padding(4, 0, 4, 0),
            MinimumSize = new Size(88, TransportUiMetrics.Control9Mm),
            Text = text,
            Width = 94
        };
    }

    /// <summary>الحصول على الزر المرتبط بنوع العملية المحدد.</summary>
    private PrimaryButton GetButton(ToolbarAction action)
    {
        return action switch
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
}

/// <summary>العمليات القياسية المتاحة داخل شريط أدوات TransportERP.</summary>
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
