namespace TransportERP.Desktop.CoreUI.Architecture;

/// <summary>
/// العقد الصغير الذي يعلن الشاشة المرجعية فقط. لا يحمل منطق أعمال أو بيانات.
/// </summary>
public sealed record ReferenceScreenDefinition(
    string Code,
    string Title,
    TransportScreenProfile Profile,
    IReadOnlyList<string> Fields,
    IReadOnlyList<string> GridColumns,
    bool UsesTree = false,
    bool IsReadOnly = false);
