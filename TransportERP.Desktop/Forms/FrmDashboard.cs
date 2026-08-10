using System.Drawing.Drawing2D;

namespace TransportERP.Desktop;

/// <summary>
/// الشاشة الرئيسية الأساسية لنظام TransportERP.
/// لا تحتوي هذه النسخة على أي استدعاء لشاشات فرعية.
/// </summary>
public partial class FrmDashboard : Form
{
    public FrmDashboard()
    {
        InitializeComponent();
        LoadDevelopmentPreviewData();
    }

    private void LoadDevelopmentPreviewData()
    {
        dgvRecentTransactions.Rows.Clear();
        dgvRecentTransactions.Rows.Add(
            "لا توجد معاملات بعد",
            "—",
            "—");

        statusBar.UpdateContext(
            "الشركة الحالية",
            "الفرع الحالي",
            DateTime.Today.Year.ToString(),
            "الفترة الحالية",
            "المستخدم الحالي",
            "الدور الحالي",
            "TransportERP",
            "1.0.0",
            false);
    }

    private void btnLogout_Click(object? sender, EventArgs e)
    {
        var answer = MessageBox.Show(
            "هل تريد تسجيل الخروج؟",
            "تسجيل الخروج",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button2,
            MessageBoxOptions.RtlReading |
            MessageBoxOptions.RightAlign);

        if (answer == DialogResult.Yes)
        {
            Close();
        }
    }

    private void QuickAction_Click(object? sender, EventArgs e)
    {
        if (sender is not Button button)
        {
            return;
        }

        MessageBox.Show(
            $"الاختصار «{button.Text.Replace("\r\n", " ", StringComparison.Ordinal)}» غير مفعل حاليًا.",
            "TransportERP",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information,
            MessageBoxDefaultButton.Button1,
            MessageBoxOptions.RtlReading |
            MessageBoxOptions.RightAlign);
    }

    private void pnlRevenueChart_Paint(
        object? sender,
        PaintEventArgs e)
    {
        DrawPreviewLineChart(
            e.Graphics,
            pnlRevenueChart.ClientRectangle);
    }

    private void pnlActivityChart_Paint(
        object? sender,
        PaintEventArgs e)
    {
        DrawPreviewActivityChart(
            e.Graphics,
            pnlActivityChart.ClientRectangle);
    }

    private static void DrawPreviewLineChart(
        Graphics graphics,
        Rectangle bounds)
    {
        if (bounds.Width < 120 || bounds.Height < 100)
        {
            return;
        }

        graphics.SmoothingMode = SmoothingMode.AntiAlias;

        var chart = Rectangle.Inflate(
            bounds,
            -28,
            -58);

        if (chart.Width <= 0 || chart.Height <= 0)
        {
            return;
        }

        using var axisPen =
            new Pen(
                Color.FromArgb(220, 225, 232),
                1F);

        graphics.DrawLine(
            axisPen,
            chart.Left,
            chart.Bottom,
            chart.Right,
            chart.Bottom);

        graphics.DrawLine(
            axisPen,
            chart.Left,
            chart.Top,
            chart.Left,
            chart.Bottom);

        var points = new[]
        {
            new PointF(
                chart.Left,
                chart.Bottom - chart.Height * 0.25F),

            new PointF(
                chart.Left + chart.Width * 0.2F,
                chart.Bottom - chart.Height * 0.45F),

            new PointF(
                chart.Left + chart.Width * 0.4F,
                chart.Bottom - chart.Height * 0.36F),

            new PointF(
                chart.Left + chart.Width * 0.6F,
                chart.Bottom - chart.Height * 0.62F),

            new PointF(
                chart.Left + chart.Width * 0.8F,
                chart.Bottom - chart.Height * 0.55F),

            new PointF(
                chart.Right,
                chart.Bottom - chart.Height * 0.72F)
        };

        using var linePen =
            new Pen(
                Color.FromArgb(47, 128, 237),
                2.5F);

        graphics.DrawLines(
            linePen,
            points);
    }

    private static void DrawPreviewActivityChart(
        Graphics graphics,
        Rectangle bounds)
    {
        if (bounds.Width < 120 || bounds.Height < 100)
        {
            return;
        }

        graphics.SmoothingMode =
            SmoothingMode.AntiAlias;

        var area = Rectangle.Inflate(
            bounds,
            -34,
            -62);

        if (area.Width <= 0 || area.Height <= 0)
        {
            return;
        }

        var values =
            new[] { 0.72F, 0.54F, 0.38F, 0.63F };

        var gap = 12;

        var barWidth = Math.Max(
            14,
            (area.Width - gap * (values.Length - 1))
            / values.Length);

        using var brush =
            new SolidBrush(
                Color.FromArgb(86, 204, 200));

        for (var i = 0; i < values.Length; i++)
        {
            var height =
                (int)(area.Height * values[i]);

            var x =
                area.Left +
                i * (barWidth + gap);

            graphics.FillRectangle(
                brush,
                new Rectangle(
                    x,
                    area.Bottom - height,
                    barWidth,
                    height));
        }
    }
}