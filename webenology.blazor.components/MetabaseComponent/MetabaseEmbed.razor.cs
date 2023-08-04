
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Primitives;


namespace webenology.blazor.components.MetabaseComponent;

public partial class MetabaseEmbed
{
    private string _embedUrl;
    [Parameter] public string BaseUrl { get; set; }
    [Parameter] public string SecretKey { get; set; }
    [Parameter] public Dictionary<string,string> UrlParameters { get; set; }
    [Parameter] public bool IsDashboard { get; set; }
    [Parameter] public int Height { get; set; }
    [Parameter] public int Width { get; set; }
[Inject] private IMetabaseHelpers _metabaseHelpers { get; set; }


private string CreateUrl()
{
    var jwt = _metabaseHelpers.CreateJwt();

    var sb = new StringBuilder();
    sb.Append(BaseUrl);
    sb.Append("/");
    sb.Append(IsDashboard ? "dashboard" : "question");
    sb.Append("/");
    sb.Append(jwt);
    if (UrlParameters.Any())
    {
        sb.Append("#");
        foreach (var parameter in UrlParameters)
        {
            sb.Append(parameter.Key);
            sb.Append("=");
            sb.Append(parameter.Value);

            if (parameter != UrlParameters.Last())
                sb.Append("&");
        }
    }

    _embedUrl = sb.ToString();
}
    
}