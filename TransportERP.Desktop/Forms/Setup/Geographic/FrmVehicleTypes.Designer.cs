namespace TransportERP.Desktop.Forms.Setup.General;

partial class FrmVehicleTypes
{
    private System.ComponentModel.IContainer? components = null;
    private System.Windows.Forms.TableLayoutPanel tlpRoot = null!;
    private System.Windows.Forms.Panel pnlC002Header = null!;
    private System.Windows.Forms.Label lblScreenCode = null!;
    private System.Windows.Forms.Label lblScreenTitle = null!;
    private System.Windows.Forms.Label lblBreadcrumb = null!;
    private System.Windows.Forms.FlowLayoutPanel flpC003Toolbar = null!;
    private System.Windows.Forms.Button btnNew = null!, btnSave = null!, btnEdit = null!, btnStop = null!, btnDelete = null!, btnPrint = null!, btnExport = null!, btnCancel = null!, btnMore = null!;
    private System.Windows.Forms.TableLayoutPanel tlpC005Data = null!;
    private System.Windows.Forms.TextBox txtVehicleTypeCode = null!, txtNameAr = null!, txtNameEn = null!;
    private System.Windows.Forms.ComboBox cmbCategory = null!, cmbBodyType = null!, cmbRoofType = null!, cmbOwnershipType = null!, cmbStatus = null!;
    private System.Windows.Forms.NumericUpDown nudSeats = null!, nudPayloadTons = null!, nudLength = null!, nudWidth = null!, nudHeight = null!, nudAxles = null!;
    private System.Windows.Forms.RichTextBox rtbNotes = null!;
    private System.Windows.Forms.TableLayoutPanel tlpC007Search = null!;
    private System.Windows.Forms.TextBox txtSearch = null!;
    private System.Windows.Forms.ComboBox cmbFilterStatus = null!, cmbFilterCategory = null!;
    private System.Windows.Forms.Button btnSearch = null!, btnClearFilters = null!;
    private System.Windows.Forms.Panel pnlC008Grid = null!;
    private System.Windows.Forms.DataGridView dgvRecords = null!;
    private System.Windows.Forms.DataGridViewTextBoxColumn colCode = null!, colNameAr = null!, colCategory = null!, colSeats = null!, colPayload = null!, colStatus = null!;
    private System.Windows.Forms.FlowLayoutPanel flpC010Paging = null!;
    private System.Windows.Forms.Button btnFirst = null!, btnPrevious = null!, btnNext = null!, btnLast = null!;
    private System.Windows.Forms.Label lblPageNumber = null!, lblTotalRecords = null!;
    private System.Windows.Forms.ComboBox cmbPageSize = null!;
    private System.Windows.Forms.TableLayoutPanel tlpC011Audit = null!;
    private System.Windows.Forms.Button btnAuditLog = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        tlpRoot = new System.Windows.Forms.TableLayoutPanel();
        pnlC002Header = new System.Windows.Forms.Panel();
        lblScreenCode = new System.Windows.Forms.Label();
        lblScreenTitle = new System.Windows.Forms.Label();
        lblBreadcrumb = new System.Windows.Forms.Label();
        flpC003Toolbar = new System.Windows.Forms.FlowLayoutPanel();
        btnNew = new System.Windows.Forms.Button(); btnSave = new System.Windows.Forms.Button(); btnEdit = new System.Windows.Forms.Button(); btnStop = new System.Windows.Forms.Button(); btnDelete = new System.Windows.Forms.Button(); btnPrint = new System.Windows.Forms.Button(); btnExport = new System.Windows.Forms.Button(); btnCancel = new System.Windows.Forms.Button(); btnMore = new System.Windows.Forms.Button();
        tlpC005Data = new System.Windows.Forms.TableLayoutPanel();
        txtVehicleTypeCode = new System.Windows.Forms.TextBox(); txtNameAr = new System.Windows.Forms.TextBox(); txtNameEn = new System.Windows.Forms.TextBox();
        cmbCategory = new System.Windows.Forms.ComboBox(); cmbBodyType = new System.Windows.Forms.ComboBox(); cmbRoofType = new System.Windows.Forms.ComboBox(); cmbOwnershipType = new System.Windows.Forms.ComboBox(); cmbStatus = new System.Windows.Forms.ComboBox();
        nudSeats = new System.Windows.Forms.NumericUpDown(); nudPayloadTons = new System.Windows.Forms.NumericUpDown(); nudLength = new System.Windows.Forms.NumericUpDown(); nudWidth = new System.Windows.Forms.NumericUpDown(); nudHeight = new System.Windows.Forms.NumericUpDown(); nudAxles = new System.Windows.Forms.NumericUpDown();
        rtbNotes = new System.Windows.Forms.RichTextBox();
        tlpC007Search = new System.Windows.Forms.TableLayoutPanel(); txtSearch = new System.Windows.Forms.TextBox(); cmbFilterStatus = new System.Windows.Forms.ComboBox(); cmbFilterCategory = new System.Windows.Forms.ComboBox(); btnSearch = new System.Windows.Forms.Button(); btnClearFilters = new System.Windows.Forms.Button();
        pnlC008Grid = new System.Windows.Forms.Panel(); dgvRecords = new System.Windows.Forms.DataGridView();
        colCode = new System.Windows.Forms.DataGridViewTextBoxColumn(); colNameAr = new System.Windows.Forms.DataGridViewTextBoxColumn(); colCategory = new System.Windows.Forms.DataGridViewTextBoxColumn(); colSeats = new System.Windows.Forms.DataGridViewTextBoxColumn(); colPayload = new System.Windows.Forms.DataGridViewTextBoxColumn(); colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
        flpC010Paging = new System.Windows.Forms.FlowLayoutPanel(); btnFirst = new System.Windows.Forms.Button(); btnPrevious = new System.Windows.Forms.Button(); lblPageNumber = new System.Windows.Forms.Label(); btnNext = new System.Windows.Forms.Button(); btnLast = new System.Windows.Forms.Button(); cmbPageSize = new System.Windows.Forms.ComboBox(); lblTotalRecords = new System.Windows.Forms.Label();
        tlpC011Audit = new System.Windows.Forms.TableLayoutPanel(); btnAuditLog = new System.Windows.Forms.Button();

        SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudSeats).BeginInit(); ((System.ComponentModel.ISupportInitialize)nudPayloadTons).BeginInit(); ((System.ComponentModel.ISupportInitialize)nudLength).BeginInit(); ((System.ComponentModel.ISupportInitialize)nudWidth).BeginInit(); ((System.ComponentModel.ISupportInitialize)nudHeight).BeginInit(); ((System.ComponentModel.ISupportInitialize)nudAxles).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvRecords).BeginInit();

        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        AutoScroll = false;
        BackColor = System.Drawing.Color.FromArgb(249, 250, 251);
        ClientSize = new System.Drawing.Size(1280, 900);
        Font = new System.Drawing.Font("Arial", 11F);
        ForeColor = System.Drawing.Color.FromArgb(31, 41, 55);
        KeyPreview = true;
        MinimumSize = new System.Drawing.Size(1180, 780);
        Name = "FrmVehicleTypes";
        RightToLeft = System.Windows.Forms.RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        Text = "GEN-008 — أنواع المركبات";

        tlpRoot.Name = "tlpRoot"; tlpRoot.Dock = System.Windows.Forms.DockStyle.Fill; tlpRoot.RightToLeft = System.Windows.Forms.RightToLeft.Yes; tlpRoot.ColumnCount = 1; tlpRoot.RowCount = 7; tlpRoot.Padding = new System.Windows.Forms.Padding(16); tlpRoot.BackColor = BackColor;
        tlpRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
        tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 88F));
        tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
        tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 360F));
        tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 68F));
        tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
        tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 96F));

        pnlC002Header.Name = "pnlC002Header"; pnlC002Header.Dock = System.Windows.Forms.DockStyle.Fill; pnlC002Header.Padding = new System.Windows.Forms.Padding(16, 10, 16, 10); pnlC002Header.BackColor = System.Drawing.Color.White; pnlC002Header.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
        lblScreenCode.Name = "lblScreenCode"; lblScreenCode.Text = "GEN-008"; lblScreenCode.Dock = System.Windows.Forms.DockStyle.Top; lblScreenCode.Height = 20; lblScreenCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight; lblScreenCode.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold); lblScreenCode.ForeColor = System.Drawing.Color.FromArgb(46, 116, 181);
        lblScreenTitle.Name = "lblScreenTitle"; lblScreenTitle.Text = "أنواع المركبات"; lblScreenTitle.Dock = System.Windows.Forms.DockStyle.Top; lblScreenTitle.Height = 38; lblScreenTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight; lblScreenTitle.Font = new System.Drawing.Font("Arial", 20F, System.Drawing.FontStyle.Bold);
        lblBreadcrumb.Name = "lblBreadcrumb"; lblBreadcrumb.Text = "التهيئة العامة / البيانات المرجعية / أنواع المركبات"; lblBreadcrumb.Dock = System.Windows.Forms.DockStyle.Bottom; lblBreadcrumb.Height = 18; lblBreadcrumb.TextAlign = System.Drawing.ContentAlignment.MiddleRight; lblBreadcrumb.ForeColor = System.Drawing.Color.FromArgb(107, 114, 128);
        pnlC002Header.Controls.Add(lblBreadcrumb); pnlC002Header.Controls.Add(lblScreenTitle); pnlC002Header.Controls.Add(lblScreenCode);

        flpC003Toolbar.Name = "flpC003Toolbar"; flpC003Toolbar.Dock = System.Windows.Forms.DockStyle.Fill; flpC003Toolbar.RightToLeft = System.Windows.Forms.RightToLeft.Yes; flpC003Toolbar.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft; flpC003Toolbar.WrapContents = false; flpC003Toolbar.Padding = new System.Windows.Forms.Padding(4, 8, 4, 8); flpC003Toolbar.BackColor = System.Drawing.Color.White;
        btnNew.Name="btnNew"; btnNew.Text="جديد"; btnSave.Name="btnSave"; btnSave.Text="حفظ"; btnEdit.Name="btnEdit"; btnEdit.Text="تعديل"; btnStop.Name="btnStop"; btnStop.Text="إيقاف"; btnDelete.Name="btnDelete"; btnDelete.Text="حذف"; btnPrint.Name="btnPrint"; btnPrint.Text="طباعة"; btnExport.Name="btnExport"; btnExport.Text="تصدير"; btnCancel.Name="btnCancel"; btnCancel.Text="إلغاء"; btnMore.Name="btnMore"; btnMore.Text="المزيد";
        btnNew.Size=btnSave.Size=btnEdit.Size=btnStop.Size=btnDelete.Size=btnPrint.Size=btnExport.Size=btnCancel.Size=btnMore.Size=new System.Drawing.Size(82, 36);
        btnNew.FlatStyle=btnSave.FlatStyle=btnEdit.FlatStyle=btnStop.FlatStyle=btnDelete.FlatStyle=btnPrint.FlatStyle=btnExport.FlatStyle=btnCancel.FlatStyle=btnMore.FlatStyle=System.Windows.Forms.FlatStyle.Flat;
        btnNew.Margin=btnSave.Margin=btnEdit.Margin=btnStop.Margin=btnDelete.Margin=btnPrint.Margin=btnExport.Margin=btnCancel.Margin=btnMore.Margin=new System.Windows.Forms.Padding(4,0,4,0);
        flpC003Toolbar.Controls.AddRange(new System.Windows.Forms.Control[] { btnNew, btnSave, btnEdit, btnStop, btnDelete, btnPrint, btnExport, btnCancel, btnMore });

        tlpC005Data.Name="tlpC005Data"; tlpC005Data.Dock=System.Windows.Forms.DockStyle.Fill; tlpC005Data.RightToLeft=System.Windows.Forms.RightToLeft.Yes; tlpC005Data.ColumnCount=4; tlpC005Data.RowCount=8; tlpC005Data.Padding=new System.Windows.Forms.Padding(16, 10, 16, 10); tlpC005Data.BackColor=System.Drawing.Color.White;
        tlpC005Data.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F)); tlpC005Data.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F)); tlpC005Data.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F)); tlpC005Data.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
        for (int i = 0; i < 7; i++) tlpC005Data.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F)); tlpC005Data.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        var lblVehicleTypeCode = new System.Windows.Forms.Label { Name="lblVehicleTypeCode", Text="كود النوع *", Dock=System.Windows.Forms.DockStyle.Fill, TextAlign=System.Drawing.ContentAlignment.MiddleRight, Font=new System.Drawing.Font("Arial",11F,System.Drawing.FontStyle.Bold) };
        var lblNameAr = new System.Windows.Forms.Label { Name="lblNameAr", Text="الاسم العربي *", Dock=System.Windows.Forms.DockStyle.Fill, TextAlign=System.Drawing.ContentAlignment.MiddleRight, Font=new System.Drawing.Font("Arial",11F,System.Drawing.FontStyle.Bold) };
        var lblNameEn = new System.Windows.Forms.Label { Name="lblNameEn", Text="الاسم الإنجليزي *", Dock=System.Windows.Forms.DockStyle.Fill, TextAlign=System.Drawing.ContentAlignment.MiddleRight, Font=new System.Drawing.Font("Arial",11F,System.Drawing.FontStyle.Bold) };
        var lblCategory = new System.Windows.Forms.Label { Name="lblCategory", Text="الفئة *", Dock=System.Windows.Forms.DockStyle.Fill, TextAlign=System.Drawing.ContentAlignment.MiddleRight, Font=new System.Drawing.Font("Arial",11F,System.Drawing.FontStyle.Bold) };
        var lblSeats = new System.Windows.Forms.Label { Name="lblSeats", Text="عدد المقاعد", Dock=System.Windows.Forms.DockStyle.Fill, TextAlign=System.Drawing.ContentAlignment.MiddleRight };
        var lblPayload = new System.Windows.Forms.Label { Name="lblPayload", Text="الحمولة بالطن", Dock=System.Windows.Forms.DockStyle.Fill, TextAlign=System.Drawing.ContentAlignment.MiddleRight };
        var lblLength = new System.Windows.Forms.Label { Name="lblLength", Text="الطول", Dock=System.Windows.Forms.DockStyle.Fill, TextAlign=System.Drawing.ContentAlignment.MiddleRight };
        var lblWidth = new System.Windows.Forms.Label { Name="lblWidth", Text="العرض", Dock=System.Windows.Forms.DockStyle.Fill, TextAlign=System.Drawing.ContentAlignment.MiddleRight };
        var lblHeight = new System.Windows.Forms.Label { Name="lblHeight", Text="الارتفاع", Dock=System.Windows.Forms.DockStyle.Fill, TextAlign=System.Drawing.ContentAlignment.MiddleRight };
        var lblAxles = new System.Windows.Forms.Label { Name="lblAxles", Text="عدد المحاور", Dock=System.Windows.Forms.DockStyle.Fill, TextAlign=System.Drawing.ContentAlignment.MiddleRight };
        var lblBodyType = new System.Windows.Forms.Label { Name="lblBodyType", Text="نوع الهيكل", Dock=System.Windows.Forms.DockStyle.Fill, TextAlign=System.Drawing.ContentAlignment.MiddleRight };
        var lblRoofType = new System.Windows.Forms.Label { Name="lblRoofType", Text="نوع السطح", Dock=System.Windows.Forms.DockStyle.Fill, TextAlign=System.Drawing.ContentAlignment.MiddleRight };
        var lblOwnership = new System.Windows.Forms.Label { Name="lblOwnership", Text="نوع الملكية", Dock=System.Windows.Forms.DockStyle.Fill, TextAlign=System.Drawing.ContentAlignment.MiddleRight };
        var lblStatus = new System.Windows.Forms.Label { Name="lblStatus", Text="الحالة *", Dock=System.Windows.Forms.DockStyle.Fill, TextAlign=System.Drawing.ContentAlignment.MiddleRight, Font=new System.Drawing.Font("Arial",11F,System.Drawing.FontStyle.Bold) };
        var lblNotes = new System.Windows.Forms.Label { Name="lblNotes", Text="الملاحظات", Dock=System.Windows.Forms.DockStyle.Fill, TextAlign=System.Drawing.ContentAlignment.MiddleRight };
        txtVehicleTypeCode.Name="txtVehicleTypeCode"; txtNameAr.Name="txtNameAr"; txtNameEn.Name="txtNameEn"; cmbCategory.Name="cmbCategory"; nudSeats.Name="nudSeats"; nudPayloadTons.Name="nudPayloadTons"; nudLength.Name="nudLength"; nudWidth.Name="nudWidth"; nudHeight.Name="nudHeight"; nudAxles.Name="nudAxles"; cmbBodyType.Name="cmbBodyType"; cmbRoofType.Name="cmbRoofType"; cmbOwnershipType.Name="cmbOwnershipType"; cmbStatus.Name="cmbStatus"; rtbNotes.Name="rtbNotes";
        txtVehicleTypeCode.Dock=txtNameAr.Dock=txtNameEn.Dock=cmbCategory.Dock=nudSeats.Dock=nudPayloadTons.Dock=nudLength.Dock=nudWidth.Dock=nudHeight.Dock=nudAxles.Dock=cmbBodyType.Dock=cmbRoofType.Dock=cmbOwnershipType.Dock=cmbStatus.Dock=rtbNotes.Dock=System.Windows.Forms.DockStyle.Fill;
        txtVehicleTypeCode.Margin=txtNameAr.Margin=txtNameEn.Margin=cmbCategory.Margin=nudSeats.Margin=nudPayloadTons.Margin=nudLength.Margin=nudWidth.Margin=nudHeight.Margin=nudAxles.Margin=cmbBodyType.Margin=cmbRoofType.Margin=cmbOwnershipType.Margin=cmbStatus.Margin=rtbNotes.Margin=new System.Windows.Forms.Padding(8,4,12,4);
        tlpC005Data.Controls.Add(lblVehicleTypeCode,0,0); tlpC005Data.Controls.Add(txtVehicleTypeCode,1,0); tlpC005Data.Controls.Add(lblNameAr,2,0); tlpC005Data.Controls.Add(txtNameAr,3,0);
        tlpC005Data.Controls.Add(lblNameEn,0,1); tlpC005Data.Controls.Add(txtNameEn,1,1); tlpC005Data.Controls.Add(lblCategory,2,1); tlpC005Data.Controls.Add(cmbCategory,3,1);
        tlpC005Data.Controls.Add(lblSeats,0,2); tlpC005Data.Controls.Add(nudSeats,1,2); tlpC005Data.Controls.Add(lblPayload,2,2); tlpC005Data.Controls.Add(nudPayloadTons,3,2);
        tlpC005Data.Controls.Add(lblLength,0,3); tlpC005Data.Controls.Add(nudLength,1,3); tlpC005Data.Controls.Add(lblWidth,2,3); tlpC005Data.Controls.Add(nudWidth,3,3);
        tlpC005Data.Controls.Add(lblHeight,0,4); tlpC005Data.Controls.Add(nudHeight,1,4); tlpC005Data.Controls.Add(lblAxles,2,4); tlpC005Data.Controls.Add(nudAxles,3,4);
        tlpC005Data.Controls.Add(lblBodyType,0,5); tlpC005Data.Controls.Add(cmbBodyType,1,5); tlpC005Data.Controls.Add(lblRoofType,2,5); tlpC005Data.Controls.Add(cmbRoofType,3,5);
        tlpC005Data.Controls.Add(lblOwnership,0,6); tlpC005Data.Controls.Add(cmbOwnershipType,1,6); tlpC005Data.Controls.Add(lblStatus,2,6); tlpC005Data.Controls.Add(cmbStatus,3,6);
        tlpC005Data.Controls.Add(lblNotes,0,7); tlpC005Data.Controls.Add(rtbNotes,1,7); tlpC005Data.SetColumnSpan(rtbNotes,3);
        txtVehicleTypeCode.BackColor=txtNameAr.BackColor=txtNameEn.BackColor=cmbCategory.BackColor=cmbStatus.BackColor=System.Drawing.Color.FromArgb(255,247,204);
        cmbCategory.DropDownStyle=cmbBodyType.DropDownStyle=cmbRoofType.DropDownStyle=cmbOwnershipType.DropDownStyle=cmbStatus.DropDownStyle=System.Windows.Forms.ComboBoxStyle.DropDownList;
        nudSeats.Maximum=9999; nudPayloadTons.Maximum=nudLength.Maximum=nudWidth.Maximum=nudHeight.Maximum=999999; nudPayloadTons.DecimalPlaces=nudLength.DecimalPlaces=nudWidth.DecimalPlaces=nudHeight.DecimalPlaces=2; nudAxles.Maximum=99;
        rtbNotes.Height=72;

        tlpC007Search.Name="tlpC007Search"; tlpC007Search.Dock=System.Windows.Forms.DockStyle.Fill; tlpC007Search.RightToLeft=System.Windows.Forms.RightToLeft.Yes; tlpC007Search.ColumnCount=5; tlpC007Search.Padding=new System.Windows.Forms.Padding(16,12,16,12); tlpC007Search.BackColor=System.Drawing.Color.White;
        tlpC007Search.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 46F)); tlpC007Search.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F)); tlpC007Search.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F)); tlpC007Search.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute,88F)); tlpC007Search.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute,88F));
        txtSearch.Name="txtSearch"; txtSearch.PlaceholderText="بحث بالكود أو الاسم أو الفئة"; txtSearch.Dock=System.Windows.Forms.DockStyle.Fill; txtSearch.RightToLeft=System.Windows.Forms.RightToLeft.Yes;
        cmbFilterStatus.Name="cmbFilterStatus"; cmbFilterStatus.Dock=System.Windows.Forms.DockStyle.Fill; cmbFilterStatus.DropDownStyle=System.Windows.Forms.ComboBoxStyle.DropDownList; cmbFilterStatus.PlaceholderText="الحالة";
        cmbFilterCategory.Name="cmbFilterCategory"; cmbFilterCategory.Dock=System.Windows.Forms.DockStyle.Fill; cmbFilterCategory.DropDownStyle=System.Windows.Forms.ComboBoxStyle.DropDownList;
        btnSearch.Name="btnSearch"; btnSearch.Text="بحث"; btnSearch.Dock=System.Windows.Forms.DockStyle.Fill; btnSearch.FlatStyle=System.Windows.Forms.FlatStyle.Flat; btnSearch.BackColor=System.Drawing.Color.FromArgb(46,116,181); btnSearch.ForeColor=System.Drawing.Color.White;
        btnClearFilters.Name="btnClearFilters"; btnClearFilters.Text="مسح"; btnClearFilters.Dock=System.Windows.Forms.DockStyle.Fill; btnClearFilters.FlatStyle=System.Windows.Forms.FlatStyle.Flat;
        tlpC007Search.Controls.Add(txtSearch,0,0); tlpC007Search.Controls.Add(cmbFilterStatus,1,0); tlpC007Search.Controls.Add(cmbFilterCategory,2,0); tlpC007Search.Controls.Add(btnSearch,3,0); tlpC007Search.Controls.Add(btnClearFilters,4,0);

        pnlC008Grid.Name="pnlC008Grid"; pnlC008Grid.Dock=System.Windows.Forms.DockStyle.Fill; pnlC008Grid.Padding=new System.Windows.Forms.Padding(0,6,0,6); pnlC008Grid.BackColor=System.Drawing.Color.White;
        dgvRecords.Name="dgvRecords"; dgvRecords.Dock=System.Windows.Forms.DockStyle.Fill; dgvRecords.RightToLeft=System.Windows.Forms.RightToLeft.Yes; dgvRecords.AllowUserToAddRows=false; dgvRecords.AllowUserToDeleteRows=false; dgvRecords.AllowUserToResizeRows=false; dgvRecords.AutoGenerateColumns=false; dgvRecords.BackgroundColor=System.Drawing.Color.White; dgvRecords.BorderStyle=System.Windows.Forms.BorderStyle.FixedSingle; dgvRecords.MultiSelect=false; dgvRecords.ReadOnly=true; dgvRecords.SelectionMode=System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        colCode.Name="colCode"; colCode.HeaderText="كود النوع"; colCode.ReadOnly=true; colNameAr.Name="colNameAr"; colNameAr.HeaderText="الاسم العربي"; colNameAr.ReadOnly=true; colCategory.Name="colCategory"; colCategory.HeaderText="الفئة"; colCategory.ReadOnly=true; colSeats.Name="colSeats"; colSeats.HeaderText="المقاعد"; colSeats.ReadOnly=true; colPayload.Name="colPayload"; colPayload.HeaderText="الحمولة"; colPayload.ReadOnly=true; colStatus.Name="colStatus"; colStatus.HeaderText="الحالة"; colStatus.ReadOnly=true;
        dgvRecords.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { colCode,colNameAr,colCategory,colSeats,colPayload,colStatus }); pnlC008Grid.Controls.Add(dgvRecords);

        flpC010Paging.Name="flpC010Paging"; flpC010Paging.Dock=System.Windows.Forms.DockStyle.Fill; flpC010Paging.RightToLeft=System.Windows.Forms.RightToLeft.Yes; flpC010Paging.FlowDirection=System.Windows.Forms.FlowDirection.RightToLeft; flpC010Paging.WrapContents=false; flpC010Paging.Padding=new System.Windows.Forms.Padding(6,8,6,8); flpC010Paging.BackColor=System.Drawing.Color.White;
        btnFirst.Name="btnFirst"; btnFirst.Text="الأول"; btnPrevious.Name="btnPrevious"; btnPrevious.Text="السابق"; btnNext.Name="btnNext"; btnNext.Text="التالي"; btnLast.Name="btnLast"; btnLast.Text="الأخير";
        btnFirst.Size=btnPrevious.Size=btnNext.Size=btnLast.Size=new System.Drawing.Size(68,36); btnFirst.FlatStyle=btnPrevious.FlatStyle=btnNext.FlatStyle=btnLast.FlatStyle=System.Windows.Forms.FlatStyle.Flat;
        lblPageNumber.Name="lblPageNumber"; lblPageNumber.Text="1"; lblPageNumber.Size=new System.Drawing.Size(52,36); lblPageNumber.TextAlign=System.Drawing.ContentAlignment.MiddleCenter; cmbPageSize.Name="cmbPageSize"; cmbPageSize.Size=new System.Drawing.Size(90,36); cmbPageSize.DropDownStyle=System.Windows.Forms.ComboBoxStyle.DropDownList; lblTotalRecords.Name="lblTotalRecords"; lblTotalRecords.Text="إجمالي السجلات: 0"; lblTotalRecords.AutoSize=true; lblTotalRecords.Padding=new System.Windows.Forms.Padding(12,8,0,0);
        flpC010Paging.Controls.AddRange(new System.Windows.Forms.Control[] { btnFirst,btnPrevious,lblPageNumber,btnNext,btnLast,cmbPageSize,lblTotalRecords });

        tlpC011Audit.Name="tlpC011Audit"; tlpC011Audit.Dock=System.Windows.Forms.DockStyle.Fill; tlpC011Audit.RightToLeft=System.Windows.Forms.RightToLeft.Yes; tlpC011Audit.ColumnCount=5; tlpC011Audit.RowCount=2; tlpC011Audit.Padding=new System.Windows.Forms.Padding(16,8,16,8); tlpC011Audit.BackColor=System.Drawing.Color.FromArgb(243,244,246);
        for (int i=0;i<5;i++) tlpC011Audit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent,20F));
        tlpC011Audit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent,50F)); tlpC011Audit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent,50F));
        var auditCreatedBy = new System.Windows.Forms.Label { Text="أنشئ بواسطة: —", Dock=System.Windows.Forms.DockStyle.Fill, TextAlign=System.Drawing.ContentAlignment.MiddleRight };
        var auditCreatedAt = new System.Windows.Forms.Label { Text="تاريخ الإنشاء: —", Dock=System.Windows.Forms.DockStyle.Fill, TextAlign=System.Drawing.ContentAlignment.MiddleRight };
        var auditModifiedBy = new System.Windows.Forms.Label { Text="آخر تعديل بواسطة: —", Dock=System.Windows.Forms.DockStyle.Fill, TextAlign=System.Drawing.ContentAlignment.MiddleRight };
        var auditModifiedAt = new System.Windows.Forms.Label { Text="تاريخ التعديل: —", Dock=System.Windows.Forms.DockStyle.Fill, TextAlign=System.Drawing.ContentAlignment.MiddleRight };
        var auditCompany = new System.Windows.Forms.Label { Text="الشركة: —", Dock=System.Windows.Forms.DockStyle.Fill, TextAlign=System.Drawing.ContentAlignment.MiddleRight };
        var auditBranch = new System.Windows.Forms.Label { Text="الفرع: —", Dock=System.Windows.Forms.DockStyle.Fill, TextAlign=System.Drawing.ContentAlignment.MiddleRight };
        var auditDeviceIp = new System.Windows.Forms.Label { Text="الجهاز / IP: —", Dock=System.Windows.Forms.DockStyle.Fill, TextAlign=System.Drawing.ContentAlignment.MiddleRight };
        var auditOperation = new System.Windows.Forms.Label { Text="العملية: —", Dock=System.Windows.Forms.DockStyle.Fill, TextAlign=System.Drawing.ContentAlignment.MiddleRight };
        var auditReference = new System.Windows.Forms.Label { Text="المرجع: —", Dock=System.Windows.Forms.DockStyle.Fill, TextAlign=System.Drawing.ContentAlignment.MiddleRight };
        tlpC011Audit.Controls.Add(auditCreatedBy,0,0); tlpC011Audit.Controls.Add(auditCreatedAt,1,0); tlpC011Audit.Controls.Add(auditModifiedBy,2,0); tlpC011Audit.Controls.Add(auditModifiedAt,3,0); tlpC011Audit.Controls.Add(auditCompany,4,0);
        tlpC011Audit.Controls.Add(auditBranch,0,1); tlpC011Audit.Controls.Add(auditDeviceIp,1,1); tlpC011Audit.Controls.Add(auditOperation,2,1); tlpC011Audit.Controls.Add(auditReference,3,1);
        btnAuditLog.Name="btnAuditLog"; btnAuditLog.Text="سجل التدقيق"; btnAuditLog.Dock=System.Windows.Forms.DockStyle.Fill; btnAuditLog.FlatStyle=System.Windows.Forms.FlatStyle.Flat; tlpC011Audit.Controls.Add(btnAuditLog,4,1);

        tlpRoot.Controls.Add(pnlC002Header,0,0); tlpRoot.Controls.Add(flpC003Toolbar,0,1); tlpRoot.Controls.Add(tlpC005Data,0,2); tlpRoot.Controls.Add(tlpC007Search,0,3); tlpRoot.Controls.Add(pnlC008Grid,0,4); tlpRoot.Controls.Add(flpC010Paging,0,5); tlpRoot.Controls.Add(tlpC011Audit,0,6);
        Controls.Add(tlpRoot);

        ((System.ComponentModel.ISupportInitialize)nudSeats).EndInit(); ((System.ComponentModel.ISupportInitialize)nudPayloadTons).EndInit(); ((System.ComponentModel.ISupportInitialize)nudLength).EndInit(); ((System.ComponentModel.ISupportInitialize)nudWidth).EndInit(); ((System.ComponentModel.ISupportInitialize)nudHeight).EndInit(); ((System.ComponentModel.ISupportInitialize)nudAxles).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvRecords).EndInit();
        ResumeLayout(false);
    }
}
