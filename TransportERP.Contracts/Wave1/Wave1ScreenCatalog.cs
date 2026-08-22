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
            "GEN-003", "الدول", "MasterData", "Standard", true,
            new[] { "البيانات الرئيسية", "الاستخدام والربط", "التدقيق" },
            CrudBindings("GEN003", "/api/v1/general/countries")
                .Concat(new[] { new Wave1ActionBinding("Print", "GEN003.Print", "POST", "/api/v1/general/countries/print") })
                .ToArray()),
        Master("GEN-004", "المناطق الإدارية/المحافظات", "GEN004", "/api/v1/general/governorates"),
        Master("GEN-005", "المديريات", "GEN005", "/api/v1/general/directorates"),
        Master("GEN-006", "المدن", "GEN006", "/api/v1/general/cities"),
        Master("GEN-007", "الأحياء/المناطق", "GEN007", "/api/v1/general/areas"),

        new Wave1ScreenDefinition(
            "GEN-013", "الترقيم العام", "Settings", "NumberingControlled", true,
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

        Master("GEN-014", "اللغات", "GEN014", "/api/v1/general/languages"),
        Master("ACC-036", "مجموعات/أنواع الحسابات", "ACC036", "/api/v1/accounting/account-classifications"),

        Report("ACC-074", "أعمار الديون للعملاء", "ACC074", "/api/v1/accounting/reports/customer-aging", "Aging"),
        Report("ACC-075", "أعمار الالتزامات للموردين", "ACC075", "/api/v1/accounting/reports/supplier-aging", "Aging"),
        Report("ACC-049", "الميزانية العمومية", "ACC049", "/api/v1/accounting/reports/balance-sheet", "Report"),
        Report("ACC-050", "التدفقات النقدية", "ACC050", "/api/v1/accounting/reports/cash-flow", "Report"),
        Report("ACC-058", "ميزان المراجعة التفصيلي", "ACC058", "/api/v1/accounting/reports/detailed-trial-balance", "Report")
    };

    private static readonly IReadOnlyDictionary<string, IReadOnlyList<string>> LegacyMappings =
        new Dictionary<string, IReadOnlyList<string>>(StringComparer.OrdinalIgnoreCase)
        {
            ["SET-001"] = new[] { "GEN-003", "GEN-004" },
            ["SET-002"] = new[] { "GEN-005", "GEN-006", "GEN-007" },
            ["SET-011"] = new[] { "GEN-013" },
            ["SET-013"] = new[] { "GEN-014" },
            ["FIN-003"] = new[] { "ACC-036" },
            ["FIN-028"] = new[] { "ACC-074" },
            ["FIN-029"] = new[] { "ACC-075" },
            ["FIN-042"] = new[] { "ACC-049" },
            ["FIN-043"] = new[] { "ACC-050" },
            ["FIN-055"] = new[] { "ACC-058" }
        };

    public static IReadOnlyList<Wave1ScreenDefinition> All => Screens;
    public static IReadOnlyDictionary<string, IReadOnlyList<string>> LegacyCatalogMappings => LegacyMappings;

    public static Wave1ScreenDefinition GetRequired(string screenId)
        => Screens.FirstOrDefault(x => string.Equals(x.ScreenId, screenId, StringComparison.OrdinalIgnoreCase))
           ?? throw new KeyNotFoundException($"Unknown authoritative WAVE-1 screen '{screenId}'.");

    public static IReadOnlyList<string> ResolveLegacyCatalogTarget(string legacyScreenId)
        => LegacyMappings.TryGetValue(legacyScreenId, out var ids)
            ? ids
            : throw new KeyNotFoundException($"Unknown legacy WAVE-1 catalog target '{legacyScreenId}'.");

    private static Wave1ScreenDefinition Master(string screenId, string name, string permissionPrefix, string routeBase)
        => new(
            screenId, name, "MasterData", "Standard", true,
            new[] { "البيانات الرئيسية", "الاستخدام والربط", "التدقيق" },
            CrudBindings(permissionPrefix, routeBase));

    private static Wave1ScreenDefinition Report(
        string screenId,
        string name,
        string permissionPrefix,
        string routeBase,
        string variant)
        => new(
            screenId, name, "ReportInquiry", variant, true,
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
            new Wave1ActionBinding("ViewDetails", $"{permissionPrefix}.View", "GET", $"{routeBase}/{{id}}"),
            new Wave1ActionBinding("New", $"{permissionPrefix}.Create", "POST", routeBase),
            new Wave1ActionBinding("Save", $"{permissionPrefix}.Create", "POST", routeBase),
            new Wave1ActionBinding("Edit", $"{permissionPrefix}.Edit", "PUT", $"{routeBase}/{{id}}"),
            new Wave1ActionBinding("Activate/Disable", $"{permissionPrefix}.Disable", "POST", $"{routeBase}/{{id}}/disable"),
            new Wave1ActionBinding("Search", $"{permissionPrefix}.View", "GET", routeBase),
            new Wave1ActionBinding("Refresh", $"{permissionPrefix}.View", "GET", routeBase)
        };
}
