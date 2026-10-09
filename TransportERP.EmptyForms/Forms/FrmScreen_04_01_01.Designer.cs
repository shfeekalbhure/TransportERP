using System.Drawing;
using System.Windows.Forms;
namespace TransportERP.EmptyForms;
partial class FrmScreen_04_01_01
{
    private System.ComponentModel.IContainer? components;
    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }
    private void InitializeComponent()
    {
        content = new UcChartOfAccounts();
        SuspendLayout();
        content.Dock = DockStyle.Fill;
        content.Name = "content";
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(1200, 900);
        MinimumSize = new Size(960, 640);
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterParent;
        Name = "FrmScreen_04_01_01";
        Text = "دليل الحسابات";
        Tag = "04.01.01";
        Controls.Add(content);
        ResumeLayout(false);
    }
    private UcChartOfAccounts content = null!;
}
