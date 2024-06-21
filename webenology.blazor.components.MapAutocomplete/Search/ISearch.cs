using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webenology.blazor.components.MapAutocompleteComponent.Search;
internal interface ISearch
{
    Task<List<GeoAutoAddress>> Search(string query);
    Task<GeoAutoItem> LookupBy(string id);
}
