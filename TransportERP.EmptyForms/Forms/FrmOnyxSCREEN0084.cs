using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;
/// <summary>SCREEN-0084: partial V8 mirror-backed design; local UI editor choices; persistence and provider bindings remain disconnected.</summary>
public partial class UcOnyxSCREEN0084 : UserControl, IFoundationScreen
{
    public UcOnyxSCREEN0084()
    {
        InitializeComponent();
        ScreenProperties.Apply(this);
        SharedScreenProperties.Apply(this);
        UpdateScrollExtent();
        field_T03_E0048.SelectedIndexChanged += (_, _) => MarkLocalChanges();
        field_T03_E0049.TextChanged += (_, _) => MarkLocalChanges();
        field_T03_E0051.CheckStateChanged += (_, _) => MarkLocalChanges();
        field_T03_E0052.CheckStateChanged += (_, _) => MarkLocalChanges();
        field_T03_E0053.CheckStateChanged += (_, _) => MarkLocalChanges();
        field_T03_E0054.CheckStateChanged += (_, _) => MarkLocalChanges();
        field_R05_AT_0153.TextChanged += (_, _) => MarkLocalChanges();
        field_R05_AT_0154.TextChanged += (_, _) => MarkLocalChanges();
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
        field_T03_E0048.SelectedIndex = -1;
        field_T03_E0049.Clear();
        field_T03_E0051.CheckState = CheckState.Indeterminate;
        field_T03_E0052.CheckState = CheckState.Indeterminate;
        field_T03_E0053.CheckState = CheckState.Indeterminate;
        field_T03_E0054.CheckState = CheckState.Indeterminate;
        field_R05_AT_0153.Clear();
        field_R05_AT_0154.Clear();

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
    public void SetSaveAvailable(bool available) => btn_T03_E0056.Enabled = available && SaveRequested is not null;
    private void SaveButton_Click(object? sender, EventArgs e) { if (Foundation.HasCommandBinding) return; if (!ValidateChildren()) return; SaveRequested?.Invoke(this, EventArgs.Empty); }
    public event EventHandler? ViewRequested;
    public void SetViewAvailable(bool available) => btn_T03_E0055.Enabled = available && ViewRequested is not null;
    private void ViewButton_Click(object? sender, EventArgs e) {
        if (Foundation.HasCommandBinding) return;
        if (!ValidateChildren()) return; if (Foundation.ConfirmLeave()) ViewRequested?.Invoke(this, EventArgs.Empty); }

    public event EventHandler? CloseRequested;
    private void BtnClose_Click(object? sender, EventArgs e) => CloseRequested?.Invoke(this, EventArgs.Empty);
}
