﻿using System.Net.Http.Json;
using System.Text.Json;

namespace webenology.blazor.components.MapAutocompleteComponent.Search;
internal class GoogleMapsSearch : ISearch
{
    private readonly MapSettings _mapSettings;

    public GoogleMapsSearch(MapSettings mapSettings)
    {
        _mapSettings = mapSettings;
    }

    public async Task<List<GeoAutoAddress>> Search(string query)
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri("https://places.googleapis.com/");
        client.DefaultRequestHeaders.Add("X-Goog-Api-Key", _mapSettings.ApiKey);
        //client.DefaultRequestHeaders.Add("X-Goog-FieldMask", "formattedAddress,addressComponents,location,id");
        var model = new
        {
            input = query,
            regionCode = "us"
        };

        var results = await client.PostAsJsonAsync("/v1/places:autocomplete", model);

        if (results.IsSuccessStatusCode)
        {
            var str = await results.Content.ReadAsStringAsync();
            var suggestions =
                JsonSerializer.Deserialize<GoogleSuggestion>(str,
                    new JsonSerializerOptions(JsonSerializerDefaults.Web));

            var geoAutoItems = new List<GeoAutoAddress>();

            if (suggestions is { Suggestions: { } })
            {
                foreach (var suggest in suggestions.Suggestions)
                {
                    var label = suggest.PlacePrediction.Text.Text;
                    foreach (var gh in suggest.PlacePrediction.Text.Matches.OrderByDescending(x => x.EndOffset))
                    {
                        label = label.Insert(gh.EndOffset, "</mark>").Insert(gh.StartOffset, "<mark>");
                    }
                    var geoAutoItem = new GeoAutoAddress
                    {
                        LabelHighlighted = label,
                        Label = suggest.PlacePrediction.Text.Text,
                        Id = suggest.PlacePrediction.PlaceId
                    };
                    geoAutoItems.Add(geoAutoItem);
                }
            }

            return geoAutoItems;
            //var places =
            //    JsonSerializer.Deserialize<PlacesDto>(str, new JsonSerializerOptions(JsonSerializerDefaults.Web));

            //if (places != null)
            //{
            //    var geoAutoItems = new List<GeoAutoAddress>();
            //    if (places.Places != null)
            //    {

            //        foreach (var place in places.Places)
            //        {
            //            var geoAutoItem = new GeoAutoAddress()
            //            {
            //                Id = place.Id,
            //                Position = new LatLng(place.Location.Latitude, place.Location.Longitude),
            //                HouseNumber = GetAddressComponent(place.AddressComponents, true, "street_number"),
            //                Street = GetAddressComponent(place.AddressComponents, true, "route"),
            //                City = GetAddressComponent(place.AddressComponents, false, "locality"),
            //                State = GetAddressComponent(place.AddressComponents, false, "administrative_area_level_1"),
            //                StateCode = GetAddressComponent(place.AddressComponents, true, "administrative_area_level_1"),
            //                CountryCode = GetAddressComponent(place.AddressComponents, true, "country"),
            //                CountryName = GetAddressComponent(place.AddressComponents, false, "country"),
            //                PostalCode = GetAddressComponent(place.AddressComponents, false, "postal_code"),
            //                Suite = GetAddressComponent(place.AddressComponents, false, "subpremise"),
            //                Label = BuildLabel(place)
            //            };
            //            geoAutoItems.Add(geoAutoItem);
            //        }
            //    }
            //    return geoAutoItems;
            //}
        }
        else
        {
            var str = await results.Content.ReadAsStringAsync();
            return new List<GeoAutoAddress>();
        }

        return new List<GeoAutoAddress>();
    }

    private async Task<Place> GetPlaceDetails(string placeId)
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri("https://places.googleapis.com/");
        client.DefaultRequestHeaders.Add("X-Goog-Api-Key", _mapSettings.ApiKey);
        client.DefaultRequestHeaders.Add("X-Goog-FieldMask", "formattedAddress,addressComponents,location,id");

        var results = await client.GetFromJsonAsync<Place>($"/v1/places/{placeId}");

        return results;
    }

    private string GetAddressComponent(List<AddressComponent> address, bool showShort, string type)
    {
        var addr = address.Find(x => x.Types.Contains(type));
        if (addr != null)
            return showShort ? addr.ShortText : addr.LongText;

        return string.Empty;
    }

    private string BuildLabel(Place? p)
    {
        if (p == null)
            return string.Empty;

        if (p?.DisplayName != null && !string.IsNullOrEmpty(p.FormattedAddress))
        {
            if (!string.IsNullOrEmpty(p.DisplayName.Text) && p.FormattedAddress?.IndexOf(p.DisplayName.Text) > -1)
            {
                return p.FormattedAddress;
            }

            if (!string.IsNullOrEmpty(p.DisplayName.Text))
            {
                return $"{p.DisplayName.Text} {p.FormattedAddress}";
            }
        }

        if (!string.IsNullOrEmpty(p!.FormattedAddress))
            return p.FormattedAddress;

        return string.Empty;
    }

    public async Task<GeoAutoItem> LookupBy(string hereId)
    {
        var place = await GetPlaceDetails(hereId);

        var geoAutoItem = new GeoAutoAddress()
        {
            Id = place.Id,
            Position = new LatLng(place.Location.Latitude, place.Location.Longitude),
            HouseNumber = GetAddressComponent(place.AddressComponents, true, "street_number"),
            Street = GetAddressComponent(place.AddressComponents, true, "route"),
            City = GetAddressComponent(place.AddressComponents, false, "locality"),
            State = GetAddressComponent(place.AddressComponents, false, "administrative_area_level_1"),
            StateCode = GetAddressComponent(place.AddressComponents, true, "administrative_area_level_1"),
            CountryCode = GetAddressComponent(place.AddressComponents, true, "country"),
            CountryName = GetAddressComponent(place.AddressComponents, false, "country"),
            PostalCode = GetAddressComponent(place.AddressComponents, false, "postal_code"),
            Suite = GetAddressComponent(place.AddressComponents, false, "subpremise"),
            Label = BuildLabel(place)
        };


        return new GeoAutoItem
        {
            Id = hereId,
            Position = new LatLng(place.Location.Latitude, place.Location.Longitude),
            Address = geoAutoItem
        };
    }
}
