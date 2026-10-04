#nullable enable
using System.Drawing;
using System.Windows.Forms;

namespace TransportERP.Desktop.Forms.SystemSettings.General;

partial class UcCompanyManagement
{
    private System.ComponentModel.IContainer? components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
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
        lblTitle = new Label();
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
        pnlHeader.BackColor = Color.FromArgb(224, 224, 224);
        pnlHeader.Controls.Add(pnlToolbar);
        pnlHeader.Controls.Add(lblTitle);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Location = new Point(0, 0);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Size = new Size(1654, 47);
        pnlHeader.TabIndex = 13;
        // 
        // pnlToolbar
        // 
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
        pnlToolbar.Dock = DockStyle.Fill;
        pnlToolbar.Location = new Point(0, 0);
        pnlToolbar.Margin = new Padding(5);
        pnlToolbar.Name = "pnlToolbar";
        pnlToolbar.Padding = new Padding(5);
        pnlToolbar.RightToLeft = RightToLeft.Yes;
        pnlToolbar.Size = new Size(1473, 47);
        pnlToolbar.TabIndex = 3;
        pnlToolbar.WrapContents = false;
        // 
        // button1
        // 
        button1.FlatStyle = FlatStyle.Flat;
        button1.Font = new Font("Microsoft Sans Serif", 10F);
        button1.Location = new Point(1389, 9);
        button1.Margin = new Padding(4);
        button1.Name = "button1";
        button1.Size = new Size(70, 30);
        button1.TabIndex = 0;
        button1.Text = "جديد";
        button1.UseVisualStyleBackColor = true;
        // 
        // btnSave
        // 
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Font = new Font("Microsoft Sans Serif", 10F);
        btnSave.Location = new Point(1311, 9);
        btnSave.Margin = new Padding(4);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(70, 30);
        btnSave.TabIndex = 1;
        btnSave.Text = "حفظ";
        btnSave.UseVisualStyleBackColor = true;
        // 
        // btnEdit
        // 
        btnEdit.FlatStyle = FlatStyle.Flat;
        btnEdit.Font = new Font("Microsoft Sans Serif", 10F);
        btnEdit.Location = new Point(1233, 9);
        btnEdit.Margin = new Padding(4);
        btnEdit.Name = "btnEdit";
        btnEdit.Size = new Size(70, 30);
        btnEdit.TabIndex = 2;
        btnEdit.Text = "تعديل";
        btnEdit.UseVisualStyleBackColor = true;
        // 
        // btnDelete
        // 
        btnDelete.FlatStyle = FlatStyle.Flat;
        btnDelete.Font = new Font("Microsoft Sans Serif", 10F);
        btnDelete.Location = new Point(1155, 9);
        btnDelete.Margin = new Padding(4);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(70, 30);
        btnDelete.TabIndex = 3;
        btnDelete.Text = "حذف";
        btnDelete.UseVisualStyleBackColor = true;
        // 
        // button2
        // 
        button2.FlatStyle = FlatStyle.Flat;
        button2.Font = new Font("Microsoft Sans Serif", 10F);
        button2.Location = new Point(1077, 9);
        button2.Margin = new Padding(4);
        button2.Name = "button2";
        button2.Size = new Size(70, 30);
        button2.TabIndex = 4;
        button2.Text = "تحديث";
        button2.UseVisualStyleBackColor = true;
        // 
        // btnClose
        // 
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Microsoft Sans Serif", 10F);
        btnClose.Location = new Point(999, 9);
        btnClose.Margin = new Padding(4);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(70, 30);
        btnClose.TabIndex = 5;
        btnClose.Text = "اغلاق";
        btnClose.UseVisualStyleBackColor = true;
        btnClose.Click += BtnClose_Click;
        // 
        // btnFirst
        // 
        btnFirst.FlatStyle = FlatStyle.Flat;
        btnFirst.Font = new Font("Microsoft Sans Serif", 10F);
        btnFirst.Location = new Point(921, 9);
        btnFirst.Margin = new Padding(4);
        btnFirst.Name = "btnFirst";
        btnFirst.Size = new Size(70, 30);
        btnFirst.TabIndex = 6;
        btnFirst.Text = "الاول ";
        btnFirst.UseVisualStyleBackColor = true;
        // 
        // btnPrevious
        // 
        btnPrevious.FlatStyle = FlatStyle.Flat;
        btnPrevious.Font = new Font("Microsoft Sans Serif", 10F);
        btnPrevious.Location = new Point(843, 9);
        btnPrevious.Margin = new Padding(4);
        btnPrevious.Name = "btnPrevious";
        btnPrevious.Size = new Size(70, 30);
        btnPrevious.TabIndex = 7;
        btnPrevious.Text = "السابق";
        btnPrevious.UseVisualStyleBackColor = true;
        // 
        // txtCurrentRecordNo
        // 
        txtCurrentRecordNo.Location = new Point(772, 10);
        txtCurrentRecordNo.Margin = new Padding(5);
        txtCurrentRecordNo.Multiline = true;
        txtCurrentRecordNo.Name = "txtCurrentRecordNo";
        txtCurrentRecordNo.ReadOnly = true;
        txtCurrentRecordNo.Size = new Size(62, 30);
        txtCurrentRecordNo.TabIndex = 8;
        txtCurrentRecordNo.TextAlign = HorizontalAlignment.Center;
        // 
        // btnNext
        // 
        btnNext.FlatStyle = FlatStyle.Flat;
        btnNext.Font = new Font("Microsoft Sans Serif", 10F);
        btnNext.Location = new Point(693, 9);
        btnNext.Margin = new Padding(4);
        btnNext.Name = "btnNext";
        btnNext.Size = new Size(70, 30);
        btnNext.TabIndex = 9;
        btnNext.Text = "التالي";
        btnNext.UseVisualStyleBackColor = true;
        // 
        // btnLast
        // 
        btnLast.FlatStyle = FlatStyle.Flat;
        btnLast.Font = new Font("Microsoft Sans Serif", 10F);
        btnLast.Location = new Point(615, 9);
        btnLast.Margin = new Padding(4);
        btnLast.Name = "btnLast";
        btnLast.Size = new Size(70, 30);
        btnLast.TabIndex = 10;
        btnLast.Text = "الاخير";
        btnLast.UseVisualStyleBackColor = true;
        // 
        // btnUndo
        // 
        btnUndo.FlatStyle = FlatStyle.Flat;
        btnUndo.Font = new Font("Microsoft Sans Serif", 10F);
        btnUndo.Location = new Point(537, 9);
        btnUndo.Margin = new Padding(4);
        btnUndo.Name = "btnUndo";
        btnUndo.Size = new Size(70, 30);
        btnUndo.TabIndex = 11;
        btnUndo.Text = "تراجع";
        btnUndo.UseVisualStyleBackColor = true;
        // 
        // btnSearch
        // 
        btnSearch.FlatStyle = FlatStyle.Flat;
        btnSearch.Font = new Font("Microsoft Sans Serif", 10F);
        btnSearch.Location = new Point(459, 9);
        btnSearch.Margin = new Padding(4);
        btnSearch.Name = "btnSearch";
        btnSearch.Size = new Size(70, 30);
        btnSearch.TabIndex = 12;
        btnSearch.Text = "بحث";
        btnSearch.UseVisualStyleBackColor = true;
        // 
        // btnPrint
        // 
        btnPrint.FlatStyle = FlatStyle.Flat;
        btnPrint.Font = new Font("Microsoft Sans Serif", 10F);
        btnPrint.Location = new Point(381, 9);
        btnPrint.Margin = new Padding(4);
        btnPrint.Name = "btnPrint";
        btnPrint.Size = new Size(70, 30);
        btnPrint.TabIndex = 13;
        btnPrint.Text = "طباعة";
        btnPrint.UseVisualStyleBackColor = true;
        // 
        // button4
        // 
        button4.FlatStyle = FlatStyle.Flat;
        button4.Font = new Font("Microsoft Sans Serif", 10F);
        button4.Location = new Point(303, 9);
        button4.Margin = new Padding(4);
        button4.Name = "button4";
        button4.Size = new Size(70, 30);
        button4.TabIndex = 14;
        button4.Text = "اعتماد";
        button4.UseVisualStyleBackColor = true;
        // 
        // lblTitle
        // 
        lblTitle.AutoSize = true;
        lblTitle.BackColor = Color.FromArgb(192, 192, 255);
        lblTitle.Dock = DockStyle.Right;
        lblTitle.Font = new Font("Segoe UI", 16F);
        lblTitle.Location = new Point(1473, 0);
        lblTitle.Name = "lblTitle";
        lblTitle.Padding = new Padding(0, 0, 15, 0);
        lblTitle.RightToLeft = RightToLeft.Yes;
        lblTitle.Size = new Size(181, 37);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "ادارة الشركات";
        lblTitle.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // tlpAuditInfo
        // 
        tlpAuditInfo.AccessibleName = "البوليصه";
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
        tlpAuditInfo.Location = new Point(0, 1020);
        tlpAuditInfo.Margin = new Padding(0);
        tlpAuditInfo.Name = "tlpAuditInfo";
        tlpAuditInfo.Padding = new Padding(8);
        tlpAuditInfo.RightToLeft = RightToLeft.Yes;
        tlpAuditInfo.RowCount = 1;
        tlpAuditInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpAuditInfo.Size = new Size(1654, 40);
        tlpAuditInfo.TabIndex = 15;
        // 
        // lblPrintCount
        // 
        lblPrintCount.BackColor = Color.FromArgb(192, 255, 255);
        lblPrintCount.Dock = DockStyle.Fill;
        lblPrintCount.Font = new Font("Microsoft Sans Serif", 9F);
        lblPrintCount.Location = new Point(11, 8);
        lblPrintCount.Name = "lblPrintCount";
        lblPrintCount.RightToLeft = RightToLeft.No;
        lblPrintCount.Size = new Size(192, 24);
        lblPrintCount.TabIndex = 8;
        lblPrintCount.Text = "عدد مرات الطباعة :- 00";
        lblPrintCount.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblLastPrintedAt
        // 
        lblLastPrintedAt.BackColor = Color.FromArgb(192, 255, 255);
        lblLastPrintedAt.Dock = DockStyle.Fill;
        lblLastPrintedAt.Font = new Font("Microsoft Sans Serif", 9F);
        lblLastPrintedAt.Location = new Point(209, 8);
        lblLastPrintedAt.Name = "lblLastPrintedAt";
        lblLastPrintedAt.RightToLeft = RightToLeft.No;
        lblLastPrintedAt.Size = new Size(256, 24);
        lblLastPrintedAt.TabIndex = 5;
        lblLastPrintedAt.Text = "تاريخ اخر طباعة:  27/09/2026";
        lblLastPrintedAt.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblEditCount
        // 
        lblEditCount.BackColor = Color.FromArgb(192, 255, 255);
        lblEditCount.Dock = DockStyle.Fill;
        lblEditCount.Font = new Font("Microsoft Sans Serif", 9F);
        lblEditCount.Location = new Point(471, 8);
        lblEditCount.Name = "lblEditCount";
        lblEditCount.RightToLeft = RightToLeft.No;
        lblEditCount.Size = new Size(190, 24);
        lblEditCount.TabIndex = 4;
        lblEditCount.Text = "عدد التعديلات :- 00";
        lblEditCount.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblModifiedAt
        // 
        lblModifiedAt.BackColor = Color.FromArgb(192, 255, 255);
        lblModifiedAt.Dock = DockStyle.Fill;
        lblModifiedAt.Font = new Font("Microsoft Sans Serif", 9F);
        lblModifiedAt.Location = new Point(667, 8);
        lblModifiedAt.Name = "lblModifiedAt";
        lblModifiedAt.RightToLeft = RightToLeft.No;
        lblModifiedAt.Size = new Size(256, 24);
        lblModifiedAt.TabIndex = 3;
        lblModifiedAt.Text = " تاريخ التعديل:  27/09/2026";
        lblModifiedAt.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblModifiedBy
        // 
        lblModifiedBy.BackColor = Color.FromArgb(192, 255, 255);
        lblModifiedBy.Dock = DockStyle.Fill;
        lblModifiedBy.Font = new Font("Microsoft Sans Serif", 9F);
        lblModifiedBy.Location = new Point(929, 8);
        lblModifiedBy.Name = "lblModifiedBy";
        lblModifiedBy.RightToLeft = RightToLeft.No;
        lblModifiedBy.Size = new Size(223, 24);
        lblModifiedBy.TabIndex = 2;
        lblModifiedBy.Text = "عدل بواسطة: - مدير النظام";
        lblModifiedBy.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblCreatedAt
        // 
        lblCreatedAt.BackColor = Color.FromArgb(192, 255, 255);
        lblCreatedAt.Dock = DockStyle.Fill;
        lblCreatedAt.Font = new Font("Microsoft Sans Serif", 9F);
        lblCreatedAt.Location = new Point(1158, 8);
        lblCreatedAt.Name = "lblCreatedAt";
        lblCreatedAt.RightToLeft = RightToLeft.No;
        lblCreatedAt.Size = new Size(256, 24);
        lblCreatedAt.TabIndex = 1;
        lblCreatedAt.Text = " تاريخ الانشاء:  27/09/2026";
        lblCreatedAt.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblCreatedBy
        // 
        lblCreatedBy.BackColor = Color.FromArgb(192, 255, 255);
        lblCreatedBy.Dock = DockStyle.Fill;
        lblCreatedBy.Font = new Font("Microsoft Sans Serif", 9F);
        lblCreatedBy.Location = new Point(1420, 8);
        lblCreatedBy.Name = "lblCreatedBy";
        lblCreatedBy.RightToLeft = RightToLeft.No;
        lblCreatedBy.Size = new Size(223, 24);
        lblCreatedBy.TabIndex = 0;
        lblCreatedBy.Text = " أنشأ بواسطة: - مدير النظام";
        lblCreatedBy.TextAlign = ContentAlignment.MiddleRight;
        // 
        // tabStatus
        // 
        tabStatus.AutoScroll = true;
        tabStatus.BackColor = Color.FromArgb(248, 250, 252);
        tabStatus.Location = new Point(4, 32);
        tabStatus.Name = "tabStatus";
        tabStatus.Padding = new Padding(16);
        tabStatus.Size = new Size(1646, 977);
        tabStatus.TabIndex = 2;
        tabStatus.Text = "الحالة";
        // 
        // tabBasicData
        // 
        tabBasicData.AutoScroll = true;
        tabBasicData.BackColor = Color.FromArgb(248, 250, 252);
        tabBasicData.Location = new Point(4, 32);
        tabBasicData.Name = "tabBasicData";
        tabBasicData.Padding = new Padding(16);
        tabBasicData.Size = new Size(1646, 977);
        tabBasicData.TabIndex = 1;
        tabBasicData.Text = "البيانات الأظافية";
        // 
        // tabCompanies
        // 
        tabCompanies.AutoScroll = true;
        tabCompanies.BackColor = Color.LightCyan;
        tabCompanies.Controls.Add(tabCompaniesSection);
        tabCompanies.Location = new Point(4, 32);
        tabCompanies.Name = "tabCompanies";
        tabCompanies.Padding = new Padding(16);
        tabCompanies.Size = new Size(1646, 977);
        tabCompanies.TabIndex = 0;
        tabCompanies.Text = "الشركات";
        // 
        // tabCompaniesSection
        // 
        tabCompaniesSection.BackColor = Color.LightCyan;
        tabCompaniesSection.Controls.Add(tabCompaniesFields);
        tabCompaniesSection.Controls.Add(groupBox1);
        tabCompaniesSection.Dock = DockStyle.Fill;
        tabCompaniesSection.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        tabCompaniesSection.ForeColor = Color.FromArgb(0, 53, 128);
        tabCompaniesSection.Location = new Point(16, 16);
        tabCompaniesSection.Name = "tabCompaniesSection";
        tabCompaniesSection.Padding = new Padding(16, 24, 16, 16);
        tabCompaniesSection.Size = new Size(1614, 945);
        tabCompaniesSection.TabIndex = 0;
        tabCompaniesSection.TabStop = false;
        tabCompaniesSection.Text = "بينات الشركة الاساسية";
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
        tabCompaniesFields.Name = "tabCompaniesFields";
        tabCompaniesFields.RightToLeft = RightToLeft.Yes;
        tabCompaniesFields.RowCount = 11;
        tabCompaniesFields.RowStyles.Add(new RowStyle(SizeType.Percent, 9.090217F));
        tabCompaniesFields.RowStyles.Add(new RowStyle(SizeType.Percent, 9.090215F));
        tabCompaniesFields.RowStyles.Add(new RowStyle(SizeType.Percent, 9.090213F));
        tabCompaniesFields.RowStyles.Add(new RowStyle(SizeType.Percent, 9.090213F));
        tabCompaniesFields.RowStyles.Add(new RowStyle(SizeType.Percent, 9.090213F));
        tabCompaniesFields.RowStyles.Add(new RowStyle(SizeType.Percent, 9.090213F));
        tabCompaniesFields.RowStyles.Add(new RowStyle(SizeType.Percent, 9.090213F));
        tabCompaniesFields.RowStyles.Add(new RowStyle(SizeType.Percent, 9.090213F));
        tabCompaniesFields.RowStyles.Add(new RowStyle(SizeType.Percent, 9.094036F));
        tabCompaniesFields.RowStyles.Add(new RowStyle(SizeType.Percent, 9.094036F));
        tabCompaniesFields.RowStyles.Add(new RowStyle(SizeType.Percent, 9.090215F));
        tabCompaniesFields.Size = new Size(1310, 433);
        tabCompaniesFields.TabIndex = 3;
        // 
        // txtAddress
        // 
        txtAddress.AccessibleName = "العنوان التفصيلي";
        txtAddress.BackColor = Color.White;
        txtAddress.Dock = DockStyle.Fill;
        txtAddress.Font = new Font("Segoe UI", 11F);
        txtAddress.ForeColor = Color.FromArgb(16, 24, 40);
        txtAddress.Location = new Point(3, 237);
        txtAddress.Name = "txtAddress";
        txtAddress.RightToLeft = RightToLeft.Yes;
        txtAddress.Size = new Size(432, 32);
        txtAddress.TabIndex = 15;
        txtAddress.Tag = "FLD-COMPANY-NAME-EN";
        // 
        // label5
        // 
        label5.Dock = DockStyle.Fill;
        label5.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label5.ForeColor = Color.FromArgb(16, 24, 40);
        label5.Location = new Point(441, 237);
        label5.Margin = new Padding(3);
        label5.Name = "label5";
        label5.Size = new Size(212, 33);
        label5.TabIndex = 189;
        label5.Text = "العنوان التفصيلي";
        label5.TextAlign = ContentAlignment.MiddleRight;
        // 
        // label9
        // 
        label9.Dock = DockStyle.Fill;
        label9.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label9.ForeColor = Color.FromArgb(16, 24, 40);
        label9.Location = new Point(1095, 159);
        label9.Margin = new Padding(3);
        label9.Name = "label9";
        label9.Size = new Size(212, 33);
        label9.TabIndex = 188;
        label9.Text = "نشط";
        label9.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cmbCity
        // 
        cmbCity.AccessibleName = "المدينة";
        cmbCity.BackColor = Color.FromArgb(255, 249, 219);
        cmbCity.Dock = DockStyle.Fill;
        cmbCity.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        cmbCity.FormattingEnabled = true;
        cmbCity.Location = new Point(659, 237);
        cmbCity.Name = "cmbCity";
        cmbCity.RightToLeft = RightToLeft.No;
        cmbCity.Size = new Size(430, 28);
        cmbCity.TabIndex = 14;
        // 
        // cmbCountry
        // 
        cmbCountry.AccessibleName = "الدولة";
        cmbCountry.BackColor = Color.FromArgb(255, 249, 219);
        cmbCountry.Dock = DockStyle.Fill;
        cmbCountry.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        cmbCountry.FormattingEnabled = true;
        cmbCountry.Location = new Point(659, 198);
        cmbCountry.Name = "cmbCountry";
        cmbCountry.RightToLeft = RightToLeft.No;
        cmbCountry.Size = new Size(430, 28);
        cmbCountry.TabIndex = 12;
        // 
        // txtNotes
        // 
        txtNotes.AccessibleName = "ملاحظات";
        txtNotes.BackColor = Color.White;
        txtNotes.Dock = DockStyle.Fill;
        txtNotes.Font = new Font("Segoe UI", 11F);
        txtNotes.ForeColor = Color.FromArgb(16, 24, 40);
        txtNotes.Location = new Point(3, 354);
        txtNotes.Multiline = true;
        txtNotes.Name = "txtNotes";
        txtNotes.RightToLeft = RightToLeft.Yes;
        txtNotes.Size = new Size(432, 33);
        txtNotes.TabIndex = 21;
        txtNotes.Tag = "FLD-COMPANY-NAME-EN";
        // 
        // txtPostalCode
        // 
        txtPostalCode.AccessibleName = "الرمز البريدي";
        txtPostalCode.BackColor = Color.White;
        txtPostalCode.Dock = DockStyle.Fill;
        txtPostalCode.Font = new Font("Segoe UI", 11F);
        txtPostalCode.ForeColor = Color.FromArgb(16, 24, 40);
        txtPostalCode.Location = new Point(659, 354);
        txtPostalCode.Name = "txtPostalCode";
        txtPostalCode.RightToLeft = RightToLeft.Yes;
        txtPostalCode.Size = new Size(430, 32);
        txtPostalCode.TabIndex = 20;
        txtPostalCode.Tag = "FLD-COMPANY-NAME-EN";
        // 
        // txtWebsite
        // 
        txtWebsite.AccessibleName = "الموقع الالكتروني";
        txtWebsite.BackColor = Color.White;
        txtWebsite.Dock = DockStyle.Fill;
        txtWebsite.Font = new Font("Segoe UI", 11F);
        txtWebsite.ForeColor = Color.FromArgb(16, 24, 40);
        txtWebsite.Location = new Point(3, 315);
        txtWebsite.Name = "txtWebsite";
        txtWebsite.RightToLeft = RightToLeft.Yes;
        txtWebsite.Size = new Size(432, 32);
        txtWebsite.TabIndex = 19;
        txtWebsite.Tag = "FLD-COMPANY-NAME-EN";
        // 
        // txtEmail
        // 
        txtEmail.AccessibleName = "البريد الالكتروني";
        txtEmail.BackColor = Color.White;
        txtEmail.Dock = DockStyle.Fill;
        txtEmail.Font = new Font("Segoe UI", 11F);
        txtEmail.ForeColor = Color.FromArgb(16, 24, 40);
        txtEmail.Location = new Point(659, 315);
        txtEmail.Name = "txtEmail";
        txtEmail.RightToLeft = RightToLeft.Yes;
        txtEmail.Size = new Size(430, 32);
        txtEmail.TabIndex = 18;
        txtEmail.Tag = "FLD-COMPANY-NAME-EN";
        // 
        // txtMobile
        // 
        txtMobile.AccessibleName = "رقم الجوال";
        txtMobile.BackColor = Color.White;
        txtMobile.Dock = DockStyle.Fill;
        txtMobile.Font = new Font("Segoe UI", 11F);
        txtMobile.ForeColor = Color.FromArgb(16, 24, 40);
        txtMobile.Location = new Point(3, 276);
        txtMobile.Name = "txtMobile";
        txtMobile.RightToLeft = RightToLeft.Yes;
        txtMobile.Size = new Size(432, 32);
        txtMobile.TabIndex = 17;
        txtMobile.Tag = "FLD-COMPANY-NAME-EN";
        // 
        // txtPhone
        // 
        txtPhone.AccessibleName = "رقم الهاتف";
        txtPhone.BackColor = Color.White;
        txtPhone.Dock = DockStyle.Fill;
        txtPhone.Font = new Font("Segoe UI", 11F);
        txtPhone.ForeColor = Color.FromArgb(16, 24, 40);
        txtPhone.Location = new Point(659, 276);
        txtPhone.Name = "txtPhone";
        txtPhone.RightToLeft = RightToLeft.Yes;
        txtPhone.Size = new Size(430, 32);
        txtPhone.TabIndex = 16;
        txtPhone.Tag = "FLD-COMPANY-NAME-EN";
        // 
        // label24
        // 
        label24.Dock = DockStyle.Fill;
        label24.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label24.ForeColor = Color.FromArgb(16, 24, 40);
        label24.Location = new Point(441, 354);
        label24.Margin = new Padding(3);
        label24.Name = "label24";
        label24.Size = new Size(212, 33);
        label24.TabIndex = 149;
        label24.Text = "ملاحظات";
        label24.TextAlign = ContentAlignment.MiddleRight;
        // 
        // label22
        // 
        label22.Dock = DockStyle.Fill;
        label22.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label22.ForeColor = Color.FromArgb(16, 24, 40);
        label22.Location = new Point(1095, 354);
        label22.Margin = new Padding(3);
        label22.Name = "label22";
        label22.Size = new Size(212, 33);
        label22.TabIndex = 147;
        label22.Text = "الرمز البريدي";
        label22.TextAlign = ContentAlignment.MiddleRight;
        // 
        // label20
        // 
        label20.Dock = DockStyle.Fill;
        label20.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label20.ForeColor = Color.FromArgb(16, 24, 40);
        label20.Location = new Point(441, 315);
        label20.Margin = new Padding(3);
        label20.Name = "label20";
        label20.Size = new Size(212, 33);
        label20.TabIndex = 145;
        label20.Text = "الموقع الالكتروني";
        label20.TextAlign = ContentAlignment.MiddleRight;
        // 
        // label18
        // 
        label18.Dock = DockStyle.Fill;
        label18.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label18.ForeColor = Color.FromArgb(16, 24, 40);
        label18.Location = new Point(1095, 315);
        label18.Margin = new Padding(3);
        label18.Name = "label18";
        label18.Size = new Size(212, 33);
        label18.TabIndex = 143;
        label18.Text = "البريد الالكتروني";
        label18.TextAlign = ContentAlignment.MiddleRight;
        // 
        // label15
        // 
        label15.Dock = DockStyle.Fill;
        label15.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label15.ForeColor = Color.FromArgb(16, 24, 40);
        label15.Location = new Point(441, 276);
        label15.Margin = new Padding(3);
        label15.Name = "label15";
        label15.Size = new Size(212, 33);
        label15.TabIndex = 141;
        label15.Text = "رقم الجوال";
        label15.TextAlign = ContentAlignment.MiddleRight;
        // 
        // label11
        // 
        label11.Dock = DockStyle.Fill;
        label11.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label11.ForeColor = Color.FromArgb(16, 24, 40);
        label11.Location = new Point(1095, 276);
        label11.Margin = new Padding(3);
        label11.Name = "label11";
        label11.Size = new Size(212, 33);
        label11.TabIndex = 139;
        label11.Text = "رقم الهاتف";
        label11.TextAlign = ContentAlignment.MiddleRight;
        // 
        // label3
        // 
        label3.Dock = DockStyle.Fill;
        label3.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label3.ForeColor = Color.FromArgb(16, 24, 40);
        label3.Location = new Point(1095, 237);
        label3.Margin = new Padding(3);
        label3.Name = "label3";
        label3.Size = new Size(212, 33);
        label3.TabIndex = 135;
        label3.Text = "المدينة";
        label3.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtCommercialRegistrationNo
        // 
        txtCommercialRegistrationNo.AccessibleName = "رقم السجل التجاري";
        txtCommercialRegistrationNo.BackColor = Color.White;
        txtCommercialRegistrationNo.Dock = DockStyle.Fill;
        txtCommercialRegistrationNo.Font = new Font("Segoe UI", 11F);
        txtCommercialRegistrationNo.ForeColor = Color.FromArgb(16, 24, 40);
        txtCommercialRegistrationNo.Location = new Point(3, 159);
        txtCommercialRegistrationNo.Name = "txtCommercialRegistrationNo";
        txtCommercialRegistrationNo.RightToLeft = RightToLeft.Yes;
        txtCommercialRegistrationNo.Size = new Size(432, 32);
        txtCommercialRegistrationNo.TabIndex = 11;
        txtCommercialRegistrationNo.Tag = "FLD-COMPANY-NAME-EN";
        // 
        // cmbGovernorate
        // 
        cmbGovernorate.AccessibleName = "المحافظة";
        cmbGovernorate.Dock = DockStyle.Fill;
        cmbGovernorate.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        cmbGovernorate.FormattingEnabled = true;
        cmbGovernorate.Location = new Point(3, 198);
        cmbGovernorate.Name = "cmbGovernorate";
        cmbGovernorate.RightToLeft = RightToLeft.No;
        cmbGovernorate.Size = new Size(432, 28);
        cmbGovernorate.TabIndex = 13;
        // 
        // cmbBaseCurrency
        // 
        cmbBaseCurrency.AccessibleName = "العملة الاساسية";
        cmbBaseCurrency.BackColor = Color.FromArgb(255, 249, 219);
        cmbBaseCurrency.Dock = DockStyle.Fill;
        cmbBaseCurrency.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        cmbBaseCurrency.FormattingEnabled = true;
        cmbBaseCurrency.Location = new Point(3, 120);
        cmbBaseCurrency.Name = "cmbBaseCurrency";
        cmbBaseCurrency.RightToLeft = RightToLeft.No;
        cmbBaseCurrency.Size = new Size(432, 28);
        cmbBaseCurrency.TabIndex = 9;
        // 
        // txtTaxNumber
        // 
        txtTaxNumber.AccessibleName = "الرقم الضريبي";
        txtTaxNumber.BackColor = Color.White;
        txtTaxNumber.Dock = DockStyle.Fill;
        txtTaxNumber.Font = new Font("Segoe UI", 11F);
        txtTaxNumber.ForeColor = Color.FromArgb(16, 24, 40);
        txtTaxNumber.Location = new Point(659, 120);
        txtTaxNumber.Name = "txtTaxNumber";
        txtTaxNumber.RightToLeft = RightToLeft.Yes;
        txtTaxNumber.Size = new Size(430, 32);
        txtTaxNumber.TabIndex = 8;
        txtTaxNumber.Tag = "FLD-COMPANY-NAME-EN";
        // 
        // cmbActivityType
        // 
        cmbActivityType.AccessibleName = "نوع النشاط";
        cmbActivityType.BackColor = Color.FromArgb(255, 249, 219);
        cmbActivityType.Dock = DockStyle.Fill;
        cmbActivityType.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        cmbActivityType.FormattingEnabled = true;
        cmbActivityType.Location = new Point(3, 81);
        cmbActivityType.Name = "cmbActivityType";
        cmbActivityType.RightToLeft = RightToLeft.No;
        cmbActivityType.Size = new Size(432, 28);
        cmbActivityType.TabIndex = 7;
        // 
        // cmbCompanyGroup
        // 
        cmbCompanyGroup.AccessibleName = "مجموعة الشركة";
        cmbCompanyGroup.Dock = DockStyle.Fill;
        cmbCompanyGroup.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        cmbCompanyGroup.FormattingEnabled = true;
        cmbCompanyGroup.Location = new Point(659, 81);
        cmbCompanyGroup.Name = "cmbCompanyGroup";
        cmbCompanyGroup.RightToLeft = RightToLeft.No;
        cmbCompanyGroup.Size = new Size(430, 28);
        cmbCompanyGroup.TabIndex = 6;
        // 
        // label2
        // 
        label2.Dock = DockStyle.Fill;
        label2.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label2.ForeColor = Color.FromArgb(16, 24, 40);
        label2.Location = new Point(1095, 81);
        label2.Margin = new Padding(3);
        label2.Name = "label2";
        label2.Size = new Size(212, 33);
        label2.TabIndex = 29;
        label2.Text = "مجموعة الشركات";
        label2.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtCompanyNameEn
        // 
        txtCompanyNameEn.AccessibleName = "اسم الشركة الإنجليزي";
        txtCompanyNameEn.BackColor = Color.White;
        txtCompanyNameEn.Dock = DockStyle.Fill;
        txtCompanyNameEn.Font = new Font("Segoe UI", 11F);
        txtCompanyNameEn.ForeColor = Color.FromArgb(16, 24, 40);
        txtCompanyNameEn.Location = new Point(3, 42);
        txtCompanyNameEn.Name = "txtCompanyNameEn";
        txtCompanyNameEn.RightToLeft = RightToLeft.Yes;
        txtCompanyNameEn.Size = new Size(432, 32);
        txtCompanyNameEn.TabIndex = 5;
        txtCompanyNameEn.Tag = "FLD-COMPANY-NAME-EN";
        // 
        // lblCompanyNameEn
        // 
        lblCompanyNameEn.Dock = DockStyle.Fill;
        lblCompanyNameEn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblCompanyNameEn.ForeColor = Color.FromArgb(16, 24, 40);
        lblCompanyNameEn.Location = new Point(441, 42);
        lblCompanyNameEn.Margin = new Padding(3);
        lblCompanyNameEn.Name = "lblCompanyNameEn";
        lblCompanyNameEn.Size = new Size(212, 33);
        lblCompanyNameEn.TabIndex = 27;
        lblCompanyNameEn.Text = "اسم الشركة الإنجليزي";
        lblCompanyNameEn.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtCompanyId
        // 
        txtCompanyId.AccessibleName = "معرف الشريكة";
        txtCompanyId.BackColor = Color.White;
        txtCompanyId.Dock = DockStyle.Fill;
        txtCompanyId.Font = new Font("Segoe UI", 11F);
        txtCompanyId.ForeColor = Color.FromArgb(16, 24, 40);
        txtCompanyId.Location = new Point(659, 3);
        txtCompanyId.Name = "txtCompanyId";
        txtCompanyId.ReadOnly = true;
        txtCompanyId.RightToLeft = RightToLeft.Yes;
        txtCompanyId.Size = new Size(430, 32);
        txtCompanyId.TabIndex = 26;
        txtCompanyId.TabStop = false;
        txtCompanyId.Tag = "FLD-COMPANY-NAME-AR";
        // 
        // label6
        // 
        label6.Dock = DockStyle.Fill;
        label6.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label6.ForeColor = Color.FromArgb(16, 24, 40);
        label6.Location = new Point(1095, 120);
        label6.Margin = new Padding(3);
        label6.Name = "label6";
        label6.Size = new Size(212, 33);
        label6.TabIndex = 25;
        label6.Text = "الرقم الضريبي";
        label6.TextAlign = ContentAlignment.MiddleRight;
        // 
        // label16
        // 
        label16.Dock = DockStyle.Fill;
        label16.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label16.ForeColor = SystemColors.ActiveCaptionText;
        label16.Location = new Point(441, 198);
        label16.Margin = new Padding(3);
        label16.Name = "label16";
        label16.Size = new Size(212, 33);
        label16.TabIndex = 22;
        label16.Text = "المحافظة";
        label16.TextAlign = ContentAlignment.MiddleRight;
        // 
        // label14
        // 
        label14.Dock = DockStyle.Fill;
        label14.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label14.ForeColor = Color.FromArgb(180, 35, 24);
        label14.Location = new Point(1095, 198);
        label14.Margin = new Padding(3);
        label14.Name = "label14";
        label14.Size = new Size(212, 33);
        label14.TabIndex = 20;
        label14.Text = "الدولة *";
        label14.TextAlign = ContentAlignment.MiddleRight;
        // 
        // label12
        // 
        label12.Dock = DockStyle.Fill;
        label12.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label12.ForeColor = SystemColors.ActiveCaptionText;
        label12.Location = new Point(441, 159);
        label12.Margin = new Padding(3);
        label12.Name = "label12";
        label12.Size = new Size(212, 33);
        label12.TabIndex = 18;
        label12.Text = "رقم السجل التجاري";
        label12.TextAlign = ContentAlignment.MiddleRight;
        // 
        // label8
        // 
        label8.Dock = DockStyle.Fill;
        label8.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label8.ForeColor = Color.FromArgb(180, 35, 24);
        label8.Location = new Point(441, 120);
        label8.Margin = new Padding(3);
        label8.Name = "label8";
        label8.Size = new Size(212, 33);
        label8.TabIndex = 14;
        label8.Text = "العملة الاساسية *";
        label8.TextAlign = ContentAlignment.MiddleRight;
        // 
        // label4
        // 
        label4.Dock = DockStyle.Fill;
        label4.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label4.ForeColor = Color.FromArgb(180, 35, 24);
        label4.Location = new Point(441, 81);
        label4.Margin = new Padding(3);
        label4.Name = "label4";
        label4.Size = new Size(212, 33);
        label4.TabIndex = 10;
        label4.Text = "نوع النشاط *";
        label4.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtCompanyCode
        // 
        txtCompanyCode.AccessibleName = "كود الشركة";
        txtCompanyCode.BackColor = Color.White;
        txtCompanyCode.Dock = DockStyle.Fill;
        txtCompanyCode.Font = new Font("Segoe UI", 11F);
        txtCompanyCode.ForeColor = Color.FromArgb(16, 24, 40);
        txtCompanyCode.Location = new Point(3, 3);
        txtCompanyCode.Name = "txtCompanyCode";
        txtCompanyCode.ReadOnly = true;
        txtCompanyCode.RightToLeft = RightToLeft.Yes;
        txtCompanyCode.Size = new Size(432, 32);
        txtCompanyCode.TabIndex = 7;
        txtCompanyCode.TabStop = false;
        txtCompanyCode.Tag = "FLD-COMPANY-NAME-AR";
        // 
        // label1
        // 
        label1.Dock = DockStyle.Fill;
        label1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label1.ForeColor = SystemColors.ActiveCaptionText;
        label1.Location = new Point(441, 3);
        label1.Margin = new Padding(3);
        label1.Name = "label1";
        label1.Size = new Size(212, 33);
        label1.TabIndex = 6;
        label1.Text = "كود الشركة";
        label1.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblCompanyId
        // 
        lblCompanyId.Dock = DockStyle.Fill;
        lblCompanyId.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblCompanyId.ForeColor = SystemColors.ActiveCaptionText;
        lblCompanyId.Location = new Point(1095, 3);
        lblCompanyId.Margin = new Padding(3);
        lblCompanyId.Name = "lblCompanyId";
        lblCompanyId.Size = new Size(212, 33);
        lblCompanyId.TabIndex = 0;
        lblCompanyId.Text = "معرف الشركة ";
        lblCompanyId.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblCompanyNameAr
        // 
        lblCompanyNameAr.Dock = DockStyle.Fill;
        lblCompanyNameAr.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblCompanyNameAr.ForeColor = Color.FromArgb(180, 35, 24);
        lblCompanyNameAr.Location = new Point(1095, 42);
        lblCompanyNameAr.Margin = new Padding(3);
        lblCompanyNameAr.Name = "lblCompanyNameAr";
        lblCompanyNameAr.Size = new Size(212, 33);
        lblCompanyNameAr.TabIndex = 2;
        lblCompanyNameAr.Text = "اسم الشركة العربي *";
        lblCompanyNameAr.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtCompanyNameAr
        // 
        txtCompanyNameAr.AccessibleName = "اسم الشركة العربي";
        txtCompanyNameAr.BackColor = Color.FromArgb(255, 249, 219);
        txtCompanyNameAr.Dock = DockStyle.Fill;
        txtCompanyNameAr.Font = new Font("Segoe UI", 11F);
        txtCompanyNameAr.ForeColor = Color.FromArgb(16, 24, 40);
        txtCompanyNameAr.Location = new Point(659, 42);
        txtCompanyNameAr.Name = "txtCompanyNameAr";
        txtCompanyNameAr.RightToLeft = RightToLeft.Yes;
        txtCompanyNameAr.Size = new Size(430, 32);
        txtCompanyNameAr.TabIndex = 0;
        txtCompanyNameAr.Tag = "FLD-COMPANY-NAME-AR";
        // 
        // chkIsActive
        // 
        chkIsActive.AccessibleName = "نشط";
        chkIsActive.AutoSize = true;
        chkIsActive.CheckAlign = ContentAlignment.MiddleCenter;
        chkIsActive.Checked = true;
        chkIsActive.CheckState = CheckState.Checked;
        chkIsActive.Dock = DockStyle.Fill;
        chkIsActive.Location = new Point(659, 159);
        chkIsActive.Name = "chkIsActive";
        chkIsActive.Size = new Size(430, 33);
        chkIsActive.TabIndex = 10;
        chkIsActive.UseVisualStyleBackColor = true;
        // 
        // groupBox1
        // 
        groupBox1.BackColor = Color.LightCyan;
        groupBox1.Controls.Add(tableLayoutPanel1);
        groupBox1.Dock = DockStyle.Left;
        groupBox1.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        groupBox1.ForeColor = Color.FromArgb(0, 53, 128);
        groupBox1.Location = new Point(16, 51);
        groupBox1.Name = "groupBox1";
        groupBox1.Padding = new Padding(16, 24, 16, 16);
        groupBox1.Size = new Size(272, 878);
        groupBox1.TabIndex = 2;
        groupBox1.TabStop = false;
        groupBox1.Text = "الهوية";
        // 
        // tableLayoutPanel1
        // 
        tableLayoutPanel1.ColumnCount = 1;
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 1);
        tableLayoutPanel1.Controls.Add(picCompanyLogo, 0, 0);
        tableLayoutPanel1.Dock = DockStyle.Top;
        tableLayoutPanel1.Location = new Point(16, 51);
        tableLayoutPanel1.Name = "tableLayoutPanel1";
        tableLayoutPanel1.RightToLeft = RightToLeft.Yes;
        tableLayoutPanel1.RowCount = 2;
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        tableLayoutPanel1.Size = new Size(240, 257);
        tableLayoutPanel1.TabIndex = 0;
        // 
        // tableLayoutPanel2
        // 
        tableLayoutPanel2.ColumnCount = 2;
        tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableLayoutPanel2.Controls.Add(btnAddLogo, 0, 0);
        tableLayoutPanel2.Controls.Add(btnRemoveLogo, 1, 0);
        tableLayoutPanel2.Dock = DockStyle.Fill;
        tableLayoutPanel2.Location = new Point(3, 208);
        tableLayoutPanel2.Name = "tableLayoutPanel2";
        tableLayoutPanel2.RightToLeft = RightToLeft.Yes;
        tableLayoutPanel2.RowCount = 1;
        tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        tableLayoutPanel2.Size = new Size(234, 46);
        tableLayoutPanel2.TabIndex = 4;
        // 
        // btnAddLogo
        // 
        btnAddLogo.Dock = DockStyle.Fill;
        btnAddLogo.FlatStyle = FlatStyle.Flat;
        btnAddLogo.Font = new Font("Microsoft Sans Serif", 10F);
        btnAddLogo.Location = new Point(121, 4);
        btnAddLogo.Margin = new Padding(4);
        btnAddLogo.Name = "btnAddLogo";
        btnAddLogo.Size = new Size(109, 38);
        btnAddLogo.TabIndex = 16;
        btnAddLogo.Text = "اضافة";
        btnAddLogo.UseVisualStyleBackColor = true;
        // 
        // btnRemoveLogo
        // 
        btnRemoveLogo.Dock = DockStyle.Fill;
        btnRemoveLogo.FlatStyle = FlatStyle.Flat;
        btnRemoveLogo.Font = new Font("Microsoft Sans Serif", 10F);
        btnRemoveLogo.Location = new Point(4, 4);
        btnRemoveLogo.Margin = new Padding(4);
        btnRemoveLogo.Name = "btnRemoveLogo";
        btnRemoveLogo.Size = new Size(109, 38);
        btnRemoveLogo.TabIndex = 15;
        btnRemoveLogo.Text = "حذف";
        btnRemoveLogo.UseVisualStyleBackColor = true;
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
        tabCompanyManagement.AccessibleName = "الشريكات";
        tabCompanyManagement.Controls.Add(tabCompanies);
        tabCompanyManagement.Controls.Add(tabBasicData);
        tabCompanyManagement.Controls.Add(tabStatus);
        tabCompanyManagement.Dock = DockStyle.Fill;
        tabCompanyManagement.Location = new Point(0, 47);
        tabCompanyManagement.Name = "tabCompanyManagement";
        tabCompanyManagement.RightToLeft = RightToLeft.Yes;
        tabCompanyManagement.RightToLeftLayout = true;
        tabCompanyManagement.SelectedIndex = 0;
        tabCompanyManagement.Size = new Size(1654, 1013);
        tabCompanyManagement.TabIndex = 14;
        // 
        // UcCompanyManagement
        // 
        AutoScaleDimensions = new SizeF(120F, 120F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.FromArgb(248, 250, 252);
        Controls.Add(tlpAuditInfo);
        Controls.Add(tabCompanyManagement);
        Controls.Add(pnlHeader);
        Font = new Font("Segoe UI", 10F);
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
    private Label lblTitle;
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
