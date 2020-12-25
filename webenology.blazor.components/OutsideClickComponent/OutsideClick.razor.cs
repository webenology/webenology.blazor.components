using System;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace webenology.blazor.components
{
    public partial class OutsideClick : IDisposable
    {
        [Parameter]
        public EventCallback OnOutsideClick { get; set; }
        [Parameter]
        public EventCallback OnInsideFocus { get; set; }
        [Parameter]
        public RenderFragment ChildContent { get; set; }
        [Inject]
        private IOutsideClickJsHelper js { get; set; }
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

        public void Dispose()
        {
            js.RemoveInstance(elRef);
            _theInstance?.Dispose();
        }
    }
}
