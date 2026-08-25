using TransportERP.Contracts.Localization;

namespace TransportERP.Tests;

public sealed class W0LocalizationTests
{
    [Fact]
    public void Arabic_is_default_and_unknown_culture_falls_back_to_arabic()
    {
        var catalog = Catalog();
        Assert.Equal("arabic-default", catalog.Get("test.key", ""));
        Assert.Equal("arabic-default", catalog.Get("test.key", "fr-FR"));
    }

    [Fact]
    public void Supported_culture_changes_display_text_without_changing_key()
        => Assert.Equal("english-test", Catalog().Get("test.key", "en-US"));

    [Fact]
    public void Missing_key_returns_neutral_key()
        => Assert.Equal("missing.key", Catalog().Get("missing.key", "ar"));

    private static CultureAwareTextCatalog Catalog() => new(new Dictionary<string, IReadOnlyDictionary<string, string>>
    {
        ["ar"] = new Dictionary<string, string> { ["test.key"] = "arabic-default" },
        ["en"] = new Dictionary<string, string> { ["test.key"] = "english-test" }
    });
}
