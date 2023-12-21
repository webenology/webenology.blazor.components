using System.Collections.Generic;

namespace webenology.blazor.components.MapAutocompleteComponent;

public class LatLng
{
    public decimal Lat { get; set; }
    public decimal Lng { get; set; }
    public string ToRoute() => $"{Lat},{Lng}";

    public LatLng()
    {
    }
    public LatLng(decimal lat, decimal lng)
    {
        Lat = lat;
        Lng = lng;
    }
}

public class GeoAutoResult
{
    public List<GeoAutoItem> Items { get; set; }
}

public class GeoAutoItem
{
    public string Title { get; set; }
    public GeoAutoAddress Address { get; set; }
    public GeoAutoHighlights Highlights { get; set; }
    public LatLng Position { get; set; }
    public string Id { get; set; }
}

public class GeoAutoHighlights
{
    public List<GeoAutoStartEnd> Title { get; set; }
    public GeoAutoStartEndAddress Address { get; set; }
}

public class GeoAutoStartEnd
{
    public int Start { get; set; }
    public int End { get; set; }
}

public class GeoAutoStartEndAddress
{
    public List<GeoAutoStartEnd> Label { get; set; }
    public List<GeoAutoStartEnd> City { get; set; }
    public List<GeoAutoStartEnd> Street { get; set; }
    public List<GeoAutoStartEnd> HouseNumber { get; set; }
}

public class GeoAutoAddress
{
    public string Label { get; set; }
    public string StateCode { get; set; }
    public string State { get; set; }
    public string CountryName { get; set; }
    public string CountryCode { get; set; }
    public string City { get; set; }
    public string District { get; set; }
    public string Street { get; set; }
    public string PostalCode { get; set; }
    public string HouseNumber { get; set; }
    public LatLng Position { get; set; }
}
