namespace TransportERP.Domain.Waybills;

public enum WaybillStatus
{
    Draft,
    ReadyForApproval,
    Approved,
    Cancelled
}

public enum WaybillPartyRole
{
    Sender,
    Receiver,
    Payer
}

public sealed record WaybillPartyValue(
    WaybillPartyRole Role,
    Guid? OperationalPartyId,
    string Name,
    string Mobile,
    string? IdentityType,
    string? IdentityNo,
    Guid? CountryId,
    Guid? GovernorateId,
    Guid? DirectorateId,
    Guid? CityId,
    Guid? AreaId,
    string? AddressText)
{
    public void EnsureValid()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new WaybillRuleException("PARTY_NAME_REQUIRED");
        if (string.IsNullOrWhiteSpace(Mobile))
            throw new WaybillRuleException("PARTY_MOBILE_REQUIRED");
        if (!string.IsNullOrWhiteSpace(IdentityNo) && string.IsNullOrWhiteSpace(IdentityType))
            throw new WaybillRuleException("IDENTITY_TYPE_REQUIRED");
        if (AreaId.HasValue && !CityId.HasValue)
            throw new WaybillRuleException("AREA_REQUIRES_CITY");
        if (CityId.HasValue && !DirectorateId.HasValue)
            throw new WaybillRuleException("CITY_REQUIRES_DIRECTORATE");
        if (DirectorateId.HasValue && !GovernorateId.HasValue)
            throw new WaybillRuleException("DIRECTORATE_REQUIRES_GOVERNORATE");
        if (GovernorateId.HasValue && !CountryId.HasValue)
            throw new WaybillRuleException("GOVERNORATE_REQUIRES_COUNTRY");
        if (!CountryId.HasValue && string.IsNullOrWhiteSpace(AddressText))
            throw new WaybillRuleException("PARTY_ADDRESS_REQUIRED");
    }
}

public sealed record WaybillItemValue(
    Guid Id,
    int LineNo,
    string ItemType,
    string Contents,
    decimal Quantity,
    int? Pieces,
    decimal? Weight,
    decimal? Length,
    decimal? Width,
    decimal? Height,
    decimal? DeclaredValue,
    Guid? OriginCountryId,
    string RiskFlagsJson,
    string? Notes)
{
    public void EnsureValid()
    {
        if (Id == Guid.Empty)
            throw new WaybillRuleException("ITEM_ID_REQUIRED");
        if (LineNo < 1)
            throw new WaybillRuleException("ITEM_LINE_INVALID");
        if (string.IsNullOrWhiteSpace(ItemType))
            throw new WaybillRuleException("ITEM_TYPE_REQUIRED");
        if (string.IsNullOrWhiteSpace(Contents))
            throw new WaybillRuleException("ITEM_CONTENTS_REQUIRED");
        if (Quantity <= 0m)
            throw new WaybillRuleException("ITEM_QUANTITY_INVALID");
        if (Pieces is <= 0)
            throw new WaybillRuleException("ITEM_PIECES_INVALID");
        foreach (var value in new[] { Weight, Length, Width, Height, DeclaredValue })
        {
            if (value < 0m)
                throw new WaybillRuleException("ITEM_MEASUREMENT_INVALID");
        }
    }
}

public sealed class WaybillAggregate
{
    private readonly List<WaybillPartyValue> _parties = [];
    private readonly List<WaybillItemValue> _items = [];

    private WaybillAggregate() { }

    public Guid Id { get; private set; }
    public Guid CompanyId { get; private set; }
    public Guid BranchId { get; private set; }
    public string DraftNo { get; private set; } = string.Empty;
    public string? WaybillNo { get; private set; }
    public DateTimeOffset WaybillDateTime { get; private set; }
    public string ServiceType { get; private set; } = "STANDARD";
    public string Priority { get; private set; } = "NORMAL";
    public Guid OriginId { get; private set; }
    public Guid DestinationId { get; private set; }
    public Guid CurrencyId { get; private set; }
    public decimal ExchangeRate { get; private set; }
    public decimal FreightTotal { get; private set; }
    public decimal DiscountTotal { get; private set; }
    public decimal NetAmount => FreightTotal - DiscountTotal;
    public WaybillStatus Status { get; private set; }
    public long Version { get; private set; }
    public IReadOnlyList<WaybillPartyValue> Parties => _parties;
    public IReadOnlyList<WaybillItemValue> Items => _items;

    public static WaybillAggregate CreateDraft(
        Guid id,
        Guid companyId,
        Guid branchId,
        string draftNo,
        DateTimeOffset waybillDateTime,
        Guid originId,
        Guid destinationId,
        Guid currencyId,
        decimal exchangeRate,
        string serviceType = "STANDARD",
        string priority = "NORMAL")
    {
        if (id == Guid.Empty || companyId == Guid.Empty || branchId == Guid.Empty ||
            originId == Guid.Empty || destinationId == Guid.Empty || currencyId == Guid.Empty)
            throw new WaybillRuleException("REQUIRED_REFERENCE_MISSING");
        if (originId == destinationId)
            throw new WaybillRuleException("ORIGIN_DESTINATION_SAME");
        if (string.IsNullOrWhiteSpace(draftNo))
            throw new WaybillRuleException("DRAFT_NO_REQUIRED");
        if (exchangeRate <= 0m)
            throw new WaybillRuleException("EXCHANGE_RATE_INVALID");

        return new WaybillAggregate
        {
            Id = id,
            CompanyId = companyId,
            BranchId = branchId,
            DraftNo = draftNo.Trim(),
            WaybillDateTime = waybillDateTime,
            OriginId = originId,
            DestinationId = destinationId,
            CurrencyId = currencyId,
            ExchangeRate = exchangeRate,
            ServiceType = string.IsNullOrWhiteSpace(serviceType) ? "STANDARD" : serviceType.Trim(),
            Priority = string.IsNullOrWhiteSpace(priority) ? "NORMAL" : priority.Trim(),
            Status = WaybillStatus.Draft,
            Version = 1
        };
    }

    public static WaybillAggregate Rehydrate(
        Guid id, Guid companyId, Guid branchId, string draftNo, string? waybillNo,
        DateTimeOffset waybillDateTime, string serviceType, string priority,
        Guid originId, Guid destinationId, Guid currencyId, decimal exchangeRate,
        decimal freightTotal, decimal discountTotal, WaybillStatus status, long version,
        IEnumerable<WaybillPartyValue> parties, IEnumerable<WaybillItemValue> items)
    {
        var aggregate = new WaybillAggregate
        {
            Id = id,
            CompanyId = companyId,
            BranchId = branchId,
            DraftNo = draftNo,
            WaybillNo = waybillNo,
            WaybillDateTime = waybillDateTime,
            ServiceType = serviceType,
            Priority = priority,
            OriginId = originId,
            DestinationId = destinationId,
            CurrencyId = currencyId,
            ExchangeRate = exchangeRate,
            FreightTotal = freightTotal,
            DiscountTotal = discountTotal,
            Status = status,
            Version = version
        };
        aggregate._parties.AddRange(parties);
        aggregate._items.AddRange(items);
        return aggregate;
    }

    public void UpdateDraft(
        DateTimeOffset waybillDateTime,
        Guid originId,
        Guid destinationId,
        Guid currencyId,
        decimal exchangeRate,
        decimal freightTotal,
        decimal discountTotal,
        string serviceType,
        string priority,
        IEnumerable<WaybillPartyValue> parties,
        IEnumerable<WaybillItemValue> items)
    {
        EnsureState(WaybillStatus.Draft);
        if (originId == Guid.Empty || destinationId == Guid.Empty || currencyId == Guid.Empty)
            throw new WaybillRuleException("REQUIRED_REFERENCE_MISSING");
        if (originId == destinationId)
            throw new WaybillRuleException("ORIGIN_DESTINATION_SAME");
        if (exchangeRate <= 0m)
            throw new WaybillRuleException("EXCHANGE_RATE_INVALID");
        if (freightTotal < 0m || discountTotal < 0m || discountTotal > freightTotal)
            throw new WaybillRuleException("AMOUNT_INVALID");

        WaybillDateTime = waybillDateTime;
        OriginId = originId;
        DestinationId = destinationId;
        CurrencyId = currencyId;
        ExchangeRate = exchangeRate;
        FreightTotal = freightTotal;
        DiscountTotal = discountTotal;
        ServiceType = string.IsNullOrWhiteSpace(serviceType) ? "STANDARD" : serviceType.Trim();
        Priority = string.IsNullOrWhiteSpace(priority) ? "NORMAL" : priority.Trim();

        _parties.Clear();
        foreach (var party in parties)
        {
            party.EnsureValid();
            _parties.Add(party);
        }
        if (_parties.Count(x => x.Role == WaybillPartyRole.Sender) > 1 ||
            _parties.Count(x => x.Role == WaybillPartyRole.Receiver) > 1)
            throw new WaybillRuleException("PARTY_ROLE_DUPLICATE");

        _items.Clear();
        foreach (var item in items.OrderBy(x => x.LineNo))
        {
            item.EnsureValid();
            _items.Add(item);
        }
        if (_items.Select(x => x.LineNo).Distinct().Count() != _items.Count)
            throw new WaybillRuleException("ITEM_LINE_DUPLICATE");

        Version++;
    }

    public IReadOnlyList<string> ValidateForApproval()
    {
        var errors = new List<string>();
        if (WaybillNo is not null)
            errors.Add("DRAFT_HAS_OFFICIAL_NUMBER");
        if (!_parties.Any(x => x.Role == WaybillPartyRole.Sender))
            errors.Add("SENDER_REQUIRED");
        if (!_parties.Any(x => x.Role == WaybillPartyRole.Receiver))
            errors.Add("RECEIVER_REQUIRED");
        if (_items.Count == 0)
            errors.Add("ITEM_REQUIRED");
        if (_items.Any(x => x.Quantity <= 0m))
            errors.Add("ITEM_QUANTITY_INVALID");
        if (ExchangeRate <= 0m)
            errors.Add("EXCHANGE_RATE_INVALID");
        if (FreightTotal < 0m || DiscountTotal < 0m || DiscountTotal > FreightTotal)
            errors.Add("AMOUNT_INVALID");
        return errors;
    }

    public void SubmitForApproval()
    {
        EnsureState(WaybillStatus.Draft);
        var errors = ValidateForApproval();
        if (errors.Count > 0)
            throw new WaybillValidationException(errors);
        Status = WaybillStatus.ReadyForApproval;
        Version++;
    }

    public void ReturnForCorrection()
    {
        EnsureState(WaybillStatus.ReadyForApproval);
        Status = WaybillStatus.Draft;
        Version++;
    }

    public void ApplyApproval(string officialNumber)
    {
        EnsureState(WaybillStatus.ReadyForApproval);
        if (string.IsNullOrWhiteSpace(officialNumber))
            throw new WaybillRuleException("OFFICIAL_NUMBER_REQUIRED");
        if (WaybillNo is not null && !string.Equals(WaybillNo, officialNumber, StringComparison.Ordinal))
            throw new WaybillRuleException("OFFICIAL_NUMBER_IMMUTABLE");
        var errors = ValidateForApproval().Where(x => x != "DRAFT_HAS_OFFICIAL_NUMBER").ToList();
        if (errors.Count > 0)
            throw new WaybillValidationException(errors);
        WaybillNo = officialNumber.Trim();
        Status = WaybillStatus.Approved;
        Version++;
    }

    public void Cancel()
    {
        if (Status == WaybillStatus.Cancelled)
            return;
        if (Status == WaybillStatus.Approved)
            throw new WaybillRuleException("APPROVED_CANCEL_REQUIRES_LATER_CONTROL_POLICY");
        Status = WaybillStatus.Cancelled;
        Version++;
    }

    private void EnsureState(WaybillStatus expected)
    {
        if (Status != expected)
            throw new WaybillRuleException("INVALID_STATE");
    }
}

public sealed class WaybillRuleException(string code) : InvalidOperationException(code)
{
    public string Code { get; } = code;
}

public sealed class WaybillValidationException(IReadOnlyList<string> errors) : InvalidOperationException("VALIDATION_ERROR")
{
    public IReadOnlyList<string> Errors { get; } = errors;
}
