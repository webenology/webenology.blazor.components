﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace webenology.blazor.components.OutsideClickComponent
{
    public class OutsideClick : ComponentBase, IAsyncDisposable
    {
        [Parameter] public EventCallback OnLoseFocus { get; set; }
        [Parameter] public EventCallback OnGainFocus { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> Attributes { get; set; }

        /// <summary>
        /// The type of html element
        /// <remarks><b>DEFAULT:</b> div</remarks>
        /// <example>tr, span, aside, img</example>
        /// </summary>
        [Parameter]
        public string? Type { get; set; }

        [Parameter] public string? Style { get; set; }
        [Parameter] public string? CssClass { get; set; }

        private IOutsideClickJsHelper js { get; set; }
        [Inject] private IJSRuntime _jsRuntime { get; set; }
        [Inject] private ILogger<OutsideClickJsHelper> _logger { get; set; }
        private ElementReference elRef { get; set; }
        private DotNetObjectReference<OutsideClick> _theInstance;
        private bool _hasFocus;

        protected override void OnInitialized()
        {
            js = new OutsideClickJsHelper(_jsRuntime, _logger);
            _theInstance = DotNetObjectReference.Create(this);
            base.OnInitialized();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                js.Register(elRef, _theInstance);
            }

            base.OnAfterRender(firstRender);
        }

        [JSInvokable]
        public void OnInsideClick()
        {
            _hasFocus = true;
            if (OnGainFocus.HasDelegate)
                OnGainFocus.InvokeAsync();
        }

        [JSInvokable]
        public void OnOutsideClick()
        {
            if (OnLoseFocus.HasDelegate && _hasFocus)
                OnLoseFocus.InvokeAsync();

            _hasFocus = false;
        }


        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if ((Attributes?.ContainsKey("class") ?? false) && !string.IsNullOrEmpty(CssClass))
            {
                var data = (string)Attributes["class"];
                if (!data.Contains(CssClass))
                    Attributes["class"] += " " + CssClass;
            }
            else if (!string.IsNullOrEmpty(CssClass))
            {
                Attributes ??= new Dictionary<string, object>();
                Attributes.Add("class", CssClass);
            }
            builder.OpenElement(0, string.IsNullOrEmpty(Type) ? "div" : Type);
            builder.AddAttribute(1, "style", Style);
            builder.AddMultipleAttributes(2, Attributes);
            builder.AddElementReferenceCapture(3, (Action<ElementReference>)(__value => this.elRef = __value));
            builder.AddContent(4, this.ChildContent);
            builder.CloseElement();
        }

        public async ValueTask DisposeAsync()
        {
            await js.UnRegister(elRef, _theInstance);
            _theInstance.Dispose();
            await js.DisposeAsync();
        }
    }

    public class OutsideClickChild : ComponentBase, IAsyncDisposable
    {
        public async ValueTask DisposeAsync()
        {
            // TODO release managed resources here
        }
    }
}