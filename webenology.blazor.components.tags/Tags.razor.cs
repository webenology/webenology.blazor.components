using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace webenology.blazor.components.tags;
public partial class Tags
{
    [Parameter] public string? TagsList { get; set; }
    [Parameter]
    public EventCallback<string> TagsListChanged { get; set; }
    [Parameter] public char Separator { get; set; } = ',';
    [Parameter] public string CustomCss { get; set; }
    [Inject] private IJSRuntime jsRuntime { get; set; }
    private string Tag { get; set; } = default!;
    private List<string> _tags = new();
    private ElementReference _input;
    private TagsJs js { get; set; }
    private bool _showAll;


    protected override void OnInitialized()
    {
        var dotnetRef = DotNetObjectReference.Create(this);
        js = new TagsJs(dotnetRef, jsRuntime);
        base.OnInitialized();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await js.PreventEnter(_input);
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    protected override void OnParametersSet()
    {
        var tags = SplitTags(TagsList!);
        if (!AreTagsEqual(_tags, tags))
        {
            _tags = tags;
            StateHasChanged();
        }
        base.OnParametersSet();
    }


    [JSInvokable("OnAddTag")]
    public Task OnAddTag()
    {
        if(string.IsNullOrWhiteSpace(Tag)) return Task.CompletedTask;

        var tags = JoinTags(_tags, Tag);
        if (TagsListChanged.HasDelegate)
            TagsListChanged.InvokeAsync(tags);

        Tag = string.Empty;
        StateHasChanged();
        return Task.CompletedTask;
    }

    private List<string> SplitTags(string tags)
    {
        return tags?.Split(Separator).Where(x=> !string.IsNullOrEmpty(x)).ToList() ?? new();
    }

    private string JoinTags(List<string> tags, string? newTag)
    {
        var str = string.Join(Separator, tags);
        if (tags.Any(x => x.Equals(newTag, StringComparison.OrdinalIgnoreCase)))
            return str;

        if (string.IsNullOrWhiteSpace(newTag)) return str;

        if (tags.Count > 0)
            str += Separator;
        str += newTag;

        return str;
    }

    private bool AreTagsEqual(List<string> tag1, List<string> tag2)
    {
        var diff = tag2.Except(tag1);
        return !diff.Any();
    }

    private Task RemoveTag(string s)
    {
        _tags.Remove(s);
        var tags = JoinTags(_tags, null);
        if (TagsListChanged.HasDelegate)
            TagsListChanged.InvokeAsync(tags);
        if (_tags.Count <= 3 && _showAll)
            _showAll = false;

        return Task.CompletedTask;
    }
}
