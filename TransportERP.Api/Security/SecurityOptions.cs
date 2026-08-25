using Microsoft.Extensions.Options;

namespace TransportERP.Api.Security;

public enum TransportAuthMode { LocalSessions, ExternalAuthority }

public sealed class TransportSecurityOptions
{
    public const string SectionName = "Auth";
    public TransportAuthMode Mode { get; set; } = TransportAuthMode.LocalSessions;
    public string? Authority { get; set; }
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string SigningKey { get; set; } = string.Empty;
    public string SigningKeyId { get; set; } = string.Empty;
    public Dictionary<string, string> PreviousSigningKeys { get; set; } = new(StringComparer.Ordinal);
    public int AccessTokenMinutes { get; set; } = 15;
    public int RefreshTokenDays { get; set; } = 30;
    public int MaxFailures { get; set; } = 5;
    public int LockoutMinutes { get; set; } = 15;
    public int LoginRateLimitPermitCount { get; set; } = 10;
    public int RefreshRateLimitPermitCount { get; set; } = 20;
    public int RateLimitWindowSeconds { get; set; } = 60;
    public string RateLimiterMode { get; set; } = "SingleNode";
    public int ApplicationInstanceCount { get; set; } = 1;
}

public sealed class TransportSecurityOptionsValidator : IValidateOptions<TransportSecurityOptions>
{
    public ValidateOptionsResult Validate(string? name, TransportSecurityOptions value)
    {
        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(value.Audience)) errors.Add("Auth:Audience is required.");
        if (value.AccessTokenMinutes is < 1 or > 120) errors.Add("Auth:AccessTokenMinutes must be between 1 and 120.");
        if (value.RefreshTokenDays is < 1 or > 365) errors.Add("Auth:RefreshTokenDays must be between 1 and 365.");
        if (value.MaxFailures is < 1 or > 100) errors.Add("Auth:MaxFailures must be between 1 and 100.");
        if (value.LockoutMinutes is < 1 or > 1440) errors.Add("Auth:LockoutMinutes must be between 1 and 1440.");
        if (value.LoginRateLimitPermitCount is < 1 or > 1000) errors.Add("Auth:LoginRateLimitPermitCount must be between 1 and 1000.");
        if (value.RefreshRateLimitPermitCount is < 1 or > 1000) errors.Add("Auth:RefreshRateLimitPermitCount must be between 1 and 1000.");
        if (value.RateLimitWindowSeconds is < 1 or > 3600) errors.Add("Auth:RateLimitWindowSeconds must be between 1 and 3600.");
        if (!string.Equals(value.RateLimiterMode, "SingleNode", StringComparison.Ordinal))
            errors.Add("Auth:RateLimiterMode supports only SingleNode until a distributed limiter is configured.");
        if (value.ApplicationInstanceCount != 1)
            errors.Add("Auth:ApplicationInstanceCount must be 1 while Auth:RateLimiterMode is SingleNode.");
        if (value.Mode == TransportAuthMode.LocalSessions)
        {
            if (string.IsNullOrWhiteSpace(value.Issuer)) errors.Add("Auth:Issuer is required for LocalSessions.");
            if (string.IsNullOrWhiteSpace(value.SigningKey) || value.SigningKey.Length < 32)
                errors.Add("Auth:SigningKey must contain at least 32 characters for LocalSessions.");
            if (string.IsNullOrWhiteSpace(value.SigningKeyId) || value.SigningKeyId.Length > 80)
                errors.Add("Auth:SigningKeyId is required and must be at most 80 characters for LocalSessions.");
            if (value.PreviousSigningKeys.Any(x => string.IsNullOrWhiteSpace(x.Key) || x.Key.Length > 80 || string.IsNullOrWhiteSpace(x.Value) || x.Value.Length < 32))
                errors.Add("Every Auth:PreviousSigningKeys entry needs a non-empty key id and a key of at least 32 characters.");
            if (value.PreviousSigningKeys.ContainsKey(value.SigningKeyId))
                errors.Add("Auth:SigningKeyId cannot also exist in Auth:PreviousSigningKeys.");
            if (!string.IsNullOrWhiteSpace(value.Authority)) errors.Add("Auth:Authority cannot be combined with LocalSessions.");
        }
        else
        {
            if (string.IsNullOrWhiteSpace(value.Authority)) errors.Add("Auth:Authority is required for ExternalAuthority.");
            if (!string.IsNullOrWhiteSpace(value.SigningKey)) errors.Add("Auth:SigningKey cannot be combined with ExternalAuthority.");
            if (!string.IsNullOrWhiteSpace(value.SigningKeyId)) errors.Add("Auth:SigningKeyId cannot be combined with ExternalAuthority.");
            if (value.PreviousSigningKeys.Count > 0) errors.Add("Auth:PreviousSigningKeys cannot be combined with ExternalAuthority.");
        }
        return errors.Count == 0 ? ValidateOptionsResult.Success : ValidateOptionsResult.Fail(errors);
    }
}
