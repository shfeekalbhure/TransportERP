namespace TransportERP.Desktop.Views.Setup.Geographic;

/// <summary>
/// GEN-005 — المديريات.
/// شاشة عمل داخل الـ Main Shell وتُصمم عبر Windows Forms Designer/Toolbox.
/// </summary>
public partial class UcDirectorates : UserControl
{
    public UcDirectorates()
    {
        InitializeComponent();
        ConfigureRuntimeDefaults();
    }

    private void ConfigureRuntimeDefaults()
    {
        RightToLeft = RightToLeft.Yes;
        cmbStatus.SelectedIndex = 0;
        lblCreatedValue.Text = "—";
        lblModifiedValue.Text = "—";
        lblEditCountValue.Text = "0";
        lblPrintCountValue.Text = "0";
    }

    private void btnNew_Click(object? sender, EventArgs e)
    {
        txtDirectorateCode.Clear();
        txtNameAr.Clear();
        txtNameEn.Clear();
        txtNotes.Clear();
        cmbCountry.SelectedIndex = -1;
        cmbGovernorate.SelectedIndex = -1;
        cmbStatus.SelectedIndex = 0;
        txtDirectorateCode.Focus();
    }

    private void txtSearch_TextChanged(object? sender, EventArgs e)
    {
        // الربط الفعلي بالـ API سيضاف في طبقة التنفيذ اللاحقة.
        // يبقى عنصر البحث جاهزًا من الـ Designer دون منطق بيانات وهمي.
    }
}
