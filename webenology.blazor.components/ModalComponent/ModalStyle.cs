using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webenology.blazor.components
{
    public class ModalStyle
    {
        public string BackdropCss { get; set; }
        public string ModalCss { get; set; }
        public string ModalSizeDefaultCss { get; set; }
        public string ModalSizeSmallCss { get; set; }
        public string ModalSizeLargeCss { get; set; }
        public string ModalBodyCss { get; set; }
        public string ModalFooterCss { get; set; }
        public string ModalHeaderCss { get; set; }
        public string ModalHeaderTitleCss { get; set; }
        public string ModalHeaderSubheadingCss { get; set; }
        public string ModalCloseCss { get; set; }
        public string ModalCloseIconCss { get; set; }
        public string ModalHideCss { get; set; }
        public string ModalAnimateInCss { get; set; }
        public string ModalAnimateOutCss { get; set; }

        public static ModalStyle WebenologyStyle => new ModalStyle
        {
            BackdropCss = "webenology-backdrop",
            ModalHideCss = "hide",
            ModalCss = "webenology-modal",
            ModalSizeLargeCss = "webenology-modal-lg",
            ModalSizeSmallCss = "webenology-modal-sm",
            ModalAnimateInCss = "animate-up",
            ModalAnimateOutCss = "animate-away",
            ModalBodyCss = "webenology-modal-body",
            ModalFooterCss = "webenology-modal-footer",
            ModalHeaderCss = "webenology-modal-header",
            ModalHeaderTitleCss = "webenology-modal-title",
            ModalHeaderSubheadingCss = "webenology-subheading",
            ModalCloseCss = "webenology-close",
            ModalCloseIconCss = "mdi mdi-close"
        };

    }
}
