namespace TransportERP.Desktop.CoreUI;

/// <summary>الأساس الموحد لشاشات النظام العربية.</summary>
public partial class FrmBase : Form
{
    public FrmBase()
    {
        InitializeComponent();
        SettingsFormStyle.ApplyFormStyle(this);
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    }
}
