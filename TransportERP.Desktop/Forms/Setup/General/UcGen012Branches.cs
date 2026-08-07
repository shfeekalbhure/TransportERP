using TransportERP.Desktop.CoreUI.Controls;

namespace TransportERP.Desktop.Forms.Setup.General;

/// <summary>GEN-012 — واجهة الفروع المرتبطة بالشركات والنطاق التشغيلي.</summary>
public partial class UcGen012Branches : UserControl
{
    public UcGen012Branches() => InitializeComponent();
    internal TransportReferenceScreenShell ScreenShell => screenShell;
}
