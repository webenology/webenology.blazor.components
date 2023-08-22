using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Primitives;


namespace webenology.blazor.components;

public partial class MetabaseEmbed
{
    [Parameter] public string BaseUrl { get; set; }
    [Parameter] public string SecretKey { get; set; }
    [Parameter] public Dictionary<string, string>? UrlParameters { get; set; }
    [Parameter] public bool IsDashboard { get; set; }
    [Parameter] public int Height { get; set; }
    [Parameter] public int Width { get; set; }
    [Parameter] public int ResourceId { get; set; }
    [Parameter] public bool IsBordered { get; set; }
    [Parameter] public bool IsTitled { get; set; }
    [Parameter] public MetabaseTheme Theme { get; set; }
    [Inject] private IMetabaseHelper _metabaseHelpers { get; set; }

    private string _embedUrl;
    private string _jwt;

    protected override void OnInitialized()
    {
        Height = 400;
        Width = 600;
        IsBordered = true;
        IsTitled = true;
        UrlParameters = new();
        base.OnInitialized();
    }

    protected override void OnParametersSet()
    {
        var newUrl = CreateUrl();
        if (_embedUrl != newUrl)
        {
            _embedUrl = newUrl;
        }

        base.OnParametersSet();
    }

    private string CreateUrl()
    {
        _jwt = _metabaseHelpers.GenerateJwt(ResourceId, SecretKey, IsDashboard, _jwt);

        var sb = new StringBuilder();
        sb.Append($"{BaseUrl}");
        if (!BaseUrl.EndsWith("/"))
            sb.Append("/");
        sb.Append(IsDashboard ? "embed/dashboard/" : "embed/question/");
        sb.Append(_jwt);

        if (IsBordered)
        {
            UrlParameters.Add("bordered", IsBordered.ToString().ToLower());
        }

        if (IsTitled)
        {
            UrlParameters.Add("titled", IsTitled.ToString().ToLower());
        }

        if (Theme != MetabaseTheme.Light)
        {
            UrlParameters.Add("theme", Theme == MetabaseTheme.Dark ? "night" : "transparent");
        }

        if (UrlParameters?.Any() ?? false)
        {
            var allParams = string.Join("&", UrlParameters.Select(x => $"{x.Key}={x.Value}"));
            sb.Append($"#{allParams}");
        }

        return sb.ToString();
    }
}