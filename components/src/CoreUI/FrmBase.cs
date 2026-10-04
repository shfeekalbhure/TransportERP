namespace TransportERP.Desktop.CoreUI;

/// <summary>الأساس الموحد لشاشات النظام العربية.</summary>
public partial class FrmBase : Form
{
    public FrmBase()
    {
        InitializeComponent();
        SettingsFormStyle.ApplyFormStyle(this);
    }
}
