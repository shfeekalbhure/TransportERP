namespace TransportERP.Contracts.Wave1;

public static class ACC036Kinds
{
    public const string Group = "GROUP";
    public const string Type = "TYPE";
    public static bool IsKnown(string? value) => value is Group or Type;
}

public sealed record ACC036Dto(
    Guid Id,
    Guid CompanyId,
    string Kind,
    string Code,
    string ArabicName,
    string? EnglishName,
    string? FinancialClassification,
    string? NormalBalance,
    bool? AllowsPostingAccounts,
    bool? ShowInFinancialStatements,
    int? DisplayOrder,
    bool IsActive,
    long Version);

public sealed record CreateACC036Request(
    string Kind,
    string Code,
    string ArabicName,
    string? EnglishName = null,
    string? FinancialClassification = null,
    string? NormalBalance = null,
    bool? AllowsPostingAccounts = null,
    bool? ShowInFinancialStatements = null,
    int? DisplayOrder = null);

public sealed record UpdateACC036Request(
    string Kind,
    string Code,
    string ArabicName,
    string? EnglishName,
    string? FinancialClassification,
    string? NormalBalance,
    bool? AllowsPostingAccounts,
    bool? ShowInFinancialStatements,
    int? DisplayOrder,
    long ExpectedVersion);
