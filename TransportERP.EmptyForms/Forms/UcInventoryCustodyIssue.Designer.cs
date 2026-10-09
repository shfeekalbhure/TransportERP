using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;
partial class UcInventoryCustodyIssue
{
    private AuditMetadataControl standardAuditMetadata = null!;
    private TableLayoutPanel mainLayout = null!;
    private Label lblTitle = null!;
    private StandardCommandBar commandBar = null!;
    private TableLayoutPanel headerLayout = null!;
    private DataGridView gridItems = null!;
    private DataGridViewButtonColumn colItemLookup = null!;
    private Panel auditFooter = null!;
    private Label lblOperationStatus = null!;
    private Label lblBranch = null!;
    private ComboBox fieldBranch = null!;
    private Label lblDocumentNumber = null!;
    private TextBox fieldDocumentNumber = null!;
    private Label lblDocumentDate = null!;
    private DateTimePicker fieldDocumentDate = null!;
    private Label lblSourceNumber = null!;
    private TextBox fieldSourceNumber = null!;
    private Label lblCustomerNumber = null!;
    private TextBox fieldCustomerNumber = null!;
    private Label lblAccountNumber = null!;
    private TextBox fieldAccountNumber = null!;
    private Label lblCustomerName = null!;
    private TextBox fieldCustomerName = null!;
    private Label lblWarehouse = null!;
    private ComboBox fieldWarehouse = null!;
    private Label lblCurrency = null!;
    private ComboBox fieldCurrency = null!;
    private Label lblExchangeRate = null!;
    private TextBox fieldExchangeRate = null!;
    private Label lblCostCenter = null!;
    private ComboBox fieldCostCenter = null!;
    private Label lblReference = null!;
    private TextBox fieldReference = null!;
    private Label lblRecipient = null!;
    private TextBox fieldRecipient = null!;
    private Label lblDescription = null!;
    private TextBox fieldDescription = null!;
    private DataGridViewTextBoxColumn colSequence = null!;
    private DataGridViewTextBoxColumn colItemNumber = null!;
    private DataGridViewTextBoxColumn colItemName = null!;
    private DataGridViewTextBoxColumn colUnit = null!;
    private DataGridViewTextBoxColumn colQuantity = null!;
    private DataGridViewTextBoxColumn colFreeQuantity = null!;
    private DataGridViewTextBoxColumn colPrice = null!;
    private DataGridViewTextBoxColumn colValue = null!;
    private TableLayoutPanel totalsLayout = null!;
    private Label lblTotal = null!;
    private TextBox txtTotal = null!;
    private void InitializeComponent()
    {
        SuspendLayout();
        mainLayout = new TableLayoutPanel();
        mainLayout.Name = "mainLayout";
        mainLayout.Dock = DockStyle.Fill;
        mainLayout.Margin = Padding.Empty;
        mainLayout.ColumnCount = 1;
        mainLayout.RowCount = 7;
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,100F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute,26F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute,34F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute,224F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent,100F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute,30F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute,80F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute,28F));
        lblTitle = new Label();
        lblTitle.Name="lblTitle";
        lblTitle.Text="إذن صرف أمانات";
        lblTitle.Dock=DockStyle.Fill;
        lblTitle.TextAlign=ContentAlignment.MiddleLeft;
        lblTitle.BackColor=Color.FromArgb(23,50,78);
        lblTitle.ForeColor=Color.White;
        mainLayout.Controls.Add(lblTitle,0,0);
        commandBar = new StandardCommandBar();
        commandBar.Name="commandBar";
        commandBar.Dock=DockStyle.Fill;
        commandBar.Margin=Padding.Empty;
        mainLayout.Controls.Add(commandBar,0,1);
        headerLayout = new TableLayoutPanel();
        headerLayout.Name="headerLayout";
        headerLayout.Dock=DockStyle.Fill;
        headerLayout.ColumnCount=6;
        headerLayout.RowCount=7;
        headerLayout.Padding=new Padding(16,6,16,6);
        headerLayout.Margin=Padding.Empty;
        headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,105F));
        headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,33.333F));
        headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,105F));
        headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,33.333F));
        headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,105F));
        headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,33.333F));
        headerLayout.RowStyles.Add(new RowStyle(SizeType.Absolute,30F));
        headerLayout.RowStyles.Add(new RowStyle(SizeType.Absolute,30F));
        headerLayout.RowStyles.Add(new RowStyle(SizeType.Absolute,30F));
        headerLayout.RowStyles.Add(new RowStyle(SizeType.Absolute,30F));
        headerLayout.RowStyles.Add(new RowStyle(SizeType.Absolute,30F));
        headerLayout.RowStyles.Add(new RowStyle(SizeType.Absolute,30F));
        headerLayout.RowStyles.Add(new RowStyle(SizeType.Absolute,30F));
        lblBranch = new Label();
        lblBranch.Name="lblBranch";
        lblBranch.Text="الفرع";
        lblBranch.Dock=DockStyle.Fill;
        lblBranch.TextAlign=ContentAlignment.MiddleLeft;
        fieldBranch = new ComboBox();
        fieldBranch.Name="fieldBranch";
        fieldBranch.AccessibleName="الفرع";
        fieldBranch.Dock=DockStyle.Fill;
        fieldBranch.TabStop=false;
        headerLayout.Controls.Add(lblBranch,0,0);
        headerLayout.Controls.Add(fieldBranch,1,0);
        headerLayout.SetColumnSpan(fieldBranch,5);
        fieldBranch.DropDownStyle = ComboBoxStyle.DropDownList;
        fieldBranch.Enabled = false;
        fieldBranch.BackColor = Color.LightYellow;
        lblDocumentNumber = new Label();
        lblDocumentNumber.Name="lblDocumentNumber";
        lblDocumentNumber.Text="رقم الوثيقة";
        lblDocumentNumber.Dock=DockStyle.Fill;
        lblDocumentNumber.TextAlign=ContentAlignment.MiddleLeft;
        fieldDocumentNumber = new TextBox();
        fieldDocumentNumber.Name="fieldDocumentNumber";
        fieldDocumentNumber.AccessibleName="رقم الوثيقة";
        fieldDocumentNumber.Dock=DockStyle.Fill;
        fieldDocumentNumber.TabStop=false;
        headerLayout.Controls.Add(lblDocumentNumber,0,1);
        headerLayout.Controls.Add(fieldDocumentNumber,1,1);
        headerLayout.SetColumnSpan(fieldDocumentNumber,1);
        fieldDocumentNumber.ReadOnly = true;
        lblDocumentDate = new Label();
        lblDocumentDate.Name="lblDocumentDate";
        lblDocumentDate.Text="التاريخ";
        lblDocumentDate.Dock=DockStyle.Fill;
        lblDocumentDate.TextAlign=ContentAlignment.MiddleLeft;
        fieldDocumentDate = new DateTimePicker();
        fieldDocumentDate.Name="fieldDocumentDate";
        fieldDocumentDate.AccessibleName="التاريخ";
        fieldDocumentDate.Dock=DockStyle.Fill;
        fieldDocumentDate.TabStop=false;
        headerLayout.Controls.Add(lblDocumentDate,2,1);
        headerLayout.Controls.Add(fieldDocumentDate,3,1);
        headerLayout.SetColumnSpan(fieldDocumentDate,1);
        fieldDocumentDate.Format = DateTimePickerFormat.Custom;
        fieldDocumentDate.CustomFormat = "yyyy-MM-dd";
        fieldDocumentDate.ShowCheckBox = true;
        fieldDocumentDate.Checked = false;
        fieldDocumentDate.Enabled = false;
        lblSourceNumber = new Label();
        lblSourceNumber.Name="lblSourceNumber";
        lblSourceNumber.Text="إذن توريد أمانة";
        lblSourceNumber.Dock=DockStyle.Fill;
        lblSourceNumber.TextAlign=ContentAlignment.MiddleLeft;
        fieldSourceNumber = new TextBox();
        fieldSourceNumber.Name="fieldSourceNumber";
        fieldSourceNumber.AccessibleName="إذن توريد أمانة";
        fieldSourceNumber.Dock=DockStyle.Fill;
        fieldSourceNumber.TabStop=true;
        headerLayout.Controls.Add(lblSourceNumber,4,1);
        headerLayout.Controls.Add(fieldSourceNumber,5,1);
        headerLayout.SetColumnSpan(fieldSourceNumber,1);
        fieldSourceNumber.ReadOnly = true;
        lblCustomerNumber = new Label();
        lblCustomerNumber.Name="lblCustomerNumber";
        lblCustomerNumber.Text="رقم العميل";
        lblCustomerNumber.Dock=DockStyle.Fill;
        lblCustomerNumber.TextAlign=ContentAlignment.MiddleLeft;
        fieldCustomerNumber = new TextBox();
        fieldCustomerNumber.Name="fieldCustomerNumber";
        fieldCustomerNumber.AccessibleName="رقم العميل";
        fieldCustomerNumber.Dock=DockStyle.Fill;
        fieldCustomerNumber.TabStop=false;
        headerLayout.Controls.Add(lblCustomerNumber,0,2);
        headerLayout.Controls.Add(fieldCustomerNumber,1,2);
        headerLayout.SetColumnSpan(fieldCustomerNumber,1);
        fieldCustomerNumber.ReadOnly = true;
        lblAccountNumber = new Label();
        lblAccountNumber.Name="lblAccountNumber";
        lblAccountNumber.Text="رقم الحساب";
        lblAccountNumber.Dock=DockStyle.Fill;
        lblAccountNumber.TextAlign=ContentAlignment.MiddleLeft;
        fieldAccountNumber = new TextBox();
        fieldAccountNumber.Name="fieldAccountNumber";
        fieldAccountNumber.AccessibleName="رقم الحساب";
        fieldAccountNumber.Dock=DockStyle.Fill;
        fieldAccountNumber.TabStop=false;
        headerLayout.Controls.Add(lblAccountNumber,2,2);
        headerLayout.Controls.Add(fieldAccountNumber,3,2);
        headerLayout.SetColumnSpan(fieldAccountNumber,1);
        fieldAccountNumber.ReadOnly = true;
        lblCustomerName = new Label();
        lblCustomerName.Name="lblCustomerName";
        lblCustomerName.Text="";
        lblCustomerName.Dock=DockStyle.Fill;
        lblCustomerName.TextAlign=ContentAlignment.MiddleLeft;
        fieldCustomerName = new TextBox();
        fieldCustomerName.Name="fieldCustomerName";
        fieldCustomerName.AccessibleName="اسم العميل";
        fieldCustomerName.Dock=DockStyle.Fill;
        fieldCustomerName.TabStop=false;
        headerLayout.Controls.Add(lblCustomerName,4,2);
        headerLayout.Controls.Add(fieldCustomerName,5,2);
        headerLayout.SetColumnSpan(fieldCustomerName,1);
        fieldCustomerName.ReadOnly = true;
        lblWarehouse = new Label();
        lblWarehouse.Name="lblWarehouse";
        lblWarehouse.Text="المخزن";
        lblWarehouse.Dock=DockStyle.Fill;
        lblWarehouse.TextAlign=ContentAlignment.MiddleLeft;
        fieldWarehouse = new ComboBox();
        fieldWarehouse.Name="fieldWarehouse";
        fieldWarehouse.AccessibleName="المخزن";
        fieldWarehouse.Dock=DockStyle.Fill;
        fieldWarehouse.TabStop=false;
        headerLayout.Controls.Add(lblWarehouse,0,3);
        headerLayout.Controls.Add(fieldWarehouse,1,3);
        headerLayout.SetColumnSpan(fieldWarehouse,1);
        fieldWarehouse.DropDownStyle = ComboBoxStyle.DropDownList;
        fieldWarehouse.Enabled = false;
        fieldWarehouse.BackColor = Color.LightYellow;
        lblCurrency = new Label();
        lblCurrency.Name="lblCurrency";
        lblCurrency.Text="العملة";
        lblCurrency.Dock=DockStyle.Fill;
        lblCurrency.TextAlign=ContentAlignment.MiddleLeft;
        fieldCurrency = new ComboBox();
        fieldCurrency.Name="fieldCurrency";
        fieldCurrency.AccessibleName="العملة";
        fieldCurrency.Dock=DockStyle.Fill;
        fieldCurrency.TabStop=false;
        headerLayout.Controls.Add(lblCurrency,2,3);
        headerLayout.Controls.Add(fieldCurrency,3,3);
        headerLayout.SetColumnSpan(fieldCurrency,1);
        fieldCurrency.DropDownStyle = ComboBoxStyle.DropDownList;
        fieldCurrency.Enabled = false;
        fieldCurrency.BackColor = Color.LightYellow;
        lblExchangeRate = new Label();
        lblExchangeRate.Name="lblExchangeRate";
        lblExchangeRate.Text="سعر التحويل";
        lblExchangeRate.Dock=DockStyle.Fill;
        lblExchangeRate.TextAlign=ContentAlignment.MiddleLeft;
        fieldExchangeRate = new TextBox();
        fieldExchangeRate.Name="fieldExchangeRate";
        fieldExchangeRate.AccessibleName="سعر التحويل";
        fieldExchangeRate.Dock=DockStyle.Fill;
        fieldExchangeRate.TabStop=false;
        headerLayout.Controls.Add(lblExchangeRate,4,3);
        headerLayout.Controls.Add(fieldExchangeRate,5,3);
        headerLayout.SetColumnSpan(fieldExchangeRate,1);
        fieldExchangeRate.ReadOnly = true;
        lblCostCenter = new Label();
        lblCostCenter.Name="lblCostCenter";
        lblCostCenter.Text="مركز التكلفة";
        lblCostCenter.Dock=DockStyle.Fill;
        lblCostCenter.TextAlign=ContentAlignment.MiddleLeft;
        fieldCostCenter = new ComboBox();
        fieldCostCenter.Name="fieldCostCenter";
        fieldCostCenter.AccessibleName="مركز التكلفة";
        fieldCostCenter.Dock=DockStyle.Fill;
        fieldCostCenter.TabStop=false;
        headerLayout.Controls.Add(lblCostCenter,0,4);
        headerLayout.Controls.Add(fieldCostCenter,1,4);
        headerLayout.SetColumnSpan(fieldCostCenter,1);
        fieldCostCenter.DropDownStyle = ComboBoxStyle.DropDownList;
        fieldCostCenter.Enabled = false;
        fieldCostCenter.BackColor = Color.LightYellow;
        lblReference = new Label();
        lblReference.Name="lblReference";
        lblReference.Text="رقم المرجع";
        lblReference.Dock=DockStyle.Fill;
        lblReference.TextAlign=ContentAlignment.MiddleLeft;
        fieldReference = new TextBox();
        fieldReference.Name="fieldReference";
        fieldReference.AccessibleName="رقم المرجع";
        fieldReference.Dock=DockStyle.Fill;
        fieldReference.TabStop=false;
        headerLayout.Controls.Add(lblReference,2,5);
        headerLayout.Controls.Add(fieldReference,3,5);
        headerLayout.SetColumnSpan(fieldReference,1);
        fieldReference.ReadOnly = true;
        lblRecipient = new Label();
        lblRecipient.Name="lblRecipient";
        lblRecipient.Text="اسم المستلم";
        lblRecipient.Dock=DockStyle.Fill;
        lblRecipient.TextAlign=ContentAlignment.MiddleLeft;
        fieldRecipient = new TextBox();
        fieldRecipient.Name="fieldRecipient";
        fieldRecipient.AccessibleName="اسم المستلم";
        fieldRecipient.Dock=DockStyle.Fill;
        fieldRecipient.TabStop=false;
        headerLayout.Controls.Add(lblRecipient,4,5);
        headerLayout.Controls.Add(fieldRecipient,5,5);
        headerLayout.SetColumnSpan(fieldRecipient,1);
        fieldRecipient.ReadOnly = true;
        lblDescription = new Label();
        lblDescription.Name="lblDescription";
        lblDescription.Text="البيان";
        lblDescription.Dock=DockStyle.Fill;
        lblDescription.TextAlign=ContentAlignment.MiddleLeft;
        fieldDescription = new TextBox();
        fieldDescription.Name="fieldDescription";
        fieldDescription.AccessibleName="البيان";
        fieldDescription.Dock=DockStyle.Fill;
        fieldDescription.TabStop=true;
        headerLayout.Controls.Add(lblDescription,0,6);
        headerLayout.Controls.Add(fieldDescription,1,6);
        headerLayout.SetColumnSpan(fieldDescription,5);
        fieldDescription.ReadOnly = false;
        mainLayout.Controls.Add(headerLayout,0,2);
        gridItems = new DataGridView();
        gridItems.Name="gridItems";
        gridItems.Dock=DockStyle.Fill;
        gridItems.AutoGenerateColumns=false;
        gridItems.ReadOnly=false;
        gridItems.AllowUserToAddRows=false;
        gridItems.AllowUserToDeleteRows=false;
        gridItems.AllowUserToOrderColumns=false;
        gridItems.RowHeadersVisible=false;
        gridItems.BackgroundColor=Color.White;
        gridItems.AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill;
        gridItems.ColumnHeadersHeight=44;
        gridItems.ColumnHeadersHeightSizeMode=DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        gridItems.AccessibleName = "بيانات الأصناف";
        colItemLookup = new DataGridViewButtonColumn();
        colItemLookup.Name="itemLookup";
        colItemLookup.HeaderText="";
        colItemLookup.Text="...";
        colItemLookup.UseColumnTextForButtonValue=true;
        colItemLookup.Width=25;
        colItemLookup.MinimumWidth=25;
        colItemLookup.AutoSizeMode=DataGridViewAutoSizeColumnMode.None;
        colItemLookup.ReadOnly=true;
        gridItems.Columns.Add(colItemLookup);
        colSequence = new DataGridViewTextBoxColumn();
        colSequence.Name="Sequence";
        colSequence.HeaderText="م";
        colSequence.ReadOnly=true;
        colSequence.Width=35;
        colSequence.FillWeight=35;
        colSequence.MinimumWidth=30;
        colSequence.SortMode=DataGridViewColumnSortMode.NotSortable;
        gridItems.Columns.Add(colSequence);
        colSequence.DefaultCellStyle.BackColor = Color.WhiteSmoke;
        colItemNumber = new DataGridViewTextBoxColumn();
        colItemNumber.Name="ItemNumber";
        colItemNumber.HeaderText="رقم الصنف";
        colItemNumber.ReadOnly=true;
        colItemNumber.Width=120;
        colItemNumber.FillWeight=120;
        colItemNumber.MinimumWidth=55;
        colItemNumber.SortMode=DataGridViewColumnSortMode.NotSortable;
        gridItems.Columns.Add(colItemNumber);
        colItemNumber.DefaultCellStyle.BackColor = Color.LightYellow;
        colItemName = new DataGridViewTextBoxColumn();
        colItemName.Name="ItemName";
        colItemName.HeaderText="اسم الصنف";
        colItemName.ReadOnly=true;
        colItemName.Width=230;
        colItemName.FillWeight=230;
        colItemName.MinimumWidth=55;
        colItemName.SortMode=DataGridViewColumnSortMode.NotSortable;
        gridItems.Columns.Add(colItemName);
        colItemName.DefaultCellStyle.BackColor = Color.WhiteSmoke;
        colUnit = new DataGridViewTextBoxColumn();
        colUnit.Name="Unit";
        colUnit.HeaderText="الوحدة";
        colUnit.ReadOnly=true;
        colUnit.Width=70;
        colUnit.FillWeight=70;
        colUnit.MinimumWidth=55;
        colUnit.SortMode=DataGridViewColumnSortMode.NotSortable;
        gridItems.Columns.Add(colUnit);
        colUnit.DefaultCellStyle.BackColor = Color.WhiteSmoke;
        colQuantity = new DataGridViewTextBoxColumn();
        colQuantity.Name="Quantity";
        colQuantity.HeaderText="الكمية";
        colQuantity.ReadOnly=false;
        colQuantity.ValueType=typeof(decimal);
        colQuantity.Width=85;
        colQuantity.FillWeight=85;
        colQuantity.MinimumWidth=55;
        colQuantity.SortMode=DataGridViewColumnSortMode.NotSortable;
        gridItems.Columns.Add(colQuantity);
        colQuantity.DefaultCellStyle.BackColor = Color.LightYellow;
        colFreeQuantity = new DataGridViewTextBoxColumn();
        colFreeQuantity.Name="FreeQuantity";
        colFreeQuantity.HeaderText="ك. المجانية";
        colFreeQuantity.ReadOnly=true;
        colFreeQuantity.Width=90;
        colFreeQuantity.FillWeight=90;
        colFreeQuantity.MinimumWidth=55;
        colFreeQuantity.SortMode=DataGridViewColumnSortMode.NotSortable;
        gridItems.Columns.Add(colFreeQuantity);
        colFreeQuantity.DefaultCellStyle.BackColor = Color.LightCyan;
        colPrice = new DataGridViewTextBoxColumn();
        colPrice.Name="Price";
        colPrice.HeaderText="السعر";
        colPrice.ReadOnly=true;
        colPrice.Width=90;
        colPrice.FillWeight=90;
        colPrice.MinimumWidth=55;
        colPrice.SortMode=DataGridViewColumnSortMode.NotSortable;
        gridItems.Columns.Add(colPrice);
        colPrice.DefaultCellStyle.BackColor = Color.WhiteSmoke;
        colValue = new DataGridViewTextBoxColumn();
        colValue.Name="Value";
        colValue.HeaderText="القيمة";
        colValue.ReadOnly=true;
        colValue.Width=105;
        colValue.FillWeight=105;
        colValue.MinimumWidth=55;
        colValue.SortMode=DataGridViewColumnSortMode.NotSortable;
        gridItems.Columns.Add(colValue);
        colValue.DefaultCellStyle.BackColor = Color.MistyRose;
        mainLayout.Controls.Add(gridItems,0,3);
        totalsLayout = new TableLayoutPanel();
        totalsLayout.Name="totalsLayout";
        totalsLayout.Dock=DockStyle.Fill;
        totalsLayout.ColumnCount=3;
        totalsLayout.RowCount=1;
        totalsLayout.RightToLeft=RightToLeft.No;
        totalsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,180F));
        totalsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,180F));
        lblTotal = new Label();
        lblTotal.Text="المجموع";
        lblTotal.Dock=DockStyle.Fill;
        lblTotal.TextAlign=ContentAlignment.MiddleLeft;
        txtTotal = new TextBox();
        txtTotal.Name="txtTotal";
        txtTotal.AccessibleName="المجموع";
        txtTotal.ReadOnly=true;
        txtTotal.Dock=DockStyle.Fill;
        txtTotal.BackColor=Color.FromArgb(190,255,190);
        txtTotal.TabStop=false;
        totalsLayout.Controls.Add(lblTotal,1,0);
        totalsLayout.Controls.Add(txtTotal,0,0);
        mainLayout.Controls.Add(totalsLayout,0,4);
        auditFooter = new Panel();
        auditFooter.Name="auditFooter";
        auditFooter.Dock=DockStyle.Fill;
        auditFooter.Margin=Padding.Empty;
        standardAuditMetadata = new AuditMetadataControl();
        standardAuditMetadata.Name = "standardAuditMetadata";
        standardAuditMetadata.Dock = DockStyle.Fill;
        standardAuditMetadata.Margin = Padding.Empty;
        standardAuditMetadata.Profile = AuditMetadataProfile.Standard;
        auditFooter.Controls.Add(standardAuditMetadata);
        mainLayout.Controls.Add(auditFooter,0,5);
        lblOperationStatus = new Label();
        lblOperationStatus.Name="lblOperationStatus";
        lblOperationStatus.Text="لم يتم ربط خدمة أمانات الأصناف بعد.";
        lblOperationStatus.Dock=DockStyle.Fill;
        lblOperationStatus.TextAlign=ContentAlignment.MiddleLeft;
        mainLayout.Controls.Add(lblOperationStatus,0,6);
        AutoScaleDimensions = new SizeF(96F,96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        Font = new Font("Segoe UI",9F);
        RightToLeft = RightToLeft.Yes;
        Name = "UcInventoryCustodyIssue";
        Text = "إذن صرف أمانات";
        Tag = "INV:CUSTODY-ISSUE";
        Size = new Size(1180,720);
        MinimumSize = new Size(1000,600);
        Dock = DockStyle.Fill;
        Controls.Add(mainLayout);
        ResumeLayout(false);
    }
}
