﻿using System.Timers;

using Maps;

using Microsoft.AspNetCore.Components;

using webenology.blazor.components.MapAutocompleteComponent.Search;
using webenology.blazor.components.OutsideClickComponent;
using webenology.blazor.components.shared;

namespace webenology.blazor.components.MapAutocompleteComponent;

public partial class MapAutocomplete
{
    [Parameter] public string DefaultSearch { get; set; }
    [Parameter] public EventCallback<GeoAutoAddress> OnSelectAddress { get; set; }
    [Parameter] public EventCallback<List<GeoAutoAddress>> OnApiSelectResults { get; set; }
    [Parameter] public bool IsDisabled { get; set; }
    [Parameter] public double? CentralLat { get; set; }
    [Parameter] public double? CentralLng { get; set; }
    [Parameter] public string? HereMapsApiKey { get; set; }
    [Parameter] public string? GoogleApiKey { get; set; }
    [Parameter] public bool AutoHighlight { get; set; } = true;

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

    private System.Timers.Timer _debounceTimer;
    private string _searchData;
    private bool _debouncing;
    private bool _searching;
    private bool _showResults;
    private List<GeoAutoAddress> _autoItem;
    private OutsideClick _outsideClick;

    private ISearch? _searchService;

    protected override void OnInitialized()
    {
        _debounceTimer = new System.Timers.Timer(TimeSpan.FromMilliseconds(300));
        _debounceTimer.Elapsed += DebounceTimerOnElapsed;
        _debounceTimer.AutoReset = false;

        base.OnInitialized();
    }

    protected override void OnParametersSet()
    {
        if (!string.IsNullOrEmpty(GoogleApiKey) && _searchService == null)
        {
            _searchService = new GoogleMapsSearch(new MapSettings
            {
                CenterLat = CentralLat ?? 37.0902,
                CenterLng = CentralLng ?? -95.7129,
                ApiKey = GoogleApiKey
            });
        }
        if (!string.IsNullOrEmpty(HereMapsApiKey) && _searchService == null)
        {
            _searchService = new HereMapsSearch(new MapSettings
            {
                CenterLat = CentralLat ?? 37.0902,
                CenterLng = CentralLng ?? -95.7129,
                ApiKey = HereMapsApiKey
            });
        }
        base.OnParametersSet();
    }

    public void ApiSearch(string searchStr, TimeSpan debounceTime)
    {
        if (_debounceTimer.Interval != debounceTime.TotalMilliseconds)
        {
            _debounceTimer.Interval = debounceTime.TotalMilliseconds;
        }
        _search = searchStr;
        DoSearch();
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
                var results = await _searchService!.Search(_searchData);
                _autoItem = results;
                _searching = false;
                _showResults = _autoItem?.Any() ?? false;
                if (OnApiSelectResults.HasDelegate)
                    await OnApiSelectResults.InvokeAsync(results);
            }
        }
        catch (Exception e)
        {
            _autoItem = null;
            if (OnApiSelectResults.HasDelegate)
                await OnApiSelectResults.InvokeAsync(null);
            _searching = false;
        }

        _debouncing = false;
        StateHasChanged();
    }

    private async Task ShowResults()
    {
        if (_autoItem != null && _autoItem.Any() && !_showResults)
        {
            _showResults = true;
        }

        if (!string.IsNullOrEmpty(_search) && !string.IsNullOrEmpty(_previousSearch))
        {
            _search = _previousSearch;
        }
    }

    private void HideResults()
    {
        if (!string.IsNullOrEmpty(_addressSelectedLabel))
        {
            _search = _addressSelectedLabel;
        }

        _showResults = false;
    }

    public async Task SelectAddress(GeoAutoAddress g)
    {
        if (string.IsNullOrEmpty(g.Label))
            return;

        _showResults = false;
        _search = g.Label;
        _addressSelectedLabel = g.Label;
        _debouncing = false;
        _debounceTimer.Stop();

        if (g.Position == null && !string.IsNullOrEmpty(g.Id))
        {
            var results = (await _searchService.LookupBy(g.Id));
            var address = results.Address;
            address.Position = results.Position;
            g.Label = !string.IsNullOrEmpty(results.Address.Label) ? results.Address.Label : g.Label;
            g.HouseNumber = !string.IsNullOrEmpty(results.Address.HouseNumber) ? results.Address.HouseNumber : g.HouseNumber;
            g.City = !string.IsNullOrEmpty(results.Address.City) ? results.Address.City : g.City;
            g.PostalCode = !string.IsNullOrEmpty(results.Address.PostalCode) ? results.Address.PostalCode : g.PostalCode;
            g.State = !string.IsNullOrEmpty(results.Address.State) ? results.Address.State : g.State;
            g.StateCode = !string.IsNullOrEmpty(results.Address.StateCode) ? results.Address.StateCode : g.StateCode;
            g.Street = !string.IsNullOrEmpty(results.Address.Street) ? results.Address.Street : g.Street;
            g.Suite = !string.IsNullOrEmpty(results.Address.Suite) ? results.Address.Suite : g.Suite;
            if (_addressSelectedLabel != g.Label)
                _addressSelectedLabel = g.Label;
        }

        if (OnSelectAddress.HasDelegate)
            await OnSelectAddress.InvokeAsync(g);
    }

    private MarkupString GetLabel(GeoAutoAddress g)
    {
        if (AutoHighlight)
        {
            return (MarkupString)g.Label.Highlight(_searchData, searchSubstitute: Helpers.Substitutions);
        }

        return (MarkupString)g.LabelHighlighted;
    }

    private async Task AfterBlur()
    {
        if (string.IsNullOrEmpty(Search))
        {
            var g = new GeoAutoAddress();
            if (OnSelectAddress.HasDelegate)
                await OnSelectAddress.InvokeAsync(g);
        }
    }
}