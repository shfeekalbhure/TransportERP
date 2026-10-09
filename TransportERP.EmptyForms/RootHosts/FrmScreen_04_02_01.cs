using System;
using System.Drawing;
using System.Windows.Forms;
using TransportERP.Desktop.CoreUI;

namespace TransportERP.EmptyForms;

public partial class FrmScreen_04_02_01 : Form
{
    public FrmScreen_04_02_01()
    {
        var screen = new UcScreen_04_02_01();
        AutoScaleMode = AutoScaleMode.None;
        Text = screen.Text;
        ClientSize = screen.Size;
        screen.Dock = DockStyle.Fill;
        screen.CloseRequested += (_, _) => Close();
        Controls.Add(screen);
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
        InitializeComponent();
    }
}
