namespace webenology.blazor.components.TreeViewComponent
{
    public class TreeNode
    {
        public bool IsSelected { get; set; }
        public bool IsDisabled { get; set; }
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
