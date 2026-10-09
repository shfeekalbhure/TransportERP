using System.Drawing;
using System.Windows.Forms;
namespace TransportERP.EmptyForms;
partial class FrmOnyxSCREEN0093
{
    private System.ComponentModel.IContainer? components;
    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }
    private void InitializeComponent()
    {
        content = new UcAccountCostCenterLinking();
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
        Name = "FrmOnyxSCREEN0093";
        Text = "ربط الحسابات بالمراكز";
        Tag = "ONYX:SCREEN-0093";
        Controls.Add(content);
        ResumeLayout(false);
    }
    private UcAccountCostCenterLinking content = null!;
}
