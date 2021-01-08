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
            UlCss = "list-group list-group-flush no-margins",
            LiCss = "list-group-item",
            InputCss = "form-check-input",
            SelectAllCheckedCss = "mdi mdi-file-tree mdi-18px",
            SelectAllUncheckedCss = "mdi mdi-file-tree-outline mdi-18px",
            LinkCss = "link",
            LiHolderCss = "li-holder",
            MinusCss = "mdi mdi-minus mdi-24px",
            PlusCss = "mdi mdi-plus mdi-24px",
            EmptyStyle = "display: inline-block; width: 24px;",
            CheckedCss = "mdi mdi-checkbox-marked-outline mdi-24px",
            IntermediateCss = "mdi mdi-checkbox-intermediate mdi-24px",
            UncheckedCss = "mdi mdi-checkbox-blank-outline mdi-24px"
        };

        public string UlCss { get; set; }
        public string LiCss { get; set; }
        public string InputCss { get; set; }
        public string SelectAllCheckedCss { get; set; }
        public string SelectAllUncheckedCss { get; set; }
        public string LinkCss { get; set; }
        public string LiHolderCss { get; set; }
        public string MinusCss { get; set; }
        public string PlusCss { get; set; }
        public string EmptyStyle { get; set; }
        public string CheckedCss { get; set; }
        public string IntermediateCss { get; set; }
        public string UncheckedCss { get; set; }
    }
}
