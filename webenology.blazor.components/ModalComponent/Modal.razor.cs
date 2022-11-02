using System;
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
        [Parameter] public Func<bool> CanClose { get; set; } = () => true;
        [Parameter]
        public ModalStyle CssStyle { get; set; } = ModalStyle.WebenologyStyle;
        public bool IsOpen => _isOpen;
        
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
            var css = new List<string> { CssStyle.BackdropCss };

            if (!_isOpen)
                css.Add(CssStyle.ModalHideCss);

            return string.Join(" ", css);
        }

        private string GetModalCss()
        {
            var css = new List<string> { CssStyle.ModalCss };


            if (Size == ModalSize.Small)
            {
                css.Add(CssStyle.ModalSizeSmallCss);
            }

            if (Size == ModalSize.Large)
            {
                css.Add(CssStyle.ModalSizeLargeCss);
            }

            if (Size == ModalSize.Default)
            {
                css.Add(CssStyle.ModalSizeDefaultCss);
            }

            if (_showAnimateUp)
                css.Add(CssStyle.ModalAnimateInCss);

            if (_showAnimateAway)
                css.Add(CssStyle.ModalAnimateOutCss);

            return string.Join(" ", css);
        }

        public async Task CloseModal()
        {
            if (CanClose.Invoke())
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
