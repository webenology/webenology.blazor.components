using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

namespace webenology.blazor.components
{
    public class TreeNode
    {
        public bool IsSelected { get; set; }
        public string Node { get; set; }
        public string NodeDescription { get; set; }
        public List<TreeNode> Nodes { get; set; }
        public bool IsExpanded { get; set; }

        public TreeNode()
        {
            Nodes = new();
        }

        public TreeNode(string nodeName) : this()
        {
            Node = nodeName;
        }

        public TreeNode(string nodeName, string nodeDescription) : this()
        {
            Node = nodeName;
            NodeDescription = nodeDescription;
        }
    }
}
