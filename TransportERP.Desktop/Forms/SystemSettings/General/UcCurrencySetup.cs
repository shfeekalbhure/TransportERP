using System.ComponentModel;
using TransportERP.Desktop.CoreUI;

namespace TransportERP.Desktop.Forms.SystemSettings.General;

/// <summary>
/// Designer-backed Currency Setup UserControl. Layout and component ownership are in InitializeComponent.
/// Visual authority: supplied currency screenshots; text authority: Scribd 973153595, printed pp.13–15.
/// </summary>
public partial class UcCurrencySetup : UserControl, IFoundationScreen, IExplicitScreenLayout
{
    private readonly FoundationUiSession foundation;
    private bool updatingCurrencyType;

    public UcCurrencySetup()
    {
        InitializeComponent();
        // No service/database access in the constructor or designer. All original UI keys are retained.
        // Designer audit display editors are outside the business-field snapshot.
        foundation = new FoundationUiSession(goldenShell, bindDisabledActions: false);
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    }

    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public FoundationUiSession Foundation => foundation;
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool HasUnsavedChanges => foundation.HasUnsavedChanges;
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsBusy => foundation.IsBusy;
    public bool ConfirmLeave() => foundation.ConfirmLeave();

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        TransportERP.Desktop.CoreUI.AuditMetadataInstaller.Apply(this, TransportERP.Desktop.CoreUI.AuditMetadataProfile.Standard);
        // PDF pp.6-9: foreign currency exposes header limits and the user-limits tab.
        // Keep all three existing pages available to the Visual Studio designer.
        if (!DesignMode && LicenseManager.UsageMode != LicenseUsageMode.Designtime)
        {
            UpdateCurrencyScrollExtent();
            ApplyCurrencyTypePresentation();
        }
    }

    protected override void OnDpiChangedAfterParent(EventArgs e)
    {
        base.OnDpiChangedAfterParent(e);
        UpdateCurrencyScrollExtent();
    }

    private void UpdateCurrencyScrollExtent()
    {
        // Dock=Fill does not contribute the child's minimum bounds to AutoScroll.
        // Use the already DPI-scaled Designer size, without changing shared root policy.
        AutoScrollMinSize = Size.Empty;
        rootWorkspaceViewport.AutoScrollMinSize = new Size(goldenShell.MinimumSize.Width + rootWorkspaceViewport.Padding.Horizontal,
            goldenShell.MinimumSize.Height + rootWorkspaceViewport.Padding.Vertical);
    }

    private void currencyType_CheckedChanged(object? sender, EventArgs e)
    {
        if (updatingCurrencyType || DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            return;

        updatingCurrencyType = true;
        try
        {
            // Retain the existing CheckBox field keys used by FoundationUiSession.
            // Do not enable foreign-currency editing until its prerequisite is connected.
            if (sender == chkLocal && chkLocal.Checked) chkForeign.Checked = false;
            if (sender == chkForeign && chkForeign.Checked) chkLocal.Checked = false;
            ApplyCurrencyTypePresentation();
        }
        finally { updatingCurrencyType = false; }
    }

    private void currencyOption_CheckedChanged(object? sender, EventArgs e)
    {
        if (updatingCurrencyType) return;
        // RadioButton deselection is followed by selection of its peer. Only commit
        // the selected option, so one gesture produces one change per binding key.
        if (sender == optLocal && optLocal.Checked) chkLocal.Checked = true;
        if (sender == optForeign && optForeign.Checked) chkForeign.Checked = true;
    }

    private void currencyType_EnabledChanged(object? sender, EventArgs e)
    {
        optLocal.Enabled = chkLocal.Enabled;
        optForeign.Enabled = chkForeign.Enabled;
    }

    private void ApplyCurrencyTypePresentation()
    {
        bool foreign = chkForeign.Checked && !chkLocal.Checked;
        // The conversion rate is applicable to foreign currencies; do not mark local-rate input as required.
        txtRate.BackColor = foreign ? Color.FromArgb(255, 255, 225) : Color.White;
        bool wasUpdating = updatingCurrencyType;
        updatingCurrencyType = true;
        try
        {
            optLocal.Checked = chkLocal.Checked;
            optForeign.Checked = foreign;
            currencyType_EnabledChanged(this, EventArgs.Empty);
        }
        finally { updatingCurrencyType = wasUpdating; }
        txtMaximum.Visible = lbltxtMaximum.Visible = foreign;
        txtMinimum.Visible = lbltxtMinimum.Visible = foreign;
        ShowUserLimitsPreview(foreign);
    }

    /// <summary>UI review state only; does not activate the unconnected foreign-currency prerequisite.</summary>
    public void ShowUserLimitsPreview(bool visible)
    {
        var selected = tabsSetup.SelectedTab;
        if (visible && tabgridLimits.Parent != tabsSetup)
        {
            goldenConditionalTabs.TabPages.Remove(tabgridLimits);
            tabsSetup.TabPages.Insert(1, tabgridLimits);
        }
        else if (!visible && tabgridLimits.Parent == tabsSetup)
        {
            tabsSetup.TabPages.Remove(tabgridLimits);
            goldenConditionalTabs.TabPages.Add(tabgridLimits);
        }
        // Inserting at index 1 must not replace a selected denominations/history page.
        // A hidden selected limits page falls back to history, without discarding its data.
        tabsSetup.SelectedTab = selected != null && tabsSetup.TabPages.Contains(selected)
            ? selected : tabgridHistory;
    }

    private void availableCurrencies_Click(object? sender, EventArgs e)
    {
        using var dialog = new FrmAvailableCurrencies();
        dialog.ShowDialog(FindForm());
    }

    private void btnAdd_Click(object? sender, EventArgs e)
    {
        txtNumber.Enabled = true;
        txtNameLocal.Enabled = true;
        txtNameForeign.Enabled = true;
        txtFractionLocal.Enabled = true;
        txtFractionForeign.Enabled = true;
        txtDecimals.Enabled = true;
        chkLocal.Enabled = true;
        chkStock.Enabled = true;
        btnAdd.Enabled = false;
        txtNumber.Focus();
        // Save, lookup data, POS and foreign-currency prerequisites stay unconnected.
    }

    private void emptyGrid_Paint(object? sender, PaintEventArgs e)
    {
        if (sender is not DataGridView grid || grid.Rows.Count != 0 || grid.DataSource is not null) return;
        // Decorative empty ruling only; the grid/columns are real Designer components, with no fake records.
        using var line = new Pen(grid.GridColor);
        using var white = new SolidBrush(grid.DefaultCellStyle.BackColor);
        using var cyan = new SolidBrush(grid.DefaultCellStyle.SelectionBackColor);
        int rowCount = Math.Max(0, (int)Math.Ceiling((grid.ClientSize.Height - grid.ColumnHeadersHeight) / (double)grid.RowTemplate.Height));
        for (int row = 0; row < rowCount; row++)
            foreach (DataGridViewColumn column in grid.Columns)
            {
                var header = grid.GetCellDisplayRectangle(column.Index, -1, true);
                var cell = new Rectangle(header.X, grid.ColumnHeadersHeight + row * grid.RowTemplate.Height,
                    Math.Max(1, header.Width - 1), grid.RowTemplate.Height - 2);
                e.Graphics.FillRectangle(row == 0 ? cyan : white, cell);
                e.Graphics.DrawRectangle(line, cell);
            }
    }
}
