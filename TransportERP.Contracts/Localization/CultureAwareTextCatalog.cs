namespace TransportERP.Contracts.Localization;

/// <summary>Culture selection and Arabic-default fallback; product resources are supplied separately.</summary>
public sealed class CultureAwareTextCatalog : ILocalizedTextCatalog
{
    private readonly IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> _resources;

    public CultureAwareTextCatalog(IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> resources)
        => _resources = resources;

    public string Get(string key, string cultureName)
    {
        var requested = string.IsNullOrWhiteSpace(cultureName) ? "ar" : cultureName;
        var neutral = requested.Split('-', 2)[0];
        if (_resources.TryGetValue(requested, out var exact) && exact.TryGetValue(key, out var exactValue)) return exactValue;
        if (_resources.TryGetValue(neutral, out var localized) && localized.TryGetValue(key, out var localizedValue)) return localizedValue;
        return _resources.TryGetValue("ar", out var arabic) && arabic.TryGetValue(key, out var arabicValue) ? arabicValue : key;
    }
}
