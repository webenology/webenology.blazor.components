using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webenology.blazor.components
{
    internal static class TreeNodeExtensions
    {
        public static void ToggleCheck(this List<TreeNode> nodes, bool isChecked)
        {
            foreach (var n in nodes)
            {
                n.IsSelected = isChecked;
                ToggleCheck(n.Nodes, isChecked);
            }
        }
    }
}
