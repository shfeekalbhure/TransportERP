using System.Globalization;
using TransportERP.Contracts.Accounting;

namespace TransportERP.EmptyForms;

public partial class UcScreen_04_04_01
{
    private readonly ComboBox receiptLinkedDocument = new() { Name = "receiptLinkedDocument", Dock = DockStyle.Top, DropDownStyle = ComboBoxStyle.DropDownList, AccessibleName = "المستند المرتبط" };
    private readonly ListBox receiptAttachmentList = new() { Name = "receiptAttachmentList", Dock = DockStyle.Fill, DisplayMember = "FileName" };
    private readonly Button receiptUpload = new() { Name = "receiptUpload", Text = "إضافة مرفق", AutoSize = true };
    private readonly Button receiptDownload = new() { Name = "receiptDownload", Text = "حفظ نسخة من المرفق", AutoSize = true };
    private readonly Button receiptUndo = new() { Name = "receiptUndo", Text = "تراجع", Width = 88, Height = 36, Enabled = false };
    private readonly Button receiptSaveDefaults = new() { Text = "حفظ القيم الافتراضية", AutoSize = true, Dock = DockStyle.Bottom, Enabled = false };
    private readonly Label receiptAccountsStatus = new() { Dock = DockStyle.Top, Height = 26 };
    private ReceiptBootstrap? receiptBootstrap;
    private bool receiptTabsBusy;
    private Func<ReceiptConfiguration, Task<ReceiptBootstrap>>? saveReceiptDefaults;
    private Func<Guid, ReceiptAttachmentUpload, Task<ReceiptDocument>>? uploadReceiptAttachment;
    private Func<Guid, Guid, Task<ReceiptAttachmentDownload>>? downloadReceiptAttachment;

    private void ConfigureReceiptTabControls()
    {
        lbl_voucherType.Text = "نوع السند *";
        receiptHints.SetToolTip(field_voucherType, "اختر نوع سند معتمد — حقل مطلوب للحفظ.");
        foreach (var c in new[] { referenceField15, referenceField16, referenceField17 })
        { c.TabStop = true; c.AccessibleDescription = "اختيار مرجع من الأبعاد المالية للشركة"; c.BackColor = Color.White; }
        receiptUndo.AutoSize = true; receiptUndo.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        receiptUndo.MinimumSize = btnClear.MinimumSize; receiptUndo.Size = btnClear.Size;
        receiptUndo.Margin = btnClear.Margin; receiptUndo.Font = btnClear.Font;
        flpActions.Controls.Add(receiptUndo); receiptUndo.TabIndex = 11;
        field_postingMethod.ReadOnly = true;
        field_postingMethod.TabStop = false;
        tp2.Controls.Clear();
        var attachmentButtons = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 36, FlowDirection = FlowDirection.RightToLeft };
        attachmentButtons.Controls.AddRange([receiptUpload, receiptDownload]);
        var unlink = new Button { Text = "مسح الربط من المسودة", AutoSize = true };
        unlink.Click += (_, _) => { if (receiptDocument == null || receiptDocument.State == "DRAFT") receiptLinkedDocument.SelectedIndex = -1; };
        attachmentButtons.Controls.Add(unlink);
        tp2.Controls.Add(receiptAttachmentList); tp2.Controls.Add(attachmentButtons);
        tp2.Controls.Add(receiptLinkedDocument);
        tp2.Controls.Add(new Label { Text = "المستند المرتبط (مرجع للسند)", Dock = DockStyle.Top, Height = 24 });
        documentDefaults.Controls.Add(receiptSaveDefaults);
        documentAccounts.Controls.Add(receiptAccountsStatus);
        documentAccountsGrid.BringToFront();
        string[] headings = ["الحساب", "مرجع تحليلي", "البيان", "العملة", "المبلغ بعملة السطر", "سعر التحويل للسند", "مدين أساسي", "دائن أساسي"];
        for (int i = 0; i < headings.Length; i++) documentAccountsGrid.Columns[i].HeaderText = headings[i];
        documentAccountsGrid.AccessibleDescription = "تفاصيل المسودة، وبعد الترحيل يعرض أسطر القيد المحفوظة فعليًا";
        var template = new Button { Text = "حفظ قالب CSV", AutoSize = true, Dock = DockStyle.Bottom };
        documentImport.Controls.Add(template);
        template.Click += (_, _) =>
        {
            using var dialog = new SaveFileDialog { FileName = "receipt-template.csv", Filter = "CSV UTF-8|*.csv", OverwritePrompt = true };
            if (dialog.ShowDialog(this) != DialogResult.OK) return;
            try
            {
                var columns = dgvLines.Columns.Cast<DataGridViewColumn>().Where(c => !c.ReadOnly).ToArray();
                File.WriteAllText(dialog.FileName, string.Join(',', columns.Select(c => c.Name)) + Environment.NewLine, new System.Text.UTF8Encoding(true));
                lblStatus.Text = "حُفظ القالب. استخدم معرفات القوائم المعتمدة، والتاريخ yyyy-MM-dd؛ لا يغيّر الاستيراد السند قبل الحفظ.";
            }
            catch (IOException ex) { lblStatus.Text = "تعذر حفظ القالب: " + ex.Message; }
        };
    }

    private void WireReceiptTabControls()
    {
        receiptUndo.Click += (_, _) => Binding.UndoLocalChanges(() => MessageBox.Show(this,
            "التراجع عن الإدخال غير المحفوظ والعودة إلى آخر نسخة؟", "تراجع عن الإدخال", MessageBoxButtons.YesNo,
            MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes);
        Binding.StateChanged += RefreshReceiptTabState;
        Binding.AdditionalPendingChanges = () => receiptBootstrap != null &&
            (SelectedDefault(cboDefaultCurrency) != receiptBootstrap.Configuration.DefaultCurrencyId || SelectedDefault(cboDefaultCostCenter) != receiptBootstrap.Configuration.DefaultCostCenterId);
        cboDefaultCurrency.SelectedIndexChanged += (_, _) => RefreshReceiptTabState();
        cboDefaultCostCenter.SelectedIndexChanged += (_, _) => RefreshReceiptTabState();
        Binding.DocumentLoaded += () => { RestoreDefaultSelections(); RefreshReceiptAccounts(); RefreshReceiptTabState(); };
        dgvLines.CellValueChanged += (_, _) => RefreshReceiptAccounts();
        dgvLines.RowsRemoved += (_, _) => RefreshReceiptAccounts();
        dgvLines.CellFormatting += (_, e) =>
        {
            if (e.RowIndex < 0 || dgvLines.Columns[e.ColumnIndex].Name is not ("costCenter" or "project" or "activity")) return;
            if (Guid.TryParse(Convert.ToString(e.Value), out var id) && receiptBootstrap?.Dimensions?.FirstOrDefault(d => d.Id == id) is { } dimension)
            { e.Value = dimension.Label; e.FormattingApplied = true; }
        };
        receiptAttachmentList.SelectedIndexChanged += (_, _) => RefreshReceiptTabState();
        receiptSaveDefaults.Click += async (_, _) =>
        {
            if (receiptBootstrap == null || saveReceiptDefaults == null || receiptTabsBusy) return;
            Guid? Selected(ComboBox c) => c.SelectedItem is BatchFiveChoice choice && Guid.TryParse(choice.Id, out var id) ? id : null;
            var configuration = receiptBootstrap.Configuration with { DefaultCurrencyId = Selected(cboDefaultCurrency), DefaultCostCenterId = Selected(cboDefaultCostCenter) };
            await ReceiptTabOperation(async () => { receiptBootstrap = await saveReceiptDefaults(configuration); lblStatus.Text = "حُفظت القيم الافتراضية؛ تُطبق على السند الجديد والأسطر الجديدة فقط."; });
        };
        receiptUpload.Click += async (_, _) =>
        {
            if (receiptDocument == null || uploadReceiptAttachment == null || HasUnsavedChanges) { lblStatus.Text = "احفظ تعديلات السند قبل إضافة المرفق."; return; }
            using var dialog = new OpenFileDialog { Filter = "مرفقات (*.pdf;*.png;*.jpg;*.jpeg;*.txt)|*.pdf;*.png;*.jpg;*.jpeg;*.txt", CheckFileExists = true };
            if (dialog.ShowDialog(this) != DialogResult.OK) return;
            await ReceiptTabOperation(async () =>
            {
                if (new FileInfo(dialog.FileName).Length > 5 * 1024 * 1024) throw new InvalidOperationException("الحد الأقصى للمرفق 5 ميجابايت.");
                var upload = new ReceiptAttachmentUpload(Guid.NewGuid(), receiptDocument.Version, Path.GetFileName(dialog.FileName), await File.ReadAllBytesAsync(dialog.FileName));
                var result = await uploadReceiptAttachment(receiptDocument.Draft.Id, upload);
                ApplyReceiptTabDocument(result); lblStatus.Text = "حُفظ المرفق واستُرجع ضمن السند.";
            });
        };
        receiptDownload.Click += async (_, _) =>
        {
            if (receiptDocument == null || downloadReceiptAttachment == null || receiptAttachmentList.SelectedItem is not ReceiptAttachmentInfo selected) return;
            using var dialog = new SaveFileDialog { FileName = Path.GetFileName(selected.FileName), OverwritePrompt = true, Filter = "ملف مرفق|*.*" };
            if (dialog.ShowDialog(this) != DialogResult.OK) return;
            await ReceiptTabOperation(async () =>
            {
                var result = await downloadReceiptAttachment(receiptDocument.Draft.Id, selected.Id);
                if (Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(result.Content)) != result.Info.Hash) throw new InvalidOperationException("بصمة المرفق غير مطابقة.");
                await File.WriteAllBytesAsync(dialog.FileName, result.Content); lblStatus.Text = "حُفظت نسخة المرفق في المسار المختار.";
            });
        };
        btnAddRow.Click += (_, _) =>
        {
            if (receiptBootstrap == null || dgvLines.Rows.Count == 0) return;
            var row = dgvLines.Rows[dgvLines.Rows.Count - 1];
            if (row.Cells["currencyRef"].Value == null) row.Cells["currencyRef"].Value = (field_currencyRef.SelectedItem as BatchFiveChoice)?.Id ?? receiptBootstrap.Configuration.DefaultCurrencyId?.ToString();
            foreach (var (key, choice) in new[] { ("costCenter", referenceField15), ("project", referenceField16), ("activity", referenceField17) })
                if (row.Cells[key].Value == null) row.Cells[key].Value = (choice.SelectedItem as BatchFiveChoice)?.Id;
        };
    }

    private static Guid? SelectedDefault(ComboBox c) => c.SelectedItem is BatchFiveChoice choice && Guid.TryParse(choice.Id, out var id) ? id : null;
    private void RestoreDefaultSelections()
    {
        if (receiptBootstrap == null) return;
        cboDefaultCurrency.SelectedIndex = -1; cboDefaultCostCenter.SelectedIndex = -1;
        if (receiptBootstrap.Configuration.DefaultCurrencyId is { } currency) cboDefaultCurrency.SelectedValue = currency.ToString();
        if (receiptBootstrap.Configuration.DefaultCostCenterId is { } center) cboDefaultCostCenter.SelectedValue = center.ToString();
    }

    public void ConnectReceiptTabServices(Func<ReceiptConfiguration, Task<ReceiptBootstrap>> saveDefaults,
        Func<Guid, ReceiptAttachmentUpload, Task<ReceiptDocument>> upload,
        Func<Guid, Guid, Task<ReceiptAttachmentDownload>> download)
    { saveReceiptDefaults = saveDefaults; uploadReceiptAttachment = upload; downloadReceiptAttachment = download; RefreshReceiptTabState(); }

    private async Task ReceiptTabOperation(Func<Task> operation)
    {
        if (receiptTabsBusy || Binding.IsBusy) return;
        receiptTabsBusy = true; mainLayout.Enabled = false;
        try { await operation(); }
        catch (Exception ex) { lblStatus.Text = "لم تكتمل العملية: " + ex.Message; }
        finally { receiptTabsBusy = false; mainLayout.Enabled = true; RefreshReceiptTabState(); }
    }

    private void ApplyReceiptTabDocument(ReceiptDocument result)
    {
        EnsureReceiptDocumentChoices(result); pendingReceiptDocument = result;
        var doc = ToBindingDocument(result);
        Binding.LoadDocument(doc.Fields, doc.Rows, result.Version, doc.CanEdit);
    }

    private void RefreshReceiptTabState()
    {
        RefreshReceiptBranchState();
        bool idle = !receiptTabsBusy && !Binding.IsBusy;
        receiptUndo.Enabled = idle && Binding.HasPendingChanges;
        receiptUpload.Enabled = idle && !HasUnsavedChanges && receiptDocument?.State == "DRAFT" && uploadReceiptAttachment != null && receiptBootstrap?.Actions.Contains("ACC043.Edit") == true;
        receiptDownload.Enabled = idle && receiptAttachmentList.SelectedItem != null && downloadReceiptAttachment != null;
        RefreshReceiptWorkflowState();
        receiptSaveDefaults.Enabled = idle && saveReceiptDefaults != null && receiptBootstrap?.Actions.Contains("accounting.receipts.configure") == true;
    }

    private void ShowReceiptAttachments(ReceiptDocument document)
    {
        receiptAttachmentList.DataSource = (document.Attachments ?? []).ToList();
        field_attachmentCount.Text = (document.Attachments?.Count ?? 0).ToString();
        RefreshReceiptAccounts(); RefreshReceiptTabState();
    }

    private void RefreshReceiptAccounts()
    {
        documentAccountsGrid.Rows.Clear();
        if (receiptDocument?.JournalId != null)
        {
            receiptAccountsStatus.Text = "أسطر القيد المرحّل — المدين والدائن بالعملة الأساسية";
            foreach (var row in receiptDocument.AccountRows ?? [])
                documentAccountsGrid.Rows.Add(row.Account, "", row.Description, row.Currency, row.ForeignAmount, "", row.Debit, row.Credit);
        }
        else
        {
            receiptAccountsStatus.Text = "تفاصيل السند — لا يوجد قيد مرحّل بعد";
            foreach (DataGridViewRow row in dgvLines.Rows)
            {
                if (row.IsNewRow) continue;
                string Value(string key) => Convert.ToString(row.Cells[key].FormattedValue) ?? "";
                documentAccountsGrid.Rows.Add(Value("counterAccountRef"), Value("analyticalAccount"), Value("lineDescription"), Value("currencyRef"), Value("amount"), Value("exchangeRate"), null, null);
            }
        }
    }

    private void LoadReceiptTabChoices(ReceiptBootstrap bootstrap)
    {
        receiptBootstrap = bootstrap;
        List<BatchFiveChoice> Dimensions(string? code) => (bootstrap.Dimensions ?? []).Where(d => code != null && d.DimensionCode == code)
            .Select(d => new BatchFiveChoice(d.Id.ToString(), d.Label)).ToList();
        Binding.SetChoices("receiptCostCenter", Dimensions(bootstrap.Configuration.CostCenterDimensionCode));
        Binding.SetChoices("receiptProject", Dimensions(bootstrap.Configuration.ProjectDimensionCode));
        Binding.SetChoices("receiptActivity", Dimensions(bootstrap.Configuration.ActivityDimensionCode));
        Binding.SetChoices("sourceCashBankRef", bootstrap.Configuration.Destinations.Select(d => new BatchFiveChoice(d.Id.ToString(), d.Label)));
        Binding.SetChoices("linkedDocument", (bootstrap.Documents ?? []).Select(d => new BatchFiveChoice(d.Kind + ":" + d.Id, d.Label)));
        void DefaultChoices(ComboBox combo, IEnumerable<BatchFiveChoice> choices, Guid? id)
        { combo.DisplayMember = "Label"; combo.ValueMember = "Id"; combo.DataSource = choices.ToArray(); combo.SelectedIndex = -1; if (id.HasValue) combo.SelectedValue = id.Value.ToString(); combo.Enabled = bootstrap.Actions.Contains("accounting.receipts.configure"); }
        DefaultChoices(cboDefaultCurrency, bootstrap.Currencies.Select(c => new BatchFiveChoice(c.Id.ToString(), c.Label)), bootstrap.Configuration.DefaultCurrencyId);
        DefaultChoices(cboDefaultCostCenter, Dimensions(bootstrap.Configuration.CostCenterDimensionCode), bootstrap.Configuration.DefaultCostCenterId);
    }

    private void ApplyReceiptDefaults()
    {
        if (receiptBootstrap?.Configuration.DefaultCurrencyId is { } currency) field_currencyRef.SelectedValue = currency.ToString();
        if (receiptBootstrap?.Configuration.DefaultCostCenterId is { } center) referenceField15.SelectedValue = center.ToString();
        receiptAttachmentList.DataSource = null;
        RefreshReceiptWorkflowState(); Binding.AcceptLocalBaseline(); RefreshReceiptAccounts();
    }
}
