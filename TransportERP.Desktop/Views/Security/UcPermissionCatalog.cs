using TransportERP.Desktop.Views.Security.Shared;

namespace TransportERP.Desktop.Views.Security;

/// <summary>SEC-020 — كتالوج الصلاحيات؛ المصدر المركزي لتعريف مفاتيح الصلاحيات النظامية.</summary>
public partial class UcPermissionCatalog : UserControl
{
    public UcPermissionCatalog()
    {
        InitializeComponent();
        SecurityViewRuntime.Initialize(this, screenShell, "كتالوج الصلاحيات", "ابحث في كتالوج الصلاحيات...", SecurityWorkspaceMode.Edit);
    }
}