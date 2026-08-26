using System.Collections.ObjectModel;
using Microsoft.Maui.ApplicationModel;
using TransportERP.Mobile.Driver.Offline;
using TransportERP.Offline;
using TransportERP.Offline.Transport;

namespace TransportERP.Mobile.Driver;

public sealed class MainPage : ContentPage
{
    private readonly DriverOfflineActivationService _activation;
    private readonly ObservableCollection<DriverOfflineOperationStatusView> _operations = [];
    private readonly Label _mode = new();
    private readonly Label _reason = new();
    private readonly Label _evidence = new();
    private readonly Label _actionResult = new();
    private readonly CollectionView _operationList;
    private readonly Button _retry = new() { Text = "Manual retry", IsEnabled = false };
    private readonly Button _keepServer = new() { Text = "KEEP_SERVER", IsEnabled = false };
    private readonly Button _reapply = new() { Text = "REAPPLY", IsEnabled = false };
    private readonly Entry _resolutionReason = new()
    {
        Placeholder = "Resolution reason (required)",
        MaxLength = 500,
        IsEnabled = false
    };
    private readonly Entry _reapplyBaseVersion = new()
    {
        Placeholder = "Current base version for REAPPLY",
        Keyboard = Keyboard.Numeric,
        IsEnabled = false
    };
    private DriverOfflineOperationStatusView? _selected;
    private bool _subscribed;
    private bool _busy;

    public MainPage(DriverOfflineActivationService activation)
    {
        _activation = activation ?? throw new ArgumentNullException(nameof(activation));
        Title = "TransportERP Driver";
        BackgroundColor = Color.FromArgb("#F4F7F8");

        _mode.FontSize = 18;
        _mode.FontAttributes = FontAttributes.Bold;
        _reason.TextColor = Color.FromArgb("#455A64");
        _evidence.TextColor = Color.FromArgb("#455A64");
        _actionResult.TextColor = Color.FromArgb("#0B3A53");

        _operationList = new CollectionView
        {
            ItemsSource = _operations,
            SelectionMode = SelectionMode.Single,
            EmptyView = new Label { Text = "No local synchronization operation metadata." },
            ItemTemplate = new DataTemplate(() =>
            {
                var summary = new Label
                {
                    Margin = new Thickness(0, 6),
                    LineBreakMode = LineBreakMode.WordWrap,
                    TextColor = Color.FromArgb("#263238")
                };
                summary.SetBinding(Label.TextProperty, nameof(DriverOfflineOperationStatusView.SafeSummary));
                return summary;
            })
        };
        _operationList.SelectionChanged += OnSelectionChanged;

        var refresh = new Button { Text = "Refresh operation status" };
        refresh.Clicked += async (_, _) => await RefreshAsync();
        _retry.Clicked += async (_, _) => await RetrySelectedAsync();
        _keepServer.Clicked += async (_, _) =>
            await ResolveSelectedAsync(OfflineConflictDecision.KeepServer);
        _reapply.Clicked += async (_, _) =>
            await ResolveSelectedAsync(OfflineConflictDecision.Reapply);

        var header = new VerticalStackLayout
        {
            Spacing = 8,
            Children =
            {
                new Label
                {
                    Text = "Synchronization operations",
                    FontSize = 28,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Color.FromArgb("#0B3A53")
                },
                _mode,
                _reason,
                _evidence,
                _actionResult,
                refresh
            }
        };
        var actions = new VerticalStackLayout
        {
            Spacing = 8,
            Children =
            {
                _retry,
                _resolutionReason,
                _reapplyBaseVersion,
                new HorizontalStackLayout
                {
                    Spacing = 8,
                    Children = { _keepServer, _reapply }
                }
            }
        };
        var content = new Grid
        {
            Padding = new Thickness(24),
            RowDefinitions =
            {
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Star),
                new RowDefinition(GridLength.Auto)
            }
        };
        Grid.SetRow(_operationList, 1);
        Grid.SetRow(actions, 2);
        content.Children.Add(header);
        content.Children.Add(_operationList);
        content.Children.Add(actions);
        Content = content;

        RenderClosed();
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    private async void OnLoaded(object? sender, EventArgs args)
    {
        if (!_subscribed)
        {
            _activation.StateChanged += OnActivationStateChanged;
            _subscribed = true;
        }
        await RefreshAsync();
    }

    private void OnUnloaded(object? sender, EventArgs args)
    {
        if (!_subscribed) return;
        _activation.StateChanged -= OnActivationStateChanged;
        _subscribed = false;
    }

    private void OnActivationStateChanged(object? sender, EventArgs args) =>
        MainThread.BeginInvokeOnMainThread(async () => await RefreshAsync());

    private async Task RefreshAsync()
    {
        if (_busy) return;
        _busy = true;
        try
        {
            var active = _activation.Active;
            if (active is null)
            {
                RenderClosed();
                return;
            }

            var runtime = active.Runtime;
            _mode.Text = $"Offline runtime: {runtime.Status.Mode.ToString().ToUpperInvariant()}";
            _reason.Text = $"Reason: {SanitizeCode(runtime.Status.ReasonCode)}";
            var statuses = await runtime.ListOperationStatusesAsync();
            _operations.Clear();
            foreach (var status in statuses) _operations.Add(status);
            _evidence.Text =
                $"Sanitized evidence: {_operations.Count} scoped metadata row(s); payload, keys, bearer and proof omitted.";
            _selected = null;
            _operationList.SelectedItem = null;
            UpdateActionAvailability(runtime);
        }
        catch (Exception exception)
        {
            _actionResult.Text = $"Result: {SafeCode(exception)}";
            DisableActions();
        }
        finally
        {
            _busy = false;
        }
    }

    private void RenderClosed()
    {
        _mode.Text = "Offline runtime: CLOSED";
        _reason.Text = "Reason: OFFLINE_CLOSED";
        _evidence.Text = "No offline store is opened and no local operation evidence is available.";
        _actionResult.Text = "Sign in and explicitly activate an authorized scope to use synchronization.";
        _operations.Clear();
        _selected = null;
        DisableActions();
    }

    private void OnSelectionChanged(object? sender, SelectionChangedEventArgs args)
    {
        _selected = args.CurrentSelection.FirstOrDefault() as DriverOfflineOperationStatusView;
        var active = _activation.Active;
        if (active is null) DisableActions();
        else UpdateActionAvailability(active.Runtime);
    }

    private async Task RetrySelectedAsync()
    {
        var active = _activation.Active;
        if (active is null || _selected is null)
        {
            RenderClosed();
            return;
        }

        try
        {
            await active.Runtime.RetryFailedOperationAsync(_selected.LocalOperationId);
            _actionResult.Text = "Result: RETRY_QUEUED";
            await RefreshAsync();
        }
        catch (Exception exception)
        {
            _actionResult.Text = $"Result: {SafeCode(exception)}";
        }
    }

    private async Task ResolveSelectedAsync(OfflineConflictDecision decision)
    {
        var active = _activation.Active;
        if (active is null || _selected is null)
        {
            RenderClosed();
            return;
        }

        var reason = _resolutionReason.Text ?? string.Empty;
        long? baseVersion = null;
        if (decision == OfflineConflictDecision.Reapply)
        {
            if (!long.TryParse(_reapplyBaseVersion.Text, out var parsed) || parsed < 0)
            {
                _actionResult.Text = "Result: REAPPLY_BASE_VERSION_REQUIRED";
                return;
            }
            baseVersion = parsed;
        }

        try
        {
            await active.Runtime.ResolveConflictAsync(
                _selected.LocalOperationId, decision, reason, baseVersion);
            _resolutionReason.Text = string.Empty;
            _reapplyBaseVersion.Text = string.Empty;
            _actionResult.Text = "Result: CONFLICT_RESOLVED";
            await RefreshAsync();
        }
        catch (Exception exception)
        {
            _actionResult.Text = $"Result: {SafeCode(exception)}";
        }
    }

    private void UpdateActionAvailability(DriverOfflineRuntime runtime)
    {
        var ready = runtime.Status.Mode == DriverOfflineRuntimeMode.Ready;
        var conflictSelected = _selected?.Status == OfflineOperationStatus.Conflict;
        _retry.IsEnabled = ready && runtime.OperationPermissions.CanRetryFailedOperations &&
            _selected?.Status == OfflineOperationStatus.Failed;
        _resolutionReason.IsEnabled = ready && runtime.OperationPermissions.CanResolveConflicts && conflictSelected;
        _reapplyBaseVersion.IsEnabled = _resolutionReason.IsEnabled;
        _keepServer.IsEnabled = _resolutionReason.IsEnabled;
        _reapply.IsEnabled = _resolutionReason.IsEnabled;
    }

    private void DisableActions()
    {
        _retry.IsEnabled = false;
        _resolutionReason.IsEnabled = false;
        _reapplyBaseVersion.IsEnabled = false;
        _keepServer.IsEnabled = false;
        _reapply.IsEnabled = false;
    }

    private static string SafeCode(Exception exception) => exception switch
    {
        DriverOfflineUnavailableException driver => SanitizeCode(driver.Code),
        OfflineStoreException store => SanitizeCode(store.Code),
        SyncTransportException transport => SanitizeCode(transport.Code),
        _ => "OPERATION_FAILED"
    };

    private static string SanitizeCode(string? code) =>
        !string.IsNullOrEmpty(code) && code.Length <= 64 &&
        code.All(character => character is >= 'A' and <= 'Z' or >= '0' and <= '9' or '_')
            ? code
            : "OPERATION_FAILED";
}
