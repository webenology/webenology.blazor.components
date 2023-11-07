using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using webenology.blazor.components.dropdown;

namespace TestApp.Pages;

public partial class NewTests
{
    private List<DropDownItem<string>> Items { get; set; }

    private DropDownItem<string> _selected = null;

    protected override void OnInitialized()
    {
        Items = new List<DropDownItem<string>>();
        for (var i = 500; i > 0; i--)
        {
            var isDisabled = RandomNumberGenerator.GetInt32(5) < 2;
            Items.Add(new DropDownItem<string> { Key = $"abc{i}", Value = $"value{i}", IsDisabled = isDisabled });
        }

        Items[^1].IsDisabled = true;
        Items[^2].IsDisabled = false;
        _selected = Items[^2];

        base.OnInitialized();
    }


    private Task OnSelect(DropDownItem<string> arg)
    {
        _selected = arg;
        return Task.CompletedTask;
    }
}
