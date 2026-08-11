using Microsoft.AspNetCore.Mvc;
using TransportERP.Api.Controllers;
using TransportERP.Contracts.Geo;
using TransportERP.Desktop.Forms.Setup.Geo;
using TransportERP.Domain.Geo;

namespace TransportERP.Tests;

public sealed class GeoContractTests
{
    [Fact]
    public void GeographyDomain_UsesOnlyTheApprovedFiveLevelHierarchy_AndExcludesCountryChangeCandidates()
    {
        Assert.Equal(typeof(Country), typeof(Governorate).GetProperty(nameof(Governorate.Country))!.PropertyType);
        Assert.Equal(typeof(Governorate), typeof(Directorate).GetProperty(nameof(Directorate.Governorate))!.PropertyType);
        Assert.Equal(typeof(Directorate), typeof(City).GetProperty(nameof(City.Directorate))!.PropertyType);
        Assert.Equal(typeof(City), typeof(Area).GetProperty(nameof(Area.City))!.PropertyType);
        var countryProperties = typeof(Country).GetProperties().Select(x => x.Name).ToHashSet();
        Assert.DoesNotContain("Iso2", countryProperties); Assert.DoesNotContain("Iso3", countryProperties); Assert.DoesNotContain("DialingCode", countryProperties);
    }

    [Fact]
    public void ContractFields_UseTheApprovedLengthsAndNoExcludedFields()
    {
        Assert.Equal(64, new CreateCountryRequest(new string('A', 64), "اليمن", null, null).Code.Length);
        Assert.DoesNotContain(typeof(CountryDto).GetProperties(), x => x.Name is "Iso2" or "Iso3" or "DialingCode");
    }

    [Fact]
    public void ApiControllers_ExposeExactlyTheFiveApprovedResourceRoutes()
    {
        var routes = new[] { typeof(CountriesController), typeof(GovernoratesController), typeof(DirectoratesController), typeof(CitiesController), typeof(AreasController) }
            .Select(type => type.GetCustomAttributes(typeof(RouteAttribute), false).Cast<RouteAttribute>().Single().Template).ToArray();
        Assert.Equal(new[] { "api/v1/general/countries", "api/v1/general/governorates", "api/v1/general/directorates", "api/v1/general/cities", "api/v1/general/areas" }, routes);
    }

    [Fact]
    public void DesktopScreens_UseTheSharedShell_AndNeverOfferPhysicalDelete()
    {
        GeoMasterDataScreen[] screens = [new FrmCountries(), new FrmGovernorates(), new FrmDirectorates(), new FrmCities(), new FrmAreas()];
        Assert.Equal(new[] { "GEN-003", "GEN-004", "GEN-005", "GEN-006", "GEN-007" }, screens.Select(x => x.ScreenCode));
        Assert.All(screens, screen => Assert.False(screen.Shell.Toolbar.DeleteButton.Visible));
        Assert.All(screens, screen => Assert.False(screen.Shell.Toolbar.PrintButton.Visible));
    }
}
