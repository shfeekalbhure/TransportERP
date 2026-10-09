using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TransportERP.Desktop.CoreUI;

namespace TransportERP.Desktop.Forms.SystemSettings.DocumentControl;

// نافذة تستضيف شاشة ترقيم المستندات.
public sealed partial class FrmDocumentNumbering : FrmBase
{
    public FrmDocumentNumbering()
    {
        var content = new UcDocumentNumbering
        {
            Dock = DockStyle.Fill
        };
        SuspendLayout();
        Name = nameof(FrmDocumentNumbering);
        Text = "ترقيم المستندات";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        AutoScaleMode = AutoScaleMode.None;
        ClientSize = new Size(1200, 900);
        MinimumSize = SizeFromClientSize(new Size(1000, 700));
        StartPosition = FormStartPosition.CenterParent;
        Controls.Add(content);
        CancelButton = content.CloseButton;
        content.CloseRequested += (_, _) => Close();
        ResumeLayout(true);
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
        InitializeComponent();
    }
}
