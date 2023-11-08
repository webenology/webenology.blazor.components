using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

using webenology.blazor.components.shared;

namespace webenology.blazor.components.dropdown;

public partial class DropDown<TValue> : ComponentBase where TValue : IConvertible
{
    [Parameter] public List<DropDownItem<TValue>> Items { get; set; }
    private DropDownItem<TValue> _oldSelectedItem { get; set; }
    [Parameter] public DropDownItem<TValue> SelectedItem { get; set; }
    [Parameter] public EventCallback<DropDownItem<TValue>> SelectedItemChanged { get; set; }
    [Parameter] public RenderFragment<DropDownItem<TValue>> ItemContent { get; set; }
    [Parameter] public EventCallback<string> OnAddNew { get; set; }
    [Parameter] public string Value { get; set; }
    [Inject]
    private IJSRuntime js { get; set; }

    private ElementReference _el;
    private Js _jsObj { get; set; }
    public string Search { get; set; }

    private bool isActive;
    private bool showAdd;
    private bool _shouldFilter;
    private int _originalIndex = -1;

    protected override void OnInitialized()
    {
        _jsObj = new Js(js);

        foreach (var item in Items)
        {
            if (item.Equals(SelectedItem))
            {
                item.IsSelected = true;
                Search = item.Value;
            }

        }

        base.OnInitialized();
    }

    protected override async Task OnParametersSetAsync()
    {
        if (_oldSelectedItem != SelectedItem)
        {
            Items.ForEach(x => x.IsSelected = false);
            _oldSelectedItem = SelectedItem;
            if (SelectedItem != null)
            {
                var item = Items.First(x => x.Equals(SelectedItem));
                item.IsSelected = true;
                Search = item.Value;
            }
        }

        _originalIndex = -1;
        await base.OnParametersSetAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await _jsObj.Register(_el, OnOutsideClick);
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    private void OnOutsideClick()
    {
        isActive = false;
        if (SelectedItem != null)
        {
            Search = SelectedItem.Value;
            if (_originalIndex > -1)
            {
                if (Items.Count > 0)
                {
                    Items.FindAll(x => x.IsSelected).ForEach(x => x.IsSelected = false);

                    Items[_originalIndex].IsSelected = true;
                }
            }
        }

        StateHasChanged();
    }

    private async Task _onSelected(DropDownItem<TValue> item)
    {
        if (item.IsDisabled)
            return;

        if (SelectedItemChanged.HasDelegate)
            await SelectedItemChanged.InvokeAsync(item);

        isActive = false;
    }

    private async Task SetActive()
    {
        if (!isActive)
        {
            isActive = true;
            _shouldFilter = false;
            await Task.Yield();
            await _jsObj.ScrollToActive();
        }
    }

    private IEnumerable<DropDownItem<TValue>> GetSearched()
    {
        showAdd = true;
        var search = string.Empty;
        var hasSelected = false;
        if (_shouldFilter)
            search = Search;
        foreach (var item in Items.Search(search, x => x.Value))
        {
            showAdd = false;
            if (item.IsSelected)
                hasSelected = true;
            yield return item;
        }

        if (hasSelected) yield break;

        var index = GetAvailableIndex(Items, -1, 1);
        Items[index].IsSelected = true;
    }

    private Task UnSelect()
    {
        if (SelectedItemChanged.HasDelegate)
            SelectedItemChanged.InvokeAsync(null);

        Search = string.Empty;
        isActive = false;

        return Task.CompletedTask;
    }

    private Task SetToggle()
    {
        if (isActive)
        {
            OnOutsideClick();
        }
        else
        {
            isActive = true;
        }

        return Task.CompletedTask;
    }

    private Task DoFilter()
    {
        _shouldFilter = true;
        return Task.CompletedTask;
    }

    private async Task OnKeyDown(KeyboardEventArgs args)
    {
        await SetActive();
        if (args.Key == "ArrowUp")
            await _jsObj.SetCursorToEnd();

        if (!(args.Key is "ArrowDown" or "ArrowUp" or "Enter"))
            return;

        var searchedItems = GetSearched().ToList();
        if (!searchedItems.Any())
            return;

        var index = searchedItems.FindIndex(x => x.IsSelected);
        if (index == -1)
            index = GetAvailableIndex(searchedItems, index, 1);

        if (_originalIndex == -1)
        {
            var largeIndex = Items.FindIndex(x => x.IsSelected);
            if (largeIndex > -1)
            {
                _originalIndex = largeIndex;
            }
        }

        switch (args.Key)
        {
            case "ArrowDown":
                {
                    var nextIndex = GetAvailableIndex(searchedItems, index, 1);
                    searchedItems[index].IsSelected = false;
                    searchedItems[nextIndex].IsSelected = true;
                    break;
                }
            case "ArrowUp":
                {
                    var prevIndex = GetAvailableIndex(searchedItems, index, -1);
                    searchedItems[index].IsSelected = false;
                    searchedItems[prevIndex].IsSelected = true;
                    break;
                }
            case "Enter":
                {
                    var item = searchedItems.Find(x => x.IsSelected);
                    if (item != null)
                    {
                        await _onSelected(item);
                    }

                    break;
                }
        }

        await Task.Yield();
        await _jsObj.ScrollToActive();
    }

    private int GetAvailableIndex(List<DropDownItem<TValue>> items, int currentIndex, int goIndex)
    {
        var nextIndex = currentIndex;

        while (true)
        {
            nextIndex += goIndex;
            if (goIndex < 0 && nextIndex < 0)
            {
                nextIndex = GetAvailableIndex(items, nextIndex, 1);
                break;
            }
            if (goIndex > 0 && nextIndex > items.Count - 1)
            {
                nextIndex = GetAvailableIndex(items, nextIndex, -1);
                break;
            }
            if (!items[nextIndex].IsDisabled)
                break;
        }

        return nextIndex;
    }
}