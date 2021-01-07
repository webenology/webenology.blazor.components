using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webenology.blazor.components
{
    public class TreeViewStyle
    {
        public static TreeViewStyle WebenologyStyle => new TreeViewStyle
        {
            UlStyle = "list-group list-group-flush",
            LiStyle = "list-group-item",
            InputStyle = "form-check-input"
        };

        public string UlStyle { get; set; }
        public string LiStyle { get; set; }
        public string InputStyle { get; set; }
    }
}
