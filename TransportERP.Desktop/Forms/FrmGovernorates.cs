using TransportERP.Desktop.Controls;

namespace TransportERP.Desktop;

/// <summary>
/// شاشة تجريبية لـ GEN-004 — المحافظات لاختبار الرأس المشترك فقط.
/// لا تتضمن بعد البيانات الأساسية أو البحث أو الجدول أو التذييل.
/// </summary>
public sealed class FrmGovernorates : Form
{
    public FrmGovernorates()
    {
        Text = "TransportERP - المحافظات";
        BackColor = Color.FromArgb(244, 247, 251);
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        WindowState = FormWindowState.Maximized;
        MinimumSize = new Size(1280, 800);

        var header = new ScreenHeaderControl
        {
            ScreenTitle = "المحافظات",
            Breadcrumb = "التهيئة العامة  ‹  البيانات الجغرافية  ‹  المحافظات",
            RecordPosition = "0 / 0"
        };

        var workspace = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(244, 247, 251),
            Padding = new Padding(14)
        };

        var notice = new Label
        {
            Dock = DockStyle.Top,
            Height = 46,
            Text = "منطقة مؤقتة لاختبار رأس الشاشة المشترك فقط",
            Font = new Font("Segoe UI", 11F, FontStyle.Regular),
            ForeColor = Color.FromArgb(100, 110, 125),
            TextAlign = ContentAlignment.MiddleRight,
            BackColor = Color.White,
            Padding = new Padding(12, 0, 12, 0)
        };

        workspace.Controls.Add(notice);
        Controls.Add(workspace);
        Controls.Add(header);
    }
}
