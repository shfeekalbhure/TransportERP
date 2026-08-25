using Microsoft.EntityFrameworkCore;
using TransportERP.Contracts.Waybills;

namespace TransportERP.Infrastructure.Persistence;

public sealed record SystemPermissionDefinition(string Code, string Resource, string Action, string ScopeType, string NameAr);

public static class SystemPermissionCatalog
{
    public static readonly IReadOnlyList<SystemPermissionDefinition> Definitions =
    [
        new("auth.scope.select", "auth.scope", "select", "PLATFORM", "اختيار نطاق التشغيل"),
        new("sync.operations.execute", "sync.operations", "execute", "BRANCH", "تنفيذ عمليات المزامنة"),
        new("audit.events.read", "audit.events", "read", "BRANCH", "قراءة أحداث التدقيق"),
        new(WaybillPermissionCodes.View, "waybill", "view", "BRANCH", "عرض البوليصة"),
        new(WaybillPermissionCodes.Create, "waybill", "create", "BRANCH", "إنشاء البوليصة"),
        new(WaybillPermissionCodes.Edit, "waybill", "edit", "BRANCH", "تعديل البوليصة"),
        new(WaybillPermissionCodes.Validate, "waybill", "validate", "BRANCH", "فحص البوليصة"),
        new(WaybillPermissionCodes.Submit, "waybill", "submit", "BRANCH", "إرسال البوليصة للاعتماد"),
        new(WaybillPermissionCodes.Approve, "waybill", "approve", "BRANCH", "اعتماد البوليصة"),
        new(WaybillPermissionCodes.Return, "waybill", "return", "BRANCH", "إرجاع البوليصة للتصحيح"),
        new(WaybillPermissionCodes.Cancel, "waybill", "cancel", "BRANCH", "إلغاء البوليصة"),
        new(WaybillPermissionCodes.PartyView, "party", "view", "BRANCH", "عرض الأطراف"),
        new(WaybillPermissionCodes.PartyCreate, "party", "create", "BRANCH", "إنشاء طرف"),
        new(WaybillFinancePermissionCodes.PaymentPlan, "waybill.payment", "plan", "BRANCH", "إدارة خطة الدفع"),
        new(WaybillFinancePermissionCodes.CollectionCreate, "waybill.collection", "create", "BRANCH", "تسجيل التحصيل"),
        new(WaybillFinancePermissionCodes.CollectionReverse, "waybill.collection", "reverse", "BRANCH", "عكس التحصيل"),
        new(ShippingExecutionPermissionCodes.Release, "waybill.release", "execute", "BRANCH", "صرف أصناف البوليصة"),
        new(ShippingExecutionPermissionCodes.TripCreate, "trip", "create", "BRANCH", "إنشاء رحلة"),
        new(ShippingExecutionPermissionCodes.Allocate, "waybill.allocate", "execute", "BRANCH", "تخصيص صنف للرحلة"),
        new(ShippingExecutionPermissionCodes.Unallocate, "waybill.unallocate", "execute", "BRANCH", "إلغاء تخصيص الصنف"),
        new(ShippingExecutionPermissionCodes.ManifestCreate, "manifest", "create", "BRANCH", "إنشاء كشف التحميل"),
        new(ShippingExecutionPermissionCodes.ManifestLoad, "manifest", "load", "BRANCH", "تحميل سطر الكشف"),
        new(ShippingExecutionPermissionCodes.ManifestFinalize, "manifest", "finalize", "BRANCH", "إقفال كشف التحميل"),
        new(ShippingExecutionPermissionCodes.ManifestHandover, "manifest", "handover", "BRANCH", "تسليم كشف التحميل"),
        new(ShippingExecutionPermissionCodes.TripStart, "trip", "start", "BRANCH", "بدء الرحلة")
    ];

    public static async Task EnsureAsync(TransportErpDbContext db, bool allowCreate, CancellationToken ct = default)
    {
        var codes = Definitions.Select(x => x.Code).ToArray();
        var existing = await db.Permissions.IgnoreQueryFilters().Where(x => codes.Contains(x.Code))
            .ToDictionaryAsync(x => x.Code, StringComparer.Ordinal, ct);
        foreach (var definition in Definitions)
        {
            if (existing.TryGetValue(definition.Code, out var permission))
            {
                if (permission.DeletedAt.HasValue || permission.Status != "ACTIVE" || !permission.IsSystem ||
                    permission.ScopeType != definition.ScopeType || permission.Resource != definition.Resource ||
                    permission.Action != definition.Action || permission.NameAr != definition.NameAr)
                    throw new InvalidOperationException($"PERMISSION_CATALOG_DRIFT:{definition.Code}");
                continue;
            }
            if (!allowCreate) throw new InvalidOperationException($"PERMISSION_CATALOG_MISSING:{definition.Code}");
            var now = DateTimeOffset.UtcNow;
            db.Permissions.Add(new Permission
            {
                Id = Guid.NewGuid(), Code = definition.Code, NameAr = definition.NameAr,
                Resource = definition.Resource, Action = definition.Action, ScopeType = definition.ScopeType,
                IsSystem = true, Status = "ACTIVE", CreatedAt = now, UpdatedAt = now,
                RowVersion = Guid.NewGuid().ToByteArray()
            });
        }
        if (allowCreate) await db.SaveChangesAsync(ct);
    }
}

public interface ISystemPermissionCatalogVerifier
{
    Task VerifyAsync(CancellationToken ct = default);
}

public sealed class SystemPermissionCatalogVerifier(TransportErpDbContext db) : ISystemPermissionCatalogVerifier
{
    public Task VerifyAsync(CancellationToken ct = default)
        => SystemPermissionCatalog.EnsureAsync(db, allowCreate: false, ct);
}
