﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.JSInterop;

namespace webenology.blazor.components
{
    internal interface IToastrJsHelper
    {
        Task ShowToast(string toastType, string body, string header, object options);
    }

    internal class ToastrJsHelper : BaseJsHelper, IToastrJsHelper
    {
        private readonly IJSRuntime _jsRuntime;

        public ToastrJsHelper(IJSRuntime jsRuntime) : base(jsRuntime, "./_content/webenology.blazor.components/js/toastr.min.js")
        {
            _jsRuntime = jsRuntime;
        }

        public async Task ShowToast(string toastType, string body, string header, object options)
        {
            await _jsRuntime.InvokeVoidAsync("console.log", body, header, options);
            var module = await ModuleTask.Value;
            await module.InvokeVoidAsync($"toastr.{toastType}", body, header, options);
        }
    }
}
