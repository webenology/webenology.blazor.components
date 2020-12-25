using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

namespace webenology.blazor.components.ModalComponent
{
    public partial class ModalStyle
    {
        [CascadingParameter]
        public Modal modal { get; set; }
        [Parameter]
        public string BackdropCss { get; set; }
        [Parameter]
        public string ModalCss { get; set; }
        [Parameter]
        public string ModalSizeDefaultCss { get; set; }
        [Parameter]
        public string ModalSizeSmallCss { get; set; }
        [Parameter]
        public string ModalSizeLargeCss { get; set; }
        [Parameter]
        public string ModalBodyCss { get; set; }
        [Parameter]
        public string ModalFooterCss { get; set; }
        [Parameter]
        public string ModalHeaderCss { get; set; }
        [Parameter]
        public string ModalHeaderTitleCss { get; set; }
        [Parameter]
        public string ModalHeaderSubheadingCss { get; set; }
        [Parameter]
        public string ModalCloseCss { get; set; }
        [Parameter]
        public string ModalCloseIconCss { get; set; }
        [Parameter]
        public string ModalHideCss { get; set; }
        [Parameter]
        public string ModalAnimateInCss { get; set; }
        [Parameter]
        public string ModalAnimateOutCss { get; set; }


        protected override void OnInitialized()
        {
            if (modal == null)
                throw new ArgumentNullException("Modal style can only be a child of a modal.");

            modal.AddModalStyle(this);

            base.OnInitialized();
        }
    }
}
