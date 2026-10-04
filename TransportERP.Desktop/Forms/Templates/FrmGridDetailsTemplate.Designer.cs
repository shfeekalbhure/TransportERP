#nullable enable

using TransportERP.Desktop.CoreUI;

namespace TransportERP.Desktop.Forms.Templates;

partial class FrmGridDetailsTemplate
{
    private System.ComponentModel.IContainer? components; private Panel pnlHeader = null!; private Label lblTitle = null!; private Label lblSubtitle = null!; private Panel pnlFilters = null!; private TextBox txtSearch = null!; private SplitContainer splitMain = null!; private DataGridView dgvItems = null!; private GroupBox grpDetails = null!; private Panel pnlDetailsPlaceholder = null!; private Panel pnlActions = null!; private FlowLayoutPanel flpActions = null!; private Button btnSave = null!; private Button btnClose = null!; private StatusStrip statusStrip = null!; private ToolStripStatusLabel lblStatus = null!;
    protected override void Dispose(bool disposing) { if (disposing && components is not null) components.Dispose(); base.Dispose(disposing); }
    private void InitializeComponent()
    {
        pnlHeader = new Panel();
        lblSubtitle = new Label();
        lblTitle = new Label();
        pnlFilters = new Panel();
        txtSearch = new TextBox();
        splitMain = new SplitContainer();
        dgvItems = new DataGridView();
        grpDetails = new GroupBox();
        pnlDetailsPlaceholder = new Panel();
        pnlActions = new Panel();
        flpActions = new FlowLayoutPanel();
        btnSave = new Button();
        btnClose = new Button();
        statusStrip = new StatusStrip();
        lblStatus = new ToolStripStatusLabel();
        pnlHeader.SuspendLayout();
        pnlFilters.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
        splitMain.Panel1.SuspendLayout();
        splitMain.Panel2.SuspendLayout();
        splitMain.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvItems).BeginInit();
        grpDetails.SuspendLayout();
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
        pnlHeader.TabIndex = 5;
        // 
        // lblSubtitle
        // 
        lblSubtitle.Dock = DockStyle.Fill;
        lblSubtitle.Location = new Point(0, 45);
        lblSubtitle.Name = "lblSubtitle";
        lblSubtitle.Size = new Size(1180, 55);
        lblSubtitle.TabIndex = 0;
        lblSubtitle.Text = "مرشح وقائمة وتفاصيل قابلة للتوسع";
        // 
        // lblTitle
        // 
        lblTitle.Dock = DockStyle.Top;
        lblTitle.Location = new Point(0, 0);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(1180, 45);
        lblTitle.TabIndex = 1;
        lblTitle.Text = "عنوان Grid + Details";
        // 
        // pnlFilters
        // 
        pnlFilters.Controls.Add(txtSearch);
        pnlFilters.Dock = DockStyle.Top;
        pnlFilters.Location = new Point(0, 100);
        pnlFilters.Name = "pnlFilters";
        pnlFilters.Padding = new Padding(16);
        pnlFilters.Size = new Size(1180, 66);
        pnlFilters.TabIndex = 2;
        // 
        // txtSearch
        // 
        txtSearch.Dock = DockStyle.Fill;
        txtSearch.Location = new Point(16, 16);
        txtSearch.Name = "txtSearch";
        txtSearch.PlaceholderText = "بحث";
        txtSearch.Size = new Size(1148, 30);
        txtSearch.TabIndex = 0;
        // 
        // splitMain
        // 
        splitMain.Dock = DockStyle.Fill;
        splitMain.Location = new Point(0, 166);
        splitMain.Name = "splitMain";
        // 
        // splitMain.Panel1
        // 
        splitMain.Panel1.Controls.Add(dgvItems);
        splitMain.Panel1.RightToLeft = RightToLeft.Yes;
        splitMain.Panel1MinSize = 520;
        // 
        // splitMain.Panel2
        // 
        splitMain.Panel2.Controls.Add(grpDetails);
        splitMain.Panel2.RightToLeft = RightToLeft.Yes;
        splitMain.Panel2MinSize = 360;
        splitMain.RightToLeft = RightToLeft.Yes;
        splitMain.Size = new Size(1180, 488);
        splitMain.SplitterDistance = 680;
        splitMain.TabIndex = 1;
        // 
        // dgvItems
        // 
        dgvItems.ColumnHeadersHeight = 29;
        dgvItems.Dock = DockStyle.Fill;
        dgvItems.Location = new Point(0, 0);
        dgvItems.Name = "dgvItems";
        dgvItems.RowHeadersWidth = 51;
        dgvItems.Size = new Size(680, 488);
        dgvItems.TabIndex = 0;
        // 
        // grpDetails
        // 
        grpDetails.Controls.Add(pnlDetailsPlaceholder);
        grpDetails.Dock = DockStyle.Fill;
        grpDetails.Location = new Point(0, 0);
        grpDetails.Name = "grpDetails";
        grpDetails.Size = new Size(496, 488);
        grpDetails.TabIndex = 0;
        grpDetails.TabStop = false;
        grpDetails.Text = "التفاصيل";
        // 
        // pnlDetailsPlaceholder
        // 
        pnlDetailsPlaceholder.Dock = DockStyle.Fill;
        pnlDetailsPlaceholder.Location = new Point(3, 26);
        pnlDetailsPlaceholder.Name = "pnlDetailsPlaceholder";
        pnlDetailsPlaceholder.Size = new Size(490, 459);
        pnlDetailsPlaceholder.TabIndex = 0;
        // 
        // pnlActions
        // 
        pnlActions.Controls.Add(flpActions);
        pnlActions.Dock = DockStyle.Bottom;
        pnlActions.Location = new Point(0, 654);
        pnlActions.Name = "pnlActions";
        pnlActions.Size = new Size(1180, 60);
        pnlActions.TabIndex = 3;
        // 
        // flpActions
        // 
        flpActions.Controls.Add(btnSave);
        flpActions.Controls.Add(btnClose);
        flpActions.Dock = DockStyle.Fill;
        flpActions.FlowDirection = FlowDirection.RightToLeft;
        flpActions.Location = new Point(0, 0);
        flpActions.Name = "flpActions";
        flpActions.Size = new Size(1180, 60);
        flpActions.TabIndex = 0;
        // 
        // btnSave
        // 
        btnSave.Location = new Point(3, 3);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(75, 44);
        btnSave.TabIndex = 0;
        btnSave.Text = "حفظ";
        // 
        // btnClose
        // 
        btnClose.Location = new Point(84, 3);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(75, 44);
        btnClose.TabIndex = 1;
        btnClose.Text = "إغلاق";
        // 
        // statusStrip
        // 
        statusStrip.ImageScalingSize = new Size(20, 20);
        statusStrip.Items.AddRange(new ToolStripItem[] { lblStatus });
        statusStrip.Location = new Point(0, 714);
        statusStrip.Name = "statusStrip";
        statusStrip.Size = new Size(1180, 26);
        statusStrip.TabIndex = 4;
        // 
        // lblStatus
        // 
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(38, 20);
        lblStatus.Text = "جاهز";
        // 
        // FrmGridDetailsTemplate
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1180, 740);
        Controls.Add(splitMain);
        Controls.Add(pnlFilters);
        Controls.Add(pnlActions);
        Controls.Add(statusStrip);
        Controls.Add(pnlHeader);
        Name = "FrmGridDetailsTemplate";
        Text = "Grid + Details Template";
        pnlHeader.ResumeLayout(false);
        pnlFilters.ResumeLayout(false);
        pnlFilters.PerformLayout();
        splitMain.Panel1.ResumeLayout(false);
        splitMain.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
        splitMain.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvItems).EndInit();
        grpDetails.ResumeLayout(false);
        pnlActions.ResumeLayout(false);
        flpActions.ResumeLayout(false);
        statusStrip.ResumeLayout(false);
        statusStrip.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }
}
