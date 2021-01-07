using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

namespace webenology.blazor.components
{
    public partial class TreeView
    {
        [Parameter]
        public bool AllowSelectAll { get; set; }
        [Parameter]
        public List<TreeNode> TreeNodes { get; set; }
        [Parameter]
        public TreeViewStyle CssStyle { get; set; } = TreeViewStyle.WebenologyStyle;

        private void ToggleAll(ChangeEventArgs e)
        {
            if (e.Value != null)
            {
                var isChecked = (bool)e.Value;
                TreeNodes.ToggleCheck(isChecked);
            }
        }
    }
}
