using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

namespace webenology.blazor.components.ModalComponent
{
    public partial class ModalHeader
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }
        [Parameter]
        public bool HideClose { get; set; }
        [Parameter]
        public string Header { get; set; }
        [Parameter]
        public string SubHeader { get; set; }
        [CascadingParameter]
        public Modal Modal { get; set; }

        protected override void OnInitialized()
        {
            if (Modal == null)
                throw new ArgumentNullException("Modal Header must be a child a modal.");

            base.OnInitialized();
        }

        public async Task OnClose()
        {
            await Modal.CloseModal();
        }

        private string GetHeaderCss()
        {
            return Modal.ModalStyle?.ModalHeaderCss.IfNullOrEmpty("webenology-modal-header");
        }

        private string GetModalTitleCss()
        {
            return Modal.ModalStyle?.ModalHeaderTitleCss.IfNullOrEmpty("webenology-modal-title");
        }
        private string GetModalSubheadingCss()
        {
            return Modal.ModalStyle?.ModalHeaderSubheadingCss.IfNullOrEmpty("webenology-subheading");
        }

        private string GetModalCloseCss()
        {
            return Modal.ModalStyle?.ModalCloseCss.IfNullOrEmpty("webenology-close");
        }

        private string GetModalCloseIconCss()
        {
            return Modal.ModalStyle?.ModalCloseIconCss.IfNullOrEmpty("mdi mdi-close");
        }
    }
}
