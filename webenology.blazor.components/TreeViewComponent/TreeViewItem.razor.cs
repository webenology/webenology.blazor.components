using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

namespace webenology.blazor.components
{
    public partial class TreeViewItem
    {
        [Parameter]
        public List<TreeNode> TreeNodes { get; set; }
        [Parameter]
        public TreeViewStyle CssStyle { get; set; }

        public void ToggleCheck(ChangeEventArgs e, TreeNode t)
        {
            if (e.Value != null)
            {
                t.Nodes.ToggleCheck((bool)e.Value);
            }
            StateHasChanged();
        }
    }
}
