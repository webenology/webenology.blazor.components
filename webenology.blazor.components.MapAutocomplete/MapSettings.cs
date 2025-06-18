namespace webenology.blazor.components.MapAutocompleteComponent;
internal class MapSettings
{
    public double CenterLat { get; set; }
    public double CenterLng { get; set; }
    public SearchType SearchType { get; set; }
    public string? ApiKey { get; set; }
}

internal enum SearchType
{
    HereMaps,
    GoogleMaps
}
