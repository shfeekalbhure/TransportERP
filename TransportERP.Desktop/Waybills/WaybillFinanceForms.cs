using TransportERP.Contracts.Waybills;
using TransportERP.Desktop.CoreUI.Architecture;

namespace TransportERP.Desktop.Waybills;

public static class WaybillFinanceScreenCatalog
{
    public static readonly WaybillScreenDefinition Pricing = new("SHP-009", "الخدمات والرسوم", TransportScreenProfile.Transaction);
    public static readonly WaybillScreenDefinition PaymentPlan = new("SHP-010", "خطة الدفع", TransportScreenProfile.Transaction);
    public static readonly WaybillScreenDefinition Collections = new("SHP-011", "تحصيلات البوليصة", TransportScreenProfile.Transaction);
    public static readonly WaybillScreenDefinition FinancialStatus = new("SHP-012", "حالة السداد والمتبقي", TransportScreenProfile.ReportInquiry);
}
