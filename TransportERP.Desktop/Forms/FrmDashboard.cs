using TransportERP.Desktop.Themes;

namespace TransportERP.Desktop;

/// <summary>
/// الشاشة الرئيسية لنظام TransportERP.
/// تعرض مؤشرات الأداء والاختصارات وآخر العمليات والتنبيهات ضمن واجهة عربية RTL.
/// </summary>
public partial class FrmDashboard : Form
{
    /// <summary>
    /// إنشاء الشاشة الرئيسية وتهيئة مكوناتها المصممة بصريًا.
    /// </summary>
    public FrmDashboard()
    {
        InitializeComponent();
        ApplyApprovedReferenceLayout();
        LoadDevelopmentPreviewData();
    }

    /// <summary>
    /// تطبيق توزيع الصورة المرجعية المعتمدة:
    /// حاوية النظام في يمين الشاشة وشريط بحث أعلى مساحة العمل.
    /// </summary>
    private void ApplyApprovedReferenceLayout()
    {
        // عدم عكس أعمدة الحاوية الجذرية؛ العمود الأخير يبقى فعليًا في يمين النافذة.
        tblRoot.RightToLeft = RightToLeft.No;
        pnlSidebar.RightToLeft = RightToLeft.Yes;
        tblMain.RightToLeft = RightToLeft.Yes;

        // منع إضافة شريط البحث مرتين عند إعادة فتح المصمم أو إعادة تهيئة النموذج.
        if (tblMain.Controls.ContainsKey("pnlGlobalSearch"))
        {
            return;
        }

        tblMain.SuspendLayout();

        // إضافة صف جديد أعلى جميع حاويات Dashboard.
        tblMain.RowCount += 1;
        tblMain.RowStyles.Insert(0, new RowStyle(SizeType.Absolute, 64F));

        // تحريك العناصر الحالية صفًا واحدًا إلى الأسفل.
        foreach (Control control in tblMain.Controls)
        {
            var currentRow = tblMain.GetRow(control);
            tblMain.SetRow(control, currentRow + 1);
        }

        var searchContainer = new Panel
        {
            Name = "pnlGlobalSearch",
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Margin = new Padding(0, 0, 0, 12),
            Padding = new Padding(18, 10, 18, 10),
            RightToLeft = RightToLeft.Yes
        };

        var searchLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 3,
            RowCount = 1,
            BackColor = Color.Transparent,
            Margin = Padding.Empty,
            RightToLeft = RightToLeft.Yes
        };
        searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 42F));
        searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 230F));

        var searchIcon = new Label
        {
            Dock = DockStyle.Fill,
            Text = "⌕",
            ForeColor = UiTheme.PrimaryBlue,
            Font = UiTheme.CreateBoldFont(18F),
            TextAlign = ContentAlignment.MiddleCenter
        };

        var searchTextBox = new TextBox
        {
            Name = "txtGlobalSearch",
            Dock = DockStyle.Fill,
            BorderStyle = BorderStyle.FixedSingle,
            BackColor = Color.FromArgb(248, 250, 253),
            ForeColor = UiTheme.HeadingText,
            Font = UiTheme.CreateRegularFont(11F),
            RightToLeft = RightToLeft.Yes,
            TextAlign = HorizontalAlignment.Left,
            PlaceholderText = "ابحث في الشاشات، العمليات، العملاء، الحسابات...",
            Margin = new Padding(0, 3, 12, 3)
        };

        var systemContainerTitle = new Label
        {
            Dock = DockStyle.Fill,
            Text = "حاوية النظام",
            ForeColor = UiTheme.HeadingText,
            Font = UiTheme.CreateBoldFont(12F),
            TextAlign = ContentAlignment.MiddleRight,
            Padding = new Padding(8, 0, 8, 0)
        };

        searchLayout.Controls.Add(searchIcon, 0, 0);
        searchLayout.Controls.Add(searchTextBox, 1, 0);
        searchLayout.Controls.Add(systemContainerTitle, 2, 0);
        searchContainer.Controls.Add(searchLayout);

        tblMain.Controls.Add(searchContainer, 0, 0);
        tblMain.ResumeLayout(true);
    }

    /// <summary>
    /// تحميل بيانات معاينة مؤقتة أثناء مرحلة تطوير الواجهات فقط.
    /// تُستبدل لاحقًا ببيانات الخدمات وواجهة API.
    /// </summary>
    private void LoadDevelopmentPreviewData()
    {
        dgvRecentOperations.Rows.Clear();
        dgvRecentOperations.Rows.Add("سند قبض", "RV-000125", "شركة النقل الرئيسية", "125,000", "معلق");
        dgvRecentOperations.Rows.Add("قيد يومي", "JV-000084", "الفرع الرئيسي", "75,500", "معتمد");
        dgvRecentOperations.Rows.Add("سند صرف", "PV-000041", "الصندوق الرئيسي", "32,000", "مراجع");
        dgvRecentOperations.Rows.Add("تحديث مستخدم", "USR-000012", "إدارة النظام", "—", "مكتمل");

        statusBar.CompanyName = "شركة النقل الرئيسية";
        statusBar.BranchName = "الفرع الرئيسي";
        statusBar.FiscalYear = DateTime.Today.Year.ToString();
        statusBar.FinancialPeriod = "الفترة الحالية";
        statusBar.CurrentUser = "مستخدم تجريبي";
        statusBar.CurrentRole = "مدير النظام";
        statusBar.EnvironmentName = "بيئة التطوير";
        statusBar.SystemVersion = "1.0.0";
        statusBar.SetConnectionStatus(false, "لم يتم ربط API بعد");
    }

    /// <summary>
    /// إغلاق الشاشة الرئيسية والعودة إلى شاشة الدخول في وضع التطوير.
    /// </summary>
    private void btnLogout_Click(object? sender, EventArgs e)
    {
        Close();
    }

    /// <summary>
    /// عرض رسالة مؤقتة عند الضغط على اختصار لم تُربط شاشته بعد.
    /// </summary>
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
}
