using TransportERP.Desktop.CoreUI.Controls;

namespace TransportERP.Desktop.Forms.Setup.General;

/// <summary>GEN-008 — واجهة إدارة أنواع المركبات ضمن التهيئة العامة.</summary>
public partial class UcGen008VehicleTypes : UserControl
{
    public UcGen008VehicleTypes() => InitializeComponent();

    internal TransportReferenceScreenShell ScreenShell => screenShell;
}
