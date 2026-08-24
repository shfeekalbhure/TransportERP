namespace TransportERP.Contracts.Wave1;

public enum Wave1VisualFieldKind
{
    Text,
    Date,
    Boolean,
    Lookup,
    Number,
    Multiline
}

public sealed record Wave1VisualField(
    string Key,
    string ArabicLabel,
    Wave1VisualFieldKind Kind,
    bool Required = false,
    bool ReadOnly = false,
    string? LookupSource = null);

public sealed record Wave1VisualColumn(
    string Key,
    string ArabicHeader,
    int Width = 120,
    string? Format = null);

public sealed record Wave1VisualDefinition(
    string ScreenId,
    string Mode,
    IReadOnlyList<Wave1VisualField> Fields,
    IReadOnlyList<Wave1VisualColumn> Columns,
    IReadOnlyList<string> SummaryKeys,
    string NativeDesignRule);

public static class Wave1VisualCatalog
{
    private const string Rule = "TransportERP-native Arabic RTL W3 definition bound only to Current Approved V1.25/V1.3 and scoped owner decisions SRC-055/SRC-056/SRC-057; no unissued field is inferred.";

    private static readonly IReadOnlyDictionary<string, Wave1VisualDefinition> Definitions =
        new Dictionary<string, Wave1VisualDefinition>(StringComparer.OrdinalIgnoreCase)
        {
            ["GEN-003"] = new(
                "GEN-003", "MasterData",
                new[]
                {
                    F("Code", "الرمز", Wave1VisualFieldKind.Text, true),
                    F("ArabicName", "الاسم العربي", Wave1VisualFieldKind.Text, true),
                    F("EnglishName", "الاسم الإنجليزي", Wave1VisualFieldKind.Text),
                    F("NationalityName", "اسم الجنسية", Wave1VisualFieldKind.Text),
                    F("Notes", "ملاحظات", Wave1VisualFieldKind.Multiline),
                    F("ISO2", "ISO2", Wave1VisualFieldKind.Text, true),
                    F("ISO3", "ISO3", Wave1VisualFieldKind.Text),
                    F("DialingCode", "مفتاح الاتصال", Wave1VisualFieldKind.Text),
                    F("Status", "الحالة", Wave1VisualFieldKind.Lookup, readOnly:true, lookup:"Active|Stopped"),
                    F("Version", "الإصدار", Wave1VisualFieldKind.Number, readOnly:true)
                },
                new[]
                {
                    C("Code", "الرمز", 90), C("ArabicName", "الاسم العربي", 170), C("EnglishName", "الاسم الإنجليزي", 150),
                    C("ISO2", "ISO2", 65), C("ISO3", "ISO3", 65), C("DialingCode", "مفتاح الاتصال", 95),
                    C("Status", "الحالة", 85)
                }, Array.Empty<string>(), Rule),

            ["GEN-004"] = GeoMaster("GEN-004", "الدولة", "Countries"),
            ["GEN-005"] = GeoMaster("GEN-005", "المحافظة", "Governorates"),
            ["GEN-006"] = GeoMaster("GEN-006", "المديرية", "Directorates"),
            ["GEN-007"] = GeoMaster("GEN-007", "المدينة", "Cities"),

            ["GEN-013"] = new(
                "GEN-013", "Settings",
                new[]
                {
                    F("Code", "الرمز", Wave1VisualFieldKind.Text, true),
                    F("ArabicName", "الاسم العربي", Wave1VisualFieldKind.Text, true),
                    F("EnglishName", "الاسم الإنجليزي", Wave1VisualFieldKind.Text),
                    F("Status", "الحالة", Wave1VisualFieldKind.Lookup, readOnly:true, lookup:"Active|Stopped"),
                    F("Notes", "ملاحظات", Wave1VisualFieldKind.Multiline),
                    F("Scope", "النطاق", Wave1VisualFieldKind.Lookup, true, lookup:"NumberingScopes"),
                    F("DocumentType", "نوع المستند", Wave1VisualFieldKind.Lookup, true, lookup:"DocumentTypes"),
                    F("Prefix", "البادئة", Wave1VisualFieldKind.Text),
                    F("LastNumber", "آخر رقم", Wave1VisualFieldKind.Number, readOnly:true),
                    F("ResetPolicy", "سياسة إعادة الترقيم", Wave1VisualFieldKind.Lookup, true, lookup:"ResetPolicies")
                },
                new[]
                {
                    C("Code", "الرمز", 90), C("DocumentType", "نوع المستند", 150), C("Scope", "النطاق", 110),
                    C("Prefix", "البادئة", 90), C("LastNumber", "آخر رقم", 95), C("ResetPolicy", "سياسة الإعادة", 130), C("Status", "الحالة", 85)
                }, new[] { "ActiveSequences", "Reserved", "Committed", "Cancelled" }, Rule),

            ["GEN-014"] = new(
                "GEN-014", "MasterData",
                new[]
                {
                    F("Code", "رمز اللغة", Wave1VisualFieldKind.Text, true),
                    F("CultureCode", "رمز الثقافة", Wave1VisualFieldKind.Text, true),
                    F("Direction", "اتجاه الكتابة", Wave1VisualFieldKind.Lookup, true, lookup:"RTL|LTR"),
                    F("Status", "الحالة", Wave1VisualFieldKind.Lookup, readOnly:true, lookup:"Active|Stopped"),
                    F("Version", "الإصدار", Wave1VisualFieldKind.Number, readOnly:true)
                },
                new[]
                {
                    C("Code", "الرمز", 90), C("CultureCode", "رمز الثقافة", 120), C("Direction", "الاتجاه", 90),
                    C("Status", "الحالة", 85), C("Version", "الإصدار", 70)
                }, Array.Empty<string>(), Rule),

            ["ACC-036"] = new(
                "ACC-036", "MasterData",
                new[]
                {
                    F("Code", "رمز المجموعة", Wave1VisualFieldKind.Text, true),
                    F("ArabicName", "الاسم العربي", Wave1VisualFieldKind.Text, true),
                    F("EnglishName", "الاسم الإنجليزي", Wave1VisualFieldKind.Text),
                    F("AccountType", "نوع الحساب", Wave1VisualFieldKind.Lookup, true, lookup:"ASSET|LIABILITY|EQUITY|REVENUE|EXPENSE"),
                    F("IsActive", "نشط", Wave1VisualFieldKind.Boolean, readOnly:true),
                    F("Version", "الإصدار", Wave1VisualFieldKind.Number, readOnly:true)
                },
                new[]
                {
                    C("Code", "الرمز", 90), C("ArabicName", "الاسم العربي", 180), C("EnglishName", "الاسم الإنجليزي", 160),
                    C("AccountType", "نوع الحساب", 120), C("IsActive", "الحالة", 80), C("Version", "الإصدار", 70)
                }, Array.Empty<string>(), Rule),

            ["ACC-074"] = Aging("ACC-074", "العميل", "Customers"),
            ["ACC-075"] = Aging("ACC-075", "المورد", "Suppliers"),

            ["ACC-049"] = new(
                "ACC-049", "ReportInquiry",
                new[]
                {
                    F("AsOf", "كما في تاريخ", Wave1VisualFieldKind.Date, true),
                    F("BranchId", "الفرع", Wave1VisualFieldKind.Lookup, lookup:"Branches"),
                    F("CurrencyId", "العملة", Wave1VisualFieldKind.Lookup, lookup:"Currencies"),
                    F("SearchText", "بحث", Wave1VisualFieldKind.Text)
                },
                new[]
                {
                    C("AccountCode", "رمز الحساب", 110), C("AccountNameAr", "اسم الحساب", 220), C("AccountType", "النوع", 100), C("Balance", "الرصيد", 120, "N2")
                }, new[] { "AssetsTotal", "LiabilitiesTotal", "EquityTotal", "CurrentEarnings", "EquationDifference" }, Rule),

            ["ACC-050"] = new(
                "ACC-050", "ReportInquiry",
                new[]
                {
                    F("From", "من تاريخ", Wave1VisualFieldKind.Date, true),
                    F("To", "إلى تاريخ", Wave1VisualFieldKind.Date, true),
                    F("BranchId", "الفرع", Wave1VisualFieldKind.Lookup, lookup:"Branches"),
                    F("CurrencyId", "العملة", Wave1VisualFieldKind.Lookup, lookup:"Currencies"),
                    F("SearchText", "بحث", Wave1VisualFieldKind.Text)
                },
                new[]
                {
                    C("Activity", "النشاط", 100), C("SourceType", "المصدر", 120), C("DocumentNo", "المستند", 120), C("Date", "التاريخ", 100, "yyyy-MM-dd"),
                    C("Inflow", "تدفق داخل", 110, "N2"), C("Outflow", "تدفق خارج", 110, "N2"), C("Net", "الصافي", 110, "N2")
                }, new[] { "OperatingNet", "InvestingNet", "FinancingNet", "UnclassifiedNet", "NetCashMovement" }, Rule),

            ["ACC-058"] = new(
                "ACC-058", "ReportInquiry",
                new[]
                {
                    F("From", "من تاريخ", Wave1VisualFieldKind.Date, true),
                    F("To", "إلى تاريخ", Wave1VisualFieldKind.Date, true),
                    F("BranchId", "الفرع", Wave1VisualFieldKind.Lookup, lookup:"Branches"),
                    F("CurrencyId", "العملة", Wave1VisualFieldKind.Lookup, lookup:"Currencies"),
                    F("AccountId", "الحساب", Wave1VisualFieldKind.Lookup, lookup:"Accounts"),
                    F("FinancialDimensionId", "البعد المالي", Wave1VisualFieldKind.Lookup, lookup:"FinancialDimensions"),
                    F("SearchText", "بحث", Wave1VisualFieldKind.Text)
                },
                new[]
                {
                    C("AccountCode", "رمز الحساب", 100), C("AccountNameAr", "اسم الحساب", 190), C("OpeningDebit", "افتتاحي مدين", 110, "N2"),
                    C("OpeningCredit", "افتتاحي دائن", 110, "N2"), C("PeriodDebit", "حركة مدين", 110, "N2"), C("PeriodCredit", "حركة دائن", 110, "N2"),
                    C("ClosingDebit", "ختامي مدين", 110, "N2"), C("ClosingCredit", "ختامي دائن", 110, "N2")
                }, new[] { "TotalOpeningDebit", "TotalOpeningCredit", "TotalPeriodDebit", "TotalPeriodCredit", "TotalClosingDebit", "TotalClosingCredit" }, Rule)
        };

    public static IReadOnlyCollection<Wave1VisualDefinition> All => Definitions.Values.ToArray();

    public static Wave1VisualDefinition GetRequired(string screenId)
        => Definitions.TryGetValue(screenId, out var value)
            ? value
            : throw new KeyNotFoundException($"No authoritative WAVE-1 visual definition for '{screenId}'.");

    private static Wave1VisualDefinition GeoMaster(string screenId, string parentLabel, string lookup)
    {
        var fields = new[]
        {
            F("ParentId", parentLabel, Wave1VisualFieldKind.Lookup, true, lookup:lookup),
            F("Code", "الرمز", Wave1VisualFieldKind.Text, true),
            F("ArabicName", "الاسم العربي", Wave1VisualFieldKind.Text, true),
            F("EnglishName", "الاسم الإنجليزي", Wave1VisualFieldKind.Text),
            F("Status", "الحالة", Wave1VisualFieldKind.Lookup, readOnly:true, lookup:"Active|Stopped"),
            F("Version", "الإصدار", Wave1VisualFieldKind.Number, readOnly:true)
        };

        var columns = new[]
        {
            C("Code", "الرمز", 90), C("ArabicName", "الاسم العربي", 180), C("EnglishName", "الاسم الإنجليزي", 160),
            C("ParentName", parentLabel, 160), C("Status", "الحالة", 85), C("Version", "الإصدار", 70)
        };
        return new(screenId, "MasterData", fields, columns, Array.Empty<string>(), Rule);
    }

    private static Wave1VisualDefinition Aging(string screenId, string partyLabel, string lookup) => new(
        screenId, "ReportInquiry",
        new[]
        {
            F("AsOf", "كما في تاريخ", Wave1VisualFieldKind.Date, true),
            F("BranchId", "الفرع", Wave1VisualFieldKind.Lookup, lookup:"Branches"),
            F("CurrencyId", "العملة", Wave1VisualFieldKind.Lookup, lookup:"Currencies"),
            F("PartyId", partyLabel, Wave1VisualFieldKind.Lookup, lookup:lookup),
            F("SearchText", "بحث", Wave1VisualFieldKind.Text)
        },
        new[]
        {
            C("PartyCode", $"رمز {partyLabel}", 100), C("PartyName", partyLabel, 180), C("Current", "غير مستحق", 100, "N2"),
            C("Days1To30", "1-30", 90, "N2"), C("Days31To60", "31-60", 90, "N2"), C("Days61To90", "61-90", 90, "N2"),
            C("Over90", ">90", 90, "N2"), C("TotalOutstanding", "الإجمالي", 110, "N2")
        }, new[] { "GrandTotal" }, Rule);

    private static Wave1VisualField F(
        string key,
        string label,
        Wave1VisualFieldKind kind,
        bool required = false,
        bool readOnly = false,
        string? lookup = null)
        => new(key, label, kind, required, readOnly, lookup);

    private static Wave1VisualColumn C(string key, string header, int width, string? format = null)
        => new(key, header, width, format);
}
