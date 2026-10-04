using System.Drawing;
using System.Windows.Forms;
namespace TransportERP.EmptyForms;
partial class FrmScreen_04_01_02
{
    private System.ComponentModel.IContainer? components;
    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }
    private void InitializeComponent()
    {
        content = new UcAccountGroupsAndTypes();
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
        Name = "FrmScreen_04_01_02";
        Text = "مجموعات وأنواع الحسابات";
        Tag = "04.01.02";
        Controls.Add(content);
        ResumeLayout(false);
    }
    private UcAccountGroupsAndTypes content = null!;
}
