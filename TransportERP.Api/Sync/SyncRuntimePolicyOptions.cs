using Microsoft.Extensions.Options;
using TransportERP.Application.Sync;

namespace TransportERP.Api.Sync;

public sealed class SyncRuntimePolicyOptions
{
    public bool? OfflineEnabled { get; init; }
    public string? OfflineActivationDecisionId { get; init; }
    public string? OfflineActivationImplementationSha { get; init; }
    public BuildIdentityV1[] OfflineAuthorizedBuilds { get; init; } = [];
    public bool? ServerExecutionEnabled { get; init; }
    public string[] AllowedActions { get; init; } = [];
    public string[] AllowedProtocolVersions { get; init; } = [];
    public int? ClientTransportMaxRetryCount { get; init; }
    public int? ClientTransportBaseSeconds { get; init; }
    public int? ClientTransportMaxDelayMinutes { get; init; }
    public int? ServerExecutionMaxRetryCount { get; init; }
    public int? ServerExecutionBaseSeconds { get; init; }
    public int? ServerExecutionMaxDelayMinutes { get; init; }
    public int? MaxBatchOperations { get; init; }
    public bool? ConflictAutoMerge { get; init; }
    public int? LocalSuccessHours { get; init; }
    public int? LocalRejectedDays { get; init; }
    public int? ServerPayloadDays { get; init; }
    public int? CacheMaxAgeHours { get; init; }
    public int? MaximumRequestBodyBytes { get; init; }
    public int? MaximumPayloadBytes { get; init; }

    public static SyncRuntimePolicyOptions Load(IConfiguration configuration) => new()
    {
        OfflineEnabled = configuration.GetValue<bool?>("Sync:Offline:Enabled"),
        OfflineActivationDecisionId = configuration["Sync:Offline:ActivationDecisionId"],
        OfflineActivationImplementationSha = configuration["Sync:Offline:ActivationImplementationSha"],
        OfflineAuthorizedBuilds = configuration.GetSection("Sync:Offline:AuthorizedBuilds")
            .Get<BuildIdentityV1[]>() ?? [],
        ServerExecutionEnabled = configuration.GetValue<bool?>("Sync:ServerExecution:Enabled"),
        AllowedActions = configuration.GetSection("Sync:Offline:AllowedActions").Get<string[]>() ?? [],
        AllowedProtocolVersions = configuration.GetSection("Sync:Protocol:AllowedVersions").Get<string[]>() ?? [],
        ClientTransportMaxRetryCount = configuration.GetValue<int?>("Sync:Retry:ClientTransport:MaxCount"),
        ClientTransportBaseSeconds = configuration.GetValue<int?>("Sync:Retry:ClientTransport:BaseSeconds"),
        ClientTransportMaxDelayMinutes = configuration.GetValue<int?>("Sync:Retry:ClientTransport:MaxDelayMinutes"),
        ServerExecutionMaxRetryCount = configuration.GetValue<int?>("Sync:Retry:ServerExecution:MaxCount"),
        ServerExecutionBaseSeconds = configuration.GetValue<int?>("Sync:Retry:ServerExecution:BaseSeconds"),
        ServerExecutionMaxDelayMinutes = configuration.GetValue<int?>("Sync:Retry:ServerExecution:MaxDelayMinutes"),
        MaxBatchOperations = configuration.GetValue<int?>("Sync:Batch:MaxOperations"),
        ConflictAutoMerge = configuration.GetValue<bool?>("Sync:Conflict:AutoMerge"),
        LocalSuccessHours = configuration.GetValue<int?>("Sync:Retention:LocalSuccessHours"),
        LocalRejectedDays = configuration.GetValue<int?>("Sync:Retention:LocalRejectedDays"),
        ServerPayloadDays = configuration.GetValue<int?>("Sync:Retention:ServerPayloadDays"),
        CacheMaxAgeHours = configuration.GetValue<int?>("Sync:Cache:MaxAgeHours"),
        MaximumRequestBodyBytes = configuration.GetValue<int?>("Sync:Proof:MaximumRequestBodyBytes"),
        MaximumPayloadBytes = configuration.GetValue<int?>("Sync:Proof:MaximumPayloadBytes")
    };
}

/// <summary>
/// Validates the fixed global ceiling. Offline is closed by default and can only be
/// opened by an explicit, traceable G5 deployment decision bound to an exact commit.
/// </summary>
public sealed class SyncRuntimePolicyOptionsValidator : IValidateOptions<SyncRuntimePolicyOptions>
{
    private static readonly HashSet<string> ContractActions = SyncActionCatalog.Definitions
        .Select(x => x.ActionCodeValue).ToHashSet(StringComparer.Ordinal);

    public ValidateOptionsResult Validate(string? name, SyncRuntimePolicyOptions options)
    {
        var errors = new List<string>();
        if (!options.OfflineEnabled.HasValue)
            errors.Add("Sync:Offline:Enabled must be explicitly configured.");
        if (!options.ServerExecutionEnabled.HasValue)
            errors.Add("Sync:ServerExecution:Enabled must be explicitly configured.");
        ValidateOfflineActivation(options, errors);
        ValidateExactSet(options.AllowedProtocolVersions, ["sync-v1"], "Sync:Protocol:AllowedVersions", errors);
        ValidateActions(options.AllowedActions, errors);
        ValidateRange(options.ClientTransportMaxRetryCount, 0, 5, "Sync:Retry:ClientTransport:MaxCount", errors);
        ValidateExact(options.ClientTransportBaseSeconds, 5, "Sync:Retry:ClientTransport:BaseSeconds", errors);
        ValidateExact(options.ClientTransportMaxDelayMinutes, 30, "Sync:Retry:ClientTransport:MaxDelayMinutes", errors);
        ValidateRange(options.ServerExecutionMaxRetryCount, 0, 5, "Sync:Retry:ServerExecution:MaxCount", errors);
        ValidateExact(options.ServerExecutionBaseSeconds, 5, "Sync:Retry:ServerExecution:BaseSeconds", errors);
        ValidateExact(options.ServerExecutionMaxDelayMinutes, 30, "Sync:Retry:ServerExecution:MaxDelayMinutes", errors);
        ValidateExact(options.MaxBatchOperations, SyncApiModule.MaximumBatchOperations,
            "Sync:Batch:MaxOperations", errors);
        if (options.ConflictAutoMerge is not false)
            errors.Add("Sync:Conflict:AutoMerge must be explicitly false.");
        ValidateExact(options.LocalSuccessHours, 24, "Sync:Retention:LocalSuccessHours", errors);
        ValidateExact(options.LocalRejectedDays, 7, "Sync:Retention:LocalRejectedDays", errors);
        ValidateExact(options.ServerPayloadDays, 90, "Sync:Retention:ServerPayloadDays", errors);
        ValidateExact(options.CacheMaxAgeHours, 24, "Sync:Cache:MaxAgeHours", errors);
        ValidateExact(options.MaximumRequestBodyBytes, SyncApiModule.MaximumRequestBodyBytes,
            "Sync:Proof:MaximumRequestBodyBytes", errors);
        ValidateExact(options.MaximumPayloadBytes, SyncApiModule.MaximumPayloadBytes,
            "Sync:Proof:MaximumPayloadBytes", errors);
        if (options.MaximumPayloadBytes.HasValue && options.MaximumRequestBodyBytes.HasValue &&
            options.MaximumPayloadBytes > options.MaximumRequestBodyBytes)
            errors.Add("Sync payload limit cannot exceed the request body limit.");
        return errors.Count == 0 ? ValidateOptionsResult.Success : ValidateOptionsResult.Fail(errors);
    }

    private static void ValidateOfflineActivation(
        SyncRuntimePolicyOptions options,
        ICollection<string> errors)
    {
        var decisionId = options.OfflineActivationDecisionId;
        var implementationSha = options.OfflineActivationImplementationSha;
        var authorizedBuilds = options.OfflineAuthorizedBuilds ?? [];
        if (options.OfflineEnabled is not true)
        {
            if (!string.IsNullOrWhiteSpace(decisionId) || !string.IsNullOrWhiteSpace(implementationSha) ||
                authorizedBuilds.Length != 0)
                errors.Add("Offline activation evidence must be absent while Sync:Offline:Enabled is false.");
            return;
        }

        if (options.ServerExecutionEnabled is not true)
            errors.Add("Sync:ServerExecution:Enabled must be true before Offline can be activated.");
        if (!IsSafeDecisionId(decisionId))
            errors.Add("Sync:Offline:ActivationDecisionId must be an explicit safe G5 decision identifier.");
        if (!IsExactCommitSha(implementationSha))
            errors.Add("Sync:Offline:ActivationImplementationSha must bind G5 activation to an exact 40-character commit SHA.");
        if (authorizedBuilds.Length == 0 ||
            authorizedBuilds.Any(identity => identity is not { IsValid: true }) ||
            authorizedBuilds.Select(identity => identity.Platform)
                .Distinct(StringComparer.Ordinal).Count() != authorizedBuilds.Length)
            errors.Add("Sync:Offline:AuthorizedBuilds must contain one valid exact identity per approved platform.");
    }

    private static bool IsSafeDecisionId(string? value) =>
        value is { Length: >= 8 and <= 120 } &&
        value.StartsWith("DEC-G5-", StringComparison.Ordinal) &&
        value.All(character => char.IsAsciiLetterOrDigit(character) || character is '-' or '_' or '.');

    private static bool IsExactCommitSha(string? value) =>
        value is { Length: 40 } && value.All(character => character is >= '0' and <= '9' or >= 'a' and <= 'f');

    private static void ValidateActions(string[] values, ICollection<string> errors)
    {
        if (values.Length == 0 || values.Any(string.IsNullOrWhiteSpace) ||
            values.Distinct(StringComparer.Ordinal).Count() != values.Length ||
            values.Any(x => !ContractActions.Contains(x)))
            errors.Add("Sync:Offline:AllowedActions must be a non-empty, unique subset of the typed sync action catalog.");
    }

    private static void ValidateExactSet(string[] actual, string[] expected, string key, ICollection<string> errors)
    {
        if (actual.Length != expected.Length || !actual.SequenceEqual(expected, StringComparer.Ordinal))
            errors.Add($"{key} must contain only sync-v1.");
    }

    private static void ValidateExact(int? actual, int expected, string key, ICollection<string> errors)
    {
        if (actual != expected) errors.Add($"{key} must be explicitly set to {expected}.");
    }

    private static void ValidateRange(int? actual, int minimum, int maximum, string key, ICollection<string> errors)
    {
        if (actual is null || actual < minimum || actual > maximum)
            errors.Add($"{key} must be between {minimum} and {maximum}.");
    }
}

public sealed record SyncPolicyRestriction(
    bool? Enabled = null,
    IReadOnlyCollection<string>? AllowedActions = null,
    IReadOnlyCollection<string>? AllowedProtocolVersions = null,
    int? MaxBatchOperations = null,
    int? MaximumRequestBodyBytes = null,
    int? MaximumPayloadBytes = null,
    int? ClientTransportMaxRetryCount = null,
    int? ServerExecutionMaxRetryCount = null,
    int? ClientTransportBaseSeconds = null,
    int? ServerExecutionBaseSeconds = null,
    int? ClientTransportMaxDelayMinutes = null,
    int? ServerExecutionMaxDelayMinutes = null,
    int? LocalSuccessHours = null,
    int? LocalRejectedDays = null,
    int? ServerPayloadDays = null,
    int? CacheMaxAgeHours = null);

public sealed record EffectiveSyncPolicy(
    bool Enabled,
    IReadOnlySet<string> AllowedActions,
    IReadOnlySet<string> AllowedProtocolVersions,
    int MaxBatchOperations,
    int MaximumRequestBodyBytes,
    int MaximumPayloadBytes,
    int ClientTransportMaxRetryCount,
    int ServerExecutionMaxRetryCount,
    int ClientTransportBaseSeconds,
    int ServerExecutionBaseSeconds,
    int ClientTransportMaxDelayMinutes,
    int ServerExecutionMaxDelayMinutes,
    int LocalSuccessHours,
    int LocalRejectedDays,
    int ServerPayloadDays,
    int CacheMaxAgeHours,
    string? ClosedReason,
    string? SourceVersion = null,
    string? SourceFingerprint = null);

/// <summary>
/// Resolves the fixed global ceiling through company and branch restrictions.
/// The runtime provider supplies the immutable configured restrictions and the
/// mandatory device and current-permission action intersections.
/// </summary>
public sealed class SyncEffectivePolicyResolver(IOptions<SyncRuntimePolicyOptions> configured)
{
    public EffectiveSyncPolicy Resolve(
        SyncPolicyRestriction? company,
        SyncPolicyRestriction? branch,
        IReadOnlyCollection<string>? deviceAllowedActions,
        IReadOnlyCollection<string>? permissionAllowedActions,
        SyncPolicyRestriction? device = null)
    {
        var global = configured.Value;
        var effective = new MutablePolicy(
            global.OfflineEnabled == true,
            global.AllowedActions.ToHashSet(StringComparer.Ordinal),
            global.AllowedProtocolVersions.ToHashSet(StringComparer.Ordinal),
            global.MaxBatchOperations ?? 0,
            global.MaximumRequestBodyBytes ?? 0,
            global.MaximumPayloadBytes ?? 0,
            global.ClientTransportMaxRetryCount ?? 0,
            global.ServerExecutionMaxRetryCount ?? 0,
            global.ClientTransportBaseSeconds ?? 0,
            global.ServerExecutionBaseSeconds ?? 0,
            global.ClientTransportMaxDelayMinutes ?? 0,
            global.ServerExecutionMaxDelayMinutes ?? 0,
            global.LocalSuccessHours ?? 0,
            global.LocalRejectedDays ?? 0,
            global.ServerPayloadDays ?? 0,
            global.CacheMaxAgeHours ?? 0);

        if (!ApplyRestriction(effective, company) || !ApplyRestriction(effective, branch) ||
            !ApplyRestriction(effective, device))
            return Close(effective, "INVALID_SCOPE_OVERRIDE");
        if (deviceAllowedActions is null || permissionAllowedActions is null)
            return Close(effective, "DEVICE_OR_PERMISSION_POLICY_MISSING");
        effective.AllowedActions.IntersectWith(deviceAllowedActions);
        effective.AllowedActions.IntersectWith(permissionAllowedActions);
        if (effective.AllowedActions.Count == 0 || effective.AllowedProtocolVersions.Count == 0)
            effective.Enabled = false;
        return Freeze(effective, effective.Enabled ? null : "OFFLINE_DISABLED");
    }

    public EffectiveSyncPolicy Close(string reason)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(reason);
        var global = configured.Value;
        return new EffectiveSyncPolicy(
            false,
            new HashSet<string>(StringComparer.Ordinal),
            global.AllowedProtocolVersions.ToHashSet(StringComparer.Ordinal),
            global.MaxBatchOperations ?? 0,
            global.MaximumRequestBodyBytes ?? 0,
            global.MaximumPayloadBytes ?? 0,
            global.ClientTransportMaxRetryCount ?? 0,
            global.ServerExecutionMaxRetryCount ?? 0,
            global.ClientTransportBaseSeconds ?? 0,
            global.ServerExecutionBaseSeconds ?? 0,
            global.ClientTransportMaxDelayMinutes ?? 0,
            global.ServerExecutionMaxDelayMinutes ?? 0,
            global.LocalSuccessHours ?? 0,
            global.LocalRejectedDays ?? 0,
            global.ServerPayloadDays ?? 0,
            global.CacheMaxAgeHours ?? 0,
            reason);
    }

    private static bool ApplyRestriction(MutablePolicy policy, SyncPolicyRestriction? restriction)
    {
        if (restriction is null) return true;
        if (restriction.Enabled.HasValue) policy.Enabled &= restriction.Enabled.Value;
        if (!TightenSet(policy.AllowedActions, restriction.AllowedActions) ||
            !TightenSet(policy.AllowedProtocolVersions, restriction.AllowedProtocolVersions)) return false;
        if (!TightenMaximum(ref policy.MaxBatchOperations, restriction.MaxBatchOperations, 1) ||
            !TightenMaximum(ref policy.MaximumRequestBodyBytes, restriction.MaximumRequestBodyBytes, 1) ||
            !TightenMaximum(ref policy.MaximumPayloadBytes, restriction.MaximumPayloadBytes, 1) ||
            !TightenMaximum(ref policy.ClientTransportMaxRetryCount, restriction.ClientTransportMaxRetryCount) ||
            !TightenMaximum(ref policy.ServerExecutionMaxRetryCount, restriction.ServerExecutionMaxRetryCount) ||
            !TightenMinimum(ref policy.ClientTransportBaseSeconds, restriction.ClientTransportBaseSeconds) ||
            !TightenMinimum(ref policy.ServerExecutionBaseSeconds, restriction.ServerExecutionBaseSeconds) ||
            !TightenMinimum(ref policy.ClientTransportMaxDelayMinutes, restriction.ClientTransportMaxDelayMinutes) ||
            !TightenMinimum(ref policy.ServerExecutionMaxDelayMinutes, restriction.ServerExecutionMaxDelayMinutes) ||
            !TightenMaximum(ref policy.LocalSuccessHours, restriction.LocalSuccessHours, 1) ||
            !TightenMaximum(ref policy.LocalRejectedDays, restriction.LocalRejectedDays, 1) ||
            !TightenMaximum(ref policy.ServerPayloadDays, restriction.ServerPayloadDays, 1) ||
            !TightenMaximum(ref policy.CacheMaxAgeHours, restriction.CacheMaxAgeHours, 1)) return false;
        if (policy.MaximumPayloadBytes > policy.MaximumRequestBodyBytes) return false;
        return true;
    }

    private static bool TightenSet(HashSet<string> current, IReadOnlyCollection<string>? candidate)
    {
        if (candidate is null) return true;
        if (candidate.Any(x => !current.Contains(x))) return false;
        current.IntersectWith(candidate);
        return true;
    }

    private static bool TightenMaximum(ref int current, int? candidate, int minimum = 0)
    {
        if (!candidate.HasValue) return true;
        if (candidate.Value < minimum || candidate.Value > current) return false;
        current = candidate.Value;
        return true;
    }

    private static bool TightenMinimum(ref int current, int? candidate)
    {
        if (!candidate.HasValue) return true;
        if (candidate.Value < current) return false;
        current = candidate.Value;
        return true;
    }

    private static EffectiveSyncPolicy Close(MutablePolicy policy, string reason)
    {
        policy.Enabled = false;
        policy.AllowedActions.Clear();
        return Freeze(policy, reason);
    }

    private static EffectiveSyncPolicy Freeze(MutablePolicy policy, string? reason) => new(
        policy.Enabled,
        policy.AllowedActions.ToHashSet(StringComparer.Ordinal),
        policy.AllowedProtocolVersions.ToHashSet(StringComparer.Ordinal),
        policy.MaxBatchOperations,
        policy.MaximumRequestBodyBytes,
        policy.MaximumPayloadBytes,
        policy.ClientTransportMaxRetryCount,
        policy.ServerExecutionMaxRetryCount,
        policy.ClientTransportBaseSeconds,
        policy.ServerExecutionBaseSeconds,
        policy.ClientTransportMaxDelayMinutes,
        policy.ServerExecutionMaxDelayMinutes,
        policy.LocalSuccessHours,
        policy.LocalRejectedDays,
        policy.ServerPayloadDays,
        policy.CacheMaxAgeHours,
        reason);

    private sealed class MutablePolicy(
        bool enabled,
        HashSet<string> allowedActions,
        HashSet<string> allowedProtocolVersions,
        int maxBatchOperations,
        int maximumRequestBodyBytes,
        int maximumPayloadBytes,
        int clientTransportMaxRetryCount,
        int serverExecutionMaxRetryCount,
        int clientTransportBaseSeconds,
        int serverExecutionBaseSeconds,
        int clientTransportMaxDelayMinutes,
        int serverExecutionMaxDelayMinutes,
        int localSuccessHours,
        int localRejectedDays,
        int serverPayloadDays,
        int cacheMaxAgeHours)
    {
        public bool Enabled { get; set; } = enabled;
        public HashSet<string> AllowedActions { get; } = allowedActions;
        public HashSet<string> AllowedProtocolVersions { get; } = allowedProtocolVersions;
        public int MaxBatchOperations = maxBatchOperations;
        public int MaximumRequestBodyBytes = maximumRequestBodyBytes;
        public int MaximumPayloadBytes = maximumPayloadBytes;
        public int ClientTransportMaxRetryCount = clientTransportMaxRetryCount;
        public int ServerExecutionMaxRetryCount = serverExecutionMaxRetryCount;
        public int ClientTransportBaseSeconds = clientTransportBaseSeconds;
        public int ServerExecutionBaseSeconds = serverExecutionBaseSeconds;
        public int ClientTransportMaxDelayMinutes = clientTransportMaxDelayMinutes;
        public int ServerExecutionMaxDelayMinutes = serverExecutionMaxDelayMinutes;
        public int LocalSuccessHours = localSuccessHours;
        public int LocalRejectedDays = localRejectedDays;
        public int ServerPayloadDays = serverPayloadDays;
        public int CacheMaxAgeHours = cacheMaxAgeHours;
    }
}
