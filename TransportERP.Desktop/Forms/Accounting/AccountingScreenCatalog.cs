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
            "ACC-016" => new FrmJournalReversal(),
            "ACC-017" => new FrmAccountingAdjustments(),
            "ACC-018" => new FrmPeriodClosing(),
            "ACC-019" => new FrmAnnualClosing(),
            "ACC-020" => new FrmPeriodReopening(),
            "ACC-021" => new FrmJournalReport(),
            "ACC-022" => new FrmGeneralLedger(),
            "ACC-023" => new FrmAccountStatement(),
            "ACC-024" => new FrmTrialBalance(),
            "ACC-025" => new FrmIncomeStatement(),
            "ACC-026" => new FrmBalanceSheet(),
            "ACC-027" => new FrmCashFlowStatement(),
            "ACC-028" => new FrmCurrencyTrialBalance(),
            "ACC-029" => new FrmCostCenterActivity(),
            "ACC-030" => new FrmCashStatement(),
            "ACC-031" => new FrmBankStatement(),
            "ACC-032" => new FrmUnpostedEntriesReport(),
            "ACC-033" => new FrmBankReconciliation(),
            "ACC-034" => new FrmBankReconciliationItems(),
            "ACC-035" => new FrmBankReconciliationApprovals(),
            "ACC-036" => new FrmOpeningBalances(),
            "ACC-037" => new FrmOpeningBalanceImport(),
            "ACC-038" => new FrmOpeningBalanceApproval(),
            "ACC-039" => new FrmAccountingApprovals(),
            "ACC-040" => new FrmAccountingApprovalInbox(),
            "ACC-041" => new FrmAccountingApprovalPolicies(),
            "ACC-042" => new FrmPostingControls(),
            "ACC-043" => new FrmAccountingAuditLog(),
            "ACC-044" => new FrmJournalIntegrityMonitor(),
            _ => null
        };
        return form is not null;
    }
}
