using Microsoft.Extensions.Options;
using TransportERP.Api.Security;
using TransportERP.Api.Sync;

namespace TransportERP.Api.Startup;

internal sealed class SyncRuntimePolicyStartupOptionsValidationException(
    IEnumerable<string> failures)
    : OptionsValidationException("Sync", typeof(SyncRuntimePolicyOptions), failures);

internal sealed class EffectivePolicyStartupOptionsValidationException(
    IEnumerable<string> failures)
    : OptionsValidationException(
        "Sync:EffectivePolicy", typeof(EffectivePolicyConfiguration), failures);

internal sealed class AuthStartupOptionsValidationException(
    IEnumerable<string> failures)
    : OptionsValidationException("Auth", typeof(TransportSecurityOptions), failures);
