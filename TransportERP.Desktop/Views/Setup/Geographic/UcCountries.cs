namespace TransportERP.Desktop.Views.Setup.Geographic;

/// <summary>
/// GEN-003 — الدول.
/// شاشة عمل داخل الـ Main Shell وتُصمم عبر Windows Forms Designer/Toolbox.
/// </summary>
public partial class UcCountries : UserControl
{
    public UcCountries()
    {
        InitializeComponent();
        ConfigureRuntimeDefaults();
    }

    private void ConfigureRuntimeDefaults()
    {
        RightToLeft = RightToLeft.Yes;
        cmbStatus.SelectedIndex = 0;
        cmbStatusFilter.SelectedIndex = 0;
        lblCreatedValue.Text = "—";
        lblModifiedValue.Text = "—";
        lblEditCountValue.Text = "0";
        lblPrintCountValue.Text = "0";
    }

    private void btnNew_Click(object? sender, EventArgs e)
    {
        txtCountryCode.Clear();
        txtNameAr.Clear();
        txtNameEn.Clear();
        txtIso2.Clear();
        txtIso3.Clear();
        txtDialCode.Clear();
        txtCurrencyCode.Clear();
        txtNotes.Clear();
        cmbStatus.SelectedIndex = 0;
        txtCountryCode.Focus();
    }

    private void txtSearch_TextChanged(object? sender, EventArgs e)
    {
        // البحث الحقيقي سيتم ربطه لاحقًا عبر API.
    }
}
