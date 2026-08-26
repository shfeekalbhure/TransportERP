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
        actions.Controls.AddRange([_refresh, _retry, _keepServer, _reapply, _message]);

        _refresh.Click += async (_, _) => await RefreshRowsAsync();
        _retry.Click += async (_, _) => await RunSelectedAsync(_controller.RetryAsync);
        _keepServer.Click += async (_, _) => await RunSelectedAsync(_controller.KeepServerAsync);
        _reapply.Click += async (_, _) => await RunSelectedAsync(_controller.ReapplyAsync);
        _operations.SelectionChanged += (_, _) => UpdateActions();
        Shown += async (_, _) => await RefreshRowsAsync();

        Controls.Add(_operations);
        Controls.Add(actions);
        UpdateActions();
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
        _refresh.Enabled = !_busy;
        _retry.Enabled = !_busy && _controller.CanRetry(selected);
        _keepServer.Enabled = !_busy && _controller.CanResolve(selected, SyncConflictDecision.KeepServer);
        _reapply.Enabled = !_busy && _controller.CanResolve(selected, SyncConflictDecision.Reapply);
    }

    private static DataGridViewTextBoxColumn Column(string property, string title) => new()
    {
        DataPropertyName = property,
        HeaderText = title,
        ReadOnly = true
    };
}
