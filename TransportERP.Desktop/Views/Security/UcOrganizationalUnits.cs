using TransportERP.Desktop.Views.Security.Shared;

namespace TransportERP.Desktop.Views.Security;

/// <summary>SEC-026 — الوحدات التنظيمية؛ شاشة شجرية متعددة المستويات وفق النمط P005.</summary>
public partial class UcOrganizationalUnits : UserControl
{
    public UcOrganizationalUnits()
    {
        InitializeComponent();
        SecurityViewRuntime.Initialize(this, screenShell, "الوحدات التنظيمية", "ابحث بالرمز أو الاسم أو المدير...", SecurityWorkspaceMode.Tree);
    }
}