using TransportERP.Desktop.Services;

namespace TransportERP.Desktop;

/// <summary>
/// يربط شاشة الدول بخدمة الخطوط المركزية للنظام.
/// </summary>
public partial class FrmCountries
{
    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        UiTypographyService.Apply(this, "GEN-003");
    }
}
