namespace TransportERP.EmptyForms;
partial class UcOnyxSCREEN0085
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
    private Button btnClose = null!;
    private Label lblDataStatus = null!;
    private Panel pnlContent = null!;
    private TableLayoutPanel fieldsLayout = null!;
    private Button btn_T03_E0063 = null!;
    private TextBox field_T03_E0059 = null!;
    private Label lbl_T03_E0059 = null!;
    private ComboBox field_T03_E0061 = null!;
    private Label lbl_T03_E0061 = null!;
    private TextBox field_R05_AT_0155 = null!;
    private Label lbl_R05_AT_0155 = null!;
    private TextBox field_R05_AT_0156 = null!;
    private Label lbl_R05_AT_0156 = null!;
    private Button btnAccountLookup = null!;
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
        btn_T03_E0063 = new Button();
        btnClose = new Button();
        btnAccountLookup = new Button();
        btnClearForm = new Button();
        lblDataStatus = new Label();
        pnlContent = new Panel();
        fieldsLayout = new TableLayoutPanel();
        lbl_T03_E0059 = new Label();
        field_T03_E0059 = new TextBox();
        lbl_T03_E0061 = new Label();
        field_T03_E0061 = new ComboBox();
        lbl_R05_AT_0155 = new Label();
        field_R05_AT_0155 = new TextBox();
        lbl_R05_AT_0156 = new Label();
        field_R05_AT_0156 = new TextBox();
        mainLayout.SuspendLayout();
        pnlHeader.SuspendLayout();
        pnlToolbar.SuspendLayout();
        pnlContent.SuspendLayout();
        fieldsLayout.SuspendLayout();
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
        pnlHeader.Location = new Point(3, 3);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.RowCount = 2;
        pnlHeader.RowStyles.Add(new RowStyle());
        pnlHeader.RowStyles.Add(new RowStyle());
        pnlHeader.Size = new Size(1094, 160);
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
        lblTitle.Text = "ترميز البيان";
        // 
        // pnlToolbar
        // 
        pnlToolbar.AutoSize = true;
        pnlToolbar.AutoSizeMode = AutoSizeMode.GrowAndShrink;

        pnlToolbar.Controls.Add(btnAccountLookup);
        pnlToolbar.Controls.Add(btnClearForm);
        pnlToolbar.Dock = DockStyle.Top;
        pnlToolbar.Location = new Point(3, 40);
        pnlToolbar.Name = "pnlToolbar";
        pnlToolbar.RightToLeft = RightToLeft.Yes;
        pnlToolbar.Size = new Size(1088, 39);
        pnlToolbar.TabIndex = 1;
        // 
        // btn_T03_E0063
        // 
        btn_T03_E0063.AutoSize = true;
        btn_T03_E0063.Enabled = false;
        btn_T03_E0063.Location = new Point(1015, 3);
        btn_T03_E0063.Name = "btn_T03_E0063";
        btn_T03_E0063.Size = new Size(70, 33);
        btn_T03_E0063.TabIndex = 0;
        btn_T03_E0063.Tag = "T03-E0063";
        btn_T03_E0063.Text = "حفظ";
        btn_T03_E0063.Click += SaveButton_Click;
        // 
        // btnClose
        // 
        btnClose.AutoSize = true;
        btnClose.Location = new Point(939, 3);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(70, 33);
        btnClose.TabIndex = 1;
        btnClose.Text = "إغلاق";
        btnClose.Click += BtnClose_Click;
        // 
        // btnAccountLookup
        // 
        btnAccountLookup.AutoSize = true;
        btnAccountLookup.Enabled = false;
        btnAccountLookup.Location = new Point(778, 3);
        btnAccountLookup.Name = "btnAccountLookup";
        btnAccountLookup.Size = new Size(155, 33);
        btnAccountLookup.TabIndex = 2;
        btnAccountLookup.Tag = "T03-E0062";
        btnAccountLookup.Text = "F9 — بحث الحساب";
        btnAccountLookup.Click += LookupButton_Click;
        // 
        // btnClearForm
        // 
        btnClearForm.AutoSize = true;
        btnClearForm.Location = new Point(654, 3);
        btnClearForm.Name = "btnClearForm";
        btnClearForm.Size = new Size(118, 33);
        btnClearForm.TabIndex = 20;
        btnClearForm.Text = "تفريغ النموذج";
        btnClearForm.Click += ClearForm_Click;
        // 
        // lblDataStatus
        // 
        lblDataStatus.AutoSize = true;
        lblDataStatus.Dock = DockStyle.Top;
        lblDataStatus.Location = new Point(3, 166);
        lblDataStatus.Name = "lblDataStatus";
        lblDataStatus.Size = new Size(1094, 23);
        lblDataStatus.TabIndex = 1;
        lblDataStatus.Text = "لا تتوفر البيانات حاليًا. الحفظ غير متاح.";
        // 
        // pnlContent
        // 
        pnlContent.AutoScroll = true;
        pnlContent.Controls.Add(fieldsLayout);
        pnlContent.Dock = DockStyle.Fill;
        pnlContent.Location = new Point(3, 192);
        pnlContent.Name = "pnlContent";
        pnlContent.Padding = new Padding(8);
        pnlContent.Size = new Size(1094, 565);
        pnlContent.TabIndex = 2;
        // 
        // fieldsLayout
        // 
        fieldsLayout.AutoSize = true;
        fieldsLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        fieldsLayout.ColumnCount = 2;
        fieldsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
        fieldsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
        fieldsLayout.Controls.Add(lbl_T03_E0059, 0, 0);
        fieldsLayout.Controls.Add(field_T03_E0059, 1, 0);
        fieldsLayout.Controls.Add(lbl_T03_E0061, 0, 1);
        fieldsLayout.Controls.Add(field_T03_E0061, 1, 1);
        fieldsLayout.Controls.Add(lbl_R05_AT_0155, 0, 2);
        fieldsLayout.Controls.Add(field_R05_AT_0155, 1, 2);
        fieldsLayout.Controls.Add(lbl_R05_AT_0156, 0, 3);
        fieldsLayout.Controls.Add(field_R05_AT_0156, 1, 3);
        fieldsLayout.Dock = DockStyle.Top;
        fieldsLayout.Location = new Point(8, 8);
        fieldsLayout.MinimumSize = new Size(740, 0);
        fieldsLayout.Name = "fieldsLayout";
        fieldsLayout.RowCount = 4;
        fieldsLayout.RowStyles.Add(new RowStyle());
        fieldsLayout.RowStyles.Add(new RowStyle());
        fieldsLayout.RowStyles.Add(new RowStyle());
        fieldsLayout.RowStyles.Add(new RowStyle());
        fieldsLayout.Size = new Size(1078, 302);
        fieldsLayout.TabIndex = 0;
        // 
        // lbl_T03_E0059
        // 
        lbl_T03_E0059.AutoSize = true;
        lbl_T03_E0059.Dock = DockStyle.Fill;
        lbl_T03_E0059.Location = new Point(492, 8);
        lbl_T03_E0059.Margin = new Padding(6, 8, 6, 8);
        lbl_T03_E0059.Name = "lbl_T03_E0059";
        lbl_T03_E0059.Size = new Size(580, 30);
        lbl_T03_E0059.TabIndex = 0;
        lbl_T03_E0059.Text = "رقم البيان";
        lbl_T03_E0059.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_T03_E0059
        // 
        field_T03_E0059.AccessibleDescription = "تعديلات النموذج محلية؛ لم يتم ربط القراءة والحفظ بالخدمة.";
        field_T03_E0059.AccessibleName = "رقم البيان";
        field_T03_E0059.Dock = DockStyle.Fill;
        field_T03_E0059.Location = new Point(6, 8);
        field_T03_E0059.Margin = new Padding(6, 8, 6, 8);
        field_T03_E0059.Name = "field_T03_E0059";
        field_T03_E0059.Size = new Size(474, 30);
        field_T03_E0059.TabIndex = 0;
        field_T03_E0059.Tag = "T03-E0059";
        // 
        // lbl_T03_E0061
        // 
        lbl_T03_E0061.AutoSize = true;
        lbl_T03_E0061.Dock = DockStyle.Fill;
        lbl_T03_E0061.Location = new Point(492, 54);
        lbl_T03_E0061.Margin = new Padding(6, 8, 6, 8);
        lbl_T03_E0061.Name = "lbl_T03_E0061";
        lbl_T03_E0061.Size = new Size(580, 28);
        lbl_T03_E0061.TabIndex = 1;
        lbl_T03_E0061.Text = "رقم الحساب";
        lbl_T03_E0061.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_T03_E0061
        // 
        field_T03_E0061.AccessibleDescription = "تعديلات النموذج محلية؛ لم يتم ربط القراءة والحفظ بالخدمة.";
        field_T03_E0061.AccessibleName = "رقم الحساب";
        field_T03_E0061.Dock = DockStyle.Fill;
        field_T03_E0061.DropDownStyle = ComboBoxStyle.DropDownList;
        field_T03_E0061.Location = new Point(6, 54);
        field_T03_E0061.Margin = new Padding(6, 8, 6, 8);
        field_T03_E0061.Name = "field_T03_E0061";
        field_T03_E0061.Size = new Size(474, 31);
        field_T03_E0061.TabIndex = 1;
        field_T03_E0061.Tag = "T03-E0061";
        // 
        // lbl_R05_AT_0155
        // 
        lbl_R05_AT_0155.AutoSize = true;
        lbl_R05_AT_0155.Dock = DockStyle.Fill;
        lbl_R05_AT_0155.Location = new Point(492, 98);
        lbl_R05_AT_0155.Margin = new Padding(6, 8, 6, 8);
        lbl_R05_AT_0155.Name = "lbl_R05_AT_0155";
        lbl_R05_AT_0155.Size = new Size(580, 90);
        lbl_R05_AT_0155.TabIndex = 2;
        lbl_R05_AT_0155.Text = "نص البيان المحلي";
        lbl_R05_AT_0155.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_R05_AT_0155
        // 
        field_R05_AT_0155.AccessibleDescription = "تعديلات النموذج محلية؛ لم يتم ربط القراءة والحفظ بالخدمة.";
        field_R05_AT_0155.AccessibleName = "نص البيان المحلي";
        field_R05_AT_0155.Dock = DockStyle.Fill;
        field_R05_AT_0155.Location = new Point(6, 98);
        field_R05_AT_0155.Margin = new Padding(6, 8, 6, 8);
        field_R05_AT_0155.MinimumSize = new Size(0, 90);
        field_R05_AT_0155.Multiline = true;
        field_R05_AT_0155.Name = "field_R05_AT_0155";
        field_R05_AT_0155.ScrollBars = ScrollBars.Vertical;
        field_R05_AT_0155.Size = new Size(474, 90);
        field_R05_AT_0155.TabIndex = 2;
        field_R05_AT_0155.Tag = "R05-AT-0155";
        // 
        // lbl_R05_AT_0156
        // 
        lbl_R05_AT_0156.AutoSize = true;
        lbl_R05_AT_0156.Dock = DockStyle.Fill;
        lbl_R05_AT_0156.Location = new Point(492, 204);
        lbl_R05_AT_0156.Margin = new Padding(6, 8, 6, 8);
        lbl_R05_AT_0156.Name = "lbl_R05_AT_0156";
        lbl_R05_AT_0156.Size = new Size(580, 90);
        lbl_R05_AT_0156.TabIndex = 3;
        lbl_R05_AT_0156.Text = "نص البيان الأجنبي";
        lbl_R05_AT_0156.TextAlign = ContentAlignment.MiddleRight;
        // 
        // field_R05_AT_0156
        // 
        field_R05_AT_0156.AccessibleDescription = "تعديلات النموذج محلية؛ لم يتم ربط القراءة والحفظ بالخدمة.";
        field_R05_AT_0156.AccessibleName = "نص البيان الأجنبي";
        field_R05_AT_0156.Dock = DockStyle.Fill;
        field_R05_AT_0156.Location = new Point(6, 204);
        field_R05_AT_0156.Margin = new Padding(6, 8, 6, 8);
        field_R05_AT_0156.MinimumSize = new Size(0, 90);
        field_R05_AT_0156.Multiline = true;
        field_R05_AT_0156.Name = "field_R05_AT_0156";
        field_R05_AT_0156.RightToLeft = RightToLeft.No;
        field_R05_AT_0156.ScrollBars = ScrollBars.Vertical;
        field_R05_AT_0156.Size = new Size(474, 90);
        field_R05_AT_0156.TabIndex = 3;
        field_R05_AT_0156.Tag = "R05-AT-0156";
        // 
        // UcOnyxSCREEN0085
        // 
        AutoScaleDimensions = new SizeF(120F, 120F);
        AutoScaleMode = AutoScaleMode.Dpi;
        Controls.Add(mainLayout);
        Font = new Font("Segoe UI", 10F);
        Name = "UcOnyxSCREEN0085";
        RightToLeft = RightToLeft.Yes;
        Size = new Size(1100, 760);
        Tag = "ONYX.SCREEN-0085";
        mainLayout.ResumeLayout(false);
        mainLayout.PerformLayout();
        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        pnlToolbar.ResumeLayout(false);
        pnlToolbar.PerformLayout();
        pnlContent.ResumeLayout(false);
        pnlContent.PerformLayout();
        fieldsLayout.ResumeLayout(false);
        fieldsLayout.PerformLayout();
        ResumeLayout(false);
    
        // Explicit Designer-owned command catalog.
        designerCommandBar.Dock = DockStyle.Top;
        designerCommandBar.Location = new Point(3, 40);
        designerCommandBar.Size = new Size(1088, 39);
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
        btn_T03_E0063.AutoSize = false;
        btn_T03_E0063.Dock = DockStyle.None;
        btn_T03_E0063.MinimumSize = Size.Empty;
        btn_T03_E0063.Size = new Size(26, 24);
        btn_T03_E0063.Margin = new Padding(1);
        btn_T03_E0063.FlatStyle = FlatStyle.Flat;
        btn_T03_E0063.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btn_T03_E0063.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Save;
        btn_T03_E0063.Text = "";
        pnlToolbar.Controls.Add(btn_T03_E0063);
        designerCommandBar.SetCommandRole(btn_T03_E0063, TransportERP.Desktop.CoreUI.DesignerCommandRole.Save);
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

    private Button btnClearForm = null!;
}
