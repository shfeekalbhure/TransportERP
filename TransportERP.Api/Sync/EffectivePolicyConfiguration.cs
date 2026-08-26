using System.Collections.Frozen;
using Microsoft.Extensions.Options;
using TransportERP.Api.Security;
using TransportERP.Application.Sync;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Api.Sync;

public sealed record ConfiguredDeviceSyncPolicy(
    Guid RegisteredDeviceId,
    Guid CompanyId,
    Guid BranchId,
    string DeviceId,
    IReadOnlySet<string> AllowedActions,
    SyncPolicyRestriction? Restriction = null);

/// <summary>
/// Immutable startup snapshot of governed scope restrictions. No request can
/// mutate this source and no management API is implied by this representation.
/// </summary>
public sealed class EffectivePolicyConfiguration
{
    private readonly IReadOnlyDictionary<Guid, SyncPolicyRestriction> companies;
    private readonly IReadOnlyDictionary<(Guid CompanyId, Guid BranchId), SyncPolicyRestriction> branches;
    private readonly IReadOnlyDictionary<Guid, ConfiguredDeviceSyncPolicy> devices;

    private EffectivePolicyConfiguration(
        IReadOnlyDictionary<Guid, SyncPolicyRestriction> companies,
        IReadOnlyDictionary<(Guid CompanyId, Guid BranchId), SyncPolicyRestriction> branches,
        IReadOnlyDictionary<Guid, ConfiguredDeviceSyncPolicy> devices,
        IReadOnlyList<string> loadErrors)
    {
        this.companies = companies.ToFrozenDictionary(
            x => x.Key, x => Freeze(x.Value));
        this.branches = branches.ToFrozenDictionary(
            x => x.Key, x => Freeze(x.Value));
        this.devices = devices.ToFrozenDictionary(
            x => x.Key,
            x => x.Value with
            {
                AllowedActions = x.Value.AllowedActions.ToFrozenSet(StringComparer.Ordinal),
                Restriction = x.Value.Restriction is null ? null : Freeze(x.Value.Restriction)
            });
        LoadErrors = Array.AsReadOnly(loadErrors.ToArray());
    }

    public IReadOnlyList<string> LoadErrors { get; }
    public IEnumerable<KeyValuePair<Guid, SyncPolicyRestriction>> Companies => companies;
    public IEnumerable<KeyValuePair<(Guid CompanyId, Guid BranchId), SyncPolicyRestriction>> Branches => branches;
    public IEnumerable<ConfiguredDeviceSyncPolicy> Devices => devices.Values;

    public bool TryGetCompany(Guid companyId, out SyncPolicyRestriction restriction)
        => companies.TryGetValue(companyId, out restriction!);

    public bool TryGetBranch(Guid companyId, Guid branchId, out SyncPolicyRestriction restriction)
        => branches.TryGetValue((companyId, branchId), out restriction!);

    public bool TryGetDevice(Guid registeredDeviceId, out ConfiguredDeviceSyncPolicy policy)
        => devices.TryGetValue(registeredDeviceId, out policy!);

    public static EffectivePolicyConfiguration Load(IConfiguration configuration)
    {
        var errors = new List<string>();
        var companies = new Dictionary<Guid, SyncPolicyRestriction>();
        var branches = new Dictionary<(Guid, Guid), SyncPolicyRestriction>();
        var devices = new Dictionary<Guid, ConfiguredDeviceSyncPolicy>();
        var root = configuration.GetSection("Sync:EffectivePolicy");

        foreach (var companySection in root.GetSection("Companies").GetChildren())
        {
            if (!TryNonEmptyGuid(companySection.Key, out var companyId))
            {
                errors.Add($"Sync:EffectivePolicy:Companies:{companySection.Key} must use a non-empty company UUID key.");
                continue;
            }
            companies[companyId] = ReadRestriction(companySection);
        }

        foreach (var companySection in root.GetSection("Branches").GetChildren())
        {
            if (!TryNonEmptyGuid(companySection.Key, out var companyId))
            {
                errors.Add($"Sync:EffectivePolicy:Branches:{companySection.Key} must use a non-empty company UUID key.");
                continue;
            }
            foreach (var branchSection in companySection.GetChildren())
            {
                if (!TryNonEmptyGuid(branchSection.Key, out var branchId))
                {
                    errors.Add($"Sync:EffectivePolicy:Branches:{companySection.Key}:{branchSection.Key} must use a non-empty branch UUID key.");
                    continue;
                }
                branches[(companyId, branchId)] = ReadRestriction(branchSection);
            }
        }

        foreach (var deviceSection in root.GetSection("Devices").GetChildren())
        {
            if (!TryNonEmptyGuid(deviceSection.Key, out var registeredDeviceId) ||
                !TryNonEmptyGuid(deviceSection["CompanyId"], out var companyId) ||
                !TryNonEmptyGuid(deviceSection["BranchId"], out var branchId))
            {
                errors.Add($"Sync:EffectivePolicy:Devices:{deviceSection.Key} requires non-empty device, company and branch UUIDs.");
                continue;
            }
            var deviceId = deviceSection["DeviceId"];
            var actions = deviceSection.GetSection("AllowedActions").Get<string[]>() ?? [];
            if (string.IsNullOrWhiteSpace(deviceId) || deviceId != deviceId.Trim() || deviceId.Length > 200 ||
                actions.Length == 0 || actions.Any(string.IsNullOrWhiteSpace) ||
                actions.Distinct(StringComparer.Ordinal).Count() != actions.Length)
            {
                errors.Add($"Sync:EffectivePolicy:Devices:{deviceSection.Key} requires DeviceId and a non-empty unique AllowedActions set.");
                continue;
            }
            devices[registeredDeviceId] = new ConfiguredDeviceSyncPolicy(
                registeredDeviceId, companyId, branchId, deviceId,
                actions.ToHashSet(StringComparer.Ordinal), ReadRestriction(deviceSection));
        }

        return new EffectivePolicyConfiguration(companies, branches, devices, errors);
    }

    public static EffectivePolicyConfiguration Create(
        IReadOnlyDictionary<Guid, SyncPolicyRestriction> companies,
        IReadOnlyDictionary<(Guid CompanyId, Guid BranchId), SyncPolicyRestriction> branches,
        IReadOnlyCollection<ConfiguredDeviceSyncPolicy> devices)
        => new(
            new Dictionary<Guid, SyncPolicyRestriction>(companies),
            new Dictionary<(Guid CompanyId, Guid BranchId), SyncPolicyRestriction>(branches),
            devices.ToDictionary(x => x.RegisteredDeviceId),
            []);

    private static SyncPolicyRestriction ReadRestriction(IConfiguration section) => new(
        section.GetValue<bool?>(nameof(SyncPolicyRestriction.Enabled)),
        section.GetSection(nameof(SyncPolicyRestriction.AllowedActions)).Get<string[]>(),
        section.GetSection(nameof(SyncPolicyRestriction.AllowedProtocolVersions)).Get<string[]>(),
        section.GetValue<int?>(nameof(SyncPolicyRestriction.MaxBatchOperations)),
        section.GetValue<int?>(nameof(SyncPolicyRestriction.MaximumRequestBodyBytes)),
        section.GetValue<int?>(nameof(SyncPolicyRestriction.MaximumPayloadBytes)),
        section.GetValue<int?>(nameof(SyncPolicyRestriction.ClientTransportMaxRetryCount)),
        section.GetValue<int?>(nameof(SyncPolicyRestriction.ServerExecutionMaxRetryCount)),
        section.GetValue<int?>(nameof(SyncPolicyRestriction.ClientTransportBaseSeconds)),
        section.GetValue<int?>(nameof(SyncPolicyRestriction.ServerExecutionBaseSeconds)),
        section.GetValue<int?>(nameof(SyncPolicyRestriction.ClientTransportMaxDelayMinutes)),
        section.GetValue<int?>(nameof(SyncPolicyRestriction.ServerExecutionMaxDelayMinutes)),
        section.GetValue<int?>(nameof(SyncPolicyRestriction.LocalSuccessHours)),
        section.GetValue<int?>(nameof(SyncPolicyRestriction.LocalRejectedDays)),
        section.GetValue<int?>(nameof(SyncPolicyRestriction.ServerPayloadDays)),
        section.GetValue<int?>(nameof(SyncPolicyRestriction.CacheMaxAgeHours)));

    private static SyncPolicyRestriction Freeze(SyncPolicyRestriction restriction)
        => restriction with
        {
            AllowedActions = restriction.AllowedActions?.ToFrozenSet(StringComparer.Ordinal),
            AllowedProtocolVersions = restriction.AllowedProtocolVersions?.ToFrozenSet(StringComparer.Ordinal)
        };

    private static bool TryNonEmptyGuid(string? value, out Guid parsed)
        => Guid.TryParse(value, out parsed) && parsed != Guid.Empty;
}

public sealed class EffectivePolicyConfigurationValidator(
    SyncEffectivePolicyResolver resolver,
    IOptions<SyncRuntimePolicyOptions> global)
{
    public ValidateOptionsResult Validate(EffectivePolicyConfiguration configuration)
    {
        var errors = configuration.LoadErrors.ToList();
        var allActions = global.Value.AllowedActions;

        foreach (var company in configuration.Companies)
        {
            var effective = resolver.Resolve(company.Value, null, allActions, allActions);
            if (effective.ClosedReason == "INVALID_SCOPE_OVERRIDE")
                errors.Add($"Company {company.Key:D} sync policy widens or invalidates the global ceiling.");
        }

        foreach (var branch in configuration.Branches)
        {
            if (!configuration.TryGetCompany(branch.Key.CompanyId, out var company))
            {
                errors.Add($"Branch {branch.Key.BranchId:D} has no configured company policy.");
                continue;
            }
            var effective = resolver.Resolve(company, branch.Value, allActions, allActions);
            if (effective.ClosedReason == "INVALID_SCOPE_OVERRIDE")
                errors.Add($"Branch {branch.Key.BranchId:D} sync policy widens or invalidates its company ceiling.");
        }

        foreach (var device in configuration.Devices)
        {
            if (!configuration.TryGetCompany(device.CompanyId, out var company) ||
                !configuration.TryGetBranch(device.CompanyId, device.BranchId, out var branch))
            {
                errors.Add($"Device {device.RegisteredDeviceId:D} has no complete configured company/branch policy chain.");
                continue;
            }
            var effective = resolver.Resolve(
                company, branch, device.AllowedActions, allActions, device.Restriction);
            var branchEffective = resolver.Resolve(company, branch, allActions, allActions);
            if (effective.ClosedReason == "INVALID_SCOPE_OVERRIDE" ||
                device.AllowedActions.Any(x => !branchEffective.AllowedActions.Contains(x)))
                errors.Add($"Device {device.RegisteredDeviceId:D} action policy contains an action outside its branch ceiling.");
        }

        return errors.Count == 0 ? ValidateOptionsResult.Success : ValidateOptionsResult.Fail(errors);
    }
}

public interface IEffectiveSyncPolicyProvider
{
    Task<EffectiveSyncPolicy> ResolveAsync(
        CurrentSecurityContext current,
        CancellationToken cancellationToken = default);
}

public sealed class EffectiveSyncPolicyProvider(
    EffectivePolicyConfiguration configuration,
    SyncEffectivePolicyResolver resolver,
    IEffectivePermissionResolver permissions) : IEffectiveSyncPolicyProvider
{
    public async Task<EffectiveSyncPolicy> ResolveAsync(
        CurrentSecurityContext current,
        CancellationToken cancellationToken = default)
    {
        if (!current.IsLocalSession || !current.BranchId.HasValue ||
            !current.RegisteredDeviceId.HasValue || string.IsNullOrEmpty(current.DeviceId))
            return resolver.Close("SYNC_SECURITY_CONTEXT_INCOMPLETE");
        if (!configuration.TryGetCompany(current.CompanyId, out var company) ||
            !configuration.TryGetBranch(current.CompanyId, current.BranchId.Value, out var branch) ||
            !configuration.TryGetDevice(current.RegisteredDeviceId.Value, out var device))
            return resolver.Close("SYNC_SCOPE_POLICY_MISSING");
        if (device.CompanyId != current.CompanyId || device.BranchId != current.BranchId ||
            !string.Equals(device.DeviceId, current.DeviceId, StringComparison.Ordinal))
            return resolver.Close("SYNC_DEVICE_POLICY_MISMATCH");

        var permissionActions = new HashSet<string>(StringComparer.Ordinal);
        foreach (var definition in SyncActionCatalog.Definitions)
            if (await permissions.HasPermissionAsync(
                    current.UserId, current.CompanyId, current.BranchId,
                    definition.RequiredPermission, cancellationToken))
                permissionActions.Add(definition.ActionCodeValue);

        return resolver.Resolve(
            company, branch, device.AllowedActions, permissionActions, device.Restriction);
    }
}

/// <summary>
/// Worker-side projection of the same immutable hierarchy used by HTTP. Retry
/// numbers are resolved again after restart from operation scope and device
/// identity; no process-local snapshot is required for correctness.
/// </summary>
public sealed class EffectiveSyncRetryPolicyResolver(
    EffectivePolicyConfiguration configuration,
    SyncEffectivePolicyResolver resolver,
    IOptions<SyncRuntimePolicyOptions> global) : ISyncRetryPolicyResolver
{
    public ValueTask<SyncRetryPolicy?> ResolveAsync(
        Guid companyId,
        Guid? branchId,
        Guid? registeredDeviceId,
        string? deviceId,
        CancellationToken cancellationToken = default)
    {
        if (!branchId.HasValue || !registeredDeviceId.HasValue || string.IsNullOrEmpty(deviceId) ||
            !configuration.TryGetCompany(companyId, out var company) ||
            !configuration.TryGetBranch(companyId, branchId.Value, out var branch) ||
            !configuration.TryGetDevice(registeredDeviceId.Value, out var device) ||
            device.CompanyId != companyId || device.BranchId != branchId ||
            !string.Equals(device.DeviceId, deviceId, StringComparison.Ordinal))
            return ValueTask.FromResult<SyncRetryPolicy?>(null);

        var globalActions = global.Value.AllowedActions;
        var effective = resolver.Resolve(
            company, branch, device.AllowedActions, globalActions, device.Restriction);
        if (effective.ClosedReason == "INVALID_SCOPE_OVERRIDE")
            return ValueTask.FromResult<SyncRetryPolicy?>(null);
        return ValueTask.FromResult<SyncRetryPolicy?>(new SyncRetryPolicy(
            effective.ServerExecutionMaxRetryCount,
            TimeSpan.FromSeconds(effective.ServerExecutionBaseSeconds),
            TimeSpan.FromMinutes(effective.ServerExecutionMaxDelayMinutes)).Validate());
    }
}

public sealed class EffectivePolicySyncRuntimeGate(IEffectiveSyncPolicyProvider provider) : ISyncRuntimeGate
{
    public Task<EffectiveSyncPolicy> ResolveAsync(
        CurrentSecurityContext current,
        CancellationToken cancellationToken)
        => provider.ResolveAsync(current, cancellationToken);
}
