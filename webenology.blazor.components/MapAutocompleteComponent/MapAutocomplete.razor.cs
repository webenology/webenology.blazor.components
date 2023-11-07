using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;

using Microsoft.AspNetCore.Components;

namespace webenology.blazor.components;

public partial class MapAutocomplete
{
    [Parameter] public string DefaultSearch { get; set; }
    [Parameter] public EventCallback<GeoAutoAddress> OnSelectAddress { get; set; }
    [Parameter] public bool IsDisabled { get; set; }
    [Parameter] public decimal? CentralLat { get; set; }
    [Parameter] public decimal? CentralLng { get; set; }
    [Parameter] public string ApiKey { get; set; }
    [Parameter] public CountryEnum Country { get; set; } = CountryEnum.USA | CountryEnum.CAN | CountryEnum.MEX;

    private string _previousSearch;
    private string _addressSelectedLabel;
    private string _search { get; set; }
    private string Search
    {
        get => _search;
        set
        {
            _search = value;
            DoSearch();
        }
    }

    private Timer _debounceTimer;
    private string _searchData;
    private bool _debouncing;
    private bool _searching;
    private bool _showResults;
    private GeoAutoResult _autoItem;
    private OutsideClick _outsideClick;

    protected override void OnInitialized()
    {
        _debounceTimer = new Timer(TimeSpan.FromMilliseconds(300));
        _debounceTimer.Elapsed += DebounceTimerOnElapsed;
        _debounceTimer.AutoReset = false;
        base.OnInitialized();
    }

    private async void DoSearch()
    {
        await Task.Yield();
        _previousSearch = _search;
        _debouncing = true;
        _debounceTimer.Stop();
        _debounceTimer.Start();
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            _search = DefaultSearch;
            GetFocus();
            StateHasChanged();
        }

        base.OnAfterRender(firstRender);
    }

    private void DebounceTimerOnElapsed(object? sender, ElapsedEventArgs e)
    {
        _searchData = _search;

        InvokeAsync(SearchData);
    }

    private async Task SearchData()
    {
        try
        {
            if (!_searching)
            {
                _searching = true;
                var results = await AutoComplete(_searchData);
                _autoItem = results;
                _searching = false;
                _showResults = _autoItem?.Items.Any() ?? false;
            }
        }
        catch (Exception e)
        {
            _autoItem = null;
            _searching = false;
        }

        _debouncing = false;
        StateHasChanged();
    }

    private MarkupString Highlight(string lbl, List<GeoAutoStartEnd> highlights)
    {
        if (highlights == null || !highlights.Any())
            return (MarkupString)lbl;

        var str = new StringBuilder();
        var lastStart = 0;
        if (highlights.Any())
        {
            foreach (var h in highlights)
            {
                if (h.Start != lastStart)
                {
                    str.Append(lbl[lastStart..h.Start]);
                }

                str.Append($"<mark>{lbl[h.Start..h.End]}</mark>");
                lastStart = h.End;
            }

            if (lastStart < lbl.Length)
            {
                str.Append(lbl.Substring(lastStart));
            }

            return (MarkupString)str.ToString();
        }

        return (MarkupString)lbl;
    }

    private async Task ShowResults()
    {
        if (_autoItem != null && _autoItem.Items.Any() && !_showResults)
        {
            _showResults = true;
            GetFocus();
        }

        if (!string.IsNullOrEmpty(_search) && !string.IsNullOrEmpty(_previousSearch))
        {
            _search = _previousSearch;
        }
    }


    private void GetFocus()
    {
        if (_outsideClick == null) return;
        var t = typeof(OutsideClick);
        var method = t.GetMethod("onInsideFocus", BindingFlags.NonPublic | BindingFlags.Instance);
        method.Invoke(_outsideClick, null);
    }

    private void HideResults()
    {
        if (!string.IsNullOrEmpty(_addressSelectedLabel))
        {
            _search = _addressSelectedLabel;
        }

        _showResults = false;
    }

    private async Task SelectAddress(GeoAutoItem item)
    {
        if (string.IsNullOrEmpty(item?.Address?.Label))
            return;

        _showResults = false;
        _search = item?.Address?.Label;
        _addressSelectedLabel = item?.Address?.Label;
        _debouncing = false;
        _debounceTimer.Stop();

        var lookup = await LookupBy(item.Id);

        var address = lookup.Address;
        address.Position = lookup.Position;

        if (OnSelectAddress.HasDelegate)
            OnSelectAddress.InvokeAsync(address);
    }

    private async Task<GeoAutoResult> AutoComplete(string query)
    {
        if (string.IsNullOrEmpty(query))
            return null;

        using var http = new HttpClient();
        http.BaseAddress = new Uri("https://autocomplete.search.hereapi.com/v1/");
        var encodedQuery = WebUtility.UrlEncode(query);
        var lat = CentralLat?.ToString(CultureInfo.InvariantCulture) ?? "37.0902";
        var lng = CentralLng?.ToString(CultureInfo.InvariantCulture) ?? "-95.7129";
        var countryCode = Country.ToString().Replace(" ", "");
        var searchUrl =
            $"autocomplete?q={encodedQuery}&in=countryCode:{countryCode}&at={lat},{lng}&limit=5&apiKey={ApiKey}";
        try
        {
            var str = await http.GetStringAsync(searchUrl);
            var opts = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            var results = JsonSerializer.Deserialize<GeoAutoResult>(str, opts);
            return results;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return null;

    }

    public async Task<GeoAutoItem> LookupBy(string hereId)
    {
        if (string.IsNullOrEmpty(hereId))
            return null;

        using var http = new HttpClient();
        http.BaseAddress = new Uri("https://lookup.search.hereapi.com/v1/");
        var results = await http.GetFromJsonAsync<GeoAutoItem>($"lookup?id={hereId}&apiKey={ApiKey}");

        return results;
    }
}