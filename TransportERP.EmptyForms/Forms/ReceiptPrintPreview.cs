using System.Text;

namespace TransportERP.EmptyForms;

public partial class UcScreen_04_04_01
{
    private string receiptCompanyName = "";
    public void SetReceiptOrganization(string companyName) => receiptCompanyName = companyName;
    /// <summary>Captures the currently displayed receipt without saving or posting it.</summary>
    public string CreateReceiptPrintHtml()
    {
        if (!dgvLines.EndEdit()) throw new InvalidOperationException("أكمل تصحيح قيمة السطر قبل المعاينة.");
        RefreshReceiptTotals();
        var header = new List<ReceiptPrintTemplate.Item>();
        if (!string.IsNullOrWhiteSpace(receiptCompanyName)) header.Add(new("الشركة", receiptCompanyName));
        void Add(string label, Control control) => header.Add(new(label, control.Text));
        Add("الفرع", textBox1);
        Add("رقم السند", field_voucherNumber);
        header.Add(new("التاريخ", field_voucherDate.Value.ToString("yyyy-MM-dd")));
        Add("الصندوق / البنك", field_destinationCashBankRef);
        Add("العملة", field_currencyRef);
        Add("المبلغ", field_amount);
        Add("سعر الصرف", field_exchangeRate);
        Add("المستلم", field_partyRef);
        Add("نوع السند", field_voucherType);
        Add("المندوب", field_salesperson);
        Add("المحصل", field_collector);
        Add("رقم المرجع", field_referenceNumber);
        Add("اسم المرجع", field_referenceName);
        Add("البيان", field_description);
        Add("مركز التكلفة", referenceField15); Add("المشروع", referenceField16); Add("النشاط", referenceField17);
        Add("المستند المرتبط", receiptLinkedDocument);
        var rows = dgvLines.Rows.Cast<DataGridViewRow>().Where(r => !r.IsNewRow &&
            r.Cells.Cast<DataGridViewCell>().Any(c => c.OwningColumn.Name != "rowNo" && !string.IsNullOrWhiteSpace(Convert.ToString(c.Value)))).ToArray();
        // Split wide details into indexed tables so every column remains legible on paper.
        var columns = dgvLines.Columns.Cast<DataGridViewColumn>().Where(c => c.Visible && c.Name != "rowNo")
            .OrderBy(c => c.DisplayIndex).ToArray();
        var tables = new List<ReceiptPrintTemplate.Table>();
        foreach (var group in columns.Chunk(6))
        {
            var headings = new[] { "م" }.Concat(group.Select(c => c.HeaderText)).ToArray();
            var values = rows.Select((r, index) => (IReadOnlyList<string>)new[] { (index + 1).ToString() }
                .Concat(group.Select(c => Convert.ToString(r.Cells[c.Index].FormattedValue) ?? "")).ToArray()).ToArray();
            tables.Add(new("تفاصيل السند" + (tables.Count == 0 ? "" : " — تابع"), headings, values));
        }
        var state = field_state.Text switch { "DRAFT" => "مسودة محفوظة", "APPROVED" => "معتمد", "POSTED" => "مرحّل", "CANCELLED" => "ملغى", "REVERSED" => "معكوس", _ => field_state.Text };
        return ReceiptPrintTemplate.Render(field_operation.Text, state,
            HasUnsavedChanges || string.IsNullOrWhiteSpace(Binding.ExpectedVersion) || string.IsNullOrWhiteSpace(field_state.Text), header, tables,
            field_total.Text, field_difference.Text, lblTotalsMessage.Text);
    }

    private void ShowReceiptPrintPreview()
    {
        try
        {
            string html = CreateReceiptPrintHtml();
            using var preview = new Form { Text = "معاينة سند القبض", Width = 1100, Height = 780,
                StartPosition = FormStartPosition.CenterParent, RightToLeft = RightToLeft.Yes };
            var browser = new WebBrowser { Dock = DockStyle.Fill, AllowWebBrowserDrop = false,
                IsWebBrowserContextMenuEnabled = false, WebBrowserShortcutsEnabled = false };
            var actions = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 42, FlowDirection = FlowDirection.RightToLeft };
            var print = new Button { Text = "معاينة الطباعة", AutoSize = true, Enabled = false };
            var export = new Button { Text = "حفظ نسخة HTML", AutoSize = true };
            print.Click += (_, _) => browser.ShowPrintPreviewDialog();
            export.Click += (_, _) =>
            {
                using var dialog = new SaveFileDialog { Filter = "HTML (*.html)|*.html", FileName = "receipt.html", OverwritePrompt = true };
                if (dialog.ShowDialog(preview) == DialogResult.OK) File.WriteAllText(dialog.FileName, html, new UTF8Encoding(true));
            };
            browser.DocumentCompleted += (_, _) => print.Enabled = true;
            actions.Controls.AddRange(new Control[] { print, export });
            preview.Controls.Add(browser); preview.Controls.Add(actions);
            browser.DocumentText = html;
            preview.ShowDialog(this);
        }
        catch (Exception ex) when (ex is InvalidOperationException or System.Runtime.InteropServices.COMException or IOException)
        { MessageBox.Show(this, "تعذرت معاينة السند: " + ex.Message, "معاينة الطباعة", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
    }
}
