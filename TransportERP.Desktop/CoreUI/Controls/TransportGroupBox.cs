using System.ComponentModel;

namespace TransportERP.Desktop.CoreUI.Controls;

/// <summary>
/// حاوية موحدة لأقسام الشاشة مع لون حدود ثابت وخفيف ومقاسات مركزية.
/// نستخدمها بدل GroupBox العادي حتى تكون جميع الحواف والمسافات متطابقة في كل الشاشات.
/// </summary>
[ToolboxItem(true)]
public sealed class TransportGroupBox : GroupBox
{
    // لون الحواف المعتمد: رمادي مزرق هادئ وواضح بدون أن يكون ثقيلًا بصريًا.
    private static readonly Color BorderColor = Color.FromArgb(214, 222, 233); // #D6DEE9

    public TransportGroupBox()
    {
        SetStyle(ControlStyles.UserPaint |
                 ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.OptimizedDoubleBuffer, true);

        BackColor = Color.White;
        ForeColor = Color.FromArgb(45, 55, 72);
        Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        Padding = new Padding(
            TransportUiMetrics.GroupHorizontalPadding,
            TransportUiMetrics.GroupTopPadding,
            TransportUiMetrics.GroupHorizontalPadding,
            TransportUiMetrics.GroupBottomPadding);
        RightToLeft = RightToLeft.Yes;
        Margin = Padding.Empty;
    }

    /// <summary>
    /// يرسم عنوان الحاوية وحدودها بلون موحد مع ترك فراغ للعنوان في الحد العلوي.
    /// </summary>
    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.Clear(BackColor);

        var title = Text ?? string.Empty;
        var textSize = TextRenderer.MeasureText(title, Font);
        var borderY = Math.Max(8, textSize.Height / 2);
        var titleX = RightToLeft == RightToLeft.Yes
            ? Math.Max(8, Width - Padding.Right - textSize.Width - 10)
            : Padding.Left + 10;

        using var pen = new Pen(BorderColor, 1F);

        e.Graphics.DrawLine(pen, 0, borderY, 0, Height - 1);
        e.Graphics.DrawLine(pen, Width - 1, borderY, Width - 1, Height - 1);
        e.Graphics.DrawLine(pen, 0, Height - 1, Width - 1, Height - 1);

        var gapStart = Math.Max(0, titleX - 5);
        var gapEnd = Math.Min(Width - 1, titleX + textSize.Width + 5);
        if (gapStart > 0)
        {
            e.Graphics.DrawLine(pen, 0, borderY, gapStart, borderY);
        }
        if (gapEnd < Width - 1)
        {
            e.Graphics.DrawLine(pen, gapEnd, borderY, Width - 1, borderY);
        }

        var flags = TextFormatFlags.NoPadding | TextFormatFlags.VerticalCenter;
        if (RightToLeft == RightToLeft.Yes)
        {
            flags |= TextFormatFlags.RightToLeft;
        }

        TextRenderer.DrawText(
            e.Graphics,
            title,
            Font,
            new Rectangle(titleX, 0, textSize.Width, textSize.Height + 2),
            ForeColor,
            BackColor,
            flags);
    }
}
