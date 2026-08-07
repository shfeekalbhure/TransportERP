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
    // النص الذي يراه المستخدم داخل شريط التنبيه.
    private readonly Label _messageLabel = new();

    // زر إغلاق التنبيه دون التأثير على بقية الشاشة.
    private readonly Button _closeButton = new();

    /// <summary>
    /// إنشاء شريط التنبيه وتطبيق التصميم الموحد عليه.
    /// </summary>
    public TransportAlertBar()
    {
        InitializeLayout();
        HideMessage();
    }

    public void ShowSuccess(string message) => ShowMessage(message, AlertKind.Success);
    public void ShowInfo(string message) => ShowMessage(message, AlertKind.Info);
    public void ShowWarning(string message) => ShowMessage(message, AlertKind.Warning);
    public void ShowError(string message) => ShowMessage(message, AlertKind.Error);

    /// <summary>
    /// إخفاء الشريط عندما لا توجد رسالة، بحيث لا يحجز مساحة من الجدول أو بقية الشاشة.
    /// </summary>
    public void HideMessage()
    {
        Visible = false;
        _messageLabel.Text = string.Empty;
    }

    /// <summary>
    /// الحاوية ارتفاعها 12 مم تقريبًا، وزر الإغلاق والمحتوى بارتفاع 9 مم تقريبًا.
    /// </summary>
    private void InitializeLayout()
    {
        Dock = DockStyle.Fill;
        Height = TransportUiMetrics.Container12Mm;
        MinimumSize = new Size(0, TransportUiMetrics.Container12Mm);
        Padding = new Padding(8, 5, 8, 5);
        RightToLeft = RightToLeft.Yes;

        _closeButton.Dock = DockStyle.Left;
        _closeButton.FlatStyle = FlatStyle.Flat;
        _closeButton.FlatAppearance.BorderSize = 0;
        _closeButton.Font = UiTheme.CreateBoldFont(9.5F);
        _closeButton.Text = "×";
        _closeButton.Width = TransportUiMetrics.Control9Mm;
        _closeButton.Height = TransportUiMetrics.Control9Mm;
        _closeButton.Cursor = Cursors.Hand;
        _closeButton.Click += (_, _) => HideMessage();

        _messageLabel.Dock = DockStyle.Fill;
        _messageLabel.Font = UiTheme.CreateRegularFont(9.5F);
        _messageLabel.MinimumSize = new Size(0, TransportUiMetrics.Control9Mm);
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

/// <summary>أنواع الرسائل التي يدعمها شريط التنبيهات داخل الشاشة.</summary>
public enum AlertKind
{
    Info,
    Success,
    Warning,
    Error
}
