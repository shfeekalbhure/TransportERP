using System.Drawing.Printing;
using TransportERP.Contracts.Waybills;

namespace TransportERP.Desktop.Waybills;

public sealed partial class UcManifestLoading : ShippingRtlControl
{
    private ManifestResponse? _manifest;

    public UcManifestLoading()
    {
        InitializeComponent();
        TransportERP.Desktop.CoreUI.SharedScreenProperties.Apply(this);
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}

    public event Action<Guid, Guid, LoadManifestLineRequest>? LoadRequested;

    public void Bind(ManifestResponse manifest, IReadOnlyList<ManifestLoadingRow> rows)
    {
        _manifest = manifest;
        _lines.DataSource = rows.ToList();
        SyncSelected();
    }

    public void Bind(ManifestResponse manifest)
        => Bind(
            manifest,
            manifest.Lines.Select(x => ManifestLoadingRow.FromManifestLine(
                x,
                x.WaybillItemId.ToString(),
                "بيانات المخاطر غير محملة")).ToList());

    public ManifestLoadingRow? SelectedLine => _lines.CurrentRow?.DataBoundItem as ManifestLoadingRow;

    private void SyncSelected()
    {
        var line = SelectedLine;
        if (line is null)
        {
            _loadQty.Maximum = 0m;
            _loadQty.Value = 0m;
            _risk.Text = "لا يوجد سطر محدد.";
            return;
        }

        var remaining = Math.Max(0m, line.AllocatedQty - line.LoadedQty);
        _loadQty.Maximum = remaining;
        if (_loadQty.Value > remaining)
            _loadQty.Value = remaining;
        _risk.Text = $"المخاطر: {RiskText(line.RiskFlags)} — حالة الخادم: {line.Status}";
    }

    private void RequestLoad()
    {
        var line = SelectedLine;
        if (_manifest is null || line is null || _loadQty.Value <= 0m) return;

        LoadRequested?.Invoke(
            _manifest.Id,
            line.ManifestLineId,
            new LoadManifestLineRequest(
                _loadQty.Value,
                DateTimeOffset.UtcNow,
                _resourceConfirmed.Checked,
                OperationId("desktop-load")));
    }

    private void btnLoad_Click(object? sender, EventArgs e) => RequestLoad();

    private void Grid_SelectionChanged(object? sender, EventArgs e) => SyncSelected();
}
