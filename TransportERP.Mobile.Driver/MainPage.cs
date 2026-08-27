using System.Collections.ObjectModel;
using Microsoft.Maui.ApplicationModel;
using TransportERP.Mobile.Driver.Offline;
using TransportERP.Offline;
using TransportERP.Offline.Transport;

namespace TransportERP.Mobile.Driver;

public sealed class MainPage : ContentPage
{
    private readonly DriverOfflineActivationService _activation;
    private readonly DriverAuthenticatedActivationCoordinator _authenticatedActivation;
    private readonly ObservableCollection<DriverOfflineOperationStatusView> _operations = [];
    private readonly Label _mode = new();
    private readonly Label _reason = new();
    private readonly Label _evidence = new();
    private readonly Label _conflictReview = new() { Text = "Conflict review: NOT_SELECTED" };
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
    private readonly CheckBox _resolutionConfirmed = new() { IsEnabled = false };
    private readonly Entry _userName = new() { Placeholder = "User name or email" };
    private readonly Entry _password = new() { Placeholder = "Password", IsPassword = true };
    private readonly Entry _companyId = new() { Placeholder = "Company UUID (required)" };
    private readonly Entry _branchId = new() { Placeholder = "Branch UUID (required)" };
    private readonly Entry _deviceId = new() { Placeholder = "Registered DeviceId" };
    private readonly Entry _deviceCredential = new()
    {
        Placeholder = "Device credential (local session)",
        IsPassword = true
    };
    private readonly Button _signIn = new() { Text = "Sign in and activate authorized Offline scope" };
    private readonly Button _signOut = new() { Text = "Sign out", IsEnabled = false };
    private readonly Entry _partyName = new() { Placeholder = "Operational party name" };
    private readonly Entry _partyMobile = new() { Placeholder = "Operational party mobile", Keyboard = Keyboard.Telephone };
    private readonly Entry _partyAddress = new() { Placeholder = "Operational party address" };
    private readonly Button _queueParty = new()
    {
        Text = "Queue encrypted operational party",
        IsEnabled = false
    };
    private DriverOfflineOperationStatusView? _selected;
    private bool _subscribed;
    private bool _busy;

    public MainPage(
        DriverOfflineActivationService activation,
        DriverAuthenticatedActivationCoordinator authenticatedActivation)
    {
        _activation = activation ?? throw new ArgumentNullException(nameof(activation));
        _authenticatedActivation = authenticatedActivation ?? throw new ArgumentNullException(nameof(authenticatedActivation));
        Title = "TransportERP Driver";
        BackgroundColor = Color.FromArgb("#F4F7F8");

        _mode.FontSize = 18;
        _mode.FontAttributes = FontAttributes.Bold;
        _reason.TextColor = Color.FromArgb("#455A64");
        _evidence.TextColor = Color.FromArgb("#455A64");
        _conflictReview.TextColor = Color.FromArgb("#7A2E0B");
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
        _resolutionConfirmed.CheckedChanged += (_, _) =>
        {
            if (_activation.Active is { } active) UpdateActionAvailability(active.Runtime);
        };

        var refresh = new Button { Text = "Refresh operation status" };
        refresh.Clicked += async (_, _) => await RefreshAsync();
        _signIn.Clicked += async (_, _) => await SignInAndActivateAsync();
        _signOut.Clicked += async (_, _) => await SignOutAsync();
        _queueParty.Clicked += async (_, _) => await QueueOperationalPartyAsync();
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
                _userName,
                _password,
                _companyId,
                _branchId,
                _deviceId,
                _deviceCredential,
                new HorizontalStackLayout { Spacing = 8, Children = { _signIn, _signOut } },
                new Label { Text = "Offline business action — CreateOperationalParty" },
                _partyName,
                _partyMobile,
                _partyAddress,
                _queueParty,
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
                new HorizontalStackLayout
                {
                    Spacing = 8,
                    Children =
                    {
                        _resolutionConfirmed,
                        new Label { Text = "I reviewed the redacted conflict evidence and confirm this decision." }
                    }
                },
                _conflictReview,
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
            _signOut.IsEnabled = true;
            _signIn.IsEnabled = false;
            _mode.Text = $"Offline runtime: {runtime.Status.Mode.ToString().ToUpperInvariant()}";
            _reason.Text = $"Reason: {SanitizeCode(runtime.Status.ReasonCode)}";
            var statuses = await runtime.ListOperationStatusesAsync();
            _operations.Clear();
            foreach (var status in statuses) _operations.Add(status);
            _evidence.Text =
                $"Sanitized evidence: {_operations.Count} scoped metadata row(s); payload, keys, bearer and proof omitted.";
            _selected = null;
            _operationList.SelectedItem = null;
            _conflictReview.Text = "Conflict review: NOT_SELECTED";
            _resolutionReason.Text = string.Empty;
            _resolutionConfirmed.IsChecked = false;
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
        _signIn.IsEnabled = true;
        _signOut.IsEnabled = false;
        _queueParty.IsEnabled = false;
        _operations.Clear();
        _selected = null;
        _conflictReview.Text = "Conflict review: NOT_SELECTED";
        _resolutionReason.Text = string.Empty;
        DisableActions();
    }

    private void OnSelectionChanged(object? sender, SelectionChangedEventArgs args)
    {
        _selected = args.CurrentSelection.FirstOrDefault() as DriverOfflineOperationStatusView;
        _conflictReview.Text = _selected?.SafeConflictReview ?? "Conflict review: NOT_SELECTED";
        _resolutionConfirmed.IsChecked = false;
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

    private async Task QueueOperationalPartyAsync()
    {
        var active = _activation.Active;
        if (active is null)
        {
            RenderClosed();
            return;
        }

        _queueParty.IsEnabled = false;
        try
        {
            var result = await active.Runtime.CreateBusinessProducer().QueueOperationalPartyAsync(
                _partyName.Text ?? string.Empty,
                _partyMobile.Text ?? string.Empty,
                _partyAddress.Text ?? string.Empty);
            _partyName.Text = string.Empty;
            _partyMobile.Text = string.Empty;
            _partyAddress.Text = string.Empty;
            _actionResult.Text = result.Created ? "Result: BUSINESS_OPERATION_QUEUED" : "Result: BUSINESS_OPERATION_EXISTS";
            await RefreshAsync();
        }
        catch (Exception exception)
        {
            _actionResult.Text = $"Result: {SafeCode(exception)}";
        }
        finally
        {
            if (_activation.Active is { Runtime: { Status.Mode: DriverOfflineRuntimeMode.Ready,
                    CanQueueOperationalParties: true } })
                _queueParty.IsEnabled = true;
        }
    }

    private async Task SignInAndActivateAsync()
    {
        if (_busy) return;
        var password = _password.Text ?? string.Empty;
        var deviceCredential = string.IsNullOrEmpty(_deviceCredential.Text) ? null : _deviceCredential.Text;
        _password.Text = string.Empty;
        _deviceCredential.Text = string.Empty;
        if (!TryRequiredGuid(_companyId.Text, out var companyId) ||
            !TryRequiredGuid(_branchId.Text, out var branchId))
        {
            _actionResult.Text = "Result: AUTHENTICATION_INPUT_INVALID";
            return;
        }

        _busy = true;
        _signIn.IsEnabled = false;
        try
        {
            await _authenticatedActivation.SignInAndActivateAsync(
                new DriverInteractiveSignInRequest(
                    _userName.Text ?? string.Empty,
                    password,
                    companyId,
                    branchId,
                    _deviceId.Text ?? string.Empty,
                    deviceCredential));
            _actionResult.Text = "Result: OFFLINE_ACTIVATED";
        }
        catch (Exception exception)
        {
            _actionResult.Text = $"Result: {SafeCode(exception)}";
        }
        finally
        {
            _password.Text = string.Empty;
            _deviceCredential.Text = string.Empty;
            _busy = false;
            await RefreshAsync();
        }
    }

    private async Task SignOutAsync()
    {
        if (_busy) return;
        _busy = true;
        try
        {
            await _authenticatedActivation.SignOutAsync();
            _actionResult.Text = "Result: SIGNED_OUT";
        }
        catch (Exception exception)
        {
            _actionResult.Text = $"Result: {SafeCode(exception)}";
        }
        finally
        {
            _password.Text = string.Empty;
            _deviceCredential.Text = string.Empty;
            _busy = false;
            await RefreshAsync();
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
        if (!_selected.ConflictDecisionReady || _selected.ConflictBaseVersion is not > 0)
        {
            _actionResult.Text = "Result: CONFLICT_REVIEW_REQUIRED";
            return;
        }
        if (string.IsNullOrWhiteSpace(reason))
        {
            _actionResult.Text = "Result: RESOLUTION_REASON_REQUIRED";
            return;
        }
        if (!_resolutionConfirmed.IsChecked)
        {
            _actionResult.Text = "Result: RESOLUTION_CONFIRMATION_REQUIRED";
            return;
        }
        long? baseVersion = null;
        if (decision == OfflineConflictDecision.Reapply)
        {
            if (_selected.ConflictServerVersion is not > 0)
            {
                _actionResult.Text = "Result: REAPPLY_BASE_VERSION_REQUIRED";
                return;
            }
            baseVersion = _selected.ConflictServerVersion;
        }

        try
        {
            await active.Runtime.ResolveConflictAsync(
                _selected.LocalOperationId, decision, reason, baseVersion);
            _resolutionReason.Text = string.Empty;
            _resolutionConfirmed.IsChecked = false;
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
        _queueParty.IsEnabled = ready && runtime.CanQueueOperationalParties;
        var conflictSelected = _selected is
            { Status: OfflineOperationStatus.Conflict, ConflictDecisionReady: true, ConflictBaseVersion: > 0 };
        _retry.IsEnabled = ready && runtime.OperationPermissions.CanRetryFailedOperations &&
            _selected?.Status == OfflineOperationStatus.Failed;
        _resolutionReason.IsEnabled = ready && runtime.OperationPermissions.CanResolveConflicts && conflictSelected;
        _resolutionConfirmed.IsEnabled = _resolutionReason.IsEnabled;
        _keepServer.IsEnabled = _resolutionReason.IsEnabled && _resolutionConfirmed.IsChecked;
        _reapply.IsEnabled = _resolutionReason.IsEnabled && _resolutionConfirmed.IsChecked &&
            _selected?.ConflictServerVersion is > 0;
    }

    private void DisableActions()
    {
        _retry.IsEnabled = false;
        _resolutionReason.IsEnabled = false;
        _resolutionConfirmed.IsEnabled = false;
        _resolutionConfirmed.IsChecked = false;
        _keepServer.IsEnabled = false;
        _reapply.IsEnabled = false;
        _queueParty.IsEnabled = false;
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

    private static bool TryRequiredGuid(string? value, out Guid? result)
    {
        result = null;
        if (string.IsNullOrWhiteSpace(value)) return false;
        if (!Guid.TryParse(value, out var parsed) || parsed == Guid.Empty) return false;
        result = parsed;
        return true;
    }
}
