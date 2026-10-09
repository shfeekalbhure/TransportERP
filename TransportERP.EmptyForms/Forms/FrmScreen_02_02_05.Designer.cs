namespace TransportERP.EmptyForms;

partial class UcScreen_02_02_05
{
    private TransportERP.Desktop.CoreUI.AuditMetadataControl standardAuditMetadata = null!;

    private TransportERP.Desktop.CoreUI.DesignerCommandBar designerCommandBar = null!;
    private Panel designerCloseHost = null!;
    private Button standardCommandDelete = null!;
    private Button standardCommandCancel = null!;
    private Button standardCommandLast = null!;
    private Button standardCommandNext = null!;
    private Button standardCommandPrevious = null!;
    private Button standardCommandFirst = null!;
    private Button standardCommandPrint = null!;
    private Button standardCommandRefresh = null!;
    private Button standardCommandImport = null!;
    private Button standardCommandExport = null!;
    private Button standardCommandHelp = null!;

    private TableLayoutPanel mainLayout = null!;
    private TableLayoutPanel pnlHeader = null!;
    private Label lblTitle = null!;
    private FlowLayoutPanel pnlToolbar = null!;
    private Button btnView = null!;
    private Button btnNew = null!;
    private Button btnSave = null!;
    private Button btnEdit = null!;
    private Button btnDisable = null!;
    private Button btnClose = null!;
    private Label lblDataStatus = null!;
    private Panel pnlContent = null!;
    private TableLayoutPanel contentLayout = null!;
    private TableLayoutPanel fieldsLayout = null!;
    private Label lblCountryId = null!;
    private ComboBox cmbCountryId = null!;
    private Label lblCode = null!;
    private TextBox txtCode = null!;
    private Label lblArabicName = null!;
    private TextBox txtArabicName = null!;
    private Label lblEnglishName = null!;
    private TextBox txtEnglishName = null!;
    private Label lblStatus = null!;
    private TextBox txtStatus = null!;
    private TextBox txtVersion = null!;
    private FlowLayoutPanel paging = null!;
    private Button btnPrevious = null!;
    private Button btnNext = null!;
    private Label lblPage = null!;
    private TableLayoutPanel tlpAuditInfo = null!;
    private Label lblCreatedBy = null!;
    private Label lblCreatedAt = null!;
    private Label lblModifiedBy = null!;
    private Label lblModifiedAt = null!;

    private void InitializeComponent()
    {
        designerCommandBar = new TransportERP.Desktop.CoreUI.DesignerCommandBar();
        designerCloseHost = new Panel();
        standardCommandDelete = new Button();
        standardCommandCancel = new Button();
        standardCommandLast = new Button();
        standardCommandNext = new Button();
        standardCommandPrevious = new Button();
        standardCommandFirst = new Button();
        standardCommandPrint = new Button();
        standardCommandRefresh = new Button();
        standardCommandImport = new Button();
        standardCommandExport = new Button();
        standardCommandHelp = new Button();
        mainLayout = new TableLayoutPanel();
        pnlHeader = new TableLayoutPanel();
        lblTitle = new Label();
        pnlToolbar = new FlowLayoutPanel();
        btnView = new Button();
        btnNew = new Button();
        btnSave = new Button();
        btnEdit = new Button();
        btnDisable = new Button();
        btnClose = new Button();
        lblDataStatus = new Label();
        pnlContent = new Panel();
        contentLayout = new TableLayoutPanel();
        searchLayout = new TableLayoutPanel();
        lblFilterSearchText = new Label();
        filterSearchText = new TextBox();
        lblFilterStatus = new Label();
        filterStatus = new ComboBox();
        lblFilterCountryId = new Label();
        filterCountryId = new ComboBox();
        btnSearch = new Button();
        dgvRecords = new DataGridView();
        colCode = new DataGridViewTextBoxColumn();
        colArabicName = new DataGridViewTextBoxColumn();
        colEnglishName = new DataGridViewTextBoxColumn();
        colStatus = new DataGridViewTextBoxColumn();
        paging = new FlowLayoutPanel();
        btnPrevious = new Button();
        btnNext = new Button();
        lblPage = new Label();
        fieldsLayout = new TableLayoutPanel();
        lblCountryId = new Label();
        cmbCountryId = new ComboBox();
        lblCode = new Label();
        txtCode = new TextBox();
        lblArabicName = new Label();
        txtArabicName = new TextBox();
        lblEnglishName = new Label();
        txtEnglishName = new TextBox();
        lblStatus = new Label();
        txtStatus = new TextBox();
        tlpAuditInfo = new TableLayoutPanel();
        lblCreatedBy = new Label();
        lblCreatedAt = new Label();
        lblModifiedBy = new Label();
        lblModifiedAt = new Label();
        txtVersion = new TextBox();
        mainLayout.SuspendLayout();
        pnlHeader.SuspendLayout();
        pnlToolbar.SuspendLayout();
        pnlContent.SuspendLayout();
        contentLayout.SuspendLayout();
        searchLayout.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvRecords).BeginInit();
        paging.SuspendLayout();
        fieldsLayout.SuspendLayout();
        tlpAuditInfo.SuspendLayout();
        SuspendLayout();
        // 
        // mainLayout
        // 
        mainLayout.ColumnCount = 1;
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        mainLayout.Controls.Add(pnlHeader, 0, 0);
        mainLayout.Controls.Add(lblDataStatus, 0, 1);
        mainLayout.Controls.Add(pnlContent, 0, 2);
        mainLayout.Dock = DockStyle.Fill;
        mainLayout.Location = new Point(0, 0);
        mainLayout.Margin = new Padding(0);
        mainLayout.Name = "mainLayout";
        mainLayout.RowCount = 3;
        mainLayout.RowStyles.Add(new RowStyle());
        mainLayout.RowStyles.Add(new RowStyle());
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        mainLayout.Size = new Size(1100, 760);
        mainLayout.TabIndex = 0;
        // 
        // pnlHeader
        // 
        pnlHeader.AutoSize = true;
        pnlHeader.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        pnlHeader.ColumnCount = 1;
        pnlHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        pnlHeader.Controls.Add(lblTitle, 0, 0);
        pnlHeader.Controls.Add(designerCommandBar, 0, 1);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Location = new Point(0, 0);
        pnlHeader.Margin = new Padding(0);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.RowCount = 2;
        pnlHeader.RowStyles.Add(new RowStyle());
        pnlHeader.RowStyles.Add(new RowStyle());
        pnlHeader.Size = new Size(1100, 282);
        pnlHeader.TabIndex = 0;
        // 
        // lblTitle
        // 
        lblTitle.AutoSize = true;
        lblTitle.BackColor = Color.FromArgb(192, 192, 255);
        lblTitle.Dock = DockStyle.Fill;
        lblTitle.Font = new Font("Segoe UI", 16F);
        lblTitle.Location = new Point(3, 0);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(1094, 30);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "المحافظات";
        // 
        // pnlToolbar
        // 
        pnlToolbar.AutoSize = true;
        pnlToolbar.AutoSizeMode = AutoSizeMode.GrowAndShrink;

        pnlToolbar.Controls.Add(btnDisable);

        pnlToolbar.Dock = DockStyle.Top;
        pnlToolbar.Location = new Point(3, 33);
        pnlToolbar.Name = "pnlToolbar";
        pnlToolbar.RightToLeft = RightToLeft.Yes;
        pnlToolbar.Size = new Size(1094, 41);
        pnlToolbar.TabIndex = 1;
        // 
        // btnView
        // 
        btnView.AccessibleDescription = "غير منفذ: لم يتم ربط خدمة البيانات أو الحفظ.";
        btnView.AutoSize = true;
        btnView.Enabled = false;
        btnView.Location = new Point(1020, 4);
        btnView.Margin = new Padding(4);
        btnView.Name = "btnView";
        btnView.Size = new Size(70, 33);
        btnView.TabIndex = 0;
        btnView.Text = "عرض";
        // 
        // btnNew
        // 
        btnNew.AccessibleDescription = "غير منفذ: لم يتم ربط خدمة البيانات أو الحفظ.";
        btnNew.AutoSize = true;
        btnNew.Enabled = false;
        btnNew.Location = new Point(942, 4);
        btnNew.Margin = new Padding(4);
        btnNew.Name = "btnNew";
        btnNew.Size = new Size(70, 33);
        btnNew.TabIndex = 1;
        btnNew.Text = "جديد";
        // 
        // btnSave
        // 
        btnSave.AccessibleDescription = "غير منفذ: لم يتم ربط خدمة البيانات أو الحفظ.";
        btnSave.AutoSize = true;
        btnSave.Enabled = false;
        btnSave.Location = new Point(864, 4);
        btnSave.Margin = new Padding(4);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(70, 33);
        btnSave.TabIndex = 2;
        btnSave.Text = "حفظ";
        // 
        // btnEdit
        // 
        btnEdit.AccessibleDescription = "غير منفذ: لم يتم ربط خدمة البيانات أو الحفظ.";
        btnEdit.AutoSize = true;
        btnEdit.Enabled = false;
        btnEdit.Location = new Point(786, 4);
        btnEdit.Margin = new Padding(4);
        btnEdit.Name = "btnEdit";
        btnEdit.Size = new Size(70, 33);
        btnEdit.TabIndex = 3;
        btnEdit.Text = "تعديل";
        // 
        // btnDisable
        // 
        btnDisable.AccessibleDescription = "غير منفذ: لم يتم ربط خدمة البيانات أو الحفظ.";
        btnDisable.AutoSize = true;
        btnDisable.Enabled = false;
        btnDisable.Location = new Point(708, 4);
        btnDisable.Margin = new Padding(4);
        btnDisable.Name = "btnDisable";
        btnDisable.Size = new Size(70, 33);
        btnDisable.TabIndex = 4;
        btnDisable.Text = "إيقاف";
        // 
        // btnClose
        // 
        btnClose.AccessibleDescription = "إغلاق الشاشة";
        btnClose.AutoSize = true;
        btnClose.Location = new Point(630, 4);
        btnClose.Margin = new Padding(4);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(70, 33);
        btnClose.TabIndex = 5;
        btnClose.Text = "إغلاق";
        btnClose.Click += BtnClose_Click;
        // 
        // lblDataStatus
        // 
        lblDataStatus.AutoSize = true;
        lblDataStatus.Dock = DockStyle.Fill;
        lblDataStatus.Location = new Point(3, 282);
        lblDataStatus.Name = "lblDataStatus";
        lblDataStatus.Size = new Size(1094, 19);
        lblDataStatus.TabIndex = 1;
        lblDataStatus.Text = "واجهة تصميم — لم يتم تحميل بيانات؛ الحفظ والبحث والتصفح غير مربوطين بالخدمات.";
        // 
        // pnlContent
        // 
        pnlContent.AutoScroll = true;
        pnlContent.Controls.Add(contentLayout);
        pnlContent.Controls.Add(txtVersion);
        pnlContent.Dock = DockStyle.Left;
        pnlContent.Location = new Point(264, 304);
        pnlContent.Name = "pnlContent";
        pnlContent.Size = new Size(833, 453);
        pnlContent.TabIndex = 2;
        // 
        // contentLayout
        // 
        contentLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        contentLayout.ColumnCount = 1;
        contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        contentLayout.Controls.Add(searchLayout, 0, 1);
        contentLayout.Controls.Add(dgvRecords, 0, 2);
        contentLayout.Controls.Add(paging, 0, 3);
        contentLayout.Controls.Add(tlpAuditInfo, 0, 4);
        contentLayout.Dock = DockStyle.Top;
        contentLayout.Location = new Point(0, 0);
        contentLayout.Margin = new Padding(0);
        contentLayout.MinimumSize = new Size(700, 0);
        contentLayout.Name = "contentLayout";
        contentLayout.RowCount = 5;
        contentLayout.RowStyles.Add(new RowStyle());
        contentLayout.RowStyles.Add(new RowStyle());
        contentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        contentLayout.RowStyles.Add(new RowStyle());
        contentLayout.RowStyles.Add(new RowStyle());
        contentLayout.Size = new Size(833, 100);
        contentLayout.TabIndex = 0;
        // 
        // searchLayout
        // 
        searchLayout.AutoSize = true;
        searchLayout.ColumnCount = 4;
        searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
        searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
        searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        searchLayout.Controls.Add(lblFilterSearchText, 0, 0);
        searchLayout.Controls.Add(filterSearchText, 1, 0);
        searchLayout.Controls.Add(lblFilterStatus, 2, 0);
        searchLayout.Controls.Add(filterStatus, 3, 0);
        searchLayout.Controls.Add(lblFilterCountryId, 0, 1);
        searchLayout.Controls.Add(filterCountryId, 1, 1);
        searchLayout.Controls.Add(btnSearch, 3, 1);
        searchLayout.Location = new Point(0, 0);
        searchLayout.Margin = new Padding(0);
        searchLayout.Name = "searchLayout";
        searchLayout.RowCount = 2;
        searchLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        searchLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        searchLayout.Size = new Size(833, 80);
        searchLayout.TabIndex = 1;
        searchLayout.Paint += searchLayout_Paint;
        // 
        // lblFilterSearchText
        // 
        lblFilterSearchText.Dock = DockStyle.Fill;
        lblFilterSearchText.Location = new Point(686, 0);
        lblFilterSearchText.Name = "lblFilterSearchText";
        lblFilterSearchText.Size = new Size(144, 40);
        lblFilterSearchText.TabIndex = 0;
        lblFilterSearchText.Text = "نص البحث";
        // 
        // filterSearchText
        // 
        filterSearchText.AccessibleName = "نص البحث";
        filterSearchText.Dock = DockStyle.Fill;
        filterSearchText.Location = new Point(420, 3);
        filterSearchText.Name = "filterSearchText";
        filterSearchText.Size = new Size(260, 25);
        filterSearchText.TabIndex = 0;
        // 
        // lblFilterStatus
        // 
        lblFilterStatus.Dock = DockStyle.Fill;
        lblFilterStatus.Location = new Point(270, 0);
        lblFilterStatus.Name = "lblFilterStatus";
        lblFilterStatus.Size = new Size(144, 40);
        lblFilterStatus.TabIndex = 1;
        lblFilterStatus.Text = "الحالة";
        // 
        // filterStatus
        // 
        filterStatus.AccessibleName = "الحالة";
        filterStatus.Dock = DockStyle.Fill;
        filterStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        filterStatus.Items.AddRange(new object[] { "الكل", "Active", "Stopped" });
        filterStatus.Location = new Point(3, 3);
        filterStatus.Name = "filterStatus";
        filterStatus.Size = new Size(261, 25);
        filterStatus.TabIndex = 1;
        // 
        // lblFilterCountryId
        // 
        lblFilterCountryId.Dock = DockStyle.Fill;
        lblFilterCountryId.Location = new Point(686, 40);
        lblFilterCountryId.Name = "lblFilterCountryId";
        lblFilterCountryId.Size = new Size(144, 40);
        lblFilterCountryId.TabIndex = 2;
        lblFilterCountryId.Text = "الدولة";
        // 
        // filterCountryId
        // 
        filterCountryId.AccessibleName = "الدولة";
        filterCountryId.Dock = DockStyle.Fill;
        filterCountryId.DropDownStyle = ComboBoxStyle.DropDownList;
        filterCountryId.Items.AddRange(new object[] { "الكل" });
        filterCountryId.Location = new Point(420, 43);
        filterCountryId.Name = "filterCountryId";
        filterCountryId.Size = new Size(260, 25);
        filterCountryId.TabIndex = 2;
        // 
        // btnSearch
        // 
        btnSearch.AccessibleDescription = "البحث بالخدمة غير مربوط.";
        btnSearch.AutoSize = true;
        btnSearch.Enabled = false;
        btnSearch.Location = new Point(212, 43);
        btnSearch.Name = "btnSearch";
        btnSearch.Size = new Size(52, 33);
        btnSearch.TabIndex = 3;
        btnSearch.Text = "بحث";
        // 
        // dgvRecords
        // 
        dgvRecords.AccessibleName = "قائمة البيانات — لا توجد بيانات محملة";
        dgvRecords.AllowUserToAddRows = false;
        dgvRecords.AllowUserToDeleteRows = false;
        dgvRecords.ColumnHeadersHeight = 29;
        dgvRecords.Columns.AddRange(new DataGridViewColumn[] { colCode, colArabicName, colEnglishName, colStatus });
        dgvRecords.Location = new Point(820, 83);
        dgvRecords.MinimumSize = new Size(0, 280);
        dgvRecords.MultiSelect = false;
        dgvRecords.Name = "dgvRecords";
        dgvRecords.ReadOnly = true;
        dgvRecords.RowHeadersVisible = false;
        dgvRecords.RowHeadersWidth = 51;
        dgvRecords.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvRecords.Size = new Size(10, 280);
        dgvRecords.TabIndex = 2;
        dgvRecords.CellContentClick += dgvRecords_CellContentClick;
        // 
        // colCode
        // 
        colCode.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
        colCode.HeaderText = "الرمز";
        colCode.MinimumWidth = 75;
        colCode.Name = "colCode";
        colCode.ReadOnly = true;
        colCode.SortMode = DataGridViewColumnSortMode.NotSortable;
        colCode.Width = 75;
        // 
        // colArabicName
        // 
        colArabicName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        colArabicName.FillWeight = 160F;
        colArabicName.HeaderText = "الاسم العربي";
        colArabicName.MinimumWidth = 75;
        colArabicName.Name = "colArabicName";
        colArabicName.ReadOnly = true;
        colArabicName.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // colEnglishName
        // 
        colEnglishName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        colEnglishName.HeaderText = "الاسم الإنجليزي";
        colEnglishName.MinimumWidth = 75;
        colEnglishName.Name = "colEnglishName";
        colEnglishName.ReadOnly = true;
        colEnglishName.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // colStatus
        // 
        colStatus.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
        colStatus.HeaderText = "الحالة";
        colStatus.MinimumWidth = 75;
        colStatus.Name = "colStatus";
        colStatus.ReadOnly = true;
        colStatus.SortMode = DataGridViewColumnSortMode.NotSortable;
        colStatus.Width = 75;
        // 
        // paging
        // 
        paging.AutoSize = true;
        paging.Controls.Add(btnPrevious);
        paging.Controls.Add(btnNext);
        paging.Controls.Add(lblPage);
        paging.Controls.Add(fieldsLayout);
        paging.Dock = DockStyle.Top;
        paging.Location = new Point(3, -102);
        paging.Name = "paging";
        paging.RightToLeft = RightToLeft.Yes;
        paging.Size = new Size(827, 161);
        paging.TabIndex = 3;
        // 
        // btnPrevious
        // 
        btnPrevious.AccessibleDescription = "غير منفذ: لم يتم ربط خدمة البيانات أو الحفظ.";
        btnPrevious.AutoSize = true;
        btnPrevious.Enabled = false;
        btnPrevious.Location = new Point(753, 4);
        btnPrevious.Margin = new Padding(4);
        btnPrevious.Name = "btnPrevious";
        btnPrevious.Size = new Size(70, 33);
        btnPrevious.TabIndex = 0;
        btnPrevious.Text = "السابق";
        // 
        // btnNext
        // 
        btnNext.AccessibleDescription = "غير منفذ: لم يتم ربط خدمة البيانات أو الحفظ.";
        btnNext.AutoSize = true;
        btnNext.Enabled = false;
        btnNext.Location = new Point(675, 4);
        btnNext.Margin = new Padding(4);
        btnNext.Name = "btnNext";
        btnNext.Size = new Size(70, 33);
        btnNext.TabIndex = 1;
        btnNext.Text = "التالي";
        // 
        // lblPage
        // 
        lblPage.AutoSize = true;
        lblPage.Location = new Point(449, 0);
        lblPage.Name = "lblPage";
        lblPage.Size = new Size(219, 19);
        lblPage.TabIndex = 2;
        lblPage.Text = "الصفحات: — لم يتصل مزود البيانات";
        // 
        // fieldsLayout
        // 
        fieldsLayout.AutoSize = true;
        fieldsLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        fieldsLayout.ColumnCount = 4;
        fieldsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
        fieldsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        fieldsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
        fieldsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        fieldsLayout.Controls.Add(lblCountryId, 0, 0);
        fieldsLayout.Controls.Add(cmbCountryId, 1, 0);
        fieldsLayout.Controls.Add(lblCode, 2, 0);
        fieldsLayout.Controls.Add(txtCode, 3, 0);
        fieldsLayout.Controls.Add(lblArabicName, 0, 1);
        fieldsLayout.Controls.Add(txtArabicName, 1, 1);
        fieldsLayout.Controls.Add(lblEnglishName, 2, 1);
        fieldsLayout.Controls.Add(txtEnglishName, 3, 1);
        fieldsLayout.Controls.Add(lblStatus, 0, 2);
        fieldsLayout.Controls.Add(txtStatus, 1, 2);
        fieldsLayout.Location = new Point(0, 41);
        fieldsLayout.Margin = new Padding(0);
        fieldsLayout.Name = "fieldsLayout";
        fieldsLayout.RowCount = 3;
        fieldsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        fieldsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        fieldsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        fieldsLayout.Size = new Size(827, 120);
        fieldsLayout.TabIndex = 0;
        // 
        // lblCountryId
        // 
        lblCountryId.Dock = DockStyle.Fill;
        lblCountryId.Location = new Point(680, 0);
        lblCountryId.Name = "lblCountryId";
        lblCountryId.Size = new Size(144, 40);
        lblCountryId.TabIndex = 0;
        lblCountryId.Text = "الدولة *";
        lblCountryId.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cmbCountryId
        // 
        cmbCountryId.AccessibleDescription = "مرجع مطلوب؛ لا يوجد مزود بيانات متصل. اختيار الأصل خاص بالإنشاء ولا يُغيّر عند التعديل.";
        cmbCountryId.AccessibleName = "الدولة";
        cmbCountryId.BackColor = Color.FromArgb(255, 249, 219);
        cmbCountryId.Dock = DockStyle.Fill;
        cmbCountryId.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbCountryId.Location = new Point(417, 3);
        cmbCountryId.Name = "cmbCountryId";
        cmbCountryId.Size = new Size(257, 25);
        cmbCountryId.TabIndex = 0;
        cmbCountryId.Tag = "CountryId";
        // 
        // lblCode
        // 
        lblCode.Dock = DockStyle.Fill;
        lblCode.Location = new Point(267, 0);
        lblCode.Name = "lblCode";
        lblCode.Size = new Size(144, 40);
        lblCode.TabIndex = 1;
        lblCode.Text = "الرمز *";
        lblCode.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtCode
        // 
        txtCode.AccessibleName = "الرمز";
        txtCode.BackColor = Color.FromArgb(255, 249, 219);
        txtCode.Dock = DockStyle.Fill;
        txtCode.Location = new Point(3, 3);
        txtCode.Name = "txtCode";
        txtCode.RightToLeft = RightToLeft.No;
        txtCode.Size = new Size(258, 25);
        txtCode.TabIndex = 1;
        txtCode.Tag = "Code";
        // 
        // lblArabicName
        // 
        lblArabicName.Dock = DockStyle.Fill;
        lblArabicName.Location = new Point(680, 40);
        lblArabicName.Name = "lblArabicName";
        lblArabicName.Size = new Size(144, 40);
        lblArabicName.TabIndex = 2;
        lblArabicName.Text = "الاسم العربي *";
        lblArabicName.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtArabicName
        // 
        txtArabicName.AccessibleName = "الاسم العربي";
        txtArabicName.BackColor = Color.FromArgb(255, 249, 219);
        txtArabicName.Dock = DockStyle.Fill;
        txtArabicName.Location = new Point(417, 43);
        txtArabicName.Name = "txtArabicName";
        txtArabicName.Size = new Size(257, 25);
        txtArabicName.TabIndex = 2;
        txtArabicName.Tag = "ArabicName";
        // 
        // lblEnglishName
        // 
        lblEnglishName.Dock = DockStyle.Fill;
        lblEnglishName.Location = new Point(267, 40);
        lblEnglishName.Name = "lblEnglishName";
        lblEnglishName.Size = new Size(144, 40);
        lblEnglishName.TabIndex = 3;
        lblEnglishName.Text = "الاسم الإنجليزي";
        lblEnglishName.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtEnglishName
        // 
        txtEnglishName.AccessibleName = "الاسم الإنجليزي";
        txtEnglishName.Dock = DockStyle.Fill;
        txtEnglishName.Location = new Point(3, 43);
        txtEnglishName.Name = "txtEnglishName";
        txtEnglishName.RightToLeft = RightToLeft.No;
        txtEnglishName.Size = new Size(258, 25);
        txtEnglishName.TabIndex = 3;
        txtEnglishName.Tag = "EnglishName";
        // 
        // lblStatus
        // 
        lblStatus.Dock = DockStyle.Fill;
        lblStatus.Location = new Point(680, 80);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(144, 40);
        lblStatus.TabIndex = 4;
        lblStatus.Text = "الحالة";
        lblStatus.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtStatus
        // 
        txtStatus.AccessibleDescription = "إسقاط حالة من الخدمة؛ لا تُعدل الحالة مباشرة.";
        txtStatus.AccessibleName = "الحالة";
        txtStatus.Dock = DockStyle.Fill;
        txtStatus.Location = new Point(417, 83);
        txtStatus.Name = "txtStatus";
        txtStatus.ReadOnly = true;
        txtStatus.Size = new Size(257, 25);
        txtStatus.TabIndex = 4;
        txtStatus.Tag = "Status";
        // 
        // tlpAuditInfo
        // 
        tlpAuditInfo.AutoSize = true;
        tlpAuditInfo.ColumnCount = 2;
        tlpAuditInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpAuditInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpAuditInfo.Controls.Add(lblCreatedBy, 0, 0);
        tlpAuditInfo.Controls.Add(lblCreatedAt, 1, 0);
        tlpAuditInfo.Controls.Add(lblModifiedBy, 0, 1);
        tlpAuditInfo.Controls.Add(lblModifiedAt, 1, 1);
        tlpAuditInfo.Dock = DockStyle.Top;
        tlpAuditInfo.Location = new Point(0, 62);
        tlpAuditInfo.Margin = new Padding(0);
        tlpAuditInfo.Name = "tlpAuditInfo";
        tlpAuditInfo.RowCount = 2;
        tlpAuditInfo.RowStyles.Add(new RowStyle());
        tlpAuditInfo.RowStyles.Add(new RowStyle());
        tlpAuditInfo.Size = new Size(833, 38);
        tlpAuditInfo.TabIndex = 4;
        // 
        // lblCreatedBy
        // 
        lblCreatedBy.AutoSize = true;
        lblCreatedBy.Dock = DockStyle.Fill;
        lblCreatedBy.Location = new Point(420, 0);
        lblCreatedBy.Name = "lblCreatedBy";
        lblCreatedBy.Size = new Size(410, 19);
        lblCreatedBy.TabIndex = 0;
        lblCreatedBy.Text = "أنشأ بواسطة: —";
        // 
        // lblCreatedAt
        // 
        lblCreatedAt.AutoSize = true;
        lblCreatedAt.Dock = DockStyle.Fill;
        lblCreatedAt.Location = new Point(3, 0);
        lblCreatedAt.Name = "lblCreatedAt";
        lblCreatedAt.Size = new Size(411, 19);
        lblCreatedAt.TabIndex = 1;
        lblCreatedAt.Text = "تاريخ الإنشاء: —";
        // 
        // lblModifiedBy
        // 
        lblModifiedBy.AutoSize = true;
        lblModifiedBy.Dock = DockStyle.Fill;
        lblModifiedBy.Location = new Point(420, 19);
        lblModifiedBy.Name = "lblModifiedBy";
        lblModifiedBy.Size = new Size(410, 19);
        lblModifiedBy.TabIndex = 2;
        lblModifiedBy.Text = "عدل بواسطة: —";
        // 
        // lblModifiedAt
        // 
        lblModifiedAt.AutoSize = true;
        lblModifiedAt.Dock = DockStyle.Fill;
        lblModifiedAt.Location = new Point(3, 19);
        lblModifiedAt.Name = "lblModifiedAt";
        lblModifiedAt.Size = new Size(411, 19);
        lblModifiedAt.TabIndex = 3;
        lblModifiedAt.Text = "تاريخ التعديل: —";
        // 
        // txtVersion
        // 
        txtVersion.Location = new Point(0, 0);
        txtVersion.Name = "txtVersion";
        txtVersion.ReadOnly = true;
        txtVersion.Size = new Size(100, 25);
        txtVersion.TabIndex = 1;
        txtVersion.TabStop = false;
        txtVersion.Tag = "Version";
        txtVersion.Visible = false;
        // 
        // UcScreen_02_02_05
        // 
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        Controls.Add(mainLayout);
        Font = new Font("Segoe UI", 10F);
        Name = "UcScreen_02_02_05";
        RightToLeft = RightToLeft.Yes;
        Size = new Size(1100, 760);
        Tag = "02.02.05";
        mainLayout.ResumeLayout(false);
        mainLayout.PerformLayout();
        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        pnlToolbar.ResumeLayout(false);
        pnlToolbar.PerformLayout();
        pnlContent.ResumeLayout(false);
        pnlContent.PerformLayout();
        contentLayout.ResumeLayout(false);
        contentLayout.PerformLayout();
        searchLayout.ResumeLayout(false);
        searchLayout.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvRecords).EndInit();
        paging.ResumeLayout(false);
        paging.PerformLayout();
        fieldsLayout.ResumeLayout(false);
        fieldsLayout.PerformLayout();
        tlpAuditInfo.ResumeLayout(false);
        tlpAuditInfo.PerformLayout();
        ResumeLayout(false);
    
        // Explicit Designer-owned command catalog.
        designerCommandBar.Dock = DockStyle.Top;
        designerCommandBar.Location = new Point(3, 33);
        designerCommandBar.Size = new Size(1094, 41);
        designerCommandBar.TabIndex = 1;
        designerCommandBar.RightToLeft = RightToLeft.Yes;
        designerCommandBar.Controls.Add(pnlToolbar);
        pnlToolbar.Dock = DockStyle.Fill;
        pnlToolbar.Margin = Padding.Empty;
        designerCommandBar.Name = "designerCommandBar";
        designerCommandBar.BackColor = Color.FromArgb(239, 239, 239);
        designerCommandBar.BorderStyle = BorderStyle.FixedSingle;
        designerCommandBar.Controls.Add(designerCloseHost);
        designerCloseHost.Dock = DockStyle.Left;
        designerCloseHost.Width = 28;
        designerCloseHost.Name = "designerCloseHost";
        pnlToolbar.Name = "pnlToolbar";
        pnlToolbar.Dock = DockStyle.Fill;
        pnlToolbar.AutoSize = false;
        pnlToolbar.WrapContents = false;
        pnlToolbar.FlowDirection = FlowDirection.RightToLeft;
        pnlToolbar.RightToLeft = RightToLeft.No;
        pnlToolbar.Padding = new Padding(2);
        designerCommandBar.MinimumSize = new Size(0, 30);
        designerCommandBar.Height = 44;
        standardCommandDelete.Name = "standardCommandDelete";
        standardCommandDelete.Enabled = false;
        standardCommandDelete.Visible = true;
        standardCommandDelete.AccessibleName = "حذف";
        standardCommandCancel.Name = "standardCommandCancel";
        standardCommandCancel.Enabled = false;
        standardCommandCancel.Visible = true;
        standardCommandCancel.AccessibleName = "تراجع";
        standardCommandLast.Name = "standardCommandLast";
        standardCommandLast.Enabled = false;
        standardCommandLast.Visible = true;
        standardCommandLast.AccessibleName = "الأخير";
        standardCommandNext.Name = "standardCommandNext";
        standardCommandNext.Enabled = false;
        standardCommandNext.Visible = true;
        standardCommandNext.AccessibleName = "التالي";
        standardCommandPrevious.Name = "standardCommandPrevious";
        standardCommandPrevious.Enabled = false;
        standardCommandPrevious.Visible = true;
        standardCommandPrevious.AccessibleName = "السابق";
        standardCommandFirst.Name = "standardCommandFirst";
        standardCommandFirst.Enabled = false;
        standardCommandFirst.Visible = true;
        standardCommandFirst.AccessibleName = "الأول";
        standardCommandPrint.Name = "standardCommandPrint";
        standardCommandPrint.Enabled = false;
        standardCommandPrint.Visible = true;
        standardCommandPrint.AccessibleName = "طباعة";
        standardCommandRefresh.Name = "standardCommandRefresh";
        standardCommandRefresh.Enabled = false;
        standardCommandRefresh.Visible = false;
        standardCommandRefresh.AccessibleName = "تحديث";
        standardCommandImport.Name = "standardCommandImport";
        standardCommandImport.Enabled = false;
        standardCommandImport.Visible = false;
        standardCommandImport.AccessibleName = "استيراد";
        standardCommandExport.Name = "standardCommandExport";
        standardCommandExport.Enabled = false;
        standardCommandExport.Visible = false;
        standardCommandExport.AccessibleName = "تصدير";
        standardCommandHelp.Name = "standardCommandHelp";
        standardCommandHelp.Enabled = false;
        standardCommandHelp.Visible = false;
        standardCommandHelp.AccessibleName = "مساعدة";
        btnNew.AutoSize = false;
        btnNew.Dock = DockStyle.None;
        btnNew.MinimumSize = Size.Empty;
        btnNew.Size = new Size(26, 24);
        btnNew.Margin = new Padding(1);
        btnNew.FlatStyle = FlatStyle.Flat;
        btnNew.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnNew.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Add;
        btnNew.Text = "";
        pnlToolbar.Controls.Add(btnNew);
        designerCommandBar.SetCommandRole(btnNew, TransportERP.Desktop.CoreUI.DesignerCommandRole.Add);
        btnEdit.AutoSize = false;
        btnEdit.Dock = DockStyle.None;
        btnEdit.MinimumSize = Size.Empty;
        btnEdit.Size = new Size(26, 24);
        btnEdit.Margin = new Padding(1);
        btnEdit.FlatStyle = FlatStyle.Flat;
        btnEdit.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnEdit.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Edit;
        btnEdit.Text = "";
        pnlToolbar.Controls.Add(btnEdit);
        designerCommandBar.SetCommandRole(btnEdit, TransportERP.Desktop.CoreUI.DesignerCommandRole.Edit);
        standardCommandDelete.AutoSize = false;
        standardCommandDelete.Dock = DockStyle.None;
        standardCommandDelete.MinimumSize = Size.Empty;
        standardCommandDelete.Size = new Size(26, 24);
        standardCommandDelete.Margin = new Padding(1);
        standardCommandDelete.FlatStyle = FlatStyle.Flat;
        standardCommandDelete.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandDelete.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Delete;
        standardCommandDelete.Text = "";
        pnlToolbar.Controls.Add(standardCommandDelete);
        designerCommandBar.SetCommandRole(standardCommandDelete, TransportERP.Desktop.CoreUI.DesignerCommandRole.Delete);
        standardCommandCancel.AutoSize = false;
        standardCommandCancel.Dock = DockStyle.None;
        standardCommandCancel.MinimumSize = Size.Empty;
        standardCommandCancel.Size = new Size(26, 24);
        standardCommandCancel.Margin = new Padding(1);
        standardCommandCancel.FlatStyle = FlatStyle.Flat;
        standardCommandCancel.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandCancel.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Cancel;
        standardCommandCancel.Text = "";
        pnlToolbar.Controls.Add(standardCommandCancel);
        designerCommandBar.SetCommandRole(standardCommandCancel, TransportERP.Desktop.CoreUI.DesignerCommandRole.Cancel);
        btnView.AutoSize = false;
        btnView.Dock = DockStyle.None;
        btnView.MinimumSize = Size.Empty;
        btnView.Size = new Size(26, 24);
        btnView.Margin = new Padding(1);
        btnView.FlatStyle = FlatStyle.Flat;
        btnView.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnView.Image = TransportERP.Desktop.CoreUI.CommandBarImages.View;
        btnView.Text = "";
        pnlToolbar.Controls.Add(btnView);
        designerCommandBar.SetCommandRole(btnView, TransportERP.Desktop.CoreUI.DesignerCommandRole.View);
        standardCommandLast.AutoSize = false;
        standardCommandLast.Dock = DockStyle.None;
        standardCommandLast.MinimumSize = Size.Empty;
        standardCommandLast.Size = new Size(26, 24);
        standardCommandLast.Margin = new Padding(1);
        standardCommandLast.FlatStyle = FlatStyle.Flat;
        standardCommandLast.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandLast.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Last;
        standardCommandLast.Text = "";
        pnlToolbar.Controls.Add(standardCommandLast);
        designerCommandBar.SetCommandRole(standardCommandLast, TransportERP.Desktop.CoreUI.DesignerCommandRole.Last);
        standardCommandNext.AutoSize = false;
        standardCommandNext.Dock = DockStyle.None;
        standardCommandNext.MinimumSize = Size.Empty;
        standardCommandNext.Size = new Size(26, 24);
        standardCommandNext.Margin = new Padding(1);
        standardCommandNext.FlatStyle = FlatStyle.Flat;
        standardCommandNext.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandNext.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Next;
        standardCommandNext.Text = "";
        pnlToolbar.Controls.Add(standardCommandNext);
        designerCommandBar.SetCommandRole(standardCommandNext, TransportERP.Desktop.CoreUI.DesignerCommandRole.Next);
        standardCommandPrevious.AutoSize = false;
        standardCommandPrevious.Dock = DockStyle.None;
        standardCommandPrevious.MinimumSize = Size.Empty;
        standardCommandPrevious.Size = new Size(26, 24);
        standardCommandPrevious.Margin = new Padding(1);
        standardCommandPrevious.FlatStyle = FlatStyle.Flat;
        standardCommandPrevious.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandPrevious.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Previous;
        standardCommandPrevious.Text = "";
        pnlToolbar.Controls.Add(standardCommandPrevious);
        designerCommandBar.SetCommandRole(standardCommandPrevious, TransportERP.Desktop.CoreUI.DesignerCommandRole.Previous);
        standardCommandFirst.AutoSize = false;
        standardCommandFirst.Dock = DockStyle.None;
        standardCommandFirst.MinimumSize = Size.Empty;
        standardCommandFirst.Size = new Size(26, 24);
        standardCommandFirst.Margin = new Padding(1);
        standardCommandFirst.FlatStyle = FlatStyle.Flat;
        standardCommandFirst.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandFirst.Image = TransportERP.Desktop.CoreUI.CommandBarImages.First;
        standardCommandFirst.Text = "";
        pnlToolbar.Controls.Add(standardCommandFirst);
        designerCommandBar.SetCommandRole(standardCommandFirst, TransportERP.Desktop.CoreUI.DesignerCommandRole.First);
        btnSave.AutoSize = false;
        btnSave.Dock = DockStyle.None;
        btnSave.MinimumSize = Size.Empty;
        btnSave.Size = new Size(26, 24);
        btnSave.Margin = new Padding(1);
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnSave.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Save;
        btnSave.Text = "";
        pnlToolbar.Controls.Add(btnSave);
        designerCommandBar.SetCommandRole(btnSave, TransportERP.Desktop.CoreUI.DesignerCommandRole.Save);
        standardCommandPrint.AutoSize = false;
        standardCommandPrint.Dock = DockStyle.None;
        standardCommandPrint.MinimumSize = Size.Empty;
        standardCommandPrint.Size = new Size(26, 24);
        standardCommandPrint.Margin = new Padding(1);
        standardCommandPrint.FlatStyle = FlatStyle.Flat;
        standardCommandPrint.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandPrint.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Print;
        standardCommandPrint.Text = "";
        pnlToolbar.Controls.Add(standardCommandPrint);
        designerCommandBar.SetCommandRole(standardCommandPrint, TransportERP.Desktop.CoreUI.DesignerCommandRole.Print);
        btnClose.AutoSize = false;
        btnClose.Dock = DockStyle.None;
        btnClose.MinimumSize = Size.Empty;
        btnClose.Size = new Size(26, 24);
        btnClose.Margin = new Padding(1);
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnClose.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Close;
        btnClose.Text = "";
        designerCloseHost.Controls.Add(btnClose);
        btnClose.Location = new Point(1, 1);
        designerCommandBar.SetCommandRole(btnClose, TransportERP.Desktop.CoreUI.DesignerCommandRole.Close);
        standardCommandRefresh.AutoSize = false;
        standardCommandRefresh.Dock = DockStyle.None;
        standardCommandRefresh.MinimumSize = Size.Empty;
        standardCommandRefresh.Size = new Size(26, 24);
        standardCommandRefresh.Margin = new Padding(1);
        standardCommandRefresh.FlatStyle = FlatStyle.Flat;
        standardCommandRefresh.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandRefresh.Text = "تحديث";
        pnlToolbar.Controls.Add(standardCommandRefresh);
        designerCommandBar.SetCommandRole(standardCommandRefresh, TransportERP.Desktop.CoreUI.DesignerCommandRole.Refresh);
        standardCommandImport.AutoSize = false;
        standardCommandImport.Dock = DockStyle.None;
        standardCommandImport.MinimumSize = Size.Empty;
        standardCommandImport.Size = new Size(26, 24);
        standardCommandImport.Margin = new Padding(1);
        standardCommandImport.FlatStyle = FlatStyle.Flat;
        standardCommandImport.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandImport.Text = "استيراد";
        pnlToolbar.Controls.Add(standardCommandImport);
        designerCommandBar.SetCommandRole(standardCommandImport, TransportERP.Desktop.CoreUI.DesignerCommandRole.Import);
        standardCommandExport.AutoSize = false;
        standardCommandExport.Dock = DockStyle.None;
        standardCommandExport.MinimumSize = Size.Empty;
        standardCommandExport.Size = new Size(26, 24);
        standardCommandExport.Margin = new Padding(1);
        standardCommandExport.FlatStyle = FlatStyle.Flat;
        standardCommandExport.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandExport.Text = "تصدير";
        pnlToolbar.Controls.Add(standardCommandExport);
        designerCommandBar.SetCommandRole(standardCommandExport, TransportERP.Desktop.CoreUI.DesignerCommandRole.Export);
        standardCommandHelp.AutoSize = false;
        standardCommandHelp.Dock = DockStyle.None;
        standardCommandHelp.MinimumSize = Size.Empty;
        standardCommandHelp.Size = new Size(26, 24);
        standardCommandHelp.Margin = new Padding(1);
        standardCommandHelp.FlatStyle = FlatStyle.Flat;
        standardCommandHelp.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandHelp.Text = "مساعدة";
        pnlToolbar.Controls.Add(standardCommandHelp);
        designerCommandBar.SetCommandRole(standardCommandHelp, TransportERP.Desktop.CoreUI.DesignerCommandRole.Help);
    
        // Shared audit presentation; original sources remain owned by this screen.
        standardAuditMetadata = new TransportERP.Desktop.CoreUI.AuditMetadataControl();
        standardAuditMetadata.Name = "standardAuditMetadata";
        standardAuditMetadata.Dock = DockStyle.Bottom;
        standardAuditMetadata.Margin = Padding.Empty;
        standardAuditMetadata.TabStop = false;
        standardAuditMetadata.Size = new Size(800, 64);
        tlpAuditInfo.Visible = false;
        Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}

    private TableLayoutPanel searchLayout;
    private Label lblFilterSearchText;
    private TextBox filterSearchText;
    private Label lblFilterStatus;
    private ComboBox filterStatus;
    private Label lblFilterCountryId;
    private ComboBox filterCountryId;
    private Button btnSearch;
    private DataGridView dgvRecords;
    private DataGridViewTextBoxColumn colCode;
    private DataGridViewTextBoxColumn colArabicName;
    private DataGridViewTextBoxColumn colEnglishName;
    private DataGridViewTextBoxColumn colStatus;
}
