using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using HtmlAgilityPack;
using PuppeteerSharp;

namespace webenology.blazor.components.BlazorPdfComponent
{
    public interface IHtmlToPdfManager
    {
        Task<string> GeneratePdf(string markup, string title, List<string> cssFiles, List<string> jsFiles,
            PdfOptions pdfOptions = null, string baseUrl = "", bool useBaseUrl = false,
            PdfOrHtml returnType = PdfOrHtml.Pdf);
    }

    public class HtmlToPdfManager : IHtmlToPdfManager, IDisposable
    {
        private readonly IWFileWriter _fileWriter;
        private readonly IExecuteProcess _executeProcess;

        public HtmlToPdfManager(IWFileWriter fileWriter, IExecuteProcess executeProcess)
        {
            _fileWriter = fileWriter;
            _executeProcess = executeProcess;
        }

        public async Task<string> GeneratePdf(string markup, string title, List<string> cssFiles, List<string> jsFiles,
            PdfOptions pdfOptions = null, string baseUrl = "", bool useBaseUrl = false,
            PdfOrHtml returnType = PdfOrHtml.Pdf)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(markup);
            if (useBaseUrl)
            {
                var baseUrlNode = HtmlNode.CreateNode($"<base href='{baseUrl}'>");
                if (doc.DocumentNode.ChildNodes.FindFirst("head") != null)
                {
                    var headNode = doc.DocumentNode.ChildNodes.FindFirst("head");
                    headNode.AppendChild(baseUrlNode);
                }
                else
                {
                    doc.DocumentNode.InsertBefore(baseUrlNode, doc.DocumentNode.FirstChild);
                }
            }

            if (cssFiles != null)
            {
                foreach (var cssLocation in cssFiles)
                {
                    var style = HtmlNode.CreateNode($"<link rel='stylesheet' href='{cssLocation}'>");
                    doc.DocumentNode.AppendChild(style);
                }
            }

            if (jsFiles != null)
            {
                foreach (var jsLocation in jsFiles)
                {
                    var script = HtmlNode.CreateNode($"<script type='text/javascript' src='{jsLocation}'>");
                    doc.DocumentNode.AppendChild(script);
                }
            }

            if (returnType == PdfOrHtml.Html)
                return doc.DocumentNode.OuterHtml;

            var temp = _fileWriter.GetTempPath();
            var tempFileName = Guid.NewGuid().ToString("N");

            var tempFile = $"{temp}{tempFileName}";
            try
            {
                if (pdfOptions == null)
                {
                    pdfOptions = new PdfOptions
                    {
                        PreferCSSPageSize = true,
                        PrintBackground = false
                    };
                }

                await _executeProcess.GeneratePdf(doc.DocumentNode.OuterHtml, $"{tempFile}.pdf", pdfOptions);
                doc = null;
                if (_fileWriter.Exists($"{tempFile}.pdf"))
                {
                    var bytes = _fileWriter.ReadAllBytes($"{tempFile}.pdf");
                    return Convert.ToBase64String(bytes);
                }
            }
            catch (Exception e)
            {
                return string.Empty;
            }
            finally
            {
                if (_fileWriter.Exists($"{tempFile}.html"))
                    _fileWriter.Delete($"{tempFile}.html");

                if (_fileWriter.Exists($"{tempFile}.pdf"))
                    _fileWriter.Delete($"{tempFile}.pdf");

            }

            return string.Empty;
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }

    public enum PdfOrHtml
    {
        Pdf,
        Html
    }
}