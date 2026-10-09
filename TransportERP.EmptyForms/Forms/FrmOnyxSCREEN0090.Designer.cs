namespace TransportERP.EmptyForms;
partial class UcOnyxSCREEN0090
{
    private TransportERP.Desktop.CoreUI.AuditMetadataControl standardAuditMetadata = null!;

    private TransportERP.Desktop.CoreUI.DesignerCommandBar designerCommandBar = null!;
    private Panel designerCloseHost = null!;
    private Button standardCommandAdd = null!;
    private Button standardCommandEdit = null!;
    private Button standardCommandDelete = null!;
    private Button standardCommandCancel = null!;
    private Button standardCommandView = null!;
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
    private Label lblDataStatus = null!;
    private Panel pnlContent = null!;
    private TableLayoutPanel contentLayout = null!;
    private TableLayoutPanel fieldsLayout = null!;
    private DataGridView dgvRecords = null!;
    private Button btnT03_E0132 = null!;
    private Button btnT03_E0133 = null!;
    private Button btnClose = null!;
    private DataGridViewTextBoxColumn colT03_E0131 = null!;
    private DataGridViewComboBoxColumn colR05_AT_0180 = null!;
    private DataGridViewTextBoxColumn colR05_AT_0181 = null!;
    private DataGridViewComboBoxColumn colR05_AT_0182 = null!;
    private DataGridViewTextBoxColumn colR05_AT_0183 = null!;
    private DataGridViewComboBoxColumn colR05_AT_0184 = null!;
    private DataGridViewTextBoxColumn colR05_AT_0185 = null!;
    private void InitializeComponent()
    {
        designerCommandBar = new TransportERP.Desktop.CoreUI.DesignerCommandBar();
        designerCloseHost = new Panel();
        standardCommandAdd = new Button();
        standardCommandEdit = new Button();
        standardCommandDelete = new Button();
        standardCommandCancel = new Button();
        standardCommandView = new Button();
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
        btnT03_E0132 = new Button();
        btnT03_E0133 = new Button();
        btnClose = new Button();
        lblDataStatus = new Label();
        pnlContent = new Panel();
        contentLayout = new TableLayoutPanel();
        fieldsLayout = new TableLayoutPanel();
        dgvRecords = new DataGridView();
        colT03_E0131 = new DataGridViewTextBoxColumn();
        colR05_AT_0180 = new DataGridViewComboBoxColumn();
        colR05_AT_0181 = new DataGridViewTextBoxColumn();
        colR05_AT_0182 = new DataGridViewComboBoxColumn();
        colR05_AT_0183 = new DataGridViewTextBoxColumn();
        colR05_AT_0184 = new DataGridViewComboBoxColumn();
        colR05_AT_0185 = new DataGridViewTextBoxColumn();
        mainLayout.SuspendLayout();
        pnlHeader.SuspendLayout();
        pnlToolbar.SuspendLayout();
        pnlContent.SuspendLayout();
        contentLayout.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvRecords).BeginInit();
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
        mainLayout.Name = "mainLayout";
        mainLayout.RowCount = 3;
        mainLayout.RowStyles.Add(new RowStyle());
        mainLayout.RowStyles.Add(new RowStyle());
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        mainLayout.Size = new Size(1100, 720);
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
        pnlHeader.Location = new Point(3, 3);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.RowCount = 2;
        pnlHeader.RowStyles.Add(new RowStyle());
        pnlHeader.RowStyles.Add(new RowStyle());
        pnlHeader.Size = new Size(1094, 81);
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
        lblTitle.Size = new Size(1088, 37);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "ربط الحسابات بالأنواع الضريبية";
        // 
        // pnlToolbar
        // 
        pnlToolbar.AutoSize = true;
        pnlToolbar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        pnlToolbar.Controls.Add(btnT03_E0132);

        pnlToolbar.Dock = DockStyle.Top;
        pnlToolbar.Location = new Point(3, 40);
        pnlToolbar.Name = "pnlToolbar";
        pnlToolbar.RightToLeft = RightToLeft.Yes;
        pnlToolbar.Size = new Size(1088, 38);
        pnlToolbar.TabIndex = 1;
        // 
        // btnT03_E0132
        // 
        btnT03_E0132.AccessibleDescription = "إجراء موثق في المرجع؛ لم يتم ربط الخدمة والتنفيذ.";
        btnT03_E0132.Enabled = false;
        btnT03_E0132.Location = new Point(1014, 4);
        btnT03_E0132.Margin = new Padding(4);
        btnT03_E0132.Name = "btnT03_E0132";
        btnT03_E0132.Size = new Size(70, 30);
        btnT03_E0132.TabIndex = 0;
        btnT03_E0132.Tag = "T03-E0132";
        btnT03_E0132.Text = "إنزال البيانات";
        btnT03_E0132.Click += LoadButtonClick;
        // 
        // btnT03_E0133
        // 
        btnT03_E0133.AccessibleDescription = "إجراء موثق في المرجع؛ لم يتم ربط الخدمة والتنفيذ.";
        btnT03_E0133.Enabled = false;
        btnT03_E0133.Location = new Point(936, 4);
        btnT03_E0133.Margin = new Padding(4);
        btnT03_E0133.Name = "btnT03_E0133";
        btnT03_E0133.Size = new Size(70, 30);
        btnT03_E0133.TabIndex = 1;
        btnT03_E0133.Tag = "T03-E0133";
        btnT03_E0133.Text = "حفظ";
        btnT03_E0133.Click += SaveButtonClick;
        // 
        // btnClose
        // 
        btnClose.CausesValidation = false;
        btnClose.Location = new Point(858, 4);
        btnClose.Margin = new Padding(4);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(70, 30);
        btnClose.TabIndex = 2;
        btnClose.Text = "إغلاق";
        btnClose.Click += BtnClose_Click;
        // 
        // lblDataStatus
        // 
        lblDataStatus.AutoSize = true;
        lblDataStatus.Dock = DockStyle.Top;
        lblDataStatus.Location = new Point(3, 87);
        lblDataStatus.Name = "lblDataStatus";
        lblDataStatus.Size = new Size(1094, 23);
        lblDataStatus.TabIndex = 1;
        lblDataStatus.Text = "الواجهة جاهزة للربط — لم يتم تحميل بيانات أو توصيل إجراءات الخدمات.";
        // 
        // pnlContent
        // 
        pnlContent.AutoScroll = true;
        pnlContent.Controls.Add(contentLayout);
        pnlContent.Dock = DockStyle.Fill;
        pnlContent.Location = new Point(3, 113);
        pnlContent.Name = "pnlContent";
        pnlContent.Padding = new Padding(8);
        pnlContent.Size = new Size(1094, 604);
        pnlContent.TabIndex = 1;
        pnlContent.SizeChanged += ContentSizeChanged;
        // 
        // contentLayout
        // 
        contentLayout.ColumnCount = 1;
        contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        contentLayout.Controls.Add(fieldsLayout, 0, 0);
        contentLayout.Controls.Add(dgvRecords, 0, 1);
        contentLayout.Location = new Point(0, 0);
        contentLayout.Margin = new Padding(0);
        contentLayout.MinimumSize = new Size(720, 0);
        contentLayout.Name = "contentLayout";
        contentLayout.RowCount = 2;
        contentLayout.RowStyles.Add(new RowStyle());
        contentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        contentLayout.Size = new Size(720, 100);
        contentLayout.TabIndex = 0;
        // 
        // fieldsLayout
        // 
        fieldsLayout.AutoSize = true;
        fieldsLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        fieldsLayout.ColumnCount = 4;
        fieldsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 175F));
        fieldsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        fieldsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 175F));
        fieldsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        fieldsLayout.Dock = DockStyle.Top;
        fieldsLayout.Location = new Point(0, 0);
        fieldsLayout.Margin = new Padding(0);
        fieldsLayout.Name = "fieldsLayout";
        fieldsLayout.Size = new Size(720, 0);
        fieldsLayout.TabIndex = 0;
        fieldsLayout.Visible = false;
        // 
        // dgvRecords
        // 
        dgvRecords.AccessibleName = "بيانات مرجعية — لا توجد بيانات محملة";
        dgvRecords.AllowUserToAddRows = false;
        dgvRecords.AllowUserToDeleteRows = false;
        dgvRecords.ColumnHeadersHeight = 29;
        dgvRecords.Columns.AddRange(new DataGridViewColumn[] { colT03_E0131, colR05_AT_0180, colR05_AT_0181, colR05_AT_0182, colR05_AT_0183, colR05_AT_0184, colR05_AT_0185 });
        dgvRecords.Dock = DockStyle.Fill;
        dgvRecords.Location = new Point(3, 3);
        dgvRecords.MinimumSize = new Size(0, 200);
        dgvRecords.MultiSelect = false;
        dgvRecords.Name = "dgvRecords";
        dgvRecords.RowHeadersVisible = false;
        dgvRecords.RowHeadersWidth = 51;
        dgvRecords.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvRecords.Size = new Size(714, 200);
        dgvRecords.TabIndex = 1;
        dgvRecords.CellValueChanged += GridValueChanged;
        dgvRecords.CurrentCellDirtyStateChanged += GridDirtyStateChanged;
        dgvRecords.DataError += GridDataError;
        // 
        // colT03_E0131
        // 
        colT03_E0131.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        colT03_E0131.HeaderText = "نسبة الضريبة";
        colT03_E0131.MinimumWidth = 90;
        colT03_E0131.Name = "colT03_E0131";
        colT03_E0131.ReadOnly = true;
        colT03_E0131.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // colR05_AT_0180
        // 
        colR05_AT_0180.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        colR05_AT_0180.HeaderText = "رقم الحساب";
        colR05_AT_0180.MinimumWidth = 90;
        colR05_AT_0180.Name = "colR05_AT_0180";
        // 
        // colR05_AT_0181
        // 
        colR05_AT_0181.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        colR05_AT_0181.HeaderText = "اسم الحساب";
        colR05_AT_0181.MinimumWidth = 90;
        colR05_AT_0181.Name = "colR05_AT_0181";
        colR05_AT_0181.ReadOnly = true;
        colR05_AT_0181.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // colR05_AT_0182
        // 
        colR05_AT_0182.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        colR05_AT_0182.HeaderText = "رقم الضريبة";
        colR05_AT_0182.MinimumWidth = 90;
        colR05_AT_0182.Name = "colR05_AT_0182";
        // 
        // colR05_AT_0183
        // 
        colR05_AT_0183.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        colR05_AT_0183.HeaderText = "اسم الضريبة";
        colR05_AT_0183.MinimumWidth = 90;
        colR05_AT_0183.Name = "colR05_AT_0183";
        colR05_AT_0183.ReadOnly = true;
        colR05_AT_0183.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // colR05_AT_0184
        // 
        colR05_AT_0184.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        colR05_AT_0184.HeaderText = "رقم الجهة";
        colR05_AT_0184.MinimumWidth = 90;
        colR05_AT_0184.Name = "colR05_AT_0184";
        // 
        // colR05_AT_0185
        // 
        colR05_AT_0185.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        colR05_AT_0185.HeaderText = "اسم الجهة";
        colR05_AT_0185.MinimumWidth = 90;
        colR05_AT_0185.Name = "colR05_AT_0185";
        colR05_AT_0185.ReadOnly = true;
        colR05_AT_0185.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // UcOnyxSCREEN0090
        // 
        AutoScaleDimensions = new SizeF(120F, 120F);
        AutoScaleMode = AutoScaleMode.Dpi;
        AutoValidate = AutoValidate.EnableAllowFocusChange;
        Controls.Add(mainLayout);
        Font = new Font("Segoe UI", 10F);
        Name = "UcOnyxSCREEN0090";
        RightToLeft = RightToLeft.Yes;
        Size = new Size(1100, 720);
        Tag = "ONYX:SCREEN-0090";
        mainLayout.ResumeLayout(false);
        mainLayout.PerformLayout();
        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        pnlToolbar.ResumeLayout(false);
        pnlContent.ResumeLayout(false);
        contentLayout.ResumeLayout(false);
        contentLayout.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvRecords).EndInit();
        ResumeLayout(false);
    
        // Explicit Designer-owned command catalog.
        designerCommandBar.Dock = DockStyle.Top;
        designerCommandBar.Location = new Point(3, 40);
        designerCommandBar.Size = new Size(1088, 38);
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
        standardCommandAdd.Name = "standardCommandAdd";
        standardCommandAdd.Enabled = false;
        standardCommandAdd.Visible = true;
        standardCommandAdd.AccessibleName = "إضافة";
        standardCommandEdit.Name = "standardCommandEdit";
        standardCommandEdit.Enabled = false;
        standardCommandEdit.Visible = true;
        standardCommandEdit.AccessibleName = "تعديل";
        standardCommandDelete.Name = "standardCommandDelete";
        standardCommandDelete.Enabled = false;
        standardCommandDelete.Visible = true;
        standardCommandDelete.AccessibleName = "حذف";
        standardCommandCancel.Name = "standardCommandCancel";
        standardCommandCancel.Enabled = false;
        standardCommandCancel.Visible = true;
        standardCommandCancel.AccessibleName = "تراجع";
        standardCommandView.Name = "standardCommandView";
        standardCommandView.Enabled = false;
        standardCommandView.Visible = true;
        standardCommandView.AccessibleName = "عرض";
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
        standardCommandAdd.AutoSize = false;
        standardCommandAdd.Dock = DockStyle.None;
        standardCommandAdd.MinimumSize = Size.Empty;
        standardCommandAdd.Size = new Size(26, 24);
        standardCommandAdd.Margin = new Padding(1);
        standardCommandAdd.FlatStyle = FlatStyle.Flat;
        standardCommandAdd.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandAdd.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Add;
        standardCommandAdd.Text = "";
        pnlToolbar.Controls.Add(standardCommandAdd);
        designerCommandBar.SetCommandRole(standardCommandAdd, TransportERP.Desktop.CoreUI.DesignerCommandRole.Add);
        standardCommandEdit.AutoSize = false;
        standardCommandEdit.Dock = DockStyle.None;
        standardCommandEdit.MinimumSize = Size.Empty;
        standardCommandEdit.Size = new Size(26, 24);
        standardCommandEdit.Margin = new Padding(1);
        standardCommandEdit.FlatStyle = FlatStyle.Flat;
        standardCommandEdit.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandEdit.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Edit;
        standardCommandEdit.Text = "";
        pnlToolbar.Controls.Add(standardCommandEdit);
        designerCommandBar.SetCommandRole(standardCommandEdit, TransportERP.Desktop.CoreUI.DesignerCommandRole.Edit);
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
        standardCommandView.AutoSize = false;
        standardCommandView.Dock = DockStyle.None;
        standardCommandView.MinimumSize = Size.Empty;
        standardCommandView.Size = new Size(26, 24);
        standardCommandView.Margin = new Padding(1);
        standardCommandView.FlatStyle = FlatStyle.Flat;
        standardCommandView.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandView.Image = TransportERP.Desktop.CoreUI.CommandBarImages.View;
        standardCommandView.Text = "";
        pnlToolbar.Controls.Add(standardCommandView);
        designerCommandBar.SetCommandRole(standardCommandView, TransportERP.Desktop.CoreUI.DesignerCommandRole.View);
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
        btnT03_E0133.AutoSize = false;
        btnT03_E0133.Dock = DockStyle.None;
        btnT03_E0133.MinimumSize = Size.Empty;
        btnT03_E0133.Size = new Size(26, 24);
        btnT03_E0133.Margin = new Padding(1);
        btnT03_E0133.FlatStyle = FlatStyle.Flat;
        btnT03_E0133.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnT03_E0133.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Save;
        btnT03_E0133.Text = "";
        pnlToolbar.Controls.Add(btnT03_E0133);
        designerCommandBar.SetCommandRole(btnT03_E0133, TransportERP.Desktop.CoreUI.DesignerCommandRole.Save);
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
        Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}
}
