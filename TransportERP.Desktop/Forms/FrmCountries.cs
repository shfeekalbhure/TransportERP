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
    /// تثبيت عنوان الدول والمسار التعريفي في أقصى يمين الحاوية ومنع قصهما.
    /// </summary>
    private void ConfigureHeaderVisibility()
    {
        pnlHeader.SuspendLayout();
        pnlHeader.Padding = new Padding(18, 8, 18, 6);
        pnlHeader.RightToLeft = RightToLeft.No;

        Label? titleLabel = null;
        Label? trailLabel = null;

        foreach (Control control in pnlHeader.Controls)
        {
            if (control is not Label label)
            {
                continue;
            }

            label.Dock = DockStyle.None;
            label.AutoSize = false;
            label.AutoEllipsis = false;
            label.RightToLeft = RightToLeft.Yes;
            label.TextAlign = ContentAlignment.MiddleRight;
            label.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            if (string.Equals(label.Text.Trim(), "الدول", StringComparison.Ordinal))
            {
                titleLabel = label;
            }
            else
            {
                trailLabel = label;
            }
        }

        void ApplyHeaderBounds()
        {
            var width = Math.Max(100, pnlHeader.ClientSize.Width - pnlHeader.Padding.Horizontal);
            var left = pnlHeader.Padding.Left;

            if (titleLabel is not null)
            {
                titleLabel.SetBounds(left, 4, width, 44);
                titleLabel.TextAlign = ContentAlignment.MiddleRight;
                titleLabel.BringToFront();
            }

            if (trailLabel is not null)
            {
                trailLabel.SetBounds(left, 48, width, 28);
                trailLabel.TextAlign = ContentAlignment.MiddleRight;
                trailLabel.BringToFront();
            }
        }

        ApplyHeaderBounds();
        pnlHeader.Resize += (_, _) => ApplyHeaderBounds();
        pnlHeader.ResumeLayout(true);
    }

    /// <summary>
    /// بناء شريط الأدوات بترتيب ثابت من أقصى اليمين دون الاعتماد على انعكاس FlowLayoutPanel.
    /// </summary>
    private void ConfigureActionAndNavigationBars()
    {
        pnlActions.SuspendLayout();
        pnlActions.Controls.Clear();
        pnlActions.BackColor = Color.White;
        pnlActions.Padding = new Padding(10, 6, 10, 6);
        pnlActions.Margin = new Padding(0, 0, 0, 8);

        var toolbar = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 14,
            RowCount = 1,
            RightToLeft = RightToLeft.No,
            Margin = Padding.Empty,
            Padding = Padding.Empty,
            BackColor = Color.White,
            GrowStyle = TableLayoutPanelGrowStyle.FixedSize
        };

        // العمود صفر مساحة مرنة في اليسار، ثم تبدأ الأزرار حتى يصل زر جديد إلى أقصى اليمين.
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 94F));  // إغلاق
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 94F));  // تحديث
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 94F));  // طباعة
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 94F));  // حذف
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 58F));  // الأخير
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 58F));  // التالي
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 76F));  // رقم السجل
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 58F));  // السابق
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 58F));  // الأول
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 94F));  // بحث
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 94F));  // تعديل
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 94F));  // حفظ
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 94F));  // جديد
        toolbar.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        var btnToolbarSearch = CreateToolbarButton("بحث", Color.FromArgb(28, 105, 225), Color.White);
        btnToolbarSearch.Click += (_, _) => txtSearchAll.Focus();

        var btnFirst = CreateNavigationButton("|◀");
        btnFirst.AccessibleName = "الأول";
        var btnPrevious = CreateNavigationButton("◀");
        btnPrevious.AccessibleName = "السابق";
        _lblCurrentRecord = new Label
        {
            Dock = DockStyle.Fill,
            Margin = new Padding(4, 0, 4, 0),
            Text = "1 / 5",
            TextAlign = ContentAlignment.MiddleCenter,
            BorderStyle = BorderStyle.FixedSingle,
            BackColor = Color.FromArgb(245, 248, 252),
            ForeColor = Color.FromArgb(33, 45, 65),
            Font = new Font(Font.FontFamily, 9F, FontStyle.Bold)
        };
        var btnNext = CreateNavigationButton("▶");
        btnNext.AccessibleName = "التالي";
        var btnLast = CreateNavigationButton("▶|");
        btnLast.AccessibleName = "الأخير";

        btnFirst.Click += (_, _) => SelectCountryRow(0);
        btnPrevious.Click += (_, _) => SelectCountryRow(Math.Max(0, GetSelectedCountryRowIndex() - 1));
        btnNext.Click += (_, _) => SelectCountryRow(Math.Min(dgvCountries.Rows.Count - 1, GetSelectedCountryRowIndex() + 1));
        btnLast.Click += (_, _) => SelectCountryRow(dgvCountries.Rows.Count - 1);

        PrepareToolbarControl(btnNew);
        PrepareToolbarControl(btnSave);
        PrepareToolbarControl(btnEdit);
        PrepareToolbarControl(btnDelete);
        PrepareToolbarControl(btnPrint);
        PrepareToolbarControl(btnRefresh);
        PrepareToolbarControl(btnClose);
        PrepareToolbarControl(btnToolbarSearch);
        PrepareToolbarControl(btnFirst);
        PrepareToolbarControl(btnPrevious);
        PrepareToolbarControl(btnNext);
        PrepareToolbarControl(btnLast);

        // الترتيب من أقصى اليمين إلى اليسار:
        // جديد، حفظ، تعديل، بحث، الأول، السابق، رقم السجل، التالي، الأخير، حذف، طباعة، تحديث، إغلاق.
        toolbar.Controls.Add(btnClose, 1, 0);
        toolbar.Controls.Add(btnRefresh, 2, 0);
        toolbar.Controls.Add(btnPrint, 3, 0);
        toolbar.Controls.Add(btnDelete, 4, 0);
        toolbar.Controls.Add(btnLast, 5, 0);
        toolbar.Controls.Add(btnNext, 6, 0);
        toolbar.Controls.Add(_lblCurrentRecord, 7, 0);
        toolbar.Controls.Add(btnPrevious, 8, 0);
        toolbar.Controls.Add(btnFirst, 9, 0);
        toolbar.Controls.Add(btnToolbarSearch, 10, 0);
        toolbar.Controls.Add(btnEdit, 11, 0);
        toolbar.Controls.Add(btnSave, 12, 0);
        toolbar.Controls.Add(btnNew, 13, 0);

        pnlActions.Controls.Add(toolbar);
        pnlActions.ResumeLayout(true);
    }

    private static void PrepareToolbarControl(Control control)
    {
        control.Dock = DockStyle.Fill;
        control.Margin = new Padding(4, 0, 4, 0);
    }

    private Button CreateToolbarButton(string text, Color backColor, Color foreColor)
    {
        return new Button
        {
            Dock = DockStyle.Fill,
            Margin = new Padding(4, 0, 4, 0),
            Text = text,
            FlatStyle = FlatStyle.Flat,
            BackColor = backColor,
            ForeColor = foreColor,
            Font = new Font(Font.FontFamily, 9F, FontStyle.Bold),
            UseVisualStyleBackColor = false
        };
    }

    private Button CreateNavigationButton(string text)
    {
        return new Button
        {
            Dock = DockStyle.Fill,
            Margin = new Padding(4, 0, 4, 0),
            Text = text,
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.White,
            ForeColor = Color.FromArgb(33, 45, 65),
            Font = new Font(Font.FontFamily, 10F, FontStyle.Bold),
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
