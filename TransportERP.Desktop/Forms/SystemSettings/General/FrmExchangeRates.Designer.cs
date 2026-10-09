using System.Drawing;
using System.Windows.Forms;
namespace TransportERP.Desktop.Forms.SystemSettings.General;
partial class FrmExchangeRates
{
    private System.ComponentModel.IContainer? components;
    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }
    private void InitializeComponent()
    {
        content = new UcExchangeRates();
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
        Name = "FrmExchangeRates";
        Text = "أسعار الصرف";
        Tag = "02.04.03";
        Controls.Add(content);
        ResumeLayout(false);
    }
    private UcExchangeRates content = null!;
}
