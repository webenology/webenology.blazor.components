using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webenology.blazor.components.JoditEditor;

[AttributeUsage(AttributeTargets.Property)]
public class JoditMergeTag : Attribute
{
    public string Description { get; set; }
    public JoditMergeTag(string description)
    {
        Description = description;
    }
}
