namespace webenology.blazor.components.PdfComponent.Models
{
    public enum PdfSizeUnit
    {
        /// <summary>Specifies the Measurement is in centimeters.</summary>
        Centimeter,
        /// <summary>
        /// Specifies the Measurement is in picas. A pica represents 12 points.
        /// </summary>
        Pica,
        /// <summary>Specifies the unit of measurement is 1 pixel.</summary>
        /// <remarks>Pixel unit is device dependent unit. The result depends on the default Dpi on the machine.</remarks>
        Pixel,
        /// <summary>
        /// Specifies a printer's point (1/72 inch) as the unit of measure.
        /// </summary>
        Point,
        /// <summary>Specifies the inch as the unit of measure.</summary>
        Inch,
        /// <summary>
        /// Specifies the document unit (1/300 inch) as the unit of measure.
        /// </summary>
        Document,
        /// <summary>Specifies the Measurement is in millimeters.</summary>
        Millimeter,
    }
}