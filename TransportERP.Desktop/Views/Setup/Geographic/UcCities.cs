namespace TransportERP.Desktop.Views.Setup.Geographic;

/// <summary>
/// GEN-006 — المدن.
/// شاشة عمل داخل الـ Main Shell وتُصمم عبر Windows Forms Designer/Toolbox.
/// </summary>
public partial class UcCities : UserControl
{
    public UcCities()
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
        cmbCountry.SelectedIndex = -1;
        cmbGovernorate.SelectedIndex = -1;
        cmbDirectorate.SelectedIndex = -1;
        txtCityCode.Clear();
        txtNameAr.Clear();
        txtNameEn.Clear();
        txtNotes.Clear();
        cmbStatus.SelectedIndex = 0;
        cmbCountry.Focus();
    }

    private void txtSearch_TextChanged(object? sender, EventArgs e)
    {
        // البحث الحقيقي سيتم ربطه لاحقًا عبر API.
    }
}
