using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using webenology.blazor.components.shared;

namespace webenology.blazor.components.dropdown;

public partial class WebenologyDropDown<TValue> : ComponentBase
{
    private List<DropDownItem<TValue>> _filtered = new();
    private int _oldHash;
    private int _originalIndex = -1;
    private bool _shouldFilter;

    private ElementReference el;
    private ElementReference inputEl;

    private bool isActive;
    [Parameter] public string? BaseCssClass { get; set; }
    [Parameter] public List<DropDownItem<TValue>> Items { get; set; }
    private DropDownItem<TValue> _oldSelectedItem { get; set; }
    [Parameter] public DropDownItem<TValue> SelectedItem { get; set; }
    [Parameter] public EventCallback<DropDownItem<TValue>> SelectedItemChanged { get; set; }
    [Parameter] public RenderFragment<DropDownItem<TValue>> ItemContent { get; set; }
    [Parameter] public EventCallback<string> OnAddNew { get; set; }
    [Parameter] public string Value { get; set; }
    [Parameter] public string? Placeholder { get; set; }
    [Parameter] public bool IsDisabled { get; set; }
    [Inject] private IJSRuntime js { get; set; }
    private Js _jsObj { get; set; }
    public string Search { get; set; }
    private string _searchText { get; set; } = string.Empty;

    protected override void OnInitialized()
    {
        _jsObj = new Js(js);

        if (Items != null)
        {
            var found = Items.FirstOrDefault(x => x.Equals(SelectedItem));
            if (found != null)
            {
                found.IsSelected = true;
                Search = found.Value;
            }

            _filtered = Items;
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
        if (Items != null)
        {
            if (Items.GetHashCode() != _oldHash)
            {
                _filtered = Items;
                _oldHash = Items.GetHashCode();
            }
        }

        if (_oldSelectedItem != SelectedItem)
        {
            Search = string.Empty;
            _searchText = string.Empty;
            _oldSelectedItem = SelectedItem;
            if (Items != null && Items.Any())
            {
                if (SelectedItem != null)
                {
                    _filtered = Items;
                    var item = _filtered.FirstOrDefault(x => x.Equals(SelectedItem));
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
        _filtered = Items;
        _filtered.FindAll(x => x.IsSelected).ForEach(x => x.IsSelected = false);
        var found = _filtered.Find(x => x.Equals(SelectedItem));
        if (found != null)
        {
            found.IsSelected = true;
        }

        Search = SelectedItem != null ? SelectedItem.Value : string.Empty;

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
            _filtered.FindAll(x => x.IsSelected).ForEach(x => x.IsSelected = false);
            var found = _filtered.FirstOrDefault(x => x.Equals(SelectedItem));
            if (found != null)
                found.IsSelected = true;

            var index = _filtered.FindIndex(x => x.IsSelected);
            isActive = true;
            _shouldFilter = false;
            await Task.Yield();
            await _jsObj.ScrollToActive(index);
        }
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
        _filtered = Items.Search(_searchText, x => x.Value).ToList();
        var hasSelected = _filtered.Any(x => x.IsSelected);
        if (hasSelected)
            return Task.CompletedTask;

        foreach (var dropDownItem in _filtered.Where(x => x.IsSelected))
        {
            dropDownItem.IsSelected = false;
        }

        var index = GetAvailableIndex(_filtered, -1, 1);
        if (_filtered.Count > 0)
        {
            _filtered[index].IsSelected = true;
        }

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


        var index = _filtered.FindIndex(x => x.IsSelected);
        if (index == -1)
            index = GetAvailableIndex(_filtered, index, 1);

        if (_originalIndex == -1)
        {
            var largeIndex = _filtered.FindIndex(x => x.IsSelected);
            if (largeIndex > -1)
            {
                _originalIndex = largeIndex;
            }
        }

        switch (args.Key)
        {
            case "ArrowDown":
            {
                var nextIndex = GetAvailableIndex(_filtered, index, 1);
                _filtered[index].IsSelected = false;
                _filtered[nextIndex].IsSelected = true;
                Search = _filtered[nextIndex].Value;
                break;
            }
            case "ArrowUp":
            {
                var prevIndex = GetAvailableIndex(_filtered, index, -1);
                _filtered[index].IsSelected = false;
                _filtered[prevIndex].IsSelected = true;
                Search = _filtered[prevIndex].Value;
                break;
            }
            case "Enter":
            {
                var item = _filtered.Find(x => x.IsSelected);
                if (item != null)
                {
                    await _onSelected(item);
                }

                break;
            }
        }

        StateHasChanged();
        var foundIndex = _filtered.FindIndex(x => x.IsSelected);
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
        if (_filtered == null)
            return false;
        if (string.IsNullOrEmpty(Search))
            return false;
        if (_filtered.Any(x => x.Value.Equals(Search, StringComparison.OrdinalIgnoreCase)))
            return false;

        return true;
    }
}