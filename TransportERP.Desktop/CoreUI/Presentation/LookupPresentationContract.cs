namespace TransportERP.Desktop.CoreUI.Presentation;

/// <summary>
/// نمط الاختيار الذي يخص العرض فقط.
/// </summary>
public enum LookupSelectionMode
{
    Single,
    Multiple
}

/// <summary>
/// عقد عرض واختيار Lookup. لا يعرف Endpoint أو قاعدة بيانات أو Entity أو Permission أو Cache/Offline policy.
/// </summary>
public sealed record LookupPresentationContract
{
    public LookupPresentationContract(
        string lookupId,
        string lookupType,
        string context,
        LookupSelectionMode selectionMode,
        IReadOnlyCollection<string>? allowedFilters = null)
    {
        LookupId = RequireValue(lookupId, nameof(lookupId));
        LookupType = RequireValue(lookupType, nameof(lookupType));
        Context = RequireValue(context, nameof(context));
        SelectionMode = selectionMode;
        AllowedFilters = (allowedFilters ?? Array.Empty<string>())
            .Select(filter => RequireValue(filter, nameof(allowedFilters)))
            .Distinct(StringComparer.Ordinal)
            .ToArray();
    }

    public string LookupId { get; }

    public string LookupType { get; }

    public string Context { get; }

    public LookupSelectionMode SelectionMode { get; }

    public IReadOnlyCollection<string> AllowedFilters { get; }

    private static string RequireValue(string value, string paramName) =>
        string.IsNullOrWhiteSpace(value)
            ? throw new ArgumentException("A presentation identifier is required.", paramName)
            : value.Trim();
}

/// <summary>
/// عنصر عرض محايد المصدر. ملؤه مسؤولية تنفيذ مصرح به لاحقًا خارج W1-PLATFORM.
/// </summary>
public sealed record LookupPresentationItem(
    string Id,
    string Code,
    string DisplayName,
    string? SecondaryName = null,
    string? Status = null,
    string? Scope = null)
{
    public override string ToString() => string.IsNullOrWhiteSpace(Code)
        ? DisplayName
        : $"{Code} — {DisplayName}";
}

/// <summary>
/// نتيجة اختيار العرض. تحمل المعرف الذي اختاره المستخدم فقط ولا تضيف دلالة أعمال أو مصدر بيانات.
/// </summary>
public sealed record LookupPresentationSelection(LookupPresentationContract Contract, string SelectedId)
{
    public void EnsureComplete()
    {
        ArgumentNullException.ThrowIfNull(Contract);

        if (string.IsNullOrWhiteSpace(SelectedId))
        {
            throw new ArgumentException("A selected presentation item identifier is required.", nameof(SelectedId));
        }
    }
}
