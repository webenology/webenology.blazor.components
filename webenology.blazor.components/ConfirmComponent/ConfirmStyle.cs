using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webenology.blazor.components
{
    public class ConfirmStyle
    {

        public static ConfirmStyle WebenologyStyle => new ConfirmStyle
        {
            BackdropCss = "backdrop",
            HideCss = "hidden",
            BoundInCss = "bounce-in",
            CardCss = "card",
            CardHeaderCss = "card-header",
            CardBodyCss = "card-body",
            CardFooterCss = "card-footer text-right",
            CancelButtonCss = "btn btn-default btn-sm",
            NoButtonCss = "btn btn-danger btn-sm",
            YesButtonCss = "btn btn-primary btn-sm"
        };

        public string BackdropCss { get; set; }
        public string HideCss { get; set; }
        public string BoundInCss { get; set; }
        public string CardCss { get; set; }
        public string CardHeaderCss { get; set; }
        public string CardBodyCss { get; set; }
        public string CardFooterCss { get; set; }
        public string CancelButtonCss { get; set; }
        public string NoButtonCss { get; set; }
        public string YesButtonCss { get; set; }
    }
}
