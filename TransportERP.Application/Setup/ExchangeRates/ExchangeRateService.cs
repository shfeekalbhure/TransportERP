using TransportERP.Contracts.Setup.ExchangeRates;

namespace TransportERP.Application.Setup.ExchangeRates;

public interface IExchangeRateService
{
    Task<ExchangeRateSearchResponse> SearchAsync(ExchangeRateSearchRequest request, CancellationToken cancellationToken);
    Task<ExchangeRateCommandResponse> CreateAsync(CreateExchangeRateRequest request, CancellationToken cancellationToken);
    Task<ExchangeRateCommandResponse> UpdateAsync(Guid id, UpdateExchangeRateRequest request, CancellationToken cancellationToken);
    Task<ExchangeRateCommandResponse> SuspendAsync(Guid id, CancellationToken cancellationToken);
    Task<ExchangeRateCommandResponse> DeleteAsync(Guid id, CancellationToken cancellationToken);
}

public sealed class ExchangeRateService : IExchangeRateService
{
    private const string Message = "لا يتوفر تخزين معتمد لأسعار الصرف بعد. لم تُنشأ بيانات بديلة.";
    public Task<ExchangeRateSearchResponse> SearchAsync(ExchangeRateSearchRequest request, CancellationToken cancellationToken)
        => Task.FromResult(new ExchangeRateSearchResponse(Array.Empty<ExchangeRateDto>(), 0, Math.Max(1, request.Page), Math.Clamp(request.PageSize, 1, 100), false, ExchangeRateBlockers.ApprovedStorageUnavailable, Message));
    public Task<ExchangeRateCommandResponse> CreateAsync(CreateExchangeRateRequest request, CancellationToken cancellationToken)
        => Task.FromResult(Validate(request.ForeignCurrencyCode, request.LocalCurrencyCode, request.ReferenceRate, request.MinimumRate, request.MaximumRate) ?? Blocked());
    public Task<ExchangeRateCommandResponse> UpdateAsync(Guid id, UpdateExchangeRateRequest request, CancellationToken cancellationToken)
        => Task.FromResult(Validate(request.ForeignCurrencyCode, request.LocalCurrencyCode, request.ReferenceRate, request.MinimumRate, request.MaximumRate) ?? Blocked());
    public Task<ExchangeRateCommandResponse> SuspendAsync(Guid id, CancellationToken cancellationToken)=>Task.FromResult(Blocked());
    public Task<ExchangeRateCommandResponse> DeleteAsync(Guid id, CancellationToken cancellationToken)=>Task.FromResult(Blocked());
    private static ExchangeRateCommandResponse? Validate(string foreign, string local, decimal reference, decimal minimum, decimal maximum)
        => string.IsNullOrWhiteSpace(foreign)||string.IsNullOrWhiteSpace(local)||reference<=0||minimum<=0||maximum<minimum||reference<minimum||reference>maximum ? new(false,true,"VALIDATION_FAILED","تحقق من العملتين والسعر المرجعي والحدين الأدنى والأعلى.",null) : null;
    private static ExchangeRateCommandResponse Blocked()=>new(false,false,ExchangeRateBlockers.ApprovedStorageUnavailable,Message,null);
}
