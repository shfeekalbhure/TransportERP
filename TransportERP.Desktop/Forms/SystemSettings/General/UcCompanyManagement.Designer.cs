#nullable enable
using System.Drawing;
using System.Windows.Forms;

namespace TransportERP.Desktop.Forms.SystemSettings.General;

partial class UcCompanyManagement
{
    private TransportERP.Desktop.CoreUI.AuditMetadataControl standardAuditMetadata = null!;

    private TransportERP.Desktop.CoreUI.DesignerCommandBar designerCommandBar = null!;
    private FlowLayoutPanel designerHiddenCommands = null!;
    private Button standardCommandImport = null!;
    private Button standardCommandExport = null!;
    private Button standardCommandHelp = null!;

    private System.ComponentModel.IContainer? components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        designerCommandBar = new TransportERP.Desktop.CoreUI.DesignerCommandBar();
        designerHiddenCommands = new FlowLayoutPanel();
        standardCommandImport = new Button();
        standardCommandExport = new Button();
        standardCommandHelp = new Button();
        pnlHeader = new Panel();
        pnlToolbar = new FlowLayoutPanel();
        button1 = new Button();
        btnSave = new Button();
        btnEdit = new Button();
        btnDelete = new Button();
        button2 = new Button();
        btnClose = new Button();
        btnFirst = new Button();
        btnPrevious = new Button();
        txtCurrentRecordNo = new TextBox();
        btnNext = new Button();
        btnLast = new Button();
        btnUndo = new Button();
        btnSearch = new Button();
        btnPrint = new Button();
        button4 = new Button();
        tlpAuditInfo = new TableLayoutPanel();
        lblPrintCount = new Label();
        lblLastPrintedAt = new Label();
        lblEditCount = new Label();
        lblModifiedAt = new Label();
        lblModifiedBy = new Label();
        lblCreatedAt = new Label();
        lblCreatedBy = new Label();
        tabStatus = new TabPage();
        tabBasicData = new TabPage();
        tabCompanies = new TabPage();
        tabCompaniesSection = new GroupBox();
        tabCompaniesFields = new TableLayoutPanel();
        txtAddress = new TextBox();
        label5 = new Label();
        label9 = new Label();
        cmbCity = new ComboBox();
        cmbCountry = new ComboBox();
        txtNotes = new TextBox();
        txtPostalCode = new TextBox();
        txtWebsite = new TextBox();
        txtEmail = new TextBox();
        txtMobile = new TextBox();
        txtPhone = new TextBox();
        label24 = new Label();
        label22 = new Label();
        label20 = new Label();
        label18 = new Label();
        label15 = new Label();
        label11 = new Label();
        label3 = new Label();
        txtCommercialRegistrationNo = new TextBox();
        cmbGovernorate = new ComboBox();
        cmbBaseCurrency = new ComboBox();
        txtTaxNumber = new TextBox();
        cmbActivityType = new ComboBox();
        cmbCompanyGroup = new ComboBox();
        label2 = new Label();
        txtCompanyNameEn = new TextBox();
        lblCompanyNameEn = new Label();
        txtCompanyId = new TextBox();
        label6 = new Label();
        label16 = new Label();
        label14 = new Label();
        label12 = new Label();
        label8 = new Label();
        label4 = new Label();
        txtCompanyCode = new TextBox();
        label1 = new Label();
        lblCompanyId = new Label();
        lblCompanyNameAr = new Label();
        txtCompanyNameAr = new TextBox();
        chkIsActive = new CheckBox();
        groupBox1 = new GroupBox();
        tableLayoutPanel1 = new TableLayoutPanel();
        tableLayoutPanel2 = new TableLayoutPanel();
        btnAddLogo = new Button();
        btnRemoveLogo = new Button();
        picCompanyLogo = new PictureBox();
        tabCompanyManagement = new TabControl();
        pnlHeader.SuspendLayout();
        pnlToolbar.SuspendLayout();
        tlpAuditInfo.SuspendLayout();
        tabCompanies.SuspendLayout();
        tabCompaniesSection.SuspendLayout();
        tabCompaniesFields.SuspendLayout();
        groupBox1.SuspendLayout();
        tableLayoutPanel1.SuspendLayout();
        tableLayoutPanel2.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)picCompanyLogo).BeginInit();
        tabCompanyManagement.SuspendLayout();
        SuspendLayout();
        // 
        // pnlHeader
        // 
        pnlHeader.AutoSize = true;
        pnlHeader.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        pnlHeader.BackColor = Color.FromArgb(224, 224, 224);
        pnlHeader.Controls.Add(designerCommandBar);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Location = new Point(0, 0);
        pnlHeader.MinimumSize = new Size(0, 48);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Size = new Size(1654, 48);
        pnlHeader.TabIndex = 1;
        // 
        // pnlToolbar
        // 
        pnlToolbar.AutoSize = true;
        pnlToolbar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        pnlToolbar.BackColor = Color.FromArgb(224, 224, 224);
        pnlToolbar.Controls.Add(button1);
        pnlToolbar.Controls.Add(btnSave);
        pnlToolbar.Controls.Add(btnEdit);
        pnlToolbar.Controls.Add(btnDelete);
        pnlToolbar.Controls.Add(button2);
        pnlToolbar.Controls.Add(btnClose);
        pnlToolbar.Controls.Add(btnFirst);
        pnlToolbar.Controls.Add(btnPrevious);
        pnlToolbar.Controls.Add(txtCurrentRecordNo);
        pnlToolbar.Controls.Add(btnNext);
        pnlToolbar.Controls.Add(btnLast);
        pnlToolbar.Controls.Add(btnUndo);
        pnlToolbar.Controls.Add(btnSearch);
        pnlToolbar.Controls.Add(btnPrint);
        pnlToolbar.Controls.Add(button4);
        pnlToolbar.Dock = DockStyle.Top;
        pnlToolbar.Location = new Point(0, 0);
        pnlToolbar.Margin = new Padding(5);
        pnlToolbar.MinimumSize = new Size(0, 48);
        pnlToolbar.Name = "pnlToolbar";
        pnlToolbar.Padding = new Padding(5);
        pnlToolbar.RightToLeft = RightToLeft.Yes;
        pnlToolbar.Size = new Size(1654, 48);
        pnlToolbar.TabIndex = 3;
        // 
        // button1
        // 
        button1.AutoEllipsis = true;
        button1.BackColor = Color.FromArgb(224, 224, 224);
        button1.FlatStyle = FlatStyle.Flat;
        button1.Font = new Font("Tahoma", 9F);
        button1.ForeColor = Color.FromArgb(16, 24, 40);
        button1.Location = new Point(1570, 9);
        button1.Margin = new Padding(4);
        button1.Name = "button1";
        button1.Size = new Size(70, 30);
        button1.TabIndex = 0;
        button1.Text = "جديد";
        button1.UseVisualStyleBackColor = false;
        // 
        // btnSave
        // 
        btnSave.AutoEllipsis = true;
        btnSave.BackColor = Color.FromArgb(224, 224, 224);
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Font = new Font("Tahoma", 9F);
        btnSave.ForeColor = Color.FromArgb(16, 24, 40);
        btnSave.Location = new Point(1492, 9);
        btnSave.Margin = new Padding(4);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(70, 30);
        btnSave.TabIndex = 1;
        btnSave.Text = "حفظ";
        btnSave.UseVisualStyleBackColor = false;
        // 
        // btnEdit
        // 
        btnEdit.AutoEllipsis = true;
        btnEdit.BackColor = Color.FromArgb(224, 224, 224);
        btnEdit.FlatStyle = FlatStyle.Flat;
        btnEdit.Font = new Font("Tahoma", 9F);
        btnEdit.ForeColor = Color.FromArgb(16, 24, 40);
        btnEdit.Location = new Point(1414, 9);
        btnEdit.Margin = new Padding(4);
        btnEdit.Name = "btnEdit";
        btnEdit.Size = new Size(70, 30);
        btnEdit.TabIndex = 2;
        btnEdit.Text = "تعديل";
        btnEdit.UseVisualStyleBackColor = false;
        // 
        // btnDelete
        // 
        btnDelete.AutoEllipsis = true;
        btnDelete.BackColor = Color.FromArgb(224, 224, 224);
        btnDelete.FlatStyle = FlatStyle.Flat;
        btnDelete.Font = new Font("Tahoma", 9F);
        btnDelete.ForeColor = Color.FromArgb(16, 24, 40);
        btnDelete.Location = new Point(1336, 9);
        btnDelete.Margin = new Padding(4);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(70, 30);
        btnDelete.TabIndex = 3;
        btnDelete.Text = "حذف";
        btnDelete.UseVisualStyleBackColor = false;
        // 
        // button2
        // 
        button2.AutoEllipsis = true;
        button2.BackColor = Color.FromArgb(224, 224, 224);
        button2.FlatStyle = FlatStyle.Flat;
        button2.Font = new Font("Tahoma", 9F);
        button2.ForeColor = Color.FromArgb(16, 24, 40);
        button2.Location = new Point(1258, 9);
        button2.Margin = new Padding(4);
        button2.Name = "button2";
        button2.Size = new Size(70, 30);
        button2.TabIndex = 4;
        button2.Text = "تحديث";
        button2.UseVisualStyleBackColor = false;
        // 
        // btnClose
        // 
        btnClose.AutoEllipsis = true;
        btnClose.BackColor = Color.FromArgb(224, 224, 224);
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Tahoma", 9F);
        btnClose.ForeColor = Color.FromArgb(16, 24, 40);
        btnClose.Location = new Point(1180, 9);
        btnClose.Margin = new Padding(4);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(70, 30);
        btnClose.TabIndex = 5;
        btnClose.Text = "اغلاق";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += BtnClose_Click;
        // 
        // btnFirst
        // 
        btnFirst.AutoEllipsis = true;
        btnFirst.BackColor = Color.FromArgb(224, 224, 224);
        btnFirst.FlatStyle = FlatStyle.Flat;
        btnFirst.Font = new Font("Tahoma", 9F);
        btnFirst.ForeColor = Color.FromArgb(16, 24, 40);
        btnFirst.Location = new Point(1102, 9);
        btnFirst.Margin = new Padding(4);
        btnFirst.Name = "btnFirst";
        btnFirst.Size = new Size(70, 30);
        btnFirst.TabIndex = 6;
        btnFirst.Text = "الاول ";
        btnFirst.UseVisualStyleBackColor = false;
        // 
        // btnPrevious
        // 
        btnPrevious.AutoEllipsis = true;
        btnPrevious.BackColor = Color.FromArgb(224, 224, 224);
        btnPrevious.FlatStyle = FlatStyle.Flat;
        btnPrevious.Font = new Font("Tahoma", 9F);
        btnPrevious.ForeColor = Color.FromArgb(16, 24, 40);
        btnPrevious.Location = new Point(1024, 9);
        btnPrevious.Margin = new Padding(4);
        btnPrevious.Name = "btnPrevious";
        btnPrevious.Size = new Size(70, 30);
        btnPrevious.TabIndex = 7;
        btnPrevious.Text = "السابق";
        btnPrevious.UseVisualStyleBackColor = false;
        // 
        // txtCurrentRecordNo
        // 
        txtCurrentRecordNo.BackColor = Color.White;
        txtCurrentRecordNo.Font = new Font("Tahoma", 9F);
        txtCurrentRecordNo.ForeColor = Color.FromArgb(16, 24, 40);
        txtCurrentRecordNo.Location = new Point(954, 9);
        txtCurrentRecordNo.Margin = new Padding(4);
        txtCurrentRecordNo.Multiline = true;
        txtCurrentRecordNo.Name = "txtCurrentRecordNo";
        txtCurrentRecordNo.ReadOnly = true;
        txtCurrentRecordNo.Size = new Size(62, 30);
        txtCurrentRecordNo.TabIndex = 8;
        txtCurrentRecordNo.TabStop = false;
        txtCurrentRecordNo.TextAlign = HorizontalAlignment.Center;
        // 
        // btnNext
        // 
        btnNext.AutoEllipsis = true;
        btnNext.BackColor = Color.FromArgb(224, 224, 224);
        btnNext.FlatStyle = FlatStyle.Flat;
        btnNext.Font = new Font("Tahoma", 9F);
        btnNext.ForeColor = Color.FromArgb(16, 24, 40);
        btnNext.Location = new Point(876, 9);
        btnNext.Margin = new Padding(4);
        btnNext.Name = "btnNext";
        btnNext.Size = new Size(70, 30);
        btnNext.TabIndex = 9;
        btnNext.Text = "التالي";
        btnNext.UseVisualStyleBackColor = false;
        // 
        // btnLast
        // 
        btnLast.AutoEllipsis = true;
        btnLast.BackColor = Color.FromArgb(224, 224, 224);
        btnLast.FlatStyle = FlatStyle.Flat;
        btnLast.Font = new Font("Tahoma", 9F);
        btnLast.ForeColor = Color.FromArgb(16, 24, 40);
        btnLast.Location = new Point(798, 9);
        btnLast.Margin = new Padding(4);
        btnLast.Name = "btnLast";
        btnLast.Size = new Size(70, 30);
        btnLast.TabIndex = 10;
        btnLast.Text = "الاخير";
        btnLast.UseVisualStyleBackColor = false;
        // 
        // btnUndo
        // 
        btnUndo.AutoEllipsis = true;
        btnUndo.BackColor = Color.FromArgb(224, 224, 224);
        btnUndo.FlatStyle = FlatStyle.Flat;
        btnUndo.Font = new Font("Tahoma", 9F);
        btnUndo.ForeColor = Color.FromArgb(16, 24, 40);
        btnUndo.Location = new Point(720, 9);
        btnUndo.Margin = new Padding(4);
        btnUndo.Name = "btnUndo";
        btnUndo.Size = new Size(70, 30);
        btnUndo.TabIndex = 11;
        btnUndo.Text = "تراجع";
        btnUndo.UseVisualStyleBackColor = false;
        // 
        // btnSearch
        // 
        btnSearch.AutoEllipsis = true;
        btnSearch.BackColor = Color.FromArgb(224, 224, 224);
        btnSearch.FlatStyle = FlatStyle.Flat;
        btnSearch.Font = new Font("Tahoma", 9F);
        btnSearch.ForeColor = Color.FromArgb(16, 24, 40);
        btnSearch.Location = new Point(642, 9);
        btnSearch.Margin = new Padding(4);
        btnSearch.Name = "btnSearch";
        btnSearch.Size = new Size(70, 30);
        btnSearch.TabIndex = 12;
        btnSearch.Text = "بحث";
        btnSearch.UseVisualStyleBackColor = false;
        // 
        // btnPrint
        // 
        btnPrint.AutoEllipsis = true;
        btnPrint.BackColor = Color.FromArgb(224, 224, 224);
        btnPrint.FlatStyle = FlatStyle.Flat;
        btnPrint.Font = new Font("Tahoma", 9F);
        btnPrint.ForeColor = Color.FromArgb(16, 24, 40);
        btnPrint.Location = new Point(564, 9);
        btnPrint.Margin = new Padding(4);
        btnPrint.Name = "btnPrint";
        btnPrint.Size = new Size(70, 30);
        btnPrint.TabIndex = 13;
        btnPrint.Text = "طباعة";
        btnPrint.UseVisualStyleBackColor = false;
        // 
        // button4
        // 
        button4.AutoEllipsis = true;
        button4.BackColor = Color.FromArgb(224, 224, 224);
        button4.FlatStyle = FlatStyle.Flat;
        button4.Font = new Font("Tahoma", 9F);
        button4.ForeColor = Color.FromArgb(16, 24, 40);
        button4.Location = new Point(486, 9);
        button4.Margin = new Padding(4);
        button4.Name = "button4";
        button4.Size = new Size(70, 30);
        button4.TabIndex = 14;
        button4.Text = "اعتماد";
        button4.UseVisualStyleBackColor = false;
        // 
        // tlpAuditInfo
        // 
        tlpAuditInfo.AccessibleName = "بيانات إنشاء وتعديل الشركة";
        tlpAuditInfo.BackColor = Color.FromArgb(248, 250, 252);
        tlpAuditInfo.ColumnCount = 7;
        tlpAuditInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14F));
        tlpAuditInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16F));
        tlpAuditInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14F));
        tlpAuditInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16F));
        tlpAuditInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12F));
        tlpAuditInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16F));
        tlpAuditInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12F));
        tlpAuditInfo.Controls.Add(lblPrintCount, 6, 0);
        tlpAuditInfo.Controls.Add(lblLastPrintedAt, 5, 0);
        tlpAuditInfo.Controls.Add(lblEditCount, 4, 0);
        tlpAuditInfo.Controls.Add(lblModifiedAt, 3, 0);
        tlpAuditInfo.Controls.Add(lblModifiedBy, 2, 0);
        tlpAuditInfo.Controls.Add(lblCreatedAt, 1, 0);
        tlpAuditInfo.Controls.Add(lblCreatedBy, 0, 0);
        tlpAuditInfo.Dock = DockStyle.Bottom;
        tlpAuditInfo.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
        tlpAuditInfo.Location = new Point(0, 1012);
        tlpAuditInfo.Margin = new Padding(0);
        tlpAuditInfo.Name = "tlpAuditInfo";
        tlpAuditInfo.Padding = new Padding(8);
        tlpAuditInfo.RightToLeft = RightToLeft.Yes;
        tlpAuditInfo.RowCount = 1;
        tlpAuditInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpAuditInfo.Size = new Size(1654, 48);
        tlpAuditInfo.TabIndex = 2;
        // 
        // lblPrintCount
        // 
        lblPrintCount.AutoEllipsis = true;
        lblPrintCount.BackColor = Color.FromArgb(192, 255, 255);
        lblPrintCount.Dock = DockStyle.Fill;
        lblPrintCount.Font = new Font("Tahoma", 9F);
        lblPrintCount.ForeColor = Color.FromArgb(16, 24, 40);
        lblPrintCount.Location = new Point(11, 11);
        lblPrintCount.Margin = new Padding(3);
        lblPrintCount.Name = "lblPrintCount";
        lblPrintCount.RightToLeft = RightToLeft.Yes;
        lblPrintCount.Size = new Size(192, 26);
        lblPrintCount.TabIndex = 8;
        lblPrintCount.Text = "عدد مرات الطباعة: —";
        lblPrintCount.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblLastPrintedAt
        // 
        lblLastPrintedAt.AutoEllipsis = true;
        lblLastPrintedAt.BackColor = Color.FromArgb(192, 255, 255);
        lblLastPrintedAt.Dock = DockStyle.Fill;
        lblLastPrintedAt.Font = new Font("Tahoma", 9F);
        lblLastPrintedAt.ForeColor = Color.FromArgb(16, 24, 40);
        lblLastPrintedAt.Location = new Point(209, 11);
        lblLastPrintedAt.Margin = new Padding(3);
        lblLastPrintedAt.Name = "lblLastPrintedAt";
        lblLastPrintedAt.RightToLeft = RightToLeft.Yes;
        lblLastPrintedAt.Size = new Size(256, 26);
        lblLastPrintedAt.TabIndex = 5;
        lblLastPrintedAt.Text = "تاريخ اخر طباعة: —";
        lblLastPrintedAt.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblEditCount
        // 
        lblEditCount.AutoEllipsis = true;
        lblEditCount.BackColor = Color.FromArgb(192, 255, 255);
        lblEditCount.Dock = DockStyle.Fill;
        lblEditCount.Font = new Font("Tahoma", 9F);
        lblEditCount.ForeColor = Color.FromArgb(16, 24, 40);
        lblEditCount.Location = new Point(471, 11);
        lblEditCount.Margin = new Padding(3);
        lblEditCount.Name = "lblEditCount";
        lblEditCount.RightToLeft = RightToLeft.Yes;
        lblEditCount.Size = new Size(190, 26);
        lblEditCount.TabIndex = 4;
        lblEditCount.Text = "عدد التعديلات: —";
        lblEditCount.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblModifiedAt
        // 
        lblModifiedAt.AutoEllipsis = true;
        lblModifiedAt.BackColor = Color.FromArgb(192, 255, 255);
        lblModifiedAt.Dock = DockStyle.Fill;
        lblModifiedAt.Font = new Font("Tahoma", 9F);
        lblModifiedAt.ForeColor = Color.FromArgb(16, 24, 40);
        lblModifiedAt.Location = new Point(667, 11);
        lblModifiedAt.Margin = new Padding(3);
        lblModifiedAt.Name = "lblModifiedAt";
        lblModifiedAt.RightToLeft = RightToLeft.Yes;
        lblModifiedAt.Size = new Size(256, 26);
        lblModifiedAt.TabIndex = 3;
        lblModifiedAt.Text = "تاريخ التعديل: —";
        lblModifiedAt.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblModifiedBy
        // 
        lblModifiedBy.AutoEllipsis = true;
        lblModifiedBy.BackColor = Color.FromArgb(192, 255, 255);
        lblModifiedBy.Dock = DockStyle.Fill;
        lblModifiedBy.Font = new Font("Tahoma", 9F);
        lblModifiedBy.ForeColor = Color.FromArgb(16, 24, 40);
        lblModifiedBy.Location = new Point(929, 11);
        lblModifiedBy.Margin = new Padding(3);
        lblModifiedBy.Name = "lblModifiedBy";
        lblModifiedBy.RightToLeft = RightToLeft.Yes;
        lblModifiedBy.Size = new Size(223, 26);
        lblModifiedBy.TabIndex = 2;
        lblModifiedBy.Text = "عدل بواسطة: —";
        lblModifiedBy.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblCreatedAt
        // 
        lblCreatedAt.AutoEllipsis = true;
        lblCreatedAt.BackColor = Color.FromArgb(192, 255, 255);
        lblCreatedAt.Dock = DockStyle.Fill;
        lblCreatedAt.Font = new Font("Tahoma", 9F);
        lblCreatedAt.ForeColor = Color.FromArgb(16, 24, 40);
        lblCreatedAt.Location = new Point(1158, 11);
        lblCreatedAt.Margin = new Padding(3);
        lblCreatedAt.Name = "lblCreatedAt";
        lblCreatedAt.RightToLeft = RightToLeft.Yes;
        lblCreatedAt.Size = new Size(256, 26);
        lblCreatedAt.TabIndex = 1;
        lblCreatedAt.Text = "تاريخ الانشاء: —";
        lblCreatedAt.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblCreatedBy
        // 
        lblCreatedBy.AutoEllipsis = true;
        lblCreatedBy.BackColor = Color.FromArgb(192, 255, 255);
        lblCreatedBy.Dock = DockStyle.Fill;
        lblCreatedBy.Font = new Font("Tahoma", 9F);
        lblCreatedBy.ForeColor = Color.FromArgb(16, 24, 40);
        lblCreatedBy.Location = new Point(1420, 11);
        lblCreatedBy.Margin = new Padding(3);
        lblCreatedBy.Name = "lblCreatedBy";
        lblCreatedBy.RightToLeft = RightToLeft.Yes;
        lblCreatedBy.Size = new Size(223, 26);
        lblCreatedBy.TabIndex = 0;
        lblCreatedBy.Text = "أنشأ بواسطة: —";
        lblCreatedBy.TextAlign = ContentAlignment.MiddleRight;
        // 
        // tabStatus
        // 
        tabStatus.AutoScroll = true;
        tabStatus.BackColor = Color.LightCyan;
        tabStatus.Location = new Point(4, 29);
        tabStatus.Margin = new Padding(0);
        tabStatus.Name = "tabStatus";
        tabStatus.Padding = new Padding(3);
        tabStatus.Size = new Size(1646, 931);
        tabStatus.TabIndex = 2;
        tabStatus.Text = "الحالة";
        // 
        // tabBasicData
        // 
        tabBasicData.AutoScroll = true;
        tabBasicData.BackColor = Color.LightCyan;
        tabBasicData.Location = new Point(4, 29);
        tabBasicData.Margin = new Padding(0);
        tabBasicData.Name = "tabBasicData";
        tabBasicData.Padding = new Padding(3);
        tabBasicData.Size = new Size(1646, 931);
        tabBasicData.TabIndex = 1;
        tabBasicData.Text = "البيانات الإضافية";
        // 
        // tabCompanies
        // 
        tabCompanies.AutoScroll = true;
        tabCompanies.BackColor = Color.LightCyan;
        tabCompanies.Controls.Add(tabCompaniesSection);
        tabCompanies.Location = new Point(4, 29);
        tabCompanies.Margin = new Padding(0);
        tabCompanies.Name = "tabCompanies";
        tabCompanies.Padding = new Padding(3);
        tabCompanies.Size = new Size(1646, 931);
        tabCompanies.TabIndex = 0;
        tabCompanies.Text = "الشركات";
        // 
        // tabCompaniesSection
        // 
        tabCompaniesSection.BackColor = Color.LightCyan;
        tabCompaniesSection.Controls.Add(tabCompaniesFields);
        tabCompaniesSection.Controls.Add(groupBox1);
        tabCompaniesSection.Dock = DockStyle.Fill;
        tabCompaniesSection.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        tabCompaniesSection.ForeColor = Color.FromArgb(0, 53, 128);
        tabCompaniesSection.Location = new Point(3, 3);
        tabCompaniesSection.MinimumSize = new Size(0, 500);
        tabCompaniesSection.Name = "tabCompaniesSection";
        tabCompaniesSection.Padding = new Padding(16, 24, 16, 16);
        tabCompaniesSection.Size = new Size(1640, 925);
        tabCompaniesSection.TabIndex = 0;
        tabCompaniesSection.TabStop = false;
        tabCompaniesSection.Text = "بيانات الشركة الأساسية";
        // 
        // tabCompaniesFields
        // 
        tabCompaniesFields.BackColor = Color.LightCyan;
        tabCompaniesFields.ColumnCount = 4;
        tabCompaniesFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.66667F));
        tabCompaniesFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
        tabCompaniesFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
        tabCompaniesFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
        tabCompaniesFields.Controls.Add(txtAddress, 3, 6);
        tabCompaniesFields.Controls.Add(label5, 2, 6);
        tabCompaniesFields.Controls.Add(label9, 0, 4);
        tabCompaniesFields.Controls.Add(cmbCity, 1, 6);
        tabCompaniesFields.Controls.Add(cmbCountry, 1, 5);
        tabCompaniesFields.Controls.Add(txtNotes, 3, 9);
        tabCompaniesFields.Controls.Add(txtPostalCode, 1, 9);
        tabCompaniesFields.Controls.Add(txtWebsite, 3, 8);
        tabCompaniesFields.Controls.Add(txtEmail, 1, 8);
        tabCompaniesFields.Controls.Add(txtMobile, 3, 7);
        tabCompaniesFields.Controls.Add(txtPhone, 1, 7);
        tabCompaniesFields.Controls.Add(label24, 2, 9);
        tabCompaniesFields.Controls.Add(label22, 0, 9);
        tabCompaniesFields.Controls.Add(label20, 2, 8);
        tabCompaniesFields.Controls.Add(label18, 0, 8);
        tabCompaniesFields.Controls.Add(label15, 2, 7);
        tabCompaniesFields.Controls.Add(label11, 0, 7);
        tabCompaniesFields.Controls.Add(label3, 0, 6);
        tabCompaniesFields.Controls.Add(txtCommercialRegistrationNo, 3, 4);
        tabCompaniesFields.Controls.Add(cmbGovernorate, 3, 5);
        tabCompaniesFields.Controls.Add(cmbBaseCurrency, 3, 3);
        tabCompaniesFields.Controls.Add(txtTaxNumber, 1, 3);
        tabCompaniesFields.Controls.Add(cmbActivityType, 3, 2);
        tabCompaniesFields.Controls.Add(cmbCompanyGroup, 1, 2);
        tabCompaniesFields.Controls.Add(label2, 0, 2);
        tabCompaniesFields.Controls.Add(txtCompanyNameEn, 3, 1);
        tabCompaniesFields.Controls.Add(lblCompanyNameEn, 2, 1);
        tabCompaniesFields.Controls.Add(txtCompanyId, 1, 0);
        tabCompaniesFields.Controls.Add(label6, 0, 3);
        tabCompaniesFields.Controls.Add(label16, 2, 5);
        tabCompaniesFields.Controls.Add(label14, 0, 5);
        tabCompaniesFields.Controls.Add(label12, 2, 4);
        tabCompaniesFields.Controls.Add(label8, 2, 3);
        tabCompaniesFields.Controls.Add(label4, 2, 2);
        tabCompaniesFields.Controls.Add(txtCompanyCode, 3, 0);
        tabCompaniesFields.Controls.Add(label1, 2, 0);
        tabCompaniesFields.Controls.Add(lblCompanyId, 0, 0);
        tabCompaniesFields.Controls.Add(lblCompanyNameAr, 0, 1);
        tabCompaniesFields.Controls.Add(txtCompanyNameAr, 1, 1);
        tabCompaniesFields.Controls.Add(chkIsActive, 1, 4);
        tabCompaniesFields.Dock = DockStyle.Top;
        tabCompaniesFields.Location = new Point(288, 51);
        tabCompaniesFields.Margin = new Padding(0);
        tabCompaniesFields.Name = "tabCompaniesFields";
        tabCompaniesFields.RightToLeft = RightToLeft.Yes;
        tabCompaniesFields.RowCount = 10;
        tabCompaniesFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        tabCompaniesFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        tabCompaniesFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        tabCompaniesFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        tabCompaniesFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        tabCompaniesFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        tabCompaniesFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        tabCompaniesFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        tabCompaniesFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        tabCompaniesFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 72F));
        tabCompaniesFields.Size = new Size(1336, 432);
        tabCompaniesFields.TabIndex = 0;
        // 
        // txtAddress
        // 
        txtAddress.AccessibleName = "العنوان التفصيلي";
        txtAddress.BackColor = Color.White;
        txtAddress.Dock = DockStyle.Fill;
        txtAddress.Font = new Font("Tahoma", 9F);
        txtAddress.ForeColor = Color.FromArgb(16, 24, 40);
        txtAddress.Location = new Point(3, 243);
        txtAddress.Name = "txtAddress";
        txtAddress.RightToLeft = RightToLeft.Yes;
        txtAddress.Size = new Size(441, 32);
        txtAddress.TabIndex = 13;
        txtAddress.Tag = "FLD-COMPANY-NAME-EN";
        // 
        // label5
        // 
        label5.AutoEllipsis = true;
        label5.BackColor = Color.Transparent;
        label5.Dock = DockStyle.Fill;
        label5.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        label5.ForeColor = Color.FromArgb(16, 24, 40);
        label5.Location = new Point(450, 243);
        label5.Margin = new Padding(3);
        label5.Name = "label5";
        label5.RightToLeft = RightToLeft.Yes;
        label5.Size = new Size(216, 34);
        label5.TabIndex = 189;
        label5.Text = "العنوان التفصيلي";
        label5.TextAlign = ContentAlignment.MiddleRight;
        // 
        // label9
        // 
        label9.AutoEllipsis = true;
        label9.BackColor = Color.Transparent;
        label9.Dock = DockStyle.Fill;
        label9.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        label9.ForeColor = Color.FromArgb(16, 24, 40);
        label9.Location = new Point(1117, 163);
        label9.Margin = new Padding(3);
        label9.Name = "label9";
        label9.RightToLeft = RightToLeft.Yes;
        label9.Size = new Size(216, 34);
        label9.TabIndex = 188;
        label9.Text = "نشط";
        label9.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cmbCity
        // 
        cmbCity.AccessibleName = "المدينة";
        cmbCity.BackColor = Color.FromArgb(255, 249, 219);
        cmbCity.Dock = DockStyle.Fill;
        cmbCity.Font = new Font("Tahoma", 9F);
        cmbCity.ForeColor = Color.FromArgb(16, 24, 40);
        cmbCity.FormattingEnabled = true;
        cmbCity.Location = new Point(672, 243);
        cmbCity.Name = "cmbCity";
        cmbCity.RightToLeft = RightToLeft.Yes;
        cmbCity.Size = new Size(439, 33);
        cmbCity.TabIndex = 12;
        // 
        // cmbCountry
        // 
        cmbCountry.AccessibleName = "الدولة";
        cmbCountry.BackColor = Color.FromArgb(255, 249, 219);
        cmbCountry.Dock = DockStyle.Fill;
        cmbCountry.Font = new Font("Tahoma", 9F);
        cmbCountry.ForeColor = Color.FromArgb(16, 24, 40);
        cmbCountry.FormattingEnabled = true;
        cmbCountry.Location = new Point(672, 203);
        cmbCountry.Name = "cmbCountry";
        cmbCountry.RightToLeft = RightToLeft.Yes;
        cmbCountry.Size = new Size(439, 33);
        cmbCountry.TabIndex = 10;
        // 
        // txtNotes
        // 
        txtNotes.AccessibleName = "ملاحظات";
        txtNotes.BackColor = Color.White;
        txtNotes.Dock = DockStyle.Fill;
        txtNotes.Font = new Font("Tahoma", 9F);
        txtNotes.ForeColor = Color.FromArgb(16, 24, 40);
        txtNotes.Location = new Point(3, 363);
        txtNotes.Multiline = true;
        txtNotes.Name = "txtNotes";
        txtNotes.RightToLeft = RightToLeft.Yes;
        txtNotes.ScrollBars = ScrollBars.Vertical;
        txtNotes.Size = new Size(441, 66);
        txtNotes.TabIndex = 19;
        txtNotes.Tag = "FLD-COMPANY-NAME-EN";
        // 
        // txtPostalCode
        // 
        txtPostalCode.AccessibleName = "الرمز البريدي";
        txtPostalCode.BackColor = Color.White;
        txtPostalCode.Dock = DockStyle.Fill;
        txtPostalCode.Font = new Font("Tahoma", 9F);
        txtPostalCode.ForeColor = Color.FromArgb(16, 24, 40);
        txtPostalCode.Location = new Point(672, 363);
        txtPostalCode.Name = "txtPostalCode";
        txtPostalCode.RightToLeft = RightToLeft.No;
        txtPostalCode.Size = new Size(439, 32);
        txtPostalCode.TabIndex = 18;
        txtPostalCode.Tag = "FLD-COMPANY-NAME-EN";
        // 
        // txtWebsite
        // 
        txtWebsite.AccessibleName = "الموقع الالكتروني";
        txtWebsite.BackColor = Color.White;
        txtWebsite.Dock = DockStyle.Fill;
        txtWebsite.Font = new Font("Tahoma", 9F);
        txtWebsite.ForeColor = Color.FromArgb(16, 24, 40);
        txtWebsite.Location = new Point(3, 323);
        txtWebsite.Name = "txtWebsite";
        txtWebsite.RightToLeft = RightToLeft.No;
        txtWebsite.Size = new Size(441, 32);
        txtWebsite.TabIndex = 17;
        txtWebsite.Tag = "FLD-COMPANY-NAME-EN";
        // 
        // txtEmail
        // 
        txtEmail.AccessibleName = "البريد الالكتروني";
        txtEmail.BackColor = Color.White;
        txtEmail.Dock = DockStyle.Fill;
        txtEmail.Font = new Font("Tahoma", 9F);
        txtEmail.ForeColor = Color.FromArgb(16, 24, 40);
        txtEmail.Location = new Point(672, 323);
        txtEmail.Name = "txtEmail";
        txtEmail.RightToLeft = RightToLeft.No;
        txtEmail.Size = new Size(439, 32);
        txtEmail.TabIndex = 16;
        txtEmail.Tag = "FLD-COMPANY-NAME-EN";
        // 
        // txtMobile
        // 
        txtMobile.AccessibleName = "رقم الجوال";
        txtMobile.BackColor = Color.White;
        txtMobile.Dock = DockStyle.Fill;
        txtMobile.Font = new Font("Tahoma", 9F);
        txtMobile.ForeColor = Color.FromArgb(16, 24, 40);
        txtMobile.Location = new Point(3, 283);
        txtMobile.Name = "txtMobile";
        txtMobile.RightToLeft = RightToLeft.No;
        txtMobile.Size = new Size(441, 32);
        txtMobile.TabIndex = 15;
        txtMobile.Tag = "FLD-COMPANY-NAME-EN";
        // 
        // txtPhone
        // 
        txtPhone.AccessibleName = "رقم الهاتف";
        txtPhone.BackColor = Color.White;
        txtPhone.Dock = DockStyle.Fill;
        txtPhone.Font = new Font("Tahoma", 9F);
        txtPhone.ForeColor = Color.FromArgb(16, 24, 40);
        txtPhone.Location = new Point(672, 283);
        txtPhone.Name = "txtPhone";
        txtPhone.RightToLeft = RightToLeft.No;
        txtPhone.Size = new Size(439, 32);
        txtPhone.TabIndex = 14;
        txtPhone.Tag = "FLD-COMPANY-NAME-EN";
        // 
        // label24
        // 
        label24.AutoEllipsis = true;
        label24.BackColor = Color.Transparent;
        label24.Dock = DockStyle.Fill;
        label24.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        label24.ForeColor = Color.FromArgb(16, 24, 40);
        label24.Location = new Point(450, 363);
        label24.Margin = new Padding(3);
        label24.Name = "label24";
        label24.RightToLeft = RightToLeft.Yes;
        label24.Size = new Size(216, 66);
        label24.TabIndex = 149;
        label24.Text = "ملاحظات";
        label24.TextAlign = ContentAlignment.MiddleRight;
        // 
        // label22
        // 
        label22.AutoEllipsis = true;
        label22.BackColor = Color.Transparent;
        label22.Dock = DockStyle.Fill;
        label22.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        label22.ForeColor = Color.FromArgb(16, 24, 40);
        label22.Location = new Point(1117, 363);
        label22.Margin = new Padding(3);
        label22.Name = "label22";
        label22.RightToLeft = RightToLeft.Yes;
        label22.Size = new Size(216, 66);
        label22.TabIndex = 147;
        label22.Text = "الرمز البريدي";
        label22.TextAlign = ContentAlignment.MiddleRight;
        // 
        // label20
        // 
        label20.AutoEllipsis = true;
        label20.BackColor = Color.Transparent;
        label20.Dock = DockStyle.Fill;
        label20.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        label20.ForeColor = Color.FromArgb(16, 24, 40);
        label20.Location = new Point(450, 323);
        label20.Margin = new Padding(3);
        label20.Name = "label20";
        label20.RightToLeft = RightToLeft.Yes;
        label20.Size = new Size(216, 34);
        label20.TabIndex = 145;
        label20.Text = "الموقع الالكتروني";
        label20.TextAlign = ContentAlignment.MiddleRight;
        // 
        // label18
        // 
        label18.AutoEllipsis = true;
        label18.BackColor = Color.Transparent;
        label18.Dock = DockStyle.Fill;
        label18.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        label18.ForeColor = Color.FromArgb(16, 24, 40);
        label18.Location = new Point(1117, 323);
        label18.Margin = new Padding(3);
        label18.Name = "label18";
        label18.RightToLeft = RightToLeft.Yes;
        label18.Size = new Size(216, 34);
        label18.TabIndex = 143;
        label18.Text = "البريد الالكتروني";
        label18.TextAlign = ContentAlignment.MiddleRight;
        // 
        // label15
        // 
        label15.AutoEllipsis = true;
        label15.BackColor = Color.Transparent;
        label15.Dock = DockStyle.Fill;
        label15.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        label15.ForeColor = Color.FromArgb(16, 24, 40);
        label15.Location = new Point(450, 283);
        label15.Margin = new Padding(3);
        label15.Name = "label15";
        label15.RightToLeft = RightToLeft.Yes;
        label15.Size = new Size(216, 34);
        label15.TabIndex = 141;
        label15.Text = "رقم الجوال";
        label15.TextAlign = ContentAlignment.MiddleRight;
        // 
        // label11
        // 
        label11.AutoEllipsis = true;
        label11.BackColor = Color.Transparent;
        label11.Dock = DockStyle.Fill;
        label11.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        label11.ForeColor = Color.FromArgb(16, 24, 40);
        label11.Location = new Point(1117, 283);
        label11.Margin = new Padding(3);
        label11.Name = "label11";
        label11.RightToLeft = RightToLeft.Yes;
        label11.Size = new Size(216, 34);
        label11.TabIndex = 139;
        label11.Text = "رقم الهاتف";
        label11.TextAlign = ContentAlignment.MiddleRight;
        // 
        // label3
        // 
        label3.AutoEllipsis = true;
        label3.BackColor = Color.Transparent;
        label3.Dock = DockStyle.Fill;
        label3.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        label3.ForeColor = Color.FromArgb(16, 24, 40);
        label3.Location = new Point(1117, 243);
        label3.Margin = new Padding(3);
        label3.Name = "label3";
        label3.RightToLeft = RightToLeft.Yes;
        label3.Size = new Size(216, 34);
        label3.TabIndex = 135;
        label3.Text = "المدينة";
        label3.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtCommercialRegistrationNo
        // 
        txtCommercialRegistrationNo.AccessibleName = "رقم السجل التجاري";
        txtCommercialRegistrationNo.BackColor = Color.White;
        txtCommercialRegistrationNo.Dock = DockStyle.Fill;
        txtCommercialRegistrationNo.Font = new Font("Tahoma", 9F);
        txtCommercialRegistrationNo.ForeColor = Color.FromArgb(16, 24, 40);
        txtCommercialRegistrationNo.Location = new Point(3, 163);
        txtCommercialRegistrationNo.Name = "txtCommercialRegistrationNo";
        txtCommercialRegistrationNo.RightToLeft = RightToLeft.No;
        txtCommercialRegistrationNo.Size = new Size(441, 32);
        txtCommercialRegistrationNo.TabIndex = 9;
        txtCommercialRegistrationNo.Tag = "FLD-COMPANY-NAME-EN";
        // 
        // cmbGovernorate
        // 
        cmbGovernorate.AccessibleName = "المحافظة";
        cmbGovernorate.BackColor = Color.White;
        cmbGovernorate.Dock = DockStyle.Fill;
        cmbGovernorate.Font = new Font("Tahoma", 9F);
        cmbGovernorate.ForeColor = Color.FromArgb(16, 24, 40);
        cmbGovernorate.FormattingEnabled = true;
        cmbGovernorate.Location = new Point(3, 203);
        cmbGovernorate.Name = "cmbGovernorate";
        cmbGovernorate.RightToLeft = RightToLeft.Yes;
        cmbGovernorate.Size = new Size(441, 33);
        cmbGovernorate.TabIndex = 11;
        // 
        // cmbBaseCurrency
        // 
        cmbBaseCurrency.AccessibleName = "العملة الاساسية";
        cmbBaseCurrency.BackColor = Color.FromArgb(255, 249, 219);
        cmbBaseCurrency.Dock = DockStyle.Fill;
        cmbBaseCurrency.Font = new Font("Tahoma", 9F);
        cmbBaseCurrency.ForeColor = Color.FromArgb(16, 24, 40);
        cmbBaseCurrency.FormattingEnabled = true;
        cmbBaseCurrency.Location = new Point(3, 123);
        cmbBaseCurrency.Name = "cmbBaseCurrency";
        cmbBaseCurrency.RightToLeft = RightToLeft.Yes;
        cmbBaseCurrency.Size = new Size(441, 33);
        cmbBaseCurrency.TabIndex = 7;
        // 
        // txtTaxNumber
        // 
        txtTaxNumber.AccessibleName = "الرقم الضريبي";
        txtTaxNumber.BackColor = Color.White;
        txtTaxNumber.Dock = DockStyle.Fill;
        txtTaxNumber.Font = new Font("Tahoma", 9F);
        txtTaxNumber.ForeColor = Color.FromArgb(16, 24, 40);
        txtTaxNumber.Location = new Point(672, 123);
        txtTaxNumber.Name = "txtTaxNumber";
        txtTaxNumber.RightToLeft = RightToLeft.No;
        txtTaxNumber.Size = new Size(439, 32);
        txtTaxNumber.TabIndex = 6;
        txtTaxNumber.Tag = "FLD-COMPANY-NAME-EN";
        // 
        // cmbActivityType
        // 
        cmbActivityType.AccessibleName = "نوع النشاط";
        cmbActivityType.BackColor = Color.FromArgb(255, 249, 219);
        cmbActivityType.Dock = DockStyle.Fill;
        cmbActivityType.Font = new Font("Tahoma", 9F);
        cmbActivityType.ForeColor = Color.FromArgb(16, 24, 40);
        cmbActivityType.FormattingEnabled = true;
        cmbActivityType.Location = new Point(3, 83);
        cmbActivityType.Name = "cmbActivityType";
        cmbActivityType.RightToLeft = RightToLeft.Yes;
        cmbActivityType.Size = new Size(441, 33);
        cmbActivityType.TabIndex = 5;
        // 
        // cmbCompanyGroup
        // 
        cmbCompanyGroup.AccessibleName = "مجموعة الشركة";
        cmbCompanyGroup.BackColor = Color.White;
        cmbCompanyGroup.Dock = DockStyle.Fill;
        cmbCompanyGroup.Font = new Font("Tahoma", 9F);
        cmbCompanyGroup.ForeColor = Color.FromArgb(16, 24, 40);
        cmbCompanyGroup.FormattingEnabled = true;
        cmbCompanyGroup.Location = new Point(672, 83);
        cmbCompanyGroup.Name = "cmbCompanyGroup";
        cmbCompanyGroup.RightToLeft = RightToLeft.Yes;
        cmbCompanyGroup.Size = new Size(439, 33);
        cmbCompanyGroup.TabIndex = 4;
        // 
        // label2
        // 
        label2.AutoEllipsis = true;
        label2.BackColor = Color.Transparent;
        label2.Dock = DockStyle.Fill;
        label2.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        label2.ForeColor = Color.FromArgb(16, 24, 40);
        label2.Location = new Point(1117, 83);
        label2.Margin = new Padding(3);
        label2.Name = "label2";
        label2.RightToLeft = RightToLeft.Yes;
        label2.Size = new Size(216, 34);
        label2.TabIndex = 29;
        label2.Text = "مجموعة الشركات";
        label2.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtCompanyNameEn
        // 
        txtCompanyNameEn.AccessibleName = "اسم الشركة الإنجليزي";
        txtCompanyNameEn.BackColor = Color.White;
        txtCompanyNameEn.Dock = DockStyle.Fill;
        txtCompanyNameEn.Font = new Font("Tahoma", 9F);
        txtCompanyNameEn.ForeColor = Color.FromArgb(16, 24, 40);
        txtCompanyNameEn.Location = new Point(3, 43);
        txtCompanyNameEn.Name = "txtCompanyNameEn";
        txtCompanyNameEn.RightToLeft = RightToLeft.No;
        txtCompanyNameEn.Size = new Size(441, 32);
        txtCompanyNameEn.TabIndex = 3;
        txtCompanyNameEn.Tag = "FLD-COMPANY-NAME-EN";
        // 
        // lblCompanyNameEn
        // 
        lblCompanyNameEn.AutoEllipsis = true;
        lblCompanyNameEn.BackColor = Color.Transparent;
        lblCompanyNameEn.Dock = DockStyle.Fill;
        lblCompanyNameEn.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblCompanyNameEn.ForeColor = Color.FromArgb(16, 24, 40);
        lblCompanyNameEn.Location = new Point(450, 43);
        lblCompanyNameEn.Margin = new Padding(3);
        lblCompanyNameEn.Name = "lblCompanyNameEn";
        lblCompanyNameEn.RightToLeft = RightToLeft.Yes;
        lblCompanyNameEn.Size = new Size(216, 34);
        lblCompanyNameEn.TabIndex = 27;
        lblCompanyNameEn.Text = "اسم الشركة الإنجليزي";
        lblCompanyNameEn.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtCompanyId
        // 
        txtCompanyId.AccessibleName = "معرف الشركة";
        txtCompanyId.BackColor = Color.White;
        txtCompanyId.Dock = DockStyle.Fill;
        txtCompanyId.Font = new Font("Tahoma", 9F);
        txtCompanyId.ForeColor = Color.FromArgb(16, 24, 40);
        txtCompanyId.Location = new Point(672, 3);
        txtCompanyId.Name = "txtCompanyId";
        txtCompanyId.ReadOnly = true;
        txtCompanyId.RightToLeft = RightToLeft.No;
        txtCompanyId.Size = new Size(439, 32);
        txtCompanyId.TabIndex = 0;
        txtCompanyId.TabStop = false;
        txtCompanyId.Tag = "FLD-COMPANY-NAME-AR";
        // 
        // label6
        // 
        label6.AutoEllipsis = true;
        label6.BackColor = Color.Transparent;
        label6.Dock = DockStyle.Fill;
        label6.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        label6.ForeColor = Color.FromArgb(16, 24, 40);
        label6.Location = new Point(1117, 123);
        label6.Margin = new Padding(3);
        label6.Name = "label6";
        label6.RightToLeft = RightToLeft.Yes;
        label6.Size = new Size(216, 34);
        label6.TabIndex = 25;
        label6.Text = "الرقم الضريبي";
        label6.TextAlign = ContentAlignment.MiddleRight;
        // 
        // label16
        // 
        label16.AutoEllipsis = true;
        label16.BackColor = Color.Transparent;
        label16.Dock = DockStyle.Fill;
        label16.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        label16.ForeColor = Color.FromArgb(16, 24, 40);
        label16.Location = new Point(450, 203);
        label16.Margin = new Padding(3);
        label16.Name = "label16";
        label16.RightToLeft = RightToLeft.Yes;
        label16.Size = new Size(216, 34);
        label16.TabIndex = 22;
        label16.Text = "المحافظة";
        label16.TextAlign = ContentAlignment.MiddleRight;
        // 
        // label14
        // 
        label14.AutoEllipsis = true;
        label14.BackColor = Color.Transparent;
        label14.Dock = DockStyle.Fill;
        label14.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        label14.ForeColor = Color.FromArgb(180, 35, 24);
        label14.Location = new Point(1117, 203);
        label14.Margin = new Padding(3);
        label14.Name = "label14";
        label14.RightToLeft = RightToLeft.Yes;
        label14.Size = new Size(216, 34);
        label14.TabIndex = 20;
        label14.Text = "الدولة *";
        label14.TextAlign = ContentAlignment.MiddleRight;
        // 
        // label12
        // 
        label12.AutoEllipsis = true;
        label12.BackColor = Color.Transparent;
        label12.Dock = DockStyle.Fill;
        label12.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        label12.ForeColor = Color.FromArgb(16, 24, 40);
        label12.Location = new Point(450, 163);
        label12.Margin = new Padding(3);
        label12.Name = "label12";
        label12.RightToLeft = RightToLeft.Yes;
        label12.Size = new Size(216, 34);
        label12.TabIndex = 18;
        label12.Text = "رقم السجل التجاري";
        label12.TextAlign = ContentAlignment.MiddleRight;
        // 
        // label8
        // 
        label8.AutoEllipsis = true;
        label8.BackColor = Color.Transparent;
        label8.Dock = DockStyle.Fill;
        label8.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        label8.ForeColor = Color.FromArgb(180, 35, 24);
        label8.Location = new Point(450, 123);
        label8.Margin = new Padding(3);
        label8.Name = "label8";
        label8.RightToLeft = RightToLeft.Yes;
        label8.Size = new Size(216, 34);
        label8.TabIndex = 14;
        label8.Text = "العملة الاساسية *";
        label8.TextAlign = ContentAlignment.MiddleRight;
        // 
        // label4
        // 
        label4.AutoEllipsis = true;
        label4.BackColor = Color.Transparent;
        label4.Dock = DockStyle.Fill;
        label4.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        label4.ForeColor = Color.FromArgb(180, 35, 24);
        label4.Location = new Point(450, 83);
        label4.Margin = new Padding(3);
        label4.Name = "label4";
        label4.RightToLeft = RightToLeft.Yes;
        label4.Size = new Size(216, 34);
        label4.TabIndex = 10;
        label4.Text = "نوع النشاط *";
        label4.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtCompanyCode
        // 
        txtCompanyCode.AccessibleName = "كود الشركة";
        txtCompanyCode.BackColor = Color.White;
        txtCompanyCode.Dock = DockStyle.Fill;
        txtCompanyCode.Font = new Font("Tahoma", 9F);
        txtCompanyCode.ForeColor = Color.FromArgb(16, 24, 40);
        txtCompanyCode.Location = new Point(3, 3);
        txtCompanyCode.Name = "txtCompanyCode";
        txtCompanyCode.ReadOnly = true;
        txtCompanyCode.RightToLeft = RightToLeft.No;
        txtCompanyCode.Size = new Size(441, 32);
        txtCompanyCode.TabIndex = 1;
        txtCompanyCode.TabStop = false;
        txtCompanyCode.Tag = "FLD-COMPANY-NAME-AR";
        // 
        // label1
        // 
        label1.AutoEllipsis = true;
        label1.BackColor = Color.Transparent;
        label1.Dock = DockStyle.Fill;
        label1.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        label1.ForeColor = Color.FromArgb(16, 24, 40);
        label1.Location = new Point(450, 3);
        label1.Margin = new Padding(3);
        label1.Name = "label1";
        label1.RightToLeft = RightToLeft.Yes;
        label1.Size = new Size(216, 34);
        label1.TabIndex = 6;
        label1.Text = "كود الشركة";
        label1.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblCompanyId
        // 
        lblCompanyId.AutoEllipsis = true;
        lblCompanyId.BackColor = Color.Transparent;
        lblCompanyId.Dock = DockStyle.Fill;
        lblCompanyId.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblCompanyId.ForeColor = Color.FromArgb(16, 24, 40);
        lblCompanyId.Location = new Point(1117, 3);
        lblCompanyId.Margin = new Padding(3);
        lblCompanyId.Name = "lblCompanyId";
        lblCompanyId.RightToLeft = RightToLeft.Yes;
        lblCompanyId.Size = new Size(216, 34);
        lblCompanyId.TabIndex = 0;
        lblCompanyId.Text = "معرف الشركة ";
        lblCompanyId.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblCompanyNameAr
        // 
        lblCompanyNameAr.AutoEllipsis = true;
        lblCompanyNameAr.BackColor = Color.Transparent;
        lblCompanyNameAr.Dock = DockStyle.Fill;
        lblCompanyNameAr.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblCompanyNameAr.ForeColor = Color.FromArgb(180, 35, 24);
        lblCompanyNameAr.Location = new Point(1117, 43);
        lblCompanyNameAr.Margin = new Padding(3);
        lblCompanyNameAr.Name = "lblCompanyNameAr";
        lblCompanyNameAr.RightToLeft = RightToLeft.Yes;
        lblCompanyNameAr.Size = new Size(216, 34);
        lblCompanyNameAr.TabIndex = 2;
        lblCompanyNameAr.Text = "اسم الشركة العربي *";
        lblCompanyNameAr.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtCompanyNameAr
        // 
        txtCompanyNameAr.AccessibleName = "اسم الشركة العربي";
        txtCompanyNameAr.BackColor = Color.FromArgb(255, 249, 219);
        txtCompanyNameAr.Dock = DockStyle.Fill;
        txtCompanyNameAr.Font = new Font("Tahoma", 9F);
        txtCompanyNameAr.ForeColor = Color.FromArgb(16, 24, 40);
        txtCompanyNameAr.Location = new Point(672, 43);
        txtCompanyNameAr.Name = "txtCompanyNameAr";
        txtCompanyNameAr.RightToLeft = RightToLeft.Yes;
        txtCompanyNameAr.Size = new Size(439, 32);
        txtCompanyNameAr.TabIndex = 2;
        txtCompanyNameAr.Tag = "FLD-COMPANY-NAME-AR";
        // 
        // chkIsActive
        // 
        chkIsActive.AccessibleName = "نشط";
        chkIsActive.AutoSize = true;
        chkIsActive.BackColor = Color.Transparent;
        chkIsActive.CheckAlign = ContentAlignment.MiddleCenter;
        chkIsActive.Checked = true;
        chkIsActive.CheckState = CheckState.Checked;
        chkIsActive.Dock = DockStyle.Fill;
        chkIsActive.Font = new Font("Tahoma", 9F);
        chkIsActive.ForeColor = Color.FromArgb(16, 24, 40);
        chkIsActive.Location = new Point(672, 163);
        chkIsActive.Name = "chkIsActive";
        chkIsActive.Size = new Size(439, 34);
        chkIsActive.TabIndex = 8;
        chkIsActive.UseVisualStyleBackColor = false;
        // 
        // groupBox1
        // 
        groupBox1.BackColor = Color.LightCyan;
        groupBox1.Controls.Add(tableLayoutPanel1);
        groupBox1.Dock = DockStyle.Left;
        groupBox1.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        groupBox1.ForeColor = Color.FromArgb(0, 53, 128);
        groupBox1.Location = new Point(16, 51);
        groupBox1.Name = "groupBox1";
        groupBox1.Padding = new Padding(16, 24, 16, 16);
        groupBox1.Size = new Size(272, 858);
        groupBox1.TabIndex = 1;
        groupBox1.TabStop = false;
        groupBox1.Text = "الهوية";
        // 
        // tableLayoutPanel1
        // 
        tableLayoutPanel1.BackColor = Color.LightCyan;
        tableLayoutPanel1.ColumnCount = 1;
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 1);
        tableLayoutPanel1.Controls.Add(picCompanyLogo, 0, 0);
        tableLayoutPanel1.Dock = DockStyle.Top;
        tableLayoutPanel1.Location = new Point(16, 51);
        tableLayoutPanel1.Margin = new Padding(0);
        tableLayoutPanel1.Name = "tableLayoutPanel1";
        tableLayoutPanel1.RightToLeft = RightToLeft.Yes;
        tableLayoutPanel1.RowCount = 2;
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
        tableLayoutPanel1.Size = new Size(240, 257);
        tableLayoutPanel1.TabIndex = 0;
        // 
        // tableLayoutPanel2
        // 
        tableLayoutPanel2.BackColor = Color.LightCyan;
        tableLayoutPanel2.ColumnCount = 2;
        tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableLayoutPanel2.Controls.Add(btnAddLogo, 0, 0);
        tableLayoutPanel2.Controls.Add(btnRemoveLogo, 1, 0);
        tableLayoutPanel2.Dock = DockStyle.Fill;
        tableLayoutPanel2.Location = new Point(0, 205);
        tableLayoutPanel2.Margin = new Padding(0);
        tableLayoutPanel2.Name = "tableLayoutPanel2";
        tableLayoutPanel2.RightToLeft = RightToLeft.Yes;
        tableLayoutPanel2.RowCount = 1;
        tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tableLayoutPanel2.Size = new Size(240, 52);
        tableLayoutPanel2.TabIndex = 4;
        // 
        // btnAddLogo
        // 
        btnAddLogo.AutoEllipsis = true;
        btnAddLogo.BackColor = Color.FromArgb(224, 224, 224);
        btnAddLogo.Dock = DockStyle.Fill;
        btnAddLogo.FlatStyle = FlatStyle.Flat;
        btnAddLogo.Font = new Font("Tahoma", 9F);
        btnAddLogo.ForeColor = Color.FromArgb(16, 24, 40);
        btnAddLogo.Location = new Point(124, 4);
        btnAddLogo.Margin = new Padding(4);
        btnAddLogo.Name = "btnAddLogo";
        btnAddLogo.Size = new Size(112, 44);
        btnAddLogo.TabIndex = 0;
        btnAddLogo.Text = "اضافة";
        btnAddLogo.UseVisualStyleBackColor = false;
        // 
        // btnRemoveLogo
        // 
        btnRemoveLogo.AutoEllipsis = true;
        btnRemoveLogo.BackColor = Color.FromArgb(224, 224, 224);
        btnRemoveLogo.Dock = DockStyle.Fill;
        btnRemoveLogo.FlatStyle = FlatStyle.Flat;
        btnRemoveLogo.Font = new Font("Tahoma", 9F);
        btnRemoveLogo.ForeColor = Color.FromArgb(16, 24, 40);
        btnRemoveLogo.Location = new Point(4, 4);
        btnRemoveLogo.Margin = new Padding(4);
        btnRemoveLogo.Name = "btnRemoveLogo";
        btnRemoveLogo.Size = new Size(112, 44);
        btnRemoveLogo.TabIndex = 1;
        btnRemoveLogo.Text = "حذف";
        btnRemoveLogo.UseVisualStyleBackColor = false;
        // 
        // picCompanyLogo
        // 
        picCompanyLogo.BackColor = Color.White;
        picCompanyLogo.BorderStyle = BorderStyle.FixedSingle;
        picCompanyLogo.Dock = DockStyle.Fill;
        picCompanyLogo.Location = new Point(3, 3);
        picCompanyLogo.Name = "picCompanyLogo";
        picCompanyLogo.Size = new Size(234, 199);
        picCompanyLogo.SizeMode = PictureBoxSizeMode.Zoom;
        picCompanyLogo.TabIndex = 3;
        picCompanyLogo.TabStop = false;
        // 
        // tabCompanyManagement
        // 
        tabCompanyManagement.AccessibleName = "الشركات";
        tabCompanyManagement.Controls.Add(tabCompanies);
        tabCompanyManagement.Controls.Add(tabBasicData);
        tabCompanyManagement.Controls.Add(tabStatus);
        tabCompanyManagement.Dock = DockStyle.Fill;
        tabCompanyManagement.Font = new Font("Tahoma", 9F);
        tabCompanyManagement.Location = new Point(0, 48);
        tabCompanyManagement.Margin = new Padding(0);
        tabCompanyManagement.Name = "tabCompanyManagement";
        tabCompanyManagement.RightToLeft = RightToLeft.Yes;
        tabCompanyManagement.RightToLeftLayout = true;
        tabCompanyManagement.SelectedIndex = 0;
        tabCompanyManagement.Size = new Size(1654, 964);
        tabCompanyManagement.TabIndex = 0;
        // 
        // UcCompanyManagement
        // 
        AutoScaleDimensions = new SizeF(120F, 120F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.FromArgb(248, 250, 252);
        Controls.Add(tabCompanyManagement);
        Controls.Add(tlpAuditInfo);
        Controls.Add(pnlHeader);
        Font = new Font("Tahoma", 9F);
        ForeColor = Color.FromArgb(16, 24, 40);
        Margin = new Padding(0);
        Name = "UcCompanyManagement";
        RightToLeft = RightToLeft.Yes;
        Size = new Size(1654, 1060);
        Tag = "SCR-SET-004";
        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        pnlToolbar.ResumeLayout(false);
        pnlToolbar.PerformLayout();
        tlpAuditInfo.ResumeLayout(false);
        tabCompanies.ResumeLayout(false);
        tabCompaniesSection.ResumeLayout(false);
        tabCompaniesFields.ResumeLayout(false);
        tabCompaniesFields.PerformLayout();
        groupBox1.ResumeLayout(false);
        tableLayoutPanel1.ResumeLayout(false);
        tableLayoutPanel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)picCompanyLogo).EndInit();
        tabCompanyManagement.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    
        // Explicit Designer-owned command catalog.
        designerCommandBar.Dock = DockStyle.Top;
        designerCommandBar.Location = new Point(0, 0);
        designerCommandBar.Size = new Size(1654, 48);
        designerCommandBar.Margin = new Padding(5);
        designerCommandBar.MinimumSize = new Size(0, 48);
        designerCommandBar.TabIndex = 3;
        designerCommandBar.RightToLeft = RightToLeft.Yes;
        designerCommandBar.Controls.Add(pnlToolbar);
        pnlToolbar.Dock = DockStyle.Fill;
        pnlToolbar.Margin = Padding.Empty;
        designerCommandBar.Name = "designerCommandBar";
        designerCommandBar.BackColor = Color.FromArgb(239, 239, 239);
        designerCommandBar.BorderStyle = BorderStyle.FixedSingle;
        designerHiddenCommands.Name = "designerHiddenCommands";
        designerHiddenCommands.Visible = false;
        designerCommandBar.Controls.Add(designerHiddenCommands);
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
        designerCommandBar.SetCommandRole(button1, TransportERP.Desktop.CoreUI.DesignerCommandRole.Add);
        designerCommandBar.SetCommandRole(btnEdit, TransportERP.Desktop.CoreUI.DesignerCommandRole.Edit);
        designerCommandBar.SetCommandRole(btnDelete, TransportERP.Desktop.CoreUI.DesignerCommandRole.Delete);
        designerCommandBar.SetCommandRole(btnUndo, TransportERP.Desktop.CoreUI.DesignerCommandRole.Cancel);
        designerCommandBar.SetCommandRole(btnSearch, TransportERP.Desktop.CoreUI.DesignerCommandRole.View);
        designerCommandBar.SetCommandRole(btnLast, TransportERP.Desktop.CoreUI.DesignerCommandRole.Last);
        designerCommandBar.SetCommandRole(btnNext, TransportERP.Desktop.CoreUI.DesignerCommandRole.Next);
        designerCommandBar.SetCommandRole(btnPrevious, TransportERP.Desktop.CoreUI.DesignerCommandRole.Previous);
        designerCommandBar.SetCommandRole(btnFirst, TransportERP.Desktop.CoreUI.DesignerCommandRole.First);
        designerCommandBar.SetCommandRole(btnSave, TransportERP.Desktop.CoreUI.DesignerCommandRole.Save);
        designerCommandBar.SetCommandRole(btnPrint, TransportERP.Desktop.CoreUI.DesignerCommandRole.Print);
        designerCommandBar.SetCommandRole(btnClose, TransportERP.Desktop.CoreUI.DesignerCommandRole.Close);
        designerCommandBar.SetCommandRole(button2, TransportERP.Desktop.CoreUI.DesignerCommandRole.Refresh);
        standardCommandImport.AutoSize = false;
        standardCommandImport.Dock = DockStyle.None;
        standardCommandImport.MinimumSize = Size.Empty;
        standardCommandImport.Size = new Size(26, 24);
        standardCommandImport.Margin = new Padding(1);
        standardCommandImport.FlatStyle = FlatStyle.Flat;
        standardCommandImport.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandImport.Text = "استيراد";
        designerHiddenCommands.Controls.Add(standardCommandImport);
        designerCommandBar.SetCommandRole(standardCommandImport, TransportERP.Desktop.CoreUI.DesignerCommandRole.Import);
        standardCommandExport.AutoSize = false;
        standardCommandExport.Dock = DockStyle.None;
        standardCommandExport.MinimumSize = Size.Empty;
        standardCommandExport.Size = new Size(26, 24);
        standardCommandExport.Margin = new Padding(1);
        standardCommandExport.FlatStyle = FlatStyle.Flat;
        standardCommandExport.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandExport.Text = "تصدير";
        designerHiddenCommands.Controls.Add(standardCommandExport);
        designerCommandBar.SetCommandRole(standardCommandExport, TransportERP.Desktop.CoreUI.DesignerCommandRole.Export);
        standardCommandHelp.AutoSize = false;
        standardCommandHelp.Dock = DockStyle.None;
        standardCommandHelp.MinimumSize = Size.Empty;
        standardCommandHelp.Size = new Size(26, 24);
        standardCommandHelp.Margin = new Padding(1);
        standardCommandHelp.FlatStyle = FlatStyle.Flat;
        standardCommandHelp.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandHelp.Text = "مساعدة";
        designerHiddenCommands.Controls.Add(standardCommandHelp);
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

    private Panel pnlHeader;
    private FlowLayoutPanel pnlToolbar;
    private Button button1;
    private Button btnSave;
    private Button btnEdit;
    private Button btnDelete;
    private Button button2;
    private Button btnClose;
    private Button btnFirst;
    private Button btnPrevious;
    private TextBox txtCurrentRecordNo;
    private Button btnNext;
    private Button btnLast;
    private Button btnUndo;
    private Button btnSearch;
    private Button btnPrint;
    private Button button4;
    private TableLayoutPanel tlpAuditInfo;
    private Label lblPrintCount;
    private Label lblLastPrintedAt;
    private Label lblEditCount;
    private Label lblModifiedAt;
    private Label lblModifiedBy;
    private Label lblCreatedAt;
    private Label lblCreatedBy;
    private TabPage tabStatus;
    private TabPage tabBasicData;
    private TabPage tabCompanies;
    private TabControl tabCompanyManagement;
    private GroupBox tabCompaniesSection;
    private GroupBox groupBox1;
    private TableLayoutPanel tableLayoutPanel1;
    private PictureBox picCompanyLogo;
    private TableLayoutPanel tabCompaniesFields;
    private TextBox txtAddress;
    private Label label5;
    private Label label9;
    private ComboBox cmbCity;
    private ComboBox cmbCountry;
    private TextBox txtNotes;
    private TextBox txtPostalCode;
    private TextBox txtWebsite;
    private TextBox txtEmail;
    private TextBox txtMobile;
    private TextBox txtPhone;
    private Label label24;
    private Label label22;
    private Label label20;
    private Label label18;
    private Label label15;
    private Label label11;
    private Label label3;
    private TextBox txtCommercialRegistrationNo;
    private ComboBox cmbGovernorate;
    private ComboBox cmbBaseCurrency;
    private TextBox txtTaxNumber;
    private ComboBox cmbActivityType;
    private ComboBox cmbCompanyGroup;
    private Label label2;
    private TextBox txtCompanyNameEn;
    private Label lblCompanyNameEn;
    private TextBox txtCompanyId;
    private Label label6;
    private Label label16;
    private Label label14;
    private Label label12;
    private Label label8;
    private Label label4;
    private TextBox txtCompanyCode;
    private Label label1;
    private Label lblCompanyId;
    private Label lblCompanyNameAr;
    private TextBox txtCompanyNameAr;
    private CheckBox chkIsActive;
    private TableLayoutPanel tableLayoutPanel2;
    private Button btnAddLogo;
    private Button btnRemoveLogo;
}
