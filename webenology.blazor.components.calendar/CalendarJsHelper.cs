using Microsoft.JSInterop;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace webenology.blazor.components.calendar;
internal class CalendarJsHelper
{
    public Lazy<Task<IJSObjectReference>> _moduleTask { get; set; }
    private IJSObjectReference _classReference;

    public CalendarJsHelper(IJSRuntime jsRuntime)
    {
        _moduleTask = new(() =>
            jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/webenology.blazor.components.calendar/js/calendar.js").AsTask());
    }

    public async Task PositionCalendar(ElementReference el)
    {
        var module = await _moduleTask.Value;
        await module.InvokeVoidAsync("PositionCalendar", el);
    }
}
