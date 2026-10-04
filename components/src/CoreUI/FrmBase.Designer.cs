#nullable enable

namespace TransportERP.Desktop.CoreUI;

public partial class FrmBase
{
    private System.ComponentModel.IContainer? components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components is not null)
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        SuspendLayout();
        //
        // FrmBase
        //
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        Name = "FrmBase";
        ResumeLayout(false);
    }
}
