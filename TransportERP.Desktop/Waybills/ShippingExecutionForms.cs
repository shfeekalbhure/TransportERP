using System.Drawing.Printing;
using TransportERP.Contracts.Waybills;
using TransportERP.Desktop.CoreUI.Architecture;

namespace TransportERP.Desktop.Waybills;

public static class ShippingExecutionScreenCatalog
{
    public static readonly WaybillScreenDefinition Release = new("SHP-015", "إطلاق كميات الأصناف", TransportScreenProfile.Transaction);
    public static readonly WaybillScreenDefinition Allocation = new("SHP-016", "توزيع الأصناف على الرحلات", TransportScreenProfile.Transaction);
    public static readonly WaybillScreenDefinition Remaining = new("SHP-019", "المتبقي غير المرحل", TransportScreenProfile.ReportInquiry);
    public static readonly WaybillScreenDefinition ReadyToLoad = new("SHP-023", "البوالص الجاهزة للتحميل", TransportScreenProfile.ReportInquiry);
    public static readonly WaybillScreenDefinition LoadPlanning = new("SHP-024", "تخطيط الحمولة", TransportScreenProfile.Transaction);
    public static readonly WaybillScreenDefinition Trip = new("SHP-025", "إنشاء الرحلة", TransportScreenProfile.Transaction);
    public static readonly WaybillScreenDefinition Loading = new("SHP-027", "تحميل الرحلة", TransportScreenProfile.Transaction);
    public static readonly WaybillScreenDefinition Manifest = new("SHP-028", "كشف الحمولة", TransportScreenProfile.Transaction);
    public static readonly WaybillScreenDefinition Handover = new("SHP-029", "تسليم عهدة الحمولة للسائق", TransportScreenProfile.ControlApproval);
    public static readonly WaybillScreenDefinition Departure = new("SHP-030", "انطلاق الرحلة", TransportScreenProfile.ControlApproval);
}

public abstract class ShippingRtlForm : Form
{
    protected ShippingRtlForm(string title, int width = 900, int height = 520)
    {
        Text = title;
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterParent;
        MinimumSize = new Size(width, height);
    }

}
