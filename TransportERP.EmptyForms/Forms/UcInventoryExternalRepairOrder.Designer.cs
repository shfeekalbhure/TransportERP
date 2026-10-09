using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;
partial class UcInventoryExternalRepairOrder
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
    private Label lblOrderNumber = null!;
    private TextBox fieldOrderNumber = null!;
    private Label lblOrderDate = null!;
    private DateTimePicker fieldOrderDate = null!;
    private Label lblReference = null!;
    private TextBox fieldReference = null!;
    private Label lblWarehouse = null!;
    private ComboBox fieldWarehouse = null!;
    private Label lblDescription = null!;
    private TextBox fieldDescription = null!;
    private DataGridViewTextBoxColumn colSequence = null!;
    private DataGridViewTextBoxColumn colItemNumber = null!;
    private DataGridViewTextBoxColumn colItemName = null!;
    private DataGridViewTextBoxColumn colUnit = null!;
    private DataGridViewTextBoxColumn colQuantity = null!;
    private DataGridViewTextBoxColumn colCost = null!;
    private DataGridViewTextBoxColumn colReason = null!;
    private DataGridViewTextBoxColumn colNotes = null!;
    private DataGridViewTextBoxColumn colDepartureDate = null!;
    private DataGridViewTextBoxColumn colReturnDate = null!;
    private void InitializeComponent()
    {
        SuspendLayout();
        mainLayout = new TableLayoutPanel();
        mainLayout.Name = "mainLayout";
        mainLayout.Dock = DockStyle.Fill;
        mainLayout.Margin = Padding.Empty;
        mainLayout.ColumnCount = 1;
        mainLayout.RowCount = 6;
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,100F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute,26F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute,34F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute,140F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent,100F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute,80F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute,28F));
        lblTitle = new Label();
        lblTitle.Name="lblTitle";
        lblTitle.Text="أمر إصلاح خارجي";
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
        headerLayout.RowCount=4;
        headerLayout.Padding=new Padding(16,6,16,6);
        headerLayout.Margin=Padding.Empty;
        headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,85F));
        headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,33.333F));
        headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,85F));
        headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,33.333F));
        headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,85F));
        headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,33.333F));
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
        headerLayout.SetColumnSpan(fieldBranch,3);
        fieldBranch.DropDownStyle = ComboBoxStyle.DropDownList;
        fieldBranch.Enabled = false;
        fieldBranch.BackColor = Color.LightYellow;
        lblOrderNumber = new Label();
        lblOrderNumber.Name="lblOrderNumber";
        lblOrderNumber.Text="رقم الأمر";
        lblOrderNumber.Dock=DockStyle.Fill;
        lblOrderNumber.TextAlign=ContentAlignment.MiddleLeft;
        fieldOrderNumber = new TextBox();
        fieldOrderNumber.Name="fieldOrderNumber";
        fieldOrderNumber.AccessibleName="رقم الأمر";
        fieldOrderNumber.Dock=DockStyle.Fill;
        fieldOrderNumber.TabStop=false;
        headerLayout.Controls.Add(lblOrderNumber,0,1);
        headerLayout.Controls.Add(fieldOrderNumber,1,1);
        headerLayout.SetColumnSpan(fieldOrderNumber,1);
        fieldOrderNumber.ReadOnly = true;
        lblOrderDate = new Label();
        lblOrderDate.Name="lblOrderDate";
        lblOrderDate.Text="تاريخ الأمر";
        lblOrderDate.Dock=DockStyle.Fill;
        lblOrderDate.TextAlign=ContentAlignment.MiddleLeft;
        fieldOrderDate = new DateTimePicker();
        fieldOrderDate.Name="fieldOrderDate";
        fieldOrderDate.AccessibleName="تاريخ الأمر";
        fieldOrderDate.Dock=DockStyle.Fill;
        fieldOrderDate.TabStop=true;
        headerLayout.Controls.Add(lblOrderDate,2,1);
        headerLayout.Controls.Add(fieldOrderDate,3,1);
        headerLayout.SetColumnSpan(fieldOrderDate,1);
        fieldOrderDate.Format = DateTimePickerFormat.Custom;
        fieldOrderDate.CustomFormat = "yyyy-MM-dd";
        fieldOrderDate.ShowCheckBox = true;
        fieldOrderDate.Checked = false;
        fieldOrderDate.Enabled = true;
        lblReference = new Label();
        lblReference.Name="lblReference";
        lblReference.Text="رقم المرجع";
        lblReference.Dock=DockStyle.Fill;
        lblReference.TextAlign=ContentAlignment.MiddleLeft;
        fieldReference = new TextBox();
        fieldReference.Name="fieldReference";
        fieldReference.AccessibleName="رقم المرجع";
        fieldReference.Dock=DockStyle.Fill;
        fieldReference.TabStop=true;
        headerLayout.Controls.Add(lblReference,4,1);
        headerLayout.Controls.Add(fieldReference,5,1);
        headerLayout.SetColumnSpan(fieldReference,1);
        fieldReference.ReadOnly = false;
        lblWarehouse = new Label();
        lblWarehouse.Name="lblWarehouse";
        lblWarehouse.Text="المخزن";
        lblWarehouse.Dock=DockStyle.Fill;
        lblWarehouse.TextAlign=ContentAlignment.MiddleLeft;
        fieldWarehouse = new ComboBox();
        fieldWarehouse.Name="fieldWarehouse";
        fieldWarehouse.AccessibleName="المخزن";
        fieldWarehouse.Dock=DockStyle.Fill;
        fieldWarehouse.TabStop=true;
        headerLayout.Controls.Add(lblWarehouse,0,2);
        headerLayout.Controls.Add(fieldWarehouse,1,2);
        headerLayout.SetColumnSpan(fieldWarehouse,3);
        fieldWarehouse.DropDownStyle = ComboBoxStyle.DropDownList;
        fieldWarehouse.Enabled = true;
        fieldWarehouse.BackColor = Color.LightYellow;
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
        headerLayout.Controls.Add(lblDescription,0,3);
        headerLayout.Controls.Add(fieldDescription,1,3);
        headerLayout.SetColumnSpan(fieldDescription,5);
        fieldDescription.ReadOnly = false;
        mainLayout.Controls.Add(headerLayout,0,2);
        gridItems = new DataGridView();
        gridItems.Name="gridItems";
        gridItems.Dock=DockStyle.Fill;
        gridItems.AutoGenerateColumns=false;
        gridItems.ReadOnly=false;
        gridItems.AllowUserToAddRows=true;
        gridItems.AllowUserToDeleteRows=true;
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
        colItemNumber.ReadOnly=false;
        colItemNumber.Width=110;
        colItemNumber.FillWeight=110;
        colItemNumber.MinimumWidth=50;
        colItemNumber.SortMode=DataGridViewColumnSortMode.NotSortable;
        gridItems.Columns.Add(colItemNumber);
        colItemNumber.DefaultCellStyle.BackColor = Color.LightYellow;
        colItemName = new DataGridViewTextBoxColumn();
        colItemName.Name="ItemName";
        colItemName.HeaderText="اسم الصنف";
        colItemName.ReadOnly=true;
        colItemName.Width=175;
        colItemName.FillWeight=175;
        colItemName.MinimumWidth=50;
        colItemName.SortMode=DataGridViewColumnSortMode.NotSortable;
        gridItems.Columns.Add(colItemName);
        colItemName.DefaultCellStyle.BackColor = Color.WhiteSmoke;
        colUnit = new DataGridViewTextBoxColumn();
        colUnit.Name="Unit";
        colUnit.HeaderText="الوحدة";
        colUnit.ReadOnly=true;
        colUnit.Width=65;
        colUnit.FillWeight=65;
        colUnit.MinimumWidth=50;
        colUnit.SortMode=DataGridViewColumnSortMode.NotSortable;
        gridItems.Columns.Add(colUnit);
        colUnit.DefaultCellStyle.BackColor = Color.WhiteSmoke;
        colQuantity = new DataGridViewTextBoxColumn();
        colQuantity.Name="Quantity";
        colQuantity.HeaderText="الكمية";
        colQuantity.ReadOnly=false;
        colQuantity.ValueType=typeof(decimal);
        colQuantity.Width=75;
        colQuantity.FillWeight=75;
        colQuantity.MinimumWidth=50;
        colQuantity.SortMode=DataGridViewColumnSortMode.NotSortable;
        gridItems.Columns.Add(colQuantity);
        colQuantity.DefaultCellStyle.BackColor = Color.LightYellow;
        colCost = new DataGridViewTextBoxColumn();
        colCost.Name="Cost";
        colCost.HeaderText="التكلفة";
        colCost.ReadOnly=true;
        colCost.Width=85;
        colCost.FillWeight=85;
        colCost.MinimumWidth=50;
        colCost.SortMode=DataGridViewColumnSortMode.NotSortable;
        gridItems.Columns.Add(colCost);
        colCost.DefaultCellStyle.BackColor = Color.WhiteSmoke;
        colReason = new DataGridViewTextBoxColumn();
        colReason.Name="Reason";
        colReason.HeaderText="السبب";
        colReason.ReadOnly=false;
        colReason.Width=120;
        colReason.FillWeight=120;
        colReason.MinimumWidth=50;
        colReason.SortMode=DataGridViewColumnSortMode.NotSortable;
        gridItems.Columns.Add(colReason);
        colReason.DefaultCellStyle.BackColor = Color.WhiteSmoke;
        colNotes = new DataGridViewTextBoxColumn();
        colNotes.Name="Notes";
        colNotes.HeaderText="ملاحظات";
        colNotes.ReadOnly=false;
        colNotes.Width=120;
        colNotes.FillWeight=120;
        colNotes.MinimumWidth=50;
        colNotes.SortMode=DataGridViewColumnSortMode.NotSortable;
        gridItems.Columns.Add(colNotes);
        colNotes.DefaultCellStyle.BackColor = Color.WhiteSmoke;
        colDepartureDate = new DataGridViewTextBoxColumn();
        colDepartureDate.Name="DepartureDate";
        colDepartureDate.HeaderText="تاريخ الخروج";
        colDepartureDate.ReadOnly=false;
        colDepartureDate.Width=90;
        colDepartureDate.FillWeight=90;
        colDepartureDate.MinimumWidth=50;
        colDepartureDate.SortMode=DataGridViewColumnSortMode.NotSortable;
        gridItems.Columns.Add(colDepartureDate);
        colDepartureDate.DefaultCellStyle.BackColor = Color.LightCyan;
        colReturnDate = new DataGridViewTextBoxColumn();
        colReturnDate.Name="ReturnDate";
        colReturnDate.HeaderText="تاريخ الإرجاع";
        colReturnDate.ReadOnly=true;
        colReturnDate.Width=90;
        colReturnDate.FillWeight=90;
        colReturnDate.MinimumWidth=50;
        colReturnDate.SortMode=DataGridViewColumnSortMode.NotSortable;
        gridItems.Columns.Add(colReturnDate);
        colReturnDate.DefaultCellStyle.BackColor = Color.LightCyan;
        mainLayout.Controls.Add(gridItems,0,3);
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
        mainLayout.Controls.Add(auditFooter,0,4);
        lblOperationStatus = new Label();
        lblOperationStatus.Name="lblOperationStatus";
        lblOperationStatus.Text="لم يتم ربط خدمة أوامر الإصلاح الخارجي بعد.";
        lblOperationStatus.Dock=DockStyle.Fill;
        lblOperationStatus.TextAlign=ContentAlignment.MiddleLeft;
        mainLayout.Controls.Add(lblOperationStatus,0,5);
        AutoScaleDimensions = new SizeF(96F,96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        Font = new Font("Segoe UI",9F);
        RightToLeft = RightToLeft.Yes;
        Name = "UcInventoryExternalRepairOrder";
        Text = "أمر إصلاح خارجي";
        Tag = "INV:EXTERNAL-REPAIR";
        Size = new Size(1180,720);
        MinimumSize = new Size(1000,600);
        Dock = DockStyle.Fill;
        Controls.Add(mainLayout);
        ResumeLayout(false);
    }
}
