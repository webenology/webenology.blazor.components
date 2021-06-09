using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace webenology.blazor.components
{
    internal interface IDateTimerPickerJsHelper
    {
        Task SetupPicker<TRef>(DotNetObjectReference<TRef> instance, ElementReference el, string type, bool time,
            bool isStatic, bool isInline) where TRef : class;
        Task UpdateSettings(ElementReference el, string setting, string value);
        Task OpenCalendar(ElementReference el);
    }

    internal class DateTimerPickerJsHelper : BaseJsHelper, IDateTimerPickerJsHelper
    {
        public DateTimerPickerJsHelper(IJSRuntime jsRuntime) : base(jsRuntime, "./_content/webenology.blazor.components/js/datetimepicker.js")
        {
            jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./_content/webenology.blazor.components/js/flatpickr.min.js");
        }

        public async Task SetupPicker<TRef>(DotNetObjectReference<TRef> instance, ElementReference el, string type,
            bool time, bool isStatic, bool isInline) where TRef : class
        {
            var module = await ModuleTask.Value;
            await module.InvokeVoidAsync("setupPicker", instance, el, type, time, isStatic, isInline);
        }
        public async Task UpdateSettings(ElementReference el, string setting, string value)
        {
            var module = await ModuleTask.Value;
            await module.InvokeVoidAsync("UpdateSetting", el, setting, value);
        }

        public async Task OpenCalendar(ElementReference el)
        {
            var module = await ModuleTask.Value;
            await module.InvokeVoidAsync("openCalendar", el);
        }
    }
}
