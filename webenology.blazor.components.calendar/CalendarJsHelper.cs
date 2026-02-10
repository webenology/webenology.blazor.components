using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;

namespace webenology.blazor.components.calendar;
internal class CalendarJsHelper
{
    public Lazy<Task<IJSObjectReference>> _moduleTask { get; set; }
    private IJSObjectReference _classReference;

    public CalendarJsHelper(IJSRuntime jsRuntime)
    {
        _moduleTask = new(() =>
            jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/webenology.blazor.components.calendar/js/calendar.js?dt=1_22_25").AsTask());
    }

    public async Task PositionCalendar(ElementReference el)
    {
        var module = await _moduleTask.Value;
        await module.InvokeVoidAsync("PositionCalendar", el);
    }

    public async Task SelectAll(ElementReference el)
    {
        var module = await _moduleTask.Value;
        await module.InvokeVoidAsync("SelectAll", el);
    }

    public async Task StopPropagationOnEnter(ElementReference el, DotNetObjectReference<WebenologyDatePicker> reference)
    {
        var module = await _moduleTask.Value;
        await module.InvokeVoidAsync("StopPropagation", el, reference);
    }
}
