using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;
/// <summary>SCREEN-0085: partial V8 mirror-backed design; local UI editor choices; persistence and provider bindings remain disconnected.</summary>
public partial class UcOnyxSCREEN0085 : UserControl, IFoundationScreen
{
    public UcOnyxSCREEN0085()
    {
        InitializeComponent();
        ScreenProperties.Apply(this);
        SharedScreenProperties.Apply(this);
        UpdateScrollExtent();
        field_T03_E0059.TextChanged += (_, _) => MarkLocalChanges();
        field_T03_E0061.SelectedIndexChanged += (_, _) => MarkLocalChanges();
        field_R05_AT_0155.TextChanged += (_, _) => MarkLocalChanges();
        field_R05_AT_0156.TextChanged += (_, _) => MarkLocalChanges();
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
            pnlContent.AutoScrollMinSize = new Size(fieldsLayout.MinimumSize.Width, fieldsLayout.Height);
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
        if (Foundation.HasUnsavedChanges && MessageBox.Show(this,
            "توجد تعديلات محلية غير محفوظة. هل تريد تفريغ النموذج وفقدان هذه التعديلات؟",
            "تفريغ النموذج", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
        updatingEditors = true;
        try
        {
        field_T03_E0059.Clear();
        field_T03_E0061.SelectedIndex = -1;
        field_R05_AT_0155.Clear();
        field_R05_AT_0156.Clear();

        lblDataStatus.Text = "تم تفريغ النموذج محليًا. الحفظ غير متاح حاليًا.";
        }
        finally { updatingEditors = false; }
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
    public void SetSaveAvailable(bool available) => btn_T03_E0063.Enabled = available && SaveRequested is not null;
    private void SaveButton_Click(object? sender, EventArgs e) { if (Foundation.HasCommandBinding) return; if (!ValidateChildren()) return; SaveRequested?.Invoke(this, EventArgs.Empty); }
    public event EventHandler? LookupRequested;
    public void SetLookupAvailable(bool available) => btnAccountLookup.Enabled = available && LookupRequested is not null;
    private void LookupButton_Click(object? sender, EventArgs e) { if (Foundation.HasCommandBinding) return; if (!ValidateChildren()) return; LookupRequested?.Invoke(this, EventArgs.Empty); }
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == Keys.F9 && btnAccountLookup.Enabled && LookupRequested is not null)
        {
            btnAccountLookup.PerformClick();
            return true;
        }
        return base.ProcessCmdKey(ref msg, keyData);
    }

    public event EventHandler? CloseRequested;
    private void BtnClose_Click(object? sender, EventArgs e) => CloseRequested?.Invoke(this, EventArgs.Empty);
}
