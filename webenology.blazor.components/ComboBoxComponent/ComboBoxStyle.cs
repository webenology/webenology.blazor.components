using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webenology.blazor.components
{
    public class ComboBoxStyle
    {
        public static ComboBoxStyle WebenologyStyle = new ComboBoxStyle
        {
            ContainerCss = "form-group",
            LabelCss = "",
            InputGroupCss = "input-group combobox-border",
            InputCss = "form-control",
            InputShowRemoveInputCss = "show-remove-input",
            RemoveIconContainerCss = "input-group-text show-remove-remove",
            RemoveIconCss = "mdi mdi-close-circle show-remove-icon",
            ChevronContainerCss = "input-group-text",
            ChevronIconCss = "mdi mdi-chevron-down",
            ErrorCss = "form-error",
            ListGroupCss = "list-group-container list-group",
            ListGroupHideCss = "hide",
            ListGroupItemCss = "list-group-item list-group-item-action",
            ListGroupItemFocusedCss = "focused",
            ItemNotFoundCss = "list-group-item not-found",
            AddNewContainerCss = "list-group-item add-new",
            AddNewIconCss = "mdi mdi-plus-circle-outline"
        };

        public string ContainerCss { get; set; }
        public string LabelCss { get; set; }
        public string InputGroupCss { get; set; }
        public string InputCss { get; set; }
        public string InputShowRemoveInputCss { get; set; }
        public string RemoveIconContainerCss { get; set; }
        public string RemoveIconCss { get; set; }
        public string ChevronContainerCss { get; set; }
        public string ChevronIconCss { get; set; }
        public string ErrorCss { get; set; }
        public string ListGroupCss { get; set; }
        public string ListGroupHideCss { get; set; }
        public string ListGroupItemCss { get; set; }
        public string ListGroupItemFocusedCss { get; set; }
        public string ItemNotFoundCss { get; set; }
        public string AddNewContainerCss { get; set; }
        public string AddNewIconCss { get; set; }
    }
}
