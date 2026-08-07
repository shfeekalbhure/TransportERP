using TransportERP.Desktop.CoreUI.Controls;

namespace TransportERP.Desktop.Forms.Setup.General;

/// <summary>GEN-016 — واجهة المتغيرات العامة المصنفة حسب إعدادات النظام والسياسات والأمن.</summary>
public partial class UcGen016GlobalVariables : UserControl
{
    public UcGen016GlobalVariables() => InitializeComponent();
    internal TransportReferenceScreenShell ScreenShell => screenShell;
}
