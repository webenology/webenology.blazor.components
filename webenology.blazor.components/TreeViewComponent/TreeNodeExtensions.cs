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

        public static bool AreAllSelected(this List<TreeNode> nodes, List<bool> selected)
        {
            foreach (var t in nodes)
            {
                selected.Add(t.IsSelected);
                AreAllSelected(t.Nodes, selected);
            }

            return selected.All(x => x);
        }

        public static bool AreAnySelected(this List<TreeNode> nodes, List<bool> selected)
        {
            foreach (var t in nodes)
            {
                selected.Add(t.IsSelected);
                AreAnySelected(t.Nodes, selected);
            }

            return !selected.All(x => x) && selected.Any(x => x);
        }
    }
}
