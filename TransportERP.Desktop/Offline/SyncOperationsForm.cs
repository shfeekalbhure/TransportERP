using TransportERP.Offline;

namespace TransportERP.Desktop.Offline;

/// <summary>
/// Read-only operational status surface. It intentionally binds an explicit,
/// metadata-only projection rather than the persisted operation object.
/// </summary>
public sealed class SyncOperationsForm : Form
{
    private readonly SyncOperationsController _controller;
    private readonly DataGridView _operations = new();
    private readonly Button _refresh = new() { Text = "تحديث", AutoSize = true };
    private readonly Button _retry = new() { Text = "إعادة المحاولة", AutoSize = true };
    private readonly Button _keepServer = new() { Text = "الاحتفاظ بنسخة الخادم", AutoSize = true };
    private readonly Button _reapply = new() { Text = "إعادة التطبيق", AutoSize = true };
    private readonly TextBox _reason = new() { Width = 280, PlaceholderText = "سبب قرار التعارض (مطلوب)" };
    private readonly CheckBox _reviewConfirmed = new() { Text = "راجعت بيانات التعارض المعروضة", AutoSize = true };
    private readonly Label _baseVersion = new() { AutoSize = true };
    private readonly Label _conflictReason = new() { AutoSize = true };
    private readonly Label _localSnapshot = new() { AutoSize = true, MaximumSize = new Size(900, 0) };
    private readonly Label _serverSnapshot = new() { AutoSize = true, MaximumSize = new Size(900, 0) };
    private readonly Label _decision = new() { AutoSize = true };
    private readonly Label _resolver = new() { AutoSize = true };
    private readonly Label _conflictResult = new() { AutoSize = true };
    private readonly Label _message = new() { AutoSize = true };
    private readonly CancellationTokenSource _lifetime = new();
    private bool _busy;

    public SyncOperationsForm(SyncOperationsController controller)
    {
        _controller = controller ?? throw new ArgumentNullException(nameof(controller));
        Text = "عمليات المزامنة";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterParent;
        MinimumSize = new Size(980, 580);

        ConfigureGrid();
        var actions = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 58,
            FlowDirection = FlowDirection.RightToLeft,
            Padding = new Padding(8)
        };
        actions.Controls.AddRange([_refresh, _retry, _keepServer, _reapply, _reason, _reviewConfirmed, _message]);

        _refresh.Click += async (_, _) => await RefreshRowsAsync();
        _retry.Click += async (_, _) => await RunSelectedAsync(_controller.RetryAsync);
        _keepServer.Click += async (_, _) => await RunConflictAsync(_controller.KeepServerAsync);
        _reapply.Click += async (_, _) => await RunConflictAsync(_controller.ReapplyAsync);
        _operations.SelectionChanged += (_, _) => UpdateConflictReview();
        _reason.TextChanged += (_, _) => UpdateActions();
        _reviewConfirmed.CheckedChanged += (_, _) => UpdateActions();
        Shown += async (_, _) => await RefreshRowsAsync();

        Controls.Add(_operations);
        Controls.Add(CreateConflictReviewPanel());
        Controls.Add(actions);
        UpdateConflictReview();
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _lifetime.Cancel();
        _lifetime.Dispose();
        base.OnFormClosed(e);
    }

    private void ConfigureGrid()
    {
        _operations.Dock = DockStyle.Fill;
        _operations.AllowUserToAddRows = false;
        _operations.AllowUserToDeleteRows = false;
        _operations.ReadOnly = true;
        _operations.AutoGenerateColumns = false;
        _operations.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _operations.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _operations.MultiSelect = false;
        _operations.RightToLeft = RightToLeft.Yes;
        _operations.Columns.AddRange(
            Column(nameof(SyncOperationDisplayRow.Action), "الإجراء"),
            Column(nameof(SyncOperationDisplayRow.EntityType), "نوع السجل"),
            Column(nameof(SyncOperationDisplayRow.StatusText), "الحالة"),
            Column(nameof(SyncOperationDisplayRow.RetryCount), "عدد المحاولات"),
            Column(nameof(SyncOperationDisplayRow.Result), "النتيجة"),
            Column(nameof(SyncOperationDisplayRow.UpdatedAt), "آخر تحديث"));
    }

    private async Task RefreshRowsAsync()
    {
        if (_busy)
            return;

        await RunBusyAsync(async () =>
        {
            _operations.DataSource = (await _controller.RefreshAsync(_lifetime.Token)).ToList();
            _message.Text = _operations.Rows.Count == 0 ? "لا توجد عمليات مزامنة محلية." : $"عدد العمليات: {_operations.Rows.Count}";
        });
    }

    private async Task RunSelectedAsync(
        Func<Guid, CancellationToken, Task<SyncUiActionResult>> action)
    {
        if (_busy || Selected is not { } selected)
            return;

        await RunBusyAsync(async () =>
        {
            var result = await action(selected.LocalOperationId, _lifetime.Token);
            _message.Text = result.Message;
            if (result.Succeeded)
                _operations.DataSource = (await _controller.RefreshAsync(_lifetime.Token)).ToList();
        });
    }

    private async Task RunConflictAsync(
        Func<Guid, string, CancellationToken, Task<SyncUiActionResult>> action)
    {
        if (_busy || Selected is not { HasCompleteConflictReview: true } selected ||
            !_reviewConfirmed.Checked || string.IsNullOrWhiteSpace(_reason.Text))
            return;

        await RunBusyAsync(async () =>
        {
            var result = await action(selected.LocalOperationId, _reason.Text, _lifetime.Token);
            _message.Text = result.Message;
            if (result.Succeeded)
            {
                _reason.Clear();
                _reviewConfirmed.Checked = false;
                _operations.DataSource = (await _controller.RefreshAsync(_lifetime.Token)).ToList();
            }
        });
    }

    private async Task RunBusyAsync(Func<Task> action)
    {
        _busy = true;
        UpdateActions();
        try
        {
            await action();
        }
        catch (OperationCanceledException) when (_lifetime.IsCancellationRequested)
        {
        }
        catch (Exception)
        {
            // The UI receives no server or transport exception detail because it
            // could include sensitive headers or remote record existence clues.
            _message.Text = "تعذر تنفيذ العملية. راجع سجل التدقيق الآمن أو حاول لاحقًا.";
        }
        finally
        {
            _busy = false;
            UpdateActions();
        }
    }

    private SyncOperationDisplayRow? Selected =>
        _operations.CurrentRow?.DataBoundItem as SyncOperationDisplayRow;

    private void UpdateActions()
    {
        var selected = Selected;
        var reviewed = selected?.HasCompleteConflictReview == true && _reviewConfirmed.Checked &&
            !string.IsNullOrWhiteSpace(_reason.Text);
        _refresh.Enabled = !_busy;
        _retry.Enabled = !_busy && _controller.CanRetry(selected);
        _keepServer.Enabled = !_busy && reviewed &&
            _controller.CanResolve(selected, SyncConflictDecision.KeepServer);
        _reapply.Enabled = !_busy && reviewed &&
            _controller.CanResolve(selected, SyncConflictDecision.Reapply);
        _reason.Enabled = !_busy && selected?.HasCompleteConflictReview == true;
        _reviewConfirmed.Enabled = !_busy && selected?.HasCompleteConflictReview == true;
    }

    private void UpdateConflictReview()
    {
        _reviewConfirmed.Checked = false;
        var review = Selected?.ConflictReview;
        _baseVersion.Text = review is null ? "—" : review.BaseVersion.ToString(System.Globalization.CultureInfo.InvariantCulture);
        _conflictReason.Text = review?.ConflictReason ?? "—";
        _localSnapshot.Text = review?.LocalSnapshot ?? "—";
        _serverSnapshot.Text = review?.ServerSnapshot ?? "—";
        _decision.Text = review?.Resolution ?? "—";
        _resolver.Text = review?.Resolver ?? "—";
        _conflictResult.Text = review?.Result ?? "—";
        UpdateActions();
    }

    private Control CreateConflictReviewPanel()
    {
        var table = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            ColumnCount = 2,
            RightToLeft = RightToLeft.Yes
        };
        table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        AddReviewRow(table, "الإصدار الأساسي", _baseVersion);
        AddReviewRow(table, "سبب التعارض", _conflictReason);
        AddReviewRow(table, "لقطة العميل المنقّحة", _localSnapshot);
        AddReviewRow(table, "لقطة الخادم المنقّحة", _serverSnapshot);
        AddReviewRow(table, "القرار", _decision);
        AddReviewRow(table, "المقرّر", _resolver);
        AddReviewRow(table, "نتيجة الحل", _conflictResult);
        return new GroupBox
        {
            Text = "مراجعة التعارض (بيانات منقّحة فقط)",
            Dock = DockStyle.Bottom,
            Height = 220,
            Padding = new Padding(8),
            Controls = { table }
        };
    }

    private static void AddReviewRow(TableLayoutPanel table, string title, Control value)
    {
        var row = table.RowCount++;
        table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        table.Controls.Add(new Label { Text = title, AutoSize = true, Font = new Font(SystemFonts.DefaultFont, FontStyle.Bold) }, 0, row);
        table.Controls.Add(value, 1, row);
    }

    private static DataGridViewTextBoxColumn Column(string property, string title) => new()
    {
        DataPropertyName = property,
        HeaderText = title,
        ReadOnly = true
    };
}
