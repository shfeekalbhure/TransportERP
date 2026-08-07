using System.ComponentModel;

namespace TransportERP.Desktop.CoreUI.Controls;

/// <summary>
/// حاوية موحدة للأوامر الخاصة بكل شاشة مثل: إضافة عضو، إزالة عضو، طلب موافقة أو إلغاء جلسة.
/// الهدف أن لا توضع الأزرار المتخصصة مباشرة على الشاشة أو داخل التبويب بصورة حرة.
/// </summary>
[ToolboxItem(true)]
public sealed class TransportActionPanel : FlowLayoutPanel
{
    public TransportActionPanel()
    {
        Dock = DockStyle.Top;
        Height = TransportUiMetrics.ActionPanelHeight;
        FlowDirection = FlowDirection.RightToLeft;
        RightToLeft = RightToLeft.Yes;
        WrapContents = false;
        AutoScroll = true;
        Padding = new Padding(TransportUiMetrics.CompactPadding);
        Margin = Padding.Empty;
        BackColor = Color.White;
    }

    /// <summary>
    /// يضيف زر إجراء متخصص بحجم موحد ويضعه داخل الحاوية من جهة اليمين.
    /// </summary>
    public Button AddAction(string text, EventHandler? clickHandler = null)
    {
        var button = new Button
        {
            AutoSize = false,
            Height = TransportUiMetrics.ActionButtonHeight,
            MinimumSize = new Size(TransportUiMetrics.ActionButtonMinWidth, TransportUiMetrics.ActionButtonHeight),
            Text = text,
            RightToLeft = RightToLeft.Yes,
            FlatStyle = FlatStyle.System,
            Margin = new Padding(TransportUiMetrics.ActionButtonGap, 0, 0, 0),
            Tag = text
        };

        if (clickHandler is not null)
        {
            button.Click += clickHandler;
        }

        Controls.Add(button);
        return button;
    }
}
