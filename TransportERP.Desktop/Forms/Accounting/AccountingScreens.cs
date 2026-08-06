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
public sealed class FrmJournalReversal : AccountingScreenForm { public FrmJournalReversal() : base("ACC-016", "القيود العكسية", true) { } }
public sealed class FrmAccountingAdjustments : AccountingScreenForm { public FrmAccountingAdjustments() : base("ACC-017", "التسويات المحاسبية", true) { } }
public sealed class FrmPeriodClosing : AccountingScreenForm { public FrmPeriodClosing() : base("ACC-018", "إقفال الفترات", true) { } }
public sealed class FrmAnnualClosing : AccountingScreenForm { public FrmAnnualClosing() : base("ACC-019", "الإقفال السنوي", true) { } }
public sealed class FrmPeriodReopening : AccountingScreenForm { public FrmPeriodReopening() : base("ACC-020", "فتح وإعادة فتح الفترات", true) { } }
public sealed class FrmJournalReport : AccountingReportScreenForm { public FrmJournalReport() : base("ACC-021", "دفتر اليومية", "من تاريخ", "إلى تاريخ", "الحساب", "الفرع", "مركز التكلفة", "العملة") { } }
public sealed class FrmGeneralLedger : AccountingReportScreenForm { public FrmGeneralLedger() : base("ACC-022", "دفتر الأستاذ العام", "الحساب", "من تاريخ", "إلى تاريخ", "الفرع", "مركز التكلفة") { } }
public sealed class FrmAccountStatement : AccountingReportScreenForm { public FrmAccountStatement() : base("ACC-023", "كشف الحساب", "نوع الطرف", "الطرف أو الحساب", "من تاريخ", "إلى تاريخ", "العملة") { } }
public sealed class FrmTrialBalance : AccountingReportScreenForm { public FrmTrialBalance() : base("ACC-024", "ميزان المراجعة", "من تاريخ", "إلى تاريخ", "مستوى الحساب", "الفرع", "العملة") { } }
public sealed class FrmIncomeStatement : AccountingReportScreenForm { public FrmIncomeStatement() : base("ACC-025", "قائمة الدخل", "من تاريخ", "إلى تاريخ", "الفرع", "مركز التكلفة", "العملة") { } }
public sealed class FrmBalanceSheet : AccountingReportScreenForm { public FrmBalanceSheet() : base("ACC-026", "الميزانية العمومية", "حتى تاريخ", "الفرع", "مركز التكلفة", "مستوى العرض", "العملة") { } }
public sealed class FrmCashFlowStatement : AccountingReportScreenForm { public FrmCashFlowStatement() : base("ACC-027", "قائمة التدفقات النقدية", "من تاريخ", "إلى تاريخ", "الفرع", "العملة", "طريقة العرض") { } }
public sealed class FrmCurrencyTrialBalance : AccountingReportScreenForm { public FrmCurrencyTrialBalance() : base("ACC-028", "ميزان المراجعة حسب العملة", "من تاريخ", "إلى تاريخ", "العملة", "الفرع", "مستوى الحساب") { } }
public sealed class FrmCostCenterActivity : AccountingReportScreenForm { public FrmCostCenterActivity() : base("ACC-029", "تقرير حركة مركز التكلفة", "مركز التكلفة", "من تاريخ", "إلى تاريخ", "الحساب", "الفرع") { } }
public sealed class FrmCashStatement : AccountingReportScreenForm { public FrmCashStatement() : base("ACC-030", "تقرير الصناديق", "الصندوق", "من تاريخ", "إلى تاريخ", "العملة", "الفرع") { } }
public sealed class FrmBankStatement : AccountingReportScreenForm { public FrmBankStatement() : base("ACC-031", "تقرير الحسابات البنكية", "البنك أو الحساب", "من تاريخ", "إلى تاريخ", "العملة", "الفرع") { } }
public sealed class FrmUnpostedEntriesReport : AccountingReportScreenForm { public FrmUnpostedEntriesReport() : base("ACC-032", "تقرير القيود غير المرحلة", "من تاريخ", "إلى تاريخ", "نوع القيد", "الفرع", "الحالة") { } }
public sealed class FrmBankReconciliation : AccountingScreenForm { public FrmBankReconciliation() : base("ACC-033", "التسويات البنكية", true) { } }
public sealed class FrmBankReconciliationItems : AccountingScreenForm { public FrmBankReconciliationItems() : base("ACC-034", "عناصر المطابقة البنكية", true) { } }
public sealed class FrmBankReconciliationApprovals : AccountingScreenForm { public FrmBankReconciliationApprovals() : base("ACC-035", "اعتماد التسويات البنكية", true) { } }
public sealed class FrmOpeningBalances : AccountingScreenForm { public FrmOpeningBalances() : base("ACC-036", "الأرصدة الافتتاحية", true) { } }
public sealed class FrmOpeningBalanceImport : AccountingScreenForm { public FrmOpeningBalanceImport() : base("ACC-037", "استيراد الأرصدة الافتتاحية", true) { } }
public sealed class FrmOpeningBalanceApproval : AccountingScreenForm { public FrmOpeningBalanceApproval() : base("ACC-038", "اعتماد الأرصدة الافتتاحية", true) { } }
public sealed class FrmAccountingApprovals : AccountingScreenForm { public FrmAccountingApprovals() : base("ACC-039", "طلبات الاعتماد المحاسبية", true) { } }
public sealed class FrmAccountingApprovalInbox : AccountingScreenForm { public FrmAccountingApprovalInbox() : base("ACC-040", "صندوق وارد الاعتماد المحاسبي", true) { } }
public sealed class FrmAccountingApprovalPolicies : AccountingScreenForm { public FrmAccountingApprovalPolicies() : base("ACC-041", "سياسات الاعتماد المحاسبي") { } }
public sealed class FrmPostingControls : AccountingScreenForm { public FrmPostingControls() : base("ACC-042", "ضوابط الترحيل المحاسبي") { } }
public sealed class FrmAccountingAuditLog : AccountingReportScreenForm { public FrmAccountingAuditLog() : base("ACC-043", "سجل التدقيق المحاسبي", "من تاريخ", "إلى تاريخ", "المستخدم", "العملية", "المرجع") { } }
public sealed class FrmJournalIntegrityMonitor : AccountingReportScreenForm { public FrmJournalIntegrityMonitor() : base("ACC-044", "مراقبة سلامة القيود", "من تاريخ", "إلى تاريخ", "الحالة", "الفرع", "نوع الفحص") { } }
