using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SelectPdf;

namespace webenology.blazor.components.PdfComponent.Models
{
    public sealed class PdfPageSettingsModel
    {
        public PdfPageOrientation Orientation { get; set; } = PdfPageOrientation.Portrait;
        public PdfSizeUnit PdfSizeUnit { get; set; } = PdfSizeUnit.Point;
        public PdfCustomPageSize PageSize { get; set; } = PdfCustomPageSize.Letter;
        public PdfMargins Margin { get; set; } = PdfMargins.Empty;
    }
}
