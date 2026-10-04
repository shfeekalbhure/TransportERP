#nullable enable

using TransportERP.Desktop.CoreUI;

namespace TransportERP.Desktop.Forms.Templates;

partial class FrmMasterDataTemplate
{
    private System.ComponentModel.IContainer? components; private Panel pnlHeader = null!; private Label lblTitle = null!; private Label lblSubtitle = null!; private SplitContainer splitMain = null!; private DataGridView dgvItems = null!; private GroupBox grpDetails = null!; private TableLayoutPanel tlpDetails = null!; private Label lblCode = null!; private TextBox txtCode = null!; private Label lblName = null!; private TextBox txtName = null!; private CheckBox chkActive = null!; private Panel pnlActions = null!; private FlowLayoutPanel flpActions = null!; private Button btnNew = null!; private Button btnSave = null!; private Button btnEdit = null!; private Button btnDisable = null!; private Button btnClose = null!; private StatusStrip statusStrip = null!; private ToolStripStatusLabel lblStatus = null!;
    protected override void Dispose(bool disposing) { if (disposing && components is not null) components.Dispose(); base.Dispose(disposing); }
    private void InitializeComponent()
    {
        pnlHeader = new Panel();
        lblSubtitle = new Label();
        lblTitle = new Label();
        splitMain = new SplitContainer();
        dgvItems = new DataGridView();
        grpDetails = new GroupBox();
        tlpDetails = new TableLayoutPanel();
        lblCode = new Label();
        txtCode = new TextBox();
        lblName = new Label();
        txtName = new TextBox();
        chkActive = new CheckBox();
        pnlActions = new Panel();
        flpActions = new FlowLayoutPanel();
        btnNew = new Button();
        btnSave = new Button();
        btnEdit = new Button();
        btnDisable = new Button();
        btnClose = new Button();
        statusStrip = new StatusStrip();
        lblStatus = new ToolStripStatusLabel();
        pnlHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
        splitMain.Panel1.SuspendLayout();
        splitMain.Panel2.SuspendLayout();
        splitMain.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvItems).BeginInit();
        grpDetails.SuspendLayout();
        tlpDetails.SuspendLayout();
        pnlActions.SuspendLayout();
        flpActions.SuspendLayout();
        statusStrip.SuspendLayout();
        SuspendLayout();
        // 
        // pnlHeader
        // 
        pnlHeader.Controls.Add(lblSubtitle);
        pnlHeader.Controls.Add(lblTitle);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Location = new Point(0, 0);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Size = new Size(1180, 100);
        pnlHeader.TabIndex = 3;
        // 
        // lblSubtitle
        // 
        lblSubtitle.Dock = DockStyle.Fill;
        lblSubtitle.Location = new Point(0, 45);
        lblSubtitle.Name = "lblSubtitle";
        lblSubtitle.Size = new Size(1180, 55);
        lblSubtitle.TabIndex = 0;
        lblSubtitle.Text = "قائمة وتعريف عنصر أساسي قابل لإعادة الاستخدام";
        // 
        // lblTitle
        // 
        lblTitle.Dock = DockStyle.Top;
        lblTitle.Location = new Point(0, 0);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(1180, 45);
        lblTitle.TabIndex = 1;
        lblTitle.Text = "عنوان البيانات الأساسية";
        // 
        // splitMain
        // 
        splitMain.Dock = DockStyle.Fill;
        splitMain.Location = new Point(0, 100);
        splitMain.Name = "splitMain";
        // 
        // splitMain.Panel1
        // 
        splitMain.Panel1.Controls.Add(dgvItems);
        splitMain.Panel1.RightToLeft = RightToLeft.Yes;
        splitMain.Panel1MinSize = 500;
        // 
        // splitMain.Panel2
        // 
        splitMain.Panel2.Controls.Add(grpDetails);
        splitMain.Panel2.RightToLeft = RightToLeft.Yes;
        splitMain.Panel2MinSize = 360;
        splitMain.RightToLeft = RightToLeft.Yes;
        splitMain.Size = new Size(1180, 554);
        splitMain.SplitterDistance = 650;
        splitMain.TabIndex = 0;
        // 
        // dgvItems
        // 
        dgvItems.ColumnHeadersHeight = 29;
        dgvItems.Dock = DockStyle.Fill;
        dgvItems.Location = new Point(0, 0);
        dgvItems.Name = "dgvItems";
        dgvItems.RowHeadersWidth = 51;
        dgvItems.Size = new Size(650, 554);
        dgvItems.TabIndex = 0;
        // 
        // grpDetails
        // 
        grpDetails.Controls.Add(tlpDetails);
        grpDetails.Dock = DockStyle.Fill;
        grpDetails.Location = new Point(0, 0);
        grpDetails.Name = "grpDetails";
        grpDetails.Size = new Size(526, 554);
        grpDetails.TabIndex = 0;
        grpDetails.TabStop = false;
        grpDetails.Text = "التفاصيل";
        // 
        // tlpDetails
        // 
        tlpDetails.ColumnCount = 2;
        tlpDetails.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
        tlpDetails.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpDetails.Controls.Add(lblCode, 0, 0);
        tlpDetails.Controls.Add(txtCode, 1, 0);
        tlpDetails.Controls.Add(lblName, 0, 1);
        tlpDetails.Controls.Add(txtName, 1, 1);
        tlpDetails.Controls.Add(chkActive, 1, 2);
        tlpDetails.Dock = DockStyle.Fill;
        tlpDetails.Location = new Point(3, 26);
        tlpDetails.Name = "tlpDetails";
        tlpDetails.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
        tlpDetails.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
        tlpDetails.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
        tlpDetails.Size = new Size(520, 525);
        tlpDetails.TabIndex = 0;
        // 
        // lblCode
        // 
        lblCode.Dock = DockStyle.Fill;
        lblCode.Location = new Point(403, 0);
        lblCode.Name = "lblCode";
        lblCode.Size = new Size(114, 48);
        lblCode.TabIndex = 0;
        lblCode.Text = "الكود *";
        // 
        // txtCode
        // 
        txtCode.Dock = DockStyle.Fill;
        txtCode.Location = new Point(3, 3);
        txtCode.Name = "txtCode";
        txtCode.Size = new Size(394, 30);
        txtCode.TabIndex = 0;
        // 
        // lblName
        // 
        lblName.Dock = DockStyle.Fill;
        lblName.Location = new Point(403, 48);
        lblName.Name = "lblName";
        lblName.Size = new Size(114, 48);
        lblName.TabIndex = 1;
        lblName.Text = "الاسم *";
        // 
        // txtName
        // 
        txtName.Dock = DockStyle.Fill;
        txtName.Location = new Point(3, 51);
        txtName.Name = "txtName";
        txtName.Size = new Size(394, 30);
        txtName.TabIndex = 1;
        // 
        // chkActive
        // 
        chkActive.Location = new Point(293, 99);
        chkActive.Name = "chkActive";
        chkActive.Size = new Size(104, 24);
        chkActive.TabIndex = 2;
        chkActive.Text = "نشط";
        // 
        // pnlActions
        // 
        pnlActions.Controls.Add(flpActions);
        pnlActions.Dock = DockStyle.Bottom;
        pnlActions.Location = new Point(0, 654);
        pnlActions.Name = "pnlActions";
        pnlActions.Size = new Size(1180, 60);
        pnlActions.TabIndex = 1;
        // 
        // flpActions
        // 
        flpActions.Controls.Add(btnNew);
        flpActions.Controls.Add(btnSave);
        flpActions.Controls.Add(btnEdit);
        flpActions.Controls.Add(btnDisable);
        flpActions.Controls.Add(btnClose);
        flpActions.Dock = DockStyle.Fill;
        flpActions.FlowDirection = FlowDirection.RightToLeft;
        flpActions.Location = new Point(0, 0);
        flpActions.Name = "flpActions";
        flpActions.Size = new Size(1180, 60);
        flpActions.TabIndex = 0;
        // 
        // btnNew
        // 
        btnNew.Location = new Point(3, 3);
        btnNew.Name = "btnNew";
        btnNew.Size = new Size(75, 23);
        btnNew.TabIndex = 0;
        btnNew.Text = "جديد";
        // 
        // btnSave
        // 
        btnSave.Location = new Point(84, 3);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(75, 23);
        btnSave.TabIndex = 1;
        btnSave.Text = "حفظ";
        // 
        // btnEdit
        // 
        btnEdit.Location = new Point(165, 3);
        btnEdit.Name = "btnEdit";
        btnEdit.Size = new Size(75, 23);
        btnEdit.TabIndex = 2;
        btnEdit.Text = "تعديل";
        // 
        // btnDisable
        // 
        btnDisable.Location = new Point(246, 3);
        btnDisable.Name = "btnDisable";
        btnDisable.Size = new Size(75, 23);
        btnDisable.TabIndex = 3;
        btnDisable.Text = "إيقاف";
        // 
        // btnClose
        // 
        btnClose.Location = new Point(327, 3);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(75, 23);
        btnClose.TabIndex = 4;
        btnClose.Text = "إغلاق";
        // 
        // statusStrip
        // 
        statusStrip.ImageScalingSize = new Size(20, 20);
        statusStrip.Items.AddRange(new ToolStripItem[] { lblStatus });
        statusStrip.Location = new Point(0, 714);
        statusStrip.Name = "statusStrip";
        statusStrip.Size = new Size(1180, 26);
        statusStrip.TabIndex = 2;
        // 
        // lblStatus
        // 
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(38, 20);
        lblStatus.Text = "جاهز";
        // 
        // FrmMasterDataTemplate
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1180, 740);
        Controls.Add(splitMain);
        Controls.Add(pnlActions);
        Controls.Add(statusStrip);
        Controls.Add(pnlHeader);
        Name = "FrmMasterDataTemplate";
        Text = "Master Data Form Template";
        pnlHeader.ResumeLayout(false);
        splitMain.Panel1.ResumeLayout(false);
        splitMain.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
        splitMain.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvItems).EndInit();
        grpDetails.ResumeLayout(false);
        tlpDetails.ResumeLayout(false);
        tlpDetails.PerformLayout();
        pnlActions.ResumeLayout(false);
        flpActions.ResumeLayout(false);
        statusStrip.ResumeLayout(false);
        statusStrip.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }
}

