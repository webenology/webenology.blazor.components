using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using webenology.blazor.components.PdfComponent.Models;

namespace webenology.blazor.components.PdfComponent
{
    public static class PdfUnitConverter
    {
        public static float ToPoint(this float num, PdfSizeUnit unitSize)
        {
            if (unitSize == PdfSizeUnit.Inch)
                return num * 72;

            return num;
        }
    }
}
