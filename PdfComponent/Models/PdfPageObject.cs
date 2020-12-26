using System.Collections.Generic;

using SelectPdf;

namespace webenology.blazor.components.PdfComponent.Models
{
    public class PdfPageObject
    {
        public List<PdfTextElement> TextElements { get; set; }
    }
}