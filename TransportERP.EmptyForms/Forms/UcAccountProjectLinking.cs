using System;
using System.Drawing;
using System.Windows.Forms;
using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;
public partial class UcAccountProjectLinking : UserControl, IFoundationScreen
{
    public UcAccountProjectLinking()
    {
        InitializeComponent();
        ScreenProperties.Apply(this);
        SharedScreenProperties.Apply(this);
        UpdateContentExtent();
        DpiChangedAfterParent += ContentSizeChanged;
        fieldsLayout.SizeChanged += ContentSizeChanged;
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
            int width = Math.Max(contentLayout.MinimumSize.Width, pnlContent.ClientSize.Width - (pnlContent.VerticalScroll.Visible ? SystemInformation.VerticalScrollBarWidth : 0));
            int height = fieldsLayout.GetPreferredSize(new Size(width, 0)).Height + dgvLinks.MinimumSize.Height + lblGridGap.GetPreferredSize(new Size(width, 0)).Height + 24;
            int viewportHeight = pnlContent.ClientSize.Height
                - (pnlContent.HorizontalScroll.Visible ? SystemInformation.HorizontalScrollBarHeight : 0);
            contentLayout.Size = new Size(width, Math.Max(height, viewportHeight));
            pnlContent.AutoScrollMinSize = new Size(contentLayout.MinimumSize.Width, height);
            lblPreview.MaximumSize = new Size(Math.Max(1, mainLayout.ClientSize.Width - lblPreview.Margin.Horizontal), 0);
        }
        finally { sizing = false; }
    }
    private void LinkCellDirtyStateChanged(object? sender, EventArgs e)
    {
        if (dgvLinks.IsCurrentCellDirty && dgvLinks.CurrentCell is DataGridViewCheckBoxCell)
            dgvLinks.CommitEdit(DataGridViewDataErrorContexts.Commit);
    }
    private void LinkCellValueChanged(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0 && e.ColumnIndex == colLink.Index)
            lblGridGap.Text = "تعديلات الربط المحلية غير محفوظة";
    }
    private EventHandler? saveRequested;
    private bool saveAvailable;
    public event EventHandler? SaveRequested
    {
        add { saveRequested += value; RefreshSaveAvailability(); }
        remove { saveRequested -= value; RefreshSaveAvailability(); }
    }
    public void SetSaveAvailable(bool available)
    {
        saveAvailable = available;
        RefreshSaveAvailability();
    }
    private void RefreshSaveAvailability() => btnSave.Enabled = saveAvailable && saveRequested != null;
    private void BtnSave_Click(object? sender, EventArgs e)
    {
        if (!dgvLinks.EndEdit()) return;
        if (Foundation.HasCommandBinding) return;
        if (!ValidateChildren()) return;
        if (saveAvailable && saveRequested != null && ValidateChildren() && dgvLinks.EndEdit())
            saveRequested.Invoke(this, EventArgs.Empty);
    }
    private EventHandler? editRequested;
    private bool editAvailable;
    public event EventHandler? EditRequested
    {
        add { editRequested += value; RefreshEditAvailability(); }
        remove { editRequested -= value; RefreshEditAvailability(); }
    }
    public void SetEditAvailable(bool available)
    {
        editAvailable = available;
        RefreshEditAvailability();
    }
    private void RefreshEditAvailability() => btnEdit.Enabled = editAvailable && editRequested != null;
    private void BtnEdit_Click(object? sender, EventArgs e)
    {
        if (Foundation.HasCommandBinding) return;
        if (!ValidateChildren()) return;
        if (editAvailable && editRequested != null)
            editRequested.Invoke(this, EventArgs.Empty);
    }
    private EventHandler? loadRequested;
    private bool loadAvailable;
    public event EventHandler? LoadRequested
    {
        add { loadRequested += value; RefreshLoadAvailability(); }
        remove { loadRequested -= value; RefreshLoadAvailability(); }
    }
    public void SetLoadAvailable(bool available)
    {
        loadAvailable = available;
        RefreshLoadAvailability();
    }
    private void RefreshLoadAvailability() => btnLoad.Enabled = loadAvailable && loadRequested != null;
    private void BtnLoad_Click(object? sender, EventArgs e)
    {
        if (Foundation.HasCommandBinding) return;
        if (!ValidateChildren()) return;
        if (loadAvailable && loadRequested != null && Foundation.ConfirmLeave())
            loadRequested.Invoke(this, EventArgs.Empty);
    }
    /// <summary>Replace display choices for a lookup identified by its evidence key; does not persist or infer a selected value.</summary>
    public void SetLookupChoices(string evidenceKey, System.Collections.Generic.IEnumerable<string> displayValues)
    {
        if (displayValues == null) throw new ArgumentNullException(nameof(displayValues));
        ComboBox lookup = evidenceKey switch
        {
            "T03-E0183" => fieldT03E0183,
            "R05-AT-0221" => fieldR05AT0221,
            "R05-AT-0222" => fieldR05AT0222,
            "R05-AT-0223" => fieldR05AT0223,
            "R05-AT-0224" => fieldR05AT0224,
            _ => throw new ArgumentException("Unknown lookup evidence key.", nameof(evidenceKey))
        };
        var values = new System.Collections.Generic.List<object>();
        foreach (string value in displayValues)
        {
            if (value == null) throw new ArgumentException("Lookup display values cannot contain null.", nameof(displayValues));
            values.Add(value);
        }
        var selected = lookup.SelectedItem;
        if (selected != null && !values.Contains(selected))
            throw new ArgumentException("The current selection must remain available.", nameof(displayValues));
        lookup.BeginUpdate();
        try
        {
            lookup.Items.Clear();
            lookup.Items.AddRange(values.ToArray());
            lookup.SelectedItem = selected;
        }
        finally { lookup.EndUpdate(); }
    }
    public event EventHandler? CloseRequested;
    internal Button CloseButton => btnClose;
    private void BtnClose_Click(object? sender, EventArgs e) => CloseRequested?.Invoke(this, EventArgs.Empty);
}
