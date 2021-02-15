using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webenology.blazor.components.DropDownComponent
{
    public class DropDownStyle
    {
        public static DropDownStyle WebenologyDropDownStyle => new DropDownStyle
        {
            SelectForm = "form-select",
            Error = "error"
        };

        public string SelectForm { get; set; }
        public string Error { get; set; }
    }
}
