using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webenology.blazor.components
{
    public class WebNumberInputStyle
    {
        public static WebNumberInputStyle WebenologyStyle => new WebNumberInputStyle
        {
            FormGroupCss = "form-group",
            InputCss = "form-control",
            InputErrorCss = "error",
            InputGroupCss = "input-group",
            InputGroupAddonCss = "input-group-text",
            InputGroupAddonErrorCss = "error",
            InputGroupAddonIconCss = "mdi mdi mdi-alert-circle-outline",
            ErrorCss = "form-error"
        };

        public string FormGroupCss { get; set; }
        public string FormGroupLabelCss { get; set; }
        public string InputCss { get; set; }
        public string InputErrorCss { get; set; }
        public string InputGroupCss { get; set; }
        public string InputGroupAddonCss { get; set; }
        public string InputGroupAddonIconCss { get; set; }
        public string InputGroupAddonErrorCss { get; set; }
        public string ErrorCss { get; set; }
    }
}
