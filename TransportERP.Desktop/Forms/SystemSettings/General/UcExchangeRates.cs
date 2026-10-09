using System;
using System.Drawing;
using System.Windows.Forms;

namespace TransportERP.Desktop.Forms.SystemSettings.General;

/// <summary>GEN-009 design surface. Server operations and record binding remain disconnected.</summary>
public partial class UcExchangeRates : UserControl, TransportERP.Desktop.CoreUI.IFoundationScreen, TransportERP.Desktop.CoreUI.IExplicitScreenLayout
{
    public TransportERP.Desktop.CoreUI.FoundationUiSession Foundation { get; }
    public bool HasUnsavedChanges => Foundation.HasUnsavedChanges;
    public bool IsBusy => Foundation.IsBusy;
    public bool ConfirmLeave() => Foundation.ConfirmLeave();

    private bool updatingContentExtent;

    public UcExchangeRates()
    {
        InitializeComponent();
        TransportERP.Desktop.CoreUI.ScreenProperties.Apply(this);
        OnyxPhaseOneProperties.Apply(this);
        // DateTimePicker has no timezone-kind editor. Seed UTC clock values to
        // match the visible UTC suffix; persistence/binding is still disconnected.
        var utcNow = DateTime.UtcNow;
        dtpRateDate.Value = utcNow;
        dtpEffectiveTo.Value = utcNow;
        dtpFilterEffectiveAt.Value = utcNow;
        dtpEffectiveTo.Checked = false;
        dtpFilterEffectiveAt.Checked = false;
        UpdateContentExtent();
        Foundation = new TransportERP.Desktop.CoreUI.FoundationUiSession(this);
        Foundation.ValidateAction = action =>
        {
            if (action == "btnDisable" && string.IsNullOrWhiteSpace(txtDisableReason.Text)) return "أدخل سبب الإيقاف.";
            if (action != "btnSave") return null;
            if (cmbFromCurrency.SelectedItem != null && Foundation.CaptureFields()[cmbFromCurrency.Name]?.ToString() == Foundation.CaptureFields()[cmbToCurrency.Name]?.ToString()) return "اختر عملتين مختلفتين.";
            if (nudExchangeRate.Value <= 0) return "سعر الصرف يجب أن يكون أكبر من صفر.";
            if (dtpEffectiveTo.Checked && dtpEffectiveTo.Value < dtpRateDate.Value) return "نهاية السريان تسبق بدايته.";
            if (chkMinimumRate.Checked && chkMaximumRate.Checked && nudMinimumRate.Value > nudMaximumRate.Value) return "الحد الأدنى أكبر من الحد الأعلى.";
            if (chkMinimumRate.Checked && nudExchangeRate.Value < nudMinimumRate.Value) return "السعر أقل من الحد الأدنى.";
            if (chkMaximumRate.Checked && nudExchangeRate.Value > nudMaximumRate.Value) return "السعر أكبر من الحد الأعلى.";
            return null;
        };
        Foundation.RequireFields(new[] { "btnSave", "btnValidate", "btnPublish" }, cmbCompany.Name, cmbFromCurrency.Name, cmbToCurrency.Name, cmbRateType.Name, dtpRateDate.Name, nudExchangeRate.Name);
        ApplyKnownRequirementColors();
        Load += (_, _) => ApplyKnownRequirementColors();
        Foundation.DocumentLoaded += (_, _) => ApplyKnownRequirementColors();
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}

    // Optional values are absent while unchecked. This enables their editor only;
    // it performs no rate calculation, validation, persistence or server operation.
    private void OptionalBounds_CheckedChanged(object? sender, EventArgs e)
    {
        nudMinimumRate.Enabled = chkMinimumRate.Checked;
        nudMaximumRate.Enabled = chkMaximumRate.Checked;
    }

    private void ApplyKnownRequirementColors()
    {
        foreach (Control editor in new Control[] { cmbCompany, cmbFromCurrency, cmbToCurrency, cmbRateType, dtpRateDate, nudExchangeRate })
            global::TransportERP.RequiredFieldAppearance.Apply(editor, true);
        // Bounds and end date can be omitted under the existing validation.
        foreach (Control editor in new Control[] { nudMinimumRate, nudMaximumRate, dtpEffectiveTo })
            global::TransportERP.RequiredFieldAppearance.Apply(editor, false);
    }

    private void ContentLayout_SizeChanged(object? sender, EventArgs e) => UpdateContentExtent();

    private void UpdateContentExtent()
    {
        if (updatingContentExtent || IsDisposed || contentLayout is null || grpSearch is null) return;
        updatingContentExtent = true;
        try
        {
            int width = Math.Max(1, contentLayout.ClientSize.Width - contentLayout.Padding.Horizontal);
            lblRateConvention.MaximumSize = new Size(width, 0);
            lblPreview.MaximumSize = new Size(Math.Max(1, mainLayout.ClientSize.Width), 0);
            int requiredHeight = grpMainData.GetPreferredSize(new Size(width, 0)).Height
                + grpMainData.Margin.Vertical
                + grpSearch.GetPreferredSize(new Size(width, 0)).Height + grpSearch.Margin.Vertical
                + dgvRates.MinimumSize.Height + dgvRates.Margin.Vertical
                + lblRateConvention.GetPreferredSize(new Size(width, 0)).Height + lblRateConvention.Margin.Vertical;
            var extent = new Size(0, requiredHeight);
            if (contentLayout.AutoScrollMinSize != extent) contentLayout.AutoScrollMinSize = extent;
        }
        finally { updatingContentExtent = false; }
    }

    public event EventHandler? CloseRequested;
    internal Button CloseButton => btnClose;
    private void BtnClose_Click(object? sender, EventArgs e) => CloseRequested?.Invoke(this, EventArgs.Empty);
}
