using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

namespace webenology.blazor.components
{
    public partial class TreeViewItem
    {
        [CascadingParameter] private TreeView _treeView { get; set; }
        [Parameter]
        public List<TreeNode> TreeNodes { get; set; }
        [Parameter]
        public TreeNode ParentNode { get; set; }
        [Parameter]
        public TreeViewStyle CssStyle { get; set; }

        public void ToggleCheck(TreeNode t)
        {
            if (!_treeView.Selectable)
            {
                if (t.Nodes.Any())
                    t.IsExpanded = !t.IsExpanded;
                return;
            }

            var anySelected = t.Nodes.Where(x => !x.IsDisabled).ToList().AreAnySelected(new List<bool>());
            var allSelected = t.Nodes.Where(x => !x.IsDisabled).ToList().AreAllSelected(new List<bool>());

            if (anySelected || !allSelected)
            {
                t.IsSelected = true;
            }
            else
            {
                t.IsSelected = !t.IsSelected;
            }

            if (allSelected && ParentNode != null)
            {
                ParentNode.IsSelected = true;
            }
            t.Nodes.ToggleCheck(t.IsSelected);
            _treeView.ChangeNodeSelection();
            StateHasChanged();
        }


        protected override void OnInitialized()
        {
            if (_treeView.StartExpanded)
            {
                foreach (var t in TreeNodes)
                {
                    t.IsExpanded = true;
                }
            }
            base.OnInitialized();
        }
    }
}
