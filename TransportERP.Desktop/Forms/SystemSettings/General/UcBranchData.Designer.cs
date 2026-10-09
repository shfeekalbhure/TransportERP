#nullable disable
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace TransportERP.Desktop.Forms.SystemSettings.General;

partial class UcBranchData
{
    private TransportERP.Desktop.CoreUI.AuditMetadataControl standardAuditMetadata = null!;

    private TransportERP.Desktop.CoreUI.DesignerCommandBar designerCommandBar = null!;
    private FlowLayoutPanel designerCommandFlow = null!;
    private Panel designerCloseHost = null!;
    private Button standardCommandEdit = null!;
    private Button standardCommandDelete = null!;
    private Button standardCommandCancel = null!;
    private Button standardCommandView = null!;
    private Button standardCommandLast = null!;
    private Button standardCommandNext = null!;
    private Button standardCommandPrevious = null!;
    private Button standardCommandFirst = null!;
    private Button standardCommandPrint = null!;
    private Button standardCommandClose = null!;
    private Button standardCommandRefresh = null!;
    private Button standardCommandImport = null!;
    private Button standardCommandExport = null!;
    private Button standardCommandHelp = null!;

    private IContainer components;
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            components?.Dispose();
            // The conditional page may be detached from tabsFields at runtime.
            tabDocuments?.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        designerCommandBar = new TransportERP.Desktop.CoreUI.DesignerCommandBar();
        designerCommandFlow = new FlowLayoutPanel();
        designerCloseHost = new Panel();
        standardCommandEdit = new Button();
        standardCommandDelete = new Button();
        standardCommandCancel = new Button();
        standardCommandView = new Button();
        standardCommandLast = new Button();
        standardCommandNext = new Button();
        standardCommandPrevious = new Button();
        standardCommandFirst = new Button();
        standardCommandPrint = new Button();
        standardCommandClose = new Button();
        standardCommandRefresh = new Button();
        standardCommandImport = new Button();
        standardCommandExport = new Button();
        standardCommandHelp = new Button();
        components = new Container();
        toolTips = new ToolTip(components);
        btnAdd = new Button();
        btnSave = new Button();
        btnToolbarAddLogo = new Button();
        btnToolbarAddresses = new Button();
        lblStatus = new Label();
        layout = new TableLayoutPanel();
        lblTitle = new Label();
        pnlToolbar = new Panel();
        commandsLayout = new TableLayoutPanel();
        tabsFields = new TabControl();
        tabMain = new TabPage();
        pnlMainFields = new Panel();
        fieldsMain = new TableLayoutPanel();
        lblcboCompany = new Label();
        cboCompany = new ComboBox();
        lbltxtCompanyLocal = new Label();
        txtCompanyLocal = new TextBox();
        lbltxtCompanyForeign = new Label();
        txtCompanyForeign = new TextBox();
        lblchkCompanyTaxGroup = new Label();
        chkCompanyTaxGroup = new CheckBox();
        lbltxtCompanyTaxCurrency = new Label();
        txtCompanyTaxCurrency = new TextBox();
        lbltxtCompanyTaxRegistration = new Label();
        txtCompanyTaxRegistration = new TextBox();
        lbltxtBranchNumber = new Label();
        txtBranchNumber = new TextBox();
        lbltxtYear = new Label();
        txtYear = new TextBox();
        lbltxtNameLocal = new Label();
        txtNameLocal = new TextBox();
        lbltxtNameForeign = new Label();
        txtNameForeign = new TextBox();
        lblcboGroup = new Label();
        cboGroup = new ComboBox();
        lblchkInvoice = new Label();
        chkInvoice = new CheckBox();
        lblchkQr = new Label();
        chkQr = new CheckBox();
        lblchkMain = new Label();
        chkMain = new CheckBox();
        lblchkLite = new Label();
        chkLite = new CheckBox();
        lbltxtConnection = new Label();
        txtConnection = new TextBox();
        lblcboConnection = new Label();
        cboConnection = new ComboBox();
        grpLogo = new GroupBox();
        picLogo = new PictureBox();
        btnAddLogo = new Button();
        tabDetails = new TabPage();
        fieldsDetails = new TableLayoutPanel();
        lbltxtReportHeader = new Label();
        txtReportHeader = new TextBox();
        lbltxtAddress = new Label();
        txtAddress = new TextBox();
        lbltxtSpecifications = new Label();
        txtSpecifications = new TextBox();
        lbltxtPan = new Label();
        txtPan = new TextBox();
        lbltxtTan = new Label();
        txtTan = new TextBox();
        lbltxtTaxNumber = new Label();
        txtTaxNumber = new TextBox();
        lbltxtSequence = new Label();
        txtSequence = new TextBox();
        lbltxtTaxArticle = new Label();
        txtTaxArticle = new TextBox();
        lbltxtStatistics = new Label();
        txtStatistics = new TextBox();
        lblcboCountry = new Label();
        cboCountry = new ComboBox();
        lblcboGovernorate = new Label();
        cboGovernorate = new ComboBox();
        lblcboCity = new Label();
        cboCity = new ComboBox();
        lblchkStopped = new Label();
        chkStopped = new CheckBox();
        tabArchive = new TabPage();
        fieldsArchive = new TableLayoutPanel();
        lblcboDocumentType = new Label();
        cboDocumentType = new ComboBox();
        lbltxtDocumentNumber = new Label();
        txtDocumentNumber = new TextBox();
        lbltxtIssuedAt = new Label();
        txtIssuedAt = new TextBox();
        lbltxtIssuedPlace = new Label();
        txtIssuedPlace = new TextBox();
        lbltxtExpiresAt = new Label();
        txtExpiresAt = new TextBox();
        lbltxtRenewedAt = new Label();
        txtRenewedAt = new TextBox();
        tabDocuments = new TabPage();
        fieldsDocuments = new TableLayoutPanel();
        lbllstDocumentsToBranch = new Label();
        lstDocumentsToBranch = new CheckedListBox();
        lbllstDocumentsToAdministration = new Label();
        lstDocumentsToAdministration = new CheckedListBox();
        tabHeaders = new TabPage();
        fieldsHeaders = new TableLayoutPanel();
        lblcboHeaderType = new Label();
        cboHeaderType = new ComboBox();
        tabAddresses = new TabPage();
        fieldsAddresses = new TableLayoutPanel();
        lbltxtAddressesIdentifiers = new Label();
        addressEditorPanel = new Panel();
        txtAddressesIdentifiers = new TextBox();
        btnSelectAddressesIdentifiers = new Button();
        grpbtnSelectAddressesIdentifiers = new GroupBox();
        lstbtnSelectAddressesIdentifiers = new ListBox();
        lblAddressEmpty = new Label();
        auditInfoContainer = new TableLayoutPanel();
        lbltxtCreatedBy = new Label();
        txtCreatedBy = new TextBox();
        lbltxtUpdatedBy = new Label();
        txtUpdatedBy = new TextBox();
        lbltxtCreatedAt = new Label();
        txtCreatedAt = new TextBox();
        lbltxtUpdatedAt = new Label();
        txtUpdatedAt = new TextBox();
        lbltxtCreatedDevice = new Label();
        txtCreatedDevice = new TextBox();
        lbltxtUpdatedDevice = new Label();
        txtUpdatedDevice = new TextBox();
        lbltxtPrintCount = new Label();
        txtPrintCount = new TextBox();
        lbltxtUpdateCount = new Label();
        txtUpdateCount = new TextBox();
        txtCreatedByCode = new TextBox();
        txtUpdatedByCode = new TextBox();
        layout.SuspendLayout();
        pnlToolbar.SuspendLayout();
        commandsLayout.SuspendLayout();
        tabsFields.SuspendLayout();
        tabMain.SuspendLayout();
        pnlMainFields.SuspendLayout();
        fieldsMain.SuspendLayout();
        grpLogo.SuspendLayout();
        ((ISupportInitialize)picLogo).BeginInit();
        tabDetails.SuspendLayout();
        fieldsDetails.SuspendLayout();
        tabArchive.SuspendLayout();
        fieldsArchive.SuspendLayout();
        tabDocuments.SuspendLayout();
        fieldsDocuments.SuspendLayout();
        tabHeaders.SuspendLayout();
        fieldsHeaders.SuspendLayout();
        tabAddresses.SuspendLayout();
        fieldsAddresses.SuspendLayout();
        addressEditorPanel.SuspendLayout();
        grpbtnSelectAddressesIdentifiers.SuspendLayout();
        auditInfoContainer.SuspendLayout();
        SuspendLayout();
        // 
        // btnAdd
        // 
        btnAdd.AccessibleName = "إضافة";
        btnAdd.Dock = DockStyle.Fill;
        btnAdd.Location = new Point(860, 1);
        btnAdd.Margin = new Padding(1);
        btnAdd.MinimumSize = new Size(144, 28);
        btnAdd.Name = "btnAdd";
        btnAdd.Size = new Size(145, 28);
        btnAdd.TabIndex = 0;
        btnAdd.Text = "إضافة";
        toolTips.SetToolTip(btnAdd, "إضافة");
        btnAdd.UseVisualStyleBackColor = true;
        btnAdd.Click += btnAdd_Click;
        // 
        // btnSave
        // 
        btnSave.AccessibleName = "حفظ";
        btnSave.Dock = DockStyle.Fill;
        btnSave.Enabled = false;
        btnSave.Location = new Point(713, 1);
        btnSave.Margin = new Padding(1);
        btnSave.MinimumSize = new Size(144, 28);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(145, 28);
        btnSave.TabIndex = 1;
        btnSave.Text = "حفظ";
        toolTips.SetToolTip(btnSave, "غير متاح قبل الإضافة أو ربط الخدمة.");
        btnSave.UseVisualStyleBackColor = true;
        // 
        // btnToolbarAddLogo
        // 
        btnToolbarAddLogo.AccessibleName = "إضافة شعار الفرع";
        btnToolbarAddLogo.Dock = DockStyle.Fill;
        btnToolbarAddLogo.Enabled = false;
        btnToolbarAddLogo.Location = new Point(566, 1);
        btnToolbarAddLogo.Margin = new Padding(1);
        btnToolbarAddLogo.MinimumSize = new Size(144, 28);
        btnToolbarAddLogo.Name = "btnToolbarAddLogo";
        btnToolbarAddLogo.Size = new Size(145, 28);
        btnToolbarAddLogo.TabIndex = 2;
        btnToolbarAddLogo.Text = "إضافة شعار الفرع";
        toolTips.SetToolTip(btnToolbarAddLogo, "غير متاح قبل الإضافة أو ربط الخدمة.");
        btnToolbarAddLogo.UseVisualStyleBackColor = true;
        // 
        // btnToolbarAddresses
        // 
        btnToolbarAddresses.AccessibleName = "العناوين والمعرفات";
        btnToolbarAddresses.Dock = DockStyle.Fill;
        btnToolbarAddresses.Enabled = false;
        btnToolbarAddresses.Location = new Point(419, 1);
        btnToolbarAddresses.Margin = new Padding(1);
        btnToolbarAddresses.MinimumSize = new Size(144, 28);
        btnToolbarAddresses.Name = "btnToolbarAddresses";
        btnToolbarAddresses.Size = new Size(145, 28);
        btnToolbarAddresses.TabIndex = 3;
        btnToolbarAddresses.Text = "العناوين والمعرفات";
        toolTips.SetToolTip(btnToolbarAddresses, "غير متاح قبل الإضافة أو ربط الخدمة.");
        btnToolbarAddresses.UseVisualStyleBackColor = true;
        btnToolbarAddresses.Click += toolbarAddresses_Click;
        // 
        // lblStatus
        // 
        lblStatus.AutoEllipsis = true;
        lblStatus.BorderStyle = BorderStyle.FixedSingle;
        lblStatus.Dock = DockStyle.Fill;
        lblStatus.Location = new Point(3, 709);
        lblStatus.Margin = new Padding(1);
        lblStatus.Name = "lblStatus";
        lblStatus.Padding = new Padding(2);
        lblStatus.Size = new Size(1014, 24);
        lblStatus.TabIndex = 4;
        lblStatus.Text = "إدخال مؤقت — الحفظ والقوائم غير مرتبطة؛ القيم لا تُطبّق على النظام.";
        lblStatus.TextAlign = ContentAlignment.MiddleRight;
        toolTips.SetToolTip(lblStatus, "بيانات الفروع — معاينة محلية دون حفظ.");
        // 
        // layout
        // 
        layout.BackColor = Color.FromArgb(244, 244, 244);
        layout.ColumnCount = 1;
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layout.Controls.Add(lblTitle, 0, 0);
        layout.Controls.Add(designerCommandBar, 0, 1);
        layout.Controls.Add(tabsFields, 0, 2);
        layout.Controls.Add(auditInfoContainer, 0, 3);
        layout.Controls.Add(lblStatus, 0, 4);
        layout.Dock = DockStyle.Fill;
        layout.Location = new Point(0, 0);
        layout.Margin = new Padding(0);
        layout.Name = "layout";
        layout.Padding = new Padding(2);
        layout.RowCount = 5;
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 66F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
        layout.Size = new Size(1020, 736);
        layout.TabIndex = 0;
        // 
        // lblTitle
        // 
        lblTitle.BackColor = Color.FromArgb(23, 50, 78);
        lblTitle.Dock = DockStyle.Fill;
        lblTitle.Font = new Font("Tahoma", 11F, FontStyle.Bold);
        lblTitle.ForeColor = Color.White;
        lblTitle.Location = new Point(3, 3);
        lblTitle.Margin = new Padding(1);
        lblTitle.Name = "lblTitle";
        lblTitle.Padding = new Padding(3);
        lblTitle.Size = new Size(1014, 28);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "بيانات الفروع";
        lblTitle.TextAlign = ContentAlignment.MiddleRight;
        // 
        // pnlToolbar
        // 
        pnlToolbar.BackColor = Color.FromArgb(232, 232, 232);
        pnlToolbar.BorderStyle = BorderStyle.FixedSingle;
        pnlToolbar.Controls.Add(commandsLayout);
        pnlToolbar.Dock = DockStyle.Fill;
        pnlToolbar.Location = new Point(3, 33);
        pnlToolbar.Margin = new Padding(1);
        pnlToolbar.MinimumSize = new Size(0, 36);
        pnlToolbar.Name = "pnlToolbar";
        pnlToolbar.Padding = new Padding(3);
        pnlToolbar.Size = new Size(1014, 38);
        pnlToolbar.TabIndex = 1;
        // 
        // commandsLayout
        // 
        commandsLayout.ColumnCount = 5;
        commandsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 147F));
        commandsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 147F));
        commandsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 147F));
        commandsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 147F));
        commandsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

        commandsLayout.Dock = DockStyle.Fill;
        commandsLayout.Location = new Point(3, 3);
        commandsLayout.Margin = new Padding(0);
        commandsLayout.Name = "commandsLayout";
        commandsLayout.RightToLeft = RightToLeft.Yes;
        commandsLayout.RowCount = 1;
        commandsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        commandsLayout.Size = new Size(1006, 30);
        commandsLayout.TabIndex = 0;
        // 
        // tabsFields
        // 
        tabsFields.Controls.Add(tabMain);
        tabsFields.Controls.Add(tabDetails);
        tabsFields.Controls.Add(tabArchive);
        tabsFields.Controls.Add(tabDocuments);
        tabsFields.Controls.Add(tabHeaders);
        tabsFields.Controls.Add(tabAddresses);
        tabsFields.Dock = DockStyle.Fill;
        tabsFields.Location = new Point(3, 73);
        tabsFields.Margin = new Padding(1);
        tabsFields.Multiline = true;
        tabsFields.Name = "tabsFields";
        tabsFields.Padding = new Point(8, 4);
        tabsFields.RightToLeft = RightToLeft.Yes;
        tabsFields.RightToLeftLayout = true;
        tabsFields.SelectedIndex = 0;
        tabsFields.Size = new Size(1014, 568);
        tabsFields.TabIndex = 2;
        // 
        // tabMain
        // 
        tabMain.AutoScroll = true;
        tabMain.BackColor = Color.FromArgb(244, 244, 244);
        tabMain.Controls.Add(pnlMainFields);
        tabMain.Controls.Add(grpLogo);
        tabMain.Location = new Point(4, 29);
        tabMain.Margin = new Padding(0);
        tabMain.Name = "tabMain";
        tabMain.Padding = new Padding(3);
        tabMain.Size = new Size(1006, 535);
        tabMain.TabIndex = 0;
        tabMain.Text = "البيانات الرئيسية";
        // 
        // pnlMainFields
        // 
        pnlMainFields.AutoScroll = true;
        pnlMainFields.Controls.Add(fieldsMain);
        pnlMainFields.Dock = DockStyle.Fill;
        pnlMainFields.Location = new Point(145, 3);
        pnlMainFields.Margin = new Padding(0);
        pnlMainFields.Name = "pnlMainFields";
        pnlMainFields.Size = new Size(858, 529);
        pnlMainFields.TabIndex = 0;
        // 
        // fieldsMain
        // 
        fieldsMain.AutoSize = true;
        fieldsMain.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        fieldsMain.BackColor = Color.White;
        fieldsMain.BorderStyle = BorderStyle.FixedSingle;
        fieldsMain.ColumnCount = 10;
        fieldsMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsMain.Controls.Add(lblcboCompany, 0, 0);
        fieldsMain.Controls.Add(cboCompany, 1, 0);
        fieldsMain.Controls.Add(lbltxtCompanyLocal, 2, 0);
        fieldsMain.Controls.Add(txtCompanyLocal, 4, 0);
        fieldsMain.Controls.Add(lbltxtCompanyForeign, 6, 0);
        fieldsMain.Controls.Add(txtCompanyForeign, 8, 0);
        fieldsMain.Controls.Add(lblchkCompanyTaxGroup, 0, 1);
        fieldsMain.Controls.Add(chkCompanyTaxGroup, 1, 1);
        fieldsMain.Controls.Add(lbltxtCompanyTaxCurrency, 2, 1);
        fieldsMain.Controls.Add(txtCompanyTaxCurrency, 3, 1);
        fieldsMain.Controls.Add(lbltxtCompanyTaxRegistration, 4, 1);
        fieldsMain.Controls.Add(txtCompanyTaxRegistration, 6, 1);
        fieldsMain.Controls.Add(lbltxtBranchNumber, 8, 1);
        fieldsMain.Controls.Add(txtBranchNumber, 9, 1);
        fieldsMain.Controls.Add(lbltxtYear, 0, 2);
        fieldsMain.Controls.Add(txtYear, 1, 2);
        fieldsMain.Controls.Add(lbltxtNameLocal, 2, 2);
        fieldsMain.Controls.Add(txtNameLocal, 4, 2);
        fieldsMain.Controls.Add(lbltxtNameForeign, 6, 2);
        fieldsMain.Controls.Add(txtNameForeign, 8, 2);
        fieldsMain.Controls.Add(lblcboGroup, 0, 3);
        fieldsMain.Controls.Add(cboGroup, 1, 3);
        fieldsMain.Controls.Add(lblchkInvoice, 2, 3);
        fieldsMain.Controls.Add(chkInvoice, 4, 3);
        fieldsMain.Controls.Add(lblchkQr, 5, 3);
        fieldsMain.Controls.Add(chkQr, 7, 3);
        fieldsMain.Controls.Add(lblchkMain, 8, 3);
        fieldsMain.Controls.Add(chkMain, 9, 3);
        fieldsMain.Controls.Add(lblchkLite, 0, 4);
        fieldsMain.Controls.Add(chkLite, 2, 4);
        fieldsMain.Controls.Add(lbltxtConnection, 3, 4);
        fieldsMain.Controls.Add(txtConnection, 4, 4);
        fieldsMain.Controls.Add(lblcboConnection, 6, 4);
        fieldsMain.Controls.Add(cboConnection, 8, 4);
        fieldsMain.Dock = DockStyle.Top;
        fieldsMain.Enabled = false;
        fieldsMain.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
        fieldsMain.Location = new Point(0, 0);
        fieldsMain.Margin = new Padding(1);
        fieldsMain.Name = "fieldsMain";
        fieldsMain.Padding = new Padding(2);
        fieldsMain.RightToLeft = RightToLeft.Yes;
        fieldsMain.RowCount = 5;
        fieldsMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        fieldsMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        fieldsMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        fieldsMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        fieldsMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        fieldsMain.Size = new Size(858, 156);
        fieldsMain.TabIndex = 0;
        // 
        // lblcboCompany
        // 
        lblcboCompany.Dock = DockStyle.Fill;
        lblcboCompany.Font = new Font("Tahoma", 9F);
        lblcboCompany.Location = new Point(770, 2);
        lblcboCompany.Margin = new Padding(1, 0, 1, 0);
        lblcboCompany.Name = "lblcboCompany";
        lblcboCompany.Size = new Size(83, 30);
        lblcboCompany.TabIndex = 0;
        lblcboCompany.Text = "رقم الشركة";
        lblcboCompany.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cboCompany
        // 
        cboCompany.AccessibleDescription = "قائمة فارغة؛ مصدر البيانات غير مرتبط.";
        cboCompany.AccessibleName = "رقم الشركة";
        cboCompany.BackColor = Color.FromArgb(255, 253, 225);
        cboCompany.DropDownStyle = ComboBoxStyle.DropDownList;
        cboCompany.Enabled = false;
        cboCompany.Font = new Font("Tahoma", 10F);
        cboCompany.Location = new Point(685, 4);
        cboCompany.Margin = new Padding(1, 2, 1, 2);
        cboCompany.Name = "cboCompany";
        cboCompany.Size = new Size(83, 29);
        cboCompany.TabIndex = 1;
        // 
        // lbltxtCompanyLocal
        // 
        fieldsMain.SetColumnSpan(lbltxtCompanyLocal, 2);
        lbltxtCompanyLocal.Dock = DockStyle.Fill;
        lbltxtCompanyLocal.Font = new Font("Tahoma", 9F);
        lbltxtCompanyLocal.Location = new Point(515, 2);
        lbltxtCompanyLocal.Margin = new Padding(1, 0, 1, 0);
        lbltxtCompanyLocal.Name = "lbltxtCompanyLocal";
        lbltxtCompanyLocal.Size = new Size(168, 30);
        lbltxtCompanyLocal.TabIndex = 2;
        lbltxtCompanyLocal.Text = "اسم الشركة (محلي)";
        lbltxtCompanyLocal.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtCompanyLocal
        // 
        txtCompanyLocal.AccessibleDescription = "قيمة للعرض فقط؛ مصدر البيانات غير مرتبط.";
        txtCompanyLocal.AccessibleName = "اسم الشركة (محلي)";
        txtCompanyLocal.BackColor = Color.FromArgb(240, 240, 240);
        txtCompanyLocal.BorderStyle = BorderStyle.FixedSingle;
        fieldsMain.SetColumnSpan(txtCompanyLocal, 2);
        txtCompanyLocal.Font = new Font("Tahoma", 10F);
        txtCompanyLocal.Location = new Point(345, 4);
        txtCompanyLocal.Margin = new Padding(1, 2, 1, 2);
        txtCompanyLocal.Name = "txtCompanyLocal";
        txtCompanyLocal.ReadOnly = true;
        txtCompanyLocal.RightToLeft = RightToLeft.Yes;
        txtCompanyLocal.Size = new Size(168, 28);
        txtCompanyLocal.TabIndex = 3;
        txtCompanyLocal.TextAlign = HorizontalAlignment.Right;
        // 
        // lbltxtCompanyForeign
        // 
        fieldsMain.SetColumnSpan(lbltxtCompanyForeign, 2);
        lbltxtCompanyForeign.Dock = DockStyle.Fill;
        lbltxtCompanyForeign.Font = new Font("Tahoma", 9F);
        lbltxtCompanyForeign.Location = new Point(175, 2);
        lbltxtCompanyForeign.Margin = new Padding(1, 0, 1, 0);
        lbltxtCompanyForeign.Name = "lbltxtCompanyForeign";
        lbltxtCompanyForeign.Size = new Size(168, 30);
        lbltxtCompanyForeign.TabIndex = 4;
        lbltxtCompanyForeign.Text = "اسم الشركة (أجنبي)";
        lbltxtCompanyForeign.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtCompanyForeign
        // 
        txtCompanyForeign.AccessibleDescription = "قيمة للعرض فقط؛ مصدر البيانات غير مرتبط.";
        txtCompanyForeign.AccessibleName = "اسم الشركة (أجنبي)";
        txtCompanyForeign.BackColor = Color.FromArgb(240, 240, 240);
        txtCompanyForeign.BorderStyle = BorderStyle.FixedSingle;
        fieldsMain.SetColumnSpan(txtCompanyForeign, 2);
        txtCompanyForeign.Font = new Font("Tahoma", 10F);
        txtCompanyForeign.Location = new Point(3, 4);
        txtCompanyForeign.Margin = new Padding(1, 2, 1, 2);
        txtCompanyForeign.Name = "txtCompanyForeign";
        txtCompanyForeign.ReadOnly = true;
        txtCompanyForeign.RightToLeft = RightToLeft.No;
        txtCompanyForeign.Size = new Size(170, 28);
        txtCompanyForeign.TabIndex = 5;
        // 
        // lblchkCompanyTaxGroup
        // 
        lblchkCompanyTaxGroup.Dock = DockStyle.Fill;
        lblchkCompanyTaxGroup.Font = new Font("Tahoma", 9F);
        lblchkCompanyTaxGroup.Location = new Point(770, 32);
        lblchkCompanyTaxGroup.Margin = new Padding(1, 0, 1, 0);
        lblchkCompanyTaxGroup.Name = "lblchkCompanyTaxGroup";
        lblchkCompanyTaxGroup.Size = new Size(83, 30);
        lblchkCompanyTaxGroup.TabIndex = 6;
        lblchkCompanyTaxGroup.Text = "مجموعة ضريبية";
        lblchkCompanyTaxGroup.TextAlign = ContentAlignment.MiddleRight;
        // 
        // chkCompanyTaxGroup
        // 
        chkCompanyTaxGroup.AccessibleName = "مجموعة ضريبية";
        chkCompanyTaxGroup.AutoCheck = false;
        chkCompanyTaxGroup.CheckAlign = ContentAlignment.MiddleRight;
        chkCompanyTaxGroup.Font = new Font("Tahoma", 10F);
        chkCompanyTaxGroup.Location = new Point(743, 34);
        chkCompanyTaxGroup.Margin = new Padding(1, 2, 1, 2);
        chkCompanyTaxGroup.Name = "chkCompanyTaxGroup";
        chkCompanyTaxGroup.Size = new Size(25, 25);
        chkCompanyTaxGroup.TabIndex = 7;
        // 
        // lbltxtCompanyTaxCurrency
        // 
        lbltxtCompanyTaxCurrency.Dock = DockStyle.Fill;
        lbltxtCompanyTaxCurrency.Font = new Font("Tahoma", 9F);
        lbltxtCompanyTaxCurrency.Location = new Point(600, 32);
        lbltxtCompanyTaxCurrency.Margin = new Padding(1, 0, 1, 0);
        lbltxtCompanyTaxCurrency.Name = "lbltxtCompanyTaxCurrency";
        lbltxtCompanyTaxCurrency.Size = new Size(83, 30);
        lbltxtCompanyTaxCurrency.TabIndex = 8;
        lbltxtCompanyTaxCurrency.Text = "عملة الضريبة";
        lbltxtCompanyTaxCurrency.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtCompanyTaxCurrency
        // 
        txtCompanyTaxCurrency.AccessibleDescription = "قيمة للعرض فقط؛ مصدر البيانات غير مرتبط.";
        txtCompanyTaxCurrency.AccessibleName = "عملة الضريبة";
        txtCompanyTaxCurrency.BackColor = Color.FromArgb(240, 240, 240);
        txtCompanyTaxCurrency.BorderStyle = BorderStyle.FixedSingle;
        txtCompanyTaxCurrency.Font = new Font("Tahoma", 10F);
        txtCompanyTaxCurrency.Location = new Point(515, 34);
        txtCompanyTaxCurrency.Margin = new Padding(1, 2, 1, 2);
        txtCompanyTaxCurrency.Name = "txtCompanyTaxCurrency";
        txtCompanyTaxCurrency.ReadOnly = true;
        txtCompanyTaxCurrency.RightToLeft = RightToLeft.Yes;
        txtCompanyTaxCurrency.Size = new Size(83, 28);
        txtCompanyTaxCurrency.TabIndex = 9;
        txtCompanyTaxCurrency.TextAlign = HorizontalAlignment.Right;
        // 
        // lbltxtCompanyTaxRegistration
        // 
        fieldsMain.SetColumnSpan(lbltxtCompanyTaxRegistration, 2);
        lbltxtCompanyTaxRegistration.Dock = DockStyle.Fill;
        lbltxtCompanyTaxRegistration.Font = new Font("Tahoma", 9F);
        lbltxtCompanyTaxRegistration.Location = new Point(345, 32);
        lbltxtCompanyTaxRegistration.Margin = new Padding(1, 0, 1, 0);
        lbltxtCompanyTaxRegistration.Name = "lbltxtCompanyTaxRegistration";
        lbltxtCompanyTaxRegistration.Size = new Size(168, 30);
        lbltxtCompanyTaxRegistration.TabIndex = 10;
        lbltxtCompanyTaxRegistration.Text = "رقم التسجيل الضريبي";
        lbltxtCompanyTaxRegistration.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtCompanyTaxRegistration
        // 
        txtCompanyTaxRegistration.AccessibleDescription = "قيمة للعرض فقط؛ مصدر البيانات غير مرتبط.";
        txtCompanyTaxRegistration.AccessibleName = "رقم التسجيل الضريبي";
        txtCompanyTaxRegistration.BackColor = Color.FromArgb(240, 240, 240);
        txtCompanyTaxRegistration.BorderStyle = BorderStyle.FixedSingle;
        fieldsMain.SetColumnSpan(txtCompanyTaxRegistration, 2);
        txtCompanyTaxRegistration.Font = new Font("Tahoma", 10F);
        txtCompanyTaxRegistration.Location = new Point(175, 34);
        txtCompanyTaxRegistration.Margin = new Padding(1, 2, 1, 2);
        txtCompanyTaxRegistration.Name = "txtCompanyTaxRegistration";
        txtCompanyTaxRegistration.ReadOnly = true;
        txtCompanyTaxRegistration.RightToLeft = RightToLeft.No;
        txtCompanyTaxRegistration.Size = new Size(168, 28);
        txtCompanyTaxRegistration.TabIndex = 11;
        // 
        // lbltxtBranchNumber
        // 
        lbltxtBranchNumber.Dock = DockStyle.Fill;
        lbltxtBranchNumber.Font = new Font("Tahoma", 9F);
        lbltxtBranchNumber.Location = new Point(90, 32);
        lbltxtBranchNumber.Margin = new Padding(1, 0, 1, 0);
        lbltxtBranchNumber.Name = "lbltxtBranchNumber";
        lbltxtBranchNumber.Size = new Size(83, 30);
        lbltxtBranchNumber.TabIndex = 12;
        lbltxtBranchNumber.Text = "رقم الفرع";
        lbltxtBranchNumber.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtBranchNumber
        // 
        txtBranchNumber.AccessibleDescription = "قيمة للعرض فقط؛ مصدر البيانات غير مرتبط.";
        txtBranchNumber.AccessibleName = "رقم الفرع";
        txtBranchNumber.BackColor = Color.FromArgb(240, 240, 240);
        txtBranchNumber.BorderStyle = BorderStyle.FixedSingle;
        txtBranchNumber.Font = new Font("Tahoma", 10F);
        txtBranchNumber.Location = new Point(3, 34);
        txtBranchNumber.Margin = new Padding(1, 2, 1, 2);
        txtBranchNumber.Name = "txtBranchNumber";
        txtBranchNumber.ReadOnly = true;
        txtBranchNumber.RightToLeft = RightToLeft.No;
        txtBranchNumber.Size = new Size(85, 28);
        txtBranchNumber.TabIndex = 13;
        // 
        // lbltxtYear
        // 
        lbltxtYear.Dock = DockStyle.Fill;
        lbltxtYear.Font = new Font("Tahoma", 9F);
        lbltxtYear.Location = new Point(770, 62);
        lbltxtYear.Margin = new Padding(1, 0, 1, 0);
        lbltxtYear.Name = "lbltxtYear";
        lbltxtYear.Size = new Size(83, 30);
        lbltxtYear.TabIndex = 14;
        lbltxtYear.Text = "السنة";
        lbltxtYear.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtYear
        // 
        txtYear.AccessibleName = "السنة";
        txtYear.BackColor = Color.FromArgb(255, 253, 225);
        txtYear.BorderStyle = BorderStyle.FixedSingle;
        txtYear.Font = new Font("Tahoma", 10F);
        txtYear.Location = new Point(685, 64);
        txtYear.Margin = new Padding(1, 2, 1, 2);
        txtYear.Name = "txtYear";
        txtYear.RightToLeft = RightToLeft.No;
        txtYear.Size = new Size(83, 28);
        txtYear.TabIndex = 15;
        // 
        // lbltxtNameLocal
        // 
        fieldsMain.SetColumnSpan(lbltxtNameLocal, 2);
        lbltxtNameLocal.Dock = DockStyle.Fill;
        lbltxtNameLocal.Font = new Font("Tahoma", 9F);
        lbltxtNameLocal.Location = new Point(515, 62);
        lbltxtNameLocal.Margin = new Padding(1, 0, 1, 0);
        lbltxtNameLocal.Name = "lbltxtNameLocal";
        lbltxtNameLocal.Size = new Size(168, 30);
        lbltxtNameLocal.TabIndex = 16;
        lbltxtNameLocal.Text = "اسم الفرع (محلي)";
        lbltxtNameLocal.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtNameLocal
        // 
        txtNameLocal.AccessibleName = "اسم الفرع (محلي)";
        txtNameLocal.BackColor = Color.FromArgb(255, 253, 225);
        txtNameLocal.BorderStyle = BorderStyle.FixedSingle;
        fieldsMain.SetColumnSpan(txtNameLocal, 2);
        txtNameLocal.Font = new Font("Tahoma", 10F);
        txtNameLocal.Location = new Point(345, 64);
        txtNameLocal.Margin = new Padding(1, 2, 1, 2);
        txtNameLocal.Name = "txtNameLocal";
        txtNameLocal.RightToLeft = RightToLeft.Yes;
        txtNameLocal.Size = new Size(168, 28);
        txtNameLocal.TabIndex = 17;
        txtNameLocal.TextAlign = HorizontalAlignment.Right;
        // 
        // lbltxtNameForeign
        // 
        fieldsMain.SetColumnSpan(lbltxtNameForeign, 2);
        lbltxtNameForeign.Dock = DockStyle.Fill;
        lbltxtNameForeign.Font = new Font("Tahoma", 9F);
        lbltxtNameForeign.Location = new Point(175, 62);
        lbltxtNameForeign.Margin = new Padding(1, 0, 1, 0);
        lbltxtNameForeign.Name = "lbltxtNameForeign";
        lbltxtNameForeign.Size = new Size(168, 30);
        lbltxtNameForeign.TabIndex = 18;
        lbltxtNameForeign.Text = "اسم الفرع (أجنبي)";
        lbltxtNameForeign.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtNameForeign
        // 
        txtNameForeign.AccessibleName = "اسم الفرع (أجنبي)";
        txtNameForeign.BackColor = Color.White;
        txtNameForeign.BorderStyle = BorderStyle.FixedSingle;
        fieldsMain.SetColumnSpan(txtNameForeign, 2);
        txtNameForeign.Font = new Font("Tahoma", 10F);
        txtNameForeign.Location = new Point(3, 64);
        txtNameForeign.Margin = new Padding(1, 2, 1, 2);
        txtNameForeign.Name = "txtNameForeign";
        txtNameForeign.RightToLeft = RightToLeft.No;
        txtNameForeign.Size = new Size(170, 28);
        txtNameForeign.TabIndex = 19;
        // 
        // lblcboGroup
        // 
        lblcboGroup.Dock = DockStyle.Fill;
        lblcboGroup.Font = new Font("Tahoma", 9F);
        lblcboGroup.Location = new Point(770, 92);
        lblcboGroup.Margin = new Padding(1, 0, 1, 0);
        lblcboGroup.Name = "lblcboGroup";
        lblcboGroup.Size = new Size(83, 30);
        lblcboGroup.TabIndex = 20;
        lblcboGroup.Text = "رقم المجموعة";
        lblcboGroup.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cboGroup
        // 
        cboGroup.AccessibleDescription = "قائمة فارغة؛ مصدر البيانات غير مرتبط.";
        cboGroup.AccessibleName = "رقم المجموعة";
        cboGroup.BackColor = Color.White;
        cboGroup.DropDownStyle = ComboBoxStyle.DropDownList;
        cboGroup.Enabled = false;
        cboGroup.Font = new Font("Tahoma", 10F);
        cboGroup.Location = new Point(685, 94);
        cboGroup.Margin = new Padding(1, 2, 1, 2);
        cboGroup.Name = "cboGroup";
        cboGroup.Size = new Size(83, 29);
        cboGroup.TabIndex = 21;
        // 
        // lblchkInvoice
        // 
        fieldsMain.SetColumnSpan(lblchkInvoice, 2);
        lblchkInvoice.Dock = DockStyle.Fill;
        lblchkInvoice.Font = new Font("Tahoma", 9F);
        lblchkInvoice.Location = new Point(515, 92);
        lblchkInvoice.Margin = new Padding(1, 0, 1, 0);
        lblchkInvoice.Name = "lblchkInvoice";
        lblchkInvoice.Size = new Size(168, 30);
        lblchkInvoice.TabIndex = 22;
        lblchkInvoice.Text = "استخدام الفاتورة الإلكترونية";
        lblchkInvoice.TextAlign = ContentAlignment.MiddleRight;
        // 
        // chkInvoice
        // 
        chkInvoice.AccessibleName = "استخدام الفاتورة الإلكترونية";
        chkInvoice.CheckAlign = ContentAlignment.MiddleRight;
        chkInvoice.Font = new Font("Tahoma", 10F);
        chkInvoice.Location = new Point(488, 94);
        chkInvoice.Margin = new Padding(1, 2, 1, 2);
        chkInvoice.Name = "chkInvoice";
        chkInvoice.Size = new Size(25, 25);
        chkInvoice.TabIndex = 23;
        // 
        // lblchkQr
        // 
        fieldsMain.SetColumnSpan(lblchkQr, 2);
        lblchkQr.Dock = DockStyle.Fill;
        lblchkQr.Font = new Font("Tahoma", 9F);
        lblchkQr.Location = new Point(260, 92);
        lblchkQr.Margin = new Padding(1, 0, 1, 0);
        lblchkQr.Name = "lblchkQr";
        lblchkQr.Size = new Size(168, 30);
        lblchkQr.TabIndex = 24;
        lblchkQr.Text = "عرض QR CODE في التقارير";
        lblchkQr.TextAlign = ContentAlignment.MiddleRight;
        // 
        // chkQr
        // 
        chkQr.AccessibleName = "عرض QR CODE في التقارير";
        chkQr.CheckAlign = ContentAlignment.MiddleRight;
        chkQr.Font = new Font("Tahoma", 10F);
        chkQr.Location = new Point(233, 94);
        chkQr.Margin = new Padding(1, 2, 1, 2);
        chkQr.Name = "chkQr";
        chkQr.Size = new Size(25, 25);
        chkQr.TabIndex = 25;
        // 
        // lblchkMain
        // 
        lblchkMain.Dock = DockStyle.Fill;
        lblchkMain.Font = new Font("Tahoma", 9F);
        lblchkMain.Location = new Point(90, 92);
        lblchkMain.Margin = new Padding(1, 0, 1, 0);
        lblchkMain.Name = "lblchkMain";
        lblchkMain.Size = new Size(83, 30);
        lblchkMain.TabIndex = 26;
        lblchkMain.Text = "رئيسي";
        lblchkMain.TextAlign = ContentAlignment.MiddleRight;
        // 
        // chkMain
        // 
        chkMain.AccessibleName = "رئيسي";
        chkMain.CheckAlign = ContentAlignment.MiddleRight;
        chkMain.Font = new Font("Tahoma", 10F);
        chkMain.Location = new Point(63, 94);
        chkMain.Margin = new Padding(1, 2, 1, 2);
        chkMain.Name = "chkMain";
        chkMain.Size = new Size(25, 25);
        chkMain.TabIndex = 27;
        // 
        // lblchkLite
        // 
        fieldsMain.SetColumnSpan(lblchkLite, 2);
        lblchkLite.Dock = DockStyle.Fill;
        lblchkLite.Font = new Font("Tahoma", 9F);
        lblchkLite.Location = new Point(685, 122);
        lblchkLite.Margin = new Padding(1, 0, 1, 0);
        lblchkLite.Name = "lblchkLite";
        lblchkLite.Size = new Size(168, 30);
        lblchkLite.TabIndex = 28;
        lblchkLite.Text = "الربط مع نظام الأونكس لايت";
        lblchkLite.TextAlign = ContentAlignment.MiddleRight;
        // 
        // chkLite
        // 
        chkLite.AccessibleName = "الربط مع نظام الأونكس لايت";
        chkLite.CheckAlign = ContentAlignment.MiddleRight;
        chkLite.Font = new Font("Tahoma", 10F);
        chkLite.Location = new Point(658, 124);
        chkLite.Margin = new Padding(1, 2, 1, 2);
        chkLite.Name = "chkLite";
        chkLite.Size = new Size(25, 25);
        chkLite.TabIndex = 29;
        chkLite.CheckedChanged += chkLite_CheckedChanged;
        // 
        // lbltxtConnection
        // 
        lbltxtConnection.Dock = DockStyle.Fill;
        lbltxtConnection.Font = new Font("Tahoma", 9F);
        lbltxtConnection.Location = new Point(515, 122);
        lbltxtConnection.Margin = new Padding(1, 0, 1, 0);
        lbltxtConnection.Name = "lbltxtConnection";
        lbltxtConnection.Size = new Size(83, 30);
        lbltxtConnection.TabIndex = 30;
        lbltxtConnection.Text = "اسم الاتصال";
        lbltxtConnection.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtConnection
        // 
        txtConnection.AccessibleName = "اسم الاتصال";
        txtConnection.BackColor = Color.White;
        txtConnection.BorderStyle = BorderStyle.FixedSingle;
        fieldsMain.SetColumnSpan(txtConnection, 2);
        txtConnection.Font = new Font("Tahoma", 10F);
        txtConnection.Location = new Point(345, 124);
        txtConnection.Margin = new Padding(1, 2, 1, 2);
        txtConnection.Name = "txtConnection";
        txtConnection.RightToLeft = RightToLeft.Yes;
        txtConnection.Size = new Size(168, 28);
        txtConnection.TabIndex = 31;
        txtConnection.TextAlign = HorizontalAlignment.Right;
        // 
        // lblcboConnection
        // 
        fieldsMain.SetColumnSpan(lblcboConnection, 2);
        lblcboConnection.Dock = DockStyle.Fill;
        lblcboConnection.Font = new Font("Tahoma", 9F);
        lblcboConnection.Location = new Point(175, 122);
        lblcboConnection.Margin = new Padding(1, 0, 1, 0);
        lblcboConnection.Name = "lblcboConnection";
        lblcboConnection.Size = new Size(168, 30);
        lblcboConnection.TabIndex = 32;
        lblcboConnection.Text = "نوع الاتصال";
        lblcboConnection.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cboConnection
        // 
        cboConnection.AccessibleName = "نوع الاتصال";
        cboConnection.BackColor = Color.White;
        fieldsMain.SetColumnSpan(cboConnection, 2);
        cboConnection.DropDownStyle = ComboBoxStyle.DropDownList;
        cboConnection.Font = new Font("Tahoma", 10F);
        cboConnection.Items.AddRange(new object[] { "شبكة", "تليفون" });
        cboConnection.Location = new Point(3, 124);
        cboConnection.Margin = new Padding(1, 2, 1, 2);
        cboConnection.Name = "cboConnection";
        cboConnection.Size = new Size(170, 29);
        cboConnection.TabIndex = 33;
        // 
        // grpLogo
        // 
        grpLogo.BackColor = Color.White;
        grpLogo.Controls.Add(picLogo);
        grpLogo.Controls.Add(btnAddLogo);
        grpLogo.Dock = DockStyle.Left;
        grpLogo.Location = new Point(3, 3);
        grpLogo.Margin = new Padding(1);
        grpLogo.Name = "grpLogo";
        grpLogo.Padding = new Padding(3, 20, 3, 3);
        grpLogo.Size = new Size(142, 529);
        grpLogo.TabIndex = 1;
        grpLogo.TabStop = false;
        grpLogo.Text = "شعار الفرع";
        // 
        // picLogo
        // 
        picLogo.AccessibleName = "شعار الفرع";
        picLogo.BorderStyle = BorderStyle.FixedSingle;
        picLogo.Dock = DockStyle.Top;
        picLogo.Location = new Point(3, 39);
        picLogo.Name = "picLogo";
        picLogo.Size = new Size(136, 112);
        picLogo.SizeMode = PictureBoxSizeMode.Zoom;
        picLogo.TabIndex = 0;
        picLogo.TabStop = false;
        // 
        // btnAddLogo
        // 
        btnAddLogo.AccessibleDescription = "خدمة استيراد الشعار غير مرتبطة.";
        btnAddLogo.Dock = DockStyle.Bottom;
        btnAddLogo.Enabled = false;
        btnAddLogo.Location = new Point(3, 498);
        btnAddLogo.Name = "btnAddLogo";
        btnAddLogo.Size = new Size(136, 28);
        btnAddLogo.TabIndex = 0;
        btnAddLogo.Text = "إضافة";
        btnAddLogo.UseVisualStyleBackColor = true;
        // 
        // tabDetails
        // 
        tabDetails.AutoScroll = true;
        tabDetails.BackColor = Color.FromArgb(244, 244, 244);
        tabDetails.Controls.Add(fieldsDetails);
        tabDetails.Location = new Point(4, 29);
        tabDetails.Margin = new Padding(0);
        tabDetails.Name = "tabDetails";
        tabDetails.Padding = new Padding(3);
        tabDetails.Size = new Size(1006, 535);
        tabDetails.TabIndex = 1;
        tabDetails.Text = "بيانات الفرع";
        // 
        // fieldsDetails
        // 
        fieldsDetails.AutoSize = true;
        fieldsDetails.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        fieldsDetails.BackColor = Color.White;
        fieldsDetails.BorderStyle = BorderStyle.FixedSingle;
        fieldsDetails.ColumnCount = 10;
        fieldsDetails.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsDetails.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsDetails.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsDetails.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsDetails.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsDetails.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsDetails.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsDetails.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsDetails.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsDetails.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsDetails.Controls.Add(lbltxtReportHeader, 0, 0);
        fieldsDetails.Controls.Add(txtReportHeader, 2, 0);
        fieldsDetails.Controls.Add(lbltxtAddress, 0, 1);
        fieldsDetails.Controls.Add(txtAddress, 2, 1);
        fieldsDetails.Controls.Add(lbltxtSpecifications, 5, 1);
        fieldsDetails.Controls.Add(txtSpecifications, 7, 1);
        fieldsDetails.Controls.Add(lbltxtPan, 0, 2);
        fieldsDetails.Controls.Add(txtPan, 2, 2);
        fieldsDetails.Controls.Add(lbltxtTan, 3, 2);
        fieldsDetails.Controls.Add(txtTan, 5, 2);
        fieldsDetails.Controls.Add(lbltxtTaxNumber, 6, 2);
        fieldsDetails.Controls.Add(txtTaxNumber, 8, 2);
        fieldsDetails.Controls.Add(lbltxtSequence, 0, 3);
        fieldsDetails.Controls.Add(txtSequence, 1, 3);
        fieldsDetails.Controls.Add(lbltxtTaxArticle, 2, 3);
        fieldsDetails.Controls.Add(txtTaxArticle, 3, 3);
        fieldsDetails.Controls.Add(lbltxtStatistics, 4, 3);
        fieldsDetails.Controls.Add(txtStatistics, 5, 3);
        fieldsDetails.Controls.Add(lblcboCountry, 6, 3);
        fieldsDetails.Controls.Add(cboCountry, 7, 3);
        fieldsDetails.Controls.Add(lblcboGovernorate, 8, 3);
        fieldsDetails.Controls.Add(cboGovernorate, 9, 3);
        fieldsDetails.Controls.Add(lblcboCity, 3, 4);
        fieldsDetails.Controls.Add(cboCity, 4, 4);
        fieldsDetails.Controls.Add(lblchkStopped, 5, 4);
        fieldsDetails.Controls.Add(chkStopped, 6, 4);
        fieldsDetails.Dock = DockStyle.Top;
        fieldsDetails.Enabled = false;
        fieldsDetails.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
        fieldsDetails.Location = new Point(3, 3);
        fieldsDetails.Margin = new Padding(1);
        fieldsDetails.MinimumSize = new Size(700, 0);
        fieldsDetails.Name = "fieldsDetails";
        fieldsDetails.Padding = new Padding(2);
        fieldsDetails.RightToLeft = RightToLeft.Yes;
        fieldsDetails.RowCount = 5;
        fieldsDetails.RowStyles.Add(new RowStyle(SizeType.Absolute, 84F));
        fieldsDetails.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        fieldsDetails.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        fieldsDetails.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        fieldsDetails.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        fieldsDetails.Size = new Size(1000, 210);
        fieldsDetails.TabIndex = 0;
        // 
        // lbltxtReportHeader
        // 
        fieldsDetails.SetColumnSpan(lbltxtReportHeader, 2);
        lbltxtReportHeader.Dock = DockStyle.Fill;
        lbltxtReportHeader.Font = new Font("Tahoma", 9F);
        lbltxtReportHeader.Location = new Point(799, 2);
        lbltxtReportHeader.Margin = new Padding(1, 0, 1, 0);
        lbltxtReportHeader.Name = "lbltxtReportHeader";
        lbltxtReportHeader.Size = new Size(196, 84);
        lbltxtReportHeader.TabIndex = 34;
        lbltxtReportHeader.Text = "الترويسة";
        lbltxtReportHeader.TextAlign = ContentAlignment.TopRight;
        // 
        // txtReportHeader
        // 
        txtReportHeader.AccessibleName = "الترويسة";
        txtReportHeader.BackColor = Color.White;
        txtReportHeader.BorderStyle = BorderStyle.FixedSingle;
        fieldsDetails.SetColumnSpan(txtReportHeader, 8);
        txtReportHeader.Font = new Font("Tahoma", 10F);
        txtReportHeader.Location = new Point(477, 4);
        txtReportHeader.Margin = new Padding(1, 2, 1, 2);
        txtReportHeader.Multiline = true;
        txtReportHeader.Name = "txtReportHeader";
        txtReportHeader.RightToLeft = RightToLeft.Yes;
        txtReportHeader.ScrollBars = ScrollBars.Vertical;
        txtReportHeader.Size = new Size(320, 78);
        txtReportHeader.TabIndex = 35;
        txtReportHeader.TextAlign = HorizontalAlignment.Right;
        // 
        // lbltxtAddress
        // 
        fieldsDetails.SetColumnSpan(lbltxtAddress, 2);
        lbltxtAddress.Dock = DockStyle.Fill;
        lbltxtAddress.Font = new Font("Tahoma", 9F);
        lbltxtAddress.Location = new Point(799, 86);
        lbltxtAddress.Margin = new Padding(1, 0, 1, 0);
        lbltxtAddress.Name = "lbltxtAddress";
        lbltxtAddress.Size = new Size(196, 30);
        lbltxtAddress.TabIndex = 36;
        lbltxtAddress.Text = "العنوان";
        lbltxtAddress.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtAddress
        // 
        txtAddress.AccessibleName = "العنوان";
        txtAddress.BackColor = Color.White;
        txtAddress.BorderStyle = BorderStyle.FixedSingle;
        fieldsDetails.SetColumnSpan(txtAddress, 3);
        txtAddress.Font = new Font("Tahoma", 10F);
        txtAddress.Location = new Point(607, 88);
        txtAddress.Margin = new Padding(1, 2, 1, 2);
        txtAddress.Name = "txtAddress";
        txtAddress.RightToLeft = RightToLeft.Yes;
        txtAddress.Size = new Size(190, 28);
        txtAddress.TabIndex = 37;
        txtAddress.TextAlign = HorizontalAlignment.Right;
        // 
        // lbltxtSpecifications
        // 
        fieldsDetails.SetColumnSpan(lbltxtSpecifications, 2);
        lbltxtSpecifications.Dock = DockStyle.Fill;
        lbltxtSpecifications.Font = new Font("Tahoma", 9F);
        lbltxtSpecifications.Location = new Point(304, 86);
        lbltxtSpecifications.Margin = new Padding(1, 0, 1, 0);
        lbltxtSpecifications.Name = "lbltxtSpecifications";
        lbltxtSpecifications.Size = new Size(196, 30);
        lbltxtSpecifications.TabIndex = 38;
        lbltxtSpecifications.Text = "المواصفات";
        lbltxtSpecifications.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtSpecifications
        // 
        txtSpecifications.AccessibleName = "المواصفات";
        txtSpecifications.BackColor = Color.White;
        txtSpecifications.BorderStyle = BorderStyle.FixedSingle;
        fieldsDetails.SetColumnSpan(txtSpecifications, 3);
        txtSpecifications.Font = new Font("Tahoma", 10F);
        txtSpecifications.Location = new Point(112, 88);
        txtSpecifications.Margin = new Padding(1, 2, 1, 2);
        txtSpecifications.Name = "txtSpecifications";
        txtSpecifications.RightToLeft = RightToLeft.Yes;
        txtSpecifications.Size = new Size(190, 28);
        txtSpecifications.TabIndex = 39;
        txtSpecifications.TextAlign = HorizontalAlignment.Right;
        // 
        // lbltxtPan
        // 
        fieldsDetails.SetColumnSpan(lbltxtPan, 2);
        lbltxtPan.Dock = DockStyle.Fill;
        lbltxtPan.Font = new Font("Tahoma", 9F);
        lbltxtPan.Location = new Point(799, 116);
        lbltxtPan.Margin = new Padding(1, 0, 1, 0);
        lbltxtPan.Name = "lbltxtPan";
        lbltxtPan.Size = new Size(196, 30);
        lbltxtPan.TabIndex = 40;
        lbltxtPan.Text = "رقم الحساب الدائم / الثابت";
        lbltxtPan.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtPan
        // 
        txtPan.AccessibleName = "رقم الحساب الدائم / الثابت";
        txtPan.BackColor = Color.White;
        txtPan.BorderStyle = BorderStyle.FixedSingle;
        txtPan.Font = new Font("Tahoma", 10F);
        txtPan.Location = new Point(702, 118);
        txtPan.Margin = new Padding(1, 2, 1, 2);
        txtPan.Name = "txtPan";
        txtPan.RightToLeft = RightToLeft.No;
        txtPan.Size = new Size(95, 28);
        txtPan.TabIndex = 41;
        // 
        // lbltxtTan
        // 
        fieldsDetails.SetColumnSpan(lbltxtTan, 2);
        lbltxtTan.Dock = DockStyle.Fill;
        lbltxtTan.Font = new Font("Tahoma", 9F);
        lbltxtTan.Location = new Point(502, 116);
        lbltxtTan.Margin = new Padding(1, 0, 1, 0);
        lbltxtTan.Name = "lbltxtTan";
        lbltxtTan.Size = new Size(196, 30);
        lbltxtTan.TabIndex = 42;
        lbltxtTan.Text = "الحساب الضريبي (تان)";
        lbltxtTan.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtTan
        // 
        txtTan.AccessibleName = "الحساب الضريبي (تان)";
        txtTan.BackColor = Color.White;
        txtTan.BorderStyle = BorderStyle.FixedSingle;
        txtTan.Font = new Font("Tahoma", 10F);
        txtTan.Location = new Point(405, 118);
        txtTan.Margin = new Padding(1, 2, 1, 2);
        txtTan.Name = "txtTan";
        txtTan.RightToLeft = RightToLeft.No;
        txtTan.Size = new Size(95, 28);
        txtTan.TabIndex = 43;
        // 
        // lbltxtTaxNumber
        // 
        fieldsDetails.SetColumnSpan(lbltxtTaxNumber, 2);
        lbltxtTaxNumber.Dock = DockStyle.Fill;
        lbltxtTaxNumber.Font = new Font("Tahoma", 9F);
        lbltxtTaxNumber.Location = new Point(205, 116);
        lbltxtTaxNumber.Margin = new Padding(1, 0, 1, 0);
        lbltxtTaxNumber.Name = "lbltxtTaxNumber";
        lbltxtTaxNumber.Size = new Size(196, 30);
        lbltxtTaxNumber.TabIndex = 44;
        lbltxtTaxNumber.Text = "الرقم الضريبي";
        lbltxtTaxNumber.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtTaxNumber
        // 
        txtTaxNumber.AccessibleName = "الرقم الضريبي";
        txtTaxNumber.BackColor = Color.White;
        txtTaxNumber.BorderStyle = BorderStyle.FixedSingle;
        txtTaxNumber.Font = new Font("Tahoma", 10F);
        txtTaxNumber.Location = new Point(108, 118);
        txtTaxNumber.Margin = new Padding(1, 2, 1, 2);
        txtTaxNumber.Name = "txtTaxNumber";
        txtTaxNumber.RightToLeft = RightToLeft.No;
        txtTaxNumber.Size = new Size(95, 28);
        txtTaxNumber.TabIndex = 45;
        // 
        // lbltxtSequence
        // 
        lbltxtSequence.Dock = DockStyle.Fill;
        lbltxtSequence.Font = new Font("Tahoma", 9F);
        lbltxtSequence.Location = new Point(898, 146);
        lbltxtSequence.Margin = new Padding(1, 0, 1, 0);
        lbltxtSequence.Name = "lbltxtSequence";
        lbltxtSequence.Size = new Size(97, 30);
        lbltxtSequence.TabIndex = 46;
        lbltxtSequence.Text = "التسلسل";
        lbltxtSequence.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtSequence
        // 
        txtSequence.AccessibleName = "التسلسل";
        txtSequence.BackColor = Color.White;
        txtSequence.BorderStyle = BorderStyle.FixedSingle;
        txtSequence.Font = new Font("Tahoma", 10F);
        txtSequence.Location = new Point(801, 148);
        txtSequence.Margin = new Padding(1, 2, 1, 2);
        txtSequence.Name = "txtSequence";
        txtSequence.RightToLeft = RightToLeft.No;
        txtSequence.Size = new Size(95, 28);
        txtSequence.TabIndex = 47;
        // 
        // lbltxtTaxArticle
        // 
        lbltxtTaxArticle.Dock = DockStyle.Fill;
        lbltxtTaxArticle.Font = new Font("Tahoma", 9F);
        lbltxtTaxArticle.Location = new Point(700, 146);
        lbltxtTaxArticle.Margin = new Padding(1, 0, 1, 0);
        lbltxtTaxArticle.Name = "lbltxtTaxArticle";
        lbltxtTaxArticle.Size = new Size(97, 30);
        lbltxtTaxArticle.TabIndex = 48;
        lbltxtTaxArticle.Text = "المادة الضريبية";
        lbltxtTaxArticle.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtTaxArticle
        // 
        txtTaxArticle.AccessibleName = "المادة الضريبية";
        txtTaxArticle.BackColor = Color.White;
        txtTaxArticle.BorderStyle = BorderStyle.FixedSingle;
        txtTaxArticle.Font = new Font("Tahoma", 10F);
        txtTaxArticle.Location = new Point(603, 148);
        txtTaxArticle.Margin = new Padding(1, 2, 1, 2);
        txtTaxArticle.Name = "txtTaxArticle";
        txtTaxArticle.RightToLeft = RightToLeft.No;
        txtTaxArticle.Size = new Size(95, 28);
        txtTaxArticle.TabIndex = 49;
        // 
        // lbltxtStatistics
        // 
        lbltxtStatistics.Dock = DockStyle.Fill;
        lbltxtStatistics.Font = new Font("Tahoma", 9F);
        lbltxtStatistics.Location = new Point(502, 146);
        lbltxtStatistics.Margin = new Padding(1, 0, 1, 0);
        lbltxtStatistics.Name = "lbltxtStatistics";
        lbltxtStatistics.Size = new Size(97, 30);
        lbltxtStatistics.TabIndex = 50;
        lbltxtStatistics.Text = "الرقم الإحصائي";
        lbltxtStatistics.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtStatistics
        // 
        txtStatistics.AccessibleName = "الرقم الإحصائي";
        txtStatistics.BackColor = Color.White;
        txtStatistics.BorderStyle = BorderStyle.FixedSingle;
        txtStatistics.Font = new Font("Tahoma", 10F);
        txtStatistics.Location = new Point(405, 148);
        txtStatistics.Margin = new Padding(1, 2, 1, 2);
        txtStatistics.Name = "txtStatistics";
        txtStatistics.RightToLeft = RightToLeft.No;
        txtStatistics.Size = new Size(95, 28);
        txtStatistics.TabIndex = 51;
        // 
        // lblcboCountry
        // 
        lblcboCountry.Dock = DockStyle.Fill;
        lblcboCountry.Font = new Font("Tahoma", 9F);
        lblcboCountry.Location = new Point(304, 146);
        lblcboCountry.Margin = new Padding(1, 0, 1, 0);
        lblcboCountry.Name = "lblcboCountry";
        lblcboCountry.Size = new Size(97, 30);
        lblcboCountry.TabIndex = 52;
        lblcboCountry.Text = "رقم الدولة";
        lblcboCountry.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cboCountry
        // 
        cboCountry.AccessibleDescription = "قائمة فارغة؛ مصدر البيانات غير مرتبط.";
        cboCountry.AccessibleName = "رقم الدولة";
        cboCountry.BackColor = Color.White;
        cboCountry.DropDownStyle = ComboBoxStyle.DropDownList;
        cboCountry.Enabled = false;
        cboCountry.Font = new Font("Tahoma", 10F);
        cboCountry.Location = new Point(207, 148);
        cboCountry.Margin = new Padding(1, 2, 1, 2);
        cboCountry.Name = "cboCountry";
        cboCountry.Size = new Size(95, 29);
        cboCountry.TabIndex = 53;
        // 
        // lblcboGovernorate
        // 
        lblcboGovernorate.Dock = DockStyle.Fill;
        lblcboGovernorate.Font = new Font("Tahoma", 9F);
        lblcboGovernorate.Location = new Point(106, 146);
        lblcboGovernorate.Margin = new Padding(1, 0, 1, 0);
        lblcboGovernorate.Name = "lblcboGovernorate";
        lblcboGovernorate.Size = new Size(97, 30);
        lblcboGovernorate.TabIndex = 54;
        lblcboGovernorate.Text = "المحافظة";
        lblcboGovernorate.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cboGovernorate
        // 
        cboGovernorate.AccessibleDescription = "قائمة فارغة؛ مصدر البيانات غير مرتبط.";
        cboGovernorate.AccessibleName = "المحافظة";
        cboGovernorate.BackColor = Color.White;
        cboGovernorate.DropDownStyle = ComboBoxStyle.DropDownList;
        cboGovernorate.Enabled = false;
        cboGovernorate.Font = new Font("Tahoma", 10F);
        cboGovernorate.Location = new Point(9, 148);
        cboGovernorate.Margin = new Padding(1, 2, 1, 2);
        cboGovernorate.Name = "cboGovernorate";
        cboGovernorate.Size = new Size(95, 29);
        cboGovernorate.TabIndex = 55;
        // 
        // lblcboCity
        // 
        lblcboCity.Dock = DockStyle.Fill;
        lblcboCity.Font = new Font("Tahoma", 9F);
        lblcboCity.Location = new Point(601, 176);
        lblcboCity.Margin = new Padding(1, 0, 1, 0);
        lblcboCity.Name = "lblcboCity";
        lblcboCity.Size = new Size(97, 30);
        lblcboCity.TabIndex = 56;
        lblcboCity.Text = "المدينة";
        lblcboCity.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cboCity
        // 
        cboCity.AccessibleDescription = "قائمة فارغة؛ مصدر البيانات غير مرتبط.";
        cboCity.AccessibleName = "المدينة";
        cboCity.BackColor = Color.White;
        cboCity.DropDownStyle = ComboBoxStyle.DropDownList;
        cboCity.Enabled = false;
        cboCity.Font = new Font("Tahoma", 10F);
        cboCity.Location = new Point(504, 178);
        cboCity.Margin = new Padding(1, 2, 1, 2);
        cboCity.Name = "cboCity";
        cboCity.Size = new Size(95, 29);
        cboCity.TabIndex = 57;
        // 
        // lblchkStopped
        // 
        lblchkStopped.Dock = DockStyle.Fill;
        lblchkStopped.Font = new Font("Tahoma", 9F);
        lblchkStopped.Location = new Point(403, 176);
        lblchkStopped.Margin = new Padding(1, 0, 1, 0);
        lblchkStopped.Name = "lblchkStopped";
        lblchkStopped.Size = new Size(97, 30);
        lblchkStopped.TabIndex = 58;
        lblchkStopped.Text = "التوقيف";
        lblchkStopped.TextAlign = ContentAlignment.MiddleRight;
        // 
        // chkStopped
        // 
        chkStopped.AccessibleName = "التوقيف";
        chkStopped.CheckAlign = ContentAlignment.MiddleRight;
        chkStopped.Font = new Font("Tahoma", 10F);
        chkStopped.Location = new Point(376, 178);
        chkStopped.Margin = new Padding(1, 2, 1, 2);
        chkStopped.Name = "chkStopped";
        chkStopped.Size = new Size(25, 25);
        chkStopped.TabIndex = 59;
        // 
        // tabArchive
        // 
        tabArchive.AutoScroll = true;
        tabArchive.BackColor = Color.FromArgb(244, 244, 244);
        tabArchive.Controls.Add(fieldsArchive);
        tabArchive.Location = new Point(4, 29);
        tabArchive.Margin = new Padding(0);
        tabArchive.Name = "tabArchive";
        tabArchive.Padding = new Padding(3);
        tabArchive.Size = new Size(1006, 535);
        tabArchive.TabIndex = 2;
        tabArchive.Text = "أرشفة الوثائق";
        // 
        // fieldsArchive
        // 
        fieldsArchive.AutoSize = true;
        fieldsArchive.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        fieldsArchive.BackColor = Color.White;
        fieldsArchive.BorderStyle = BorderStyle.FixedSingle;
        fieldsArchive.ColumnCount = 10;
        fieldsArchive.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsArchive.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsArchive.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsArchive.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsArchive.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsArchive.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsArchive.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsArchive.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsArchive.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsArchive.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsArchive.Controls.Add(lblcboDocumentType, 1, 0);
        fieldsArchive.Controls.Add(cboDocumentType, 3, 0);
        fieldsArchive.Controls.Add(lbltxtDocumentNumber, 5, 0);
        fieldsArchive.Controls.Add(txtDocumentNumber, 7, 0);
        fieldsArchive.Controls.Add(lbltxtIssuedAt, 1, 1);
        fieldsArchive.Controls.Add(txtIssuedAt, 3, 1);
        fieldsArchive.Controls.Add(lbltxtIssuedPlace, 5, 1);
        fieldsArchive.Controls.Add(txtIssuedPlace, 7, 1);
        fieldsArchive.Controls.Add(lbltxtExpiresAt, 1, 2);
        fieldsArchive.Controls.Add(txtExpiresAt, 3, 2);
        fieldsArchive.Controls.Add(lbltxtRenewedAt, 5, 2);
        fieldsArchive.Controls.Add(txtRenewedAt, 7, 2);
        fieldsArchive.Dock = DockStyle.Top;
        fieldsArchive.Enabled = false;
        fieldsArchive.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
        fieldsArchive.Location = new Point(3, 3);
        fieldsArchive.Margin = new Padding(1);
        fieldsArchive.MinimumSize = new Size(700, 0);
        fieldsArchive.Name = "fieldsArchive";
        fieldsArchive.Padding = new Padding(2);
        fieldsArchive.RightToLeft = RightToLeft.Yes;
        fieldsArchive.RowCount = 3;
        fieldsArchive.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        fieldsArchive.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        fieldsArchive.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        fieldsArchive.Size = new Size(1000, 96);
        fieldsArchive.TabIndex = 0;
        // 
        // lblcboDocumentType
        // 
        fieldsArchive.SetColumnSpan(lblcboDocumentType, 2);
        lblcboDocumentType.Dock = DockStyle.Fill;
        lblcboDocumentType.Font = new Font("Tahoma", 9F);
        lblcboDocumentType.Location = new Point(700, 2);
        lblcboDocumentType.Margin = new Padding(1, 0, 1, 0);
        lblcboDocumentType.Name = "lblcboDocumentType";
        lblcboDocumentType.Size = new Size(196, 30);
        lblcboDocumentType.TabIndex = 60;
        lblcboDocumentType.Text = "نوع الوثيقة";
        lblcboDocumentType.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cboDocumentType
        // 
        cboDocumentType.AccessibleDescription = "قائمة فارغة؛ مصدر البيانات غير مرتبط.";
        cboDocumentType.AccessibleName = "نوع الوثيقة";
        cboDocumentType.BackColor = Color.White;
        fieldsArchive.SetColumnSpan(cboDocumentType, 2);
        cboDocumentType.DropDownStyle = ComboBoxStyle.DropDownList;
        cboDocumentType.Enabled = false;
        cboDocumentType.Font = new Font("Tahoma", 10F);
        cboDocumentType.Location = new Point(528, 4);
        cboDocumentType.Margin = new Padding(1, 2, 1, 2);
        cboDocumentType.Name = "cboDocumentType";
        cboDocumentType.Size = new Size(170, 29);
        cboDocumentType.TabIndex = 61;
        // 
        // lbltxtDocumentNumber
        // 
        fieldsArchive.SetColumnSpan(lbltxtDocumentNumber, 2);
        lbltxtDocumentNumber.Dock = DockStyle.Fill;
        lbltxtDocumentNumber.Font = new Font("Tahoma", 9F);
        lbltxtDocumentNumber.Location = new Point(304, 2);
        lbltxtDocumentNumber.Margin = new Padding(1, 0, 1, 0);
        lbltxtDocumentNumber.Name = "lbltxtDocumentNumber";
        lbltxtDocumentNumber.Size = new Size(196, 30);
        lbltxtDocumentNumber.TabIndex = 62;
        lbltxtDocumentNumber.Text = "رقم الوثيقة";
        lbltxtDocumentNumber.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtDocumentNumber
        // 
        txtDocumentNumber.AccessibleName = "رقم الوثيقة";
        txtDocumentNumber.BackColor = Color.White;
        txtDocumentNumber.BorderStyle = BorderStyle.FixedSingle;
        fieldsArchive.SetColumnSpan(txtDocumentNumber, 2);
        txtDocumentNumber.Font = new Font("Tahoma", 10F);
        txtDocumentNumber.Location = new Point(207, 4);
        txtDocumentNumber.Margin = new Padding(1, 2, 1, 2);
        txtDocumentNumber.Name = "txtDocumentNumber";
        txtDocumentNumber.RightToLeft = RightToLeft.No;
        txtDocumentNumber.Size = new Size(95, 28);
        txtDocumentNumber.TabIndex = 63;
        // 
        // lbltxtIssuedAt
        // 
        fieldsArchive.SetColumnSpan(lbltxtIssuedAt, 2);
        lbltxtIssuedAt.Dock = DockStyle.Fill;
        lbltxtIssuedAt.Font = new Font("Tahoma", 9F);
        lbltxtIssuedAt.Location = new Point(700, 32);
        lbltxtIssuedAt.Margin = new Padding(1, 0, 1, 0);
        lbltxtIssuedAt.Name = "lbltxtIssuedAt";
        lbltxtIssuedAt.Size = new Size(196, 30);
        lbltxtIssuedAt.TabIndex = 64;
        lbltxtIssuedAt.Text = "تاريخ إصدار الوثيقة";
        lbltxtIssuedAt.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtIssuedAt
        // 
        txtIssuedAt.AccessibleName = "تاريخ إصدار الوثيقة";
        txtIssuedAt.BackColor = Color.White;
        txtIssuedAt.BorderStyle = BorderStyle.FixedSingle;
        fieldsArchive.SetColumnSpan(txtIssuedAt, 2);
        txtIssuedAt.Font = new Font("Tahoma", 10F);
        txtIssuedAt.Location = new Point(573, 34);
        txtIssuedAt.Margin = new Padding(1, 2, 1, 2);
        txtIssuedAt.Name = "txtIssuedAt";
        txtIssuedAt.RightToLeft = RightToLeft.No;
        txtIssuedAt.Size = new Size(125, 28);
        txtIssuedAt.TabIndex = 65;
        // 
        // lbltxtIssuedPlace
        // 
        fieldsArchive.SetColumnSpan(lbltxtIssuedPlace, 2);
        lbltxtIssuedPlace.Dock = DockStyle.Fill;
        lbltxtIssuedPlace.Font = new Font("Tahoma", 9F);
        lbltxtIssuedPlace.Location = new Point(304, 32);
        lbltxtIssuedPlace.Margin = new Padding(1, 0, 1, 0);
        lbltxtIssuedPlace.Name = "lbltxtIssuedPlace";
        lbltxtIssuedPlace.Size = new Size(196, 30);
        lbltxtIssuedPlace.TabIndex = 66;
        lbltxtIssuedPlace.Text = "مكان إصدار الوثيقة";
        lbltxtIssuedPlace.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtIssuedPlace
        // 
        txtIssuedPlace.AccessibleName = "مكان إصدار الوثيقة";
        txtIssuedPlace.BackColor = Color.White;
        txtIssuedPlace.BorderStyle = BorderStyle.FixedSingle;
        fieldsArchive.SetColumnSpan(txtIssuedPlace, 2);
        txtIssuedPlace.Font = new Font("Tahoma", 10F);
        txtIssuedPlace.Location = new Point(112, 34);
        txtIssuedPlace.Margin = new Padding(1, 2, 1, 2);
        txtIssuedPlace.Name = "txtIssuedPlace";
        txtIssuedPlace.RightToLeft = RightToLeft.Yes;
        txtIssuedPlace.Size = new Size(190, 28);
        txtIssuedPlace.TabIndex = 67;
        txtIssuedPlace.TextAlign = HorizontalAlignment.Right;
        // 
        // lbltxtExpiresAt
        // 
        fieldsArchive.SetColumnSpan(lbltxtExpiresAt, 2);
        lbltxtExpiresAt.Dock = DockStyle.Fill;
        lbltxtExpiresAt.Font = new Font("Tahoma", 9F);
        lbltxtExpiresAt.Location = new Point(700, 62);
        lbltxtExpiresAt.Margin = new Padding(1, 0, 1, 0);
        lbltxtExpiresAt.Name = "lbltxtExpiresAt";
        lbltxtExpiresAt.Size = new Size(196, 30);
        lbltxtExpiresAt.TabIndex = 68;
        lbltxtExpiresAt.Text = "تاريخ الانتهاء";
        lbltxtExpiresAt.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtExpiresAt
        // 
        txtExpiresAt.AccessibleName = "تاريخ الانتهاء";
        txtExpiresAt.BackColor = Color.White;
        txtExpiresAt.BorderStyle = BorderStyle.FixedSingle;
        fieldsArchive.SetColumnSpan(txtExpiresAt, 2);
        txtExpiresAt.Font = new Font("Tahoma", 10F);
        txtExpiresAt.Location = new Point(573, 64);
        txtExpiresAt.Margin = new Padding(1, 2, 1, 2);
        txtExpiresAt.Name = "txtExpiresAt";
        txtExpiresAt.RightToLeft = RightToLeft.No;
        txtExpiresAt.Size = new Size(125, 28);
        txtExpiresAt.TabIndex = 69;
        // 
        // lbltxtRenewedAt
        // 
        fieldsArchive.SetColumnSpan(lbltxtRenewedAt, 2);
        lbltxtRenewedAt.Dock = DockStyle.Fill;
        lbltxtRenewedAt.Font = new Font("Tahoma", 9F);
        lbltxtRenewedAt.Location = new Point(304, 62);
        lbltxtRenewedAt.Margin = new Padding(1, 0, 1, 0);
        lbltxtRenewedAt.Name = "lbltxtRenewedAt";
        lbltxtRenewedAt.Size = new Size(196, 30);
        lbltxtRenewedAt.TabIndex = 70;
        lbltxtRenewedAt.Text = "تاريخ التجديد";
        lbltxtRenewedAt.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtRenewedAt
        // 
        txtRenewedAt.AccessibleName = "تاريخ التجديد";
        txtRenewedAt.BackColor = Color.White;
        txtRenewedAt.BorderStyle = BorderStyle.FixedSingle;
        fieldsArchive.SetColumnSpan(txtRenewedAt, 2);
        txtRenewedAt.Font = new Font("Tahoma", 10F);
        txtRenewedAt.Location = new Point(177, 64);
        txtRenewedAt.Margin = new Padding(1, 2, 1, 2);
        txtRenewedAt.Name = "txtRenewedAt";
        txtRenewedAt.RightToLeft = RightToLeft.No;
        txtRenewedAt.Size = new Size(125, 28);
        txtRenewedAt.TabIndex = 71;
        // 
        // tabDocuments
        // 
        tabDocuments.AutoScroll = true;
        tabDocuments.BackColor = Color.FromArgb(244, 244, 244);
        tabDocuments.Controls.Add(fieldsDocuments);
        tabDocuments.Location = new Point(4, 29);
        tabDocuments.Margin = new Padding(0);
        tabDocuments.Name = "tabDocuments";
        tabDocuments.Padding = new Padding(3);
        tabDocuments.Size = new Size(1006, 535);
        tabDocuments.TabIndex = 3;
        tabDocuments.Text = "وثائق الفروع";
        // 
        // fieldsDocuments
        // 
        fieldsDocuments.AutoSize = true;
        fieldsDocuments.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        fieldsDocuments.BackColor = Color.White;
        fieldsDocuments.BorderStyle = BorderStyle.FixedSingle;
        fieldsDocuments.ColumnCount = 10;
        fieldsDocuments.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsDocuments.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsDocuments.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsDocuments.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsDocuments.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsDocuments.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsDocuments.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsDocuments.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsDocuments.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsDocuments.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsDocuments.Controls.Add(lbllstDocumentsToBranch, 1, 0);
        fieldsDocuments.Controls.Add(lstDocumentsToBranch, 2, 0);
        fieldsDocuments.Controls.Add(lbllstDocumentsToAdministration, 5, 0);
        fieldsDocuments.Controls.Add(lstDocumentsToAdministration, 6, 0);
        fieldsDocuments.Dock = DockStyle.Top;
        fieldsDocuments.Enabled = false;
        fieldsDocuments.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
        fieldsDocuments.Location = new Point(3, 3);
        fieldsDocuments.Margin = new Padding(1);
        fieldsDocuments.MinimumSize = new Size(700, 0);
        fieldsDocuments.Name = "fieldsDocuments";
        fieldsDocuments.Padding = new Padding(2);
        fieldsDocuments.RightToLeft = RightToLeft.Yes;
        fieldsDocuments.RowCount = 1;
        fieldsDocuments.RowStyles.Add(new RowStyle(SizeType.Absolute, 118F));
        fieldsDocuments.Size = new Size(1000, 124);
        fieldsDocuments.TabIndex = 0;
        // 
        // lbllstDocumentsToBranch
        // 
        lbllstDocumentsToBranch.Dock = DockStyle.Fill;
        lbllstDocumentsToBranch.Font = new Font("Tahoma", 9F);
        lbllstDocumentsToBranch.Location = new Point(799, 2);
        lbllstDocumentsToBranch.Margin = new Padding(1, 0, 1, 0);
        lbllstDocumentsToBranch.Name = "lbllstDocumentsToBranch";
        lbllstDocumentsToBranch.Size = new Size(97, 118);
        lbllstDocumentsToBranch.TabIndex = 76;
        lbllstDocumentsToBranch.Text = "من الإدارة إلى الفرع";
        lbllstDocumentsToBranch.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lstDocumentsToBranch
        // 
        lstDocumentsToBranch.AccessibleDescription = "قائمة فارغة؛ مصدر البيانات غير مرتبط.";
        lstDocumentsToBranch.AccessibleName = "من الإدارة إلى الفرع";
        lstDocumentsToBranch.BorderStyle = BorderStyle.FixedSingle;
        lstDocumentsToBranch.CheckOnClick = true;
        fieldsDocuments.SetColumnSpan(lstDocumentsToBranch, 3);
        lstDocumentsToBranch.Enabled = false;
        lstDocumentsToBranch.Font = new Font("Tahoma", 10F);
        lstDocumentsToBranch.IntegralHeight = false;
        lstDocumentsToBranch.Location = new Point(587, 4);
        lstDocumentsToBranch.Margin = new Padding(1, 2, 1, 2);
        lstDocumentsToBranch.Name = "lstDocumentsToBranch";
        lstDocumentsToBranch.Size = new Size(210, 110);
        lstDocumentsToBranch.TabIndex = 77;
        // 
        // lbllstDocumentsToAdministration
        // 
        lbllstDocumentsToAdministration.Dock = DockStyle.Fill;
        lbllstDocumentsToAdministration.Font = new Font("Tahoma", 9F);
        lbllstDocumentsToAdministration.Location = new Point(403, 2);
        lbllstDocumentsToAdministration.Margin = new Padding(1, 0, 1, 0);
        lbllstDocumentsToAdministration.Name = "lbllstDocumentsToAdministration";
        lbllstDocumentsToAdministration.Size = new Size(97, 118);
        lbllstDocumentsToAdministration.TabIndex = 78;
        lbllstDocumentsToAdministration.Text = "من الفرع إلى الإدارة";
        lbllstDocumentsToAdministration.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lstDocumentsToAdministration
        // 
        lstDocumentsToAdministration.AccessibleDescription = "قائمة فارغة؛ مصدر البيانات غير مرتبط.";
        lstDocumentsToAdministration.AccessibleName = "من الفرع إلى الإدارة";
        lstDocumentsToAdministration.BorderStyle = BorderStyle.FixedSingle;
        lstDocumentsToAdministration.CheckOnClick = true;
        fieldsDocuments.SetColumnSpan(lstDocumentsToAdministration, 3);
        lstDocumentsToAdministration.Enabled = false;
        lstDocumentsToAdministration.Font = new Font("Tahoma", 10F);
        lstDocumentsToAdministration.IntegralHeight = false;
        lstDocumentsToAdministration.Location = new Point(191, 4);
        lstDocumentsToAdministration.Margin = new Padding(1, 2, 1, 2);
        lstDocumentsToAdministration.Name = "lstDocumentsToAdministration";
        lstDocumentsToAdministration.Size = new Size(210, 110);
        lstDocumentsToAdministration.TabIndex = 79;
        // 
        // tabHeaders
        // 
        tabHeaders.AutoScroll = true;
        tabHeaders.BackColor = Color.FromArgb(244, 244, 244);
        tabHeaders.Controls.Add(fieldsHeaders);
        tabHeaders.Location = new Point(4, 29);
        tabHeaders.Margin = new Padding(0);
        tabHeaders.Name = "tabHeaders";
        tabHeaders.Padding = new Padding(3);
        tabHeaders.Size = new Size(1006, 535);
        tabHeaders.TabIndex = 4;
        tabHeaders.Text = "ترويسة التقارير";
        // 
        // fieldsHeaders
        // 
        fieldsHeaders.AutoSize = true;
        fieldsHeaders.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        fieldsHeaders.BackColor = Color.White;
        fieldsHeaders.BorderStyle = BorderStyle.FixedSingle;
        fieldsHeaders.ColumnCount = 10;
        fieldsHeaders.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsHeaders.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsHeaders.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsHeaders.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsHeaders.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsHeaders.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsHeaders.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsHeaders.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsHeaders.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsHeaders.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsHeaders.Controls.Add(lblcboHeaderType, 3, 0);
        fieldsHeaders.Controls.Add(cboHeaderType, 5, 0);
        fieldsHeaders.Dock = DockStyle.Top;
        fieldsHeaders.Enabled = false;
        fieldsHeaders.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
        fieldsHeaders.Location = new Point(3, 3);
        fieldsHeaders.Margin = new Padding(1);
        fieldsHeaders.MinimumSize = new Size(700, 0);
        fieldsHeaders.Name = "fieldsHeaders";
        fieldsHeaders.Padding = new Padding(2);
        fieldsHeaders.RightToLeft = RightToLeft.Yes;
        fieldsHeaders.RowCount = 1;
        fieldsHeaders.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        fieldsHeaders.Size = new Size(1000, 36);
        fieldsHeaders.TabIndex = 0;
        // 
        // lblcboHeaderType
        // 
        fieldsHeaders.SetColumnSpan(lblcboHeaderType, 2);
        lblcboHeaderType.Dock = DockStyle.Fill;
        lblcboHeaderType.Font = new Font("Tahoma", 9F);
        lblcboHeaderType.Location = new Point(502, 2);
        lblcboHeaderType.Margin = new Padding(1, 0, 1, 0);
        lblcboHeaderType.Name = "lblcboHeaderType";
        lblcboHeaderType.Size = new Size(196, 30);
        lblcboHeaderType.TabIndex = 72;
        lblcboHeaderType.Text = "نوع ترويسة التقرير";
        lblcboHeaderType.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cboHeaderType
        // 
        cboHeaderType.AccessibleDescription = "قائمة فارغة؛ مصدر البيانات غير مرتبط.";
        cboHeaderType.AccessibleName = "نوع ترويسة التقرير";
        cboHeaderType.BackColor = Color.White;
        fieldsHeaders.SetColumnSpan(cboHeaderType, 3);
        cboHeaderType.DropDownStyle = ComboBoxStyle.DropDownList;
        cboHeaderType.Enabled = false;
        cboHeaderType.Font = new Font("Tahoma", 10F);
        cboHeaderType.Location = new Point(330, 4);
        cboHeaderType.Margin = new Padding(1, 2, 1, 2);
        cboHeaderType.Name = "cboHeaderType";
        cboHeaderType.Size = new Size(170, 29);
        cboHeaderType.TabIndex = 73;
        // 
        // tabAddresses
        // 
        tabAddresses.AutoScroll = true;
        tabAddresses.BackColor = Color.FromArgb(244, 244, 244);
        tabAddresses.Controls.Add(fieldsAddresses);
        tabAddresses.Location = new Point(4, 29);
        tabAddresses.Margin = new Padding(0);
        tabAddresses.Name = "tabAddresses";
        tabAddresses.Padding = new Padding(3);
        tabAddresses.Size = new Size(1006, 535);
        tabAddresses.TabIndex = 5;
        tabAddresses.Text = "العناوين والمعرفات";
        // 
        // fieldsAddresses
        // 
        fieldsAddresses.AutoSize = true;
        fieldsAddresses.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        fieldsAddresses.BackColor = Color.White;
        fieldsAddresses.BorderStyle = BorderStyle.FixedSingle;
        fieldsAddresses.ColumnCount = 10;
        fieldsAddresses.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsAddresses.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsAddresses.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsAddresses.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsAddresses.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsAddresses.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsAddresses.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsAddresses.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsAddresses.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsAddresses.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        fieldsAddresses.Controls.Add(lbltxtAddressesIdentifiers, 2, 0);
        fieldsAddresses.Controls.Add(addressEditorPanel, 4, 0);
        fieldsAddresses.Controls.Add(grpbtnSelectAddressesIdentifiers, 0, 1);
        fieldsAddresses.Dock = DockStyle.Top;
        fieldsAddresses.Enabled = false;
        fieldsAddresses.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
        fieldsAddresses.Location = new Point(3, 3);
        fieldsAddresses.Margin = new Padding(1);
        fieldsAddresses.MinimumSize = new Size(700, 0);
        fieldsAddresses.Name = "fieldsAddresses";
        fieldsAddresses.Padding = new Padding(2);
        fieldsAddresses.RightToLeft = RightToLeft.Yes;
        fieldsAddresses.RowCount = 2;
        fieldsAddresses.RowStyles.Add(new RowStyle(SizeType.Absolute, 84F));
        fieldsAddresses.RowStyles.Add(new RowStyle());
        fieldsAddresses.Size = new Size(1000, 280);
        fieldsAddresses.TabIndex = 0;
        // 
        // lbltxtAddressesIdentifiers
        // 
        fieldsAddresses.SetColumnSpan(lbltxtAddressesIdentifiers, 2);
        lbltxtAddressesIdentifiers.Dock = DockStyle.Fill;
        lbltxtAddressesIdentifiers.Font = new Font("Tahoma", 9F);
        lbltxtAddressesIdentifiers.Location = new Point(601, 2);
        lbltxtAddressesIdentifiers.Margin = new Padding(1, 0, 1, 0);
        lbltxtAddressesIdentifiers.Name = "lbltxtAddressesIdentifiers";
        lbltxtAddressesIdentifiers.Size = new Size(196, 84);
        lbltxtAddressesIdentifiers.TabIndex = 74;
        lbltxtAddressesIdentifiers.Text = "العناوين والمعرفات";
        lbltxtAddressesIdentifiers.TextAlign = ContentAlignment.TopRight;
        // 
        // addressEditorPanel
        // 
        fieldsAddresses.SetColumnSpan(addressEditorPanel, 4);
        addressEditorPanel.Controls.Add(txtAddressesIdentifiers);
        addressEditorPanel.Controls.Add(btnSelectAddressesIdentifiers);
        addressEditorPanel.Location = new Point(307, 4);
        addressEditorPanel.Margin = new Padding(1, 2, 1, 2);
        addressEditorPanel.Name = "addressEditorPanel";
        addressEditorPanel.Size = new Size(292, 78);
        addressEditorPanel.TabIndex = 75;
        // 
        // txtAddressesIdentifiers
        // 
        txtAddressesIdentifiers.AccessibleDescription = "قيمة للعرض فقط؛ مصدر البيانات غير مرتبط.";
        txtAddressesIdentifiers.AccessibleName = "العناوين والمعرفات";
        txtAddressesIdentifiers.BackColor = Color.FromArgb(240, 240, 240);
        txtAddressesIdentifiers.BorderStyle = BorderStyle.FixedSingle;
        txtAddressesIdentifiers.Dock = DockStyle.Right;
        txtAddressesIdentifiers.Font = new Font("Tahoma", 10F);
        txtAddressesIdentifiers.Location = new Point(32, 0);
        txtAddressesIdentifiers.Margin = new Padding(1, 2, 1, 2);
        txtAddressesIdentifiers.Multiline = true;
        txtAddressesIdentifiers.Name = "txtAddressesIdentifiers";
        txtAddressesIdentifiers.ReadOnly = true;
        txtAddressesIdentifiers.RightToLeft = RightToLeft.Yes;
        txtAddressesIdentifiers.ScrollBars = ScrollBars.Vertical;
        txtAddressesIdentifiers.Size = new Size(260, 78);
        txtAddressesIdentifiers.TabIndex = 75;
        txtAddressesIdentifiers.TextAlign = HorizontalAlignment.Right;
        // 
        // btnSelectAddressesIdentifiers
        // 
        btnSelectAddressesIdentifiers.AccessibleName = "العناوين والمعرفات";
        btnSelectAddressesIdentifiers.Location = new Point(0, 0);
        btnSelectAddressesIdentifiers.Margin = new Padding(1, 2, 1, 2);
        btnSelectAddressesIdentifiers.Name = "btnSelectAddressesIdentifiers";
        btnSelectAddressesIdentifiers.Size = new Size(28, 25);
        btnSelectAddressesIdentifiers.TabIndex = 76;
        btnSelectAddressesIdentifiers.Text = "…";
        btnSelectAddressesIdentifiers.UseVisualStyleBackColor = true;
        btnSelectAddressesIdentifiers.Click += addresses_Click;
        // 
        // grpbtnSelectAddressesIdentifiers
        // 
        grpbtnSelectAddressesIdentifiers.BackColor = Color.White;
        fieldsAddresses.SetColumnSpan(grpbtnSelectAddressesIdentifiers, 10);
        grpbtnSelectAddressesIdentifiers.Controls.Add(lstbtnSelectAddressesIdentifiers);
        grpbtnSelectAddressesIdentifiers.Controls.Add(lblAddressEmpty);
        grpbtnSelectAddressesIdentifiers.Dock = DockStyle.Fill;
        grpbtnSelectAddressesIdentifiers.Location = new Point(4, 88);
        grpbtnSelectAddressesIdentifiers.Margin = new Padding(2);
        grpbtnSelectAddressesIdentifiers.MinimumSize = new Size(0, 186);
        grpbtnSelectAddressesIdentifiers.Name = "grpbtnSelectAddressesIdentifiers";
        grpbtnSelectAddressesIdentifiers.Padding = new Padding(3, 18, 3, 3);
        grpbtnSelectAddressesIdentifiers.Size = new Size(990, 186);
        grpbtnSelectAddressesIdentifiers.TabIndex = 77;
        grpbtnSelectAddressesIdentifiers.TabStop = false;
        grpbtnSelectAddressesIdentifiers.Text = "العناوين والمعرفات";
        grpbtnSelectAddressesIdentifiers.Visible = false;
        // 
        // lstbtnSelectAddressesIdentifiers
        // 
        lstbtnSelectAddressesIdentifiers.BorderStyle = BorderStyle.FixedSingle;
        lstbtnSelectAddressesIdentifiers.Dock = DockStyle.Fill;
        lstbtnSelectAddressesIdentifiers.Enabled = false;
        lstbtnSelectAddressesIdentifiers.IntegralHeight = false;
        lstbtnSelectAddressesIdentifiers.Location = new Point(3, 37);
        lstbtnSelectAddressesIdentifiers.Name = "lstbtnSelectAddressesIdentifiers";
        lstbtnSelectAddressesIdentifiers.Size = new Size(984, 120);
        lstbtnSelectAddressesIdentifiers.TabIndex = 0;
        // 
        // lblAddressEmpty
        // 
        lblAddressEmpty.Dock = DockStyle.Bottom;
        lblAddressEmpty.Location = new Point(3, 157);
        lblAddressEmpty.Margin = new Padding(2);
        lblAddressEmpty.Name = "lblAddressEmpty";
        lblAddressEmpty.Size = new Size(984, 26);
        lblAddressEmpty.TabIndex = 1;
        lblAddressEmpty.Text = "القائمة غير مرتبطة ببيانات؛ لا توجد سجلات للاختيار.";
        lblAddressEmpty.TextAlign = ContentAlignment.MiddleRight;
        // 
        // auditInfoContainer
        // 
        auditInfoContainer.BackColor = Color.FromArgb(232, 227, 246);
        auditInfoContainer.BorderStyle = BorderStyle.FixedSingle;
        auditInfoContainer.ColumnCount = 9;
        auditInfoContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
        auditInfoContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 45F));
        auditInfoContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 31F));
        auditInfoContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
        auditInfoContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 31F));
        auditInfoContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
        auditInfoContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 26F));
        auditInfoContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
        auditInfoContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12F));
        auditInfoContainer.Controls.Add(lbltxtCreatedBy, 0, 0);
        auditInfoContainer.Controls.Add(txtCreatedBy, 2, 0);
        auditInfoContainer.Controls.Add(lbltxtUpdatedBy, 0, 1);
        auditInfoContainer.Controls.Add(txtUpdatedBy, 2, 1);
        auditInfoContainer.Controls.Add(lbltxtCreatedAt, 3, 0);
        auditInfoContainer.Controls.Add(txtCreatedAt, 4, 0);
        auditInfoContainer.Controls.Add(lbltxtUpdatedAt, 3, 1);
        auditInfoContainer.Controls.Add(txtUpdatedAt, 4, 1);
        auditInfoContainer.Controls.Add(lbltxtCreatedDevice, 5, 0);
        auditInfoContainer.Controls.Add(txtCreatedDevice, 6, 0);
        auditInfoContainer.Controls.Add(lbltxtUpdatedDevice, 5, 1);
        auditInfoContainer.Controls.Add(txtUpdatedDevice, 6, 1);
        auditInfoContainer.Controls.Add(lbltxtPrintCount, 7, 0);
        auditInfoContainer.Controls.Add(txtPrintCount, 8, 0);
        auditInfoContainer.Controls.Add(lbltxtUpdateCount, 7, 1);
        auditInfoContainer.Controls.Add(txtUpdateCount, 8, 1);
        auditInfoContainer.Controls.Add(txtCreatedByCode, 1, 0);
        auditInfoContainer.Controls.Add(txtUpdatedByCode, 1, 1);
        auditInfoContainer.Dock = DockStyle.Fill;
        auditInfoContainer.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
        auditInfoContainer.Location = new Point(3, 643);
        auditInfoContainer.Margin = new Padding(1);
        auditInfoContainer.MinimumSize = new Size(1000, 64);
        auditInfoContainer.Name = "auditInfoContainer";
        auditInfoContainer.Padding = new Padding(2);
        auditInfoContainer.RightToLeft = RightToLeft.Yes;
        auditInfoContainer.RowCount = 2;
        auditInfoContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        auditInfoContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        auditInfoContainer.Size = new Size(1014, 64);
        auditInfoContainer.TabIndex = 3;
        // 
        // lbltxtCreatedBy
        // 
        lbltxtCreatedBy.BackColor = Color.Transparent;
        lbltxtCreatedBy.Dock = DockStyle.Fill;
        lbltxtCreatedBy.Font = new Font("Tahoma", 9F);
        lbltxtCreatedBy.ForeColor = Color.FromArgb(32, 32, 32);
        lbltxtCreatedBy.Location = new Point(901, 3);
        lbltxtCreatedBy.Margin = new Padding(1);
        lbltxtCreatedBy.Name = "lbltxtCreatedBy";
        lbltxtCreatedBy.RightToLeft = RightToLeft.Yes;
        lbltxtCreatedBy.Size = new Size(108, 27);
        lbltxtCreatedBy.TabIndex = 0;
        lbltxtCreatedBy.Text = "مدخل السجل";
        lbltxtCreatedBy.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtCreatedBy
        // 
        txtCreatedBy.AccessibleDescription = "بيانات عرض فقط؛ تبقى فارغة حتى ربط مصدر بيانات السجل.";
        txtCreatedBy.AccessibleName = "مدخل السجل";
        txtCreatedBy.BackColor = Color.FromArgb(244, 244, 244);
        txtCreatedBy.BorderStyle = BorderStyle.FixedSingle;
        txtCreatedBy.Dock = DockStyle.Fill;
        txtCreatedBy.Font = new Font("Tahoma", 9F);
        txtCreatedBy.ForeColor = Color.FromArgb(32, 32, 32);
        txtCreatedBy.Location = new Point(694, 4);
        txtCreatedBy.Margin = new Padding(1, 2, 1, 2);
        txtCreatedBy.Name = "txtCreatedBy";
        txtCreatedBy.ReadOnly = true;
        txtCreatedBy.RightToLeft = RightToLeft.Yes;
        txtCreatedBy.Size = new Size(160, 26);
        txtCreatedBy.TabIndex = 1;
        txtCreatedBy.TabStop = false;
        txtCreatedBy.TextAlign = HorizontalAlignment.Right;
        // 
        // lbltxtUpdatedBy
        // 
        lbltxtUpdatedBy.BackColor = Color.Transparent;
        lbltxtUpdatedBy.Dock = DockStyle.Fill;
        lbltxtUpdatedBy.Font = new Font("Tahoma", 9F);
        lbltxtUpdatedBy.ForeColor = Color.FromArgb(32, 32, 32);
        lbltxtUpdatedBy.Location = new Point(901, 32);
        lbltxtUpdatedBy.Margin = new Padding(1);
        lbltxtUpdatedBy.Name = "lbltxtUpdatedBy";
        lbltxtUpdatedBy.RightToLeft = RightToLeft.Yes;
        lbltxtUpdatedBy.Size = new Size(108, 27);
        lbltxtUpdatedBy.TabIndex = 2;
        lbltxtUpdatedBy.Text = "معدل السجل";
        lbltxtUpdatedBy.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtUpdatedBy
        // 
        txtUpdatedBy.AccessibleDescription = "بيانات عرض فقط؛ تبقى فارغة حتى ربط مصدر بيانات السجل.";
        txtUpdatedBy.AccessibleName = "معدل السجل";
        txtUpdatedBy.BackColor = Color.FromArgb(244, 244, 244);
        txtUpdatedBy.BorderStyle = BorderStyle.FixedSingle;
        txtUpdatedBy.Dock = DockStyle.Fill;
        txtUpdatedBy.Font = new Font("Tahoma", 9F);
        txtUpdatedBy.ForeColor = Color.FromArgb(32, 32, 32);
        txtUpdatedBy.Location = new Point(694, 33);
        txtUpdatedBy.Margin = new Padding(1, 2, 1, 2);
        txtUpdatedBy.Name = "txtUpdatedBy";
        txtUpdatedBy.ReadOnly = true;
        txtUpdatedBy.RightToLeft = RightToLeft.Yes;
        txtUpdatedBy.Size = new Size(160, 26);
        txtUpdatedBy.TabIndex = 3;
        txtUpdatedBy.TabStop = false;
        txtUpdatedBy.TextAlign = HorizontalAlignment.Right;
        // 
        // lbltxtCreatedAt
        // 
        lbltxtCreatedAt.BackColor = Color.Transparent;
        lbltxtCreatedAt.Dock = DockStyle.Fill;
        lbltxtCreatedAt.Font = new Font("Tahoma", 9F);
        lbltxtCreatedAt.ForeColor = Color.FromArgb(32, 32, 32);
        lbltxtCreatedAt.Location = new Point(584, 3);
        lbltxtCreatedAt.Margin = new Padding(1);
        lbltxtCreatedAt.Name = "lbltxtCreatedAt";
        lbltxtCreatedAt.RightToLeft = RightToLeft.Yes;
        lbltxtCreatedAt.Size = new Size(108, 27);
        lbltxtCreatedAt.TabIndex = 4;
        lbltxtCreatedAt.Text = "تاريخ الإدخال";
        lbltxtCreatedAt.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtCreatedAt
        // 
        txtCreatedAt.AccessibleDescription = "بيانات عرض فقط؛ تبقى فارغة حتى ربط مصدر بيانات السجل.";
        txtCreatedAt.AccessibleName = "تاريخ الإدخال";
        txtCreatedAt.BackColor = Color.FromArgb(244, 244, 244);
        txtCreatedAt.BorderStyle = BorderStyle.FixedSingle;
        txtCreatedAt.Dock = DockStyle.Fill;
        txtCreatedAt.Font = new Font("Tahoma", 9F);
        txtCreatedAt.ForeColor = Color.FromArgb(32, 32, 32);
        txtCreatedAt.Location = new Point(422, 4);
        txtCreatedAt.Margin = new Padding(1, 2, 1, 2);
        txtCreatedAt.Name = "txtCreatedAt";
        txtCreatedAt.ReadOnly = true;
        txtCreatedAt.RightToLeft = RightToLeft.No;
        txtCreatedAt.Size = new Size(160, 26);
        txtCreatedAt.TabIndex = 5;
        txtCreatedAt.TabStop = false;
        // 
        // lbltxtUpdatedAt
        // 
        lbltxtUpdatedAt.BackColor = Color.Transparent;
        lbltxtUpdatedAt.Dock = DockStyle.Fill;
        lbltxtUpdatedAt.Font = new Font("Tahoma", 9F);
        lbltxtUpdatedAt.ForeColor = Color.FromArgb(32, 32, 32);
        lbltxtUpdatedAt.Location = new Point(584, 32);
        lbltxtUpdatedAt.Margin = new Padding(1);
        lbltxtUpdatedAt.Name = "lbltxtUpdatedAt";
        lbltxtUpdatedAt.RightToLeft = RightToLeft.Yes;
        lbltxtUpdatedAt.Size = new Size(108, 27);
        lbltxtUpdatedAt.TabIndex = 6;
        lbltxtUpdatedAt.Text = "تاريخ آخر تعديل";
        lbltxtUpdatedAt.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtUpdatedAt
        // 
        txtUpdatedAt.AccessibleDescription = "بيانات عرض فقط؛ تبقى فارغة حتى ربط مصدر بيانات السجل.";
        txtUpdatedAt.AccessibleName = "تاريخ آخر تعديل";
        txtUpdatedAt.BackColor = Color.FromArgb(244, 244, 244);
        txtUpdatedAt.BorderStyle = BorderStyle.FixedSingle;
        txtUpdatedAt.Dock = DockStyle.Fill;
        txtUpdatedAt.Font = new Font("Tahoma", 9F);
        txtUpdatedAt.ForeColor = Color.FromArgb(32, 32, 32);
        txtUpdatedAt.Location = new Point(422, 33);
        txtUpdatedAt.Margin = new Padding(1, 2, 1, 2);
        txtUpdatedAt.Name = "txtUpdatedAt";
        txtUpdatedAt.ReadOnly = true;
        txtUpdatedAt.RightToLeft = RightToLeft.No;
        txtUpdatedAt.Size = new Size(160, 26);
        txtUpdatedAt.TabIndex = 7;
        txtUpdatedAt.TabStop = false;
        // 
        // lbltxtCreatedDevice
        // 
        lbltxtCreatedDevice.BackColor = Color.Transparent;
        lbltxtCreatedDevice.Dock = DockStyle.Fill;
        lbltxtCreatedDevice.Font = new Font("Tahoma", 9F);
        lbltxtCreatedDevice.ForeColor = Color.FromArgb(32, 32, 32);
        lbltxtCreatedDevice.Location = new Point(312, 3);
        lbltxtCreatedDevice.Margin = new Padding(1);
        lbltxtCreatedDevice.Name = "lbltxtCreatedDevice";
        lbltxtCreatedDevice.RightToLeft = RightToLeft.Yes;
        lbltxtCreatedDevice.Size = new Size(108, 27);
        lbltxtCreatedDevice.TabIndex = 12;
        lbltxtCreatedDevice.Text = "الجهاز المدخل";
        lbltxtCreatedDevice.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtCreatedDevice
        // 
        txtCreatedDevice.AccessibleDescription = "بيانات عرض فقط؛ تبقى فارغة حتى ربط مصدر بيانات السجل.";
        txtCreatedDevice.AccessibleName = "الجهاز المدخل";
        txtCreatedDevice.BackColor = Color.FromArgb(244, 244, 244);
        txtCreatedDevice.BorderStyle = BorderStyle.FixedSingle;
        txtCreatedDevice.Dock = DockStyle.Fill;
        txtCreatedDevice.Font = new Font("Tahoma", 9F);
        txtCreatedDevice.ForeColor = Color.FromArgb(32, 32, 32);
        txtCreatedDevice.Location = new Point(177, 4);
        txtCreatedDevice.Margin = new Padding(1, 2, 1, 2);
        txtCreatedDevice.Name = "txtCreatedDevice";
        txtCreatedDevice.ReadOnly = true;
        txtCreatedDevice.RightToLeft = RightToLeft.No;
        txtCreatedDevice.Size = new Size(133, 26);
        txtCreatedDevice.TabIndex = 13;
        txtCreatedDevice.TabStop = false;
        // 
        // lbltxtUpdatedDevice
        // 
        lbltxtUpdatedDevice.BackColor = Color.Transparent;
        lbltxtUpdatedDevice.Dock = DockStyle.Fill;
        lbltxtUpdatedDevice.Font = new Font("Tahoma", 9F);
        lbltxtUpdatedDevice.ForeColor = Color.FromArgb(32, 32, 32);
        lbltxtUpdatedDevice.Location = new Point(312, 32);
        lbltxtUpdatedDevice.Margin = new Padding(1);
        lbltxtUpdatedDevice.Name = "lbltxtUpdatedDevice";
        lbltxtUpdatedDevice.RightToLeft = RightToLeft.Yes;
        lbltxtUpdatedDevice.Size = new Size(108, 27);
        lbltxtUpdatedDevice.TabIndex = 14;
        lbltxtUpdatedDevice.Text = "الجهاز المعدل";
        lbltxtUpdatedDevice.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtUpdatedDevice
        // 
        txtUpdatedDevice.AccessibleDescription = "بيانات عرض فقط؛ تبقى فارغة حتى ربط مصدر بيانات السجل.";
        txtUpdatedDevice.AccessibleName = "الجهاز المعدل";
        txtUpdatedDevice.BackColor = Color.FromArgb(244, 244, 244);
        txtUpdatedDevice.BorderStyle = BorderStyle.FixedSingle;
        txtUpdatedDevice.Dock = DockStyle.Fill;
        txtUpdatedDevice.Font = new Font("Tahoma", 9F);
        txtUpdatedDevice.ForeColor = Color.FromArgb(32, 32, 32);
        txtUpdatedDevice.Location = new Point(177, 33);
        txtUpdatedDevice.Margin = new Padding(1, 2, 1, 2);
        txtUpdatedDevice.Name = "txtUpdatedDevice";
        txtUpdatedDevice.ReadOnly = true;
        txtUpdatedDevice.RightToLeft = RightToLeft.No;
        txtUpdatedDevice.Size = new Size(133, 26);
        txtUpdatedDevice.TabIndex = 15;
        txtUpdatedDevice.TabStop = false;
        // 
        // lbltxtPrintCount
        // 
        lbltxtPrintCount.BackColor = Color.Transparent;
        lbltxtPrintCount.Dock = DockStyle.Fill;
        lbltxtPrintCount.Font = new Font("Tahoma", 9F);
        lbltxtPrintCount.ForeColor = Color.FromArgb(32, 32, 32);
        lbltxtPrintCount.Location = new Point(67, 3);
        lbltxtPrintCount.Margin = new Padding(1);
        lbltxtPrintCount.Name = "lbltxtPrintCount";
        lbltxtPrintCount.RightToLeft = RightToLeft.Yes;
        lbltxtPrintCount.Size = new Size(108, 27);
        lbltxtPrintCount.TabIndex = 8;
        lbltxtPrintCount.Text = "مرات الطباعة";
        lbltxtPrintCount.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtPrintCount
        // 
        txtPrintCount.AccessibleDescription = "بيانات عرض فقط؛ تبقى فارغة حتى ربط مصدر بيانات السجل.";
        txtPrintCount.AccessibleName = "مرات الطباعة";
        txtPrintCount.BackColor = Color.FromArgb(244, 244, 244);
        txtPrintCount.BorderStyle = BorderStyle.FixedSingle;
        txtPrintCount.Dock = DockStyle.Fill;
        txtPrintCount.Font = new Font("Tahoma", 9F);
        txtPrintCount.ForeColor = Color.FromArgb(32, 32, 32);
        txtPrintCount.Location = new Point(3, 4);
        txtPrintCount.Margin = new Padding(1, 2, 1, 2);
        txtPrintCount.Name = "txtPrintCount";
        txtPrintCount.ReadOnly = true;
        txtPrintCount.RightToLeft = RightToLeft.No;
        txtPrintCount.Size = new Size(62, 26);
        txtPrintCount.TabIndex = 9;
        txtPrintCount.TabStop = false;
        txtPrintCount.TextAlign = HorizontalAlignment.Center;
        // 
        // lbltxtUpdateCount
        // 
        lbltxtUpdateCount.BackColor = Color.Transparent;
        lbltxtUpdateCount.Dock = DockStyle.Fill;
        lbltxtUpdateCount.Font = new Font("Tahoma", 9F);
        lbltxtUpdateCount.ForeColor = Color.FromArgb(32, 32, 32);
        lbltxtUpdateCount.Location = new Point(67, 32);
        lbltxtUpdateCount.Margin = new Padding(1);
        lbltxtUpdateCount.Name = "lbltxtUpdateCount";
        lbltxtUpdateCount.RightToLeft = RightToLeft.Yes;
        lbltxtUpdateCount.Size = new Size(108, 27);
        lbltxtUpdateCount.TabIndex = 10;
        lbltxtUpdateCount.Text = "مرات التعديل";
        lbltxtUpdateCount.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtUpdateCount
        // 
        txtUpdateCount.AccessibleDescription = "بيانات عرض فقط؛ تبقى فارغة حتى ربط مصدر بيانات السجل.";
        txtUpdateCount.AccessibleName = "مرات التعديل";
        txtUpdateCount.BackColor = Color.FromArgb(244, 244, 244);
        txtUpdateCount.BorderStyle = BorderStyle.FixedSingle;
        txtUpdateCount.Dock = DockStyle.Fill;
        txtUpdateCount.Font = new Font("Tahoma", 9F);
        txtUpdateCount.ForeColor = Color.FromArgb(32, 32, 32);
        txtUpdateCount.Location = new Point(3, 33);
        txtUpdateCount.Margin = new Padding(1, 2, 1, 2);
        txtUpdateCount.Name = "txtUpdateCount";
        txtUpdateCount.ReadOnly = true;
        txtUpdateCount.RightToLeft = RightToLeft.No;
        txtUpdateCount.Size = new Size(62, 26);
        txtUpdateCount.TabIndex = 11;
        txtUpdateCount.TabStop = false;
        txtUpdateCount.TextAlign = HorizontalAlignment.Center;
        // 
        // txtCreatedByCode
        // 
        txtCreatedByCode.AccessibleDescription = "رمز المستخدم بجانب اسمه كما في المرجع؛ يبقى فارغًا حتى الربط بمصدر السجل.";
        txtCreatedByCode.BackColor = Color.FromArgb(244, 244, 244);
        txtCreatedByCode.BorderStyle = BorderStyle.FixedSingle;
        txtCreatedByCode.Dock = DockStyle.Fill;
        txtCreatedByCode.Font = new Font("Tahoma", 9F);
        txtCreatedByCode.ForeColor = Color.FromArgb(32, 32, 32);
        txtCreatedByCode.Location = new Point(855, 4);
        txtCreatedByCode.Margin = new Padding(1, 2, 0, 2);
        txtCreatedByCode.Name = "txtCreatedByCode";
        txtCreatedByCode.ReadOnly = true;
        txtCreatedByCode.RightToLeft = RightToLeft.No;
        txtCreatedByCode.Size = new Size(44, 26);
        txtCreatedByCode.TabIndex = 16;
        txtCreatedByCode.TabStop = false;
        txtCreatedByCode.TextAlign = HorizontalAlignment.Center;
        // 
        // txtUpdatedByCode
        // 
        txtUpdatedByCode.AccessibleDescription = "رمز المستخدم بجانب اسمه كما في المرجع؛ يبقى فارغًا حتى الربط بمصدر السجل.";
        txtUpdatedByCode.BackColor = Color.FromArgb(244, 244, 244);
        txtUpdatedByCode.BorderStyle = BorderStyle.FixedSingle;
        txtUpdatedByCode.Dock = DockStyle.Fill;
        txtUpdatedByCode.Font = new Font("Tahoma", 9F);
        txtUpdatedByCode.ForeColor = Color.FromArgb(32, 32, 32);
        txtUpdatedByCode.Location = new Point(855, 33);
        txtUpdatedByCode.Margin = new Padding(1, 2, 0, 2);
        txtUpdatedByCode.Name = "txtUpdatedByCode";
        txtUpdatedByCode.ReadOnly = true;
        txtUpdatedByCode.RightToLeft = RightToLeft.No;
        txtUpdatedByCode.Size = new Size(44, 26);
        txtUpdatedByCode.TabIndex = 17;
        txtUpdatedByCode.TabStop = false;
        txtUpdatedByCode.TextAlign = HorizontalAlignment.Center;
        // 
        // UcBranchData
        // 
        AutoScaleDimensions = new SizeF(120F, 120F);
        AutoScaleMode = AutoScaleMode.Dpi;
        AutoScroll = true;
        BackColor = Color.FromArgb(244, 244, 244);
        Controls.Add(layout);
        Font = new Font("Tahoma", 9F);
        Margin = new Padding(0);
        MinimumSize = new Size(600, 440);
        Name = "UcBranchData";
        RightToLeft = RightToLeft.Yes;
        Size = new Size(1020, 736);
        layout.ResumeLayout(false);
        pnlToolbar.ResumeLayout(false);
        commandsLayout.ResumeLayout(false);
        tabsFields.ResumeLayout(false);
        tabMain.ResumeLayout(false);
        pnlMainFields.ResumeLayout(false);
        pnlMainFields.PerformLayout();
        fieldsMain.ResumeLayout(false);
        fieldsMain.PerformLayout();
        grpLogo.ResumeLayout(false);
        ((ISupportInitialize)picLogo).EndInit();
        tabDetails.ResumeLayout(false);
        tabDetails.PerformLayout();
        fieldsDetails.ResumeLayout(false);
        fieldsDetails.PerformLayout();
        tabArchive.ResumeLayout(false);
        tabArchive.PerformLayout();
        fieldsArchive.ResumeLayout(false);
        fieldsArchive.PerformLayout();
        tabDocuments.ResumeLayout(false);
        tabDocuments.PerformLayout();
        fieldsDocuments.ResumeLayout(false);
        tabHeaders.ResumeLayout(false);
        tabHeaders.PerformLayout();
        fieldsHeaders.ResumeLayout(false);
        tabAddresses.ResumeLayout(false);
        tabAddresses.PerformLayout();
        fieldsAddresses.ResumeLayout(false);
        addressEditorPanel.ResumeLayout(false);
        addressEditorPanel.PerformLayout();
        grpbtnSelectAddressesIdentifiers.ResumeLayout(false);
        auditInfoContainer.ResumeLayout(false);
        auditInfoContainer.PerformLayout();
        ResumeLayout(false);
    
        // Explicit Designer-owned command catalog.
        designerCommandBar.Dock = DockStyle.Fill;
        designerCommandBar.Location = new Point(3, 33);
        designerCommandBar.Size = new Size(1014, 38);
        designerCommandBar.Margin = new Padding(1);
        designerCommandBar.MinimumSize = new Size(0, 36);
        designerCommandBar.TabIndex = 1;
        designerCommandBar.Controls.Add(pnlToolbar);
        pnlToolbar.Dock = DockStyle.Fill;
        pnlToolbar.Margin = Padding.Empty;
        designerCommandBar.Name = "designerCommandBar";
        designerCommandBar.BackColor = Color.FromArgb(239, 239, 239);
        designerCommandBar.BorderStyle = BorderStyle.FixedSingle;
        designerCommandBar.Controls.Add(designerCommandFlow);
        designerCommandBar.Controls.Add(designerCloseHost);
        designerCloseHost.Dock = DockStyle.Left;
        designerCloseHost.Width = 28;
        designerCloseHost.Name = "designerCloseHost";
        designerCommandFlow.Name = "designerCommandFlow";
        designerCommandFlow.Dock = DockStyle.Fill;
        designerCommandFlow.AutoSize = false;
        designerCommandFlow.WrapContents = false;
        designerCommandFlow.FlowDirection = FlowDirection.RightToLeft;
        designerCommandFlow.RightToLeft = RightToLeft.No;
        designerCommandFlow.Padding = new Padding(2);
        designerCommandBar.MinimumSize = new Size(0, 30);
        designerCommandBar.Height = 30;
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
        standardCommandClose.Name = "standardCommandClose";
        standardCommandClose.Enabled = false;
        standardCommandClose.Visible = true;
        standardCommandClose.AccessibleName = "إغلاق";
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
        btnAdd.AutoSize = false;
        btnAdd.Dock = DockStyle.None;
        btnAdd.MinimumSize = Size.Empty;
        btnAdd.Size = new Size(26, 24);
        btnAdd.Margin = new Padding(1);
        btnAdd.FlatStyle = FlatStyle.Flat;
        btnAdd.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnAdd.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Add;
        btnAdd.Text = "";
        designerCommandFlow.Controls.Add(btnAdd);
        designerCommandBar.SetCommandRole(btnAdd, TransportERP.Desktop.CoreUI.DesignerCommandRole.Add);
        standardCommandEdit.AutoSize = false;
        standardCommandEdit.Dock = DockStyle.None;
        standardCommandEdit.MinimumSize = Size.Empty;
        standardCommandEdit.Size = new Size(26, 24);
        standardCommandEdit.Margin = new Padding(1);
        standardCommandEdit.FlatStyle = FlatStyle.Flat;
        standardCommandEdit.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandEdit.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Edit;
        standardCommandEdit.Text = "";
        designerCommandFlow.Controls.Add(standardCommandEdit);
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
        designerCommandFlow.Controls.Add(standardCommandDelete);
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
        designerCommandFlow.Controls.Add(standardCommandCancel);
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
        designerCommandFlow.Controls.Add(standardCommandView);
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
        designerCommandFlow.Controls.Add(standardCommandLast);
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
        designerCommandFlow.Controls.Add(standardCommandNext);
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
        designerCommandFlow.Controls.Add(standardCommandPrevious);
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
        designerCommandFlow.Controls.Add(standardCommandFirst);
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
        designerCommandFlow.Controls.Add(btnSave);
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
        designerCommandFlow.Controls.Add(standardCommandPrint);
        designerCommandBar.SetCommandRole(standardCommandPrint, TransportERP.Desktop.CoreUI.DesignerCommandRole.Print);
        standardCommandClose.AutoSize = false;
        standardCommandClose.Dock = DockStyle.None;
        standardCommandClose.MinimumSize = Size.Empty;
        standardCommandClose.Size = new Size(26, 24);
        standardCommandClose.Margin = new Padding(1);
        standardCommandClose.FlatStyle = FlatStyle.Flat;
        standardCommandClose.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandClose.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Close;
        standardCommandClose.Text = "";
        designerCloseHost.Controls.Add(standardCommandClose);
        standardCommandClose.Location = new Point(1, 1);
        designerCommandBar.SetCommandRole(standardCommandClose, TransportERP.Desktop.CoreUI.DesignerCommandRole.Close);
        standardCommandRefresh.AutoSize = false;
        standardCommandRefresh.Dock = DockStyle.None;
        standardCommandRefresh.MinimumSize = Size.Empty;
        standardCommandRefresh.Size = new Size(26, 24);
        standardCommandRefresh.Margin = new Padding(1);
        standardCommandRefresh.FlatStyle = FlatStyle.Flat;
        standardCommandRefresh.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandRefresh.Text = "تحديث";
        designerCommandFlow.Controls.Add(standardCommandRefresh);
        designerCommandBar.SetCommandRole(standardCommandRefresh, TransportERP.Desktop.CoreUI.DesignerCommandRole.Refresh);
        standardCommandImport.AutoSize = false;
        standardCommandImport.Dock = DockStyle.None;
        standardCommandImport.MinimumSize = Size.Empty;
        standardCommandImport.Size = new Size(26, 24);
        standardCommandImport.Margin = new Padding(1);
        standardCommandImport.FlatStyle = FlatStyle.Flat;
        standardCommandImport.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandImport.Text = "استيراد";
        designerCommandFlow.Controls.Add(standardCommandImport);
        designerCommandBar.SetCommandRole(standardCommandImport, TransportERP.Desktop.CoreUI.DesignerCommandRole.Import);
        standardCommandExport.AutoSize = false;
        standardCommandExport.Dock = DockStyle.None;
        standardCommandExport.MinimumSize = Size.Empty;
        standardCommandExport.Size = new Size(26, 24);
        standardCommandExport.Margin = new Padding(1);
        standardCommandExport.FlatStyle = FlatStyle.Flat;
        standardCommandExport.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandExport.Text = "تصدير";
        designerCommandFlow.Controls.Add(standardCommandExport);
        designerCommandBar.SetCommandRole(standardCommandExport, TransportERP.Desktop.CoreUI.DesignerCommandRole.Export);
        standardCommandHelp.AutoSize = false;
        standardCommandHelp.Dock = DockStyle.None;
        standardCommandHelp.MinimumSize = Size.Empty;
        standardCommandHelp.Size = new Size(26, 24);
        standardCommandHelp.Margin = new Padding(1);
        standardCommandHelp.FlatStyle = FlatStyle.Flat;
        standardCommandHelp.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandHelp.Text = "مساعدة";
        designerCommandFlow.Controls.Add(standardCommandHelp);
        designerCommandBar.SetCommandRole(standardCommandHelp, TransportERP.Desktop.CoreUI.DesignerCommandRole.Help);
        designerCommandFlow.Controls.Add(btnToolbarAddLogo);
        designerCommandFlow.Controls.Add(btnToolbarAddresses);
    
        // Shared audit presentation; original sources remain owned by this screen.
        standardAuditMetadata = new TransportERP.Desktop.CoreUI.AuditMetadataControl();
        standardAuditMetadata.Name = "standardAuditMetadata";
        standardAuditMetadata.Dock = DockStyle.Bottom;
        standardAuditMetadata.Margin = Padding.Empty;
        standardAuditMetadata.TabStop = false;
        standardAuditMetadata.Size = new Size(800, 64);
        auditInfoContainer.Visible = false;
        Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}

    private ToolTip toolTips;
    private TableLayoutPanel layout;
    private Label lblTitle;
    private Panel pnlToolbar;
    private TableLayoutPanel commandsLayout;
    private Panel addressEditorPanel;
    private TabControl tabsFields;
    private TableLayoutPanel auditInfoContainer;
    private TextBox txtCreatedByCode;
    private TextBox txtUpdatedByCode;
    private Label lblStatus;
    private Panel pnlMainFields;
    private GroupBox grpLogo;
    private PictureBox picLogo;
    private TableLayoutPanel fieldsMain;
    private TableLayoutPanel fieldsDetails;
    private TableLayoutPanel fieldsArchive;
    private TableLayoutPanel fieldsDocuments;
    private TableLayoutPanel fieldsHeaders;
    private TableLayoutPanel fieldsAddresses;
    private TabPage tabMain;
    private TabPage tabDetails;
    private TabPage tabArchive;
    private TabPage tabDocuments;
    private TabPage tabHeaders;
    private TabPage tabAddresses;
    private Button btnAdd;
    private Button btnSave;
    private Button btnToolbarAddLogo;
    private Button btnToolbarAddresses;
    private Button btnAddLogo;
    private Button btnSelectAddressesIdentifiers;
    private GroupBox grpbtnSelectAddressesIdentifiers;
    private ListBox lstbtnSelectAddressesIdentifiers;
    private Label lblAddressEmpty;
    private ComboBox cboCompany;
    private Label lblcboCompany;
    private TextBox txtCompanyLocal;
    private Label lbltxtCompanyLocal;
    private TextBox txtCompanyForeign;
    private Label lbltxtCompanyForeign;
    private CheckBox chkCompanyTaxGroup;
    private Label lblchkCompanyTaxGroup;
    private TextBox txtCompanyTaxCurrency;
    private Label lbltxtCompanyTaxCurrency;
    private TextBox txtCompanyTaxRegistration;
    private Label lbltxtCompanyTaxRegistration;
    private TextBox txtBranchNumber;
    private Label lbltxtBranchNumber;
    private TextBox txtYear;
    private Label lbltxtYear;
    private TextBox txtNameLocal;
    private Label lbltxtNameLocal;
    private TextBox txtNameForeign;
    private Label lbltxtNameForeign;
    private ComboBox cboGroup;
    private Label lblcboGroup;
    private CheckBox chkInvoice;
    private Label lblchkInvoice;
    private CheckBox chkQr;
    private Label lblchkQr;
    private CheckBox chkMain;
    private Label lblchkMain;
    private CheckBox chkLite;
    private Label lblchkLite;
    private TextBox txtConnection;
    private Label lbltxtConnection;
    private ComboBox cboConnection;
    private Label lblcboConnection;
    private TextBox txtReportHeader;
    private Label lbltxtReportHeader;
    private TextBox txtAddress;
    private Label lbltxtAddress;
    private TextBox txtSpecifications;
    private Label lbltxtSpecifications;
    private TextBox txtPan;
    private Label lbltxtPan;
    private TextBox txtTan;
    private Label lbltxtTan;
    private TextBox txtTaxNumber;
    private Label lbltxtTaxNumber;
    private TextBox txtSequence;
    private Label lbltxtSequence;
    private TextBox txtTaxArticle;
    private Label lbltxtTaxArticle;
    private TextBox txtStatistics;
    private Label lbltxtStatistics;
    private ComboBox cboCountry;
    private Label lblcboCountry;
    private ComboBox cboGovernorate;
    private Label lblcboGovernorate;
    private ComboBox cboCity;
    private Label lblcboCity;
    private CheckBox chkStopped;
    private Label lblchkStopped;
    private ComboBox cboDocumentType;
    private Label lblcboDocumentType;
    private TextBox txtDocumentNumber;
    private Label lbltxtDocumentNumber;
    private TextBox txtIssuedAt;
    private Label lbltxtIssuedAt;
    private TextBox txtIssuedPlace;
    private Label lbltxtIssuedPlace;
    private TextBox txtExpiresAt;
    private Label lbltxtExpiresAt;
    private TextBox txtRenewedAt;
    private Label lbltxtRenewedAt;
    private ComboBox cboHeaderType;
    private Label lblcboHeaderType;
    private TextBox txtAddressesIdentifiers;
    private Label lbltxtAddressesIdentifiers;
    private CheckedListBox lstDocumentsToBranch;
    private Label lbllstDocumentsToBranch;
    private CheckedListBox lstDocumentsToAdministration;
    private Label lbllstDocumentsToAdministration;
    private TextBox txtCreatedBy;
    private Label lbltxtCreatedBy;
    private TextBox txtUpdatedBy;
    private Label lbltxtUpdatedBy;
    private TextBox txtCreatedAt;
    private Label lbltxtCreatedAt;
    private TextBox txtUpdatedAt;
    private Label lbltxtUpdatedAt;
    private TextBox txtPrintCount;
    private Label lbltxtPrintCount;
    private TextBox txtUpdateCount;
    private Label lbltxtUpdateCount;
    private TextBox txtCreatedDevice;
    private Label lbltxtCreatedDevice;
    private TextBox txtUpdatedDevice;
    private Label lbltxtUpdatedDevice;
}





