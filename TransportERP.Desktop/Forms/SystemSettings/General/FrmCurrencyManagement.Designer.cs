using System.Drawing;
using System.Windows.Forms;
namespace TransportERP.Desktop.Forms.SystemSettings.General;
partial class FrmCurrencyManagement
{
    private System.ComponentModel.IContainer? components;
    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }
    private void InitializeComponent()
    {
        content = new UcCurrencyManagement();
        SuspendLayout();
        content.Dock = DockStyle.Fill;
        content.Name = "content";
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(1120, 720);
        MinimumSize = new Size(960, 640);
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterParent;
        Name = "FrmCurrencyManagement";
        Text = "إدارة العملات";
        Tag = "02.04.02";
        Controls.Add(content);
        ResumeLayout(false);
    }
    private UcCurrencyManagement content = null!;
}
