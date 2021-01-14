using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webenology.blazor.components
{
    public class DatePickerStyle
    {
        public static DatePickerStyle WebenologyStyle => new DatePickerStyle
        {
            InputCss = "form-control",
            InputErrorCss = "form-error",
            InputErrorIconHolderCss = "input-group-text error-icon-holder",
            InputErrorIconCss = "mdi mdi-alert-circle-outline",
            DatePickerHolderCss = "form-group",
            DatePickerLabelCss = "form-label",
            InputGroupCss = "input-group",
            ErrorMessageCss = "webenology-error",
            CalendarIconHolderCss = "input-group-text link",
            CalendarIconHolderErrorCss = "error-icon-holder",
            CalendarIcon= "mdi mdi-calendar"
        };

        public string CalendarIconHolderErrorCss { get; set; }
        public string CalendarIcon { get; set; }
        public string CalendarIconHolderCss { get; set; }
        public string InputCss { get; set; }
        public string InputErrorCss { get; set; }
        public string InputErrorIconHolderCss { get; set; }
        public string InputErrorIconCss { get; set; }
        public string DatePickerHolderCss { get; set; }
        public string DatePickerLabelCss { get; set; }
        public string InputGroupCss { get; set; }
        public string ErrorMessageCss { get; set; }
    }
}
