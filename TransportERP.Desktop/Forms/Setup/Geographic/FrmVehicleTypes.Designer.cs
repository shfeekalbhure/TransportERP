namespace TransportERP.Desktop.Forms.Setup.General;

partial class FrmVehicleTypes
{
    private System.ComponentModel.IContainer? components = null;
    private System.Windows.Forms.TableLayoutPanel tlpRoot;
    private System.Windows.Forms.Panel pnlScreenHeader;
    private System.Windows.Forms.Label lblScreenCode;
    private System.Windows.Forms.Label lblScreenTitle;
    private System.Windows.Forms.Label lblBreadcrumb;
    private System.Windows.Forms.FlowLayoutPanel flpToolbar;
    private System.Windows.Forms.TableLayoutPanel tlpMainData;
    
    private System.Windows.Forms.TableLayoutPanel tlpFunctionalScope;
    private System.Windows.Forms.TableLayoutPanel tlpSearchFilters;
    private System.Windows.Forms.TextBox txtSearch;
    private System.Windows.Forms.ComboBox cmbFilterStatus;
    private System.Windows.Forms.Button btnSearch;
    private System.Windows.Forms.Button btnClearFilters;
    private System.Windows.Forms.Panel pnlRecordsGrid;
    private System.Windows.Forms.DataGridView dgvRecords;
    private System.Windows.Forms.FlowLayoutPanel flpPagination;
    private System.Windows.Forms.Button btnFirst;
    private System.Windows.Forms.Button btnPrevious;
    private System.Windows.Forms.Label lblPageNumber;
    private System.Windows.Forms.Button btnNext;
    private System.Windows.Forms.Button btnLast;
    private System.Windows.Forms.ComboBox cmbPageSize;
    private System.Windows.Forms.Label lblTotalRecords;
    private System.Windows.Forms.TableLayoutPanel tlpAuditInfo;
    private System.Windows.Forms.FlowLayoutPanel flpUsageCounters;
    private System.Windows.Forms.Panel pnlEditCount;
    private System.Windows.Forms.Panel pnlPrintCount;
    
    private System.Windows.Forms.TextBox txtVehicleTypeCode;
    private System.Windows.Forms.TextBox txtNameAr;
    private System.Windows.Forms.TextBox txtNameEn;
    private System.Windows.Forms.ComboBox cmbCategory;
    private System.Windows.Forms.NumericUpDown nudSeats;
    private System.Windows.Forms.NumericUpDown nudPayloadTons;
    private System.Windows.Forms.NumericUpDown nudLength;
    private System.Windows.Forms.NumericUpDown nudWidth;
    private System.Windows.Forms.NumericUpDown nudHeight;
    private System.Windows.Forms.NumericUpDown nudAxles;
    private System.Windows.Forms.ComboBox cmbBodyType;
    private System.Windows.Forms.ComboBox cmbRoofType;
    private System.Windows.Forms.ComboBox cmbOwnershipType;
    private System.Windows.Forms.ComboBox cmbStatus;
    private System.Windows.Forms.RichTextBox rtbNotes;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        tlpRoot = new System.Windows.Forms.TableLayoutPanel();
        pnlScreenHeader = new System.Windows.Forms.Panel();
        lblScreenCode = new System.Windows.Forms.Label();
        lblScreenTitle = new System.Windows.Forms.Label();
        lblBreadcrumb = new System.Windows.Forms.Label();
        flpToolbar = new System.Windows.Forms.FlowLayoutPanel();
        tlpMainData = new System.Windows.Forms.TableLayoutPanel();
        tlpFunctionalScope = new System.Windows.Forms.TableLayoutPanel();
        tlpSearchFilters = new System.Windows.Forms.TableLayoutPanel();
        txtSearch = new System.Windows.Forms.TextBox();
        cmbFilterStatus = new System.Windows.Forms.ComboBox();
        btnSearch = new System.Windows.Forms.Button();
        btnClearFilters = new System.Windows.Forms.Button();
        pnlRecordsGrid = new System.Windows.Forms.Panel();
        dgvRecords = new System.Windows.Forms.DataGridView();
        flpPagination = new System.Windows.Forms.FlowLayoutPanel();
        btnFirst = new System.Windows.Forms.Button(); btnPrevious = new System.Windows.Forms.Button(); lblPageNumber = new System.Windows.Forms.Label(); btnNext = new System.Windows.Forms.Button(); btnLast = new System.Windows.Forms.Button(); cmbPageSize = new System.Windows.Forms.ComboBox(); lblTotalRecords = new System.Windows.Forms.Label();
        tlpAuditInfo = new System.Windows.Forms.TableLayoutPanel();
        flpUsageCounters = new System.Windows.Forms.FlowLayoutPanel();
        pnlEditCount = new System.Windows.Forms.Panel(); pnlPrintCount = new System.Windows.Forms.Panel();
        txtVehicleTypeCode = new System.Windows.Forms.TextBox();
            txtVehicleTypeCode.Name = "txtVehicleTypeCode"; txtVehicleTypeCode.Dock = System.Windows.Forms.DockStyle.Fill; txtVehicleTypeCode.RightToLeft = System.Windows.Forms.RightToLeft.Yes; txtVehicleTypeCode.Margin = new System.Windows.Forms.Padding(8, 4, 12, 4);
            txtNameAr = new System.Windows.Forms.TextBox();
            txtNameAr.Name = "txtNameAr"; txtNameAr.Dock = System.Windows.Forms.DockStyle.Fill; txtNameAr.RightToLeft = System.Windows.Forms.RightToLeft.Yes; txtNameAr.Margin = new System.Windows.Forms.Padding(8, 4, 12, 4);
            txtNameEn = new System.Windows.Forms.TextBox();
            txtNameEn.Name = "txtNameEn"; txtNameEn.Dock = System.Windows.Forms.DockStyle.Fill; txtNameEn.RightToLeft = System.Windows.Forms.RightToLeft.Yes; txtNameEn.Margin = new System.Windows.Forms.Padding(8, 4, 12, 4);
            cmbCategory = new System.Windows.Forms.ComboBox();
            cmbCategory.Name = "cmbCategory"; cmbCategory.Dock = System.Windows.Forms.DockStyle.Fill; cmbCategory.RightToLeft = System.Windows.Forms.RightToLeft.Yes; cmbCategory.Margin = new System.Windows.Forms.Padding(8, 4, 12, 4);
            nudSeats = new System.Windows.Forms.NumericUpDown();
            nudSeats.Name = "nudSeats"; nudSeats.Dock = System.Windows.Forms.DockStyle.Fill; nudSeats.RightToLeft = System.Windows.Forms.RightToLeft.Yes; nudSeats.Margin = new System.Windows.Forms.Padding(8, 4, 12, 4);
            nudSeats.DecimalPlaces = 2; nudSeats.Maximum = 100000000;
            nudPayloadTons = new System.Windows.Forms.NumericUpDown();
            nudPayloadTons.Name = "nudPayloadTons"; nudPayloadTons.Dock = System.Windows.Forms.DockStyle.Fill; nudPayloadTons.RightToLeft = System.Windows.Forms.RightToLeft.Yes; nudPayloadTons.Margin = new System.Windows.Forms.Padding(8, 4, 12, 4);
            nudPayloadTons.DecimalPlaces = 2; nudPayloadTons.Maximum = 100000000;
            nudLength = new System.Windows.Forms.NumericUpDown();
            nudLength.Name = "nudLength"; nudLength.Dock = System.Windows.Forms.DockStyle.Fill; nudLength.RightToLeft = System.Windows.Forms.RightToLeft.Yes; nudLength.Margin = new System.Windows.Forms.Padding(8, 4, 12, 4);
            nudLength.DecimalPlaces = 2; nudLength.Maximum = 100000000;
            nudWidth = new System.Windows.Forms.NumericUpDown();
            nudWidth.Name = "nudWidth"; nudWidth.Dock = System.Windows.Forms.DockStyle.Fill; nudWidth.RightToLeft = System.Windows.Forms.RightToLeft.Yes; nudWidth.Margin = new System.Windows.Forms.Padding(8, 4, 12, 4);
            nudWidth.DecimalPlaces = 2; nudWidth.Maximum = 100000000;
            nudHeight = new System.Windows.Forms.NumericUpDown();
            nudHeight.Name = "nudHeight"; nudHeight.Dock = System.Windows.Forms.DockStyle.Fill; nudHeight.RightToLeft = System.Windows.Forms.RightToLeft.Yes; nudHeight.Margin = new System.Windows.Forms.Padding(8, 4, 12, 4);
            nudHeight.DecimalPlaces = 2; nudHeight.Maximum = 100000000;
            nudAxles = new System.Windows.Forms.NumericUpDown();
            nudAxles.Name = "nudAxles"; nudAxles.Dock = System.Windows.Forms.DockStyle.Fill; nudAxles.RightToLeft = System.Windows.Forms.RightToLeft.Yes; nudAxles.Margin = new System.Windows.Forms.Padding(8, 4, 12, 4);
            nudAxles.DecimalPlaces = 2; nudAxles.Maximum = 100000000;
            cmbBodyType = new System.Windows.Forms.ComboBox();
            cmbBodyType.Name = "cmbBodyType"; cmbBodyType.Dock = System.Windows.Forms.DockStyle.Fill; cmbBodyType.RightToLeft = System.Windows.Forms.RightToLeft.Yes; cmbBodyType.Margin = new System.Windows.Forms.Padding(8, 4, 12, 4);
            cmbRoofType = new System.Windows.Forms.ComboBox();
            cmbRoofType.Name = "cmbRoofType"; cmbRoofType.Dock = System.Windows.Forms.DockStyle.Fill; cmbRoofType.RightToLeft = System.Windows.Forms.RightToLeft.Yes; cmbRoofType.Margin = new System.Windows.Forms.Padding(8, 4, 12, 4);
            cmbOwnershipType = new System.Windows.Forms.ComboBox();
            cmbOwnershipType.Name = "cmbOwnershipType"; cmbOwnershipType.Dock = System.Windows.Forms.DockStyle.Fill; cmbOwnershipType.RightToLeft = System.Windows.Forms.RightToLeft.Yes; cmbOwnershipType.Margin = new System.Windows.Forms.Padding(8, 4, 12, 4);
            cmbStatus = new System.Windows.Forms.ComboBox();
            cmbStatus.Name = "cmbStatus"; cmbStatus.Dock = System.Windows.Forms.DockStyle.Fill; cmbStatus.RightToLeft = System.Windows.Forms.RightToLeft.Yes; cmbStatus.Margin = new System.Windows.Forms.Padding(8, 4, 12, 4);
            rtbNotes = new System.Windows.Forms.RichTextBox();
            rtbNotes.Name = "rtbNotes"; rtbNotes.Dock = System.Windows.Forms.DockStyle.Fill; rtbNotes.RightToLeft = System.Windows.Forms.RightToLeft.Yes; rtbNotes.Margin = new System.Windows.Forms.Padding(8, 4, 12, 4);
            rtbNotes.Height = 54;
        
        SuspendLayout();
        tlpRoot.Name="tlpRoot"; tlpRoot.Dock=System.Windows.Forms.DockStyle.Fill; tlpRoot.ColumnCount=1; tlpRoot.RowCount=10; tlpRoot.RightToLeft=System.Windows.Forms.RightToLeft.Yes; tlpRoot.BackColor=System.Drawing.Color.FromArgb(249,250,251); tlpRoot.Padding=new System.Windows.Forms.Padding(16);
        tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute,72));
        tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute,56));
        tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute,330));
        tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute,0));
        tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute,70));
        tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent,100));
        tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute,48));
        tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute,66));
        tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute,44));
        pnlScreenHeader.Name="pnlScreenHeader"; pnlScreenHeader.Dock=System.Windows.Forms.DockStyle.Fill; pnlScreenHeader.BackColor=System.Drawing.Color.White; pnlScreenHeader.Padding=new System.Windows.Forms.Padding(16,8,16,8); pnlScreenHeader.RightToLeft=System.Windows.Forms.RightToLeft.Yes;
        lblScreenCode.Name="lblScreenCode"; lblScreenCode.Text="GEN-008"; lblScreenCode.AutoSize=true; lblScreenCode.Font=new System.Drawing.Font("Segoe UI",11F,System.Drawing.FontStyle.Bold); lblScreenCode.ForeColor=System.Drawing.Color.FromArgb(46,116,181); lblScreenCode.Location=new System.Drawing.Point(0,8);
        lblScreenTitle.Name="lblScreenTitle"; lblScreenTitle.Text="أنواع المركبات"; lblScreenTitle.AutoSize=true; lblScreenTitle.Font=new System.Drawing.Font("Segoe UI",20F,System.Drawing.FontStyle.Bold); lblScreenTitle.ForeColor=System.Drawing.Color.FromArgb(31,41,55); lblScreenTitle.Location=new System.Drawing.Point(0,27);
        lblBreadcrumb.Name="lblBreadcrumb"; lblBreadcrumb.Text="التهيئة العامة / أنواع المركبات"; lblBreadcrumb.AutoSize=true; lblBreadcrumb.Font=new System.Drawing.Font("Segoe UI",11F); lblBreadcrumb.ForeColor=System.Drawing.Color.Gray; lblBreadcrumb.Location=new System.Drawing.Point(0,49);
        pnlScreenHeader.Controls.Add(lblScreenCode); pnlScreenHeader.Controls.Add(lblScreenTitle); pnlScreenHeader.Controls.Add(lblBreadcrumb);
        flpToolbar.Name="flpToolbar"; flpToolbar.Dock=System.Windows.Forms.DockStyle.Fill; flpToolbar.RightToLeft=System.Windows.Forms.RightToLeft.Yes; flpToolbar.FlowDirection=System.Windows.Forms.FlowDirection.RightToLeft; flpToolbar.WrapContents=false; flpToolbar.Padding=new System.Windows.Forms.Padding(8,10,8,10); flpToolbar.BackColor=System.Drawing.Color.White;
        AddToolbarButton("btnNew","جديد"); AddToolbarButton("btnSave","حفظ"); AddToolbarButton("btnEdit","تعديل"); AddToolbarButton("btnStop","إيقاف"); AddToolbarButton("btnDelete","حذف"); AddToolbarButton("btnPrint","طباعة"); AddToolbarButton("btnExport","تصدير"); AddToolbarButton("btnCancel","إلغاء"); AddToolbarButton("btnMore","المزيد");
        tlpMainData.Name="tlpMainData"; tlpMainData.Dock=System.Windows.Forms.DockStyle.Top; tlpMainData.RightToLeft=System.Windows.Forms.RightToLeft.Yes; tlpMainData.ColumnCount=4; tlpMainData.RowCount=8; tlpMainData.Padding=new System.Windows.Forms.Padding(16); tlpMainData.BackColor=System.Drawing.Color.White; tlpMainData.AutoScroll=true;
        tlpMainData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent,15)); tlpMainData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent,35)); tlpMainData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent,15)); tlpMainData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent,35));
        for(int i=0;i<8;i++) tlpMainData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute,44));
        AddField(tlpMainData, "كود النوع", txtVehicleTypeCode, 0);
            AddField(tlpMainData, "الاسم العربي", txtNameAr, 1);
            AddField(tlpMainData, "الاسم الإنجليزي", txtNameEn, 2);
            AddField(tlpMainData, "الفئة", cmbCategory, 3);
            AddField(tlpMainData, "عدد المقاعد", nudSeats, 4);
            AddField(tlpMainData, "الحمولة بالطن", nudPayloadTons, 5);
            AddField(tlpMainData, "الطول", nudLength, 6);
            AddField(tlpMainData, "العرض", nudWidth, 7);
            AddField(tlpMainData, "الارتفاع", nudHeight, 8);
            AddField(tlpMainData, "عدد المحاور", nudAxles, 9);
            AddField(tlpMainData, "نوع الهيكل", cmbBodyType, 10);
            AddField(tlpMainData, "نوع السطح", cmbRoofType, 11);
            AddField(tlpMainData, "نوع الملكية", cmbOwnershipType, 12);
            AddField(tlpMainData, "الحالة", cmbStatus, 13);
            AddField(tlpMainData, "الملاحظات", rtbNotes, 14);
        tlpFunctionalScope.Name="tlpFunctionalScope"; tlpFunctionalScope.Dock=System.Windows.Forms.DockStyle.Fill; tlpFunctionalScope.RightToLeft=System.Windows.Forms.RightToLeft.Yes; tlpFunctionalScope.ColumnCount=1; tlpFunctionalScope.Padding=new System.Windows.Forms.Padding(16,8,16,8); tlpFunctionalScope.BackColor=System.Drawing.Color.White; 
        
        tlpSearchFilters.Name="tlpSearchFilters"; tlpSearchFilters.Dock=System.Windows.Forms.DockStyle.Fill; tlpSearchFilters.RightToLeft=System.Windows.Forms.RightToLeft.Yes; tlpSearchFilters.ColumnCount=4; tlpSearchFilters.Padding=new System.Windows.Forms.Padding(16); tlpSearchFilters.BackColor=System.Drawing.Color.White;
        txtSearch.Name="txtSearch"; txtSearch.PlaceholderText="بحث"; txtSearch.Dock=System.Windows.Forms.DockStyle.Fill; txtSearch.RightToLeft=System.Windows.Forms.RightToLeft.Yes;
        cmbFilterStatus.Name="cmbFilterStatus"; cmbFilterStatus.Dock=System.Windows.Forms.DockStyle.Fill; cmbFilterStatus.RightToLeft=System.Windows.Forms.RightToLeft.Yes;
        ConfigureButton(btnSearch,"btnSearch","بحث"); ConfigureButton(btnClearFilters,"btnClearFilters","مسح");
        tlpSearchFilters.Controls.Add(txtSearch,0,0); tlpSearchFilters.Controls.Add(cmbFilterStatus,1,0); tlpSearchFilters.Controls.Add(btnSearch,2,0); tlpSearchFilters.Controls.Add(btnClearFilters,3,0);
        pnlRecordsGrid.Name="pnlRecordsGrid"; pnlRecordsGrid.Dock=System.Windows.Forms.DockStyle.Fill; pnlRecordsGrid.BackColor=System.Drawing.Color.White; pnlRecordsGrid.RightToLeft=System.Windows.Forms.RightToLeft.Yes;
        dgvRecords.Name="dgvRecords"; dgvRecords.Dock=System.Windows.Forms.DockStyle.Fill; dgvRecords.RightToLeft=System.Windows.Forms.RightToLeft.Yes; dgvRecords.SelectionMode=System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect; dgvRecords.MultiSelect=false; dgvRecords.AllowUserToAddRows=false; dgvRecords.AllowUserToDeleteRows=false; dgvRecords.AllowUserToResizeRows=false; dgvRecords.AutoGenerateColumns=false; dgvRecords.BackgroundColor=System.Drawing.Color.White;
        dgvRecords.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "col1", HeaderText = "كود النوع", ReadOnly = true });
        dgvRecords.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "col2", HeaderText = "الاسم العربي", ReadOnly = true });
        dgvRecords.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "col3", HeaderText = "الفئة", ReadOnly = true });
        dgvRecords.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "col4", HeaderText = "المقاعد", ReadOnly = true });
        dgvRecords.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "col5", HeaderText = "الحمولة", ReadOnly = true });
        dgvRecords.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "col6", HeaderText = "الحالة", ReadOnly = true });
        pnlRecordsGrid.Controls.Add(dgvRecords);
        flpPagination.Name="flpPagination"; flpPagination.Dock=System.Windows.Forms.DockStyle.Fill; flpPagination.RightToLeft=System.Windows.Forms.RightToLeft.Yes; flpPagination.FlowDirection=System.Windows.Forms.FlowDirection.RightToLeft; flpPagination.WrapContents=false; flpPagination.Padding=new System.Windows.Forms.Padding(8); flpPagination.BackColor=System.Drawing.Color.White;
        ConfigureButton(btnFirst,"btnFirst","الأول"); ConfigureButton(btnPrevious,"btnPrevious","السابق"); lblPageNumber.Name="lblPageNumber"; lblPageNumber.Text="1"; lblPageNumber.AutoSize=false; lblPageNumber.Size=new System.Drawing.Size(52,36); lblPageNumber.TextAlign=System.Drawing.ContentAlignment.MiddleCenter; ConfigureButton(btnNext,"btnNext","التالي"); ConfigureButton(btnLast,"btnLast","الأخير"); cmbPageSize.Name="cmbPageSize"; cmbPageSize.Size=new System.Drawing.Size(90,36); lblTotalRecords.Name="lblTotalRecords"; lblTotalRecords.Text="إجمالي السجلات: 0"; lblTotalRecords.AutoSize=true; flpPagination.Controls.AddRange(new System.Windows.Forms.Control[]{btnFirst,btnPrevious,lblPageNumber,btnNext,btnLast,cmbPageSize,lblTotalRecords});
        tlpAuditInfo.Name="tlpAuditInfo"; tlpAuditInfo.Dock=System.Windows.Forms.DockStyle.Fill; tlpAuditInfo.RightToLeft=System.Windows.Forms.RightToLeft.Yes; tlpAuditInfo.ColumnCount=4; tlpAuditInfo.BackColor=System.Drawing.Color.FromArgb(243,244,246); tlpAuditInfo.Padding=new System.Windows.Forms.Padding(16);
        AddAuditField("أنشئ بواسطة","—",0); AddAuditField("تاريخ الإنشاء","—",1); AddAuditField("آخر تعديل بواسطة","—",2); AddAuditField("تاريخ التعديل","—",3);
        flpUsageCounters.Name="flpUsageCounters"; flpUsageCounters.Dock=System.Windows.Forms.DockStyle.Fill; flpUsageCounters.RightToLeft=System.Windows.Forms.RightToLeft.Yes; flpUsageCounters.FlowDirection=System.Windows.Forms.FlowDirection.RightToLeft; flpUsageCounters.Padding=new System.Windows.Forms.Padding(8); flpUsageCounters.BackColor=System.Drawing.Color.White; pnlEditCount.Name="pnlEditCount"; pnlEditCount.Size=new System.Drawing.Size(150,28); pnlEditCount.BorderStyle=System.Windows.Forms.BorderStyle.FixedSingle; pnlEditCount.Controls.Add(new System.Windows.Forms.Label { Text="عدد مرات التعديل: 0", Dock=System.Windows.Forms.DockStyle.Fill, TextAlign=System.Drawing.ContentAlignment.MiddleCenter }); pnlPrintCount.Name="pnlPrintCount"; pnlPrintCount.Size=new System.Drawing.Size(150,28); pnlPrintCount.BorderStyle=System.Windows.Forms.BorderStyle.FixedSingle; pnlPrintCount.Controls.Add(new System.Windows.Forms.Label { Text="عدد مرات الطباعة: 0", Dock=System.Windows.Forms.DockStyle.Fill, TextAlign=System.Drawing.ContentAlignment.MiddleCenter }); flpUsageCounters.Controls.Add(pnlEditCount); flpUsageCounters.Controls.Add(pnlPrintCount);
        
        tlpRoot.Controls.Add(pnlScreenHeader,0,0); tlpRoot.Controls.Add(flpToolbar,0,1); tlpRoot.Controls.Add(tlpMainData,0,2); tlpRoot.Controls.Add(tlpFunctionalScope,0,3); tlpRoot.Controls.Add(tlpSearchFilters,0,4); tlpRoot.Controls.Add(pnlRecordsGrid,0,5); tlpRoot.Controls.Add(flpPagination,0,6); tlpRoot.Controls.Add(tlpAuditInfo,0,7); tlpRoot.Controls.Add(flpUsageCounters,0,8);
        AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font; BackColor=System.Drawing.Color.FromArgb(249,250,251); ClientSize=new System.Drawing.Size(1280,900); Controls.Add(tlpRoot); Font=new System.Drawing.Font("Segoe UI",10F); MinimumSize=new System.Drawing.Size(1180,780); Name="FrmVehicleTypes"; RightToLeft=System.Windows.Forms.RightToLeft.Yes; RightToLeftLayout=true; StartPosition=System.Windows.Forms.FormStartPosition.CenterScreen; Text="GEN-008 — أنواع المركبات";
        ResumeLayout(false);
    }
    private void AddToolbarButton(string name, string text) { var button=new System.Windows.Forms.Button(); ConfigureButton(button,name,text); flpToolbar.Controls.Add(button); }
    private static void ConfigureButton(System.Windows.Forms.Button button,string name,string text) { button.Name=name; button.Text=text; button.Size=new System.Drawing.Size(82,36); button.FlatStyle=System.Windows.Forms.FlatStyle.Flat; button.Margin=new System.Windows.Forms.Padding(4,0,4,0); button.RightToLeft=System.Windows.Forms.RightToLeft.Yes; }
    private static void AddField(System.Windows.Forms.TableLayoutPanel panel,string title,System.Windows.Forms.Control control,int index) { int row=index/2, column=(index%2)*2; var label=new System.Windows.Forms.Label { Name="lbl"+control.Name.Substring(3)+"Title", Text=title, Dock=System.Windows.Forms.DockStyle.Fill, TextAlign=System.Drawing.ContentAlignment.MiddleRight, Font=new System.Drawing.Font("Segoe UI",10F,System.Drawing.FontStyle.Bold) }; panel.Controls.Add(label,column,row); panel.Controls.Add(control,column+1,row); }
    private static void AddScopeField(System.Windows.Forms.TableLayoutPanel panel,string title,System.Windows.Forms.Control control,int index) { var label=new System.Windows.Forms.Label { Text=title, Dock=System.Windows.Forms.DockStyle.Fill, TextAlign=System.Drawing.ContentAlignment.MiddleRight }; panel.Controls.Add(label,index*2,0); panel.Controls.Add(control,index*2+1,0); }
    private void AddAuditField(string title,string value,int column) { var label=new System.Windows.Forms.Label { Text=title+"\r\n"+value, Dock=System.Windows.Forms.DockStyle.Fill, TextAlign=System.Drawing.ContentAlignment.MiddleRight, Font=new System.Drawing.Font("Segoe UI",9F) }; tlpAuditInfo.Controls.Add(label,column,0); }
}