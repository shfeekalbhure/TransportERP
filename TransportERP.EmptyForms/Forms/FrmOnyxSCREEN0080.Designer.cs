#nullable disable
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace TransportERP.EmptyForms;

partial class UcOnyxSCREEN0080
{
    private TransportERP.Desktop.CoreUI.AuditMetadataControl standardAuditMetadata = null!;

    private TransportERP.Desktop.CoreUI.DesignerCommandBar designerCommandBar = null!;
    private FlowLayoutPanel designerCommandFlow = null!;
    private Panel designerCloseHost = null!;
    private Button standardCommandRefresh = null!;
    private Button standardCommandImport = null!;
    private Button standardCommandExport = null!;
    private Button standardCommandHelp = null!;

    private IContainer components;
    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        designerCommandBar = new TransportERP.Desktop.CoreUI.DesignerCommandBar();
        designerCommandFlow = new FlowLayoutPanel();
        designerCloseHost = new Panel();
        standardCommandRefresh = new Button();
        standardCommandImport = new Button();
        standardCommandExport = new Button();
        standardCommandHelp = new Button();
        DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
        mainLayout = new TableLayoutPanel();
        lblTitle = new Label();
        pnlToolbar = new Panel();
        btnNew = new Button();
        btnEdit = new Button();
        btnSave = new Button();
        btnUndo = new Button();
        btnDelete = new Button();
        btnView = new Button();
        btnPrint = new Button();
        btnFirst = new Button();
        btnPrevious = new Button();
        btnNext = new Button();
        btnLast = new Button();
        btnClose = new Button();
        fieldsLayout = new TableLayoutPanel();
        lblCode = new Label();
        txtCode = new TextBox();
        lblLocalName = new Label();
        txtLocalName = new TextBox();
        lblForeignName = new Label();
        txtForeignName = new TextBox();
        pnlContent = new Panel();
        gridBankGroups = new DataGridView();
        auditInfoContainer = new TableLayoutPanel();
        lblCreatedBy = new Label();
        txtCreatedBy = new TextBox();
        lblModifiedBy = new Label();
        txtModifiedBy = new TextBox();
        lblCreatedAt = new Label();
        txtCreatedAt = new TextBox();
        lblModifiedAt = new Label();
        txtModifiedAt = new TextBox();
        lblCreateCount = new Label();
        txtCreateCount = new TextBox();
        lblModifyCount = new Label();
        txtModifyCount = new TextBox();
        lblCreatedMachine = new Label();
        txtCreatedMachine = new TextBox();
        lblModifiedMachine = new Label();
        txtModifiedMachine = new TextBox();
        lblDataStatus = new Label();
        mainLayout.SuspendLayout();
        pnlToolbar.SuspendLayout();
        fieldsLayout.SuspendLayout();
        pnlContent.SuspendLayout();
        ((ISupportInitialize)gridBankGroups).BeginInit();
        auditInfoContainer.SuspendLayout();
        SuspendLayout();
        // 
        // mainLayout
        // 
        mainLayout.BackColor = Color.FromArgb(244, 244, 244);
        mainLayout.ColumnCount = 1;
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        mainLayout.Controls.Add(lblTitle, 0, 0);
        mainLayout.Controls.Add(designerCommandBar, 0, 1);
        mainLayout.Controls.Add(fieldsLayout, 0, 2);
        mainLayout.Controls.Add(pnlContent, 0, 3);
        mainLayout.Controls.Add(auditInfoContainer, 0, 4);
        mainLayout.Controls.Add(lblDataStatus, 0, 5);
        mainLayout.Dock = DockStyle.Fill;
        mainLayout.Location = new Point(0, 0);
        mainLayout.Margin = new Padding(0);
        mainLayout.Name = "mainLayout";
        mainLayout.Padding = new Padding(3);
        mainLayout.RowCount = 6;
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 110F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 68F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
        mainLayout.Size = new Size(1100, 650);
        mainLayout.TabIndex = 0;
        // 
        // lblTitle
        // 
        lblTitle.BackColor = Color.FromArgb(23, 50, 78);
        lblTitle.Dock = DockStyle.Fill;
        lblTitle.Font = new Font("Tahoma", 11F, FontStyle.Bold);
        lblTitle.ForeColor = Color.White;
        lblTitle.Location = new Point(3, 3);
        lblTitle.Margin = new Padding(0, 0, 0, 2);
        lblTitle.Name = "lblTitle";
        lblTitle.Padding = new Padding(4, 0, 4, 0);
        lblTitle.RightToLeft = RightToLeft.No;
        lblTitle.Size = new Size(1094, 32);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "مجموعات البنوك";
        lblTitle.TextAlign = ContentAlignment.MiddleRight;
        // 
        // pnlToolbar
        // 
        pnlToolbar.BackColor = Color.FromArgb(232, 227, 246);
        pnlToolbar.BorderStyle = BorderStyle.FixedSingle;

        pnlToolbar.Dock = DockStyle.Fill;
        pnlToolbar.Location = new Point(3, 37);
        pnlToolbar.Margin = new Padding(0, 0, 0, 2);
        pnlToolbar.Name = "pnlToolbar";
        pnlToolbar.Padding = new Padding(2);
        pnlToolbar.Size = new Size(1094, 44);
        pnlToolbar.TabIndex = 1;
        // 
        // btnNew
        // 
        btnNew.AccessibleName = "إضافة";
        btnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnNew.Enabled = false;
        btnNew.Location = new Point(1022, 4);
        btnNew.Margin = new Padding(2);
        btnNew.Name = "btnNew";
        btnNew.Size = new Size(66, 34);
        btnNew.TabIndex = 0;
        btnNew.Text = "إضافة";
        btnNew.UseVisualStyleBackColor = true;
        // 
        // btnEdit
        // 
        btnEdit.AccessibleName = "تعديل";
        btnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnEdit.Enabled = false;
        btnEdit.Location = new Point(952, 4);
        btnEdit.Margin = new Padding(2);
        btnEdit.Name = "btnEdit";
        btnEdit.Size = new Size(66, 34);
        btnEdit.TabIndex = 1;
        btnEdit.Text = "تعديل";
        btnEdit.UseVisualStyleBackColor = true;
        // 
        // btnSave
        // 
        btnSave.AccessibleName = "حفظ";
        btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnSave.Enabled = false;
        btnSave.Location = new Point(882, 4);
        btnSave.Margin = new Padding(2);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(66, 34);
        btnSave.TabIndex = 2;
        btnSave.Text = "حفظ";
        btnSave.UseVisualStyleBackColor = true;
        // 
        // btnUndo
        // 
        btnUndo.AccessibleName = "تراجع";
        btnUndo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnUndo.Enabled = false;
        btnUndo.Location = new Point(812, 4);
        btnUndo.Margin = new Padding(2);
        btnUndo.Name = "btnUndo";
        btnUndo.Size = new Size(66, 34);
        btnUndo.TabIndex = 3;
        btnUndo.Text = "تراجع";
        btnUndo.UseVisualStyleBackColor = true;
        // 
        // btnDelete
        // 
        btnDelete.AccessibleName = "حذف";
        btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnDelete.Enabled = false;
        btnDelete.Location = new Point(742, 4);
        btnDelete.Margin = new Padding(2);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(66, 34);
        btnDelete.TabIndex = 4;
        btnDelete.Text = "حذف";
        btnDelete.UseVisualStyleBackColor = true;
        // 
        // btnView
        // 
        btnView.AccessibleName = "عرض";
        btnView.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnView.Enabled = false;
        btnView.Location = new Point(672, 4);
        btnView.Margin = new Padding(2);
        btnView.Name = "btnView";
        btnView.Size = new Size(66, 34);
        btnView.TabIndex = 5;
        btnView.Text = "عرض";
        btnView.UseVisualStyleBackColor = true;
        // 
        // btnPrint
        // 
        btnPrint.AccessibleName = "طباعة";
        btnPrint.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnPrint.Enabled = false;
        btnPrint.Location = new Point(602, 4);
        btnPrint.Margin = new Padding(2);
        btnPrint.Name = "btnPrint";
        btnPrint.Size = new Size(66, 34);
        btnPrint.TabIndex = 6;
        btnPrint.Text = "طباعة";
        btnPrint.UseVisualStyleBackColor = true;
        // 
        // btnFirst
        // 
        btnFirst.AccessibleName = "أول";
        btnFirst.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnFirst.Enabled = false;
        btnFirst.Location = new Point(532, 4);
        btnFirst.Margin = new Padding(2);
        btnFirst.Name = "btnFirst";
        btnFirst.Size = new Size(66, 34);
        btnFirst.TabIndex = 7;
        btnFirst.Text = "أول";
        btnFirst.UseVisualStyleBackColor = true;
        // 
        // btnPrevious
        // 
        btnPrevious.AccessibleName = "السابق";
        btnPrevious.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnPrevious.Enabled = false;
        btnPrevious.Location = new Point(462, 4);
        btnPrevious.Margin = new Padding(2);
        btnPrevious.Name = "btnPrevious";
        btnPrevious.Size = new Size(66, 34);
        btnPrevious.TabIndex = 8;
        btnPrevious.Text = "السابق";
        btnPrevious.UseVisualStyleBackColor = true;
        // 
        // btnNext
        // 
        btnNext.AccessibleName = "التالي";
        btnNext.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnNext.Enabled = false;
        btnNext.Location = new Point(392, 4);
        btnNext.Margin = new Padding(2);
        btnNext.Name = "btnNext";
        btnNext.Size = new Size(66, 34);
        btnNext.TabIndex = 9;
        btnNext.Text = "التالي";
        btnNext.UseVisualStyleBackColor = true;
        // 
        // btnLast
        // 
        btnLast.AccessibleName = "آخر";
        btnLast.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnLast.Enabled = false;
        btnLast.Location = new Point(322, 4);
        btnLast.Margin = new Padding(2);
        btnLast.Name = "btnLast";
        btnLast.Size = new Size(66, 34);
        btnLast.TabIndex = 10;
        btnLast.Text = "آخر";
        btnLast.UseVisualStyleBackColor = true;
        // 
        // btnClose
        // 
        btnClose.AccessibleName = "إغلاق";
        btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnClose.Location = new Point(252, 4);
        btnClose.Margin = new Padding(2);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(66, 34);
        btnClose.TabIndex = 11;
        btnClose.Text = "إغلاق";
        btnClose.UseVisualStyleBackColor = true;
        btnClose.Click += BtnClose_Click;
        // 
        // fieldsLayout
        // 
        fieldsLayout.BackColor = Color.White;
        fieldsLayout.BorderStyle = BorderStyle.FixedSingle;
        fieldsLayout.ColumnCount = 4;
        fieldsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        fieldsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F));
        fieldsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300F));
        fieldsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        fieldsLayout.Controls.Add(lblCode, 1, 0);
        fieldsLayout.Controls.Add(txtCode, 2, 0);
        fieldsLayout.Controls.Add(lblLocalName, 1, 1);
        fieldsLayout.Controls.Add(txtLocalName, 2, 1);
        fieldsLayout.Controls.Add(lblForeignName, 1, 2);
        fieldsLayout.Controls.Add(txtForeignName, 2, 2);
        fieldsLayout.Dock = DockStyle.Fill;
        fieldsLayout.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
        fieldsLayout.Location = new Point(3, 83);
        fieldsLayout.Margin = new Padding(0, 0, 0, 2);
        fieldsLayout.Name = "fieldsLayout";
        fieldsLayout.Padding = new Padding(2);
        fieldsLayout.RightToLeft = RightToLeft.Yes;
        fieldsLayout.RowCount = 3;
        fieldsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
        fieldsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
        fieldsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
        fieldsLayout.Size = new Size(1094, 108);
        fieldsLayout.TabIndex = 2;
        // 
        // lblCode
        // 
        lblCode.Dock = DockStyle.Fill;
        lblCode.Location = new Point(627, 3);
        lblCode.Margin = new Padding(1);
        lblCode.Name = "lblCode";
        lblCode.Size = new Size(138, 32);
        lblCode.TabIndex = 0;
        lblCode.Text = "رقم المجموعة";
        lblCode.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtCode
        // 
        txtCode.AccessibleName = "رقم المجموعة";
        txtCode.BackColor = Color.FromArgb(255, 253, 225);
        txtCode.BorderStyle = BorderStyle.FixedSingle;
        txtCode.Dock = DockStyle.Fill;
        txtCode.Location = new Point(328, 8);
        txtCode.Margin = new Padding(2, 6, 2, 4);
        txtCode.Name = "txtCode";
        txtCode.ReadOnly = true;
        txtCode.RightToLeft = RightToLeft.No;
        txtCode.Size = new Size(296, 22);
        txtCode.TabIndex = 0;
        txtCode.Tag = "UI_ALIAS:Code";
        // 
        // lblLocalName
        // 
        lblLocalName.Dock = DockStyle.Fill;
        lblLocalName.Location = new Point(627, 37);
        lblLocalName.Margin = new Padding(1);
        lblLocalName.Name = "lblLocalName";
        lblLocalName.Size = new Size(138, 32);
        lblLocalName.TabIndex = 1;
        lblLocalName.Text = "اسم المجموعة";
        lblLocalName.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtLocalName
        // 
        txtLocalName.AccessibleName = "اسم المجموعة";
        txtLocalName.BackColor = Color.FromArgb(255, 253, 225);
        txtLocalName.BorderStyle = BorderStyle.FixedSingle;
        txtLocalName.Dock = DockStyle.Fill;
        txtLocalName.Location = new Point(328, 42);
        txtLocalName.Margin = new Padding(2, 6, 2, 4);
        txtLocalName.Name = "txtLocalName";
        txtLocalName.ReadOnly = true;
        txtLocalName.RightToLeft = RightToLeft.Yes;
        txtLocalName.Size = new Size(296, 22);
        txtLocalName.TabIndex = 1;
        txtLocalName.Tag = "UI_ALIAS:LocalName";
        // 
        // lblForeignName
        // 
        lblForeignName.Dock = DockStyle.Fill;
        lblForeignName.Location = new Point(627, 71);
        lblForeignName.Margin = new Padding(1);
        lblForeignName.Name = "lblForeignName";
        lblForeignName.Size = new Size(138, 32);
        lblForeignName.TabIndex = 2;
        lblForeignName.Text = "الاسم الأجنبي";
        lblForeignName.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtForeignName
        // 
        txtForeignName.AccessibleName = "الاسم الأجنبي";
        txtForeignName.BackColor = Color.White;
        txtForeignName.BorderStyle = BorderStyle.FixedSingle;
        txtForeignName.Dock = DockStyle.Fill;
        txtForeignName.Location = new Point(328, 76);
        txtForeignName.Margin = new Padding(2, 6, 2, 4);
        txtForeignName.Name = "txtForeignName";
        txtForeignName.ReadOnly = true;
        txtForeignName.RightToLeft = RightToLeft.No;
        txtForeignName.Size = new Size(296, 22);
        txtForeignName.TabIndex = 2;
        txtForeignName.Tag = "UI_ALIAS:ForeignName";
        // 
        // pnlContent
        // 
        pnlContent.BorderStyle = BorderStyle.FixedSingle;
        pnlContent.Controls.Add(gridBankGroups);
        pnlContent.Dock = DockStyle.Fill;
        pnlContent.Location = new Point(3, 193);
        pnlContent.Margin = new Padding(0, 0, 0, 2);
        pnlContent.Name = "pnlContent";
        pnlContent.Padding = new Padding(2);
        pnlContent.Size = new Size(1094, 356);
        pnlContent.TabIndex = 3;
        // 
        // gridBankGroups
        // 
        gridBankGroups.AccessibleName = "مجموعات البنوك";
        gridBankGroups.AllowUserToAddRows = false;
        gridBankGroups.AllowUserToDeleteRows = false;
        gridBankGroups.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        gridBankGroups.BackgroundColor = Color.White;
        gridBankGroups.BorderStyle = BorderStyle.None;
        dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dataGridViewCellStyle1.BackColor = Color.FromArgb(216, 255, 255);
        dataGridViewCellStyle1.ForeColor = Color.Black;
        gridBankGroups.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
        gridBankGroups.ColumnHeadersHeight = 32;
        gridBankGroups.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        gridBankGroups.Dock = DockStyle.Fill;
        gridBankGroups.EnableHeadersVisualStyles = false;
        gridBankGroups.GridColor = Color.FromArgb(176, 176, 176);
        gridBankGroups.Location = new Point(2, 2);
        gridBankGroups.MultiSelect = false;
        gridBankGroups.Name = "gridBankGroups";
        gridBankGroups.ReadOnly = true;
        gridBankGroups.RowHeadersWidth = 28;
        gridBankGroups.RowTemplate.Height = 28;
        gridBankGroups.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        gridBankGroups.Size = new Size(1088, 350);
        gridBankGroups.TabIndex = 0;
        // 
        // auditInfoContainer
        // 
        auditInfoContainer.BackColor = Color.FromArgb(232, 227, 246);
        auditInfoContainer.BorderStyle = BorderStyle.FixedSingle;
        auditInfoContainer.ColumnCount = 8;
        auditInfoContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        auditInfoContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
        auditInfoContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        auditInfoContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
        auditInfoContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        auditInfoContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
        auditInfoContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        auditInfoContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
        auditInfoContainer.Controls.Add(lblCreatedBy, 0, 0);
        auditInfoContainer.Controls.Add(txtCreatedBy, 1, 0);
        auditInfoContainer.Controls.Add(lblModifiedBy, 2, 0);
        auditInfoContainer.Controls.Add(txtModifiedBy, 3, 0);
        auditInfoContainer.Controls.Add(lblCreatedAt, 4, 0);
        auditInfoContainer.Controls.Add(txtCreatedAt, 5, 0);
        auditInfoContainer.Controls.Add(lblModifiedAt, 6, 0);
        auditInfoContainer.Controls.Add(txtModifiedAt, 7, 0);
        auditInfoContainer.Controls.Add(lblCreateCount, 0, 1);
        auditInfoContainer.Controls.Add(txtCreateCount, 1, 1);
        auditInfoContainer.Controls.Add(lblModifyCount, 2, 1);
        auditInfoContainer.Controls.Add(txtModifyCount, 3, 1);
        auditInfoContainer.Controls.Add(lblCreatedMachine, 4, 1);
        auditInfoContainer.Controls.Add(txtCreatedMachine, 5, 1);
        auditInfoContainer.Controls.Add(lblModifiedMachine, 6, 1);
        auditInfoContainer.Controls.Add(txtModifiedMachine, 7, 1);
        auditInfoContainer.Dock = DockStyle.Fill;
        auditInfoContainer.Location = new Point(3, 551);
        auditInfoContainer.Margin = new Padding(0, 0, 0, 2);
        auditInfoContainer.Name = "auditInfoContainer";
        auditInfoContainer.Padding = new Padding(2);
        auditInfoContainer.RowCount = 2;
        auditInfoContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        auditInfoContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        auditInfoContainer.Size = new Size(1094, 66);
        auditInfoContainer.TabIndex = 4;
        // 
        // lblCreatedBy
        // 
        lblCreatedBy.Dock = DockStyle.Fill;
        lblCreatedBy.Location = new Point(983, 3);
        lblCreatedBy.Margin = new Padding(1);
        lblCreatedBy.Name = "lblCreatedBy";
        lblCreatedBy.Size = new Size(106, 28);
        lblCreatedBy.TabIndex = 0;
        lblCreatedBy.Text = "مدخل السجل";
        lblCreatedBy.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtCreatedBy
        // 
        txtCreatedBy.AccessibleName = "مدخل السجل";
        txtCreatedBy.BackColor = Color.FromArgb(244, 244, 244);
        txtCreatedBy.BorderStyle = BorderStyle.FixedSingle;
        txtCreatedBy.Dock = DockStyle.Fill;
        txtCreatedBy.Location = new Point(821, 5);
        txtCreatedBy.Margin = new Padding(2, 3, 2, 2);
        txtCreatedBy.Name = "txtCreatedBy";
        txtCreatedBy.ReadOnly = true;
        txtCreatedBy.RightToLeft = RightToLeft.Yes;
        txtCreatedBy.Size = new Size(159, 22);
        txtCreatedBy.TabIndex = 1;
        txtCreatedBy.TabStop = false;
        // 
        // lblModifiedBy
        // 
        lblModifiedBy.Dock = DockStyle.Fill;
        lblModifiedBy.Location = new Point(712, 3);
        lblModifiedBy.Margin = new Padding(1);
        lblModifiedBy.Name = "lblModifiedBy";
        lblModifiedBy.Size = new Size(106, 28);
        lblModifiedBy.TabIndex = 2;
        lblModifiedBy.Text = "معدل السجل";
        lblModifiedBy.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtModifiedBy
        // 
        txtModifiedBy.AccessibleName = "معدل السجل";
        txtModifiedBy.BackColor = Color.FromArgb(244, 244, 244);
        txtModifiedBy.BorderStyle = BorderStyle.FixedSingle;
        txtModifiedBy.Dock = DockStyle.Fill;
        txtModifiedBy.Location = new Point(550, 5);
        txtModifiedBy.Margin = new Padding(2, 3, 2, 2);
        txtModifiedBy.Name = "txtModifiedBy";
        txtModifiedBy.ReadOnly = true;
        txtModifiedBy.RightToLeft = RightToLeft.Yes;
        txtModifiedBy.Size = new Size(159, 22);
        txtModifiedBy.TabIndex = 3;
        txtModifiedBy.TabStop = false;
        // 
        // lblCreatedAt
        // 
        lblCreatedAt.Dock = DockStyle.Fill;
        lblCreatedAt.Location = new Point(441, 3);
        lblCreatedAt.Margin = new Padding(1);
        lblCreatedAt.Name = "lblCreatedAt";
        lblCreatedAt.Size = new Size(106, 28);
        lblCreatedAt.TabIndex = 4;
        lblCreatedAt.Text = "تاريخ الإدخال";
        lblCreatedAt.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtCreatedAt
        // 
        txtCreatedAt.AccessibleName = "تاريخ الإدخال";
        txtCreatedAt.BackColor = Color.FromArgb(244, 244, 244);
        txtCreatedAt.BorderStyle = BorderStyle.FixedSingle;
        txtCreatedAt.Dock = DockStyle.Fill;
        txtCreatedAt.Location = new Point(279, 5);
        txtCreatedAt.Margin = new Padding(2, 3, 2, 2);
        txtCreatedAt.Name = "txtCreatedAt";
        txtCreatedAt.ReadOnly = true;
        txtCreatedAt.RightToLeft = RightToLeft.No;
        txtCreatedAt.Size = new Size(159, 22);
        txtCreatedAt.TabIndex = 5;
        txtCreatedAt.TabStop = false;
        // 
        // lblModifiedAt
        // 
        lblModifiedAt.Dock = DockStyle.Fill;
        lblModifiedAt.Location = new Point(170, 3);
        lblModifiedAt.Margin = new Padding(1);
        lblModifiedAt.Name = "lblModifiedAt";
        lblModifiedAt.Size = new Size(106, 28);
        lblModifiedAt.TabIndex = 6;
        lblModifiedAt.Text = "تاريخ آخر تعديل";
        lblModifiedAt.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtModifiedAt
        // 
        txtModifiedAt.AccessibleName = "تاريخ آخر تعديل";
        txtModifiedAt.BackColor = Color.FromArgb(244, 244, 244);
        txtModifiedAt.BorderStyle = BorderStyle.FixedSingle;
        txtModifiedAt.Dock = DockStyle.Fill;
        txtModifiedAt.Location = new Point(4, 5);
        txtModifiedAt.Margin = new Padding(2, 3, 2, 2);
        txtModifiedAt.Name = "txtModifiedAt";
        txtModifiedAt.ReadOnly = true;
        txtModifiedAt.RightToLeft = RightToLeft.No;
        txtModifiedAt.Size = new Size(163, 22);
        txtModifiedAt.TabIndex = 7;
        txtModifiedAt.TabStop = false;
        // 
        // lblCreateCount
        // 
        lblCreateCount.Dock = DockStyle.Fill;
        lblCreateCount.Location = new Point(983, 33);
        lblCreateCount.Margin = new Padding(1);
        lblCreateCount.Name = "lblCreateCount";
        lblCreateCount.Size = new Size(106, 28);
        lblCreateCount.TabIndex = 8;
        lblCreateCount.Text = "مرات الإضافة";
        lblCreateCount.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtCreateCount
        // 
        txtCreateCount.AccessibleName = "مرات الإضافة";
        txtCreateCount.BackColor = Color.FromArgb(244, 244, 244);
        txtCreateCount.BorderStyle = BorderStyle.FixedSingle;
        txtCreateCount.Dock = DockStyle.Fill;
        txtCreateCount.Location = new Point(821, 35);
        txtCreateCount.Margin = new Padding(2, 3, 2, 2);
        txtCreateCount.Name = "txtCreateCount";
        txtCreateCount.ReadOnly = true;
        txtCreateCount.RightToLeft = RightToLeft.No;
        txtCreateCount.Size = new Size(159, 22);
        txtCreateCount.TabIndex = 9;
        txtCreateCount.TabStop = false;
        // 
        // lblModifyCount
        // 
        lblModifyCount.Dock = DockStyle.Fill;
        lblModifyCount.Location = new Point(712, 33);
        lblModifyCount.Margin = new Padding(1);
        lblModifyCount.Name = "lblModifyCount";
        lblModifyCount.Size = new Size(106, 28);
        lblModifyCount.TabIndex = 10;
        lblModifyCount.Text = "مرات التعديل";
        lblModifyCount.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtModifyCount
        // 
        txtModifyCount.AccessibleName = "مرات التعديل";
        txtModifyCount.BackColor = Color.FromArgb(244, 244, 244);
        txtModifyCount.BorderStyle = BorderStyle.FixedSingle;
        txtModifyCount.Dock = DockStyle.Fill;
        txtModifyCount.Location = new Point(550, 35);
        txtModifyCount.Margin = new Padding(2, 3, 2, 2);
        txtModifyCount.Name = "txtModifyCount";
        txtModifyCount.ReadOnly = true;
        txtModifyCount.RightToLeft = RightToLeft.No;
        txtModifyCount.Size = new Size(159, 22);
        txtModifyCount.TabIndex = 11;
        txtModifyCount.TabStop = false;
        // 
        // lblCreatedMachine
        // 
        lblCreatedMachine.Dock = DockStyle.Fill;
        lblCreatedMachine.Location = new Point(441, 33);
        lblCreatedMachine.Margin = new Padding(1);
        lblCreatedMachine.Name = "lblCreatedMachine";
        lblCreatedMachine.Size = new Size(106, 28);
        lblCreatedMachine.TabIndex = 12;
        lblCreatedMachine.Text = "الجهاز المدخل";
        lblCreatedMachine.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtCreatedMachine
        // 
        txtCreatedMachine.AccessibleName = "الجهاز المدخل";
        txtCreatedMachine.BackColor = Color.FromArgb(244, 244, 244);
        txtCreatedMachine.BorderStyle = BorderStyle.FixedSingle;
        txtCreatedMachine.Dock = DockStyle.Fill;
        txtCreatedMachine.Location = new Point(279, 35);
        txtCreatedMachine.Margin = new Padding(2, 3, 2, 2);
        txtCreatedMachine.Name = "txtCreatedMachine";
        txtCreatedMachine.ReadOnly = true;
        txtCreatedMachine.RightToLeft = RightToLeft.No;
        txtCreatedMachine.Size = new Size(159, 22);
        txtCreatedMachine.TabIndex = 13;
        txtCreatedMachine.TabStop = false;
        // 
        // lblModifiedMachine
        // 
        lblModifiedMachine.Dock = DockStyle.Fill;
        lblModifiedMachine.Location = new Point(170, 33);
        lblModifiedMachine.Margin = new Padding(1);
        lblModifiedMachine.Name = "lblModifiedMachine";
        lblModifiedMachine.Size = new Size(106, 28);
        lblModifiedMachine.TabIndex = 14;
        lblModifiedMachine.Text = "الجهاز المعدل";
        lblModifiedMachine.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtModifiedMachine
        // 
        txtModifiedMachine.AccessibleName = "الجهاز المعدل";
        txtModifiedMachine.BackColor = Color.FromArgb(244, 244, 244);
        txtModifiedMachine.BorderStyle = BorderStyle.FixedSingle;
        txtModifiedMachine.Dock = DockStyle.Fill;
        txtModifiedMachine.Location = new Point(4, 35);
        txtModifiedMachine.Margin = new Padding(2, 3, 2, 2);
        txtModifiedMachine.Name = "txtModifiedMachine";
        txtModifiedMachine.ReadOnly = true;
        txtModifiedMachine.RightToLeft = RightToLeft.No;
        txtModifiedMachine.Size = new Size(163, 22);
        txtModifiedMachine.TabIndex = 15;
        txtModifiedMachine.TabStop = false;
        // 
        // lblDataStatus
        // 
        lblDataStatus.BackColor = Color.FromArgb(244, 244, 244);
        lblDataStatus.BorderStyle = BorderStyle.FixedSingle;
        lblDataStatus.Dock = DockStyle.Fill;
        lblDataStatus.Location = new Point(3, 619);
        lblDataStatus.Margin = new Padding(0);
        lblDataStatus.Name = "lblDataStatus";
        lblDataStatus.Padding = new Padding(3, 0, 3, 0);
        lblDataStatus.RightToLeft = RightToLeft.No;
        lblDataStatus.Size = new Size(1094, 28);
        lblDataStatus.TabIndex = 5;
        lblDataStatus.Text = "معاينة فقط — تحميل البيانات والحفظ غير متصلين";
        lblDataStatus.TextAlign = ContentAlignment.MiddleRight;
        // 
        // UcOnyxSCREEN0080
        // 
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.FromArgb(244, 244, 244);
        Controls.Add(mainLayout);
        Font = new Font("Tahoma", 9F);
        ForeColor = Color.Black;
        MinimumSize = new Size(900, 500);
        Name = "UcOnyxSCREEN0080";
        RightToLeft = RightToLeft.Yes;
        Size = new Size(1100, 650);
        mainLayout.ResumeLayout(false);
        pnlToolbar.ResumeLayout(false);
        fieldsLayout.ResumeLayout(false);
        fieldsLayout.PerformLayout();
        pnlContent.ResumeLayout(false);
        ((ISupportInitialize)gridBankGroups).EndInit();
        auditInfoContainer.ResumeLayout(false);
        auditInfoContainer.PerformLayout();
        ResumeLayout(false);
    
        // Explicit Designer-owned command catalog.
        designerCommandBar.Dock = DockStyle.Fill;
        designerCommandBar.Location = new Point(3, 37);
        designerCommandBar.Size = new Size(1094, 44);
        designerCommandBar.Margin = new Padding(0, 0, 0, 2);
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
        designerCommandBar.Height = 44;
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
        btnNew.AutoSize = false;
        btnNew.Dock = DockStyle.None;
        btnNew.MinimumSize = Size.Empty;
        btnNew.Size = new Size(26, 24);
        btnNew.Margin = new Padding(1);
        btnNew.FlatStyle = FlatStyle.Flat;
        btnNew.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnNew.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Add;
        btnNew.Text = "";
        designerCommandFlow.Controls.Add(btnNew);
        designerCommandBar.SetCommandRole(btnNew, TransportERP.Desktop.CoreUI.DesignerCommandRole.Add);
        btnEdit.AutoSize = false;
        btnEdit.Dock = DockStyle.None;
        btnEdit.MinimumSize = Size.Empty;
        btnEdit.Size = new Size(26, 24);
        btnEdit.Margin = new Padding(1);
        btnEdit.FlatStyle = FlatStyle.Flat;
        btnEdit.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnEdit.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Edit;
        btnEdit.Text = "";
        designerCommandFlow.Controls.Add(btnEdit);
        designerCommandBar.SetCommandRole(btnEdit, TransportERP.Desktop.CoreUI.DesignerCommandRole.Edit);
        btnDelete.AutoSize = false;
        btnDelete.Dock = DockStyle.None;
        btnDelete.MinimumSize = Size.Empty;
        btnDelete.Size = new Size(26, 24);
        btnDelete.Margin = new Padding(1);
        btnDelete.FlatStyle = FlatStyle.Flat;
        btnDelete.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnDelete.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Delete;
        btnDelete.Text = "";
        designerCommandFlow.Controls.Add(btnDelete);
        designerCommandBar.SetCommandRole(btnDelete, TransportERP.Desktop.CoreUI.DesignerCommandRole.Delete);
        btnUndo.AutoSize = false;
        btnUndo.Dock = DockStyle.None;
        btnUndo.MinimumSize = Size.Empty;
        btnUndo.Size = new Size(26, 24);
        btnUndo.Margin = new Padding(1);
        btnUndo.FlatStyle = FlatStyle.Flat;
        btnUndo.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnUndo.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Cancel;
        btnUndo.Text = "";
        designerCommandFlow.Controls.Add(btnUndo);
        designerCommandBar.SetCommandRole(btnUndo, TransportERP.Desktop.CoreUI.DesignerCommandRole.Cancel);
        btnView.AutoSize = false;
        btnView.Dock = DockStyle.None;
        btnView.MinimumSize = Size.Empty;
        btnView.Size = new Size(26, 24);
        btnView.Margin = new Padding(1);
        btnView.FlatStyle = FlatStyle.Flat;
        btnView.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnView.Image = TransportERP.Desktop.CoreUI.CommandBarImages.View;
        btnView.Text = "";
        designerCommandFlow.Controls.Add(btnView);
        designerCommandBar.SetCommandRole(btnView, TransportERP.Desktop.CoreUI.DesignerCommandRole.View);
        btnLast.AutoSize = false;
        btnLast.Dock = DockStyle.None;
        btnLast.MinimumSize = Size.Empty;
        btnLast.Size = new Size(26, 24);
        btnLast.Margin = new Padding(1);
        btnLast.FlatStyle = FlatStyle.Flat;
        btnLast.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnLast.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Last;
        btnLast.Text = "";
        designerCommandFlow.Controls.Add(btnLast);
        designerCommandBar.SetCommandRole(btnLast, TransportERP.Desktop.CoreUI.DesignerCommandRole.Last);
        btnNext.AutoSize = false;
        btnNext.Dock = DockStyle.None;
        btnNext.MinimumSize = Size.Empty;
        btnNext.Size = new Size(26, 24);
        btnNext.Margin = new Padding(1);
        btnNext.FlatStyle = FlatStyle.Flat;
        btnNext.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnNext.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Next;
        btnNext.Text = "";
        designerCommandFlow.Controls.Add(btnNext);
        designerCommandBar.SetCommandRole(btnNext, TransportERP.Desktop.CoreUI.DesignerCommandRole.Next);
        btnPrevious.AutoSize = false;
        btnPrevious.Dock = DockStyle.None;
        btnPrevious.MinimumSize = Size.Empty;
        btnPrevious.Size = new Size(26, 24);
        btnPrevious.Margin = new Padding(1);
        btnPrevious.FlatStyle = FlatStyle.Flat;
        btnPrevious.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnPrevious.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Previous;
        btnPrevious.Text = "";
        designerCommandFlow.Controls.Add(btnPrevious);
        designerCommandBar.SetCommandRole(btnPrevious, TransportERP.Desktop.CoreUI.DesignerCommandRole.Previous);
        btnFirst.AutoSize = false;
        btnFirst.Dock = DockStyle.None;
        btnFirst.MinimumSize = Size.Empty;
        btnFirst.Size = new Size(26, 24);
        btnFirst.Margin = new Padding(1);
        btnFirst.FlatStyle = FlatStyle.Flat;
        btnFirst.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnFirst.Image = TransportERP.Desktop.CoreUI.CommandBarImages.First;
        btnFirst.Text = "";
        designerCommandFlow.Controls.Add(btnFirst);
        designerCommandBar.SetCommandRole(btnFirst, TransportERP.Desktop.CoreUI.DesignerCommandRole.First);
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
        btnPrint.AutoSize = false;
        btnPrint.Dock = DockStyle.None;
        btnPrint.MinimumSize = Size.Empty;
        btnPrint.Size = new Size(26, 24);
        btnPrint.Margin = new Padding(1);
        btnPrint.FlatStyle = FlatStyle.Flat;
        btnPrint.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnPrint.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Print;
        btnPrint.Text = "";
        designerCommandFlow.Controls.Add(btnPrint);
        designerCommandBar.SetCommandRole(btnPrint, TransportERP.Desktop.CoreUI.DesignerCommandRole.Print);
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

    private TableLayoutPanel mainLayout;
    private Label lblTitle;
    private Panel pnlToolbar;
    private TableLayoutPanel fieldsLayout;
    private Panel pnlContent;
    private DataGridView gridBankGroups;
    private DataGridViewTextBoxColumn colCode;
    private DataGridViewTextBoxColumn colLocalName;
    private DataGridViewTextBoxColumn colForeignName;
    private TableLayoutPanel auditInfoContainer;
    private Label lblDataStatus;
    private Label lblCode;
    private TextBox txtCode;
    private Label lblLocalName;
    private TextBox txtLocalName;
    private Label lblForeignName;
    private TextBox txtForeignName;
    private Label lblCreatedBy;
    private TextBox txtCreatedBy;
    private Label lblModifiedBy;
    private TextBox txtModifiedBy;
    private Label lblCreatedAt;
    private TextBox txtCreatedAt;
    private Label lblModifiedAt;
    private TextBox txtModifiedAt;
    private Label lblCreateCount;
    private TextBox txtCreateCount;
    private Label lblModifyCount;
    private TextBox txtModifyCount;
    private Label lblCreatedMachine;
    private TextBox txtCreatedMachine;
    private Label lblModifiedMachine;
    private TextBox txtModifiedMachine;
    private Button btnNew;
    private Button btnEdit;
    private Button btnSave;
    private Button btnUndo;
    private Button btnDelete;
    private Button btnView;
    private Button btnPrint;
    private Button btnFirst;
    private Button btnPrevious;
    private Button btnNext;
    private Button btnLast;
    private Button btnClose;
}
