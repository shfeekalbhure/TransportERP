using System;
using System.Drawing;
using System.Windows.Forms;
using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;
public partial class UcAccountOpeningRequest : UserControl, IFoundationScreen
{
    public UcAccountOpeningRequest()
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
            int height = fieldsLayout.GetPreferredSize(new Size(width, 0)).Height + 24;
            int viewportHeight = pnlContent.ClientSize.Height
                - (pnlContent.HorizontalScroll.Visible ? SystemInformation.HorizontalScrollBarHeight : 0);
            contentLayout.Size = new Size(width, Math.Max(height, viewportHeight));
            pnlContent.AutoScrollMinSize = new Size(contentLayout.MinimumSize.Width, height);
            lblPreview.MaximumSize = new Size(Math.Max(1, mainLayout.ClientSize.Width - lblPreview.Margin.Horizontal), 0);
        }
        finally { sizing = false; }
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
        if (Foundation.HasCommandBinding) return;
        if (!ValidateChildren()) return;
        if (saveAvailable && saveRequested != null && ValidateChildren())
            saveRequested.Invoke(this, EventArgs.Empty);
    }
    /// <summary>Replace display choices for a lookup identified by its evidence key; does not persist or infer a selected value.</summary>
    public void SetLookupChoices(string evidenceKey, System.Collections.Generic.IEnumerable<string> displayValues)
    {
        if (displayValues == null) throw new ArgumentNullException(nameof(displayValues));
        ComboBox lookup = evidenceKey switch
        {
            "T03-E0066" => fieldT03E0066,
            "T03-E0067" => fieldT03E0067,
            "T03-E0070" => fieldT03E0070,
            "T03-E0071" => fieldT03E0071,
            "T03-E0073" => fieldT03E0073,
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
