using TransportERP.Contracts.Geo;

namespace TransportERP.Contracts.Org;

public sealed record OrgDto(Guid Id, string Code, string ArabicName, string EnglishName, bool IsActive, int Version, IReadOnlyDictionary<string, string?> Values);
public sealed record OrgWriteRequest(string Code, string ArabicName, string EnglishName, IReadOnlyDictionary<string, string?> Values, int? ExpectedVersion = null);
public sealed record OrgLifecycleRequest(int ExpectedVersion, string Reason);
public sealed record NumberReservationRequest(Guid SequenceId, string IdempotencyKey, string? Reason = null);
public sealed record NumberReservationDto(Guid Id, Guid SequenceId, ulong NumberValue, string RenderedNumber, string State);
public sealed record FiscalYearTransitionRequest(int ExpectedVersion, string Reason);
public static class OrgPermissions
{
    public const string CurrencyView = "GEN008.View"; public const string CurrencyManage = "GEN008.Manage";
    public const string ExchangeRateView = "GEN009.View"; public const string ExchangeRateManage = "GEN009.Manage";
    public const string CompanyView = "GEN010.View"; public const string CompanyManage = "GEN010.Manage";
    public const string BranchView = "GEN011.View"; public const string BranchManage = "GEN011.Manage";
    public const string FiscalYearView = "GEN012.View"; public const string FiscalYearManage = "GEN012.Manage";
    public const string NumberingView = "GEN013.View"; public const string NumberingManage = "GEN013.Manage"; public const string NumberingReserve = "GEN013.Reserve";
    public const string LanguageView = "GEN014.View"; public const string LanguageManage = "GEN014.Manage";
    public const string SettingsView = "GEN015.View"; public const string SettingsManage = "GEN015.Manage";
}
