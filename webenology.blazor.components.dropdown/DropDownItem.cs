using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace webenology.blazor.components.dropdown;

public class DropDownItem<T> where T : IConvertible
{
    public T Key { get; set; }
    public string Value { get; set; }
    internal bool IsSelected { get; set; }
    public bool IsDisabled { get; set; }
}