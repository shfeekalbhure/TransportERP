namespace TransportERP.Contracts.Wave1;

public sealed record Wave1ActionBinding(
    string Action,
    string Permission,
    string HttpMethod,
    string Route,
    bool CrossCutting = false);

public sealed record Wave1ScreenDefinition(
    string ScreenId,
    string ArabicName,
    string Profile,
    string Variant,
    bool IsRtl,
    IReadOnlyList<string> Areas,
    IReadOnlyList<Wave1ActionBinding> Actions);

public static class Wave1ScreenCatalog
{
    private static readonly IReadOnlyList<Wave1ScreenDefinition> Screens = new[]
    {
        new Wave1ScreenDefinition(
            "SET-001", "الدول والمناطق الإدارية", "MasterData", "Standard", true,
            new[] { "البيانات الرئيسية", "الاستخدام والربط", "التدقيق" },
            CrudBindings("GEN003", "/api/v1/general/countries")
                .Concat(CrudBindings("GEN004", "/api/v1/general/governorates"))
                .DistinctBy(x => (x.Action, x.Permission, x.Route))
                .ToArray()),

        new Wave1ScreenDefinition(
            "SET-002", "المدن والمديريات/الأحياء", "MasterData", "Standard", true,
            new[] { "البيانات الرئيسية", "الاستخدام والربط", "التدقيق" },
            CrudBindings("GEN005", "/api/v1/general/directorates")
                .Concat(CrudBindings("GEN006", "/api/v1/general/cities"))
                .Concat(CrudBindings("GEN007", "/api/v1/general/areas"))
                .DistinctBy(x => (x.Action, x.Permission, x.Route))
                .ToArray()),

        new Wave1ScreenDefinition(
            "SET-011", "الترقيم والتسلسلات", "Settings", "NumberingControlled", true,
            new[] { "سياسات الترقيم", "نطاقات الترقيم", "الاستثناءات والاعتماد", "سجل التخصيص" },
            new[]
            {
                new Wave1ActionBinding("View", "GEN013.View", "GET", "/api/v1/general/number-sequences"),
                new Wave1ActionBinding("Edit", "GEN013.Edit", "PUT", "/api/v1/general/number-sequences/{id}"),
                new Wave1ActionBinding("Reserve", "GEN013.Reserve", "POST", "/api/v1/general/number-sequences/{id}/reservations"),
                new Wave1ActionBinding("Commit", "GEN013.Commit", "POST", "/api/v1/general/number-reservations/{reservationId}/commit"),
                new Wave1ActionBinding("Cancel", "GEN013.Cancel", "POST", "/api/v1/general/number-reservations/{reservationId}/cancel"),
                new Wave1ActionBinding("Override", "GEN013.Override", "POST", "/api/v1/general/number-sequences/{id}/protected-action")
            }),

        new Wave1ScreenDefinition(
            "SET-013", "اللغات والترجمة", "MasterData", "Standard", true,
            new[] { "البيانات الرئيسية", "الاستخدام والربط", "التدقيق" },
            CrudBindings("GEN014", "/api/v1/general/languages")),

        new Wave1ScreenDefinition(
            "FIN-003", "مجموعات/أنواع الحسابات", "MasterData", "Standard", true,
            new[] { "البيانات الرئيسية", "الاستخدام والربط", "التدقيق" },
            CrudBindings("ACC036", "/api/v1/accounting/account-classifications")),

        Report("FIN-028", "أعمار الذمم المدينة", "ACC058", "/api/v1/accounting/reports/customer-aging"),
        Report("FIN-029", "أعمار الذمم الدائنة", "ACC059", "/api/v1/accounting/reports/supplier-aging"),
        Report("FIN-042", "الميزانية العمومية", "ACC049", "/api/v1/accounting/reports/balance-sheet"),
        Report("FIN-043", "التدفقات النقدية", "ACC050", "/api/v1/accounting/reports/cash-flow"),
        Report("FIN-055", "ميزان المراجعة التفصيلي", "ACC058", "/api/v1/accounting/reports/detailed-trial-balance")
    };

    public static IReadOnlyList<Wave1ScreenDefinition> All => Screens;

    public static Wave1ScreenDefinition GetRequired(string screenId)
        => Screens.FirstOrDefault(x => string.Equals(x.ScreenId, screenId, StringComparison.OrdinalIgnoreCase))
           ?? throw new KeyNotFoundException($"Unknown WAVE-1 screen '{screenId}'.");

    private static Wave1ScreenDefinition Report(string screenId, string name, string permissionPrefix, string routeBase)
        => new(
            screenId, name, "ReportInquiry", screenId is "FIN-028" or "FIN-029" ? "Aging" : "Report", true,
            new[] { "معايير التقرير", "النتائج", "الملخص والتفاصيل" },
            new[]
            {
                new Wave1ActionBinding("ApplyFilters", $"{permissionPrefix}.View", "POST", $"{routeBase}/query"),
                new Wave1ActionBinding("Refresh", $"{permissionPrefix}.View", "POST", $"{routeBase}/query"),
                new Wave1ActionBinding("DrillDown", $"{permissionPrefix}.DrillDown", "POST", $"{routeBase}/drill-down"),
                new Wave1ActionBinding("Export", $"{permissionPrefix}.Export", "POST", $"{routeBase}/export"),
                new Wave1ActionBinding("Print", $"{permissionPrefix}.Print", "POST", $"{routeBase}/print")
            });

    private static Wave1ActionBinding[] CrudBindings(string permissionPrefix, string routeBase)
        => new[]
        {
            new Wave1ActionBinding("View", $"{permissionPrefix}.View", "GET", routeBase),
            new Wave1ActionBinding("New", $"{permissionPrefix}.Create", "POST", routeBase),
            new Wave1ActionBinding("Save", $"{permissionPrefix}.Create", "POST", routeBase),
            new Wave1ActionBinding("Edit", $"{permissionPrefix}.Edit", "PUT", $"{routeBase}/{{id}}"),
            new Wave1ActionBinding("Activate/Disable", $"{permissionPrefix}.Disable", "POST", $"{routeBase}/{{id}}/disable"),
            new Wave1ActionBinding("Search", $"{permissionPrefix}.View", "GET", routeBase),
            new Wave1ActionBinding("Refresh", $"{permissionPrefix}.View", "GET", routeBase)
        };
}
