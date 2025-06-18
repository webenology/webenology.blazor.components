namespace webenology.blazor.components.MapAutocompleteComponent.Search;
internal interface ISearch
{
    Task<List<GeoAutoAddress>> Search(string query);
    Task<GeoAutoItem> LookupBy(string id);
}
