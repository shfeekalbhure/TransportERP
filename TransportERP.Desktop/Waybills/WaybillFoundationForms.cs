using TransportERP.Contracts.Waybills;
using TransportERP.Desktop.CoreUI.Architecture;

namespace TransportERP.Desktop.Waybills;

public static class WaybillFoundationScreenCatalog
{
    public static readonly WaybillScreenDefinition Header = new("SHP-005", "رأس البوليصة", TransportScreenProfile.Transaction);
    public static readonly WaybillScreenDefinition Parties = new("SHP-006", "أطراف البوليصة", TransportScreenProfile.Transaction);
    public static readonly WaybillScreenDefinition Items = new("SHP-007", "أصناف البوليصة", TransportScreenProfile.Transaction);
    public static readonly WaybillScreenDefinition Measurements = new("SHP-008", "الأوزان والأبعاد والقيم", TransportScreenProfile.Transaction);
    public static readonly WaybillScreenDefinition Approval = new("SHP-014", "اعتماد البوليصة", TransportScreenProfile.ControlApproval);
}

public sealed record WaybillScreenDefinition(string ScreenCode, string ArabicTitle, TransportScreenProfile Profile);
