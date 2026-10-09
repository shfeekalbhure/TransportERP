using System.Globalization;
using TransportERP.Contracts.Accounting;

namespace TransportERP.EmptyForms;

public partial class UcScreen_04_04_01
{
    private Guid receiptDraftId = Guid.NewGuid();
    private ReceiptDocument? receiptDocument;
    private ReceiptDocument? pendingReceiptDocument;
    public event EventHandler? ReceiptSettingsRequested;

    public void ConnectReceiptWorkspace(ReceiptBootstrap bootstrap,
        Func<string, ReceiptDraft?, Guid?, string?, Task<ReceiptDocument?>> execute)
    {
        static BatchFiveChoice Choice(ReceiptChoice c) => new(c.Id.ToString(), c.Label);
        var configuration = bootstrap.Configuration;
        Binding.SetChoices("currencyRef", bootstrap.Currencies.Select(Choice));
        Binding.SetChoices("counterAccountRef", bootstrap.Accounts.Select(Choice));
        Binding.SetChoices("partyRef", bootstrap.Accounts.Select(Choice));
        Binding.SetChoices("destinationCashBankRef", configuration.Destinations.Select(d => new BatchFiveChoice(d.Id.ToString(), d.Label)));
        SetReceiptLookups(configuration.Types.Select(t => new BatchFiveChoice(t.Id.ToString(), t.Label)),
            bootstrap.Users.Where(u => configuration.SalespersonIds.Contains(u.Id)).Select(Choice),
            bootstrap.Users.Where(u => configuration.CollectorIds.Contains(u.Id)).Select(Choice));
        SetReceiptWaybillLookups(configuration.Types.Where(t => t.RequiresWaybill).Select(t => t.Id.ToString()), bootstrap.Waybills.Select(Choice));
        LoadReceiptTabChoices(bootstrap);
        SetReceiptBranchName(bootstrap.BranchName);
        Binding.DraftReset += () =>
        {
            receiptDraftId = Guid.NewGuid(); receiptDocument = null; pendingReceiptDocument = null;
            SetReceiptBranchName(bootstrap.BranchName); field_attachmentCount.Text = "";
            field_voucherDate.Value = DateTime.Today; field_voucherDate.Checked = true;
            field_operation.SelectedValue = "CASH";
            lblCreatedBy.Text = "أنشأ بواسطة: —"; lblCreatedAt.Text = "تاريخ الإنشاء: —";
            lblModifiedBy.Text = "عدل بواسطة: —"; lblModifiedAt.Text = "تاريخ التعديل: —"; lblEditCount.Text = "عدد التعديلات: —";
            ApplyReceiptDefaults();
        };
        var allowed = bootstrap.Actions.Where(a => a.StartsWith("ACC043.")).Select(a => a[7..]).ToArray();
        Binding.CanStartNewDraft = allowed.Contains("Create");
        async Task<BatchFiveResult> Handle(BatchFiveRequest request)
        {
            try
            {
            ReceiptDraft? draft = request.Action is "Create" or "Edit" ? CaptureReceiptDraft(request) : null;
            var document = await execute(request.Action, draft, receiptDocument?.Draft.Id, request.ExpectedVersion);
            if (document == null) return new(false, false, "لم يُحمّل سند.");
            string message = document.WorkflowMessage ?? "تم تحميل الحالة المحفوظة: " + document.State;
            EnsureReceiptDocumentChoices(document);
            pendingReceiptDocument = document;
            return new(true, true, message, document.Version, ToBindingDocument(document));
            }
            catch (Exception ex) when (ex is InvalidOperationException or FormatException or OverflowException)
            { return new(false, false, ex.Message); }
        }
        string[] StateActions() => allowed.Where(action => action == "View" ||
            (action == "Post" && receiptDocument != null && EffectiveReceiptPolicy.IsReady(receiptDocument.State)) || (receiptDocument?.State switch
        {
            null => action == "Create",
            "DRAFT" => action is "Edit" or "Cancel",
            "REVIEWED" or "APPROVED" => action == "Cancel",
            "POSTED" => action == "Reverse",
            _ => false
        })).ToArray();
        Binding.BindCommands(Handle, StateActions());
        Binding.DraftReset += () => Binding.BindCommands(Handle, StateActions());
        Binding.DocumentLoaded += () =>
        {
            if (pendingReceiptDocument is { } pending && Binding.ExpectedVersion == pending.Version)
            {
                receiptDocument = pending; receiptDraftId = pending.Draft.Id;
                RestoreReceiptAdditional(pending.Draft); pendingReceiptDocument = null;
                UpdateReceiptWaybillVisibility();
                ShowReceiptAudit(pending);
            }
            if (receiptDocument != null) { ShowReceiptAudit(receiptDocument); ShowReceiptAttachments(receiptDocument); }
            RefreshReceiptWorkflowState();
            Binding.BindCommands(Handle, StateActions());
            if (allowed.Contains("Create")) btnClear.Enabled = true;
        };
        var settingsButton = btnReceiptSettings;
        settingsButton.Visible = true;
        settingsButton.Enabled = bootstrap.Actions.Contains("accounting.receipts.configure");
        settingsButton.Click += (_, _) => ReceiptSettingsRequested?.Invoke(this, EventArgs.Empty);
        var approve = btnReceiptApprove;
        approve.Visible = true;
        RefreshReceiptWorkflowState();
        approve.Click += async (_, _) =>
        {
            var stage = NextReceiptStage();
            if (Binding.IsBusy || receiptTabsBusy || HasUnsavedChanges || receiptDocument == null || stage == null ||
                !bootstrap.Actions.Contains(stage == "Review" ? "accounting.receipts.review" : "accounting.receipts.approve"))
            { lblStatus.Text = "احفظ السند أولًا؛ تتطلب المرحلة صلاحيتها وحالتها الصحيحة."; return; }
            var caption = stage == "Review" ? "مراجعة" : "اعتماد";
            if (MessageBox.Show(this, caption + " سند القبض المحفوظ؟", caption, MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            receiptWorkflowBusy = true; approve.Enabled = false;
            Enabled = false;
            try
            {
                var approved = await execute(stage, null, receiptDocument.Draft.Id, receiptDocument.Version);
                if (approved == null) return;
                EnsureReceiptDocumentChoices(approved); pendingReceiptDocument = approved;
                var document = ToBindingDocument(approved);
                if (Binding.LoadDocument(document.Fields, document.Rows, document.Version, document.CanEdit))
                { lblStatus.Text = approved.WorkflowMessage ?? "تم تحميل الحالة المحفوظة: " + approved.State; }
                else pendingReceiptDocument = null;
            }
            catch (Exception ex) { lblStatus.Text = "تعذرت " + caption + ": " + ex.Message; }
            finally { receiptWorkflowBusy = false; Enabled = true; RefreshReceiptWorkflowState(); }
        };
        lblStatus.Text = configuration.Types.Count == 0 || configuration.Destinations.Count == 0
            ? "يلزم تهيئة أنواع ووجهات سند القبض في إعدادات الشركة." : "متصل بخدمة سند القبض";
        ApplyReceiptDefaults();
    }

    private ReceiptDraft CaptureReceiptDraft(BatchFiveRequest request)
    {
        static string Text(object? value) => Convert.ToString(value, CultureInfo.CurrentCulture) ?? "";
        Guid Id(string key) => Guid.TryParse(Text(request.Fields.GetValueOrDefault(key)), out var id) ? id : Guid.Empty;
        decimal Money(string key) => decimal.Parse(Text(request.Fields.GetValueOrDefault(key)), CultureInfo.CurrentCulture);
        var extra = new Dictionary<string, string?>
        {
            ["referenceNumber"] = field_referenceNumber.Text, ["referenceName"] = field_referenceName.Text,
            ["commission"] = field_commission.Text, ["salespersonRef"] = Text(request.Fields.GetValueOrDefault("salespersonRef")),
            ["partyRef"] = Text(request.Fields.GetValueOrDefault("partyRef")), ["partyName"] = field_partyRef.Text,
            ["costCenter"] = Text(request.Fields.GetValueOrDefault("receiptCostCenter")),
            ["project"] = Text(request.Fields.GetValueOrDefault("receiptProject")),
            ["activity"] = Text(request.Fields.GetValueOrDefault("receiptActivity")),
            ["sourceCashBankRef"] = Text(request.Fields.GetValueOrDefault("sourceCashBankRef"))
        };
        var linked = Text(request.Fields.GetValueOrDefault("linkedDocument")).Split(':', 2);
        extra["linkedDocumentKind"] = linked.Length == 2 ? linked[0] : null;
        extra["linkedDocumentId"] = linked.Length == 2 ? linked[1] : null;
        extra["counterAccountRef"] = Text(request.Fields.GetValueOrDefault("counterAccountRef"));
        var lines = new List<ReceiptLine>();
        foreach (DataGridViewRow row in dgvLines.Rows)
        {
            if (row.IsNewRow || !row.Cells.Cast<DataGridViewCell>().Any(c => c.OwningColumn.Name != "rowNo" && !string.IsNullOrWhiteSpace(Text(c.Value)))) continue;
            string Value(string key) => Text(row.Cells[key].Value);
            Guid RowId(string key) => Guid.TryParse(Value(key), out var id) ? id : Guid.Empty;
            Guid waybill = RowId("waybillRef");
            var details = row.Cells.Cast<DataGridViewCell>().ToDictionary(c => c.OwningColumn.Name, c => (string?)Text(c.Value));
            foreach (var key in new[] { "costCenter", "project", "activity" }) if (string.IsNullOrWhiteSpace(details.GetValueOrDefault(key))) details[key] = extra[key];
            lines.Add(new(RowId("counterAccountRef"), RowId("currencyRef"), decimal.Parse(Value("amount"), CultureInfo.CurrentCulture),
                decimal.TryParse(Value("exchangeRate"), CultureInfo.CurrentCulture, out var rate) ? rate : null,
                Value("lineDescription"), Value("chequeNumber"),
                DateTime.TryParse(Value("chequeDueDate"), CultureInfo.CurrentCulture, out var due) ? due.Date : null,
                waybill == Guid.Empty ? null : waybill, details));
        }
        return new(receiptDraftId, request.ExpectedVersion, field_voucherDate.Value.Date,
            Text(request.Fields.GetValueOrDefault("paymentMethodCode")), Id("currencyRef"), Money("amount"), Money("exchangeRate"),
            Id("destinationCashBankRef"), Id("voucherTypeRef"), Id("collectorRef"), field_description.Text, lines, extra);
    }

    private static BatchFiveDocument ToBindingDocument(ReceiptDocument document)
    {
        var d = document.Draft;
        var fields = new Dictionary<string, object?>
        {
            ["voucherNumber"] = document.Number, ["voucherDate"] = d.Date, ["paymentMethodCode"] = d.Method,
            ["currencyRef"] = d.CurrencyId.ToString(), ["amount"] = d.Amount, ["exchangeRate"] = d.ExchangeRate,
            ["destinationCashBankRef"] = d.DestinationId.ToString(), ["voucherTypeRef"] = d.TypeId.ToString(),
            ["collectorRef"] = d.CollectorId.ToString(), ["description"] = d.Description, ["state"] = document.State,
            ["salespersonRef"] = Empty(d.Additional.GetValueOrDefault("salespersonRef")),
            ["partyRef"] = Empty(d.Additional.GetValueOrDefault("partyRef"))
        };
        fields["counterAccountRef"] = Empty(d.Additional.GetValueOrDefault("counterAccountRef"));
        fields["referenceNumber"] = d.Additional.GetValueOrDefault("referenceNumber");
        fields["referenceName"] = d.Additional.GetValueOrDefault("referenceName");
        fields["commission"] = d.Additional.GetValueOrDefault("commission");
        fields["receiptCostCenter"] = d.Additional.GetValueOrDefault("costCenter");
        fields["receiptProject"] = Empty(d.Additional.GetValueOrDefault("project"));
        fields["receiptActivity"] = Empty(d.Additional.GetValueOrDefault("activity"));
        fields["sourceCashBankRef"] = Empty(d.Additional.GetValueOrDefault("sourceCashBankRef"));
        fields["linkedDocument"] = string.IsNullOrWhiteSpace(d.Additional.GetValueOrDefault("linkedDocumentId")) ? null : d.Additional.GetValueOrDefault("linkedDocumentKind") + ":" + d.Additional["linkedDocumentId"];
        fields["receiptCostCenter"] = Empty(d.Additional.GetValueOrDefault("costCenter"));
        var rows = d.Lines.Select((l, i) =>
        {
            var row = (l.Additional ?? new()).ToDictionary(p => p.Key, p => (object?)Empty(p.Value));
            row["rowNo"] = i + 1; row["counterAccountRef"] = l.AccountId.ToString(); row["currencyRef"] = l.CurrencyId.ToString();
            row["amount"] = l.Amount; row["exchangeRate"] = l.Rate; row["lineDescription"] = l.Description;
            row["foreignAmount"] = l.Amount;
            row["accountingAmount"] = decimal.TryParse(l.Additional?.GetValueOrDefault("accountingAmount"), NumberStyles.Number,
                CultureInfo.InvariantCulture, out var accounting) ? accounting : null;
            row["chequeNumber"] = l.ChequeNumber; row["chequeDueDate"] = l.ChequeDueDate?.ToString("yyyy-MM-dd");
            row["waybillRef"] = l.WaybillId?.ToString();
            return (IReadOnlyDictionary<string, object?>)row;
        }).ToArray();
        return new(fields, rows, document.Version, document.State == "DRAFT");
    }
    private static string? Empty(string? value) => string.IsNullOrWhiteSpace(value) ? null : value;
    private void EnsureReceiptDocumentChoices(ReceiptDocument document)
    {
        void Merge(string key, ComboBox? combo, IEnumerable<string?> ids)
        {
            var choices = combo?.Items.OfType<BatchFiveChoice>().ToList() ??
                ((dgvLines.Columns[key] as DataGridViewComboBoxColumn)?.DataSource as IEnumerable<BatchFiveChoice>)?.ToList() ?? [];
            foreach (var id in ids.Where(id => !string.IsNullOrWhiteSpace(id)).Distinct())
                if (!choices.Any(c => c.Id == id)) choices.Add(new(id!, "مرجع محفوظ: " + id));
            Binding.SetChoices(key, choices);
        }
        var d = document.Draft;
        Merge("currencyRef", field_currencyRef, d.Lines.Select(l => (string?)l.CurrencyId.ToString()).Append(d.CurrencyId.ToString()));
        Merge("counterAccountRef", field_counterAccountRef, d.Lines.Select(l => (string?)l.AccountId.ToString()).Append(d.Additional.GetValueOrDefault("counterAccountRef")));
        Merge("destinationCashBankRef", field_destinationCashBankRef, [d.DestinationId.ToString()]);
        Merge("voucherTypeRef", field_voucherType, [d.TypeId.ToString()]);
        Merge("collectorRef", field_collector, [d.CollectorId.ToString()]);
        Merge("salespersonRef", field_salesperson, [d.Additional.GetValueOrDefault("salespersonRef")]);
        Merge("partyRef", field_partyRef, d.Lines.Select(l => l.Additional?.GetValueOrDefault("partyRef")).Append(d.Additional.GetValueOrDefault("partyRef")));
        Merge("waybillRef", null, d.Lines.Select(l => l.WaybillId?.ToString()));
        Merge("receiptCostCenter", referenceField15, [d.Additional.GetValueOrDefault("costCenter")]);
        Merge("receiptProject", referenceField16, [d.Additional.GetValueOrDefault("project")]);
        Merge("receiptActivity", referenceField17, [d.Additional.GetValueOrDefault("activity")]);
        Merge("sourceCashBankRef", field_sourceCashBankRef, [d.Additional.GetValueOrDefault("sourceCashBankRef")]);
        if (!string.IsNullOrWhiteSpace(d.Additional.GetValueOrDefault("linkedDocumentId")))
            Merge("linkedDocument", receiptLinkedDocument, [d.Additional.GetValueOrDefault("linkedDocumentKind") + ":" + d.Additional["linkedDocumentId"]]);
    }
    private void ShowReceiptAudit(ReceiptDocument document)
    {
        var events = document.Audit ?? [];
        var saves = events.Where(e => e.Action == "SaveReceipt").ToArray();
        lblCreatedBy.Text = "المستخدم: " + (saves.FirstOrDefault()?.Actor ?? "—");
        lblCreatedAt.Text = "تاريخ الإنشاء: " + (saves.FirstOrDefault()?.At.ToLocalTime().ToString("yyyy-MM-dd HH:mm") ?? "—");
        lblModifiedBy.Text = "المعدل: " + (saves.LastOrDefault()?.Actor ?? "—");
        lblModifiedAt.Text = "تاريخ التعديل: " + (saves.LastOrDefault()?.At.ToLocalTime().ToString("yyyy-MM-dd HH:mm") ?? "—");
        lblEditCount.Text = "عدد التعديلات: " + Math.Max(0, saves.Length - 1);
        context4.Text = string.Join(Environment.NewLine, events.Select(e => $"{e.At.ToLocalTime():yyyy-MM-dd HH:mm} — {e.Actor} — {e.Action} {e.Reason}"));
        context3.Text = document.State + Environment.NewLine + string.Join(Environment.NewLine,
            events.Where(e => e.Action == "ApproveReceipt").Select(e => $"اعتمد بواسطة {e.Actor} في {e.At.ToLocalTime():yyyy-MM-dd HH:mm}"));
    }
    private void RestoreReceiptAdditional(ReceiptDraft draft)
    {
        field_referenceNumber.Text = draft.Additional.GetValueOrDefault("referenceNumber");
        field_referenceName.Text = draft.Additional.GetValueOrDefault("referenceName");
        field_commission.Text = draft.Additional.GetValueOrDefault("commission");
        field_referenceType.Text = draft.Additional.GetValueOrDefault("linkedDocumentKind") switch { "WAYBILL" => "بوليصة", "RECEIPT" => "سند قبض", "JOURNAL" => "قيد", _ => "" };
    }
}
