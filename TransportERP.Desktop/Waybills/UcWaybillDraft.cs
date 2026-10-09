using TransportERP.Contracts.Waybills;

namespace TransportERP.Desktop.Waybills;

public sealed partial class UcWaybillDraft : UserControl
{
    public UcWaybillDraft()
    {
        InitializeComponent();
        TransportERP.Desktop.CoreUI.SharedScreenProperties.Apply(this);
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}

    public event EventHandler? CloseRequested;
    public event EventHandler? NewRequested;
    public event EventHandler? SaveRequested;
    public event EventHandler? SubmitRequested;
    public event EventHandler? CancelRequested;

    public void Bind(WaybillResponse value)
    {
        _draftNo.Text = value.DraftNo;
        _officialNo.Text = value.WaybillNo ?? "— مسودة بلا رقم رسمي —";
        _status.Text = value.Status;
        _parties.DataSource = value.Parties.ToList();
        _items.DataSource = value.Items.ToList();
        _validation.Text = value.WaybillNo is null
            ? "الترقيم الرسمي يتم من السيرفر عند الاعتماد فقط"
            : $"رقم رسمي: {value.WaybillNo}";
    }

    public void SetValidation(IReadOnlyList<string> errors)
        => _validation.Text = errors.Count == 0 ? "جاهزة" : string.Join(" | ", errors);

    private void BtnNew_Click(object? sender, EventArgs e)
        => NewRequested?.Invoke(this, EventArgs.Empty);

    private void BtnSave_Click(object? sender, EventArgs e)
        => SaveRequested?.Invoke(this, EventArgs.Empty);

    private void BtnSubmit_Click(object? sender, EventArgs e)
        => SubmitRequested?.Invoke(this, EventArgs.Empty);

    private void BtnCancel_Click(object? sender, EventArgs e)
        => CancelRequested?.Invoke(this, EventArgs.Empty);

    private void BtnClose_Click(object? sender, EventArgs e)
        => CloseRequested?.Invoke(this, EventArgs.Empty);

}
