namespace TransportERP.Contracts.Authentication;

public sealed record LoginRequest(string UserName, string Password);
public sealed record ScopeOption(Guid CompanyId, string CompanyName, Guid BranchId, string BranchName,
    Guid FiscalPeriodId, string FiscalPeriodName);
public sealed record LoginResponse(string LoginTicket, string DisplayName, List<ScopeOption> Scopes);
public sealed record SelectScopeRequest(string LoginTicket, Guid CompanyId, Guid BranchId, Guid FiscalPeriodId);
public sealed record SessionResponse(string AccessToken, DateTimeOffset ExpiresAt, string DisplayName, ScopeOption Scope);
public sealed record SelectReceiptScopeRequest(Guid BranchId);
