#nullable disable
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace TransportERP.Desktop.Forms.SystemSettings.General;
partial class UcCurrencySetup
{
    private IContainer components;
    private TransportERP.Desktop.CoreUI.AuditMetadataControl standardAuditMetadata;
    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }
    private System.Windows.Forms.Panel rootWorkspaceViewport;

        private void InitializeComponent()
    {
        rootWorkspaceViewport = new System.Windows.Forms.Panel();
        components = new Container();
        standardAuditMetadata = new TransportERP.Desktop.CoreUI.AuditMetadataControl();
        ComponentResourceManager resources = new ComponentResourceManager(typeof(UcCurrencySetup));
        DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
        goldenSourceOnly = new Panel();
        cboInternationalSymbol = new ComboBox();
        lblcboInternationalSymbol = new Label();
        goldenConditionalTabs = new TabControl();
        goldenShell = new TableLayoutPanel();
        goldenHeading = new TableLayoutPanel();
        lblTitle = new Label();
        goldenUser = new Label();
        goldenPeriod = new Label();
        goldenDate = new Label();
        goldenMainFields = new TableLayoutPanel();
        goldenConversionFields = new TableLayoutPanel();
        goldenFactorFields = new TableLayoutPanel();
        goldenCurrencyFlags = new FlowLayoutPanel();
        goldenContent = new TableLayoutPanel();
        txtNumber = new TextBox();
        lbltxtNumber = new Label();
        txtNameLocal = new TextBox();
        lbltxtNameLocal = new Label();
        txtNameForeign = new TextBox();
        lbltxtNameForeign = new Label();
        cboSymbol = new ComboBox();
        lblcboSymbol = new Label();
        txtFractionLocal = new TextBox();
        lbltxtFractionLocal = new Label();
        txtFractionForeign = new TextBox();
        lbltxtFractionForeign = new Label();
        txtRate = new TextBox();
        lbltxtRate = new Label();
        txtMaximum = new TextBox();
        lbltxtMaximum = new Label();
        txtMinimum = new TextBox();
        lbltxtMinimum = new Label();
        txtFactor = new TextBox();
        lbltxtFactor = new Label();
        btnFactorLookup = new Button();
        factorOperator = new Label();
        txtDecimals = new TextBox();
        lbltxtDecimals = new Label();
        txtPosRate = new TextBox();
        lbltxtPosRate = new Label();
        chkLocal = new CheckBox();
        optLocal = new RadioButton();
        chkForeign = new CheckBox();
        optForeign = new RadioButton();
        chkStock = new CheckBox();
        btnPrintCurrencyControl = new Button();
        lblCurrencyControlPrint = new Label();
        standardCommandRefresh = new Button();
        standardCommandImport = new Button();
        standardCommandExport = new Button();
        standardCommandHelp = new Button();
        goldenToolbarHost = new TransportERP.Desktop.CoreUI.DesignerCommandBar();
        commandBarLayout = new TableLayoutPanel();
        btnClose = new Button();
        pnlToolbar = new FlowLayoutPanel();
        btnAdd = new Button();
        btnEdit = new Button();
        btnCancel = new Button();
        btnDelete = new Button();
        btnView = new Button();
        btnLast = new Button();
        btnNext = new Button();
        btnPrevious = new Button();
        btnFirst = new Button();
        btnSave = new Button();
        btnPrint = new Button();
        goldenBrand = new Label();
        referenceTool108 = new Label();
        referenceTool390 = new Label();
        referenceTool710 = new Label();
        tabsSetup = new TabControl();
        tabgridHistory = new TabPage();
        historyLayout = new TableLayoutPanel();
        gridHistory = new DataGridView();
        gridHistory_colCurrency = new DataGridViewTextBoxColumn();
        gridHistory_colRate = new DataGridViewTextBoxColumn();
        gridHistory_colMinimum = new DataGridViewTextBoxColumn();
        gridHistory_colMaximum = new DataGridViewTextBoxColumn();
        gridHistory_colUser = new DataGridViewTextBoxColumn();
        gridHistory_colChangedAt = new DataGridViewTextBoxColumn();
        tabgridLimits = new TabPage();
        goldenLimits = new TableLayoutPanel();
        cboLimitGroup = new ComboBox();
        lblcboLimitGroup = new Label();
        cboLimitBranch = new ComboBox();
        lblcboLimitBranch = new Label();
        txtLimitReceiptMaximum = new TextBox();
        lbltxtLimitReceiptMaximum = new Label();
        txtLimitReceiptMinimum = new TextBox();
        lbltxtLimitReceiptMinimum = new Label();
        txtLimitPaymentMaximum = new TextBox();
        lbltxtLimitPaymentMaximum = new Label();
        txtLimitPaymentMinimum = new TextBox();
        lbltxtLimitPaymentMinimum = new Label();
        txtLimitMinimum = new TextBox();
        lbltxtLimitMinimum = new Label();
        txtLimitMaximum = new TextBox();
        lbltxtLimitMaximum = new Label();
        txtLimitRate = new TextBox();
        lbltxtLimitRate = new Label();
        btnLoadLimits = new Button();
        btnUpdateLimits = new Button();
        gridLimits = new DataGridView();
        gridLimits_colUser = new DataGridViewComboBoxColumn();
        gridLimits_colUserName = new DataGridViewTextBoxColumn();
        gridLimits_colConversion = new DataGridViewTextBoxColumn();
        gridLimits_colMaximum = new DataGridViewTextBoxColumn();
        gridLimits_colMinimum = new DataGridViewTextBoxColumn();
        gridLimits_colPaymentMinimum = new DataGridViewTextBoxColumn();
        gridLimits_colPaymentMaximum = new DataGridViewTextBoxColumn();
        gridLimits_colReceiptMinimum = new DataGridViewTextBoxColumn();
        gridLimits_colReceiptMaximum = new DataGridViewTextBoxColumn();
        tabgridDenominations = new TabPage();
        denominationsLayout = new TableLayoutPanel();
        gridDenominations = new DataGridView();
        gridDenominations_colDenomination = new DataGridViewTextBoxColumn();
        goldenMetadata = new Panel();
        goldenAuditFields = new TableLayoutPanel();
        auditCreatedCount = new Label();
        lblAuditCaption1 = new Label();
        auditCreatedDevice = new Label();
        lblAuditCaption2 = new Label();
        auditCreatedDateTime = new Label();
        lblAuditCaption3 = new Label();
        auditCreatedBy = new Label();
        auditCreatedById = new Label();
        lblAuditCaption4 = new Label();
        auditUpdatedCount = new Label();
        lblAuditCaption5 = new Label();
        auditUpdatedDevice = new Label();
        lblAuditCaption6 = new Label();
        auditUpdatedDateTime = new Label();
        lblAuditCaption7 = new Label();
        auditUpdatedBy = new Label();
        auditUpdatedById = new Label();
        lblAuditCaption8 = new Label();
        goldenStatus = new Panel();
        lblStatus = new Label();
        goldenScreenCode = new Label();
        commandToolTip = new ToolTip(components);
        goldenSourceOnly.SuspendLayout();
        goldenShell.SuspendLayout();
        goldenHeading.SuspendLayout();
        goldenMainFields.SuspendLayout();
        goldenConversionFields.SuspendLayout();
        goldenFactorFields.SuspendLayout();
        goldenCurrencyFlags.SuspendLayout();
        goldenContent.SuspendLayout();
        goldenToolbarHost.SuspendLayout();
        commandBarLayout.SuspendLayout();
        pnlToolbar.SuspendLayout();
        tabsSetup.SuspendLayout();
        tabgridHistory.SuspendLayout();
        ((ISupportInitialize)gridHistory).BeginInit();
        tabgridLimits.SuspendLayout();
        goldenLimits.SuspendLayout();
        ((ISupportInitialize)gridLimits).BeginInit();
        tabgridDenominations.SuspendLayout();
        ((ISupportInitialize)gridDenominations).BeginInit();
        goldenMetadata.SuspendLayout();
        goldenAuditFields.SuspendLayout();
        auditCreatedBy.SuspendLayout();
        auditUpdatedBy.SuspendLayout();
        goldenStatus.SuspendLayout();
        SuspendLayout();
        // 
        // goldenSourceOnly
        // 
        goldenSourceOnly.AccessibleDescription = "رمز العملة العالمي موصوف نصياً؛ موضعه غير مثبت في الصور المرجعية.";
        goldenSourceOnly.BackColor = Color.FromArgb(244, 244, 244);
        goldenSourceOnly.Controls.Add(cboInternationalSymbol);
        // Hidden binding state retains FoundationUiSession's existing boolean field keys.
        // The visible options below are native RadioButtons, matching PDF pp.6-9.
        goldenSourceOnly.Controls.Add(chkLocal);
        goldenSourceOnly.Controls.Add(chkForeign);
        chkLocal.Name = "chkLocal";
        chkForeign.Name = "chkForeign";
        chkLocal.Enabled = false;
        chkForeign.Enabled = false;
        chkLocal.Visible = false;
        chkForeign.Visible = false;
        chkLocal.CheckedChanged += currencyType_CheckedChanged;
        chkForeign.CheckedChanged += currencyType_CheckedChanged;
        chkLocal.EnabledChanged += currencyType_EnabledChanged;
        chkForeign.EnabledChanged += currencyType_EnabledChanged;
        goldenSourceOnly.Controls.Add(lblcboInternationalSymbol);
        goldenSourceOnly.Controls.Add(goldenConditionalTabs);
        goldenSourceOnly.Font = new Font("Tahoma", 9F);
        goldenSourceOnly.ForeColor = Color.FromArgb(45, 45, 45);
        goldenSourceOnly.Location = new Point(0, 0);
        goldenSourceOnly.Margin = new Padding(0);
        goldenSourceOnly.Name = "goldenSourceOnly";
        goldenSourceOnly.RightToLeft = RightToLeft.Yes;
        goldenSourceOnly.Size = new Size(200, 100);
        goldenSourceOnly.TabIndex = 1;
        goldenSourceOnly.Visible = false;
        // 
        // cboInternationalSymbol
        // 
        cboInternationalSymbol.AccessibleDescription = "القائمة غير مرتبطة ببيانات بعد.";
        cboInternationalSymbol.AccessibleName = "رمز العملة العالمي";
        cboInternationalSymbol.BackColor = Color.FromArgb(255, 255, 255);
        cboInternationalSymbol.Dock = DockStyle.Fill;
        cboInternationalSymbol.DropDownStyle = ComboBoxStyle.DropDownList;
        cboInternationalSymbol.Enabled = false;
        cboInternationalSymbol.FlatStyle = FlatStyle.Flat;
        cboInternationalSymbol.Font = new Font("Tahoma", 9F);
        cboInternationalSymbol.ForeColor = Color.FromArgb(45, 45, 45);
        cboInternationalSymbol.FormattingEnabled = true;
        cboInternationalSymbol.Location = new Point(0, 0);
        cboInternationalSymbol.Margin = new Padding(0);
        cboInternationalSymbol.Name = "cboInternationalSymbol";
        cboInternationalSymbol.RightToLeft = RightToLeft.Yes;
        cboInternationalSymbol.Size = new Size(200, 21);
        cboInternationalSymbol.TabIndex = 4;
        // 
        // lblcboInternationalSymbol
        // 
        lblcboInternationalSymbol.AutoSize = true;
        lblcboInternationalSymbol.BackColor = Color.FromArgb(0, 255, 255, 255);
        lblcboInternationalSymbol.Dock = DockStyle.Fill;
        lblcboInternationalSymbol.Font = new Font("Tahoma", 9F);
        lblcboInternationalSymbol.ForeColor = Color.FromArgb(45, 45, 45);
        lblcboInternationalSymbol.Location = new Point(0, 0);
        lblcboInternationalSymbol.Margin = new Padding(0);
        lblcboInternationalSymbol.Name = "lblcboInternationalSymbol";
        lblcboInternationalSymbol.RightToLeft = RightToLeft.No;
        lblcboInternationalSymbol.Size = new Size(98, 14);
        lblcboInternationalSymbol.TabIndex = 4;
        lblcboInternationalSymbol.Text = "رمز العملة العالمي";
        lblcboInternationalSymbol.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // goldenConditionalTabs
        // 
        goldenConditionalTabs.Font = new Font("Tahoma", 9F);
        goldenConditionalTabs.ForeColor = Color.FromArgb(45, 45, 45);
        goldenConditionalTabs.Location = new Point(0, 0);
        goldenConditionalTabs.Margin = new Padding(0);
        goldenConditionalTabs.Name = "goldenConditionalTabs";
        goldenConditionalTabs.RightToLeft = RightToLeft.Yes;
        goldenConditionalTabs.SelectedIndex = 0;
        goldenConditionalTabs.Size = new Size(200, 100);
        goldenConditionalTabs.TabIndex = 2;
        goldenConditionalTabs.Visible = false;
        // 
        // goldenShell
        // 
        goldenShell.BackColor = Color.FromArgb(244, 244, 244);
        goldenShell.ColumnCount = 1;
        goldenShell.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        goldenShell.Controls.Add(goldenHeading, 0, 0);
        goldenShell.Controls.Add(goldenContent, 0, 2);
        goldenShell.Controls.Add(goldenToolbarHost, 0, 1);
        goldenShell.Controls.Add(tabsSetup, 0, 3);
        goldenShell.Controls.Add(goldenMetadata, 0, 4);
        goldenShell.Controls.Add(goldenStatus, 0, 5);
        goldenShell.Dock = DockStyle.Fill;
        goldenShell.Font = new Font("Tahoma", 9F);
        goldenShell.ForeColor = Color.FromArgb(45, 45, 45);
        goldenShell.Location = new Point(2, 2);
        goldenShell.Margin = new Padding(0);
        goldenShell.MinimumSize = new Size(896, 676);
        goldenShell.Name = "goldenShell";
        goldenShell.RightToLeft = RightToLeft.No;
        goldenShell.RowCount = 6;
        goldenShell.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
        goldenShell.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        goldenShell.RowStyles.Add(new RowStyle(SizeType.Absolute, 326F));
        goldenShell.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        goldenShell.RowStyles.Add(new RowStyle(SizeType.Absolute, 0F));
        goldenShell.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
        goldenShell.Size = new Size(1196, 896);
        goldenShell.TabIndex = 2;
        // 
        // goldenHeading
        // 
        goldenHeading.BackColor = Color.FromArgb(244, 244, 244);
        goldenHeading.BorderStyle = BorderStyle.FixedSingle;
        goldenHeading.Dock = DockStyle.Fill;
        goldenHeading.Font = new Font("Tahoma", 9F);
        goldenHeading.ForeColor = Color.FromArgb(45, 45, 45);
        goldenHeading.Location = new Point(0, 0);
        goldenHeading.Margin = new Padding(0);
        goldenHeading.Name = "goldenHeading";
        goldenHeading.RightToLeft = RightToLeft.No;
        goldenHeading.Size = new Size(1196, 18);
        goldenHeading.TabIndex = 0;
        // 
        // lblTitle
        // 
        lblTitle.AutoSize = true;
        lblTitle.BackColor = Color.FromArgb(0, 255, 255, 255);
        lblTitle.Dock = DockStyle.Fill;
        lblTitle.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblTitle.ForeColor = Color.FromArgb(45, 45, 45);
        lblTitle.Location = new Point(152, 0);
        lblTitle.Margin = new Padding(3);
        lblTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        lblTitle.Name = "lblTitle";
        lblTitle.RightToLeft = RightToLeft.No;
        lblTitle.Size = new Size(213, 14);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "تهيئة النظام · التهيئة · تهيئة العملات";
        lblTitle.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // goldenUser
        // 
        goldenUser.BackColor = Color.FromArgb(0, 255, 255, 255);
        goldenUser.BorderStyle = BorderStyle.FixedSingle;
        goldenUser.Dock = DockStyle.Fill;
        goldenUser.Font = new Font("Tahoma", 9F);
        goldenUser.ForeColor = Color.FromArgb(45, 45, 45);
        goldenUser.Location = new Point(1004, 0);
        goldenUser.Margin = new Padding(3);
        goldenUser.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        goldenUser.AutoSize = true;
        goldenUser.Name = "goldenUser";
        goldenUser.RightToLeft = RightToLeft.No;
        goldenUser.Size = new Size(190, 16);
        goldenUser.TabIndex = 1;
        goldenUser.Text = "المستخدم: —";
        goldenUser.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // goldenPeriod
        // 
        goldenPeriod.BackColor = Color.FromArgb(0, 255, 255, 255);
        goldenPeriod.BorderStyle = BorderStyle.FixedSingle;
        goldenPeriod.Dock = DockStyle.Fill;
        goldenPeriod.Font = new Font("Tahoma", 9F);
        goldenPeriod.ForeColor = Color.FromArgb(45, 45, 45);
        goldenPeriod.Location = new Point(82, 0);
        goldenPeriod.Margin = new Padding(3);
        goldenPeriod.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        goldenPeriod.AutoSize = true;
        goldenPeriod.Name = "goldenPeriod";
        goldenPeriod.RightToLeft = RightToLeft.No;
        goldenPeriod.Size = new Size(70, 16);
        goldenPeriod.TabIndex = 2;
        goldenPeriod.Text = "الفترة: —";
        goldenPeriod.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // goldenDate
        // 
        goldenDate.BackColor = Color.FromArgb(0, 255, 255, 255);
        goldenDate.BorderStyle = BorderStyle.FixedSingle;
        goldenDate.Dock = DockStyle.Fill;
        goldenDate.Font = new Font("Tahoma", 9F);
        goldenDate.ForeColor = Color.FromArgb(45, 45, 45);
        goldenDate.Location = new Point(0, 0);
        goldenDate.Margin = new Padding(3);
        goldenDate.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        goldenDate.AutoSize = true;
        goldenDate.Name = "goldenDate";
        goldenDate.RightToLeft = RightToLeft.No;
        goldenDate.Size = new Size(82, 16);
        goldenDate.TabIndex = 3;
        goldenDate.Text = "التاريخ: —";
        goldenDate.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // goldenContent
        // 
        goldenContent.BackColor = Color.FromArgb(244, 244, 244);
        goldenContent.Dock = DockStyle.Fill;
        goldenContent.Font = new Font("Tahoma", 9F);
        goldenContent.ForeColor = Color.FromArgb(45, 45, 45);
        goldenContent.Location = new Point(0, 44);
        goldenContent.Margin = new Padding(0);
        goldenContent.Padding = new Padding(12, 8, 12, 8);
        goldenContent.Name = "goldenContent";
        goldenContent.RightToLeft = RightToLeft.No;
        goldenContent.Size = new Size(1196, 190);
        goldenContent.TabIndex = 1;
        // 
        // txtNumber
        // 
        txtNumber.AccessibleName = "الرقم";
        txtNumber.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        txtNumber.BackColor = Color.FromArgb(255, 255, 225);
        txtNumber.BorderStyle = BorderStyle.FixedSingle;
        txtNumber.Enabled = false;
        txtNumber.Font = new Font("Tahoma", 9F);
        txtNumber.ForeColor = Color.FromArgb(45, 45, 45);
        txtNumber.Location = new Point(906, 14);
        txtNumber.Margin = new Padding(3);
        txtNumber.Dock = DockStyle.None;
        txtNumber.MinimumSize = new Size(60, 24);
        txtNumber.Name = "txtNumber";
        txtNumber.RightToLeft = RightToLeft.No;
        txtNumber.Size = new Size(120, 24);
        txtNumber.TabIndex = 0;
        txtNumber.TextAlign = HorizontalAlignment.Right;
        // 
        // lbltxtNumber
        // 
        lbltxtNumber.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lbltxtNumber.BackColor = Color.FromArgb(0, 255, 255, 255);
        lbltxtNumber.Font = new Font("Tahoma", 9F);
        lbltxtNumber.ForeColor = Color.FromArgb(45, 45, 45);
        lbltxtNumber.Location = new Point(1018, 14);
        lbltxtNumber.Margin = new Padding(3);
        lbltxtNumber.Dock = DockStyle.None;
        lbltxtNumber.AutoSize = true;
        lbltxtNumber.Name = "lbltxtNumber";
        lbltxtNumber.RightToLeft = RightToLeft.No;
        lbltxtNumber.Size = new Size(120, 24);
        lbltxtNumber.TabIndex = 0;
        lbltxtNumber.Text = "الرقم";
        lbltxtNumber.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtNameLocal
        // 
        txtNameLocal.AccessibleName = "اسم العملة";
        txtNameLocal.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        txtNameLocal.BackColor = Color.FromArgb(255, 255, 225);
        txtNameLocal.BorderStyle = BorderStyle.FixedSingle;
        txtNameLocal.Enabled = false;
        txtNameLocal.Font = new Font("Tahoma", 9F);
        txtNameLocal.ForeColor = Color.FromArgb(45, 45, 45);
        txtNameLocal.Location = new Point(866, 30);
        txtNameLocal.Margin = new Padding(3);
        txtNameLocal.Dock = DockStyle.Fill;
        txtNameLocal.MinimumSize = new Size(60, 24);
        txtNameLocal.Name = "txtNameLocal";
        txtNameLocal.RightToLeft = RightToLeft.No;
        txtNameLocal.Size = new Size(160, 24);
        txtNameLocal.TabIndex = 1;
        txtNameLocal.TextAlign = HorizontalAlignment.Right;
        // 
        // lbltxtNameLocal
        // 
        lbltxtNameLocal.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        lbltxtNameLocal.BackColor = Color.FromArgb(0, 255, 255, 255);
        lbltxtNameLocal.Font = new Font("Tahoma", 9F);
        lbltxtNameLocal.ForeColor = Color.FromArgb(45, 45, 45);
        lbltxtNameLocal.Location = new Point(1018, 30);
        lbltxtNameLocal.Margin = new Padding(3);
        lbltxtNameLocal.Dock = DockStyle.Fill;
        lbltxtNameLocal.AutoSize = true;
        lbltxtNameLocal.Name = "lbltxtNameLocal";
        lbltxtNameLocal.RightToLeft = RightToLeft.No;
        lbltxtNameLocal.Size = new Size(92, 14);
        lbltxtNameLocal.TabIndex = 1;
        lbltxtNameLocal.Text = "اسم العملة";
        lbltxtNameLocal.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtNameForeign
        // 
        txtNameForeign.AccessibleName = "الاسم الأجنبي";
        txtNameForeign.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        txtNameForeign.BackColor = Color.FromArgb(255, 255, 255);
        txtNameForeign.BorderStyle = BorderStyle.FixedSingle;
        txtNameForeign.Enabled = false;
        txtNameForeign.Font = new Font("Tahoma", 9F);
        txtNameForeign.ForeColor = Color.FromArgb(45, 45, 45);
        txtNameForeign.Location = new Point(866, 46);
        txtNameForeign.Margin = new Padding(3);
        txtNameForeign.Dock = DockStyle.Fill;
        txtNameForeign.MinimumSize = new Size(60, 24);
        txtNameForeign.Name = "txtNameForeign";
        txtNameForeign.RightToLeft = RightToLeft.No;
        txtNameForeign.Size = new Size(160, 24);
        txtNameForeign.TabIndex = 2;
        // 
        // lbltxtNameForeign
        // 
        lbltxtNameForeign.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        lbltxtNameForeign.BackColor = Color.FromArgb(0, 255, 255, 255);
        lbltxtNameForeign.Font = new Font("Tahoma", 9F);
        lbltxtNameForeign.ForeColor = Color.FromArgb(45, 45, 45);
        lbltxtNameForeign.Location = new Point(1018, 46);
        lbltxtNameForeign.Margin = new Padding(3);
        lbltxtNameForeign.Dock = DockStyle.Fill;
        lbltxtNameForeign.AutoSize = true;
        lbltxtNameForeign.Name = "lbltxtNameForeign";
        lbltxtNameForeign.RightToLeft = RightToLeft.No;
        lbltxtNameForeign.Size = new Size(92, 14);
        lbltxtNameForeign.TabIndex = 2;
        lbltxtNameForeign.Text = "الاسم الأجنبي";
        lbltxtNameForeign.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cboSymbol
        // 
        cboSymbol.AccessibleDescription = "القائمة غير مرتبطة ببيانات بعد.";
        cboSymbol.AccessibleName = "رمز العملة";
        cboSymbol.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        cboSymbol.BackColor = Color.FromArgb(255, 255, 225);
        cboSymbol.DropDownStyle = ComboBoxStyle.DropDownList;
        cboSymbol.Enabled = false;
        cboSymbol.FlatStyle = FlatStyle.Flat;
        cboSymbol.Font = new Font("Tahoma", 9F);
        cboSymbol.ForeColor = Color.FromArgb(45, 45, 45);
        cboSymbol.FormattingEnabled = true;
        cboSymbol.Location = new Point(906, 62);
        cboSymbol.Margin = new Padding(3);
        cboSymbol.Dock = DockStyle.None;
        cboSymbol.MinimumSize = new Size(60, 24);
        cboSymbol.Name = "cboSymbol";
        cboSymbol.RightToLeft = RightToLeft.No;
        cboSymbol.Size = new Size(120, 24);
        cboSymbol.TabIndex = 3;
        // 
        // lblcboSymbol
        // 
        lblcboSymbol.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblcboSymbol.BackColor = Color.FromArgb(0, 255, 255, 255);
        lblcboSymbol.Font = new Font("Tahoma", 9F);
        lblcboSymbol.ForeColor = Color.FromArgb(45, 45, 45);
        lblcboSymbol.Location = new Point(1018, 62);
        lblcboSymbol.Margin = new Padding(3);
        lblcboSymbol.Dock = DockStyle.None;
        lblcboSymbol.AutoSize = true;
        lblcboSymbol.Name = "lblcboSymbol";
        lblcboSymbol.RightToLeft = RightToLeft.No;
        lblcboSymbol.Size = new Size(120, 24);
        lblcboSymbol.TabIndex = 3;
        lblcboSymbol.Text = "رمز العملة";
        lblcboSymbol.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtFractionLocal
        // 
        txtFractionLocal.AccessibleName = "الفكة";
        txtFractionLocal.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        txtFractionLocal.BackColor = Color.FromArgb(255, 255, 255);
        txtFractionLocal.BorderStyle = BorderStyle.FixedSingle;
        txtFractionLocal.Enabled = false;
        txtFractionLocal.Font = new Font("Tahoma", 9F);
        txtFractionLocal.ForeColor = Color.FromArgb(45, 45, 45);
        txtFractionLocal.Location = new Point(906, 78);
        txtFractionLocal.Margin = new Padding(3);
        txtFractionLocal.Dock = DockStyle.None;
        txtFractionLocal.MinimumSize = new Size(60, 24);
        txtFractionLocal.Name = "txtFractionLocal";
        txtFractionLocal.RightToLeft = RightToLeft.No;
        txtFractionLocal.Size = new Size(120, 24);
        txtFractionLocal.TabIndex = 5;
        txtFractionLocal.TextAlign = HorizontalAlignment.Right;
        // 
        // lbltxtFractionLocal
        // 
        lbltxtFractionLocal.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lbltxtFractionLocal.BackColor = Color.FromArgb(0, 255, 255, 255);
        lbltxtFractionLocal.Font = new Font("Tahoma", 9F);
        lbltxtFractionLocal.ForeColor = Color.FromArgb(45, 45, 45);
        lbltxtFractionLocal.Location = new Point(1018, 78);
        lbltxtFractionLocal.Margin = new Padding(3);
        lbltxtFractionLocal.Dock = DockStyle.None;
        lbltxtFractionLocal.AutoSize = true;
        lbltxtFractionLocal.Name = "lbltxtFractionLocal";
        lbltxtFractionLocal.RightToLeft = RightToLeft.No;
        lbltxtFractionLocal.Size = new Size(120, 24);
        lbltxtFractionLocal.TabIndex = 5;
        lbltxtFractionLocal.Text = "الفكة";
        lbltxtFractionLocal.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtFractionForeign
        // 
        txtFractionForeign.AccessibleName = "الاسم الأجنبي";
        txtFractionForeign.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        txtFractionForeign.BackColor = Color.FromArgb(255, 255, 255);
        txtFractionForeign.BorderStyle = BorderStyle.FixedSingle;
        txtFractionForeign.Enabled = false;
        txtFractionForeign.Font = new Font("Tahoma", 9F);
        txtFractionForeign.ForeColor = Color.FromArgb(45, 45, 45);
        txtFractionForeign.Location = new Point(906, 94);
        txtFractionForeign.Margin = new Padding(3);
        txtFractionForeign.Dock = DockStyle.None;
        txtFractionForeign.MinimumSize = new Size(60, 24);
        txtFractionForeign.Name = "txtFractionForeign";
        txtFractionForeign.RightToLeft = RightToLeft.No;
        txtFractionForeign.Size = new Size(120, 24);
        txtFractionForeign.TabIndex = 6;
        // 
        // lbltxtFractionForeign
        // 
        lbltxtFractionForeign.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lbltxtFractionForeign.BackColor = Color.FromArgb(0, 255, 255, 255);
        lbltxtFractionForeign.Font = new Font("Tahoma", 9F);
        lbltxtFractionForeign.ForeColor = Color.FromArgb(45, 45, 45);
        lbltxtFractionForeign.Location = new Point(1018, 94);
        lbltxtFractionForeign.Margin = new Padding(3);
        lbltxtFractionForeign.Dock = DockStyle.None;
        lbltxtFractionForeign.AutoSize = true;
        lbltxtFractionForeign.Name = "lbltxtFractionForeign";
        lbltxtFractionForeign.RightToLeft = RightToLeft.No;
        lbltxtFractionForeign.Size = new Size(120, 24);
        lbltxtFractionForeign.TabIndex = 6;
        lbltxtFractionForeign.Text = "الاسم الأجنبي";
        lbltxtFractionForeign.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtRate
        // 
        txtRate.AccessibleName = "سعر التحويل";
        txtRate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        txtRate.BackColor = Color.White;
        txtRate.BorderStyle = BorderStyle.FixedSingle;
        txtRate.Enabled = false;
        txtRate.Font = new Font("Tahoma", 9F);
        txtRate.ForeColor = Color.FromArgb(45, 45, 45);
        txtRate.Location = new Point(906, 142);
        txtRate.Margin = new Padding(3);
        txtRate.Dock = DockStyle.None;
        txtRate.MinimumSize = new Size(60, 24);
        txtRate.Name = "txtRate";
        txtRate.RightToLeft = RightToLeft.No;
        txtRate.Size = new Size(120, 24);
        txtRate.TabIndex = 10;
        txtRate.TextAlign = HorizontalAlignment.Right;
        // 
        // lbltxtRate
        // 
        lbltxtRate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lbltxtRate.BackColor = Color.FromArgb(0, 255, 255, 255);
        lbltxtRate.Font = new Font("Tahoma", 9F);
        lbltxtRate.ForeColor = Color.FromArgb(45, 45, 45);
        lbltxtRate.Location = new Point(1018, 142);
        lbltxtRate.Margin = new Padding(3);
        lbltxtRate.Dock = DockStyle.None;
        lbltxtRate.AutoSize = true;
        lbltxtRate.Name = "lbltxtRate";
        lbltxtRate.RightToLeft = RightToLeft.No;
        lbltxtRate.Size = new Size(120, 24);
        lbltxtRate.TabIndex = 10;
        lbltxtRate.Text = "سعر التحويل";
        lbltxtRate.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtMaximum
        // 
        txtMaximum.AccessibleName = "أعلى سعر تحويل";
        txtMaximum.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        txtMaximum.BackColor = Color.FromArgb(255, 255, 255);
        txtMaximum.BorderStyle = BorderStyle.FixedSingle;
        txtMaximum.Enabled = false;
        txtMaximum.Font = new Font("Tahoma", 9F);
        txtMaximum.ForeColor = Color.FromArgb(45, 45, 45);
        txtMaximum.Location = new Point(906, 158);
        txtMaximum.Margin = new Padding(3);
        txtMaximum.Dock = DockStyle.None;
        txtMaximum.MinimumSize = new Size(60, 24);
        txtMaximum.Name = "txtMaximum";
        txtMaximum.RightToLeft = RightToLeft.No;
        txtMaximum.Size = new Size(120, 24);
        txtMaximum.TabIndex = 11;
        txtMaximum.TextAlign = HorizontalAlignment.Right;
        // 
        // lbltxtMaximum
        // 
        lbltxtMaximum.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lbltxtMaximum.BackColor = Color.FromArgb(0, 255, 255, 255);
        lbltxtMaximum.Font = new Font("Tahoma", 9F);
        lbltxtMaximum.ForeColor = Color.FromArgb(45, 45, 45);
        lbltxtMaximum.Location = new Point(1018, 158);
        lbltxtMaximum.Margin = new Padding(3);
        lbltxtMaximum.Dock = DockStyle.None;
        lbltxtMaximum.AutoSize = true;
        lbltxtMaximum.Name = "lbltxtMaximum";
        lbltxtMaximum.RightToLeft = RightToLeft.No;
        lbltxtMaximum.Size = new Size(120, 24);
        lbltxtMaximum.TabIndex = 11;
        lbltxtMaximum.Text = "أعلى سعر تحويل";
        lbltxtMaximum.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtMinimum
        // 
        txtMinimum.AccessibleName = "أدنى سعر تحويل";
        txtMinimum.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        txtMinimum.BackColor = Color.FromArgb(255, 255, 255);
        txtMinimum.BorderStyle = BorderStyle.FixedSingle;
        txtMinimum.Enabled = false;
        txtMinimum.Font = new Font("Tahoma", 9F);
        txtMinimum.ForeColor = Color.FromArgb(45, 45, 45);
        txtMinimum.Location = new Point(906, 174);
        txtMinimum.Margin = new Padding(3);
        txtMinimum.Dock = DockStyle.None;
        txtMinimum.MinimumSize = new Size(60, 24);
        txtMinimum.Name = "txtMinimum";
        txtMinimum.RightToLeft = RightToLeft.No;
        txtMinimum.Size = new Size(120, 24);
        txtMinimum.TabIndex = 12;
        txtMinimum.TextAlign = HorizontalAlignment.Right;
        // 
        // lbltxtMinimum
        // 
        lbltxtMinimum.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lbltxtMinimum.BackColor = Color.FromArgb(0, 255, 255, 255);
        lbltxtMinimum.Font = new Font("Tahoma", 9F);
        lbltxtMinimum.ForeColor = Color.FromArgb(45, 45, 45);
        lbltxtMinimum.Location = new Point(1018, 174);
        lbltxtMinimum.Margin = new Padding(3);
        lbltxtMinimum.Dock = DockStyle.None;
        lbltxtMinimum.AutoSize = true;
        lbltxtMinimum.Name = "lbltxtMinimum";
        lbltxtMinimum.RightToLeft = RightToLeft.No;
        lbltxtMinimum.Size = new Size(120, 24);
        lbltxtMinimum.TabIndex = 12;
        lbltxtMinimum.Text = "أدنى سعر تحويل";
        lbltxtMinimum.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtFactor
        // 
        txtFactor.AccessibleName = "المعامل";
        txtFactor.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        txtFactor.BackColor = Color.FromArgb(255, 255, 255);
        txtFactor.BorderStyle = BorderStyle.FixedSingle;
        txtFactor.Enabled = false;
        txtFactor.Font = new Font("Tahoma", 9F);
        txtFactor.ForeColor = Color.FromArgb(45, 45, 45);
        txtFactor.Location = new Point(595, 110);
        txtFactor.Margin = new Padding(0, 3, 1, 3);
        txtFactor.Dock = DockStyle.Fill;
        txtFactor.MinimumSize = new Size(0, 24);
        txtFactor.Name = "txtFactor";
        txtFactor.RightToLeft = RightToLeft.No;
        txtFactor.Size = new Size(160, 24);
        txtFactor.TabIndex = 14;
        txtFactor.TextAlign = HorizontalAlignment.Right;
        // 
        // lbltxtFactor
        // 
        lbltxtFactor.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        lbltxtFactor.BackColor = Color.FromArgb(0, 255, 255, 255);
        lbltxtFactor.Font = new Font("Tahoma", 9F);
        lbltxtFactor.ForeColor = Color.FromArgb(45, 45, 45);
        lbltxtFactor.Location = new Point(742, 110);
        lbltxtFactor.Margin = new Padding(3);
        lbltxtFactor.Dock = DockStyle.Fill;
        lbltxtFactor.AutoSize = true;
        lbltxtFactor.Name = "lbltxtFactor";
        lbltxtFactor.RightToLeft = RightToLeft.No;
        lbltxtFactor.Size = new Size(108, 14);
        lbltxtFactor.TabIndex = 14;
        lbltxtFactor.Text = "المعامل";
        lbltxtFactor.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // btnFactorLookup
        // 
        btnFactorLookup.AccessibleName = "اختيار المعامل — غير مرتبط";
        btnFactorLookup.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        btnFactorLookup.BackColor = Color.FromArgb(239, 239, 239);
        btnFactorLookup.Enabled = false;
        btnFactorLookup.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnFactorLookup.FlatAppearance.MouseDownBackColor = Color.FromArgb(207, 220, 227);
        btnFactorLookup.FlatAppearance.MouseOverBackColor = Color.FromArgb(225, 235, 240);
        btnFactorLookup.FlatStyle = FlatStyle.Flat;
        btnFactorLookup.Font = new Font("Tahoma", 9F);
        btnFactorLookup.ForeColor = Color.FromArgb(45, 45, 45);
        btnFactorLookup.Location = new Point(706, 110);
        btnFactorLookup.Margin = new Padding(0, 3, 1, 3);
        btnFactorLookup.Dock = DockStyle.Fill;
        btnFactorLookup.MinimumSize = new Size(0, 24);
        btnFactorLookup.Name = "btnFactorLookup";
        btnFactorLookup.RightToLeft = RightToLeft.No;
        btnFactorLookup.Size = new Size(160, 24);
        btnFactorLookup.TabIndex = 15;
        btnFactorLookup.Text = "▾";
        commandToolTip.SetToolTip(btnFactorLookup, "اختيار المعامل — غير مرتبط");
        btnFactorLookup.UseVisualStyleBackColor = false;
        // 
        // factorOperator
        // 
        factorOperator.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        factorOperator.BackColor = Color.FromArgb(0, 255, 255, 255);
        factorOperator.BorderStyle = BorderStyle.FixedSingle;
        factorOperator.Font = new Font("Tahoma", 9F);
        factorOperator.ForeColor = Color.FromArgb(45, 45, 45);
        factorOperator.Location = new Point(720, 110);
        factorOperator.Margin = new Padding(0, 3, 1, 3);
        factorOperator.Dock = DockStyle.Fill;
        factorOperator.AutoSize = true;
        factorOperator.MinimumSize = new Size(0, 24);
        factorOperator.Name = "factorOperator";
        factorOperator.RightToLeft = RightToLeft.No;
        factorOperator.Size = new Size(20, 14);
        factorOperator.TabIndex = 16;
        factorOperator.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtDecimals
        // 
        txtDecimals.AccessibleName = "عدد خانات الكسور";
        txtDecimals.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        txtDecimals.BackColor = Color.FromArgb(255, 255, 225);
        txtDecimals.BorderStyle = BorderStyle.FixedSingle;
        txtDecimals.Enabled = false;
        txtDecimals.Font = new Font("Tahoma", 9F);
        txtDecimals.ForeColor = Color.FromArgb(45, 45, 45);
        txtDecimals.Location = new Point(706, 126);
        txtDecimals.Margin = new Padding(3);
        txtDecimals.Dock = DockStyle.None;
        txtDecimals.MinimumSize = new Size(60, 24);
        txtDecimals.Name = "txtDecimals";
        txtDecimals.RightToLeft = RightToLeft.No;
        txtDecimals.Size = new Size(60, 24);
        txtDecimals.TabIndex = 15;
        txtDecimals.TextAlign = HorizontalAlignment.Right;
        // 
        // lbltxtDecimals
        // 
        lbltxtDecimals.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lbltxtDecimals.BackColor = Color.FromArgb(0, 255, 255, 255);
        lbltxtDecimals.Font = new Font("Tahoma", 9F);
        lbltxtDecimals.ForeColor = Color.FromArgb(45, 45, 45);
        lbltxtDecimals.Location = new Point(742, 126);
        lbltxtDecimals.Margin = new Padding(3);
        lbltxtDecimals.Dock = DockStyle.None;
        lbltxtDecimals.AutoSize = true;
        lbltxtDecimals.Name = "lbltxtDecimals";
        lbltxtDecimals.RightToLeft = RightToLeft.No;
        lbltxtDecimals.Size = new Size(60, 24);
        lbltxtDecimals.TabIndex = 15;
        lbltxtDecimals.Text = "عدد خانات الكسور";
        lbltxtDecimals.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtPosRate
        // 
        txtPosRate.AccessibleName = "سعر تحويل نقاط البيع";
        txtPosRate.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        txtPosRate.BackColor = Color.FromArgb(255, 255, 255);
        txtPosRate.BorderStyle = BorderStyle.FixedSingle;
        txtPosRate.Enabled = false;
        txtPosRate.Font = new Font("Tahoma", 9F);
        txtPosRate.ForeColor = Color.FromArgb(45, 45, 45);
        txtPosRate.Location = new Point(595, 142);
        txtPosRate.Margin = new Padding(3);
        txtPosRate.Dock = DockStyle.Fill;
        txtPosRate.MinimumSize = new Size(60, 24);
        txtPosRate.Name = "txtPosRate";
        txtPosRate.RightToLeft = RightToLeft.No;
        txtPosRate.Size = new Size(160, 24);
        txtPosRate.TabIndex = 13;
        txtPosRate.TextAlign = HorizontalAlignment.Right;
        // 
        // lbltxtPosRate
        // 
        lbltxtPosRate.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        lbltxtPosRate.BackColor = Color.FromArgb(0, 255, 255, 255);
        lbltxtPosRate.Font = new Font("Tahoma", 9F);
        lbltxtPosRate.ForeColor = Color.FromArgb(45, 45, 45);
        lbltxtPosRate.Location = new Point(742, 142);
        lbltxtPosRate.Margin = new Padding(3);
        lbltxtPosRate.Dock = DockStyle.Fill;
        lbltxtPosRate.AutoSize = true;
        lbltxtPosRate.Name = "lbltxtPosRate";
        lbltxtPosRate.RightToLeft = RightToLeft.No;
        lbltxtPosRate.Size = new Size(108, 14);
        lbltxtPosRate.TabIndex = 13;
        lbltxtPosRate.Text = "سعر تحويل نقاط البيع";
        lbltxtPosRate.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // chkLocal
        // 

        // 
        // chkForeign
        // 

        // 
        // chkStock
        // 
        chkStock.AccessibleName = "عملة المخزون";
        chkStock.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        chkStock.BackColor = Color.FromArgb(244, 244, 244);
        chkStock.Enabled = false;
        chkStock.Font = new Font("Tahoma", 9F);
        chkStock.ForeColor = Color.FromArgb(45, 45, 45);
        chkStock.Location = new Point(931, 127);
        chkStock.Margin = new Padding(5, 4, 5, 0);
        chkStock.AutoSize = true;
        chkStock.Name = "chkStock";
        chkStock.RightToLeft = RightToLeft.Yes;
        chkStock.Size = new Size(85, 14);
        chkStock.TabIndex = 9;
        chkStock.Text = "عملة المخزون";
        chkStock.UseVisualStyleBackColor = false;
        // 
        // btnPrintCurrencyControl
        // 
        btnPrintCurrencyControl.AccessibleName = "طباعة رقابة العملات";
        btnPrintCurrencyControl.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        btnPrintCurrencyControl.BackColor = Color.FromArgb(255, 255, 255);
        btnPrintCurrencyControl.Enabled = false;
        btnPrintCurrencyControl.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnPrintCurrencyControl.FlatAppearance.MouseDownBackColor = Color.FromArgb(207, 220, 227);
        btnPrintCurrencyControl.FlatAppearance.MouseOverBackColor = Color.FromArgb(225, 235, 240);
        btnPrintCurrencyControl.FlatStyle = FlatStyle.Flat;
        btnPrintCurrencyControl.Font = new Font("Tahoma", 9F);
        btnPrintCurrencyControl.ForeColor = Color.FromArgb(45, 45, 45);
        btnPrintCurrencyControl.Location = new Point(579, 14);
        btnPrintCurrencyControl.Margin = new Padding(3);
        btnPrintCurrencyControl.Dock = DockStyle.Fill;
        btnPrintCurrencyControl.MinimumSize = new Size(60, 24);
        btnPrintCurrencyControl.Name = "btnPrintCurrencyControl";
        btnPrintCurrencyControl.RightToLeft = RightToLeft.No;
        btnPrintCurrencyControl.Size = new Size(160, 24);
        btnPrintCurrencyControl.TabIndex = 2;
        btnPrintCurrencyControl.Text = "▾";
        btnPrintCurrencyControl.TextAlign = ContentAlignment.MiddleLeft;
        commandToolTip.SetToolTip(btnPrintCurrencyControl, "طباعة رقابة العملات");
        btnPrintCurrencyControl.UseVisualStyleBackColor = false;
        // 
        // lblCurrencyControlPrint
        // 
        lblCurrencyControlPrint.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        lblCurrencyControlPrint.BackColor = Color.FromArgb(0, 255, 255, 255);
        lblCurrencyControlPrint.Font = new Font("Tahoma", 9F);
        lblCurrencyControlPrint.ForeColor = Color.FromArgb(45, 45, 45);
        lblCurrencyControlPrint.Location = new Point(742, 14);
        lblCurrencyControlPrint.Margin = new Padding(3);
        lblCurrencyControlPrint.Dock = DockStyle.Fill;
        lblCurrencyControlPrint.AutoSize = true;
        lblCurrencyControlPrint.Name = "lblCurrencyControlPrint";
        lblCurrencyControlPrint.RightToLeft = RightToLeft.No;
        lblCurrencyControlPrint.Size = new Size(110, 14);
        lblCurrencyControlPrint.TabIndex = 17;
        lblCurrencyControlPrint.Text = "طباعة رقابة العملات";
        lblCurrencyControlPrint.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // goldenToolbarHost
        // 
        goldenToolbarHost.BackColor = Color.FromArgb(239, 239, 239);
        goldenToolbarHost.BorderStyle = BorderStyle.FixedSingle;
        goldenToolbarHost.Dock = DockStyle.Fill;
        goldenToolbarHost.Font = new Font("Tahoma", 9F);
        goldenToolbarHost.ForeColor = Color.FromArgb(45, 45, 45);
        goldenToolbarHost.Location = new Point(0, 18);
        goldenToolbarHost.Margin = new Padding(0);
        goldenToolbarHost.Name = "commandBarContainer";
        goldenToolbarHost.RightToLeft = RightToLeft.Yes;
        goldenToolbarHost.MinimumSize = new Size(600, 30);
        goldenToolbarHost.Size = new Size(1196, 26);
        goldenToolbarHost.TabIndex = 2;
        // 
        // btnClose
        // 
        btnClose.AccessibleName = "إغلاق";
        btnClose.BackColor = Color.FromArgb(239, 239, 239);
        btnClose.Enabled = false;
        btnClose.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnClose.FlatAppearance.MouseDownBackColor = Color.FromArgb(207, 220, 227);
        btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(225, 235, 240);
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Tahoma", 9F);
        btnClose.ForeColor = Color.FromArgb(45, 45, 45);
        btnClose.Image = (Image)resources.GetObject("btnClose.Image");
        btnClose.Location = new Point(226, 1);
        btnClose.Margin = new Padding(1);
        btnClose.Anchor = AnchorStyles.None;
        btnClose.Name = "btnClose";
        btnClose.RightToLeft = RightToLeft.No;
        btnClose.Size = new Size(26, 24);
        btnClose.TabIndex = 11;
        commandToolTip.SetToolTip(btnClose, "إغلاق");
        btnClose.UseVisualStyleBackColor = false;
        // 
        // pnlToolbar
        // 
        pnlToolbar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        pnlToolbar.BackColor = Color.FromArgb(239, 239, 239);
        pnlToolbar.Controls.Add(referenceTool710);
        pnlToolbar.Controls.Add(btnAdd);
        pnlToolbar.Controls.Add(btnEdit);
        pnlToolbar.Controls.Add(btnDelete);
        pnlToolbar.Controls.Add(btnCancel);
        pnlToolbar.Controls.Add(btnView);
        pnlToolbar.Controls.Add(btnLast);
        pnlToolbar.Controls.Add(btnNext);
        pnlToolbar.Controls.Add(btnPrevious);
        pnlToolbar.Controls.Add(btnFirst);
        pnlToolbar.Controls.Add(btnSave);
        pnlToolbar.Controls.Add(btnPrint);
        pnlToolbar.Controls.Add(referenceTool390);
        pnlToolbar.FlowDirection = FlowDirection.RightToLeft;
        pnlToolbar.Font = new Font("Tahoma", 9F);
        pnlToolbar.ForeColor = Color.FromArgb(45, 45, 45);
        pnlToolbar.Location = new Point(824, 1);
        pnlToolbar.Margin = new Padding(0);
        pnlToolbar.Dock = DockStyle.Fill;
        pnlToolbar.AutoSize = false;
        pnlToolbar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        pnlToolbar.Name = "pnlToolbar";
        pnlToolbar.RightToLeft = RightToLeft.No;
        pnlToolbar.Size = new Size(312, 22);
        pnlToolbar.TabIndex = 0;
        pnlToolbar.WrapContents = false;
        // 
        // btnAdd
        // 
        btnAdd.AccessibleName = "إضافة";
        btnAdd.BackColor = Color.FromArgb(239, 239, 239);
        btnAdd.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnAdd.FlatAppearance.MouseDownBackColor = Color.FromArgb(207, 220, 227);
        btnAdd.FlatAppearance.MouseOverBackColor = Color.FromArgb(225, 235, 240);
        btnAdd.FlatStyle = FlatStyle.Flat;
        btnAdd.Font = new Font("Tahoma", 9F);
        btnAdd.ForeColor = Color.FromArgb(45, 45, 45);
        btnAdd.Image = (Image)resources.GetObject("btnAdd.Image");
        btnAdd.Location = new Point(288, 0);
        btnAdd.Margin = new Padding(1);
        btnAdd.Name = "btnAdd";
        btnAdd.RightToLeft = RightToLeft.No;
        btnAdd.Size = new Size(26, 24);
        btnAdd.TabIndex = 0;
        commandToolTip.SetToolTip(btnAdd, "إضافة");
        btnAdd.UseVisualStyleBackColor = false;
        btnAdd.Click += btnAdd_Click;
        // 
        // btnEdit
        // 
        btnEdit.AccessibleName = "تعديل";
        btnEdit.BackColor = Color.FromArgb(239, 239, 239);
        btnEdit.Enabled = false;
        btnEdit.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnEdit.FlatAppearance.MouseDownBackColor = Color.FromArgb(207, 220, 227);
        btnEdit.FlatAppearance.MouseOverBackColor = Color.FromArgb(225, 235, 240);
        btnEdit.FlatStyle = FlatStyle.Flat;
        btnEdit.Font = new Font("Tahoma", 9F);
        btnEdit.ForeColor = Color.FromArgb(45, 45, 45);
        btnEdit.Image = (Image)resources.GetObject("btnEdit.Image");
        btnEdit.Location = new Point(264, 0);
        btnEdit.Margin = new Padding(1);
        btnEdit.Name = "btnEdit";
        btnEdit.RightToLeft = RightToLeft.No;
        btnEdit.Size = new Size(26, 24);
        btnEdit.TabIndex = 2;
        commandToolTip.SetToolTip(btnEdit, "تعديل");
        btnEdit.UseVisualStyleBackColor = false;
        // 
        // btnCancel
        // 
        btnCancel.AccessibleName = "إلغاء";
        btnCancel.BackColor = Color.FromArgb(239, 239, 239);
        btnCancel.Enabled = false;
        btnCancel.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnCancel.FlatAppearance.MouseDownBackColor = Color.FromArgb(207, 220, 227);
        btnCancel.FlatAppearance.MouseOverBackColor = Color.FromArgb(225, 235, 240);
        btnCancel.FlatStyle = FlatStyle.Flat;
        btnCancel.Font = new Font("Tahoma", 9F);
        btnCancel.ForeColor = Color.FromArgb(45, 45, 45);
        btnCancel.AccessibleDescription = "Image recovered from PDF page 6; command identity Unverified";
        btnCancel.Image = (Image)resources.GetObject("btnCancel.Image");
        btnCancel.Location = new Point(240, 0);
        btnCancel.Margin = new Padding(8, 1, 1, 1);
        btnCancel.Name = "btnCancel";
        btnCancel.RightToLeft = RightToLeft.No;
        btnCancel.Size = new Size(26, 24);
        btnCancel.TabIndex = 3;
        commandToolTip.SetToolTip(btnCancel, "إلغاء");
        btnCancel.UseVisualStyleBackColor = false;
        // 
        // btnDelete
        // 
        btnDelete.AccessibleName = "حذف";
        btnDelete.BackColor = Color.FromArgb(239, 239, 239);
        btnDelete.Enabled = false;
        btnDelete.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnDelete.FlatAppearance.MouseDownBackColor = Color.FromArgb(207, 220, 227);
        btnDelete.FlatAppearance.MouseOverBackColor = Color.FromArgb(225, 235, 240);
        btnDelete.FlatStyle = FlatStyle.Flat;
        btnDelete.Font = new Font("Tahoma", 9F);
        btnDelete.ForeColor = Color.FromArgb(45, 45, 45);
        btnDelete.Image = (Image)resources.GetObject("btnDelete.Image");
        btnDelete.AccessibleDescription = "Generic local red X approximation of the reference; original ONYX artwork unavailable";
        btnDelete.Location = new Point(216, 0);
        btnDelete.Margin = new Padding(8, 1, 1, 1);
        btnDelete.Name = "btnDelete";
        btnDelete.RightToLeft = RightToLeft.No;
        btnDelete.Size = new Size(26, 24);
        btnDelete.TabIndex = 4;
        commandToolTip.SetToolTip(btnDelete, "حذف");
        btnDelete.UseVisualStyleBackColor = false;
        // 
        // btnView
        // 
        btnView.AccessibleName = "عرض";
        btnView.BackColor = Color.FromArgb(239, 239, 239);
        btnView.Enabled = true;
        btnView.Click += availableCurrencies_Click;
        btnView.AccessibleDescription = "عرض العملات المتوفرة — معاينة دون مصدر بيانات مرتبط.";
        btnView.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnView.FlatAppearance.MouseDownBackColor = Color.FromArgb(207, 220, 227);
        btnView.FlatAppearance.MouseOverBackColor = Color.FromArgb(225, 235, 240);
        btnView.FlatStyle = FlatStyle.Flat;
        btnView.Font = new Font("Tahoma", 9F);
        btnView.ForeColor = Color.FromArgb(45, 45, 45);
        btnView.Image = (Image)resources.GetObject("btnView.Image");
        btnView.Location = new Point(192, 0);
        btnView.Margin = new Padding(1);
        btnView.Name = "btnView";
        btnView.RightToLeft = RightToLeft.No;
        btnView.Size = new Size(26, 24);
        btnView.TabIndex = 5;
        commandToolTip.SetToolTip(btnView, "عرض");
        btnView.UseVisualStyleBackColor = false;
        // 
        // btnLast
        // 
        btnLast.AccessibleName = "السجل الأخير";
        btnLast.BackColor = Color.FromArgb(239, 239, 239);
        btnLast.Enabled = false;
        btnLast.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnLast.FlatAppearance.MouseDownBackColor = Color.FromArgb(207, 220, 227);
        btnLast.FlatAppearance.MouseOverBackColor = Color.FromArgb(225, 235, 240);
        btnLast.FlatStyle = FlatStyle.Flat;
        btnLast.Font = new Font("Tahoma", 9F);
        btnLast.ForeColor = Color.FromArgb(45, 45, 45);
        btnLast.Image = (Image)resources.GetObject("btnLast.Image");
        btnLast.Location = new Point(168, 0);
        btnLast.Margin = new Padding(1);
        btnLast.Name = "btnLast";
        btnLast.RightToLeft = RightToLeft.No;
        btnLast.Size = new Size(26, 24);
        btnLast.TabIndex = 6;
        commandToolTip.SetToolTip(btnLast, "السجل الأخير");
        btnLast.UseVisualStyleBackColor = false;
        // 
        // btnNext
        // 
        btnNext.AccessibleName = "السجل التالي";
        btnNext.BackColor = Color.FromArgb(239, 239, 239);
        btnNext.Enabled = false;
        btnNext.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnNext.FlatAppearance.MouseDownBackColor = Color.FromArgb(207, 220, 227);
        btnNext.FlatAppearance.MouseOverBackColor = Color.FromArgb(225, 235, 240);
        btnNext.FlatStyle = FlatStyle.Flat;
        btnNext.Font = new Font("Tahoma", 9F);
        btnNext.ForeColor = Color.FromArgb(45, 45, 45);
        btnNext.Image = (Image)resources.GetObject("btnNext.Image");
        btnNext.Location = new Point(144, 0);
        btnNext.Margin = new Padding(1);
        btnNext.Name = "btnNext";
        btnNext.RightToLeft = RightToLeft.No;
        btnNext.Size = new Size(26, 24);
        btnNext.TabIndex = 7;
        commandToolTip.SetToolTip(btnNext, "السجل التالي");
        btnNext.UseVisualStyleBackColor = false;
        // 
        // btnPrevious
        // 
        btnPrevious.AccessibleName = "السجل السابق";
        btnPrevious.BackColor = Color.FromArgb(239, 239, 239);
        btnPrevious.Enabled = false;
        btnPrevious.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnPrevious.FlatAppearance.MouseDownBackColor = Color.FromArgb(207, 220, 227);
        btnPrevious.FlatAppearance.MouseOverBackColor = Color.FromArgb(225, 235, 240);
        btnPrevious.FlatStyle = FlatStyle.Flat;
        btnPrevious.Font = new Font("Tahoma", 9F);
        btnPrevious.ForeColor = Color.FromArgb(45, 45, 45);
        btnPrevious.Image = (Image)resources.GetObject("btnPrevious.Image");
        btnPrevious.Location = new Point(120, 0);
        btnPrevious.Margin = new Padding(1);
        btnPrevious.Name = "btnPrevious";
        btnPrevious.RightToLeft = RightToLeft.No;
        btnPrevious.Size = new Size(26, 24);
        btnPrevious.TabIndex = 8;
        commandToolTip.SetToolTip(btnPrevious, "السجل السابق");
        btnPrevious.UseVisualStyleBackColor = false;
        // 
        // btnFirst
        // 
        btnFirst.AccessibleName = "السجل الأول";
        btnFirst.BackColor = Color.FromArgb(239, 239, 239);
        btnFirst.Enabled = false;
        btnFirst.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnFirst.FlatAppearance.MouseDownBackColor = Color.FromArgb(207, 220, 227);
        btnFirst.FlatAppearance.MouseOverBackColor = Color.FromArgb(225, 235, 240);
        btnFirst.FlatStyle = FlatStyle.Flat;
        btnFirst.Font = new Font("Tahoma", 9F);
        btnFirst.ForeColor = Color.FromArgb(45, 45, 45);
        btnFirst.Image = (Image)resources.GetObject("btnFirst.Image");
        btnFirst.Location = new Point(96, 0);
        btnFirst.Margin = new Padding(5, 1, 1, 1);
        btnFirst.Name = "btnFirst";
        btnFirst.RightToLeft = RightToLeft.No;
        btnFirst.Size = new Size(26, 24);
        btnFirst.TabIndex = 9;
        commandToolTip.SetToolTip(btnFirst, "السجل الأول");
        btnFirst.UseVisualStyleBackColor = false;
        // 
        // btnSave
        // 
        btnSave.AccessibleDescription = "الحفظ غير متاح؛ لم تُربط الشاشة بخدمة بيانات.";
        btnSave.AccessibleName = "حفظ";
        btnSave.BackColor = Color.FromArgb(239, 239, 239);
        btnSave.Enabled = false;
        btnSave.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnSave.FlatAppearance.MouseDownBackColor = Color.FromArgb(207, 220, 227);
        btnSave.FlatAppearance.MouseOverBackColor = Color.FromArgb(225, 235, 240);
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Font = new Font("Tahoma", 9F);
        btnSave.ForeColor = Color.FromArgb(45, 45, 45);
        btnSave.Image = (Image)resources.GetObject("btnSave.Image");
        btnSave.Location = new Point(72, 0);
        btnSave.Margin = new Padding(5, 1, 1, 1);
        btnSave.Name = "btnSave";
        btnSave.RightToLeft = RightToLeft.No;
        btnSave.Size = new Size(26, 24);
        btnSave.TabIndex = 1;
        commandToolTip.SetToolTip(btnSave, "حفظ");
        btnSave.UseVisualStyleBackColor = false;
        // 
        // btnPrint
        // 
        btnPrint.AccessibleName = "طباعة";
        btnPrint.BackColor = Color.FromArgb(239, 239, 239);
        btnPrint.Enabled = false;
        btnPrint.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnPrint.FlatAppearance.MouseDownBackColor = Color.FromArgb(207, 220, 227);
        btnPrint.FlatAppearance.MouseOverBackColor = Color.FromArgb(225, 235, 240);
        btnPrint.FlatStyle = FlatStyle.Flat;
        btnPrint.Font = new Font("Tahoma", 9F);
        btnPrint.ForeColor = Color.FromArgb(45, 45, 45);
        btnPrint.Image = (Image)resources.GetObject("btnPrint.Image");
        btnPrint.Location = new Point(48, 0);
        btnPrint.Margin = new Padding(1);
        btnPrint.Name = "btnPrint";
        btnPrint.RightToLeft = RightToLeft.No;
        btnPrint.Size = new Size(26, 24);
        btnPrint.TabIndex = 10;
        commandToolTip.SetToolTip(btnPrint, "طباعة");
        btnPrint.UseVisualStyleBackColor = false;
        // 
        // goldenBrand
        // 
        goldenBrand.BackColor = Color.FromArgb(0, 255, 255, 255);
        goldenBrand.Font = new Font("Tahoma", 9F);
        goldenBrand.ForeColor = Color.FromArgb(45, 45, 45);
        goldenBrand.Location = new Point(18, 1);
        goldenBrand.Margin = new Padding(0);
        goldenBrand.Dock = DockStyle.Fill;
        goldenBrand.Name = "goldenBrand";
        goldenBrand.RightToLeft = RightToLeft.No;
        goldenBrand.Size = new Size(82, 20);
        goldenBrand.TabIndex = 12;
        goldenBrand.Text = "ONYX  ERP";
        goldenBrand.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // referenceTool108
        // 
        referenceTool108.AccessibleName = "أداة مرجعية غير محددة";
        referenceTool108.BackColor = Color.FromArgb(0, 255, 255, 255);
        referenceTool108.BorderStyle = BorderStyle.None;
        referenceTool108.Enabled = false;
        referenceTool108.Font = new Font("Tahoma", 9F);
        referenceTool108.ForeColor = Color.FromArgb(45, 45, 45);
        referenceTool108.Location = new Point(108, 2);
        referenceTool108.Margin = new Padding(1);
        referenceTool108.Name = "referenceTool108";
        referenceTool108.RightToLeft = RightToLeft.No;
        referenceTool108.Size = new Size(26, 24);
        referenceTool108.TabIndex = 13;
        referenceTool108.Text = "";
        referenceTool108.AccessibleDescription = "Image recovered from PDF page 6; command identity Unverified";
        referenceTool108.Image = (Image)resources.GetObject("referenceTool108.Image");
        commandToolTip.SetToolTip(referenceTool108, "Unverified command");
        referenceTool108.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // referenceTool390
        // 
        referenceTool390.AccessibleName = "أداة مرجعية غير محددة";
        referenceTool390.BackColor = Color.FromArgb(0, 255, 255, 255);
        referenceTool390.BorderStyle = BorderStyle.None;
        referenceTool390.Enabled = false;
        referenceTool390.Font = new Font("Tahoma", 9F);
        referenceTool390.ForeColor = Color.FromArgb(45, 45, 45);
        referenceTool390.Location = new Point(390, 2);
        referenceTool390.Margin = new Padding(1);
        referenceTool390.Name = "referenceTool390";
        referenceTool390.RightToLeft = RightToLeft.No;
        referenceTool390.Size = new Size(26, 24);
        referenceTool390.TabIndex = 14;
        referenceTool390.Text = "";
        referenceTool390.AccessibleDescription = "Image recovered from PDF page 6; command identity Unverified";
        referenceTool390.Image = (Image)resources.GetObject("referenceTool390.Image");
        commandToolTip.SetToolTip(referenceTool390, "Unverified command");
        referenceTool390.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // referenceTool710
        // 
        referenceTool710.AccessibleName = "أداة مرجعية غير محددة";
        referenceTool710.BackColor = Color.FromArgb(0, 255, 255, 255);
        referenceTool710.BorderStyle = BorderStyle.None;
        referenceTool710.Enabled = false;
        referenceTool710.Font = new Font("Tahoma", 9F);
        referenceTool710.ForeColor = Color.FromArgb(45, 45, 45);
        referenceTool710.Location = new Point(710, 2);
        referenceTool710.Margin = new Padding(1);
        referenceTool710.Name = "referenceTool710";
        referenceTool710.RightToLeft = RightToLeft.No;
        referenceTool710.Size = new Size(26, 24);
        referenceTool710.TabIndex = 15;
        referenceTool710.Text = "";
        referenceTool710.AccessibleDescription = "Image recovered from PDF page 6; command identity Unverified";
        referenceTool710.Image = (Image)resources.GetObject("referenceTool710.Image");
        commandToolTip.SetToolTip(referenceTool710, "Unverified command");
        referenceTool710.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // tabsSetup
        // 
        tabsSetup.Controls.Add(tabgridHistory);
        tabsSetup.Controls.Add(tabgridLimits);
        tabsSetup.Controls.Add(tabgridDenominations);
        tabsSetup.Dock = DockStyle.Fill;
        tabsSetup.Font = new Font("Tahoma", 9F);
        tabsSetup.ForeColor = Color.FromArgb(45, 45, 45);
        tabsSetup.Location = new Point(0, 234);
        tabsSetup.Margin = new Padding(0);
        tabsSetup.Name = "tabsSetup";
        tabsSetup.Padding = new Point(5, 1);
        tabsSetup.RightToLeft = RightToLeft.Yes;
        tabsSetup.RightToLeftLayout = true;
        tabsSetup.SelectedIndex = 0;
        tabsSetup.Size = new Size(1196, 598);
        tabsSetup.TabIndex = 2;
        // 
        // tabgridHistory
        // 
        tabgridHistory.BackColor = Color.FromArgb(244, 244, 244);
        tabgridHistory.Controls.Add(historyLayout);
        historyLayout.Name = "historyLayout";
        historyLayout.Dock = DockStyle.Fill;
        historyLayout.Margin = new Padding(0);
        historyLayout.RightToLeft = RightToLeft.No;
        historyLayout.ColumnCount = 3;
        historyLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        historyLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
        historyLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
        historyLayout.RowCount = 1;
        historyLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        historyLayout.Controls.Add(gridHistory, 1, 0);
        tabgridHistory.Font = new Font("Tahoma", 9F);
        tabgridHistory.ForeColor = Color.FromArgb(45, 45, 45);
        tabgridHistory.Location = new Point(4, 20);
        tabgridHistory.Margin = new Padding(0);
        tabgridHistory.Padding = new Padding(12);
        tabgridHistory.Name = "tabgridHistory";
        tabgridHistory.RightToLeft = RightToLeft.Yes;
        tabgridHistory.Size = new Size(1188, 574);
        tabgridHistory.TabIndex = 0;
        tabgridHistory.Text = "تغيرات أسعار التحويل";
        // 
        // gridHistory
        // 
        gridHistory.AccessibleName = "تغيرات أسعار التحويل";
        gridHistory.AllowUserToAddRows = false;
        gridHistory.AllowUserToDeleteRows = false;
        dataGridViewCellStyle1.BackColor = Color.FromArgb(255, 255, 255);
        gridHistory.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
        gridHistory.Anchor = AnchorStyles.Top;
        gridHistory.BackgroundColor = Color.FromArgb(244, 244, 244);
        gridHistory.BorderStyle = BorderStyle.None;
        gridHistory.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dataGridViewCellStyle2.BackColor = Color.FromArgb(244, 244, 244);
        dataGridViewCellStyle2.Font = new Font("Tahoma", 9F);
        dataGridViewCellStyle2.ForeColor = Color.FromArgb(45, 45, 45);
        dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(0, 120, 212);
        dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(255, 255, 255);
        dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
        gridHistory.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
        gridHistory.ColumnHeadersHeight = 36;
        gridHistory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        gridHistory.Columns.AddRange(new DataGridViewColumn[] { gridHistory_colCurrency, gridHistory_colRate, gridHistory_colMinimum, gridHistory_colMaximum, gridHistory_colUser, gridHistory_colChangedAt });
        dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleRight;
        dataGridViewCellStyle3.BackColor = Color.FromArgb(255, 255, 255);
        dataGridViewCellStyle3.Font = new Font("Tahoma", 9F);
        dataGridViewCellStyle3.ForeColor = Color.FromArgb(45, 45, 45);
        dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(220, 250, 252);
        dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(45, 45, 45);
        dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
        gridHistory.DefaultCellStyle = dataGridViewCellStyle3;
        gridHistory.EnableHeadersVisualStyles = false;
        gridHistory.Font = new Font("Tahoma", 9F);
        gridHistory.GridColor = Color.FromArgb(163, 163, 163);
        gridHistory.Location = new Point(403, 18);
        gridHistory.Margin = new Padding(0);
        gridHistory.Dock = DockStyle.Fill;
        gridHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        gridHistory.Name = "gridHistory";
        gridHistory.ReadOnly = true;
        gridHistory.RightToLeft = RightToLeft.Yes;
        gridHistory.RowHeadersVisible = false;
        gridHistory.RowHeadersWidth = 51;
        gridHistory.RowTemplate.Height = 26;
        gridHistory.ScrollBars = ScrollBars.Both;
        gridHistory.Size = new Size(418, 164);
        gridHistory.TabIndex = 2;
        gridHistory.Paint += emptyGrid_Paint;
        // 
        // gridHistory_colCurrency
        // 
        gridHistory_colCurrency.HeaderText = "العملة";
        gridHistory_colCurrency.MinimumWidth = 10;
        gridHistory_colCurrency.Name = "gridHistory_colCurrency";
        gridHistory_colCurrency.ReadOnly = true;
        gridHistory_colCurrency.SortMode = DataGridViewColumnSortMode.NotSortable;
        gridHistory_colCurrency.Width = 30;
        // 
        // gridHistory_colRate
        // 
        gridHistory_colRate.HeaderText = "سعر التحويل";
        gridHistory_colRate.MinimumWidth = 10;
        gridHistory_colRate.Name = "gridHistory_colRate";
        gridHistory_colRate.ReadOnly = true;
        gridHistory_colRate.SortMode = DataGridViewColumnSortMode.NotSortable;
        gridHistory_colRate.Width = 80;
        // 
        // gridHistory_colMinimum
        // 
        gridHistory_colMinimum.HeaderText = "أدنى سعر تحويل";
        gridHistory_colMinimum.MinimumWidth = 10;
        gridHistory_colMinimum.Name = "gridHistory_colMinimum";
        gridHistory_colMinimum.ReadOnly = true;
        gridHistory_colMinimum.SortMode = DataGridViewColumnSortMode.NotSortable;
        gridHistory_colMinimum.Width = 80;
        // 
        // gridHistory_colMaximum
        // 
        gridHistory_colMaximum.HeaderText = "أعلى سعر تحويل";
        gridHistory_colMaximum.MinimumWidth = 10;
        gridHistory_colMaximum.Name = "gridHistory_colMaximum";
        gridHistory_colMaximum.ReadOnly = true;
        gridHistory_colMaximum.SortMode = DataGridViewColumnSortMode.NotSortable;
        gridHistory_colMaximum.Width = 80;
        // 
        // gridHistory_colUser
        // 
        gridHistory_colUser.HeaderText = "رقم المستخدم";
        gridHistory_colUser.MinimumWidth = 10;
        gridHistory_colUser.Name = "gridHistory_colUser";
        gridHistory_colUser.ReadOnly = true;
        gridHistory_colUser.SortMode = DataGridViewColumnSortMode.NotSortable;
        gridHistory_colUser.Width = 40;
        // 
        // gridHistory_colChangedAt
        // 
        gridHistory_colChangedAt.HeaderText = "التاريخ";
        gridHistory_colChangedAt.MinimumWidth = 10;
        gridHistory_colChangedAt.Name = "gridHistory_colChangedAt";
        gridHistory_colChangedAt.ReadOnly = true;
        gridHistory_colChangedAt.SortMode = DataGridViewColumnSortMode.NotSortable;
        gridHistory_colChangedAt.Width = 108;
        // 
        // tabgridLimits
        // 
        tabgridLimits.BackColor = Color.FromArgb(244, 244, 244);
        tabgridLimits.Controls.Add(goldenLimits);
        tabgridLimits.Font = new Font("Tahoma", 9F);
        tabgridLimits.ForeColor = Color.FromArgb(45, 45, 45);
        tabgridLimits.Location = new Point(4, 20);
        tabgridLimits.Margin = new Padding(0);
        tabgridLimits.Name = "tabgridLimits";
        tabgridLimits.RightToLeft = RightToLeft.Yes;
        tabgridLimits.Size = new Size(758, 208);
        tabgridLimits.TabIndex = 1;
        tabgridLimits.Text = "حدود المستخدمين";
        // 
        // goldenLimits
        // 
        goldenLimits.BackColor = Color.FromArgb(244, 244, 244);
        goldenLimits.Dock = DockStyle.Fill;
        goldenLimits.Font = new Font("Tahoma", 9F);
        goldenLimits.ForeColor = Color.FromArgb(45, 45, 45);
        goldenLimits.Location = new Point(0, 0);
        goldenLimits.Margin = new Padding(0);
        goldenLimits.Name = "goldenLimits";
        goldenLimits.Padding = new Padding(6);
        goldenLimits.RightToLeft = RightToLeft.No;
        goldenLimits.Size = new Size(758, 208);
        goldenLimits.TabIndex = 3;
        // 
        // cboLimitGroup
        // 
        cboLimitGroup.AccessibleDescription = "القائمة غير مرتبطة ببيانات بعد.";
        cboLimitGroup.AccessibleName = "رقم المجموعة";
        cboLimitGroup.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        cboLimitGroup.BackColor = Color.FromArgb(255, 255, 255);
        cboLimitGroup.DropDownStyle = ComboBoxStyle.DropDownList;
        cboLimitGroup.Enabled = false;
        cboLimitGroup.FlatStyle = FlatStyle.Flat;
        cboLimitGroup.Font = new Font("Tahoma", 9F);
        cboLimitGroup.ForeColor = Color.FromArgb(45, 45, 45);
        cboLimitGroup.FormattingEnabled = true;
        cboLimitGroup.Location = new Point(552, 14);
        cboLimitGroup.Margin = new Padding(3);
        cboLimitGroup.Dock = DockStyle.Fill;
        cboLimitGroup.MinimumSize = new Size(60, 24);
        cboLimitGroup.Name = "cboLimitGroup";
        cboLimitGroup.RightToLeft = RightToLeft.No;
        cboLimitGroup.Size = new Size(160, 24);
        cboLimitGroup.TabIndex = 16;
        // 
        // lblcboLimitGroup
        // 
        lblcboLimitGroup.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        lblcboLimitGroup.BackColor = Color.FromArgb(0, 255, 255, 255);
        lblcboLimitGroup.Font = new Font("Tahoma", 9F);
        lblcboLimitGroup.ForeColor = Color.FromArgb(45, 45, 45);
        lblcboLimitGroup.Location = new Point(694, 14);
        lblcboLimitGroup.Margin = new Padding(3);
        lblcboLimitGroup.Dock = DockStyle.Fill;
        lblcboLimitGroup.AutoSize = true;
        lblcboLimitGroup.Name = "lblcboLimitGroup";
        lblcboLimitGroup.RightToLeft = RightToLeft.No;
        lblcboLimitGroup.Size = new Size(58, 14);
        lblcboLimitGroup.TabIndex = 16;
        lblcboLimitGroup.Text = "رقم المجموعة";
        lblcboLimitGroup.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cboLimitBranch
        // 
        cboLimitBranch.AccessibleDescription = "القائمة غير مرتبطة ببيانات بعد.";
        cboLimitBranch.AccessibleName = "رقم الفرع";
        cboLimitBranch.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        cboLimitBranch.BackColor = Color.FromArgb(255, 255, 255);
        cboLimitBranch.DropDownStyle = ComboBoxStyle.DropDownList;
        cboLimitBranch.Enabled = false;
        cboLimitBranch.FlatStyle = FlatStyle.Flat;
        cboLimitBranch.Font = new Font("Tahoma", 9F);
        cboLimitBranch.ForeColor = Color.FromArgb(45, 45, 45);
        cboLimitBranch.FormattingEnabled = true;
        cboLimitBranch.Location = new Point(552, 30);
        cboLimitBranch.Margin = new Padding(3);
        cboLimitBranch.Dock = DockStyle.Fill;
        cboLimitBranch.MinimumSize = new Size(60, 24);
        cboLimitBranch.Name = "cboLimitBranch";
        cboLimitBranch.RightToLeft = RightToLeft.No;
        cboLimitBranch.Size = new Size(160, 24);
        cboLimitBranch.TabIndex = 17;
        // 
        // lblcboLimitBranch
        // 
        lblcboLimitBranch.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        lblcboLimitBranch.BackColor = Color.FromArgb(0, 255, 255, 255);
        lblcboLimitBranch.Font = new Font("Tahoma", 9F);
        lblcboLimitBranch.ForeColor = Color.FromArgb(45, 45, 45);
        lblcboLimitBranch.Location = new Point(694, 30);
        lblcboLimitBranch.Margin = new Padding(3);
        lblcboLimitBranch.Dock = DockStyle.Fill;
        lblcboLimitBranch.AutoSize = true;
        lblcboLimitBranch.Name = "lblcboLimitBranch";
        lblcboLimitBranch.RightToLeft = RightToLeft.No;
        lblcboLimitBranch.Size = new Size(58, 14);
        lblcboLimitBranch.TabIndex = 17;
        lblcboLimitBranch.Text = "رقم الفرع";
        lblcboLimitBranch.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtLimitReceiptMaximum
        // 
        txtLimitReceiptMaximum.AccessibleName = "الحد الأعلى للقبض";
        txtLimitReceiptMaximum.BackColor = Color.FromArgb(255, 255, 255);
        txtLimitReceiptMaximum.BorderStyle = BorderStyle.FixedSingle;
        txtLimitReceiptMaximum.Enabled = false;
        txtLimitReceiptMaximum.Font = new Font("Tahoma", 9F);
        txtLimitReceiptMaximum.ForeColor = Color.FromArgb(45, 45, 45);
        txtLimitReceiptMaximum.Location = new Point(26, 30);
        txtLimitReceiptMaximum.Margin = new Padding(3);
        txtLimitReceiptMaximum.Dock = DockStyle.Fill;
        txtLimitReceiptMaximum.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        txtLimitReceiptMaximum.MinimumSize = new Size(60, 24);
        txtLimitReceiptMaximum.Name = "txtLimitReceiptMaximum";
        txtLimitReceiptMaximum.RightToLeft = RightToLeft.No;
        txtLimitReceiptMaximum.Size = new Size(160, 24);
        txtLimitReceiptMaximum.TabIndex = 24;
        txtLimitReceiptMaximum.TextAlign = HorizontalAlignment.Right;
        // 
        // lbltxtLimitReceiptMaximum
        // 
        lbltxtLimitReceiptMaximum.BackColor = Color.FromArgb(0, 255, 255, 255);
        lbltxtLimitReceiptMaximum.Font = new Font("Tahoma", 9F);
        lbltxtLimitReceiptMaximum.ForeColor = Color.FromArgb(45, 45, 45);
        lbltxtLimitReceiptMaximum.Location = new Point(26, 16);
        lbltxtLimitReceiptMaximum.Margin = new Padding(3);
        lbltxtLimitReceiptMaximum.Dock = DockStyle.Fill;
        lbltxtLimitReceiptMaximum.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        lbltxtLimitReceiptMaximum.AutoSize = true;
        lbltxtLimitReceiptMaximum.Name = "lbltxtLimitReceiptMaximum";
        lbltxtLimitReceiptMaximum.RightToLeft = RightToLeft.No;
        lbltxtLimitReceiptMaximum.Size = new Size(74, 14);
        lbltxtLimitReceiptMaximum.TabIndex = 24;
        lbltxtLimitReceiptMaximum.Text = "الحد الأعلى للقبض";
        lbltxtLimitReceiptMaximum.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtLimitReceiptMinimum
        // 
        txtLimitReceiptMinimum.AccessibleName = "الحد الأدنى للقبض";
        txtLimitReceiptMinimum.BackColor = Color.FromArgb(255, 255, 255);
        txtLimitReceiptMinimum.BorderStyle = BorderStyle.FixedSingle;
        txtLimitReceiptMinimum.Enabled = false;
        txtLimitReceiptMinimum.Font = new Font("Tahoma", 9F);
        txtLimitReceiptMinimum.ForeColor = Color.FromArgb(45, 45, 45);
        txtLimitReceiptMinimum.Location = new Point(100, 30);
        txtLimitReceiptMinimum.Margin = new Padding(3);
        txtLimitReceiptMinimum.Dock = DockStyle.Fill;
        txtLimitReceiptMinimum.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        txtLimitReceiptMinimum.MinimumSize = new Size(60, 24);
        txtLimitReceiptMinimum.Name = "txtLimitReceiptMinimum";
        txtLimitReceiptMinimum.RightToLeft = RightToLeft.No;
        txtLimitReceiptMinimum.Size = new Size(160, 24);
        txtLimitReceiptMinimum.TabIndex = 23;
        txtLimitReceiptMinimum.TextAlign = HorizontalAlignment.Right;
        // 
        // lbltxtLimitReceiptMinimum
        // 
        lbltxtLimitReceiptMinimum.BackColor = Color.FromArgb(0, 255, 255, 255);
        lbltxtLimitReceiptMinimum.Font = new Font("Tahoma", 9F);
        lbltxtLimitReceiptMinimum.ForeColor = Color.FromArgb(45, 45, 45);
        lbltxtLimitReceiptMinimum.Location = new Point(100, 16);
        lbltxtLimitReceiptMinimum.Margin = new Padding(3);
        lbltxtLimitReceiptMinimum.Dock = DockStyle.Fill;
        lbltxtLimitReceiptMinimum.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        lbltxtLimitReceiptMinimum.AutoSize = true;
        lbltxtLimitReceiptMinimum.Name = "lbltxtLimitReceiptMinimum";
        lbltxtLimitReceiptMinimum.RightToLeft = RightToLeft.No;
        lbltxtLimitReceiptMinimum.Size = new Size(74, 14);
        lbltxtLimitReceiptMinimum.TabIndex = 23;
        lbltxtLimitReceiptMinimum.Text = "الحد الأدنى للقبض";
        lbltxtLimitReceiptMinimum.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtLimitPaymentMaximum
        // 
        txtLimitPaymentMaximum.AccessibleName = "الحد الأعلى للصرف";
        txtLimitPaymentMaximum.BackColor = Color.FromArgb(255, 255, 255);
        txtLimitPaymentMaximum.BorderStyle = BorderStyle.FixedSingle;
        txtLimitPaymentMaximum.Enabled = false;
        txtLimitPaymentMaximum.Font = new Font("Tahoma", 9F);
        txtLimitPaymentMaximum.ForeColor = Color.FromArgb(45, 45, 45);
        txtLimitPaymentMaximum.Location = new Point(174, 30);
        txtLimitPaymentMaximum.Margin = new Padding(3);
        txtLimitPaymentMaximum.Dock = DockStyle.Fill;
        txtLimitPaymentMaximum.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        txtLimitPaymentMaximum.MinimumSize = new Size(60, 24);
        txtLimitPaymentMaximum.Name = "txtLimitPaymentMaximum";
        txtLimitPaymentMaximum.RightToLeft = RightToLeft.No;
        txtLimitPaymentMaximum.Size = new Size(160, 24);
        txtLimitPaymentMaximum.TabIndex = 22;
        txtLimitPaymentMaximum.TextAlign = HorizontalAlignment.Right;
        // 
        // lbltxtLimitPaymentMaximum
        // 
        lbltxtLimitPaymentMaximum.BackColor = Color.FromArgb(0, 255, 255, 255);
        lbltxtLimitPaymentMaximum.Font = new Font("Tahoma", 9F);
        lbltxtLimitPaymentMaximum.ForeColor = Color.FromArgb(45, 45, 45);
        lbltxtLimitPaymentMaximum.Location = new Point(174, 16);
        lbltxtLimitPaymentMaximum.Margin = new Padding(3);
        lbltxtLimitPaymentMaximum.Dock = DockStyle.Fill;
        lbltxtLimitPaymentMaximum.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        lbltxtLimitPaymentMaximum.AutoSize = true;
        lbltxtLimitPaymentMaximum.Name = "lbltxtLimitPaymentMaximum";
        lbltxtLimitPaymentMaximum.RightToLeft = RightToLeft.No;
        lbltxtLimitPaymentMaximum.Size = new Size(74, 14);
        lbltxtLimitPaymentMaximum.TabIndex = 22;
        lbltxtLimitPaymentMaximum.Text = "الحد الأعلى للصرف";
        lbltxtLimitPaymentMaximum.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtLimitPaymentMinimum
        // 
        txtLimitPaymentMinimum.AccessibleName = "الحد الأدنى للصرف";
        txtLimitPaymentMinimum.BackColor = Color.FromArgb(255, 255, 255);
        txtLimitPaymentMinimum.BorderStyle = BorderStyle.FixedSingle;
        txtLimitPaymentMinimum.Enabled = false;
        txtLimitPaymentMinimum.Font = new Font("Tahoma", 9F);
        txtLimitPaymentMinimum.ForeColor = Color.FromArgb(45, 45, 45);
        txtLimitPaymentMinimum.Location = new Point(248, 30);
        txtLimitPaymentMinimum.Margin = new Padding(3);
        txtLimitPaymentMinimum.Dock = DockStyle.Fill;
        txtLimitPaymentMinimum.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        txtLimitPaymentMinimum.MinimumSize = new Size(60, 24);
        txtLimitPaymentMinimum.Name = "txtLimitPaymentMinimum";
        txtLimitPaymentMinimum.RightToLeft = RightToLeft.No;
        txtLimitPaymentMinimum.Size = new Size(160, 24);
        txtLimitPaymentMinimum.TabIndex = 21;
        txtLimitPaymentMinimum.TextAlign = HorizontalAlignment.Right;
        // 
        // lbltxtLimitPaymentMinimum
        // 
        lbltxtLimitPaymentMinimum.BackColor = Color.FromArgb(0, 255, 255, 255);
        lbltxtLimitPaymentMinimum.Font = new Font("Tahoma", 9F);
        lbltxtLimitPaymentMinimum.ForeColor = Color.FromArgb(45, 45, 45);
        lbltxtLimitPaymentMinimum.Location = new Point(248, 16);
        lbltxtLimitPaymentMinimum.Margin = new Padding(3);
        lbltxtLimitPaymentMinimum.Dock = DockStyle.Fill;
        lbltxtLimitPaymentMinimum.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        lbltxtLimitPaymentMinimum.AutoSize = true;
        lbltxtLimitPaymentMinimum.Name = "lbltxtLimitPaymentMinimum";
        lbltxtLimitPaymentMinimum.RightToLeft = RightToLeft.No;
        lbltxtLimitPaymentMinimum.Size = new Size(74, 14);
        lbltxtLimitPaymentMinimum.TabIndex = 21;
        lbltxtLimitPaymentMinimum.Text = "الحد الأدنى للصرف";
        lbltxtLimitPaymentMinimum.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtLimitMinimum
        // 
        txtLimitMinimum.AccessibleName = "أدنى سعر تحويل";
        txtLimitMinimum.BackColor = Color.FromArgb(255, 255, 255);
        txtLimitMinimum.BorderStyle = BorderStyle.FixedSingle;
        txtLimitMinimum.Enabled = false;
        txtLimitMinimum.Font = new Font("Tahoma", 9F);
        txtLimitMinimum.ForeColor = Color.FromArgb(45, 45, 45);
        txtLimitMinimum.Location = new Point(322, 30);
        txtLimitMinimum.Margin = new Padding(3);
        txtLimitMinimum.Dock = DockStyle.Fill;
        txtLimitMinimum.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        txtLimitMinimum.MinimumSize = new Size(60, 24);
        txtLimitMinimum.Name = "txtLimitMinimum";
        txtLimitMinimum.RightToLeft = RightToLeft.No;
        txtLimitMinimum.Size = new Size(160, 24);
        txtLimitMinimum.TabIndex = 20;
        txtLimitMinimum.TextAlign = HorizontalAlignment.Right;
        // 
        // lbltxtLimitMinimum
        // 
        lbltxtLimitMinimum.BackColor = Color.FromArgb(0, 255, 255, 255);
        lbltxtLimitMinimum.Font = new Font("Tahoma", 9F);
        lbltxtLimitMinimum.ForeColor = Color.FromArgb(45, 45, 45);
        lbltxtLimitMinimum.Location = new Point(322, 16);
        lbltxtLimitMinimum.Margin = new Padding(3);
        lbltxtLimitMinimum.Dock = DockStyle.Fill;
        lbltxtLimitMinimum.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        lbltxtLimitMinimum.AutoSize = true;
        lbltxtLimitMinimum.Name = "lbltxtLimitMinimum";
        lbltxtLimitMinimum.RightToLeft = RightToLeft.No;
        lbltxtLimitMinimum.Size = new Size(74, 14);
        lbltxtLimitMinimum.TabIndex = 20;
        lbltxtLimitMinimum.Text = "أدنى سعر تحويل";
        lbltxtLimitMinimum.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtLimitMaximum
        // 
        txtLimitMaximum.AccessibleName = "أعلى سعر تحويل";
        txtLimitMaximum.BackColor = Color.FromArgb(255, 255, 255);
        txtLimitMaximum.BorderStyle = BorderStyle.FixedSingle;
        txtLimitMaximum.Enabled = false;
        txtLimitMaximum.Font = new Font("Tahoma", 9F);
        txtLimitMaximum.ForeColor = Color.FromArgb(45, 45, 45);
        txtLimitMaximum.Location = new Point(396, 30);
        txtLimitMaximum.Margin = new Padding(3);
        txtLimitMaximum.Dock = DockStyle.Fill;
        txtLimitMaximum.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        txtLimitMaximum.MinimumSize = new Size(60, 24);
        txtLimitMaximum.Name = "txtLimitMaximum";
        txtLimitMaximum.RightToLeft = RightToLeft.No;
        txtLimitMaximum.Size = new Size(160, 24);
        txtLimitMaximum.TabIndex = 19;
        txtLimitMaximum.TextAlign = HorizontalAlignment.Right;
        // 
        // lbltxtLimitMaximum
        // 
        lbltxtLimitMaximum.BackColor = Color.FromArgb(0, 255, 255, 255);
        lbltxtLimitMaximum.Font = new Font("Tahoma", 9F);
        lbltxtLimitMaximum.ForeColor = Color.FromArgb(45, 45, 45);
        lbltxtLimitMaximum.Location = new Point(396, 16);
        lbltxtLimitMaximum.Margin = new Padding(3);
        lbltxtLimitMaximum.Dock = DockStyle.Fill;
        lbltxtLimitMaximum.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        lbltxtLimitMaximum.AutoSize = true;
        lbltxtLimitMaximum.Name = "lbltxtLimitMaximum";
        lbltxtLimitMaximum.RightToLeft = RightToLeft.No;
        lbltxtLimitMaximum.Size = new Size(74, 14);
        lbltxtLimitMaximum.TabIndex = 19;
        lbltxtLimitMaximum.Text = "أعلى سعر تحويل";
        lbltxtLimitMaximum.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtLimitRate
        // 
        txtLimitRate.AccessibleName = "سعر التحويل";
        txtLimitRate.BackColor = Color.FromArgb(255, 255, 255);
        txtLimitRate.BorderStyle = BorderStyle.FixedSingle;
        txtLimitRate.Enabled = false;
        txtLimitRate.Font = new Font("Tahoma", 9F);
        txtLimitRate.ForeColor = Color.FromArgb(45, 45, 45);
        txtLimitRate.Location = new Point(470, 30);
        txtLimitRate.Margin = new Padding(3);
        txtLimitRate.Dock = DockStyle.Fill;
        txtLimitRate.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        txtLimitRate.MinimumSize = new Size(60, 24);
        txtLimitRate.Name = "txtLimitRate";
        txtLimitRate.RightToLeft = RightToLeft.No;
        txtLimitRate.Size = new Size(160, 24);
        txtLimitRate.TabIndex = 18;
        txtLimitRate.TextAlign = HorizontalAlignment.Right;
        // 
        // lbltxtLimitRate
        // 
        lbltxtLimitRate.BackColor = Color.FromArgb(0, 255, 255, 255);
        lbltxtLimitRate.Font = new Font("Tahoma", 9F);
        lbltxtLimitRate.ForeColor = Color.FromArgb(45, 45, 45);
        lbltxtLimitRate.Location = new Point(470, 16);
        lbltxtLimitRate.Margin = new Padding(3);
        lbltxtLimitRate.Dock = DockStyle.Fill;
        lbltxtLimitRate.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        lbltxtLimitRate.AutoSize = true;
        lbltxtLimitRate.Name = "lbltxtLimitRate";
        lbltxtLimitRate.RightToLeft = RightToLeft.No;
        lbltxtLimitRate.Size = new Size(74, 14);
        lbltxtLimitRate.TabIndex = 18;
        lbltxtLimitRate.Text = "سعر التحويل";
        lbltxtLimitRate.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // btnLoadLimits
        // 
        btnLoadLimits.BackColor = Color.FromArgb(239, 239, 239);
        btnLoadLimits.Enabled = false;
        btnLoadLimits.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnLoadLimits.FlatAppearance.MouseDownBackColor = Color.FromArgb(207, 220, 227);
        btnLoadLimits.FlatAppearance.MouseOverBackColor = Color.FromArgb(225, 235, 240);
        btnLoadLimits.FlatStyle = FlatStyle.Flat;
        btnLoadLimits.Font = new Font("Tahoma", 9F);
        btnLoadLimits.ForeColor = Color.FromArgb(45, 45, 45);
        btnLoadLimits.Location = new Point(378, 47);
        btnLoadLimits.Margin = new Padding(3);
        btnLoadLimits.Dock = DockStyle.Fill;
        btnLoadLimits.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        btnLoadLimits.MinimumSize = new Size(60, 24);
        btnLoadLimits.Name = "btnLoadLimits";
        btnLoadLimits.RightToLeft = RightToLeft.No;
        btnLoadLimits.Size = new Size(160, 24);
        btnLoadLimits.TabIndex = 25;
        btnLoadLimits.Text = "إنزال البيانات";
        commandToolTip.SetToolTip(btnLoadLimits, "إنزال البيانات");
        btnLoadLimits.UseVisualStyleBackColor = false;
        // 
        // btnUpdateLimits
        // 
        btnUpdateLimits.BackColor = Color.FromArgb(239, 239, 239);
        btnUpdateLimits.Enabled = false;
        btnUpdateLimits.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnUpdateLimits.FlatAppearance.MouseDownBackColor = Color.FromArgb(207, 220, 227);
        btnUpdateLimits.FlatAppearance.MouseOverBackColor = Color.FromArgb(225, 235, 240);
        btnUpdateLimits.FlatStyle = FlatStyle.Flat;
        btnUpdateLimits.Font = new Font("Tahoma", 9F);
        btnUpdateLimits.ForeColor = Color.FromArgb(45, 45, 45);
        btnUpdateLimits.Location = new Point(300, 47);
        btnUpdateLimits.Margin = new Padding(3);
        btnUpdateLimits.Dock = DockStyle.Fill;
        btnUpdateLimits.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        btnUpdateLimits.MinimumSize = new Size(60, 24);
        btnUpdateLimits.Name = "btnUpdateLimits";
        btnUpdateLimits.RightToLeft = RightToLeft.No;
        btnUpdateLimits.Size = new Size(160, 24);
        btnUpdateLimits.TabIndex = 26;
        btnUpdateLimits.Text = "تحديث البيانات";
        commandToolTip.SetToolTip(btnUpdateLimits, "تحديث البيانات");
        btnUpdateLimits.UseVisualStyleBackColor = false;
        // 
        // gridLimits
        // 
        gridLimits.AccessibleName = "حدود المستخدمين";
        gridLimits.AllowUserToAddRows = false;
        gridLimits.AllowUserToDeleteRows = false;
        dataGridViewCellStyle4.BackColor = Color.FromArgb(255, 255, 255);
        gridLimits.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
        gridLimits.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        gridLimits.BackgroundColor = Color.FromArgb(244, 244, 244);
        gridLimits.BorderStyle = BorderStyle.None;
        gridLimits.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dataGridViewCellStyle5.BackColor = Color.FromArgb(244, 244, 244);
        dataGridViewCellStyle5.Font = new Font("Tahoma", 9F);
        dataGridViewCellStyle5.ForeColor = Color.FromArgb(45, 45, 45);
        dataGridViewCellStyle5.SelectionBackColor = Color.FromArgb(0, 120, 212);
        dataGridViewCellStyle5.SelectionForeColor = Color.FromArgb(255, 255, 255);
        dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
        gridLimits.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
        gridLimits.ColumnHeadersHeight = 36;
        gridLimits.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        gridLimits.Columns.AddRange(new DataGridViewColumn[] { gridLimits_colUser, gridLimits_colUserName, gridLimits_colConversion, gridLimits_colMaximum, gridLimits_colMinimum, gridLimits_colPaymentMinimum, gridLimits_colPaymentMaximum, gridLimits_colReceiptMinimum, gridLimits_colReceiptMaximum });
        dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleRight;
        dataGridViewCellStyle6.BackColor = Color.FromArgb(255, 255, 255);
        dataGridViewCellStyle6.Font = new Font("Tahoma", 9F);
        dataGridViewCellStyle6.ForeColor = Color.FromArgb(45, 45, 45);
        dataGridViewCellStyle6.SelectionBackColor = Color.FromArgb(220, 250, 252);
        dataGridViewCellStyle6.SelectionForeColor = Color.FromArgb(45, 45, 45);
        dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
        gridLimits.DefaultCellStyle = dataGridViewCellStyle6;
        gridLimits.EnableHeadersVisualStyles = false;
        gridLimits.Font = new Font("Tahoma", 9F);
        gridLimits.GridColor = Color.FromArgb(163, 163, 163);
        gridLimits.Location = new Point(26, 67);
        gridLimits.Margin = new Padding(0);
        gridLimits.Dock = DockStyle.Fill;
        gridLimits.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        gridLimits.ScrollBars = ScrollBars.Both;
        gridLimits.Name = "gridLimits";
        gridLimits.ReadOnly = true;
        gridLimits.RightToLeft = RightToLeft.Yes;
        gridLimits.RowHeadersVisible = false;
        gridLimits.RowHeadersWidth = 51;
        gridLimits.RowTemplate.Height = 26;
        gridLimits.Size = new Size(706, 105);
        gridLimits.TabIndex = 2;
        gridLimits.Paint += emptyGrid_Paint;
        // 
        // gridLimits_colUser
        // 
        gridLimits_colUser.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
        gridLimits_colUser.HeaderText = "رقم المستخدم";
        gridLimits_colUser.MinimumWidth = 10;
        gridLimits_colUser.Name = "gridLimits_colUser";
        gridLimits_colUser.ReadOnly = true;
        gridLimits_colUser.Width = 42;
        // 
        // gridLimits_colUserName
        // 
        gridLimits_colUserName.HeaderText = "اسم المستخدم";
        gridLimits_colUserName.MinimumWidth = 10;
        gridLimits_colUserName.Name = "gridLimits_colUserName";
        gridLimits_colUserName.ReadOnly = true;
        gridLimits_colUserName.SortMode = DataGridViewColumnSortMode.NotSortable;
        gridLimits_colUserName.Width = 136;
        // 
        // gridLimits_colConversion
        // 
        gridLimits_colConversion.HeaderText = "سعر التحويل";
        gridLimits_colConversion.MinimumWidth = 10;
        gridLimits_colConversion.Name = "gridLimits_colConversion";
        gridLimits_colConversion.ReadOnly = true;
        gridLimits_colConversion.SortMode = DataGridViewColumnSortMode.NotSortable;
        gridLimits_colConversion.Width = 78;
        // 
        // gridLimits_colMaximum
        // 
        gridLimits_colMaximum.HeaderText = "أعلى سعر تحويل";
        gridLimits_colMaximum.MinimumWidth = 10;
        gridLimits_colMaximum.Name = "gridLimits_colMaximum";
        gridLimits_colMaximum.ReadOnly = true;
        gridLimits_colMaximum.SortMode = DataGridViewColumnSortMode.NotSortable;
        gridLimits_colMaximum.Width = 74;
        // 
        // gridLimits_colMinimum
        // 
        gridLimits_colMinimum.HeaderText = "أدنى سعر تحويل";
        gridLimits_colMinimum.MinimumWidth = 10;
        gridLimits_colMinimum.Name = "gridLimits_colMinimum";
        gridLimits_colMinimum.ReadOnly = true;
        gridLimits_colMinimum.SortMode = DataGridViewColumnSortMode.NotSortable;
        gridLimits_colMinimum.Width = 74;
        // 
        // gridLimits_colPaymentMinimum
        // 
        gridLimits_colPaymentMinimum.HeaderText = "الحد الأدنى للصرف";
        gridLimits_colPaymentMinimum.MinimumWidth = 10;
        gridLimits_colPaymentMinimum.Name = "gridLimits_colPaymentMinimum";
        gridLimits_colPaymentMinimum.ReadOnly = true;
        gridLimits_colPaymentMinimum.SortMode = DataGridViewColumnSortMode.NotSortable;
        gridLimits_colPaymentMinimum.Width = 74;
        // 
        // gridLimits_colPaymentMaximum
        // 
        gridLimits_colPaymentMaximum.HeaderText = "الحد الأعلى للصرف";
        gridLimits_colPaymentMaximum.MinimumWidth = 10;
        gridLimits_colPaymentMaximum.Name = "gridLimits_colPaymentMaximum";
        gridLimits_colPaymentMaximum.ReadOnly = true;
        gridLimits_colPaymentMaximum.SortMode = DataGridViewColumnSortMode.NotSortable;
        gridLimits_colPaymentMaximum.Width = 74;
        // 
        // gridLimits_colReceiptMinimum
        // 
        gridLimits_colReceiptMinimum.HeaderText = "الحد الأدنى للقبض";
        gridLimits_colReceiptMinimum.MinimumWidth = 10;
        gridLimits_colReceiptMinimum.Name = "gridLimits_colReceiptMinimum";
        gridLimits_colReceiptMinimum.ReadOnly = true;
        gridLimits_colReceiptMinimum.SortMode = DataGridViewColumnSortMode.NotSortable;
        gridLimits_colReceiptMinimum.Width = 74;
        // 
        // gridLimits_colReceiptMaximum
        // 
        gridLimits_colReceiptMaximum.HeaderText = "الحد الأعلى للقبض";
        gridLimits_colReceiptMaximum.MinimumWidth = 10;
        gridLimits_colReceiptMaximum.Name = "gridLimits_colReceiptMaximum";
        gridLimits_colReceiptMaximum.ReadOnly = true;
        gridLimits_colReceiptMaximum.SortMode = DataGridViewColumnSortMode.NotSortable;
        gridLimits_colReceiptMaximum.Width = 74;
        // 
        // tabgridDenominations
        // 
        tabgridDenominations.BackColor = Color.FromArgb(244, 244, 244);
        tabgridDenominations.Controls.Add(denominationsLayout);
        denominationsLayout.Name = "denominationsLayout";
        denominationsLayout.Dock = DockStyle.Fill;
        denominationsLayout.Margin = new Padding(0);
        denominationsLayout.RightToLeft = RightToLeft.No;
        denominationsLayout.ColumnCount = 3;
        denominationsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 49F));
        denominationsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 19F));
        denominationsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32F));
        denominationsLayout.RowCount = 1;
        denominationsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        denominationsLayout.Controls.Add(gridDenominations, 1, 0);
        tabgridDenominations.Font = new Font("Tahoma", 9F);
        tabgridDenominations.ForeColor = Color.FromArgb(45, 45, 45);
        tabgridDenominations.Location = new Point(4, 20);
        tabgridDenominations.Margin = new Padding(0);
        tabgridDenominations.Padding = new Padding(12);
        tabgridDenominations.Name = "tabgridDenominations";
        tabgridDenominations.RightToLeft = RightToLeft.Yes;
        tabgridDenominations.Size = new Size(758, 208);
        tabgridDenominations.TabIndex = 2;
        tabgridDenominations.Text = "الفئات النقدية";
        // 
        // gridDenominations
        // 
        gridDenominations.AccessibleName = "الفئات النقدية";
        gridDenominations.AllowUserToAddRows = false;
        gridDenominations.AllowUserToDeleteRows = false;
        dataGridViewCellStyle7.BackColor = Color.FromArgb(255, 255, 255);
        gridDenominations.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle7;
        gridDenominations.Anchor = AnchorStyles.Top;
        gridDenominations.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        gridDenominations.BackgroundColor = Color.FromArgb(244, 244, 244);
        gridDenominations.BorderStyle = BorderStyle.None;
        gridDenominations.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dataGridViewCellStyle8.BackColor = Color.FromArgb(244, 244, 244);
        dataGridViewCellStyle8.Font = new Font("Tahoma", 9F);
        dataGridViewCellStyle8.ForeColor = Color.FromArgb(45, 45, 45);
        dataGridViewCellStyle8.SelectionBackColor = Color.FromArgb(0, 120, 212);
        dataGridViewCellStyle8.SelectionForeColor = Color.FromArgb(255, 255, 255);
        dataGridViewCellStyle8.WrapMode = DataGridViewTriState.True;
        gridDenominations.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
        gridDenominations.ColumnHeadersHeight = 36;
        gridDenominations.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        gridDenominations.Columns.AddRange(new DataGridViewColumn[] { gridDenominations_colDenomination });
        dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleRight;
        dataGridViewCellStyle9.BackColor = Color.FromArgb(255, 255, 255);
        dataGridViewCellStyle9.Font = new Font("Tahoma", 9F);
        dataGridViewCellStyle9.ForeColor = Color.FromArgb(45, 45, 45);
        dataGridViewCellStyle9.SelectionBackColor = Color.FromArgb(220, 250, 252);
        dataGridViewCellStyle9.SelectionForeColor = Color.FromArgb(45, 45, 45);
        dataGridViewCellStyle9.WrapMode = DataGridViewTriState.True;
        gridDenominations.DefaultCellStyle = dataGridViewCellStyle9;
        gridDenominations.EnableHeadersVisualStyles = false;
        gridDenominations.Font = new Font("Tahoma", 9F);
        gridDenominations.GridColor = Color.FromArgb(163, 163, 163);
        gridDenominations.Location = new Point(373, 18);
        gridDenominations.Margin = new Padding(0);
        gridDenominations.Dock = DockStyle.Fill;
        gridDenominations.Name = "gridDenominations";
        gridDenominations.ReadOnly = true;
        gridDenominations.RightToLeft = RightToLeft.Yes;
        gridDenominations.RowHeadersVisible = false;
        gridDenominations.RowHeadersWidth = 51;
        gridDenominations.RowTemplate.Height = 26;
        gridDenominations.ScrollBars = ScrollBars.Both;
        gridDenominations.Size = new Size(142, 164);
        gridDenominations.TabIndex = 2;
        gridDenominations.Paint += emptyGrid_Paint;
        // 
        // gridDenominations_colDenomination
        // 
        gridDenominations_colDenomination.HeaderText = "الفئة";
        gridDenominations_colDenomination.MinimumWidth = 10;
        gridDenominations_colDenomination.Name = "gridDenominations_colDenomination";
        gridDenominations_colDenomination.ReadOnly = true;
        gridDenominations_colDenomination.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // goldenMetadata
        // 
        goldenMetadata.AccessibleDescription = "مواضع التدقيق مثبتة بالصورة؛ القيم غير مرتبطة ولا تُختلق.";
        goldenMetadata.BackColor = Color.FromArgb(227, 223, 248);
        goldenMetadata.BorderStyle = BorderStyle.FixedSingle;
        goldenMetadata.Controls.Add(goldenAuditFields);
        goldenMetadata.Dock = DockStyle.Fill;
        goldenMetadata.Font = new Font("Tahoma", 9F);
        goldenMetadata.ForeColor = Color.FromArgb(45, 45, 45);
        goldenMetadata.Location = new Point(0, 832);
        goldenMetadata.Margin = new Padding(0);
        goldenMetadata.Name = "goldenMetadata";
        goldenMetadata.Visible = false;
        goldenMetadata.RightToLeft = RightToLeft.No;
        goldenMetadata.Size = new Size(1196, 44);
        goldenMetadata.TabIndex = 3;
        // 
        // goldenAuditFields
        // 
        goldenAuditFields.BackColor = Color.FromArgb(227, 223, 248);
        goldenAuditFields.ColumnCount = 8;
        goldenAuditFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 7F));
        goldenAuditFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        goldenAuditFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 13F));
        goldenAuditFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11F));
        goldenAuditFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14F));
        goldenAuditFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11F));
        goldenAuditFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 26F));
        goldenAuditFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8F));
        goldenAuditFields.Controls.Add(auditCreatedCount, 0, 0);
        goldenAuditFields.Controls.Add(lblAuditCaption1, 1, 0);
        goldenAuditFields.Controls.Add(auditCreatedDevice, 2, 0);
        goldenAuditFields.Controls.Add(lblAuditCaption2, 3, 0);
        goldenAuditFields.Controls.Add(auditCreatedDateTime, 4, 0);
        goldenAuditFields.Controls.Add(lblAuditCaption3, 5, 0);
        goldenAuditFields.Controls.Add(auditCreatedBy, 6, 0);
        goldenAuditFields.Controls.Add(lblAuditCaption4, 7, 0);
        goldenAuditFields.Controls.Add(auditUpdatedCount, 0, 1);
        goldenAuditFields.Controls.Add(lblAuditCaption5, 1, 1);
        goldenAuditFields.Controls.Add(auditUpdatedDevice, 2, 1);
        goldenAuditFields.Controls.Add(lblAuditCaption6, 3, 1);
        goldenAuditFields.Controls.Add(auditUpdatedDateTime, 4, 1);
        goldenAuditFields.Controls.Add(lblAuditCaption7, 5, 1);
        goldenAuditFields.Controls.Add(auditUpdatedBy, 6, 1);
        goldenAuditFields.Controls.Add(lblAuditCaption8, 7, 1);
        goldenAuditFields.Dock = DockStyle.Fill;
        goldenAuditFields.Font = new Font("Tahoma", 9F);
        goldenAuditFields.ForeColor = Color.FromArgb(45, 45, 45);
        goldenAuditFields.Location = new Point(0, 0);
        goldenAuditFields.Margin = new Padding(0);
        goldenAuditFields.Name = "goldenAuditFields";
        goldenAuditFields.Padding = new Padding(14, 5, 8, 5);
        goldenAuditFields.RightToLeft = RightToLeft.No;
        goldenAuditFields.RowCount = 2;
        goldenAuditFields.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        goldenAuditFields.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        goldenAuditFields.Size = new Size(1194, 42);
        goldenAuditFields.TabIndex = 0;
        // 
        // auditCreatedCount
        // 
        auditCreatedCount.AccessibleName = "مرات الطباعة";
        auditCreatedCount.BackColor = Color.FromArgb(255, 255, 255);
        auditCreatedCount.BorderStyle = BorderStyle.FixedSingle;
        auditCreatedCount.Dock = DockStyle.Fill;
        auditCreatedCount.Font = new Font("Tahoma", 9F);
        auditCreatedCount.ForeColor = Color.FromArgb(45, 45, 45);
        auditCreatedCount.Location = new Point(14, 5);
        auditCreatedCount.Margin = new Padding(2);
        auditCreatedCount.Name = "auditCreatedCount";
        auditCreatedCount.RightToLeft = RightToLeft.No;
        auditCreatedCount.Size = new Size(82, 16);
        auditCreatedCount.TabIndex = 0;
        auditCreatedCount.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblAuditCaption1
        // 
        lblAuditCaption1.BackColor = Color.FromArgb(227, 223, 248);
        lblAuditCaption1.Dock = DockStyle.Fill;
        lblAuditCaption1.Font = new Font("Tahoma", 9F);
        lblAuditCaption1.ForeColor = Color.FromArgb(45, 45, 45);
        lblAuditCaption1.Location = new Point(96, 5);
        lblAuditCaption1.Margin = new Padding(0);
        lblAuditCaption1.AutoSize = true;
        lblAuditCaption1.Name = "lblAuditCaption1";
        lblAuditCaption1.RightToLeft = RightToLeft.No;
        lblAuditCaption1.Size = new Size(117, 16);
        lblAuditCaption1.TabIndex = 1;
        lblAuditCaption1.Text = "مرات الطباعة";
        lblAuditCaption1.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // auditCreatedDevice
        // 
        auditCreatedDevice.AccessibleName = "الجهاز المدخل";
        auditCreatedDevice.BackColor = Color.FromArgb(255, 255, 255);
        auditCreatedDevice.BorderStyle = BorderStyle.FixedSingle;
        auditCreatedDevice.Dock = DockStyle.Fill;
        auditCreatedDevice.Font = new Font("Tahoma", 9F);
        auditCreatedDevice.ForeColor = Color.FromArgb(45, 45, 45);
        auditCreatedDevice.Location = new Point(213, 5);
        auditCreatedDevice.Margin = new Padding(2);
        auditCreatedDevice.Name = "auditCreatedDevice";
        auditCreatedDevice.RightToLeft = RightToLeft.No;
        auditCreatedDevice.Size = new Size(152, 16);
        auditCreatedDevice.TabIndex = 2;
        auditCreatedDevice.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblAuditCaption2
        // 
        lblAuditCaption2.BackColor = Color.FromArgb(227, 223, 248);
        lblAuditCaption2.Dock = DockStyle.Fill;
        lblAuditCaption2.Font = new Font("Tahoma", 9F);
        lblAuditCaption2.ForeColor = Color.FromArgb(45, 45, 45);
        lblAuditCaption2.Location = new Point(365, 5);
        lblAuditCaption2.Margin = new Padding(0);
        lblAuditCaption2.AutoSize = true;
        lblAuditCaption2.Name = "lblAuditCaption2";
        lblAuditCaption2.RightToLeft = RightToLeft.No;
        lblAuditCaption2.Size = new Size(128, 16);
        lblAuditCaption2.TabIndex = 3;
        lblAuditCaption2.Text = "الجهاز المدخل";
        lblAuditCaption2.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // auditCreatedDateTime
        // 
        auditCreatedDateTime.AccessibleName = "تاريخ الإدخال";
        auditCreatedDateTime.BackColor = Color.FromArgb(255, 255, 255);
        auditCreatedDateTime.BorderStyle = BorderStyle.FixedSingle;
        auditCreatedDateTime.Dock = DockStyle.Fill;
        auditCreatedDateTime.Font = new Font("Tahoma", 9F);
        auditCreatedDateTime.ForeColor = Color.FromArgb(45, 45, 45);
        auditCreatedDateTime.Location = new Point(493, 5);
        auditCreatedDateTime.Margin = new Padding(2);
        auditCreatedDateTime.Name = "auditCreatedDateTime";
        auditCreatedDateTime.RightToLeft = RightToLeft.No;
        auditCreatedDateTime.Size = new Size(164, 16);
        auditCreatedDateTime.TabIndex = 4;
        auditCreatedDateTime.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblAuditCaption3
        // 
        lblAuditCaption3.BackColor = Color.FromArgb(227, 223, 248);
        lblAuditCaption3.Dock = DockStyle.Fill;
        lblAuditCaption3.Font = new Font("Tahoma", 9F);
        lblAuditCaption3.ForeColor = Color.FromArgb(45, 45, 45);
        lblAuditCaption3.Location = new Point(657, 5);
        lblAuditCaption3.Margin = new Padding(0);
        lblAuditCaption3.AutoSize = true;
        lblAuditCaption3.Name = "lblAuditCaption3";
        lblAuditCaption3.RightToLeft = RightToLeft.No;
        lblAuditCaption3.Size = new Size(128, 16);
        lblAuditCaption3.TabIndex = 5;
        lblAuditCaption3.Text = "تاريخ الإدخال";
        lblAuditCaption3.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // auditCreatedBy
        // 
        auditCreatedBy.AccessibleName = "مدخل السجل";
        auditCreatedBy.BackColor = Color.FromArgb(255, 255, 255);
        auditCreatedBy.BorderStyle = BorderStyle.FixedSingle;
        auditCreatedBy.Controls.Add(auditCreatedById);
        auditCreatedBy.Dock = DockStyle.Fill;
        auditCreatedBy.Font = new Font("Tahoma", 9F);
        auditCreatedBy.ForeColor = Color.FromArgb(45, 45, 45);
        auditCreatedBy.Location = new Point(785, 5);
        auditCreatedBy.Margin = new Padding(2);
        auditCreatedBy.Name = "auditCreatedBy";
        auditCreatedBy.RightToLeft = RightToLeft.No;
        auditCreatedBy.Size = new Size(304, 16);
        auditCreatedBy.TabIndex = 6;
        auditCreatedBy.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // auditCreatedById
        // 
        auditCreatedById.AccessibleName = "مدخل السجل — الرقم";
        auditCreatedById.BackColor = Color.FromArgb(255, 255, 255);
        auditCreatedById.BorderStyle = BorderStyle.FixedSingle;
        auditCreatedById.Dock = DockStyle.Right;
        auditCreatedById.Font = new Font("Tahoma", 9F);
        auditCreatedById.ForeColor = Color.FromArgb(45, 45, 45);
        auditCreatedById.Location = new Point(258, 0);
        auditCreatedById.Margin = new Padding(0);
        auditCreatedById.Name = "auditCreatedById";
        auditCreatedById.RightToLeft = RightToLeft.No;
        auditCreatedById.Size = new Size(44, 14);
        auditCreatedById.TabIndex = 0;
        auditCreatedById.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblAuditCaption4
        // 
        lblAuditCaption4.BackColor = Color.FromArgb(227, 223, 248);
        lblAuditCaption4.Dock = DockStyle.Fill;
        lblAuditCaption4.Font = new Font("Tahoma", 9F);
        lblAuditCaption4.ForeColor = Color.FromArgb(45, 45, 45);
        lblAuditCaption4.Location = new Point(1089, 5);
        lblAuditCaption4.Margin = new Padding(0);
        lblAuditCaption4.AutoSize = true;
        lblAuditCaption4.Name = "lblAuditCaption4";
        lblAuditCaption4.RightToLeft = RightToLeft.No;
        lblAuditCaption4.Size = new Size(97, 16);
        lblAuditCaption4.TabIndex = 7;
        lblAuditCaption4.Text = "مدخل السجل";
        lblAuditCaption4.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // auditUpdatedCount
        // 
        auditUpdatedCount.AccessibleName = "مرات التعديل";
        auditUpdatedCount.BackColor = Color.FromArgb(255, 255, 255);
        auditUpdatedCount.BorderStyle = BorderStyle.FixedSingle;
        auditUpdatedCount.Dock = DockStyle.Fill;
        auditUpdatedCount.Font = new Font("Tahoma", 9F);
        auditUpdatedCount.ForeColor = Color.FromArgb(45, 45, 45);
        auditUpdatedCount.Location = new Point(14, 21);
        auditUpdatedCount.Margin = new Padding(2);
        auditUpdatedCount.Name = "auditUpdatedCount";
        auditUpdatedCount.RightToLeft = RightToLeft.No;
        auditUpdatedCount.Size = new Size(82, 16);
        auditUpdatedCount.TabIndex = 8;
        auditUpdatedCount.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblAuditCaption5
        // 
        lblAuditCaption5.BackColor = Color.FromArgb(227, 223, 248);
        lblAuditCaption5.Dock = DockStyle.Fill;
        lblAuditCaption5.Font = new Font("Tahoma", 9F);
        lblAuditCaption5.ForeColor = Color.FromArgb(45, 45, 45);
        lblAuditCaption5.Location = new Point(96, 21);
        lblAuditCaption5.Margin = new Padding(0);
        lblAuditCaption5.AutoSize = true;
        lblAuditCaption5.Name = "lblAuditCaption5";
        lblAuditCaption5.RightToLeft = RightToLeft.No;
        lblAuditCaption5.Size = new Size(117, 16);
        lblAuditCaption5.TabIndex = 9;
        lblAuditCaption5.Text = "مرات التعديل";
        lblAuditCaption5.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // auditUpdatedDevice
        // 
        auditUpdatedDevice.AccessibleName = "الجهاز المعدل";
        auditUpdatedDevice.BackColor = Color.FromArgb(255, 255, 255);
        auditUpdatedDevice.BorderStyle = BorderStyle.FixedSingle;
        auditUpdatedDevice.Dock = DockStyle.Fill;
        auditUpdatedDevice.Font = new Font("Tahoma", 9F);
        auditUpdatedDevice.ForeColor = Color.FromArgb(45, 45, 45);
        auditUpdatedDevice.Location = new Point(213, 21);
        auditUpdatedDevice.Margin = new Padding(2);
        auditUpdatedDevice.Name = "auditUpdatedDevice";
        auditUpdatedDevice.RightToLeft = RightToLeft.No;
        auditUpdatedDevice.Size = new Size(152, 16);
        auditUpdatedDevice.TabIndex = 10;
        auditUpdatedDevice.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblAuditCaption6
        // 
        lblAuditCaption6.BackColor = Color.FromArgb(227, 223, 248);
        lblAuditCaption6.Dock = DockStyle.Fill;
        lblAuditCaption6.Font = new Font("Tahoma", 9F);
        lblAuditCaption6.ForeColor = Color.FromArgb(45, 45, 45);
        lblAuditCaption6.Location = new Point(365, 21);
        lblAuditCaption6.Margin = new Padding(0);
        lblAuditCaption6.AutoSize = true;
        lblAuditCaption6.Name = "lblAuditCaption6";
        lblAuditCaption6.RightToLeft = RightToLeft.No;
        lblAuditCaption6.Size = new Size(128, 16);
        lblAuditCaption6.TabIndex = 11;
        lblAuditCaption6.Text = "الجهاز المعدل";
        lblAuditCaption6.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // auditUpdatedDateTime
        // 
        auditUpdatedDateTime.AccessibleName = "تاريخ آخر تعديل";
        auditUpdatedDateTime.BackColor = Color.FromArgb(255, 255, 255);
        auditUpdatedDateTime.BorderStyle = BorderStyle.FixedSingle;
        auditUpdatedDateTime.Dock = DockStyle.Fill;
        auditUpdatedDateTime.Font = new Font("Tahoma", 9F);
        auditUpdatedDateTime.ForeColor = Color.FromArgb(45, 45, 45);
        auditUpdatedDateTime.Location = new Point(493, 21);
        auditUpdatedDateTime.Margin = new Padding(2);
        auditUpdatedDateTime.Name = "auditUpdatedDateTime";
        auditUpdatedDateTime.RightToLeft = RightToLeft.No;
        auditUpdatedDateTime.Size = new Size(164, 16);
        auditUpdatedDateTime.TabIndex = 12;
        auditUpdatedDateTime.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblAuditCaption7
        // 
        lblAuditCaption7.BackColor = Color.FromArgb(227, 223, 248);
        lblAuditCaption7.Dock = DockStyle.Fill;
        lblAuditCaption7.Font = new Font("Tahoma", 9F);
        lblAuditCaption7.ForeColor = Color.FromArgb(45, 45, 45);
        lblAuditCaption7.Location = new Point(657, 21);
        lblAuditCaption7.Margin = new Padding(0);
        lblAuditCaption7.AutoSize = true;
        lblAuditCaption7.Name = "lblAuditCaption7";
        lblAuditCaption7.RightToLeft = RightToLeft.No;
        lblAuditCaption7.Size = new Size(128, 16);
        lblAuditCaption7.TabIndex = 13;
        lblAuditCaption7.Text = "تاريخ آخر تعديل";
        lblAuditCaption7.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // auditUpdatedBy
        // 
        auditUpdatedBy.AccessibleName = "معدل السجل";
        auditUpdatedBy.BackColor = Color.FromArgb(255, 255, 255);
        auditUpdatedBy.BorderStyle = BorderStyle.FixedSingle;
        auditUpdatedBy.Controls.Add(auditUpdatedById);
        auditUpdatedBy.Dock = DockStyle.Fill;
        auditUpdatedBy.Font = new Font("Tahoma", 9F);
        auditUpdatedBy.ForeColor = Color.FromArgb(45, 45, 45);
        auditUpdatedBy.Location = new Point(785, 21);
        auditUpdatedBy.Margin = new Padding(2);
        auditUpdatedBy.Name = "auditUpdatedBy";
        auditUpdatedBy.RightToLeft = RightToLeft.No;
        auditUpdatedBy.Size = new Size(304, 16);
        auditUpdatedBy.TabIndex = 14;
        auditUpdatedBy.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // auditUpdatedById
        // 
        auditUpdatedById.AccessibleName = "معدل السجل — الرقم";
        auditUpdatedById.BackColor = Color.FromArgb(255, 255, 255);
        auditUpdatedById.BorderStyle = BorderStyle.FixedSingle;
        auditUpdatedById.Dock = DockStyle.Right;
        auditUpdatedById.Font = new Font("Tahoma", 9F);
        auditUpdatedById.ForeColor = Color.FromArgb(45, 45, 45);
        auditUpdatedById.Location = new Point(258, 0);
        auditUpdatedById.Margin = new Padding(0);
        auditUpdatedById.Name = "auditUpdatedById";
        auditUpdatedById.RightToLeft = RightToLeft.No;
        auditUpdatedById.Size = new Size(44, 14);
        auditUpdatedById.TabIndex = 0;
        auditUpdatedById.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblAuditCaption8
        // 
        lblAuditCaption8.BackColor = Color.FromArgb(227, 223, 248);
        lblAuditCaption8.Dock = DockStyle.Fill;
        lblAuditCaption8.Font = new Font("Tahoma", 9F);
        lblAuditCaption8.ForeColor = Color.FromArgb(45, 45, 45);
        lblAuditCaption8.Location = new Point(1089, 21);
        lblAuditCaption8.Margin = new Padding(0);
        lblAuditCaption8.AutoSize = true;
        lblAuditCaption8.Name = "lblAuditCaption8";
        lblAuditCaption8.RightToLeft = RightToLeft.No;
        lblAuditCaption8.Size = new Size(97, 16);
        lblAuditCaption8.TabIndex = 15;
        lblAuditCaption8.Text = "معدل السجل";
        lblAuditCaption8.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // goldenStatus
        // 
        goldenStatus.BackColor = Color.FromArgb(244, 244, 244);
        goldenStatus.BorderStyle = BorderStyle.FixedSingle;
        goldenStatus.Controls.Add(lblStatus);
        goldenStatus.Controls.Add(goldenScreenCode);
        goldenStatus.Dock = DockStyle.Fill;
        goldenStatus.Font = new Font("Tahoma", 9F);
        goldenStatus.ForeColor = Color.FromArgb(45, 45, 45);
        goldenStatus.Location = new Point(0, 876);
        goldenStatus.Margin = new Padding(0);
        goldenStatus.Name = "goldenStatus";
        goldenStatus.RightToLeft = RightToLeft.No;
        goldenStatus.Size = new Size(1196, 20);
        goldenStatus.TabIndex = 4;
        // 
        // lblStatus
        // 
        lblStatus.AutoSize = true;
        lblStatus.BackColor = Color.FromArgb(0, 255, 255, 255);
        lblStatus.Dock = DockStyle.Fill;
        lblStatus.Font = new Font("Tahoma", 9F);
        lblStatus.ForeColor = Color.FromArgb(45, 45, 45);
        lblStatus.Location = new Point(75, 0);
        lblStatus.Margin = new Padding(0);
        lblStatus.Name = "lblStatus";
        lblStatus.RightToLeft = RightToLeft.No;
        lblStatus.Size = new Size(129, 14);
        lblStatus.TabIndex = 1;
        lblStatus.Text = "معاينة — الحفظ غير متاح";
        lblStatus.TextAlign = ContentAlignment.MiddleRight;
        // 
        // goldenScreenCode
        // 
        goldenScreenCode.BackColor = Color.FromArgb(0, 255, 255, 255);
        goldenScreenCode.BorderStyle = BorderStyle.FixedSingle;
        goldenScreenCode.Dock = DockStyle.Left;
        goldenScreenCode.Font = new Font("Tahoma", 9F);
        goldenScreenCode.ForeColor = Color.FromArgb(45, 45, 45);
        goldenScreenCode.Location = new Point(0, 0);
        goldenScreenCode.Margin = new Padding(0);
        goldenScreenCode.Name = "goldenScreenCode";
        goldenScreenCode.RightToLeft = RightToLeft.No;
        goldenScreenCode.Size = new Size(75, 18);
        goldenScreenCode.TabIndex = 2;
        goldenScreenCode.Text = "GENS004";
        goldenScreenCode.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // Responsive containers: all layout remains serialized in the WinForms Designer.
        goldenMainFields.Name = "goldenMainFields";
        goldenMainFields.Dock = DockStyle.Fill;
        goldenMainFields.Margin = new Padding(0);
        goldenMainFields.RightToLeft = RightToLeft.No;
        goldenConversionFields.Name = "goldenConversionFields";
        goldenConversionFields.Dock = DockStyle.Fill;
        goldenConversionFields.Margin = new Padding(0);
        goldenConversionFields.RightToLeft = RightToLeft.No;
        goldenFactorFields.Name = "goldenFactorFields";
        goldenFactorFields.Dock = DockStyle.Fill;
        goldenFactorFields.Margin = new Padding(0);
        goldenFactorFields.RightToLeft = RightToLeft.No;
        goldenCurrencyFlags.Name = "goldenCurrencyFlags";
        goldenCurrencyFlags.Dock = DockStyle.Fill;
        goldenCurrencyFlags.Margin = new Padding(0);
        goldenCurrencyFlags.RightToLeft = RightToLeft.No;
        goldenContent.ColumnCount = 5;
        goldenContent.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 17F));
        goldenContent.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32F));
        goldenContent.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 2F));
        goldenContent.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
        goldenContent.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16F));
        goldenContent.RowCount = 1;
        goldenContent.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        goldenContent.Controls.Add(goldenConversionFields, 1, 0);
        goldenContent.Controls.Add(goldenMainFields, 3, 0);
        goldenMainFields.ColumnCount = 2;
        goldenMainFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        goldenMainFields.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize, 0F));
        goldenMainFields.RowCount = 11;
        goldenMainFields.RowStyles.Add(new RowStyle(SizeType.Percent, 9.090909F));
        goldenMainFields.RowStyles.Add(new RowStyle(SizeType.Percent, 9.090909F));
        goldenMainFields.RowStyles.Add(new RowStyle(SizeType.Percent, 9.090909F));
        goldenMainFields.RowStyles.Add(new RowStyle(SizeType.Percent, 9.090909F));
        goldenMainFields.RowStyles.Add(new RowStyle(SizeType.Percent, 9.090909F));
        goldenMainFields.RowStyles.Add(new RowStyle(SizeType.Percent, 9.090909F));
        goldenMainFields.RowStyles.Add(new RowStyle(SizeType.Percent, 9.090909F));
        goldenMainFields.RowStyles.Add(new RowStyle(SizeType.Percent, 9.090909F));
        goldenMainFields.RowStyles.Add(new RowStyle(SizeType.Percent, 9.090909F));
        goldenMainFields.RowStyles.Add(new RowStyle(SizeType.Percent, 9.090909F));
        goldenMainFields.RowStyles.Add(new RowStyle(SizeType.Percent, 9.090909F));
        goldenMainFields.Controls.Add(txtNumber, 0, 0);
        goldenMainFields.Controls.Add(lbltxtNumber, 1, 0);
        goldenMainFields.Controls.Add(txtNameLocal, 0, 1);
        goldenMainFields.Controls.Add(lbltxtNameLocal, 1, 1);
        goldenMainFields.Controls.Add(txtNameForeign, 0, 2);
        goldenMainFields.Controls.Add(lbltxtNameForeign, 1, 2);
        goldenMainFields.Controls.Add(cboSymbol, 0, 3);
        goldenMainFields.Controls.Add(lblcboSymbol, 1, 3);
        goldenMainFields.Controls.Add(txtFractionLocal, 0, 4);
        goldenMainFields.Controls.Add(lbltxtFractionLocal, 1, 4);
        goldenMainFields.Controls.Add(txtFractionForeign, 0, 5);
        goldenMainFields.Controls.Add(lbltxtFractionForeign, 1, 5);
        goldenMainFields.Controls.Add(txtRate, 0, 8);
        goldenMainFields.Controls.Add(lbltxtRate, 1, 8);
        goldenMainFields.Controls.Add(txtMaximum, 0, 9);
        goldenMainFields.Controls.Add(lbltxtMaximum, 1, 9);
        goldenMainFields.Controls.Add(txtMinimum, 0, 10);
        goldenMainFields.Controls.Add(lbltxtMinimum, 1, 10);
        goldenMainFields.Controls.Add(goldenCurrencyFlags, 0, 6);
        goldenMainFields.SetColumnSpan(goldenCurrencyFlags, 2);
        goldenCurrencyFlags.FlowDirection = FlowDirection.RightToLeft;
        goldenCurrencyFlags.WrapContents = false;

        optLocal.AccessibleName = "العملة المحلية";
        optLocal.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        optLocal.BackColor = Color.FromArgb(244, 244, 244);
        optLocal.Enabled = false;
        optLocal.Font = new Font("Tahoma", 9F);
        optLocal.ForeColor = Color.FromArgb(45, 45, 45);
        optLocal.Location = new Point(931, 110);
        optLocal.Margin = new Padding(5, 4, 5, 0);
        optLocal.AutoSize = true;
        optLocal.Name = "optLocal";
        optLocal.RightToLeft = RightToLeft.Yes;
        optLocal.Size = new Size(85, 14);
        optLocal.TabIndex = 7;
        optLocal.Text = "عملة محلية";
        optLocal.UseVisualStyleBackColor = false;
        optLocal.CheckedChanged += currencyOption_CheckedChanged;

        optForeign.AccessibleDescription = "يتطلب تفعيل استخدام العملات الأجنبية في المتغيرات العامة؛ الإعداد غير مرتبط.";
        optForeign.AccessibleName = "العملة الأجنبية";
        optForeign.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        optForeign.BackColor = Color.FromArgb(244, 244, 244);
        optForeign.Enabled = false;
        optForeign.Font = new Font("Tahoma", 9F);
        optForeign.ForeColor = Color.FromArgb(45, 45, 45);
        optForeign.Location = new Point(824, 110);
        optForeign.Margin = new Padding(5, 4, 5, 0);
        optForeign.AutoSize = true;
        optForeign.Name = "optForeign";
        optForeign.RightToLeft = RightToLeft.Yes;
        optForeign.Size = new Size(85, 14);
        optForeign.TabIndex = 8;
        optForeign.Text = "عملة أجنبية";
        optForeign.UseVisualStyleBackColor = false;
        optForeign.CheckedChanged += currencyOption_CheckedChanged;
        goldenCurrencyFlags.Controls.Add(optForeign);
        goldenCurrencyFlags.Controls.Add(optLocal);
        goldenMainFields.Controls.Add(chkStock, 0, 7);
        goldenMainFields.SetColumnSpan(chkStock, 2);
        goldenConversionFields.ColumnCount = 2;
        goldenConversionFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        goldenConversionFields.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize, 0F));
        goldenConversionFields.RowCount = 11;
        goldenConversionFields.RowStyles.Add(new RowStyle(SizeType.Percent, 9.090909F));
        goldenConversionFields.RowStyles.Add(new RowStyle(SizeType.Percent, 9.090909F));
        goldenConversionFields.RowStyles.Add(new RowStyle(SizeType.Percent, 9.090909F));
        goldenConversionFields.RowStyles.Add(new RowStyle(SizeType.Percent, 9.090909F));
        goldenConversionFields.RowStyles.Add(new RowStyle(SizeType.Percent, 9.090909F));
        goldenConversionFields.RowStyles.Add(new RowStyle(SizeType.Percent, 9.090909F));
        goldenConversionFields.RowStyles.Add(new RowStyle(SizeType.Percent, 9.090909F));
        goldenConversionFields.RowStyles.Add(new RowStyle(SizeType.Percent, 9.090909F));
        goldenConversionFields.RowStyles.Add(new RowStyle(SizeType.Percent, 9.090909F));
        goldenConversionFields.RowStyles.Add(new RowStyle(SizeType.Percent, 9.090909F));
        goldenConversionFields.RowStyles.Add(new RowStyle(SizeType.Percent, 9.090909F));
        goldenConversionFields.Controls.Add(btnPrintCurrencyControl, 0, 0);
        goldenConversionFields.Controls.Add(lblCurrencyControlPrint, 1, 0);
        goldenConversionFields.Controls.Add(txtDecimals, 0, 7);
        goldenConversionFields.Controls.Add(lbltxtDecimals, 1, 7);
        goldenConversionFields.Controls.Add(txtPosRate, 0, 8);
        goldenConversionFields.Controls.Add(lbltxtPosRate, 1, 8);
        goldenConversionFields.Controls.Add(goldenFactorFields, 0, 6);
        goldenConversionFields.Controls.Add(lbltxtFactor, 1, 6);
        goldenFactorFields.ColumnCount = 3;
        goldenFactorFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        goldenFactorFields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 26F));
        goldenFactorFields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 28F));
        goldenFactorFields.RowCount = 1;
        goldenFactorFields.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        goldenFactorFields.Controls.Add(txtFactor, 0, 0);
        goldenFactorFields.Controls.Add(factorOperator, 1, 0);
        goldenFactorFields.Controls.Add(btnFactorLookup, 2, 0);
        goldenHeading.ColumnCount = 4;
        goldenHeading.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 24F));
        goldenHeading.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22F));
        goldenHeading.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22F));
        goldenHeading.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32F));
        goldenHeading.RowCount = 1;
        goldenHeading.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        goldenHeading.Controls.Add(goldenDate, 0, 0);
        goldenHeading.Controls.Add(goldenPeriod, 1, 0);
        goldenHeading.Controls.Add(goldenUser, 2, 0);
        goldenHeading.Controls.Add(lblTitle, 3, 0);
        // Reusable compact command-bar pattern. All children remain Designer components.
        // Missing artwork reserves a slot without inventing an ONYX symbol or action.
        goldenToolbarHost.Controls.Add(commandBarLayout);
        commandBarLayout.Name = "commandBarLayout";
        commandBarLayout.Dock = DockStyle.Fill;
        commandBarLayout.Margin = new Padding(0);
        commandBarLayout.Padding = new Padding(0);
        commandBarLayout.RightToLeft = RightToLeft.No;
        commandBarLayout.ColumnCount = 7;
        commandBarLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
        commandBarLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 28F));
        commandBarLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 116F));
        commandBarLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 28F));
        commandBarLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        commandBarLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 386F));
        commandBarLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 66F));
        commandBarLayout.RowCount = 1;
        commandBarLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        commandBarLayout.Controls.Add(goldenBrand, 0, 0);
        commandBarLayout.Controls.Add(referenceTool108, 1, 0);
        commandBarLayout.Controls.Add(btnClose, 3, 0);
        commandBarLayout.Controls.Add(pnlToolbar, 5, 0);
        // PDF p.9: seven limits in one row, group/branch stacked on the right.
        goldenLimits.ColumnCount = 9;
        goldenLimits.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        goldenLimits.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        goldenLimits.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        goldenLimits.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        goldenLimits.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        goldenLimits.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        goldenLimits.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        goldenLimits.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 19F));
        goldenLimits.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11F));
        goldenLimits.RowCount = 4;
        goldenLimits.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        goldenLimits.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        goldenLimits.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        goldenLimits.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        goldenLimits.Controls.Add(cboLimitGroup, 7, 0);
        goldenLimits.Controls.Add(lblcboLimitGroup, 8, 0);
        goldenLimits.Controls.Add(cboLimitBranch, 7, 1);
        goldenLimits.Controls.Add(lblcboLimitBranch, 8, 1);
        goldenLimits.Controls.Add(lbltxtLimitReceiptMaximum, 0, 0);
        goldenLimits.Controls.Add(txtLimitReceiptMaximum, 0, 1);
        goldenLimits.Controls.Add(lbltxtLimitReceiptMinimum, 1, 0);
        goldenLimits.Controls.Add(txtLimitReceiptMinimum, 1, 1);
        goldenLimits.Controls.Add(lbltxtLimitPaymentMaximum, 2, 0);
        goldenLimits.Controls.Add(txtLimitPaymentMaximum, 2, 1);
        goldenLimits.Controls.Add(lbltxtLimitPaymentMinimum, 3, 0);
        goldenLimits.Controls.Add(txtLimitPaymentMinimum, 3, 1);
        goldenLimits.Controls.Add(lbltxtLimitMinimum, 4, 0);
        goldenLimits.Controls.Add(txtLimitMinimum, 4, 1);
        goldenLimits.Controls.Add(lbltxtLimitMaximum, 5, 0);
        goldenLimits.Controls.Add(txtLimitMaximum, 5, 1);
        goldenLimits.Controls.Add(lbltxtLimitRate, 6, 0);
        goldenLimits.Controls.Add(txtLimitRate, 6, 1);
        goldenLimits.Controls.Add(btnLoadLimits, 5, 2);
        goldenLimits.Controls.Add(btnUpdateLimits, 4, 2);
        goldenLimits.Controls.Add(gridLimits, 0, 3);
        goldenLimits.SetColumnSpan(gridLimits, 9);
        // Proportions measured from PDF pp.7-9; set after Width assignments so Fill mode preserves them.
        gridHistory_colCurrency.FillWeight = 7F;
        gridHistory_colRate.FillWeight = 19F;
        gridHistory_colMinimum.FillWeight = 19F;
        gridHistory_colMaximum.FillWeight = 19F;
        gridHistory_colUser.FillWeight = 10F;
        gridHistory_colChangedAt.FillWeight = 26F;
        gridHistory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        gridLimits_colUser.FillWeight = 6F;
        gridLimits_colUserName.FillWeight = 19F;
        gridLimits_colConversion.FillWeight = 11F;
        gridLimits_colMaximum.FillWeight = 10.666667F;
        gridLimits_colMinimum.FillWeight = 10.666667F;
        gridLimits_colPaymentMinimum.FillWeight = 10.666667F;
        gridLimits_colPaymentMaximum.FillWeight = 10.666667F;
        gridLimits_colReceiptMinimum.FillWeight = 10.666667F;
        gridLimits_colReceiptMaximum.FillWeight = 10.666667F;
        gridLimits.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        // UcCurrencySetup
        // 
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        AutoScroll = true;
        // A docked child's MinimumSize alone does not enable WinForms scrolling.
        AutoScrollMinSize = new Size(896, 676);
        BackColor = Color.FromArgb(244, 244, 244);
        goldenContent.Controls.Add(goldenSourceOnly, 0, 0);
        rootWorkspaceViewport.Name = "rootWorkspaceViewport";
        rootWorkspaceViewport.Dock = DockStyle.Fill;
        rootWorkspaceViewport.AutoScroll = true;
        rootWorkspaceViewport.AutoScrollMinSize = new Size(1120, 845);
        rootWorkspaceViewport.Margin = Padding.Empty;
        rootWorkspaceViewport.Controls.Add(goldenShell);
        Controls.Add(rootWorkspaceViewport);
        standardAuditMetadata.Name = "standardAuditMetadata";
        standardAuditMetadata.Dock = DockStyle.Bottom;
        standardAuditMetadata.Margin = Padding.Empty;
        standardAuditMetadata.Size = new Size(1100, 64);
        Controls.Add(standardAuditMetadata);
        Font = new Font("Tahoma", 9F);
        ForeColor = Color.FromArgb(0, 0, 0);
        Margin = new Padding(0);
        Name = "UcCurrencySetup";
        Dock = DockStyle.Fill;
        Padding = new Padding(2);
        RightToLeft = RightToLeft.Yes;
        Size = new Size(1100, 760);
        goldenSourceOnly.ResumeLayout(false);
        goldenSourceOnly.PerformLayout();
        goldenShell.ResumeLayout(false);
        goldenHeading.ResumeLayout(false);
        goldenHeading.PerformLayout();
        goldenMainFields.ResumeLayout(false);
        goldenMainFields.PerformLayout();
        goldenConversionFields.ResumeLayout(false);
        goldenConversionFields.PerformLayout();
        goldenFactorFields.ResumeLayout(false);
        goldenFactorFields.PerformLayout();
        goldenCurrencyFlags.ResumeLayout(false);
        goldenCurrencyFlags.PerformLayout();
        goldenContent.ResumeLayout(false);
        goldenContent.PerformLayout();
        goldenToolbarHost.SetCommandRole(btnAdd, TransportERP.Desktop.CoreUI.DesignerCommandRole.Add);
        goldenToolbarHost.SetCommandRole(btnEdit, TransportERP.Desktop.CoreUI.DesignerCommandRole.Edit);
        goldenToolbarHost.SetCommandRole(btnDelete, TransportERP.Desktop.CoreUI.DesignerCommandRole.Delete);
        goldenToolbarHost.SetCommandRole(btnCancel, TransportERP.Desktop.CoreUI.DesignerCommandRole.Cancel);
        goldenToolbarHost.SetCommandRole(btnView, TransportERP.Desktop.CoreUI.DesignerCommandRole.View);
        goldenToolbarHost.SetCommandRole(btnLast, TransportERP.Desktop.CoreUI.DesignerCommandRole.Last);
        goldenToolbarHost.SetCommandRole(btnNext, TransportERP.Desktop.CoreUI.DesignerCommandRole.Next);
        goldenToolbarHost.SetCommandRole(btnPrevious, TransportERP.Desktop.CoreUI.DesignerCommandRole.Previous);
        goldenToolbarHost.SetCommandRole(btnFirst, TransportERP.Desktop.CoreUI.DesignerCommandRole.First);
        goldenToolbarHost.SetCommandRole(btnSave, TransportERP.Desktop.CoreUI.DesignerCommandRole.Save);
        goldenToolbarHost.SetCommandRole(btnPrint, TransportERP.Desktop.CoreUI.DesignerCommandRole.Print);
        goldenToolbarHost.SetCommandRole(btnClose, TransportERP.Desktop.CoreUI.DesignerCommandRole.Close);
        standardCommandRefresh.Name = "standardCommandRefresh";
        standardCommandRefresh.Text = "Refresh";
        standardCommandRefresh.Enabled = false;
        standardCommandRefresh.Visible = false;
        standardCommandRefresh.Size = new Size(26, 24);
        pnlToolbar.Controls.Add(standardCommandRefresh);
        goldenToolbarHost.SetCommandRole(standardCommandRefresh, TransportERP.Desktop.CoreUI.DesignerCommandRole.Refresh);
        standardCommandImport.Name = "standardCommandImport";
        standardCommandImport.Text = "Import";
        standardCommandImport.Enabled = false;
        standardCommandImport.Visible = false;
        standardCommandImport.Size = new Size(26, 24);
        pnlToolbar.Controls.Add(standardCommandImport);
        goldenToolbarHost.SetCommandRole(standardCommandImport, TransportERP.Desktop.CoreUI.DesignerCommandRole.Import);
        standardCommandExport.Name = "standardCommandExport";
        standardCommandExport.Text = "Export";
        standardCommandExport.Enabled = false;
        standardCommandExport.Visible = false;
        standardCommandExport.Size = new Size(26, 24);
        pnlToolbar.Controls.Add(standardCommandExport);
        goldenToolbarHost.SetCommandRole(standardCommandExport, TransportERP.Desktop.CoreUI.DesignerCommandRole.Export);
        standardCommandHelp.Name = "standardCommandHelp";
        standardCommandHelp.Text = "Help";
        standardCommandHelp.Enabled = false;
        standardCommandHelp.Visible = false;
        standardCommandHelp.Size = new Size(26, 24);
        pnlToolbar.Controls.Add(standardCommandHelp);
        goldenToolbarHost.SetCommandRole(standardCommandHelp, TransportERP.Desktop.CoreUI.DesignerCommandRole.Help);
        goldenToolbarHost.ResumeLayout(false);
        commandBarLayout.ResumeLayout(false);
        pnlToolbar.ResumeLayout(false);
        tabsSetup.ResumeLayout(false);
        tabgridHistory.ResumeLayout(false);
        ((ISupportInitialize)gridHistory).EndInit();
        tabgridLimits.ResumeLayout(false);
        goldenLimits.ResumeLayout(false);
        ((ISupportInitialize)gridLimits).EndInit();
        tabgridDenominations.ResumeLayout(false);
        ((ISupportInitialize)gridDenominations).EndInit();
        goldenMetadata.ResumeLayout(false);
        goldenAuditFields.ResumeLayout(false);
        auditCreatedBy.ResumeLayout(false);
        auditUpdatedBy.ResumeLayout(false);
        goldenStatus.ResumeLayout(false);
        goldenStatus.PerformLayout();
        ResumeLayout(false);
    }
    private Panel goldenSourceOnly;
    private ComboBox cboInternationalSymbol;
    private Label lblcboInternationalSymbol;
    private TabControl goldenConditionalTabs;
    private TableLayoutPanel goldenShell;
    private TableLayoutPanel goldenHeading;
    private Label lblTitle;
    private Label goldenUser;
    private Label goldenPeriod;
    private Label goldenDate;
    private TableLayoutPanel goldenMainFields;
    private TableLayoutPanel goldenConversionFields;
    private TableLayoutPanel goldenFactorFields;
    private FlowLayoutPanel goldenCurrencyFlags;
    private TableLayoutPanel goldenContent;
    private TextBox txtNumber;
    private Label lbltxtNumber;
    private TextBox txtNameLocal;
    private Label lbltxtNameLocal;
    private TextBox txtNameForeign;
    private Label lbltxtNameForeign;
    private ComboBox cboSymbol;
    private Label lblcboSymbol;
    private TextBox txtFractionLocal;
    private Label lbltxtFractionLocal;
    private TextBox txtFractionForeign;
    private Label lbltxtFractionForeign;
    private TextBox txtRate;
    private Label lbltxtRate;
    private TextBox txtMaximum;
    private Label lbltxtMaximum;
    private TextBox txtMinimum;
    private Label lbltxtMinimum;
    private TextBox txtFactor;
    private Label lbltxtFactor;
    private Button btnFactorLookup;
    private Label factorOperator;
    private TextBox txtDecimals;
    private Label lbltxtDecimals;
    private TextBox txtPosRate;
    private Label lbltxtPosRate;
    private RadioButton optLocal;
    private RadioButton optForeign;
    private CheckBox chkLocal;
    private CheckBox chkForeign;
    private CheckBox chkStock;
    private Button btnPrintCurrencyControl;
    private Label lblCurrencyControlPrint;
    private Button standardCommandRefresh;
    private Button standardCommandImport;
    private Button standardCommandExport;
    private Button standardCommandHelp;
    private TransportERP.Desktop.CoreUI.DesignerCommandBar goldenToolbarHost;
    private TableLayoutPanel commandBarLayout;
    private Button btnClose;
    private FlowLayoutPanel pnlToolbar;
    private Button btnAdd;
    private Button btnEdit;
    private Button btnCancel;
    private Button btnDelete;
    private Button btnView;
    private Button btnLast;
    private Button btnNext;
    private Button btnPrevious;
    private Button btnFirst;
    private Button btnSave;
    private Button btnPrint;
    private Label goldenBrand;
    private Label referenceTool108;
    private Label referenceTool390;
    private Label referenceTool710;
    private TabControl tabsSetup;
    private TabPage tabgridHistory;
    private TableLayoutPanel historyLayout;
    private DataGridView gridHistory;
    private TabPage tabgridLimits;
    private TableLayoutPanel goldenLimits;
    private ComboBox cboLimitGroup;
    private Label lblcboLimitGroup;
    private ComboBox cboLimitBranch;
    private Label lblcboLimitBranch;
    private TextBox txtLimitReceiptMaximum;
    private Label lbltxtLimitReceiptMaximum;
    private TextBox txtLimitReceiptMinimum;
    private Label lbltxtLimitReceiptMinimum;
    private TextBox txtLimitPaymentMaximum;
    private Label lbltxtLimitPaymentMaximum;
    private TextBox txtLimitPaymentMinimum;
    private Label lbltxtLimitPaymentMinimum;
    private TextBox txtLimitMinimum;
    private Label lbltxtLimitMinimum;
    private TextBox txtLimitMaximum;
    private Label lbltxtLimitMaximum;
    private TextBox txtLimitRate;
    private Label lbltxtLimitRate;
    private Button btnLoadLimits;
    private Button btnUpdateLimits;
    private DataGridView gridLimits;
    private TabPage tabgridDenominations;
    private TableLayoutPanel denominationsLayout;
    private DataGridView gridDenominations;
    private Panel goldenMetadata;
    private TableLayoutPanel goldenAuditFields;
    private Label auditCreatedCount;
    private Label lblAuditCaption1;
    private Label auditCreatedDevice;
    private Label lblAuditCaption2;
    private Label auditCreatedDateTime;
    private Label lblAuditCaption3;
    private Label auditCreatedBy;
    private Label auditCreatedById;
    private Label lblAuditCaption4;
    private Label auditUpdatedCount;
    private Label lblAuditCaption5;
    private Label auditUpdatedDevice;
    private Label lblAuditCaption6;
    private Label auditUpdatedDateTime;
    private Label lblAuditCaption7;
    private Label auditUpdatedBy;
    private Label auditUpdatedById;
    private Label lblAuditCaption8;
    private Panel goldenStatus;
    private Label lblStatus;
    private Label goldenScreenCode;
    private DataGridViewTextBoxColumn gridHistory_colCurrency;
    private DataGridViewTextBoxColumn gridHistory_colRate;
    private DataGridViewTextBoxColumn gridHistory_colMinimum;
    private DataGridViewTextBoxColumn gridHistory_colMaximum;
    private DataGridViewTextBoxColumn gridHistory_colUser;
    private DataGridViewTextBoxColumn gridHistory_colChangedAt;
    private DataGridViewComboBoxColumn gridLimits_colUser;
    private DataGridViewTextBoxColumn gridLimits_colUserName;
    private DataGridViewTextBoxColumn gridLimits_colConversion;
    private DataGridViewTextBoxColumn gridLimits_colMaximum;
    private DataGridViewTextBoxColumn gridLimits_colMinimum;
    private DataGridViewTextBoxColumn gridLimits_colPaymentMinimum;
    private DataGridViewTextBoxColumn gridLimits_colPaymentMaximum;
    private DataGridViewTextBoxColumn gridLimits_colReceiptMinimum;
    private DataGridViewTextBoxColumn gridLimits_colReceiptMaximum;
    private DataGridViewTextBoxColumn gridDenominations_colDenomination;
    private ToolTip commandToolTip;
}

