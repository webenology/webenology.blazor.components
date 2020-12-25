using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webenology.blazor.components.TextInputComponent
{
    public class WebTextInputStyle
    {
        public static WebTextInputStyle WebenologyStyle => new WebTextInputStyle
        {
            WebFormGroupCss = "form-group",
            LabelCss = "form-label",
            WebInputCss = "form-control",
            WebInputErrorCss = "form-error",
            ErrorMessageCss = "error-message",
            WebInputGroupCss = "input-group",
            WebInputGroupTextCss = "input-group-text form-error",
            WebInputGroupTextIconCss = "mdi mdi-alert-circle-outline"
        };

        public string WebFormGroupCss { get; set; }
        public string LabelCss { get; set; }
        public string WebInputCss { get; set; }
        public string WebInputErrorCss { get; set; }
        public string ErrorMessageCss { get; set; }
        public string WebInputGroupCss { get; set; }
        public string WebInputGroupTextCss { get; set; }
        public string WebInputGroupTextIconCss { get; set; }
    }
}
