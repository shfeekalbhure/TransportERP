using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;
/// <summary>SCREEN-0083: partial V8 mirror-backed design; local UI editor choices; persistence and provider bindings remain disconnected.</summary>
public partial class UcOnyxSCREEN0083 : UserControl, IFoundationScreen
{
    public UcOnyxSCREEN0083()
    {
        InitializeComponent();
        ScreenProperties.Apply(this);
        SharedScreenProperties.Apply(this);
        UpdateScrollExtent();
        field_T03_E0033.TextChanged += (_, _) => MarkLocalChanges();
        field_T03_E0035.SelectedIndexChanged += (_, _) => MarkLocalChanges();
        field_T03_E0036.CheckStateChanged += (_, _) => MarkLocalChanges();
        field_T03_E0037.CheckStateChanged += (_, _) => MarkLocalChanges();
        field_T03_E0038.CheckStateChanged += (_, _) => MarkLocalChanges();
        field_T03_E0039.SelectedIndexChanged += (_, _) => MarkLocalChanges();
        field_T03_E0040.CheckStateChanged += (_, _) => MarkLocalChanges();
        field_T03_E0041.CheckStateChanged += (_, _) => MarkLocalChanges();
        field_R05_AT_0151.TextChanged += (_, _) => MarkLocalChanges();
        field_R05_AT_0152.TextChanged += (_, _) => MarkLocalChanges();
        fieldsLayout.SizeChanged += (_, _) => UpdateScrollExtent();
        pnlContent.SizeChanged += (_, _) => UpdateScrollExtent();
        DpiChangedAfterParent += (_, _) => UpdateScrollExtent();
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

    private bool updatingExtent;
    private void UpdateScrollExtent()
    {
        if (updatingExtent || IsDisposed) return;
        updatingExtent = true;
        try
        {
            tpMainData.AutoScrollMinSize = new Size(fieldsLayout.MinimumSize.Width, fieldsLayout.Height);
            pnlContent.AutoScrollMinSize = tabMain.MinimumSize;
            lblDataStatus.MaximumSize = new Size(Math.Max(1, mainLayout.ClientSize.Width), 0);
        }
        finally { updatingExtent = false; }
    }

    private bool updatingEditors;
    private void MarkLocalChanges()
    {
        if (updatingEditors) return;

        lblDataStatus.Text = "تعديلات محلية غير محفوظة. الحفظ غير متاح حاليًا.";
    }
    private void ClearForm_Click(object? sender, EventArgs e)
    {
        if (!dgvPermissions.EndEdit()) return;
        if (Foundation.HasUnsavedChanges && MessageBox.Show(this,
            "توجد تعديلات محلية غير محفوظة. هل تريد تفريغ النموذج وفقدان هذه التعديلات؟",
            "تفريغ النموذج", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
        updatingEditors = true;
        try
        {
            field_T03_E0033.Clear();
            field_T03_E0035.SelectedIndex = -1;
            field_T03_E0036.CheckState = CheckState.Indeterminate;
            field_T03_E0037.CheckState = CheckState.Indeterminate;
            field_T03_E0038.CheckState = CheckState.Indeterminate;
            field_T03_E0039.SelectedIndex = -1;
            field_T03_E0040.CheckState = CheckState.Indeterminate;
            field_T03_E0041.CheckState = CheckState.Indeterminate;
            field_R05_AT_0151.Clear();
            field_R05_AT_0152.Clear();
            txtPermissionSearch.Clear();
            dgvPermissions.Rows.Clear();

            lblDataStatus.Text = "تم تفريغ النموذج محليًا. الحفظ غير متاح حاليًا.";
        }
        finally { updatingEditors = false; }
    }

    // Local editor model only: display labels are not permission IDs or API payloads.
    private void AddPermission_Click(object? sender, EventArgs e)
    {
        txtPermissionSearch.Clear();
        int row = dgvPermissions.Rows.Add(null!, string.Empty, string.Empty, string.Empty, CheckState.Indeterminate);
        dgvPermissions.CurrentCell = dgvPermissions.Rows[row].Cells[0];
        MarkLocalChanges();
    }
    private void RemovePermission_Click(object? sender, EventArgs e)
    {
        if (dgvPermissions.CurrentRow is DataGridViewRow row)
        {
            if (MessageBox.Show(this, "هل تريد حذف السطر من المسودة المحلية؟",
                "حذف سطر", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            dgvPermissions.Rows.Remove(row);
            MarkLocalChanges();
        }
    }
    private void PermissionSearchChanged(object? sender, EventArgs e)
    {
        if (!dgvPermissions.EndEdit()) return;
        dgvPermissions.CurrentCell = null;
        string query = txtPermissionSearch.Text.Trim();
        foreach (DataGridViewRow row in dgvPermissions.Rows)
            row.Visible = query.Length == 0 || row.Cells.Cast<DataGridViewCell>()
                .Any(cell => Convert.ToString(cell.Value)?.Contains(query, StringComparison.CurrentCultureIgnoreCase) == true);
    }
    private void PermissionCellDirtyChanged(object? sender, EventArgs e)
    {
        if (dgvPermissions.IsCurrentCellDirty)
            dgvPermissions.CommitEdit(DataGridViewDataErrorContexts.Commit);
    }
    private void PermissionValueChanged(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0) MarkLocalChanges();
    }

    /// <summary>Supplies display choices only; this does not resolve IDs or save records.</summary>
    public void SetLookupChoices(string evidenceId, IEnumerable<string> displayChoices)
    {
        ArgumentNullException.ThrowIfNull(displayChoices);
        var lookup = fieldsLayout.Controls.OfType<ComboBox>()
            .FirstOrDefault(control => string.Equals(control.Tag as string, evidenceId, StringComparison.Ordinal));
        if (lookup is null) throw new ArgumentException("Unknown lookup field.", nameof(evidenceId));
        var choices = displayChoices.Cast<object>().ToArray();
        var selected = lookup.SelectedItem;
        if (selected != null && !choices.Contains(selected))
            throw new ArgumentException("The current selection must remain available.", nameof(displayChoices));
        bool previousUpdating = updatingEditors;
        updatingEditors = true;
        lookup.BeginUpdate();
        try { lookup.Items.Clear(); lookup.Items.AddRange(choices); lookup.SelectedItem = selected; }
        finally { lookup.EndUpdate(); updatingEditors = previousUpdating; }
    }

    public event EventHandler? SaveRequested;
    public void SetSaveAvailable(bool available) => btn_T03_E0043.Enabled = available && SaveRequested is not null;
    private void SaveButton_Click(object? sender, EventArgs e)
    {
        if (!dgvPermissions.EndEdit()) return; if (Foundation.HasCommandBinding) return; if (!ValidateChildren()) return; SaveRequested?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler? CloseRequested;
    private void BtnClose_Click(object? sender, EventArgs e) => CloseRequested?.Invoke(this, EventArgs.Empty);

    private void fieldsLayout_Paint(object sender, PaintEventArgs e)
    {

    }
}
