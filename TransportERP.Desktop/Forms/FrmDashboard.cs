using System.Drawing.Drawing2D;

namespace TransportERP.Desktop;

/// <summary>
/// الشاشة الرئيسية لنظام TransportERP.
/// تعرض مؤشرات الأداء والاختصارات والرسوم وآخر المعاملات ضمن واجهة عربية RTL.
/// </summary>
public partial class FrmDashboard : Form
{
    public FrmDashboard()
    {
        InitializeComponent();
        BindCountriesScreen();
        LoadDevelopmentPreviewData();
    }

    /// <summary>
    /// ربط عنصر التهيئة العامة في القائمة الجانبية بفتح شاشة الدول الحالية.
    /// هذا الربط مؤقت إلى أن تُبنى القائمة الفرعية الكاملة للبيانات الجغرافية.
    /// </summary>
    private void BindCountriesScreen()
    {
        var generalSetupButton = FindButtonByText(this, "التهيئة العامة");
        if (generalSetupButton is null)
        {
            return;
        }

        generalSetupButton.Click -= GeneralSetupButton_Click;
        generalSetupButton.Click += GeneralSetupButton_Click;
    }

    /// <summary>
    /// البحث داخل عناصر النموذج عن زر يحتوي نصًا محددًا.
    /// </summary>
    private static Button? FindButtonByText(Control parent, string text)
    {
        foreach (Control control in parent.Controls)
        {
            if (control is Button button && button.Text.Contains(text, StringComparison.Ordinal))
            {
                return button;
            }

            var nestedButton = FindButtonByText(control, text);
            if (nestedButton is not null)
            {
                return nestedButton;
            }
        }

        return null;
    }

    /// <summary>
    /// فتح شاشة GEN-003 — الدول من القائمة الرئيسية.
    /// </summary>
    private void GeneralSetupButton_Click(object? sender, EventArgs e)
    {
        using var countriesForm = new FrmCountries();
        countriesForm.ShowDialog(this);
    }

    /// <summary>
    /// تحميل بيانات معاينة مؤقتة إلى أن يتم ربط الشاشة بخدمات النظام وواجهة API.
    /// </summary>
    private void LoadDevelopmentPreviewData()
    {
        dgvRecentTransactions.Rows.Clear();
        dgvRecentTransactions.Rows.Add("سند قبض رقم CP-000123", "25,000 ريال", "مكتمل");
        dgvRecentTransactions.Rows.Add("سند صرف رقم PV-000455", "15,750 ريال", "معتمد");
        dgvRecentTransactions.Rows.Add("قيد يومي رقم JV-000799", "8,900 ريال", "معلق");
        dgvRecentTransactions.Rows.Add("تحويل بنكي رقم TR-000321", "12,600 ريال", "مكتمل");
        dgvRecentTransactions.Rows.Add("سند قبض رقم CP-000122", "7,250 ريال", "ملغي");

        statusBar.CompanyName = "شركة النقل الرئيسية";
        statusBar.BranchName = "الرئيسي";
        statusBar.FiscalYear = "2026";
        statusBar.FinancialPeriod = "مايو - 2026";
        statusBar.CurrentUser = "أحمد محمد";
        statusBar.CurrentRole = "مدير النظام";
        statusBar.EnvironmentName = "التطوير";
        statusBar.SystemVersion = "1.0.0.0";
        statusBar.SetConnectionStatus(true, "متصل");

        pnlRevenueChart.Invalidate();
        pnlActivityChart.Invalidate();
    }

    private void btnLogout_Click(object? sender, EventArgs e)
    {
        Close();
    }

    private void QuickAction_Click(object? sender, EventArgs e)
    {
        if (sender is not Button button)
        {
            return;
        }

        MessageBox.Show(
            $"سيتم فتح شاشة: {button.Text} بعد تنفيذها وربطها.",
            "TransportERP",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }

    /// <summary>
    /// رسم مخطط الإيرادات والمصروفات لآخر ستة أشهر.
    /// </summary>
    private void pnlRevenueChart_Paint(object? sender, PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        var area = new Rectangle(54, 48, Math.Max(220, pnlRevenueChart.ClientSize.Width - 84), Math.Max(150, pnlRevenueChart.ClientSize.Height - 92));
        var months = new[] { "ديسمبر", "يناير", "فبراير", "مارس", "أبريل", "مايو" };
        var revenues = new[] { 0.62F, 0.68F, 0.73F, 0.79F, 0.86F, 0.94F };
        var expenses = new[] { 0.42F, 0.47F, 0.51F, 0.56F, 0.61F, 0.67F };

        using var gridPen = new Pen(Color.FromArgb(230, 234, 241), 1F);
        using var revenueBrush = new SolidBrush(Color.FromArgb(47, 128, 237));
        using var expenseBrush = new SolidBrush(Color.FromArgb(235, 87, 87));
        using var textBrush = new SolidBrush(Color.FromArgb(70, 78, 93));
        using var labelFont = new Font(Font.FontFamily, 8.5F);

        for (var i = 0; i <= 5; i++)
        {
            var y = area.Bottom - (area.Height * i / 5F);
            e.Graphics.DrawLine(gridPen, area.Left, y, area.Right, y);
        }

        var groupWidth = area.Width / 6F;
        for (var i = 0; i < 6; i++)
        {
            var baseX = area.Left + (i * groupWidth) + (groupWidth * 0.2F);
            var barWidth = groupWidth * 0.22F;
            var revenueHeight = area.Height * revenues[i];
            var expenseHeight = area.Height * expenses[i];
            e.Graphics.FillRectangle(revenueBrush, baseX, area.Bottom - revenueHeight, barWidth, revenueHeight);
            e.Graphics.FillRectangle(expenseBrush, baseX + barWidth + 5F, area.Bottom - expenseHeight, barWidth, expenseHeight);
            var labelSize = e.Graphics.MeasureString(months[i], labelFont);
            e.Graphics.DrawString(months[i], labelFont, textBrush, baseX + barWidth - (labelSize.Width / 2F), area.Bottom + 8F);
        }
    }

    /// <summary>
    /// رسم مخطط دائري لتوزيع الإيرادات حسب النشاط.
    /// </summary>
    private void pnlActivityChart_Paint(object? sender, PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        var size = Math.Min(pnlActivityChart.ClientSize.Width - 170, pnlActivityChart.ClientSize.Height - 70);
        size = Math.Max(140, size);

        // إنزال الرسم الدائري فقط بمقدار يقارب 1 سم (38 بكسل عند مقياس 96 DPI).
        var donut = new Rectangle(28, 86, size, size);

        var values = new[] { 35F, 25F, 20F, 10F, 10F };
        var colors = new[]
        {
            Color.FromArgb(47, 128, 237),
            Color.FromArgb(111, 207, 151),
            Color.FromArgb(242, 153, 74),
            Color.FromArgb(155, 81, 224),
            Color.FromArgb(86, 204, 200)
        };
        var startAngle = -90F;
        for (var i = 0; i < values.Length; i++)
        {
            var sweep = values[i] * 3.6F;
            using var brush = new SolidBrush(colors[i]);
            e.Graphics.FillPie(brush, donut, startAngle, sweep);
            startAngle += sweep;
        }

        var inner = Rectangle.Inflate(donut, -(int)(size * 0.28F), -(int)(size * 0.28F));
        using var centerBrush = new SolidBrush(Color.White);
        e.Graphics.FillEllipse(centerBrush, inner);
    }
}
