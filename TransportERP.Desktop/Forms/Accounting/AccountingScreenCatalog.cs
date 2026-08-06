namespace TransportERP.Desktop.Forms.Accounting;

/// <summary>
/// بوابة موحدة لربط قائمة الشاشة الرئيسية بنماذج المحاسبة الفعلية.
/// </summary>
public static class AccountingScreenCatalog
{
    public static bool TryCreate(string screenCode, out Form? form)
    {
        form = screenCode switch
        {
            "ACC-001" => new FrmChartOfAccounts(),
            "ACC-002" => new FrmAccountTypes(),
            "ACC-003" => new FrmAccountGroups(),
            "ACC-004" => new FrmCostCenters(),
            "ACC-005" => new FrmCashBoxes(),
            "ACC-006" => new FrmBankAccounts(),
            "ACC-007" => new FrmPaymentMethods(),
            "ACC-008" => new FrmFiscalPeriods(),
            "ACC-009" => new FrmJournalEntries(),
            "ACC-010" => new FrmReceiptVouchers(),
            "ACC-011" => new FrmPaymentVouchers(),
            "ACC-012" => new FrmFinancialTransfers(),
            "ACC-013" => new FrmJournalAdjustments(),
            "ACC-014" => new FrmPeriodClosing(),
            "ACC-015" => new FrmJournalReversal(),
            "ACC-016" => new FrmJournalReport(),
            "ACC-017" => new FrmGeneralLedger(),
            "ACC-018" => new FrmAccountStatement(),
            "ACC-019" => new FrmTrialBalance(),
            "ACC-020" => new FrmIncomeStatement(),
            "ACC-021" => new FrmBalanceSheet(),
            "ACC-022" => new FrmCashFlowStatement(),
            "ACC-023" => new FrmCurrencyTrialBalance(),
            "ACC-024" => new FrmBankReconciliation(),
            "ACC-025" => new FrmOpeningBalances(),
            "ACC-026" => new FrmAccountingApprovals(),
            "ACC-027" => new FrmAdjustmentMemos(),
            "ACC-028" => new FrmSubLedger(),
            "ACC-029" => new FrmCostCenterActivity(),
            "ACC-030" => new FrmExpenseAnalysis(),
            "ACC-031" => new FrmRevenueAnalysis(),
            "ACC-032" => new FrmCashStatement(),
            "ACC-033" => new FrmBankStatement(),
            "ACC-034" => new FrmCustomerBalanceReconciliation(),
            "ACC-035" => new FrmSupplierBalanceReconciliation(),
            "ACC-036" => new FrmAccrualEntries(),
            "ACC-037" => new FrmDepreciationEntries(),
            "ACC-038" => new FrmClosedPeriods(),
            "ACC-039" => new FrmAccountingApprovalPermissions(),
            "ACC-040" => new FrmApprovalLevels(),
            "ACC-041" => new FrmPostingLog(),
            "ACC-042" => new FrmCancellationAndReversalLog(),
            "ACC-043" => new FrmPendingEntries(),
            "ACC-044" => new FrmBalanceMonitor(),
            _ => null
        };
        return form is not null;
    }
}
