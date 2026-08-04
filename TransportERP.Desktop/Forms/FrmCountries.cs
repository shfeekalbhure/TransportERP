namespace TransportERP.Desktop;

/// <summary>
/// شاشة إدارة الدول GEN-003 ضمن مجموعة البيانات الجغرافية.
/// </summary>
public partial class FrmCountries : Form
{
    private bool _isHostedInsideDashboard;
    private Label? _lblCurrentRecord;

    public FrmCountries()
    {
        InitializeComponent();
        ConfigureApprovedLayout();
        LoadPreviewData();
    }

    /// <summary>
    /// تطبيق القالب البصري المعتمد لشاشة الدول.
    /// </summary>
    private void ConfigureApprovedLayout()
    {
        // الترتيب المعتمد: العنوان، حاوية الأزرار، البيانات الرئيسية، البحث، الجدول، التدقيق.
        tblContent.SuspendLayout();
        tblContent.Controls.Remove(pnlForm);
        tblContent.Controls.Remove(pnlActions);

        tblContent.RowStyles[0].SizeType = SizeType.Absolute;
        tblContent.RowStyles[0].Height = 92F;
        tblContent.RowStyles[1].SizeType = SizeType.Absolute;
        tblContent.RowStyles[1].Height = 66F;
        tblContent.RowStyles[2].SizeType = SizeType.Absolute;
        tblContent.RowStyles[2].Height = 250F;

        tblContent.Controls.Add(pnlActions, 0, 1);
        tblContent.Controls.Add(pnlForm, 0, 2);
        tblContent.ResumeLayout(true);

        ConfigureHeaderVisibility();
        ConfigureActionAndNavigationBars();
        ConfigureComboBoxBorders(this);
    }

    /// <summary>
    /// منع قص عنوان الدول والمسار التعريفي أعلى الشاشة.
    /// </summary>
    private void ConfigureHeaderVisibility()
    {
        pnlHeader.Padding = new Padding(18, 12, 18, 8);
        foreach (Control control in pnlHeader.Controls)
        {
            if (control is not Label label)
            {
                continue;
            }

            label.AutoEllipsis = false;
            label.RightToLeft = RightToLeft.Yes;
            label.TextAlign = ContentAlignment.MiddleRight;

            if (string.Equals(label.Text.Trim(), "الدول", StringComparison.Ordinal))
            {
                label.Height = 44;
                label.Dock = DockStyle.Top;
            }
        }
    }

    /// <summary>
    /// إعادة بناء حاوية الأزرار مع ترتيب العمليات من اليمين وإضافة أزرار التنقل.
    /// </summary>
    private void ConfigureActionAndNavigationBars()
    {
        pnlActions.SuspendLayout();
        pnlActions.Controls.Clear();
        pnlActions.BackColor = Color.White;
        pnlActions.Padding = new Padding(10, 6, 10, 6);
        pnlActions.Margin = new Padding(0, 0, 0, 8);

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            RightToLeft = RightToLeft.Yes,
            Margin = Padding.Empty,
            Padding = Padding.Empty,
            BackColor = Color.White
        };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 66F));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34F));

        var operations = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = false,
            RightToLeft = RightToLeft.Yes,
            Margin = Padding.Empty,
            Padding = Padding.Empty,
            BackColor = Color.White
        };

        // ترتيب الإضافة هنا هو نفسه الترتيب الظاهر من اليمين إلى اليسار.
        operations.Controls.AddRange(new Control[]
        {
            btnNew, btnSave, btnEdit, btnDelete, btnPrint, btnRefresh, btnClose
        });

        var navigation = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = false,
            RightToLeft = RightToLeft.Yes,
            Margin = Padding.Empty,
            Padding = new Padding(0, 1, 0, 0),
            BackColor = Color.White
        };

        var btnFirst = CreateNavigationButton("الأول");
        var btnPrevious = CreateNavigationButton("السابق");
        _lblCurrentRecord = new Label
        {
            Width = 54,
            Height = 38,
            Margin = new Padding(4, 0, 4, 0),
            Text = "1 / 5",
            TextAlign = ContentAlignment.MiddleCenter,
            BorderStyle = BorderStyle.FixedSingle,
            BackColor = Color.FromArgb(245, 248, 252),
            ForeColor = Color.FromArgb(33, 45, 65),
            Font = new Font(Font.FontFamily, 9F, FontStyle.Bold)
        };
        var btnNext = CreateNavigationButton("التالي");
        var btnLast = CreateNavigationButton("الأخير");

        btnFirst.Click += (_, _) => SelectCountryRow(0);
        btnPrevious.Click += (_, _) => SelectCountryRow(Math.Max(0, GetSelectedCountryRowIndex() - 1));
        btnNext.Click += (_, _) => SelectCountryRow(Math.Min(dgvCountries.Rows.Count - 1, GetSelectedCountryRowIndex() + 1));
        btnLast.Click += (_, _) => SelectCountryRow(dgvCountries.Rows.Count - 1);

        navigation.Controls.AddRange(new Control[]
        {
            btnFirst, btnPrevious, _lblCurrentRecord, btnNext, btnLast
        });

        root.Controls.Add(operations, 0, 0);
        root.Controls.Add(navigation, 1, 0);
        pnlActions.Controls.Add(root);
        pnlActions.ResumeLayout(true);
    }

    private Button CreateNavigationButton(string text)
    {
        return new Button
        {
            Width = 72,
            Height = 38,
            Margin = new Padding(4, 0, 4, 0),
            Text = text,
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.White,
            ForeColor = Color.FromArgb(33, 45, 65),
            Font = new Font(Font.FontFamily, 9F),
            UseVisualStyleBackColor = false
        };
    }

    /// <summary>
    /// إظهار إطار موحد لكل القوائم، ومنها حقل اللغة الافتراضية.
    /// </summary>
    private static void ConfigureComboBoxBorders(Control parent)
    {
        foreach (Control control in parent.Controls)
        {
            if (control is ComboBox comboBox)
            {
                comboBox.FlatStyle = FlatStyle.Standard;
                comboBox.BackColor = comboBox.Enabled
                    ? Color.FromArgb(255, 252, 225)
                    : Color.FromArgb(240, 242, 245);
                comboBox.RightToLeft = RightToLeft.Yes;
            }

            if (control.HasChildren)
            {
                ConfigureComboBoxBorders(control);
            }
        }
    }

    private int GetSelectedCountryRowIndex()
    {
        return dgvCountries.CurrentRow?.Index ?? 0;
    }

    private void SelectCountryRow(int index)
    {
        if (dgvCountries.Rows.Count == 0)
        {
            if (_lblCurrentRecord is not null)
            {
                _lblCurrentRecord.Text = "0 / 0";
            }
            return;
        }

        index = Math.Clamp(index, 0, dgvCountries.Rows.Count - 1);
        dgvCountries.ClearSelection();
        dgvCountries.Rows[index].Selected = true;
        dgvCountries.CurrentCell = dgvCountries.Rows[index].Cells[0];

        if (_lblCurrentRecord is not null)
        {
            _lblCurrentRecord.Text = $"{index + 1} / {dgvCountries.Rows.Count}";
        }
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

        SelectCountryRow(0);
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
