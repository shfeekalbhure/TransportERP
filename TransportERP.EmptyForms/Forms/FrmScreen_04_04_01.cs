using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;
public partial class UcScreen_04_04_01 : UserControl, IWorkspaceChangeState, IBatchFiveScreen
{
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public BatchFiveUiSession Binding { get; } = null!;
    public bool HasUnsavedChanges => Binding?.HasPendingChanges == true;
    public event EventHandler? CloseRequested;
    public UcScreen_04_04_01()
    {
        InitializeComponent();
        // A nested screen may be instantiated by the designer before Site is assigned.
        if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
            return;
        ConfigureColumnContracts();
        ConfigureReceiptWaybillColumn();
        ConfigureReceiptTabControls();
        field_operation.DisplayMember = nameof(BatchFiveChoice.Label);
        field_operation.ValueMember = nameof(BatchFiveChoice.Id);
        field_operation.DataSource = new[] { new BatchFiveChoice("CASH", "نقدي"), new BatchFiveChoice("CHEQUE", "شيك") };
        // Open the main data page by reference while preserving designer tab order.
        tabs.SelectedTab = tp0;
        // Preserve this screen's designer font and background after shared setup.
        var designerFont = Font;
        var designerBackColor = BackColor;
        ScreenProperties.Apply(this);
        Font = designerFont;
        BackColor = designerBackColor;
        ConfigureImportLayout();
        flpActions.SizeChanged += (_, _) => FitToolbarHeight();
        FitToolbarHeight();
        // Screen-specific appearance is serialized in the receipt designer.
        Binding = new BatchFiveUiSession(this,lblStatus,validationErrors,
        new Dictionary<string,BatchFiveField>
        {
            ["paymentMethodCode"] = new(field_operation, true, false, false),
            ["voucherTypeRef"] = new(field_voucherType, true, false, false),
            ["salespersonRef"] = new(field_salesperson, false, false, false),
            ["collectorRef"] = new(field_collector, false, false, false),
            ["voucherNumber"] = new(field_voucherNumber, false, false, true),
            ["voucherDate"] = new(field_voucherDate, true, false, false),
            ["partyRef"] = new(field_partyRef, false, false, false),
            ["sourceCashBankRef"] = new(field_sourceCashBankRef, false, false, false),
            ["destinationCashBankRef"] = new(field_destinationCashBankRef, false, false, false),
            ["currencyRef"] = new(field_currencyRef, true, false, false),
            ["amount"] = new(field_amount, true, true, false),
            ["exchangeRate"] = new(field_exchangeRate, true, true, false),
            ["counterAccountRef"] = new(field_counterAccountRef, true, false, false),
            ["description"] = new(field_description, true, false, false),
            ["referenceNumber"] = new(field_referenceNumber, false, false, false),
            ["referenceName"] = new(field_referenceName, false, false, false),
            ["commission"] = new(field_commission, false, true, false),
            ["receiptCostCenter"] = new(referenceField15, false, false, false),
            ["receiptProject"] = new(referenceField16, false, false, false),
            ["receiptActivity"] = new(referenceField17, false, false, false),
            ["linkedDocument"] = new(receiptLinkedDocument, false, false, false),
            ["state"] = new(field_state, false, false, true),
        }, dgvLines,new Dictionary<string,Button>
        {
            ["View"] = btnView,
            ["Create"] = btnCreate,
            ["Edit"] = btnEdit,
            ["Cancel"] = btnCancel,
            ["Post"] = btnPost,
            ["Reverse"] = btnReverse,
        }, mainLayout,btnClear,btnAddRow,btnRemoveRow,
            new[] { "analyticalAccount", "accountName", "foreignAmount", "costCenter", "project", "activity", "salespersonNumber", "cashierNumber" });
        field_voucherType.DropDown += (_, _) => ExplainMissingLookup(field_voucherType, "أنواع سند القبض المعتمدة");
        field_voucherType.SelectedIndexChanged += (_, _) => UpdateReceiptWaybillVisibility();
        Binding.DocumentLoaded += UpdateReceiptWaybillVisibility;
        Binding.DraftReset += UpdateReceiptWaybillVisibility;
        field_salesperson.DropDown += (_, _) => ExplainMissingLookup(field_salesperson, "المندوبين المعتمدين: الرقم والاسم");
        field_collector.DropDown += (_, _) => ExplainMissingLookup(field_collector, "المحصلين المعتمدين: الرقم والاسم");
        documentImportChoose.Click += (_,_) => Binding.ChooseCsv(documentImportPath,documentImportPreview);
        documentImportApply.Click += (_,_) => Binding.ImportPreview(documentImportPreview);
        btnClose.Click += (_,_) => { CloseRequested?.Invoke(this,EventArgs.Empty); };
        field_operation.SelectedIndexChanged += (_, _) => UpdatePaymentMethod();
        field_destinationCashBankRef.SelectedIndexChanged += (_, _) => field_bankName.Text = (field_destinationCashBankRef.SelectedItem as BatchFiveChoice)?.Label ?? "";
        Binding.DocumentLoaded += UpdatePaymentMethod;
        Binding.DocumentLoaded += ShowEmptyContext;
        Binding.DocumentLoaded += ApplyRequestedFieldColors;
        Binding.DraftReset += UpdatePaymentMethod;
        Binding.DraftReset += ShowEmptyContext;
        Binding.DraftReset += ApplyRequestedFieldColors;
        ApplyRequestedFieldColors();
        ShowEmptyContext();
        UpdatePaymentMethod();
        ConfigureReceiptTotals();
        WireReceiptTabControls();
        btnPrint.Enabled = true;
        receiptHints.SetToolTip(btnPrint, "معاينة سند القبض؛ المسودات موسومة بوضوح ولا تُحفظ بهذه العملية.");
        btnPrint.Click += (_, _) => ShowReceiptPrintPreview();
        mainLayout.Layout += (_, _) => UpdateReceiptMinimum();
        UpdateReceiptMinimum();

    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}
    // Binding keys and validation rules are runtime contracts, not designer component names.
    private void ConfigureColumnContracts()
    {
        col_salespersonNumber.Name = "salespersonNumber";
        col_cashierNumber.Name = "cashierNumber";
        col_referenceNumber.Name = "referenceNumber";
        col_chequeNumber.Name = "chequeNumber";
        col_chequeDueDate.Name = "chequeDueDate";
        col_chequeDueDate.Tag = "date";
        documentAccountsGridColumn0.Name = "documentAccountsGridColumn0";
        documentAccountsGridColumn1.Name = "documentAccountsGridColumn1";
        documentAccountsGridColumn2.Name = "documentAccountsGridColumn2";
        documentAccountsGridColumn3.Name = "documentAccountsGridColumn3";
        documentAccountsGridColumn4.Name = "documentAccountsGridColumn4";
        documentAccountsGridColumn5.Name = "documentAccountsGridColumn5";
        documentAccountsGridColumn6.Name = "documentAccountsGridColumn6";
        documentAccountsGridColumn7.Name = "documentAccountsGridColumn7";
        col_rowNo.Name = "rowNo";
        col_counterAccountRef.Name = "counterAccountRef";
        col_lineDescription.Name = "lineDescription";
        col_currencyRef.Name = "currencyRef";
        col_exchangeRate.Name = "exchangeRate";
        col_exchangeRate.Tag = "decimal";
        col_amount.Name = "amount";
        col_amount.Tag = "decimal";
        col_partyRef.Name = "partyRef";
        col_accountingAmount.Name = "accountingAmount";
        col_analyticalAccount.Name = "analyticalAccount";
        col_accountName.Name = "accountName";
        col_foreignAmount.Name = "foreignAmount";
        col_costCenter.Name = "costCenter";
        col_project.Name = "project";
        col_activity.Name = "activity";
    }

    // The authenticated provider must supply separately role-filtered choices.
    // IDs are persisted in the local request; labels should contain number and name.
    public void SetReceiptLookups(IEnumerable<BatchFiveChoice> voucherTypes,
        IEnumerable<BatchFiveChoice> salespeople, IEnumerable<BatchFiveChoice> collectors)
    {
        Binding.SetChoices("voucherTypeRef", voucherTypes);
        Binding.SetChoices("salespersonRef", salespeople);
        Binding.SetChoices("collectorRef", collectors);
        receiptHints.SetToolTip(field_voucherType, "أنواع سند القبض المعتمدة");
        receiptHints.SetToolTip(field_salesperson, "رقم المندوب واسمه");
        receiptHints.SetToolTip(field_collector, "رقم المحصل واسمه");
    }

    private void ExplainMissingLookup(ComboBox combo, string source)
    {
        if (combo.Items.Count == 0) lblStatus.Text = "لم تُحمّل قائمة " + source;
    }

    private bool updatingReceiptTotals;
    private string? totalsCurrencyId;

    private void ConfigureReceiptTotals()
    {
        totalsCurrencyId = (field_currencyRef.SelectedItem as BatchFiveChoice)?.Id;
        field_amount.TextChanged += (_, _) => RefreshReceiptTotals();
        field_exchangeRate.TextChanged += (_, _) => RefreshReceiptTotals();
        field_operation.SelectedIndexChanged += (_, _) => RefreshReceiptTotals();
        field_currencyRef.SelectedIndexChanged += (_, _) =>
        {
            var next = (field_currencyRef.SelectedItem as BatchFiveChoice)?.Id;
            if (next != null && next != totalsCurrencyId)
            {
                totalsCurrencyId = next;
                foreach (DataGridViewRow row in dgvLines.Rows)
                    if (!row.IsNewRow) row.Cells["exchangeRate"].Value = null;
            }
            RefreshReceiptTotals();
        };
        dgvLines.CellValueChanged += (_, e) =>
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && dgvLines.Columns[e.ColumnIndex] == col_currencyRef)
                dgvLines.Rows[e.RowIndex].Cells["exchangeRate"].Value = null;
            RefreshReceiptTotals();
        };
        dgvLines.RowsAdded += (_, _) => RefreshReceiptTotals();
        dgvLines.RowsRemoved += (_, _) => RefreshReceiptTotals();
        dgvLines.CellEndEdit += (_, _) => RefreshReceiptTotals();
        Binding.DocumentLoaded += RefreshReceiptTotals;
        Binding.DraftReset += RefreshReceiptTotals;
        col_exchangeRate.ToolTipText = "وحدة من عملة السطر = سعر الصرف من عملة السند؛ المبلغ المحوّل = مبلغ السطر × السعر.";
        receiptHints.SetToolTip(field_exchangeRate, "سعر الرأس لا يحل محل سعر التحويل الخاص بكل سطر عند اختلاف العملات.");
        receiptHints.SetToolTip(field_difference, "مبلغ السند ناقص مجموع التفاصيل بعد تحويلها إلى عملة السند، دون تقريب يخفي الفارق.");
        RefreshReceiptTotals();
    }

    private void RefreshReceiptTotals()
    {
        if (updatingReceiptTotals || IsDisposed) return;
        updatingReceiptTotals = true;
        try
        {
            var currency = field_currencyRef.SelectedItem as BatchFiveChoice;
            var lines = dgvLines.Rows.Cast<DataGridViewRow>()
                .Where(row => !row.IsNewRow && row.Cells.Cast<DataGridViewCell>().Any(cell =>
                    cell.OwningColumn.Name != "rowNo" && !string.IsNullOrWhiteSpace(Convert.ToString(cell.Value))))
                .Select(row => new ReceiptTotalsCalculator.Line(
                    Convert.ToString(row.Cells["currencyRef"].Value), row.Cells["amount"].Value, row.Cells["exchangeRate"].Value));
            var result = ReceiptTotalsCalculator.Calculate(currency?.Id, field_amount.Text, lines);
            SetCalculatedTotals(result.Total, result.Difference);
            lblTotalsMessage.Text = result.Error ?? "بعملة السند: " + currency!.Label;
            lblTotalsMessage.ForeColor = result.Error == null ? Color.FromArgb(34, 66, 96) : Color.Maroon;
            receiptHints.SetToolTip(field_total, result.Error ?? "مجموع مبالغ التفاصيل بعملة السند: " + currency!.Label);
        }
        finally { updatingReceiptTotals = false; }
    }

    // Presentation only; this does not persist or post the voucher.
    public void SetCalculatedTotals(decimal? total, decimal? difference)
    {
        field_total.Text = total?.ToString("0.############################", System.Globalization.CultureInfo.CurrentCulture) ?? "—";
        field_difference.Text = difference?.ToString("0.############################", System.Globalization.CultureInfo.CurrentCulture) ?? "—";
    }

    private void ApplyRequestedFieldColors()
    {
        foreach (Control editor in new Control[] { textBox1, field_voucherType, field_destinationCashBankRef, field_voucherNumber, field_amount, field_voucherDate })
            editor.BackColor = Color.LightYellow;
        field_voucherDate.CalendarMonthBackground = Color.LightYellow;
    }

    private void ShowEmptyContext()
    {
        if (string.IsNullOrWhiteSpace(context2.Text)) context2.Text = "لم تُحمّل مرفقات أو مستندات مرتبطة لهذا السند.";
        if (string.IsNullOrWhiteSpace(context3.Text)) context3.Text = "لم تُحمّل بيانات الاعتمادات لهذا السند.";
        if (string.IsNullOrWhiteSpace(context4.Text)) context4.Text = "لم يُحمّل سجل العمليات لهذا السند.";
    }

    private void FitToolbarHeight()
    {
        var buttons = flpActions.Controls.OfType<Button>().ToArray();
        if (buttons.Length == 0) return;
        int width = buttons.Sum(button => button.Width + button.Margin.Horizontal);
        int height = buttons.Max(button => button.Height + button.Margin.Vertical) + flpActions.Padding.Vertical;
        if (width > flpActions.ClientSize.Width - flpActions.Padding.Horizontal)
            height += SystemInformation.HorizontalScrollBarHeight;
        mainLayout.RowStyles[0].Height = height + flpActions.Margin.Vertical + 2;
    }

    private void UpdatePaymentMethod()
    {
        // Switching presentation must never discard an entered cheque or alter financial validation.
        bool cheque = (field_operation.SelectedItem as BatchFiveChoice)?.Id == "CHEQUE";
        col_chequeNumber.Visible = cheque;
        col_chequeDueDate.Visible = cheque;
        lbl_destinationCashBankRef.Text = cheque ? "رقم البنك" : "رقم الصندوق";
        col_amount.HeaderText = cheque ? "مبلغ الشيك" : "المبلغ";
        // Reference-cell highlighting is visual only; required validation is unchanged.
        foreach (var column in new DataGridViewColumn[] { col_counterAccountRef, col_analyticalAccount, col_amount })
            column.DefaultCellStyle.BackColor = Color.LightYellow;
        dgvLines.DefaultCellStyle.SelectionBackColor = Color.FromArgb(210, 250, 250);
        dgvLines.DefaultCellStyle.SelectionForeColor = Color.Black;
        DataGridViewColumn[] referenceOrder = { col_rowNo, col_counterAccountRef, col_analyticalAccount, col_accountName, col_lineDescription, col_currencyRef, col_exchangeRate, col_amount, col_foreignAmount, col_costCenter, col_referenceNumber, col_salespersonNumber, col_cashierNumber, col_chequeNumber, col_chequeDueDate, col_partyRef, col_accountingAmount, col_project, col_activity };
        for (int i = 0; i < referenceOrder.Length; i++) referenceOrder[i].DisplayIndex = i;
    }

    private void UpdateReceiptMinimum()
    {
        int width = Math.Max(1, mainLayout.ClientSize.Width - mainLayout.Padding.Horizontal);
        int height = mainLayout.Padding.Vertical + (int)Math.Ceiling(mainLayout.RowStyles[0].Height)
            + (tabs.SelectedTab == documentImport ? 180 : (int)Math.Ceiling(mainLayout.RowStyles[1].Height) + dgvLines.MinimumSize.Height + dgvLines.Margin.Vertical)
            + linesHost.GetPreferredSize(new Size(width, 0)).Height + linesHost.Margin.Vertical
            + tlpAuditInfo.GetPreferredSize(new Size(width, 0)).Height + tlpAuditInfo.Margin.Vertical
            + lblStatus.GetPreferredSize(new Size(width, 0)).Height + lblStatus.Margin.Vertical;
        var minimum = new Size(980, height);
        if (mainLayout.MinimumSize != minimum) mainLayout.MinimumSize = minimum;
        var scrollMinimum = new Size(minimum.Width + Padding.Horizontal, minimum.Height + Padding.Vertical);
        if (AutoScrollMinSize != scrollMinimum) AutoScrollMinSize = scrollMinimum;
    }

    private void ConfigureImportLayout()
    {
        var headerRow = mainLayout.RowStyles[1];
        var detailsRow = mainLayout.RowStyles[2];
        var headerSizeType = headerRow.SizeType;
        var headerHeight = headerRow.Height;
        var detailsSizeType = detailsRow.SizeType;
        var detailsHeight = detailsRow.Height;

        void UpdateLayout()
        {
            var importing = tabs.SelectedTab == documentImport;
            mainLayout.SuspendLayout();
            try
            {
                // Keep rows and bindings intact; only give the preview their space.
                dgvLines.Visible = !importing;
                detailsRow.SizeType = importing ? SizeType.Absolute : detailsSizeType;
                detailsRow.Height = importing ? 0F : detailsHeight;
                headerRow.SizeType = importing ? SizeType.Percent : headerSizeType;
                headerRow.Height = importing ? 100F : tabs.SelectedTab == tp0
                    ? referenceHeader.GetPreferredSize(new Size(Math.Max(1, tabs.DisplayRectangle.Width - tp0.Padding.Horizontal), 0)).Height
                        + tabs.Height - tabs.DisplayRectangle.Height + tp0.Padding.Vertical + tabs.Margin.Vertical + 4
                    : headerHeight;
            }
            finally
            {
                mainLayout.ResumeLayout(true);
            }
        }

        tabs.SelectedIndexChanged += (_, _) => UpdateLayout();
        tabs.SizeChanged += (_, _) => UpdateLayout();
        UpdateLayout();
    }

}
// Keep native date entry, checkbox and calendar behavior. Paint the requested
// background only when idle; native focused rendering retains keyboard selection.
public class ReceiptDateTimePicker : DateTimePicker
{
    protected override void OnGotFocus(EventArgs e) { base.OnGotFocus(e); Invalidate(); }
    protected override void OnLostFocus(EventArgs e) { base.OnLostFocus(e); Invalidate(); }
    protected override void OnValueChanged(EventArgs e) { base.OnValueChanged(e); Invalidate(); }
    protected override void WndProc(ref Message m)
    {
        base.WndProc(ref m);
        if (m.Msg != 0x000F || Focused || !IsHandleCreated) return;
        using var graphics = Graphics.FromHwnd(Handle);
        var bounds = new Rectangle(2, 2, Math.Max(0, ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 4), Math.Max(0, ClientSize.Height - 4));
        using var background = new SolidBrush(BackColor);
        graphics.FillRectangle(background, bounds);
        if (ShowCheckBox)
        {
            var checkBounds = new Rectangle(bounds.Left + 1, Math.Max(2, (Height - 13) / 2), 13, 13);
            ControlPaint.DrawCheckBox(graphics, checkBounds, Checked ? ButtonState.Checked : ButtonState.Normal);
            bounds.X += 17; bounds.Width = Math.Max(0, bounds.Width - 17);
        }
        TextRenderer.DrawText(graphics, Value.ToString(CustomFormat, System.Globalization.CultureInfo.InvariantCulture), Font, bounds,
            Enabled && (!ShowCheckBox || Checked) ? ForeColor : SystemColors.GrayText,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);
    }
}
