using TransportERP.Desktop.Views.Security.Shared;

namespace TransportERP.Desktop.Views.Security;

/// <summary>SEC-031 — إعدادات الأمان العامة؛ قيم أمنية عامة لا تنتمي إلى سياسة متخصصة ولا تسمح بإضعاف الحد الأدنى للنظام.</summary>
public partial class UcGeneralSecuritySettings : UserControl
{
    public UcGeneralSecuritySettings()
    {
        InitializeComponent();
        SecurityViewRuntime.Initialize(this, screenShell, "إعدادات الأمان العامة", "", SecurityWorkspaceMode.Settings);
    }
}