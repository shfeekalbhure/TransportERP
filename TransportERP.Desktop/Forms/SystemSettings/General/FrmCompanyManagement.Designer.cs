#nullable enable
using System.Drawing;
using System.Windows.Forms;

namespace TransportERP.Desktop.Forms.SystemSettings.General;

partial class FrmCompanyManagement
{
    private System.ComponentModel.IContainer? components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        companyManagement = new UcCompanyManagement();
        SuspendLayout();
        // 
        // companyManagement
        // 
        companyManagement.BackColor = Color.FromArgb(248, 250, 252);
        companyManagement.Dock = DockStyle.Fill;
        companyManagement.Font = new Font("Segoe UI", 10F);
        companyManagement.Location = new Point(0, 0);
        companyManagement.Margin = new Padding(4, 4, 4, 4);
        companyManagement.Name = "companyManagement";
        companyManagement.RightToLeft = RightToLeft.Yes;
        companyManagement.Size = new Size(1280, 720);
        companyManagement.TabIndex = 0;
        companyManagement.Tag = "SCR-SET-004";
        companyManagement.CloseRequested += CompanyManagement_CloseRequested;
        companyManagement.Load += companyManagement_Load;
        // 
        // FrmCompanyManagement
        // 
        AutoScaleDimensions = new SizeF(120F, 120F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(1280, 720);
        Controls.Add(companyManagement);
        Name = "FrmCompanyManagement";
        Tag = "SCR-SET-004";
        Text = "إدارة الشركات";
        ResumeLayout(false);
    }

    private UcCompanyManagement companyManagement = null!;
}
