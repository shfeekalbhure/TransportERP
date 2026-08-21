using System.ComponentModel;
using TransportERP.Contracts.Waybills;

namespace TransportERP.Desktop.Waybills;

public sealed record ItemReleaseScreenState(
    string Waybill,
    string Item,
    ItemQuantityStateResponse Quantity,
    string HoldStatus);

public sealed record TripAllocationPlanningRow(
    [property: Browsable(false)] Guid? AllocationId,
    [property: Browsable(false)] Guid WaybillItemId,
    [property: Browsable(false)] Guid ReleaseId,
    [property: Browsable(false)] Guid TripId,
    [property: DisplayName("الصنف")] string Item,
    [property: DisplayName("المتبقي المطلق")] decimal ReleasedRemaining,
    [property: DisplayName("الرحلة")] string Trip,
    [property: DisplayName("المسار")] string Route,
    [property: DisplayName("الكمية المخصصة")] decimal AllocatedQuantity,
    [property: DisplayName("حالة التخصيص")] string AllocationStatus);

public sealed record RemainingShippingRow(
    [property: DisplayName("البوليصة")] string Waybill,
    [property: DisplayName("الصنف")] string Item,
    [property: DisplayName("الأصل")] decimal Original,
    [property: DisplayName("المطلق")] decimal Released,
    [property: DisplayName("المخصص")] decimal Allocated,
    [property: DisplayName("المحمّل")] decimal Loaded,
    [property: DisplayName("المتبقي للإطلاق")] decimal RemainingToRelease,
    [property: DisplayName("المطلق غير المخصص")] decimal ReleasedRemaining,
    [property: DisplayName("المخصص غير المحمّل")] decimal AllocatedRemaining,
    [property: DisplayName("سلامة الرصيد")] string IntegrityStatus);

public sealed record ReadyToLoadRow(
    [property: Browsable(false)] Guid TripId,
    [property: DisplayName("البوليصة")] string Waybill,
    [property: DisplayName("الصنف")] string Item,
    [property: DisplayName("الرحلة")] string Trip,
    [property: DisplayName("المخصص غير المحمّل")] decimal AllocatedRemaining,
    [property: DisplayName("المخاطر")] string RiskFlags,
    [property: DisplayName("الأولوية")] string Priority,
    [property: DisplayName("الوجهة")] string Destination);

public sealed record LoadPlanningRow(
    [property: Browsable(false)] Guid TripId,
    [property: Browsable(false)] Guid WaybillItemId,
    [property: Browsable(false)] Guid ReleaseId,
    [property: DisplayName("الرحلة")] string Trip,
    [property: DisplayName("السعة المرجعية")] string Capacity,
    [property: DisplayName("سعة الوزن")] decimal CapacityWeight,
    [property: DisplayName("سعة الحجم")] decimal CapacityVolume,
    [property: DisplayName("الوزن المخصص")] decimal AllocatedWeight,
    [property: DisplayName("الحجم المخصص")] decimal AllocatedVolume,
    [property: DisplayName("الصنف")] string Item,
    [property: DisplayName("الكمية")] decimal Qty,
    [property: DisplayName("المتبقي المطلق")] decimal ReleasedRemaining,
    [property: DisplayName("الأولوية")] string Priority,
    [property: DisplayName("المخاطر")] string RiskFlags,
    [property: DisplayName("حالة السعة")] string CapacityStatus)
{
    public static LoadPlanningRow FromManifestLine(
        Guid tripId,
        Guid releaseId,
        string trip,
        string capacity,
        decimal capacityWeight,
        decimal capacityVolume,
        string item,
        decimal releasedRemaining,
        string priority,
        string riskFlags,
        ManifestLineResponse line)
        => new(
            TripId: tripId,
            WaybillItemId: line.WaybillItemId,
            ReleaseId: releaseId,
            Trip: trip,
            Capacity: capacity,
            CapacityWeight: capacityWeight,
            CapacityVolume: capacityVolume,
            AllocatedWeight: line.Weight,
            AllocatedVolume: line.Volume,
            Item: item,
            Qty: line.Quantity,
            ReleasedRemaining: releasedRemaining,
            Priority: priority,
            RiskFlags: riskFlags,
            CapacityStatus: CapacityStatusText(capacityWeight, capacityVolume, line.Weight, line.Volume));

    private static string CapacityStatusText(
        decimal capacityWeight,
        decimal capacityVolume,
        decimal allocatedWeight,
        decimal allocatedVolume)
    {
        var weight = capacityWeight > 0m ? $"{allocatedWeight:N3}/{capacityWeight:N3}" : $"{allocatedWeight:N3}/غير محدد";
        var volume = capacityVolume > 0m ? $"{allocatedVolume:N3}/{capacityVolume:N3}" : $"{allocatedVolume:N3}/غير محدد";
        var exceeded = (capacityWeight > 0m && allocatedWeight > capacityWeight) ||
                       (capacityVolume > 0m && allocatedVolume > capacityVolume);
        return $"{(exceeded ? "تجاوز مرجعي" : "ضمن المرجع")} — وزن {weight} — حجم {volume}";
    }
}

public sealed record ManifestLoadingRow(
    [property: Browsable(false)] Guid ManifestLineId,
    [property: DisplayName("الصنف")] string Item,
    [property: DisplayName("الكمية المخصصة")] decimal AllocatedQty,
    [property: DisplayName("الكمية المحملة")] decimal LoadedQty,
    [property: DisplayName("الوزن")] decimal Weight,
    [property: DisplayName("المخاطر")] string RiskFlags,
    [property: DisplayName("الحالة")] string Status)
{
    public static ManifestLoadingRow FromManifestLine(
        ManifestLineResponse line,
        string item,
        string riskFlags)
        => new(line.Id, item, line.Quantity, line.LoadedQuantity, line.Weight, riskFlags, line.LoadStatus);
}

public sealed record ManifestScreenState(ManifestResponse Manifest, TripResponse Trip)
{
    public decimal TotalQuantity => Manifest.Lines.Sum(x => x.Quantity);
    public decimal TotalWeight => Manifest.Lines.Sum(x => x.Weight);
    public decimal TotalVolume => Manifest.Lines.Sum(x => x.Volume);
}

public sealed record DepartureScreenState(TripResponse Trip, ManifestResponse Manifest);
