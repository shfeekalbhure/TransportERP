using System.Reflection;
using Microsoft.Extensions.Options;
using TransportERP.Api.Security;
using TransportERP.Api.Sync;

namespace TransportERP.Tests;

public sealed class Stage5StartupOptionsValidationTests
{
    [Theory]
    [InlineData(
        "TransportERP.Api.Startup.SyncRuntimePolicyStartupOptionsValidationException",
        "Sync",
        typeof(SyncRuntimePolicyOptions))]
    [InlineData(
        "TransportERP.Api.Startup.EffectivePolicyStartupOptionsValidationException",
        "Sync:EffectivePolicy",
        typeof(EffectivePolicyConfiguration))]
    [InlineData(
        "TransportERP.Api.Startup.AuthStartupOptionsValidationException",
        "Auth",
        typeof(TransportSecurityOptions))]
    public void Stage_specific_startup_validation_preserves_the_options_exception_contract(
        string typeName,
        string expectedOptionsName,
        Type expectedOptionsType)
    {
        string[] failures = ["FIXED_FAILURE_ONE", "FIXED_FAILURE_TWO"];
        var assembly = typeof(SyncRuntimePolicyOptions).Assembly;
        var exceptionType = assembly.GetType(typeName, throwOnError: true)!;
        var constructor = Assert.Single(exceptionType.GetConstructors(
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));

        var exception = Assert.IsAssignableFrom<OptionsValidationException>(
            constructor.Invoke(new object?[] { failures }));

        Assert.Equal(expectedOptionsName, exception.OptionsName);
        Assert.Equal(expectedOptionsType, exception.OptionsType);
        Assert.Equal(failures, exception.Failures.ToArray());
    }
}
