using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

using webenology.blazor.components.shared;

namespace webenology.blazor.components.dropdown;

public partial class WebenologyDropDown<TValue> : ComponentBase
{
    [Parameter]
    public string? BaseCssClass { get; set; }
    [Parameter] public List<DropDownItem<TValue>> Items { get; set; }
    private DropDownItem<TValue> _oldSelectedItem { get; set; }
    [Parameter] public DropDownItem<TValue> SelectedItem { get; set; }
    [Parameter] public EventCallback<DropDownItem<TValue>> SelectedItemChanged { get; set; }
    [Parameter] public RenderFragment<DropDownItem<TValue>> ItemContent { get; set; }
    [Parameter] public EventCallback<string> OnAddNew { get; set; }
    [Parameter] public string Value { get; set; }
    [Parameter] public string? Placeholder { get; set; }
    [Parameter] public bool IsDisabled { get; set; }
    [Inject]
    private IJSRuntime js { get; set; }

    private ElementReference el;
    private ElementReference inputEl;

    private Js _jsObj { get; set; }
    public string Search { get; set; }
    private string _searchText { get; set; } = string.Empty;

    private bool isActive;
    private bool _shouldFilter;
    private int _originalIndex = -1;

    protected override void OnInitialized()
    {
        _jsObj = new Js(js);

        if (Items != null)
        {
            foreach (var item in Items)
            {
                if (item.Equals(SelectedItem))
                {
                    item.IsSelected = true;
                    Search = item.Value;
                }
            }
        }

        base.OnInitialized();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await _jsObj.SetElement(el);
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    protected override async Task OnParametersSetAsync()
    {
        if (_oldSelectedItem != SelectedItem)
        {
            Search = string.Empty;
            _searchText = string.Empty;
            _oldSelectedItem = SelectedItem;
            if (Items != null && Items.Any())
            {
                Items.ForEach(x => x.IsSelected = false);
                if (SelectedItem != null)
                {
                    var item = Items.FirstOrDefault(x => x.Equals(SelectedItem));
                    if (item != null)
                    {
                        item.IsSelected = true;
                        Search = item.Value;
                    }
                }
            }

            _originalIndex = -1;
        }

        await base.OnParametersSetAsync();
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
        _searchText = String.Empty;
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
            if (Items != null && Items.Count(x => x.IsSelected) > 1 && SelectedItem != null)
            {
                foreach (var i in Items.Where(x => x.IsSelected && !x.Equals(SelectedItem)))
                {
                    i.IsSelected = false;
                }
            }

            var index = Items.FindIndex(x => x.IsSelected);
            isActive = true;
            _shouldFilter = false;
            await Task.Yield();
            await _jsObj.ScrollToActive(index);
        }
    }

    private ICollection<DropDownItem<TValue>> GetSearched()
    {
        if (Items == null || !Items.Any())
            return new List<DropDownItem<TValue>>();

        var search = string.Empty;
        var hasSelected = false;
        if (_shouldFilter)
            search = Search;

        var items = Items.Search(search, x => x.Value).ToList();
        hasSelected = items.Any(x => x.IsSelected);
        if (hasSelected)
            return items;

        var index = GetAvailableIndex(Items, -1, 1);
        Items[index].IsSelected = true;

        return items;
    }

    private Task UnSelect()
    {
        if (SelectedItemChanged.HasDelegate)
            SelectedItemChanged.InvokeAsync(null);

        Search = string.Empty;
        isActive = false;

        return Task.CompletedTask;
    }

    private async Task SetToggle()
    {
        if (isActive)
        {
            OnOutsideClick();
        }
        else
        {
            await SetActive();
        }
    }

    private Task DoFilter()
    {
        _shouldFilter = true;
        _searchText = Search;
        return Task.CompletedTask;
    }

    private async Task OnKeyDown(KeyboardEventArgs args)
    {
        await SetActive();
        if (args.Key == "ArrowUp")
            await _jsObj.SetCursorToEnd();

        if (!(args.Key is "ArrowDown" or "ArrowUp" or "Enter"))
        {
            return;
        }

        if (args.CtrlKey && args.Key == "Enter")
        {
            await OnAddNewItem();
            return;
        }

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
                    Search = searchedItems[nextIndex].Value;
                    break;
                }
            case "ArrowUp":
                {
                    var prevIndex = GetAvailableIndex(searchedItems, index, -1);
                    searchedItems[index].IsSelected = false;
                    searchedItems[prevIndex].IsSelected = true;
                    Search = searchedItems[prevIndex].Value;
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

        StateHasChanged();
        var foundIndex = searchedItems.FindIndex(x => x.IsSelected);
        await _jsObj.ScrollToActive(foundIndex);

        if (args.Key == "Enter")
            return;

    }

    private int GetAvailableIndex(List<DropDownItem<TValue>> items, int currentIndex, int goIndex)
    {
        if (!items.Any())
            return 0;

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

    private async Task OnAddNewItem()
    {
        if (OnAddNew.HasDelegate)
            await OnAddNew.InvokeAsync(Search);

        isActive = false;
        await Task.Yield();
    }

    public async ValueTask DisposeAsync()
    {
        await _jsObj.DisposeAsync();
    }

    private bool CanShowAddNew()
    {
        if (!OnAddNew.HasDelegate)
            return false;
        if (Items == null)
            return false;
        if (string.IsNullOrEmpty(Search))
            return false;
        if (Items.Any(x => x.Value.Equals(Search, StringComparison.OrdinalIgnoreCase)))
            return false;

        return true;
    }
}