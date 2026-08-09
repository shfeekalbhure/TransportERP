namespace TransportERP.Desktop.CoreUI.Architecture;

public sealed class Gen003CountriesReferenceScreen : CoreUiReferenceScreen
{
    public Gen003CountriesReferenceScreen() : base(new("GEN-003", "الدول", TransportScreenProfile.MasterData,
        ["كود الدولة", "الاسم العربي", "الاسم الإنجليزي", "رمز ISO", "الحالة"], ["الكود", "الاسم", "ISO", "الحالة"])) { }
}

public sealed class Acc035ChartOfAccountsReferenceScreen : CoreUiReferenceScreen
{
    public Acc035ChartOfAccountsReferenceScreen() : base(new("ACC-035", "دليل الحسابات", TransportScreenProfile.TreeMaster,
        ["رمز الحساب", "اسم الحساب", "نوع الحساب", "الحساب الأب"], ["الرمز", "الحساب", "النوع", "الرصيد"], UsesTree: true)) { }
}

public sealed class Acc041AccountingPeriodsReferenceScreen : CoreUiReferenceScreen
{
    public Acc041AccountingPeriodsReferenceScreen() : base(new("ACC-041", "الفترات المحاسبية", TransportScreenProfile.ControlApproval,
        ["السنة المالية", "اسم الفترة", "من تاريخ", "إلى تاريخ", "حالة الفترة"], ["الفترة", "من", "إلى", "الحالة"])) { }
}

public sealed class Acc042JournalEntryReferenceScreen : CoreUiReferenceScreen
{
    public Acc042JournalEntryReferenceScreen() : base(new("ACC-042", "القيد اليومي", TransportScreenProfile.Transaction,
        ["رقم القيد", "التاريخ", "العملة", "الوصف", "حالة القيد"], ["الحساب", "مدين", "دائن", "البيان"])) { }
}

public sealed class Acc046TrialBalanceReferenceScreen : CoreUiReferenceScreen
{
    public Acc046TrialBalanceReferenceScreen() : base(new("ACC-046", "ميزان المراجعة", TransportScreenProfile.ReportInquiry,
        ["من تاريخ", "إلى تاريخ", "الفرع", "مستوى الحساب"], ["الحساب", "مدين", "دائن", "الرصيد"], IsReadOnly: true)) { }
}

public sealed class Gen015OperationalSettingsReferenceScreen : CoreUiReferenceScreen
{
    public Gen015OperationalSettingsReferenceScreen() : base(new("GEN-015", "إعدادات التشغيل العامة", TransportScreenProfile.Settings,
        ["النطاق", "القيمة", "الأولوية", "الحالة"], ["النطاق", "المفتاح", "القيمة", "المصدر"], IsReadOnly: false)) { }
}
