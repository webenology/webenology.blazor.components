using SelectPdf;

namespace webenology.blazor.components.PdfComponent.Models
{
    public sealed class WebPageMargins
    {
        public static PdfMargins All(float all, PdfSizeUnit sizeUnit)
        {
            return new(all.ToPoint(sizeUnit));
        }

        public static PdfMargins Other(float left, float right, float top, float bottom, PdfSizeUnit sizeUnit)
        {
            return new(left.ToPoint(sizeUnit), right.ToPoint(sizeUnit), top.ToPoint(sizeUnit),
                bottom.ToPoint(sizeUnit));
        }
    }
}