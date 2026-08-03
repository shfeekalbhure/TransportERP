using TransportERP.Desktop.CoreUI.Controls;
using TransportERP.Desktop.Themes;

namespace TransportERP.Desktop;

/// <summary>
/// الشاشة الرئيسية المؤقتة لنظام TransportERP.
/// تُفتح بعد الضغط على زر الدخول أثناء مرحلة التطوير، وتعرض قالبًا أوليًا
/// مبنيًا على عناصر CoreUI إلى حين تنفيذ لوحة المعلومات النهائية المعتمدة.
/// </summary>
public sealed class FrmDashboard : Form
{
    private readonly TitlePanel _titlePanel = new();
    private readonly TransportStatusBar _statusBar = new();

    /// <summary>
    /// إنشاء الشاشة الرئيسية وتهيئة هيكلها البصري الأولي.
    /// </summary>
    public FrmDashboard()
    {
        InitializeDashboard();
    }

    /// <summary>
    /// إعداد خصائص النافذة وإضافة عناصر لوحة المعلومات الأولية.
    /// </summary>
    private void InitializeDashboard()
    {
        Text = "TransportERP - الشاشة الرئيسية";
        StartPosition = FormStartPosition.CenterScreen;
        WindowState = FormWindowState.Maximized;
        MinimumSize = new Size(1100, 700);
        BackColor = UiTheme.WindowBackground;
        Font = UiTheme.CreateRegularFont(10F);
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;

        _titlePanel.Dock = DockStyle.Top;
        _titlePanel.Height = 105;
        _titlePanel.SetContent(
            "لوحة المعلومات",
            "مرحبًا بك في نظام TransportERP لإدارة النقل والخدمات اللوجستية.",
            "الرئيسية / لوحة المعلومات");

        var cardsPanel = CreateSummaryCards();
        var welcomePanel = CreateWelcomePanel();

        _statusBar.Dock = DockStyle.Bottom;
        _statusBar.Height = 38;
        _statusBar.CompanyName = "شركة النقل الرئيسية";
        _statusBar.BranchName = "الفرع الرئيسي";
        _statusBar.FiscalYear = DateTime.Today.Year.ToString();
        _statusBar.FinancialPeriod = "-";
        _statusBar.CurrentUser = "مستخدم تجريبي";
        _statusBar.CurrentRole = "مدير النظام";
        _statusBar.EnvironmentName = "بيئة التطوير";
        _statusBar.SystemVersion = "1.0.0";
        _statusBar.SetConnectionStatus(false, "لم يتم ربط API بعد");

        Controls.Add(welcomePanel);
        Controls.Add(cardsPanel);
        Controls.Add(_titlePanel);
        Controls.Add(_statusBar);
    }

    /// <summary>
    /// إنشاء بطاقات ملخص أولية لتوضيح مكان مؤشرات لوحة المعلومات المستقبلية.
    /// </summary>
    private static Control CreateSummaryCards()
    {
        var panel = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 165,
            Padding = new Padding(24, 18, 24, 12),
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = true,
            BackColor = Color.Transparent
        };

        panel.Controls.Add(CreateSummaryCard("الشركات", "1", "الشركات المفعلة"));
        panel.Controls.Add(CreateSummaryCard("الفروع", "1", "الفروع المتاحة"));
        panel.Controls.Add(CreateSummaryCard("المستخدمون", "-", "سيظهر بعد ربط الخدمة"));
        panel.Controls.Add(CreateSummaryCard("حالة النظام", "جاهز", "مرحلة تأسيس الواجهات"));

        return panel;
    }

    /// <summary>
    /// إنشاء بطاقة ملخص واحدة داخل لوحة المعلومات.
    /// </summary>
    private static Control CreateSummaryCard(string title, string value, string description)
    {
        var card = new Panel
        {
            Width = 235,
            Height = 120,
            Margin = new Padding(10),
            Padding = new Padding(18),
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle
        };

        var titleLabel = new Label
        {
            Dock = DockStyle.Top,
            Height = 28,
            Text = title,
            ForeColor = UiTheme.SecondaryText,
            Font = UiTheme.CreateRegularFont(10F),
            TextAlign = ContentAlignment.MiddleRight
        };

        var valueLabel = new Label
        {
            Dock = DockStyle.Top,
            Height = 42,
            Text = value,
            ForeColor = UiTheme.PrimaryBlue,
            Font = UiTheme.CreateBoldFont(20F),
            TextAlign = ContentAlignment.MiddleRight
        };

        var descriptionLabel = new Label
        {
            Dock = DockStyle.Fill,
            Text = description,
            ForeColor = UiTheme.SecondaryText,
            Font = UiTheme.CreateRegularFont(8.5F),
            TextAlign = ContentAlignment.MiddleRight
        };

        card.Controls.Add(descriptionLabel);
        card.Controls.Add(valueLabel);
        card.Controls.Add(titleLabel);
        return card;
    }

    /// <summary>
    /// إنشاء مساحة ترحيبية مؤقتة إلى حين تنفيذ مكونات Dashboard النهائية.
    /// </summary>
    private static Control CreateWelcomePanel()
    {
        var panel = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(34),
            BackColor = Color.Transparent
        };

        var card = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(36),
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle
        };

        var heading = new Label
        {
            Dock = DockStyle.Top,
            Height = 48,
            Text = "تم فتح الشاشة الرئيسية بنجاح",
            ForeColor = UiTheme.HeadingText,
            Font = UiTheme.CreateBoldFont(18F),
            TextAlign = ContentAlignment.MiddleRight
        };

        var message = new Label
        {
            Dock = DockStyle.Top,
            Height = 70,
            Text = "هذه معاينة تشغيلية أولية. سيتم لاحقًا تنفيذ القوائم الجانبية، مؤشرات الأداء، التنبيهات والاختصارات وفق التصميم المعتمد.",
            ForeColor = UiTheme.SecondaryText,
            Font = UiTheme.CreateRegularFont(11F),
            TextAlign = ContentAlignment.TopRight
        };

        card.Controls.Add(message);
        card.Controls.Add(heading);
        panel.Controls.Add(card);
        return panel;
    }
}
