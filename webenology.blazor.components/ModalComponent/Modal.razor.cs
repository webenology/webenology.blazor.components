using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace webenology.blazor.components
{
    public partial class Modal
    {
        [Parameter]
        public string HeaderTitle { get; set; }
        [Parameter]
        public string SubHeaderTitle { get; set; }
        [Parameter]
        public bool HeaderHideClose { get; set; }
        [Parameter]
        public RenderFragment BodyContent { get; set; }
        [Parameter]
        public RenderFragment FooterContent { get; set; }
        [Parameter]
        public RenderFragment HeaderContent { get; set; }
        [Parameter]
        public bool DisableBackgroundCloseOnClick { get; set; }
        [Parameter]
        public ModalSize Size { get; set; }
        [Parameter]
        public ModalStyle Style { get; set; } = ModalStyle.WebenologyStyle;

        private bool _isOpen;
        private bool _showAnimateUp;
        private bool _showAnimateAway;
        private ElementReference _modal;
        public enum ModalSize
        {
            Default,
            Small,
            Large,
        }

        private string GetBackdropCss()
        {
            var css = new List<string> { Style.BackdropCss };

            if (!_isOpen)
                css.Add(Style.ModalHideCss);

            return string.Join(" ", css);
        }

        private string GetModalCss()
        {
            var css = new List<string> { Style.ModalCss };


            if (Size == ModalSize.Small)
            {
                css.Add(Style.ModalSizeSmallCss);
            }

            if (Size == ModalSize.Large)
            {
                css.Add(Style.ModalSizeLargeCss);
            }

            if (Size == ModalSize.Default)
            {
                css.Add(Style.ModalSizeDefaultCss);
            }

            if (_showAnimateUp)
                css.Add(Style.ModalAnimateInCss);

            if (_showAnimateAway)
                css.Add(Style.ModalAnimateOutCss);

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
    }
}
