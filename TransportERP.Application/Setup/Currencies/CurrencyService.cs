using TransportERP.Contracts.Setup.Currencies;

namespace TransportERP.Application.Setup.Currencies;

public interface ICurrencyService
{
    Task<CurrencySearchResponse> SearchAsync(CurrencySearchRequest request, CancellationToken cancellationToken);
    Task<CurrencyCommandResponse> CreateAsync(CreateCurrencyRequest request, CancellationToken cancellationToken);
    Task<CurrencyCommandResponse> UpdateAsync(Guid id, UpdateCurrencyRequest request, CancellationToken cancellationToken);
    Task<CurrencyCommandResponse> SuspendAsync(Guid id, CancellationToken cancellationToken);
    Task<CurrencyCommandResponse> DeleteAsync(Guid id, CancellationToken cancellationToken);
}

public sealed class CurrencyService : ICurrencyService
{
    private const string Message = "لا يتوفر تخزين معتمد للعملات بعد. لم تُنشأ بيانات بديلة.";

    public Task<CurrencySearchResponse> SearchAsync(CurrencySearchRequest request, CancellationToken cancellationToken)
        => Task.FromResult(new CurrencySearchResponse(Array.Empty<CurrencyDto>(), 0, Math.Max(1, request.Page), Math.Clamp(request.PageSize, 1, 100), false, CurrencyBlockers.ApprovedStorageUnavailable, Message));

    public Task<CurrencyCommandResponse> CreateAsync(CreateCurrencyRequest request, CancellationToken cancellationToken)
        => Task.FromResult(Validate(request.Code, request.ArabicName, request.IsoCode) ?? Blocked());

    public Task<CurrencyCommandResponse> UpdateAsync(Guid id, UpdateCurrencyRequest request, CancellationToken cancellationToken)
        => Task.FromResult(Validate("valid", request.ArabicName, request.IsoCode) ?? Blocked());

    public Task<CurrencyCommandResponse> SuspendAsync(Guid id, CancellationToken cancellationToken) => Task.FromResult(Blocked());
    public Task<CurrencyCommandResponse> DeleteAsync(Guid id, CancellationToken cancellationToken) => Task.FromResult(Blocked());

    private static CurrencyCommandResponse? Validate(string code, string name, string iso)
        => string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(iso)
            ? new CurrencyCommandResponse(false, true, "VALIDATION_FAILED", "رمز العملة والاسم العربي ورمز ISO حقول إلزامية.", null)
            : null;

    private static CurrencyCommandResponse Blocked() => new(false, false, CurrencyBlockers.ApprovedStorageUnavailable, Message, null);
}
