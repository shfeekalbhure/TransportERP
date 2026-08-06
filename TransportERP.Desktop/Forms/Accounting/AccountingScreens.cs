namespace TransportERP.Desktop.Forms.Accounting;

// 44 نماذج محاسبية فعلية؛ كل نموذج يرث القالب الموحد ويحتفظ ببياناته المحلية المنظمة.
public sealed class FrmChartOfAccounts : AccountingScreenForm { public FrmChartOfAccounts() : base("ACC-001", "دليل الحسابات") { } }
public sealed class FrmAccountTypes : AccountingScreenForm { public FrmAccountTypes() : base("ACC-002", "أنواع الحسابات") { } }
public sealed class FrmAccountGroups : AccountingScreenForm { public FrmAccountGroups() : base("ACC-003", "مجموعات الحسابات") { } }
public sealed class FrmCostCenters : AccountingScreenForm { public FrmCostCenters() : base("ACC-004", "مراكز التكلفة") { } }
public sealed class FrmCashBoxes : AccountingScreenForm { public FrmCashBoxes() : base("ACC-005", "الصناديق") { } }
public sealed class FrmBankAccounts : AccountingScreenForm { public FrmBankAccounts() : base("ACC-006", "الحسابات البنكية") { } }
public sealed class FrmPaymentMethods : AccountingScreenForm { public FrmPaymentMethods() : base("ACC-007", "طرق الدفع") { } }
public sealed class FrmFiscalPeriods : AccountingScreenForm { public FrmFiscalPeriods() : base("ACC-008", "السنوات والفترات المالية") { } }
public sealed class FrmJournalEntries : AccountingScreenForm { public FrmJournalEntries() : base("ACC-009", "القيود اليومية", true) { } }
public sealed class FrmReceiptVouchers : AccountingScreenForm { public FrmReceiptVouchers() : base("ACC-010", "سندات القبض", true) { } }
public sealed class FrmPaymentVouchers : AccountingScreenForm { public FrmPaymentVouchers() : base("ACC-011", "سندات الصرف", true) { } }
public sealed class FrmFinancialTransfers : AccountingScreenForm { public FrmFinancialTransfers() : base("ACC-012", "التحويلات المالية", true) { } }
public sealed class FrmJournalAdjustments : AccountingScreenForm { public FrmJournalAdjustments() : base("ACC-013", "تسويات القيود", true) { } }
public sealed class FrmPeriodClosing : AccountingScreenForm { public FrmPeriodClosing() : base("ACC-014", "إقفال الفترات", true) { } }
public sealed class FrmJournalReversal : AccountingScreenForm { public FrmJournalReversal() : base("ACC-015", "عكس القيود", true) { } }
public sealed class FrmJournalReport : AccountingScreenForm { public FrmJournalReport() : base("ACC-016", "دفتر اليومية") { } }
public sealed class FrmGeneralLedger : AccountingScreenForm { public FrmGeneralLedger() : base("ACC-017", "الأستاذ العام") { } }
public sealed class FrmAccountStatement : AccountingScreenForm { public FrmAccountStatement() : base("ACC-018", "كشف الحساب") { } }
public sealed class FrmTrialBalance : AccountingScreenForm { public FrmTrialBalance() : base("ACC-019", "ميزان المراجعة") { } }
public sealed class FrmIncomeStatement : AccountingScreenForm { public FrmIncomeStatement() : base("ACC-020", "قائمة الدخل") { } }
public sealed class FrmBalanceSheet : AccountingScreenForm { public FrmBalanceSheet() : base("ACC-021", "الميزانية العمومية") { } }
public sealed class FrmCashFlowStatement : AccountingScreenForm { public FrmCashFlowStatement() : base("ACC-022", "التدفقات النقدية") { } }
public sealed class FrmCurrencyTrialBalance : AccountingScreenForm { public FrmCurrencyTrialBalance() : base("ACC-023", "ميزان حسب العملة") { } }
public sealed class FrmBankReconciliation : AccountingScreenForm { public FrmBankReconciliation() : base("ACC-024", "التسويات البنكية", true) { } }
public sealed class FrmOpeningBalances : AccountingScreenForm { public FrmOpeningBalances() : base("ACC-025", "الأرصدة الافتتاحية", true) { } }
public sealed class FrmAccountingApprovals : AccountingScreenForm { public FrmAccountingApprovals() : base("ACC-026", "طلبات الاعتماد المحاسبية", true) { } }
public sealed class FrmAdjustmentMemos : AccountingScreenForm { public FrmAdjustmentMemos() : base("ACC-027", "مذكرات التسوية", true) { } }
public sealed class FrmSubLedger : AccountingScreenForm { public FrmSubLedger() : base("ACC-028", "دفتر الأستاذ المساعد") { } }
public sealed class FrmCostCenterActivity : AccountingScreenForm { public FrmCostCenterActivity() : base("ACC-029", "حركة مركز التكلفة") { } }
public sealed class FrmExpenseAnalysis : AccountingScreenForm { public FrmExpenseAnalysis() : base("ACC-030", "تحليل المصروفات") { } }
public sealed class FrmRevenueAnalysis : AccountingScreenForm { public FrmRevenueAnalysis() : base("ACC-031", "تحليل الإيرادات") { } }
public sealed class FrmCashStatement : AccountingScreenForm { public FrmCashStatement() : base("ACC-032", "كشف الصندوق") { } }
public sealed class FrmBankStatement : AccountingScreenForm { public FrmBankStatement() : base("ACC-033", "كشف البنك") { } }
public sealed class FrmCustomerBalanceReconciliation : AccountingScreenForm { public FrmCustomerBalanceReconciliation() : base("ACC-034", "مطابقة أرصدة العملاء") { } }
public sealed class FrmSupplierBalanceReconciliation : AccountingScreenForm { public FrmSupplierBalanceReconciliation() : base("ACC-035", "مطابقة أرصدة الموردين") { } }
public sealed class FrmAccrualEntries : AccountingScreenForm { public FrmAccrualEntries() : base("ACC-036", "قيود الاستحقاق", true) { } }
public sealed class FrmDepreciationEntries : AccountingScreenForm { public FrmDepreciationEntries() : base("ACC-037", "قيود الإهلاك", true) { } }
public sealed class FrmClosedPeriods : AccountingScreenForm { public FrmClosedPeriods() : base("ACC-038", "الفترات المقفلة") { } }
public sealed class FrmAccountingApprovalPermissions : AccountingScreenForm { public FrmAccountingApprovalPermissions() : base("ACC-039", "صلاحيات الاعتماد المحاسبي") { } }
public sealed class FrmApprovalLevels : AccountingScreenForm { public FrmApprovalLevels() : base("ACC-040", "مستويات الاعتماد") { } }
public sealed class FrmPostingLog : AccountingScreenForm { public FrmPostingLog() : base("ACC-041", "سجل الترحيل", true) { } }
public sealed class FrmCancellationAndReversalLog : AccountingScreenForm { public FrmCancellationAndReversalLog() : base("ACC-042", "سجل الإلغاء والعكس", true) { } }
public sealed class FrmPendingEntries : AccountingScreenForm { public FrmPendingEntries() : base("ACC-043", "القيود المعلقة", true) { } }
public sealed class FrmBalanceMonitor : AccountingScreenForm { public FrmBalanceMonitor() : base("ACC-044", "مراقبة الميزان", true) { } }
