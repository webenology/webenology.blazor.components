using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webenology.blazor.components.PdfComponent.Models
{
    public class PdfPageModel
    {
        public PdfPageSettingsModel PageSettingsModel { get; set; } = new();
        public List<PdfPageObject> PdfPages { get; set; } = new();
    }
}
