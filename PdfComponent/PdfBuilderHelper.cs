using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using SelectPdf;

using webenology.blazor.components.PdfComponent.Models;

namespace webenology.blazor.components
{
    internal interface IPdfBuilderHelper
    {
        PdfPageSettingsModel PageSettingsModel { get; set; }
        string BuildPdfToBase64(PdfPageModel pdfPageModel);
    }

    internal class PdfBuilderHelper : IPdfBuilderHelper
    {
        public PdfPageSettingsModel PageSettingsModel { get; set; }

        public string BuildPdfToBase64(PdfPageModel pdfPageModel)
        {
            var pdfDoc = new PdfDocument();
            var pageSettings = pdfPageModel.PageSettingsModel;

            foreach (var page in pdfPageModel.PdfPages)
            {
                var pdfPage = pdfDoc.AddPage(pageSettings.PageSize,
                    pageSettings.Margin, pageSettings.Orientation);

                foreach (var textEl in page.TextElements)
                {
                    pdfPage.Add(textEl);
                }

            }

            //var block = new PdfRectangleElement(0, 0, 100, 100) { BackColor = new PdfColor(50, 50, 50) };
            //pdfPage.Add(block);

            //var font = pdfDoc.AddFont(PdfStandardFont.Helvetica);
            //font.Size = 18;

            //var text = new PdfTextElement(120, 120, 100, 200, "hello there", font);
            //pdfPage.Add(text);

            using var ms = new MemoryStream();
            pdfDoc.Save(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return Convert.ToBase64String(ms.ToArray());
        }
    }

}
