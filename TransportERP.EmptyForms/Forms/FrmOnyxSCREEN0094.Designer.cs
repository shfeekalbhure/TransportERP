using System.Drawing;
using System.Windows.Forms;
namespace TransportERP.EmptyForms;
partial class FrmOnyxSCREEN0094
{
    private System.ComponentModel.IContainer? components;
    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }
    private void InitializeComponent()
    {
        content = new UcAccountProjectLinking();
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
        Name = "FrmOnyxSCREEN0094";
        Text = "ربط الحسابات بالمشاريع";
        Tag = "ONYX:SCREEN-0094";
        Controls.Add(content);
        ResumeLayout(false);
    }
    private UcAccountProjectLinking content = null!;
}
