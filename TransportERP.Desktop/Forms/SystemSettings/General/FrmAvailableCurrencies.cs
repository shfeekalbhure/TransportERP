namespace TransportERP.Desktop.Forms.SystemSettings.General;

/// <summary>Empty reference lookup until a validated currency source is connected.</summary>
public partial class FrmAvailableCurrencies : Form
{
    public FrmAvailableCurrencies()
    {
        InitializeComponent();
        global::TransportERP.ScreenRootProperties.Attach(this);
    }

    private void find_Click(object? sender, EventArgs e)
    {
        // A local preview has no records to query or select. Do not invent currencies.
        lblStatus.Text = "قائمة العملات غير مرتبطة بمصدر بيانات بعد.";
        txtSearch.Focus();
    }
}
