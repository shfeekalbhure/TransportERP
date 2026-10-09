using System.ComponentModel;
using TransportERP.Contracts.Accounting;
namespace TransportERP.Desktop;

internal sealed partial class ReceiptConfigurationDialog : Form
{
    private readonly ReceiptBootstrap? bootstrap;
    private readonly BindingList<DestinationRow> destinations = new();
    private readonly BindingList<TypeRow> types = new();
    private bool bindingConfiguration;
    private sealed record Option(string Id, string Label);
    public sealed class DestinationRow
    {
        [Browsable(false)] public Guid Id { get; set; } = Guid.NewGuid();
        public string Label { get; set; } = "";
        public string Kind { get; set; } = "";
        public Guid AccountId { get; set; }
    }
    public sealed class TypeRow
    {
        [Browsable(false)] public Guid Id { get; set; } = Guid.NewGuid();
        public string Label { get; set; } = "";
        public bool RequiresWaybill { get; set; }
    }

    // Parameterless construction exposes the existing UI to the Designer without data/service access.
    public ReceiptConfigurationDialog()
    {
        InitializeComponent();
        destinationGrid.DataSource = destinations;
        typeGrid.DataSource = types;
        global::TransportERP.ScreenRootProperties.Attach(this);
    }
    public ReceiptConfigurationDialog(ReceiptBootstrap bootstrap) : this()
    {
        this.bootstrap = bootstrap;
        bindingConfiguration = true;
        try
        {
            treatment.DisplayMember = "Label"; treatment.ValueMember = "Id";
            treatment.DataSource = new[] { new Option("", "غير مهيأ — يمنع ترحيل الشيك"), new Option("CHEQUES_RECEIVABLE", "أوراق قبض حتى التحصيل"), new Option("DIRECT_BANK", "إلى الحساب البنكي مباشرة") };
            rounding.DisplayMember = "Label"; rounding.ValueMember = "Id";
            rounding.DataSource = new[] { new Option("", "غير مهيأ — يمنع الترحيل"), new Option("TO_EVEN", "تقريب المنتصف إلى العدد الزوجي"), new Option("AWAY_FROM_ZERO", "تقريب المنتصف بعيدًا عن الصفر") };
            treatment.SelectedValue = bootstrap.Configuration.ChequeTreatment ?? "";
            rounding.SelectedValue = bootstrap.Configuration.Rounding ?? "";
            Choices(receivable, bootstrap.Accounts, bootstrap.Configuration.ChequesReceivableAccountId);
            Choices(sequence, bootstrap.Sequences, bootstrap.Configuration.NumberSequenceId);
            Choices(defaultCurrency, bootstrap.Currencies, bootstrap.Configuration.DefaultCurrencyId);
            Source(centerSource, bootstrap.Configuration.CostCenterDimensionCode);
            Source(projectSource, bootstrap.Configuration.ProjectDimensionCode);
            Source(activitySource, bootstrap.Configuration.ActivityDimensionCode);
            Centers(bootstrap.Configuration.DefaultCostCenterId);
            postingDimension.DisplayMember = "Label"; postingDimension.ValueMember = "Id";
            postingDimension.DataSource = new[] { new Option("", "مراجع للسند فقط — دون بُعد بالقيد"), new Option("costCenter", "مركز التكلفة"), new Option("project", "المشروع"), new Option("activity", "النشاط") };
            postingDimension.SelectedValue = bootstrap.Configuration.PostingDimension ?? "";
            colDestinationKind.DataSource = new[] { new Option("CASH", "صندوق"), new Option("BANK", "بنك") };
            colDestinationAccount.DataSource = new[] { new ReceiptChoice(Guid.Empty, "غير مهيأ — يمنع الترحيل") }.Concat(bootstrap.Accounts).ToList();
            foreach (var d in bootstrap.Configuration.Destinations)
                destinations.Add(new DestinationRow { Id = d.Id, Label = d.Label, Kind = d.Kind, AccountId = d.AccountId });
            foreach (var t in bootstrap.Configuration.Types)
                types.Add(new TypeRow { Id = t.Id, Label = t.Label, RequiresWaybill = t.RequiresWaybill });
            foreach (var list in new[] { collectors, salespeople })
            {
                list.DisplayMember = "Label";
                foreach (var user in bootstrap.Users) list.Items.Add(user);
            }
            for (int i = 0; i < bootstrap.Users.Count; i++)
            {
                collectors.SetItemChecked(i, bootstrap.Configuration.CollectorIds.Contains(bootstrap.Users[i].Id));
                salespeople.SetItemChecked(i, bootstrap.Configuration.SalespersonIds.Contains(bootstrap.Users[i].Id));
            }
        }
        finally { bindingConfiguration = false; }
    }
    private static void Choices(ComboBox combo, List<ReceiptChoice> choices, Guid? selected)
    {
        combo.DisplayMember = "Label"; combo.ValueMember = "Id";
        combo.DataSource = new[] { new ReceiptChoice(Guid.Empty, "غير مهيأ") }.Concat(choices).ToList();
        combo.SelectedValue = selected ?? Guid.Empty;
    }
    private void Source(ComboBox combo, string? selected)
    {
        combo.DisplayMember = "Label"; combo.ValueMember = "Id";
        combo.DataSource = new[] { new Option("", "غير مهيأ") }.Concat((bootstrap?.Dimensions ?? []).Select(d => d.DimensionCode).Distinct().Select(code => new Option(code, code))).ToList();
        combo.SelectedValue = selected ?? "";
    }
    private void Centers(Guid? id) => Choices(defaultCenter, (bootstrap?.Dimensions ?? [])
        .Where(d => d.DimensionCode == (string?)centerSource.SelectedValue)
        .Select(d => new ReceiptChoice(d.Id, d.Label)).ToList(), id);
    private void centerSource_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (!bindingConfiguration) Centers(null);
    }
    private void grid_DataError(object? sender, DataGridViewDataErrorEventArgs e)
    {
        e.ThrowException = false; e.Cancel = true;
    }
    private void save_Click(object? sender, EventArgs e)
    {
        if (destinationGrid.EndEdit() && typeGrid.EndEdit())
        {
            BindingContext![destinations]?.EndCurrentEdit();
            BindingContext![types]?.EndCurrentEdit();
            DialogResult = DialogResult.OK;
        }
    }
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ReceiptConfiguration Configuration => new(
        string.IsNullOrEmpty((string?)treatment.SelectedValue) ? null : (string)treatment.SelectedValue!,
        receivable.SelectedValue is Guid account && account != Guid.Empty ? account : null,
        string.IsNullOrEmpty((string?)rounding.SelectedValue) ? null : (string)rounding.SelectedValue!,
        sequence.SelectedValue is Guid seq && seq != Guid.Empty ? seq : null,
        destinations.Select(d => new ReceiptDestination(d.Id, d.Label, d.Kind, d.AccountId)).ToList(),
        types.Select(t => new ReceiptType(t.Id, t.Label, t.RequiresWaybill)).ToList(),
        collectors.CheckedItems.Cast<ReceiptChoice>().Select(u => u.Id).ToList(), salespeople.CheckedItems.Cast<ReceiptChoice>().Select(u => u.Id).ToList(),
        defaultCurrency.SelectedValue is Guid currency && currency != Guid.Empty ? currency : null,
        defaultCenter.SelectedValue is Guid center && center != Guid.Empty ? center : null,
        string.IsNullOrEmpty((string?)centerSource.SelectedValue) ? null : (string)centerSource.SelectedValue!,
        string.IsNullOrEmpty((string?)projectSource.SelectedValue) ? null : (string)projectSource.SelectedValue!,
        string.IsNullOrEmpty((string?)activitySource.SelectedValue) ? null : (string)activitySource.SelectedValue!,
        string.IsNullOrEmpty((string?)postingDimension.SelectedValue) ? null : (string)postingDimension.SelectedValue!);
}
