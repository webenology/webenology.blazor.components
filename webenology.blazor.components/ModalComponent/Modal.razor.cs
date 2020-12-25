using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace webenology.blazor.components.ModalComponent
{
    public partial class Modal
    {
        [Parameter]
        public string HeaderTitle { get; set; }
        [Parameter]
        public string SubHeaderTitle { get; set; }
        [Parameter]
        public RenderFragment ChildContent { get; set; }
        [Parameter]
        public bool DisableBackgroundCloseOnClick { get; set; }
        [Parameter]
        public ModalSize Size { get; set; }
        [Inject]
        private IModalJsHelper js { get; set; }

        private bool _isOpen;
        private bool _showAnimateUp;
        private bool _showAnimateAway;
        public ModalStyle ModalStyle = new();
        private ElementReference _modal;
        public enum ModalSize
        {
            Default,
            Small,
            Large,
        }

        private string GetBackdropCss()
        {
            var css = new List<string> { ModalStyle.BackdropCss.IfNullOrEmpty("webenology-backdrop") };

            if (!_isOpen)
                css.Add(ModalStyle.ModalHideCss.IfNullOrEmpty("hide"));

            return string.Join(" ", css);
        }

        private string GetModalCss()
        {
            var css = new List<string> { ModalStyle.ModalCss.IfNullOrEmpty("webenology-modal") };


            if (Size == ModalSize.Small)
            {
                css.Add(ModalStyle.ModalSizeSmallCss.IfNullOrEmpty("webenology-modal-sm"));
            }

            if (Size == ModalSize.Large)
            {
                css.Add(ModalStyle.ModalSizeLargeCss.IfNullOrEmpty("webenology-modal-lg"));
            }

            if (Size == ModalSize.Default)
            {
                css.Add(ModalStyle.ModalSizeDefaultCss.IfNullOrEmpty(""));
            }

            if (_showAnimateUp)
                css.Add(ModalStyle.ModalAnimateInCss.IfNullOrEmpty("animate-up"));

            if (_showAnimateAway)
                css.Add(ModalStyle.ModalAnimateOutCss.IfNullOrEmpty("animate-away"));

            return string.Join(" ", css);
        }

        public async Task CloseModal()
        {
            _showAnimateAway = true;
            _showAnimateUp = false;
            StateHasChanged();
            await Task.Run(() =>
            {
                Thread.Sleep(300);
                _isOpen = false;
                _showAnimateAway = false;
                InvokeAsync(StateHasChanged);
            });
        }

        private async Task CloseByBackdrop()
        {
            if (!DisableBackgroundCloseOnClick)
                await CloseModal();
        }

        public async Task OpenModal()
        {
            _isOpen = true;
            _showAnimateAway = false;
            await Task.Run(() =>
            {
                Thread.Sleep(100);
                _showAnimateUp = true;
                InvokeAsync(StateHasChanged);
            });
        }

        internal void AddModalStyle(ModalStyle style)
        {
            ModalStyle = style;
            StateHasChanged();
        }
    }
}
