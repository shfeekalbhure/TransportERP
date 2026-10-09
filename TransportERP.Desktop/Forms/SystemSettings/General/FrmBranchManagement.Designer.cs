using System.Drawing;
using System.Windows.Forms;
using TransportERP.Desktop.Forms.SystemSettings.General.الفروع;

namespace TransportERP.Desktop.Forms.SystemSettings.General;

partial class FrmBranchManagement
{
    private System.ComponentModel.IContainer? components;
    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        pnlBranchViewport = new Panel();
        branchManagement = new UcBranchManagement();
        pnlBranchViewport.SuspendLayout();
        SuspendLayout();
        pnlBranchViewport.Name = "pnlBranchViewport";
        pnlBranchViewport.Dock = DockStyle.Fill;
        pnlBranchViewport.AutoScroll = true;
        branchManagement.Name = "branchManagement";
        branchManagement.Dock = DockStyle.Fill;
        // The screen's tab pages scroll internally; keep its footer inside the host viewport.
        branchManagement.MinimumSize = Size.Empty;
        branchManagement.Size = new Size(1200, 900);
        pnlBranchViewport.Controls.Add(branchManagement);
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        Font = new Font("Segoe UI", 9F);
        ClientSize = new Size(1200, 900);
        MinimumSize = new Size(980, 680);
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterParent;
        Name = "FrmBranchManagement";
        Text = "إدارة الفروع";
        Tag = "SCR-SET-005";
        Controls.Add(pnlBranchViewport);
        pnlBranchViewport.ResumeLayout(false);
        ResumeLayout(false);
    }

    private Panel pnlBranchViewport = null!;
    private UcBranchManagement branchManagement = null!;
}
