using System.Collections.Frozen;
using System.Security.Cryptography;
using System.Text;
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
        IReadOnlyList<string> loadErrors,
        string sourceVersion,
        string sourceFingerprint)
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
        SourceVersion = sourceVersion;
        SourceFingerprint = sourceFingerprint;
    }

    public IReadOnlyList<string> LoadErrors { get; }
    public string SourceVersion { get; }
    public string SourceFingerprint { get; }
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
        var sourceVersion = root["SourceVersion"] ?? string.Empty;
        if (!IsSafeSourceVersion(sourceVersion))
            errors.Add("Sync:EffectivePolicy:SourceVersion must be 1..80 ASCII letters, digits, dot, dash or underscore.");

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

        return new EffectivePolicyConfiguration(
            companies, branches, devices, errors, sourceVersion, ComputeSourceFingerprint(configuration));
    }

    public static EffectivePolicyConfiguration Create(
        IReadOnlyDictionary<Guid, SyncPolicyRestriction> companies,
        IReadOnlyDictionary<(Guid CompanyId, Guid BranchId), SyncPolicyRestriction> branches,
        IReadOnlyCollection<ConfiguredDeviceSyncPolicy> devices,
        string sourceVersion = "test-policy-v1")
        => new(
            new Dictionary<Guid, SyncPolicyRestriction>(companies),
            new Dictionary<(Guid CompanyId, Guid BranchId), SyncPolicyRestriction>(branches),
            devices.ToDictionary(x => x.RegisteredDeviceId),
            [],
            sourceVersion,
            Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(sourceVersion))).ToLowerInvariant());

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

    private static bool IsSafeSourceVersion(string value) =>
        value.Length is >= 1 and <= 80 &&
        value.All(character => char.IsAsciiLetterOrDigit(character) || character is '-' or '_' or '.');

    private static string ComputeSourceFingerprint(IConfiguration configuration)
    {
        var canonical = string.Join('\n', configuration.AsEnumerable()
            .Where(item => item.Value is not null && item.Key.StartsWith("Sync:", StringComparison.Ordinal))
            .OrderBy(item => item.Key, StringComparer.Ordinal)
            .Select(item => $"{item.Key}={item.Value}"));
        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(canonical))).ToLowerInvariant();
    }
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
            SyncPolicyRestriction? company = configuration.TryGetCompany(
                branch.Key.CompanyId, out var configuredCompany) ? configuredCompany : null;
            var effective = resolver.Resolve(company, branch.Value, allActions, allActions);
            if (effective.ClosedReason == "INVALID_SCOPE_OVERRIDE")
                errors.Add($"Branch {branch.Key.BranchId:D} sync policy widens or invalidates its company ceiling.");
        }

        foreach (var device in configuration.Devices)
        {
            SyncPolicyRestriction? company = configuration.TryGetCompany(
                device.CompanyId, out var configuredCompany) ? configuredCompany : null;
            SyncPolicyRestriction? branch = configuration.TryGetBranch(
                device.CompanyId, device.BranchId, out var configuredBranch) ? configuredBranch : null;
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
            return Stamp(resolver.Close("SYNC_SECURITY_CONTEXT_INCOMPLETE"));
        if (!configuration.TryGetDevice(current.RegisteredDeviceId.Value, out var device))
            return Stamp(resolver.Close("SYNC_SCOPE_POLICY_MISSING"));
        SyncPolicyRestriction? company = configuration.TryGetCompany(
            current.CompanyId, out var configuredCompany) ? configuredCompany : null;
        SyncPolicyRestriction? branch = configuration.TryGetBranch(
            current.CompanyId, current.BranchId.Value, out var configuredBranch) ? configuredBranch : null;
        if (device.CompanyId != current.CompanyId || device.BranchId != current.BranchId ||
            !string.Equals(device.DeviceId, current.DeviceId, StringComparison.Ordinal))
            return Stamp(resolver.Close("SYNC_DEVICE_POLICY_MISMATCH"));

        var permissionActions = new HashSet<string>(StringComparer.Ordinal);
        foreach (var definition in SyncActionCatalog.Definitions)
            if (await permissions.HasPermissionAsync(
                    current.UserId, current.CompanyId, current.BranchId,
                    definition.RequiredPermission, cancellationToken))
                permissionActions.Add(definition.ActionCodeValue);

        return Stamp(resolver.Resolve(
            company, branch, device.AllowedActions, permissionActions, device.Restriction));
    }

    private EffectiveSyncPolicy Stamp(EffectiveSyncPolicy policy) => policy with
    {
        SourceVersion = configuration.SourceVersion,
        SourceFingerprint = configuration.SourceFingerprint
    };
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
            !configuration.TryGetDevice(registeredDeviceId.Value, out var device) ||
            device.CompanyId != companyId || device.BranchId != branchId ||
            !string.Equals(device.DeviceId, deviceId, StringComparison.Ordinal))
            return ValueTask.FromResult<SyncRetryPolicy?>(null);
        SyncPolicyRestriction? company = configuration.TryGetCompany(
            companyId, out var configuredCompany) ? configuredCompany : null;
        SyncPolicyRestriction? branch = configuration.TryGetBranch(
            companyId, branchId.Value, out var configuredBranch) ? configuredBranch : null;

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

    public ValueTask<SyncExecutionPolicyDecision> AuthorizeExecutionAsync(
        Guid companyId,
        Guid? branchId,
        Guid? registeredDeviceId,
        string? deviceId,
        string actionCode,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(actionCode) || !branchId.HasValue || !registeredDeviceId.HasValue ||
            string.IsNullOrEmpty(deviceId) ||
            !configuration.TryGetDevice(registeredDeviceId.Value, out var device) ||
            device.CompanyId != companyId || device.BranchId != branchId ||
            !string.Equals(device.DeviceId, deviceId, StringComparison.Ordinal))
            return ValueTask.FromResult(Stamp(
                SyncExecutionPolicyDecision.Denied("SYNC_RUNTIME_POLICY_UNAVAILABLE")));
        SyncPolicyRestriction? company = configuration.TryGetCompany(
            companyId, out var configuredCompany) ? configuredCompany : null;
        SyncPolicyRestriction? branch = configuration.TryGetBranch(
            companyId, branchId.Value, out var configuredBranch) ? configuredBranch : null;

        // The business executor performs the final live permission check. This worker-side
        // decision re-applies every immutable hierarchy restriction immediately before claim,
        // so a restart or policy tightening cannot execute an action accepted under stale policy.
        var globalActions = global.Value.AllowedActions;
        var effective = resolver.Resolve(
            company, branch, device.AllowedActions, globalActions, device.Restriction);
        if (!effective.Enabled)
            return ValueTask.FromResult(Stamp(SyncExecutionPolicyDecision.Denied(
                effective.ClosedReason ?? "OFFLINE_DISABLED")));
        return ValueTask.FromResult(Stamp(effective.AllowedActions.Contains(actionCode)
            ? SyncExecutionPolicyDecision.Allowed
            : SyncExecutionPolicyDecision.Denied("SCOPE_DENIED")));
    }

    private SyncExecutionPolicyDecision Stamp(SyncExecutionPolicyDecision decision) =>
        decision.WithPolicySource(configuration.SourceVersion, configuration.SourceFingerprint);
}

/// <summary>
/// Retention projection of the same immutable effective-policy source used by
/// HTTP acceptance and worker execution. It deliberately resolves while the
/// Offline gate is closed: retention is a server data-lifecycle obligation,
/// not permission to execute offline writes.
/// </summary>
public sealed class EffectiveSyncRetentionPolicyProvider(
    EffectivePolicyConfiguration configuration,
    SyncEffectivePolicyResolver resolver,
    IOptions<SyncRuntimePolicyOptions> global) : IEffectiveSyncRetentionPolicyProvider
{
    public ValueTask<EffectiveSyncRetentionPolicy?> ResolveAsync(
        Guid companyId,
        Guid? branchId,
        Guid? registeredDeviceId,
        string? deviceId,
        CancellationToken cancellationToken = default)
    {
        if (companyId == Guid.Empty || !branchId.HasValue || branchId == Guid.Empty ||
            !registeredDeviceId.HasValue || registeredDeviceId == Guid.Empty ||
            string.IsNullOrWhiteSpace(deviceId))
            return ValueTask.FromResult<EffectiveSyncRetentionPolicy?>(null);

        SyncPolicyRestriction? company = configuration.TryGetCompany(
            companyId, out var configuredCompany) ? configuredCompany : null;
        SyncPolicyRestriction? branch = configuration.TryGetBranch(
            companyId, branchId.Value, out var configuredBranch) ? configuredBranch : null;
        var globalActions = global.Value.AllowedActions;
        IReadOnlyCollection<string> deviceActions = globalActions;
        SyncPolicyRestriction? deviceRestriction = null;
        if (configuration.TryGetDevice(registeredDeviceId.Value, out var device))
        {
            if (device.CompanyId != companyId || device.BranchId != branchId ||
                !string.Equals(device.DeviceId, deviceId, StringComparison.Ordinal))
                return ValueTask.FromResult<EffectiveSyncRetentionPolicy?>(null);
            deviceActions = device.AllowedActions;
            deviceRestriction = device.Restriction;
        }
        var effective = resolver.Resolve(
            company, branch, deviceActions, globalActions, deviceRestriction);
        if (effective.ClosedReason == "INVALID_SCOPE_OVERRIDE" || effective.ServerPayloadDays < 1)
            return ValueTask.FromResult<EffectiveSyncRetentionPolicy?>(null);

        return ValueTask.FromResult<EffectiveSyncRetentionPolicy?>(new(
            effective.ServerPayloadDays,
            configuration.SourceVersion,
            configuration.SourceFingerprint));
    }
}

public sealed class EffectivePolicySyncRuntimeGate(IEffectiveSyncPolicyProvider provider) : ISyncRuntimeGate
{
    public Task<EffectiveSyncPolicy> ResolveAsync(
        CurrentSecurityContext current,
        CancellationToken cancellationToken)
        => provider.ResolveAsync(current, cancellationToken);
}
