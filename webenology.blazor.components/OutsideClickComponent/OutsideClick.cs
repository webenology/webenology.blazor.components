using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace webenology.blazor.components
{
    public class OutsideClick : ComponentBase, IDisposable
    {
        [Parameter] public EventCallback OnOutsideClick { get; set; }
        [Parameter] public EventCallback OnInsideFocus { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// The type of html element
        /// <remarks><b>DEFAULT:</b> div</remarks>
        /// <example>tr, span, aside, img</example>
        /// </summary>
        [Parameter]
        public string? Type { get; set; }

        [Parameter] public string? Style { get; set; }

        [Inject] private IOutsideClickJsHelper js { get; set; }
        private ElementReference elRef { get; set; }
        private DotNetObjectReference<OutsideClick> _theInstance;

        [JSInvokable]
        public void OnClickOutside()
        {
            OnOutsideClick.InvokeAsync(null);
        }

        private void onFocusIn()
        {
            js.SetFocus(elRef);
        }

        private void onInsideFocus()
        {
            onFocusIn();
            OnInsideFocus.InvokeAsync();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                _theInstance = DotNetObjectReference.Create(this);
                js.Setup(elRef, _theInstance);
            }

            base.OnAfterRender(firstRender);
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, string.IsNullOrEmpty(Type) ? "div" : Type);
            builder.AddAttribute(1, "style", Style);
            builder.AddAttribute<FocusEventArgs>(2, "onfocusin",
                EventCallback.Factory.Create<FocusEventArgs>((object)this, new Action(this.onInsideFocus)));
            builder.AddAttribute<MouseEventArgs>(3, "onclick",
                EventCallback.Factory.Create<MouseEventArgs>((object)this, new Action(this.onInsideFocus)));
            builder.AddElementReferenceCapture(4, (Action<ElementReference>)(__value => this.elRef = __value));
            builder.AddContent(5, this.ChildContent);
            builder.CloseElement();
        }

        public void Dispose()
        {
            js.RemoveInstance(elRef);
            _theInstance?.Dispose();
        }
    }
}