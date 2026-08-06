using System.Windows.Forms;

namespace TransportERP.Desktop.Forms.Setup.General;

public partial class FrmVehicleTypes : Form
{
    public FrmVehicleTypes()
    {
        InitializeComponent();

        // Hosted-screen appearance: the form is rendered as workspace content,
        // without a separate Windows title bar or window controls.
        FormBorderStyle = FormBorderStyle.None;
        ControlBox = false;
        MinimizeBox = false;
        MaximizeBox = false;
        ShowIcon = false;
        ShowInTaskbar = false;
        Dock = DockStyle.Fill;
    }
}
