using System.Drawing.Printing;
using TransportERP.Contracts.Waybills;

namespace TransportERP.Desktop.Waybills;

public sealed partial class UcManifest : ShippingRtlControl
{
    private Guid _manifestId;
    private long _manifestVersion;
    private ManifestLineResponse[] _boundLines = Array.Empty<ManifestLineResponse>();
    private string _printTitle = string.Empty;
    private string _printSummary = string.Empty;
    private string _printBody = string.Empty;
    private int _printOffset;
    private int _printPageNumber;

    public UcManifest()
    {
        InitializeComponent();
        TransportERP.Desktop.CoreUI.SharedScreenProperties.Apply(this);
        _printDocument.BeginPrint += BeginPrint;
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}

    public event Action<Guid, FinalizeManifestRequest>? FinalizeRequested;

    public void Bind(ManifestScreenState state)
    {
        _manifestId = state.Manifest.Id;
        _manifestVersion = state.Manifest.Version;
        _header.Text = $"{state.Manifest.ManifestNo} — إصدار {state.Manifest.Version}";
        _driver.Text = state.Trip.DriverId.ToString();
        _vehicle.Text = state.Trip.VehicleId.ToString();
        _totalQty.Text = state.TotalQuantity.ToString("N3");
        _totalWeight.Text = state.TotalWeight.ToString("N3");
        _totalVolume.Text = state.TotalVolume.ToString("N3");
        _status.Text = state.Manifest.Status;
        _boundLines = state.Manifest.Lines.ToArray();
        _lines.DataSource = _boundLines.ToList();
    }

    public void Bind(ManifestResponse manifest)
    {
        _manifestId = manifest.Id;
        _manifestVersion = manifest.Version;
        _header.Text = $"{manifest.ManifestNo} — إصدار {manifest.Version}";
        _driver.Text = "بيانات السائق غير محملة";
        _vehicle.Text = "بيانات المركبة غير محملة";
        _totalQty.Text = manifest.Lines.Sum(x => x.Quantity).ToString("N3");
        _totalWeight.Text = manifest.Lines.Sum(x => x.Weight).ToString("N3");
        _totalVolume.Text = manifest.Lines.Sum(x => x.Volume).ToString("N3");
        _status.Text = manifest.Status;
        _boundLines = manifest.Lines.ToArray();
        _lines.DataSource = _boundLines.ToList();
    }

    private void RequestFinalize()
    {
        if (_manifestId == Guid.Empty || _manifestVersion < 1) return;
        FinalizeRequested?.Invoke(
            _manifestId,
            new FinalizeManifestRequest(_manifestVersion, OperationId("desktop-manifest-finalize")));
    }

    private void ShowPrintPreview()
    {
        if (_manifestId == Guid.Empty || _manifestVersion < 1)
        {
            MessageBox.Show(this, "حمّل كشف الحمولة قبل معاينة الطباعة.", "كشف الحمولة");
            return;
        }
        using var preview = new PrintPreviewDialog
        {
            Document = _printDocument,
            Width = 1000,
            Height = 720,
            RightToLeft = RightToLeft.Yes
        };
        var owner = FindForm();
        try
        {
            if (owner is null) preview.ShowDialog();
            else preview.ShowDialog(owner);
        }
        catch (InvalidOperationException ex)
        {
            MessageBox.Show(this, ex.Message, "تعذر إعداد الطباعة", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void BeginPrint(object? sender, PrintEventArgs e)
    {
        // Freeze one loaded version for the whole job, including preview re-pagination.
        _printOffset = 0;
        _printPageNumber = 0;
        e.Cancel = _manifestId == Guid.Empty || _manifestVersion < 1;
        var lines = _boundLines.ToArray();
        _printTitle = $"كشف الحمولة — {_header.Text}";
        _printSummary = string.Join(Environment.NewLine, new[]
        {
            $"السائق: {_driver.Text}",
            $"المركبة: {_vehicle.Text}",
            $"إجمالي الكمية: {lines.Sum(x => x.Quantity):N3}",
            $"إجمالي الوزن: {lines.Sum(x => x.Weight):N3}",
            $"إجمالي الحجم: {lines.Sum(x => x.Volume):N3}",
            $"الحالة: {_status.Text}"
        });
        _printBody = lines.Length == 0 ? "لا توجد بنود في الكشف المحمل." :
            string.Join(Environment.NewLine + Environment.NewLine, lines.Select((line, index) =>
                string.Join(Environment.NewLine, new[]
                {
                    $"البند {index + 1} — معرف السطر: {line.Id}",
                    $"معرف التخصيص: {line.AllocationId}",
                    $"معرف البوليصة: {line.WaybillId}",
                    $"معرف صنف البوليصة: {line.WaybillItemId}",
                    $"الكمية: {line.Quantity:G} — الكمية المحملة: {line.LoadedQuantity:G}",
                    $"الوزن: {line.Weight:G} — الحجم: {line.Volume:G}",
                    $"حالة التحميل: {line.LoadStatus}"
                })));
    }

    private void PrintPage(object? sender, PrintPageEventArgs e)
    {
        var graphics = e.Graphics ?? throw new InvalidOperationException("سطح الطباعة غير متاح.");
        using var titleFont = new Font("Segoe UI", 14, FontStyle.Bold);
        using var bodyFont = new Font("Segoe UI", 10);
        using var format = new StringFormat
        {
            Alignment = StringAlignment.Near,
            FormatFlags = StringFormatFlags.DirectionRightToLeft | StringFormatFlags.LineLimit
        };
        var bounds = e.MarginBounds;
        if (bounds.Width <= 0 || bounds.Height <= 0)
            throw new InvalidOperationException("اضبط حجم الورق والهوامش لتوفير مساحة طباعة موجبة.");
        var heading = $"{_printTitle} — صفحة {++_printPageNumber}";
        var titleHeight = (float)Math.Ceiling(graphics.MeasureString(heading, titleFont, bounds.Width, format).Height);
        var summaryHeight = (float)Math.Ceiling(graphics.MeasureString(_printSummary, bodyFont, bounds.Width, format).Height);
        var y = bounds.Top + titleHeight + 8;
        var detailTop = y + summaryHeight + 16;
        var detailBounds = new RectangleF(bounds.Left, detailTop, bounds.Width, bounds.Bottom - detailTop);
        if (detailBounds.Height < bodyFont.GetHeight(graphics) * 2)
            throw new InvalidOperationException("مساحة الورق بعد الهوامش لا تكفي لتفاصيل الكشف. وسّع مساحة الطباعة.");
        graphics.DrawString(heading, titleFont, Brushes.Black,
            new RectangleF(bounds.Left, bounds.Top, bounds.Width, titleHeight), format);
        graphics.DrawString(_printSummary, bodyFont, Brushes.Black,
            new RectangleF(bounds.Left, y, bounds.Width, summaryHeight), format);
        var remaining = _printBody.Substring(_printOffset);
        graphics.MeasureString(remaining, bodyFont, detailBounds.Size, format, out var charactersFitted, out _);
        if (charactersFitted <= 0)
            throw new InvalidOperationException("تعذر وضع تفاصيل الكشف داخل مساحة الطباعة الحالية.");
        graphics.DrawString(remaining.Substring(0, charactersFitted), bodyFont, Brushes.Black, detailBounds, format);
        _printOffset += charactersFitted;
        e.HasMorePages = _printOffset < _printBody.Length;
    }

    private void btnFinalize_Click(object? sender, EventArgs e) => RequestFinalize();

    private void btnPrintPreview_Click(object? sender, EventArgs e) => ShowPrintPreview();
}
