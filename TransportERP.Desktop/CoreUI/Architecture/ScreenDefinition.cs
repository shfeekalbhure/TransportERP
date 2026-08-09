namespace TransportERP.Desktop.CoreUI.Architecture;

public sealed record ScreenDefinition(
    string Code,
    string Title,
    TransportScreenProfile Profile,
    string Variant,
    IReadOnlyList<string> Fields,
    IReadOnlyList<string> GridColumns,
    IReadOnlyList<string> Capabilities,
    bool IsReadOnly = false);

public static class ScreenDefinitions
{
    public static readonly ScreenDefinition Gen003 = new("GEN-003", "الدول", TransportScreenProfile.MasterData, "Standard", ["Code", "ArabicName", "EnglishName", "Status", "Notes", "ISO2", "ISO3", "DialingCode"], ["Code", "ArabicName", "EnglishName", "ISO2", "ISO3", "DialingCode", "Status"], ["New", "Save", "Edit", "Disable", "Delete", "Print"]);
    public static readonly ScreenDefinition Acc035 = new("ACC-035", "دليل الحسابات", TransportScreenProfile.TreeMaster, "Hierarchy", ["رمز الحساب", "اسم الحساب", "نوع الحساب", "الحساب الأب"], [], ["New", "Save", "Move", "Disable"]);
    public static readonly ScreenDefinition Acc041 = new("ACC-041", "الفترات المحاسبية", TransportScreenProfile.ControlApproval, "PeriodLifecycle", ["السنة المالية", "اسم الفترة", "من تاريخ", "إلى تاريخ", "حالة الفترة"], ["التاريخ", "الإجراء", "الحالة السابقة", "الحالة الجديدة", "المنفذ", "السبب"], ["Open", "Close", "Reopen", "ViewHistory"]);
    public static readonly ScreenDefinition Acc042 = new("ACC-042", "القيد اليومي", TransportScreenProfile.Transaction, "HeaderLines", ["رقم القيد", "التاريخ", "العملة", "الوصف", "الحالة"], ["الحساب", "البيان", "مركز التكلفة", "مدين", "دائن"], ["SaveDraft", "Post", "Reverse"], true);
    public static readonly ScreenDefinition Acc046 = new("ACC-046", "ميزان المراجعة", TransportScreenProfile.ReportInquiry, "Report", ["من تاريخ", "إلى تاريخ", "الفرع", "مستوى الحساب"], ["الحساب", "مدين", "دائن", "الرصيد"], ["Print", "Export", "DrillDown"], true);
    public static readonly ScreenDefinition Gen015 = new("GEN-015", "إعدادات التشغيل العامة", TransportScreenProfile.Settings, "ScopedSettings", ["Scope", "Key", "Value", "NearestOverride"], ["المفتاح", "القيمة الفعالة", "المصدر", "الأولوية"], ["SelectScope", "Override", "ResetOverride"]);
    public static IReadOnlyList<ScreenDefinition> All { get; } = [Gen003, Acc035, Acc041, Acc042, Acc046, Gen015];
}
