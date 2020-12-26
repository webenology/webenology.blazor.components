using System.Drawing;

namespace webenology.blazor.components.PdfComponent.Models
{
    public sealed class WebPageSize
    {
        public static SizeF Other(float width, float height, PdfSizeUnit sizeUnit)
        {

            return new SizeF(width.ToPoint(sizeUnit), height.ToPoint(sizeUnit));
        }
        /// <summary>Letter format.</summary>
        public static readonly SizeF Letter = new SizeF(612f, 792f);

        /// <summary>Legal format.</summary>
        public static readonly SizeF Legal = new SizeF(612f, 1008f);

        /// <summary>A4 format.</summary>
        public static readonly SizeF A4 = new SizeF(595f, 842f);

        /// <summary>11x17 format.</summary>
        public static readonly SizeF Letter11x17 = new SizeF(792f, 1224f);

    }
}