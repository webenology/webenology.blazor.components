using System.Globalization;
using System.Net;
using System.Net.Http.Json;

namespace webenology.blazor.components.MapAutocompleteComponent.Search;

internal class HereMapsSearch : ISearch
{
    private readonly MapSettings _mapSettings;

    public HereMapsSearch(MapSettings mapSettings)
    {
        _mapSettings = mapSettings;
    }

    public async Task<List<GeoAutoAddress>> Search(string query)
    {
        if (string.IsNullOrEmpty(query))
            return null;

        using var http = new HttpClient();
        http.BaseAddress = new Uri("https://autocomplete.search.hereapi.com/v1/");
        var encodedQuery = WebUtility.HtmlEncode(query);
        var lat = _mapSettings.CenterLat.ToString(CultureInfo.InvariantCulture);
        var lng = _mapSettings.CenterLng.ToString(CultureInfo.InvariantCulture);
        var searchUrl = $"autocomplete?q={encodedQuery}&in=countryCode:CAN,MEX,USA&at={lat},{lng}&limit=5&apiKey={_mapSettings.ApiKey}";
        var results = await http.GetFromJsonAsync<GeoAutoResult>(searchUrl);

        return results.Items.Select(x => new GeoAutoAddress
        {
            HouseNumber = x.Address.HouseNumber,
            City = x.Address.City,
            CountryCode = x.Address.CountryCode,
            CountryName = x.Address.CountryName,
            District = x.Address.District,
            Label = x.Address.Label,
            Position = x.Position,
            PostalCode = x.Address.PostalCode,
            State = x.Address.State,
            StateCode = x.Address.StateCode,
            Street = x.Address.Street,
            Id = x.Id
        }).ToList();
    }

    public async Task<GeoAutoItem> LookupBy(string hereId)
    {
        if (string.IsNullOrEmpty(hereId))
            return null;

        using var http = new HttpClient();
        http.BaseAddress = new Uri("https://lookup.search.hereapi.com/v1/");
        var results = await http.GetFromJsonAsync<GeoAutoItem>($"lookup?id={hereId}&apiKey={_mapSettings.ApiKey}");

        return results;
    }
}