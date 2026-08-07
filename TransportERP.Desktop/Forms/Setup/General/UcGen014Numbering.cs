using TransportERP.Desktop.CoreUI.Controls;

namespace TransportERP.Desktop.Forms.Setup.General;

/// <summary>GEN-014 — واجهة سياسات الترقيم المركزي والعدادات والاستثناءات دون MAX+1.</summary>
public partial class UcGen014Numbering : UserControl
{
    public UcGen014Numbering() => InitializeComponent();
    internal TransportReferenceScreenShell ScreenShell => screenShell;
}
