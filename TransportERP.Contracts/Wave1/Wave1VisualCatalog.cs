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
    private const string Rule = "TransportERP-native W3 RTL baseline derived from Design Constitution 1.13 and exact current W2; no screen-level Close command.";

    private static readonly IReadOnlyDictionary<string, Wave1VisualDefinition> Definitions =
        new Dictionary<string, Wave1VisualDefinition>(StringComparer.OrdinalIgnoreCase)
        {
            ["SET-001"] = new(
                "SET-001", "MasterData",
                new[]
                {
                    F("Level", "المستوى", Wave1VisualFieldKind.Lookup, true, lookup:"Country|Governorate"),
                    F("ParentId", "الدولة الأم", Wave1VisualFieldKind.Lookup, lookup:"Countries"),
                    F("Code", "الرمز", Wave1VisualFieldKind.Text, true),
                    F("ArabicName", "الاسم العربي", Wave1VisualFieldKind.Text, true),
                    F("EnglishName", "الاسم الإنجليزي", Wave1VisualFieldKind.Text),
                    F("NationalityName", "اسم الجنسية", Wave1VisualFieldKind.Text),
                    F("IsActive", "نشط", Wave1VisualFieldKind.Boolean, readOnly:true),
                    F("Version", "الإصدار", Wave1VisualFieldKind.Number, readOnly:true)
                },
                new[]
                {
                    C("Level", "المستوى", 100), C("Code", "الرمز", 90), C("ArabicName", "الاسم العربي", 180),
                    C("EnglishName", "الاسم الإنجليزي", 160), C("ParentName", "يتبع", 160), C("IsActive", "الحالة", 80), C("Version", "الإصدار", 70)
                }, Array.Empty<string>(), Rule),

            ["SET-002"] = new(
                "SET-002", "MasterData",
                new[]
                {
                    F("Level", "المستوى", Wave1VisualFieldKind.Lookup, true, lookup:"Directorate|City|Area"),
                    F("ParentId", "السجل الأب", Wave1VisualFieldKind.Lookup, true, lookup:"Governorates|Directorates|Cities"),
                    F("Code", "الرمز", Wave1VisualFieldKind.Text, true),
                    F("ArabicName", "الاسم العربي", Wave1VisualFieldKind.Text, true),
                    F("EnglishName", "الاسم الإنجليزي", Wave1VisualFieldKind.Text),
                    F("IsActive", "نشط", Wave1VisualFieldKind.Boolean, readOnly:true),
                    F("Version", "الإصدار", Wave1VisualFieldKind.Number, readOnly:true)
                },
                new[]
                {
                    C("Level", "المستوى", 100), C("Code", "الرمز", 90), C("ArabicName", "الاسم العربي", 180),
                    C("EnglishName", "الاسم الإنجليزي", 160), C("ParentName", "يتبع", 180), C("IsActive", "الحالة", 80), C("Version", "الإصدار", 70)
                }, Array.Empty<string>(), Rule),

            ["SET-011"] = new(
                "SET-011", "Settings",
                new[]
                {
                    F("DocumentType", "نوع المستند", Wave1VisualFieldKind.Lookup, true, lookup:"DocumentTypes"),
                    F("Prefix", "البادئة", Wave1VisualFieldKind.Text),
                    F("ResetPolicy", "سياسة إعادة الترقيم", Wave1VisualFieldKind.Lookup, true, lookup:"ResetPolicies"),
                    F("Status", "الحالة", Wave1VisualFieldKind.Lookup, true, lookup:"ACTIVE|INACTIVE"),
                    F("NextValue", "الرقم التالي", Wave1VisualFieldKind.Number, readOnly:true),
                    F("Version", "الإصدار", Wave1VisualFieldKind.Number, readOnly:true),
                    F("IdempotencyKey", "مفتاح عدم التكرار", Wave1VisualFieldKind.Text),
                    F("Reason", "السبب", Wave1VisualFieldKind.Multiline)
                },
                new[]
                {
                    C("DocumentType", "نوع المستند", 150), C("Prefix", "البادئة", 90), C("ResetPolicy", "سياسة الإعادة", 130),
                    C("NextValue", "التالي", 90), C("Status", "الحالة", 90), C("Version", "الإصدار", 70)
                }, new[] { "ActiveSequences", "Reserved", "Committed", "Cancelled" }, Rule),

            ["SET-013"] = new(
                "SET-013", "MasterData",
                new[]
                {
                    F("Code", "رمز اللغة", Wave1VisualFieldKind.Text, true),
                    F("ArabicName", "الاسم العربي", Wave1VisualFieldKind.Text, true),
                    F("EnglishName", "الاسم الإنجليزي", Wave1VisualFieldKind.Text),
                    F("IsRtl", "من اليمين إلى اليسار", Wave1VisualFieldKind.Boolean),
                    F("ResourceKey", "مفتاح الترجمة", Wave1VisualFieldKind.Text),
                    F("TranslationText", "النص المترجم", Wave1VisualFieldKind.Multiline),
                    F("Version", "الإصدار", Wave1VisualFieldKind.Number, readOnly:true)
                },
                new[]
                {
                    C("Code", "الرمز", 80), C("ArabicName", "الاسم العربي", 160), C("EnglishName", "الاسم الإنجليزي", 160),
                    C("IsRtl", "RTL", 60), C("TranslationCount", "عدد الترجمات", 100), C("IsActive", "الحالة", 80), C("Version", "الإصدار", 70)
                }, new[] { "Languages", "Translations" }, Rule),

            ["FIN-003"] = new(
                "FIN-003", "MasterData",
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

            ["FIN-028"] = Aging("FIN-028", "العميل"),
            ["FIN-029"] = Aging("FIN-029", "المورد"),

            ["FIN-042"] = new(
                "FIN-042", "ReportInquiry",
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

            ["FIN-043"] = new(
                "FIN-043", "ReportInquiry",
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

            ["FIN-055"] = new(
                "FIN-055", "ReportInquiry",
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
            : throw new KeyNotFoundException($"No WAVE-1 visual definition for '{screenId}'.");

    private static Wave1VisualDefinition Aging(string screenId, string partyLabel) => new(
        screenId, "ReportInquiry",
        new[]
        {
            F("AsOf", "كما في تاريخ", Wave1VisualFieldKind.Date, true),
            F("BranchId", "الفرع", Wave1VisualFieldKind.Lookup, lookup:"Branches"),
            F("CurrencyId", "العملة", Wave1VisualFieldKind.Lookup, lookup:"Currencies"),
            F("PartyId", partyLabel, Wave1VisualFieldKind.Lookup, lookup:partyLabel == "العميل" ? "Customers" : "Suppliers"),
            F("SearchText", "بحث", Wave1VisualFieldKind.Text)
        },
        new[]
        {
            C("PartyCode", $"رمز {partyLabel}", 100), C("PartyName", partyLabel, 180), C("Current", "غير مستحق", 100, "N2"),
            C("Days1To30", "1-30", 90, "N2"), C("Days31To60", "31-60", 90, "N2"), C("Days61To90", "61-90", 90, "N2"),
            C("Over90", ">90", 90, "N2"), C("TotalOutstanding", "الإجمالي", 110, "N2")
        }, new[] { "GrandTotal" }, Rule);

    private static Wave1VisualField F(string key, string label, Wave1VisualFieldKind kind, bool required = false, bool readOnly = false, string? lookup = null)
        => new(key, label, kind, required, readOnly, lookup);

    private static Wave1VisualColumn C(string key, string header, int width, string? format = null)
        => new(key, header, width, format);
}
