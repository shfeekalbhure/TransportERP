using System.Drawing;
using System.Windows.Forms;
namespace TransportERP.EmptyForms;
partial class FrmOnyxSCREEN0078
{
    private System.ComponentModel.IContainer? components;
    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }
    private void InitializeComponent()
    {
        content = new UcGeneralLedgerSettings();
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
        Name = "FrmOnyxSCREEN0078";
        Text = "متغيرات الأستاذ العام";
        Tag = "ONYX:SCREEN-0078";
        Controls.Add(content);
        ResumeLayout(false);
    }
    private UcGeneralLedgerSettings content = null!;
}
