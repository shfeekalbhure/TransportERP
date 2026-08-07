using System.ComponentModel;
using TransportERP.Desktop.Themes;

namespace TransportERP.Desktop.CoreUI.Controls;

/// <summary>
/// شريط الرسائل والتنبيهات الموحد داخل شاشات العمل.
/// نستخدمه بدل إنشاء Label أو Panel مختلف في كل شاشة،
/// وبذلك تصبح رسائل النجاح والتحذير والخطأ بنفس الشكل في جميع أجزاء النظام.
/// </summary>
[ToolboxItem(true)]
public sealed class TransportAlertBar : UserControl
{
    // هذا النص هو الرسالة التي يراها المستخدم داخل شريط التنبيه.
    private readonly Label _messageLabel = new();

    // هذا الزر يسمح للمستخدم بإخفاء الرسالة دون التأثير على بقية الشاشة.
    private readonly Button _closeButton = new();

    /// <summary>
    /// إنشاء شريط التنبيه وتطبيق التصميم الموحد عليه.
    /// </summary>
    public TransportAlertBar()
    {
        InitializeLayout();
        HideMessage();
    }

    /// <summary>
    /// عرض رسالة نجاح، وتستخدم بعد اكتمال عملية مثل الحفظ بنجاح.
    /// </summary>
    public void ShowSuccess(string message) => ShowMessage(message, AlertKind.Success);

    /// <summary>
    /// عرض رسالة معلومات عامة لا تمثل خطأ أو تحذيرًا.
    /// </summary>
    public void ShowInfo(string message) => ShowMessage(message, AlertKind.Info);

    /// <summary>
    /// عرض رسالة تحذير عند وجود حالة تحتاج انتباه المستخدم.
    /// </summary>
    public void ShowWarning(string message) => ShowMessage(message, AlertKind.Warning);

    /// <summary>
    /// عرض رسالة خطأ عند فشل عملية أو وجود بيانات غير صحيحة.
    /// </summary>
    public void ShowError(string message) => ShowMessage(message, AlertKind.Error);

    /// <summary>
    /// إخفاء الشريط عندما لا توجد رسالة يجب عرضها.
    /// </summary>
    public void HideMessage()
    {
        Visible = false;
        _messageLabel.Text = string.Empty;
    }

    /// <summary>
    /// تجهيز الحاوية الداخلية للشريط مرة واحدة ليتم إعادة استخدامها في جميع الشاشات.
    /// </summary>
    private void InitializeLayout()
    {
        Dock = DockStyle.Top;
        Height = 42;
        MinimumSize = new Size(0, 42);
        Padding = new Padding(10, 6, 10, 6);
        RightToLeft = RightToLeft.Yes;

        _closeButton.Dock = DockStyle.Left;
        _closeButton.FlatStyle = FlatStyle.Flat;
        _closeButton.FlatAppearance.BorderSize = 0;
        _closeButton.Font = UiTheme.CreateBoldFont(10F);
        _closeButton.Text = "×";
        _closeButton.Width = 34;
        _closeButton.Cursor = Cursors.Hand;
        _closeButton.Click += (_, _) => HideMessage();

        _messageLabel.Dock = DockStyle.Fill;
        _messageLabel.Font = UiTheme.CreateRegularFont(10F);
        _messageLabel.TextAlign = ContentAlignment.MiddleRight;
        _messageLabel.RightToLeft = RightToLeft.Yes;

        Controls.Add(_messageLabel);
        Controls.Add(_closeButton);
    }

    /// <summary>
    /// الدالة المركزية التي تضبط نص الرسالة وألوانها حسب نوع التنبيه.
    /// وجودها هنا يمنع تكرار نفس منطق الألوان في كل شاشة.
    /// </summary>
    private void ShowMessage(string message, AlertKind kind)
    {
        _messageLabel.Text = string.IsNullOrWhiteSpace(message) ? "—" : message.Trim();

        (BackColor, _messageLabel.ForeColor) = kind switch
        {
            AlertKind.Success => (Color.FromArgb(232, 247, 238), Color.FromArgb(31, 112, 70)),
            AlertKind.Warning => (Color.FromArgb(255, 247, 224), Color.FromArgb(142, 92, 12)),
            AlertKind.Error => (Color.FromArgb(253, 235, 235), Color.FromArgb(168, 45, 45)),
            _ => (Color.FromArgb(232, 241, 252), UiTheme.HeadingText)
        };

        _closeButton.BackColor = BackColor;
        _closeButton.ForeColor = _messageLabel.ForeColor;
        Visible = true;
        BringToFront();
    }
}

/// <summary>
/// أنواع الرسائل التي يدعمها شريط التنبيهات داخل الشاشة.
/// </summary>
public enum AlertKind
{
    Info,
    Success,
    Warning,
    Error
}
