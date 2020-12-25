using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webenology.blazor.components
{
    public class ButtonStyle
    {
        public string ButtonCss { get; set; }
        public string ButtonPrimaryCss { get; set; }
        public string ButtonSecondaryCss { get; set; }
        public string ButtonSuccessCss { get; set; }
        public string ButtonDangerCss { get; set; }
        public string SmallCss { get; set; }

        public static ButtonStyle WebenologyStyle => new ButtonStyle
        {
            ButtonCss = "btn",
            ButtonPrimaryCss = "btn-primary",
            ButtonSecondaryCss = "btn-default",
            ButtonDangerCss = "btn-danger",
            ButtonSuccessCss = "btn-success",
            SmallCss = "btn-sm"
        };

    }
}
