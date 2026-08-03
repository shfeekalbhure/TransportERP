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
    private readonly FlowLayoutPanel _buttonsPanel = new();

    /// <summary>
    /// زر إنشاء سجل جديد.
    /// </summary>
    public PrimaryButton NewButton { get; } = CreateButton("جديد");

    /// <summary>
    /// زر حفظ السجل الحالي.
    /// </summary>
    public PrimaryButton SaveButton { get; } = CreateButton("حفظ");

    /// <summary>
    /// زر تعديل السجل الحالي.
    /// </summary>
    public PrimaryButton EditButton { get; } = CreateButton("تعديل");

    /// <summary>
    /// زر إيقاف السجل الحالي.
    /// </summary>
    public PrimaryButton DisableButton { get; } = CreateButton("إيقاف");

    /// <summary>
    /// زر حذف السجل الحالي.
    /// </summary>
    public PrimaryButton DeleteButton { get; } = CreateButton("حذف");

    /// <summary>
    /// زر طباعة بيانات الشاشة.
    /// </summary>
    public PrimaryButton PrintButton { get; } = CreateButton("طباعة");

    /// <summary>
    /// زر إغلاق الشاشة.
    /// </summary>
    public PrimaryButton CloseButton { get; } = CreateButton("إغلاق");

    /// <summary>
    /// إنشاء شريط الأدوات وترتيب الأزرار وفق التسلسل المعتمد.
    /// </summary>
    public TransportToolbar()
    {
        InitializeLayout();
        RegisterEvents();
    }

    /// <summary>
    /// يحدث عند طلب إنشاء سجل جديد.
    /// </summary>
    public event EventHandler? NewRequested;

    /// <summary>
    /// يحدث عند طلب حفظ السجل الحالي.
    /// </summary>
    public event EventHandler? SaveRequested;

    /// <summary>
    /// يحدث عند طلب تعديل السجل الحالي.
    /// </summary>
    public event EventHandler? EditRequested;

    /// <summary>
    /// يحدث عند طلب إيقاف السجل الحالي.
    /// </summary>
    public event EventHandler? DisableRequested;

    /// <summary>
    /// يحدث عند طلب حذف السجل الحالي.
    /// </summary>
    public event EventHandler? DeleteRequested;

    /// <summary>
    /// يحدث عند طلب طباعة بيانات الشاشة.
    /// </summary>
    public event EventHandler? PrintRequested;

    /// <summary>
    /// يحدث عند طلب إغلاق الشاشة.
    /// </summary>
    public event EventHandler? CloseRequested;

    /// <summary>
    /// إظهار أو إخفاء زر محدد داخل شريط الأدوات.
    /// </summary>
    /// <param name="action">نوع العملية المطلوب التحكم بها.</param>
    /// <param name="isVisible">صحيح لإظهار الزر، وخطأ لإخفائه.</param>
    public void SetActionVisible(ToolbarAction action, bool isVisible)
    {
        GetButton(action).Visible = isVisible;
    }

    /// <summary>
    /// تمكين أو تعطيل زر محدد داخل شريط الأدوات.
    /// </summary>
    /// <param name="action">نوع العملية المطلوب التحكم بها.</param>
    /// <param name="isEnabled">صحيح لتمكين الزر، وخطأ لتعطيله.</param>
    public void SetActionEnabled(ToolbarAction action, bool isEnabled)
    {
        GetButton(action).Enabled = isEnabled;
    }

    /// <summary>
    /// إعادة جميع الأزرار إلى الحالة المرئية والمفعلة.
    /// </summary>
    public void ResetActions()
    {
        foreach (Control control in _buttonsPanel.Controls)
        {
            control.Visible = true;
            control.Enabled = true;
        }
    }

    /// <summary>
    /// تهيئة بنية شريط الأدوات وترتيب الأزرار من اليمين.
    /// </summary>
    private void InitializeLayout()
    {
        AutoSize = false;
        BackColor = Color.White;
        Dock = DockStyle.Top;
        Height = 62;
        MinimumSize = new Size(0, 62);
        Padding = new Padding(12, 8, 12, 8);
        RightToLeft = RightToLeft.Yes;

        _buttonsPanel.AutoScroll = true;
        _buttonsPanel.BackColor = Color.Transparent;
        _buttonsPanel.Dock = DockStyle.Fill;
        _buttonsPanel.FlowDirection = FlowDirection.RightToLeft;
        _buttonsPanel.WrapContents = false;
        _buttonsPanel.Padding = new Padding(0);

        // الترتيب المعتمد: جديد ← حفظ ← تعديل ← إيقاف ← حذف ← طباعة ← إغلاق.
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
    /// تسجيل أحداث النقر وربطها بأحداث الشريط العامة.
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
    /// إنشاء زر موحد للاستخدام داخل شريط الأدوات.
    /// </summary>
    /// <param name="text">النص العربي المعروض على الزر.</param>
    private static PrimaryButton CreateButton(string text)
    {
        return new PrimaryButton
        {
            CornerRadius = 10,
            Height = 42,
            Margin = new Padding(5, 0, 5, 0),
            MinimumSize = new Size(92, 42),
            Text = text,
            Width = 100
        };
    }

    /// <summary>
    /// الحصول على الزر المرتبط بنوع العملية المحدد.
    /// </summary>
    /// <param name="action">نوع العملية المطلوبة.</param>
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

/// <summary>
/// العمليات القياسية المتاحة داخل شريط أدوات TransportERP.
/// </summary>
public enum ToolbarAction
{
    /// <summary>إنشاء سجل جديد.</summary>
    New,

    /// <summary>حفظ السجل الحالي.</summary>
    Save,

    /// <summary>تعديل السجل الحالي.</summary>
    Edit,

    /// <summary>إيقاف السجل الحالي.</summary>
    Disable,

    /// <summary>حذف السجل الحالي.</summary>
    Delete,

    /// <summary>طباعة بيانات الشاشة.</summary>
    Print,

    /// <summary>إغلاق الشاشة.</summary>
    Close
}
