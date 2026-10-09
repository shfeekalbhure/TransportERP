using TransportERP.Desktop.CoreUI;

namespace TransportERP.EmptyForms;

/// <summary>Supplier manual pp92–97. Read-only invoice/item discount views pending service binding.</summary>
public partial class UcPurchaseAdditionalDiscount : UserControl, IExplicitScreenLayout, IWorkspaceChangeState
{
    public UcPurchaseAdditionalDiscount()
    {
        InitializeComponent();
        global::TransportERP.ScreenRootProperties.Attach(this);
        foreach (var entry in commandBar.Commands)
        {
            entry.Value.Visible = entry.Key <= StandardCommand.Close;
            entry.Value.Enabled = entry.Key == StandardCommand.Close;
        }
        commandBar.Commands[StandardCommand.Close].Click += (_, _) => CloseRequested?.Invoke(this, EventArgs.Empty);
        fieldDiscountMode.SelectedIndexChanged += (_, _) => ApplyDiscountView();
        ApplyDiscountView();
        Load += (_, _) => AuditMetadataInstaller.Apply(this);
    }

    // Only the reference-view selector is editable. Service binding must supply real dirty state.
    public bool HasUnsavedChanges => false;
    public event EventHandler? CloseRequested;

    private void ApplyDiscountView()
    {
        bool item = fieldDiscountMode.SelectedIndex == 1;
        foreach (Control control in new Control[] {
            lblInvoiceAmount, fieldInvoiceAmount, lblInvoiceDiscount, fieldInvoiceDiscount,
            lblBurdensAndTax, fieldBurdensAndTax, lblPreviousDiscounts, fieldPreviousDiscounts,
            lblReturnsAmount, fieldReturnsAmount, lblNetPurchases, fieldNetPurchases,
            lblAdditionalPercent, fieldAdditionalPercent, lblAdditionalAmount, fieldAdditionalAmount,
            lblNetAmount, fieldNetAmount }) control.Visible = !item;
        chkAffectsSupplierPrice.Visible = item;
        chkNoSupplierBalanceEffect.Visible = item;
        chkInventorySettled.Visible = item;
        colDiscountQuantity.Visible = item;
        colAdditionalPercent.Visible = item;
        colAdditionalAmount.Visible = item;
        colTotalDiscount.Visible = item;
        lblTotalDiscount.Visible = item;
        txtTotalDiscount.Visible = item;
    }
}

public class FrmPurchaseAdditionalDiscount : Form
{
    public FrmPurchaseAdditionalDiscount()
    {
        var screen = new UcPurchaseAdditionalDiscount { Dock = DockStyle.Fill };
        Text = screen.Text;
        ClientSize = screen.Size;
        MinimumSize = new Size(1060, 660);
        StartPosition = FormStartPosition.CenterParent;
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        screen.CloseRequested += (_, _) => Close();
        Controls.Add(screen);
        global::TransportERP.ScreenRootProperties.Attach(this);
    }
}
