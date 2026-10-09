namespace TransportERP.EmptyForms;

public sealed partial class BatchFiveUiSession
{
    public void ChooseJournalImport(TextBox path, DataGridView preview)
    {
        if (!editable || busy || grid == null) return;
        using var dialog = new OpenFileDialog { Filter = "Excel (*.xlsx)|*.xlsx|CSV UTF-8 (*.csv)|*.csv", CheckFileExists = true, Multiselect = false };
        if (dialog.ShowDialog(owner) == DialogResult.OK) ReadJournalImport(dialog.FileName, path, preview);
    }
    public void ReadJournalImport(string fileName, TextBox path, DataGridView preview)
    {
        if (!editable || busy || grid == null) return;
        if (string.Equals(Path.GetExtension(fileName), ".csv", StringComparison.OrdinalIgnoreCase)) { ReadCsv(fileName, path, preview); return; }
        importPreviews[preview] = path;
        try
        {
            var input = JournalXlsxReader.Read(fileName);
            // Preserve the existing request schema. These manual columns currently have no
            // editable/persisted binding: reject supplied data instead of silently dropping it.
            string?[] keys = ["accountRef", null, "lineDescription", "debit", null, "credit", null, "currencyRef", "costCenterRef", null, null, null, null, null];
            string[] labels = ["رقم الحساب", "الحساب التحليلي", "البيان", "مدين محلي", "مدين أجنبي", "دائن محلي", "دائن أجنبي", "العملة", "مركز التكلفة", "رقم المشروع", "رقم النشاط", "المرجع", "الفرع المستفيد", "رقم المستفيد"];
            var headers = grid.Columns.Cast<DataGridViewColumn>().Where(c => editableColumns.Contains(c.Name)).Select(c => c.Name).ToArray();
            var output = new List<object[]>();
            foreach (var row in input)
            {
                var mapped = headers.ToDictionary(k => k, _ => "", StringComparer.Ordinal);
                for (int c = 0; c < keys.Length; c++)
                {
                    if (keys[c] is string key && mapped.ContainsKey(key)) mapped[key] = row[c];
                    else if (!string.IsNullOrWhiteSpace(row[c])) throw new InvalidOperationException("العمود «" + labels[c] + "» غير مربوط بالحمولة الحالية؛ لم يتم استيراد أي سطر");
                }
                output.Add(headers.Select(h => (object)mapped[h]).ToArray());
            }
            preview.Rows.Clear(); preview.Columns.Clear();
            foreach (string header in headers) preview.Columns.Add(header, grid.Columns[header]!.HeaderText);
            foreach (var row in output) preview.Rows.Add(row);
            path.Text = fileName; preview.Tag = headers; Changed();
            status.Text = "معاينة XLSX فقط؛ الصف الأول متجاهل. لم تُضف أو تُحفظ بيانات. رموز الحسابات والقوائم يجب أن تكون معرّفات مرتبطة.";
        }
        catch (Exception e) when (e is IOException or InvalidDataException or UnauthorizedAccessException or InvalidOperationException or System.Xml.XmlException or ArgumentException or FormatException or OverflowException)
        { preview.Rows.Clear(); preview.Tag = null; path.Clear(); status.Text = "تعذر قراءة Excel: " + e.Message; }
    }
    public void ImportJournalPreview(DataGridView preview) => ImportJournalPreview(preview, () => MessageBox.Show(owner, "إضافة أسطر المعاينة إلى المسودة المحلية؟", "استيراد محلي", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes);
    public void ImportJournalPreview(DataGridView preview, Func<bool> confirm)
    {
        if (!editable || busy || grid == null || preview.Tag is not string[] headers) return;
        foreach (DataGridViewRow row in preview.Rows)
            foreach (string key in headers)
                if (Equals(grid.Columns[key]!.HeaderCell.Tag, "required") && string.IsNullOrWhiteSpace(Convert.ToString(row.Cells[key].Value)))
                { status.Text = $"السطر {row.Index + 1}: {grid.Columns[key]!.HeaderText} مطلوب؛ لم تُضف أي بيانات"; return; }
        ImportPreview(preview, confirm);
    }
}
