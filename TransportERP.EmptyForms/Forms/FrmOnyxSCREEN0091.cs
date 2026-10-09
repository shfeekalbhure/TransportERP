using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;
/// <summary>تهيئة الحدود: UI with external binding hooks; no persistence or financial computation.</summary>
public partial class UcOnyxSCREEN0091 : UserControl, IFoundationScreen
{
    public UcOnyxSCREEN0091()
    {
        InitializeComponent();
        ScreenProperties.Apply(this);
        SharedScreenProperties.Apply(this);
        UpdateContentExtent();
        DpiChangedAfterParent += (_, _) => UpdateContentExtent();
        Foundation = new FoundationUiSession(this);
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}
    public FoundationUiSession Foundation { get; }
    public bool HasUnsavedChanges => Foundation.HasUnsavedChanges;
    public bool IsBusy => Foundation.IsBusy;
    public bool ConfirmLeave() => Foundation.ConfirmLeave();

    private bool sizing;
    private void ContentSizeChanged(object? sender, EventArgs e) => UpdateContentExtent();
    private void UpdateContentExtent()
    {
        if (sizing || IsDisposed || contentLayout == null) return;
        sizing = true;
        try
        {
            int width = Math.Max(contentLayout.MinimumSize.Width,
                pnlContent.ClientSize.Width - pnlContent.Padding.Horizontal - SystemInformation.VerticalScrollBarWidth);
            int height = (fieldsLayout.Controls.Count > 0 ? fieldsLayout.GetPreferredSize(new Size(width, 0)).Height : 0)
                + fieldsLayout.Margin.Vertical + dgvRecords.MinimumSize.Height + dgvRecords.Margin.Vertical;
            contentLayout.Size = new Size(width, Math.Max(height, pnlContent.ClientSize.Height - pnlContent.Padding.Vertical));
            lblDataStatus.MaximumSize = new Size(Math.Max(1, mainLayout.ClientSize.Width - lblDataStatus.Margin.Horizontal), 0);
        }
        finally { sizing = false; }
    }
    // The adapter supplies all data, member names and command availability.
    // Evidence keys identify UI controls only; they are not DTO properties.
    public event EventHandler? EditorChanged;
    private bool bindingData;
    private void FieldValueChanged(object? sender, EventArgs e)
    {
        if (!bindingData) EditorChanged?.Invoke(this, EventArgs.Empty);
    }
    private void GridValueChanged(object? sender, DataGridViewCellEventArgs e)
    {
        if (!bindingData && e.RowIndex >= 0) EditorChanged?.Invoke(this, EventArgs.Empty);
    }
    private void GridDirtyStateChanged(object? sender, EventArgs e)
    {
        if (dgvRecords.IsCurrentCellDirty && dgvRecords.CurrentCell is DataGridViewCheckBoxCell)
            dgvRecords.CommitEdit(DataGridViewDataErrorContexts.Commit);
    }
    private void GridDataError(object? sender, DataGridViewDataErrorEventArgs e)
    {
        e.ThrowException = false;
        e.Cancel = true;
        if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            dgvRecords.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "تعذر عرض أو قبول القيمة؛ راجع ربط البيانات ونوعها.";
    }
    public bool ValidateEditor() => ValidateChildren() && dgvRecords.EndEdit();
    public void BindRows(object? source)
    {
        bindingData = true;
        try { dgvRecords.DataSource = source; }
        finally { bindingData = false; }
    }
    public void BindColumn(string evidenceKey, string dataPropertyName)
    {
        foreach (DataGridViewColumn column in dgvRecords.Columns)
            if (string.Equals(column.Tag as string, evidenceKey, StringComparison.Ordinal))
            { column.DataPropertyName = dataPropertyName; return; }
        throw new ArgumentException("Unknown column evidence key.", nameof(evidenceKey));
    }
    public void BindLookup(string evidenceKey, object? source, string displayMember, string valueMember)
    {
        bindingData = true;
        try
        {
            foreach (Control control in fieldsLayout.Controls)
                if (control is ComboBox lookup && string.Equals(lookup.Tag as string, evidenceKey, StringComparison.Ordinal))
                { lookup.DisplayMember = displayMember; lookup.ValueMember = valueMember; lookup.DataSource = source; lookup.SelectedIndex = -1; return; }
            foreach (DataGridViewColumn column in dgvRecords.Columns)
                if (column is DataGridViewComboBoxColumn lookupColumn && string.Equals(column.Tag as string, evidenceKey, StringComparison.Ordinal))
                { lookupColumn.DisplayMember = displayMember; lookupColumn.ValueMember = valueMember; lookupColumn.DataSource = source; return; }
            throw new ArgumentException("Unknown lookup evidence key.", nameof(evidenceKey));
        }
        finally { bindingData = false; }
    }
    public object? GetFieldValue(string evidenceKey)
    {
        foreach (Control control in fieldsLayout.Controls)
            if (string.Equals(control.Tag as string, evidenceKey, StringComparison.Ordinal))
                return control is ComboBox lookup ? lookup.SelectedValue ?? lookup.SelectedItem : control.Text;
        throw new ArgumentException("Unknown field evidence key.", nameof(evidenceKey));
    }
    public void SetFieldValue(string evidenceKey, object? value)
    {
        bindingData = true;
        try
        {
            foreach (Control control in fieldsLayout.Controls)
                if (string.Equals(control.Tag as string, evidenceKey, StringComparison.Ordinal))
                {
                    if (control is ComboBox lookup) { if (value == null) lookup.SelectedIndex = -1; else lookup.SelectedValue = value; }
                    else control.Text = Convert.ToString(value, System.Globalization.CultureInfo.CurrentCulture) ?? string.Empty;
                    return;
                }
            throw new ArgumentException("Unknown field evidence key.", nameof(evidenceKey));
        }
        finally { bindingData = false; }
    }
    // Lexical decimal validation: no numeric conversion, rounding, scale or range cap.
    private static bool IsDecimalText(string? input)
    {
        string value = (input ?? string.Empty).Trim();
        if (value.Length == 0) return true;
        string separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        return System.Text.RegularExpressions.Regex.IsMatch(value,
            @"^[+-]?(?:[0-9]+(?:" + System.Text.RegularExpressions.Regex.Escape(separator) + @"[0-9]+)?|"
            + System.Text.RegularExpressions.Regex.Escape(separator) + @"[0-9]+)$");
    }
    private void DecimalFieldValidating(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        if (sender is TextBox field)
        {
            e.Cancel = !IsDecimalText(field.Text);
            lblDataStatus.Text = !e.Cancel
                ? "الواجهة جاهزة للربط — لا يتم تنفيذ حفظ أو حسابات مالية."
                : "أدخل رقماً عشرياً بالفاصل المحلي دون فواصل تجميع؛ يمكن ترك القيمة فارغة.";
        }
    }
    private void DecimalCellValidating(object? sender, DataGridViewCellValidatingEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
        string? key = dgvRecords.Columns[e.ColumnIndex].Tag as string;
        if (key is not ("R05-AT-0203" or "R05-AT-0204")) return;
        bool valid = IsDecimalText(Convert.ToString(e.FormattedValue, System.Globalization.CultureInfo.CurrentCulture));
        dgvRecords.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = valid ? string.Empty : "قيمة عشرية غير صحيحة؛ استخدم الفاصل المحلي دون فواصل تجميع.";
        e.Cancel = !valid;
    }
    public event EventHandler? LoadRequested;
    public void SetLoadAvailable(bool available) => btnT03_E0147.Enabled = available && LoadRequested != null;
    private void LoadButtonClick(object? sender, EventArgs e) {
        if (Foundation.HasCommandBinding) return;
        if (!ValidateChildren()) return; if (Foundation.ConfirmLeave()) LoadRequested?.Invoke(this, EventArgs.Empty); }
    public event EventHandler? UpdateRequested;
    public void SetUpdateAvailable(bool available) => btnT03_E0148.Enabled = available && UpdateRequested != null;
    private void UpdateButtonClick(object? sender, EventArgs e)
    {
        if (Foundation.HasCommandBinding) return;
        if (!ValidateChildren()) return;
        if (ValidateEditor()) UpdateRequested?.Invoke(this, EventArgs.Empty);
    }
    public event EventHandler? SaveRequested;
    public void SetSaveAvailable(bool available) => btnT03_E0155.Enabled = available && SaveRequested != null;
    private void SaveButtonClick(object? sender, EventArgs e)
    {
        if (!dgvRecords.EndEdit()) return;
        if (Foundation.HasCommandBinding) return;
        if (!ValidateChildren()) return;
        if (ValidateEditor()) SaveRequested?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler? CloseRequested;
    private void BtnClose_Click(object? sender, EventArgs e) => CloseRequested?.Invoke(this, EventArgs.Empty);
}
