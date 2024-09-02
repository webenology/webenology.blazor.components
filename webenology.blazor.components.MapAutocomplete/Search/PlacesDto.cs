using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace webenology.blazor.components.MapAutocompleteComponent.Search;
internal class PlacesDto
{
    public List<Place> Places { get; set; }
}

internal class Place
{
    public string FormattedAddress { get; set; }
    public List<AddressComponent> AddressComponents { get; set; }
    public GoogleLocation Location { get; set; }
    public string Id { get; set; }
    public DisplayName DisplayName { get; set; }
}

internal class DisplayName
{
    public string Text { get; set; }
}

internal class GoogleLocation
{
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
}

internal class AddressComponent
{
    public string LongText { get; set; }
    public string ShortText { get; set; }
    public List<string> Types { get; set; }
}

internal class GoogleSuggestion
{
    public List<GoogleSuggestionDto> Suggestions { get; set; }
}

internal class GoogleSuggestionDto
{
    public GooglePlacePrediction PlacePrediction { get; set; }
}

internal class GooglePlacePrediction
{
    public string PlaceId { get; set; }
    public GooglePlaceText Text { get; set; }
}

internal class GooglePlaceText
{
    public string Text { get; set; }
    public List<GooglePlaceMatch> Matches { get; set; }
}

internal class GooglePlaceMatch
{
    public int StartOffset { get; set; }
    public int EndOffset { get; set; }
}