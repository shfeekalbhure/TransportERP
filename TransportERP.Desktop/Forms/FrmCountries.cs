namespace TransportERP.Desktop;

/// <summary>
/// شاشة إدارة الدول GEN-003 ضمن مجموعة البيانات الجغرافية.
/// </summary>
public partial class FrmCountries : Form
{
    private bool _isHostedInsideDashboard;

    public FrmCountries()
    {
        InitializeComponent();
        LoadPreviewData();
    }

    /// <summary>
    /// تهيئة الشاشة للعمل داخل تبويب الشاشة الرئيسية بدل نافذة مستقلة.
    /// يخفي شريط الحالة الداخلي لأن Dashboard تعرض شريط حالة ثابتًا واحدًا.
    /// </summary>
    public void ConfigureForTabHosting()
    {
        if (_isHostedInsideDashboard)
        {
            return;
        }

        _isHostedInsideDashboard = true;
        TopLevel = false;
        FormBorderStyle = FormBorderStyle.None;
        Dock = DockStyle.Fill;
        WindowState = FormWindowState.Normal;
        ShowInTaskbar = false;
        ControlBox = false;
        MinimizeBox = false;
        MaximizeBox = false;

        statusBar.Visible = false;
        tblRoot.RowStyles[1].SizeType = SizeType.Absolute;
        tblRoot.RowStyles[1].Height = 0F;
        tblRoot.Padding = new Padding(14, 10, 14, 0);
    }

    private void LoadPreviewData()
    {
        dgvCountries.Rows.Clear();
        dgvCountries.Rows.Add("1", "SAU", "المملكة العربية السعودية", "Kingdom of Saudi Arabia", "SA", "966", "ريال سعودي (SAR)", "آسيا", "نشط");
        dgvCountries.Rows.Add("2", "ARE", "الإمارات العربية المتحدة", "United Arab Emirates", "AE", "971", "درهم إماراتي (AED)", "آسيا", "نشط");
        dgvCountries.Rows.Add("3", "EGY", "جمهورية مصر العربية", "Arab Republic of Egypt", "EG", "20", "جنيه مصري (EGP)", "أفريقيا", "نشط");
        dgvCountries.Rows.Add("4", "TUR", "الجمهورية التركية", "Republic of Türkiye", "TR", "90", "ليرة تركية (TRY)", "آسيا", "نشط");
        dgvCountries.Rows.Add("5", "USA", "الولايات المتحدة الأمريكية", "United States of America", "US", "1", "دولار أمريكي (USD)", "أمريكا الشمالية", "نشط");

        lblResultCount.Text = "198";
        lblCreatedAtValue.Text = "2025-06-01 10:15:22";
        lblCreatedByValue.Text = "أحمد محمد";
        lblUpdatedAtValue.Text = "2025-06-10 11:45:10";
        lblUpdatedByValue.Text = "أحمد محمد";
        lblViewCountValue.Text = "28";
        lblSaveCountValue.Text = "6";
        lblEditCountValue.Text = "5";
        lblPrintCountValue.Text = "12";
        lblLastPrintAtValue.Text = "2025-06-10 12:30:00";
        lblLastPrintByValue.Text = "أحمد محمد";

        statusBar.CompanyName = "شركة الطائر السعيد للنقل";
        statusBar.BranchName = "الرئيسي - عدن";
        statusBar.FiscalYear = "2025";
        statusBar.FinancialPeriod = "يونيو";
        statusBar.CurrentUser = "أحمد محمد";
        statusBar.CurrentRole = "مدير النظام";
        statusBar.EnvironmentName = "التطوير";
        statusBar.SystemVersion = "1.0.0.0";
        statusBar.SetConnectionStatus(true, "متصل");
    }

    private void btnNew_Click(object? sender, EventArgs e)
    {
        txtCountryCode.Clear();
        txtCountryNameAr.Clear();
        txtCountryNameEn.Clear();
        txtIsoCode.Clear();
        txtDialCode.Clear();
        txtNotes.Clear();
        txtCountryCode.Focus();
    }

    private void btnClose_Click(object? sender, EventArgs e) => Close();

    private void btnSearch_Click(object? sender, EventArgs e)
    {
        MessageBox.Show("تم تطبيق البحث على بيانات المعاينة.", "بحث الدول", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void btnResetSearch_Click(object? sender, EventArgs e)
    {
        txtSearchAll.Clear();
        txtSearchCode.Clear();
        txtSearchName.Clear();
        cboSearchStatus.SelectedIndex = 0;
    }
}
