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

namespace webenology.blazor.components.BlazorPdfComponent
{
    public interface IHtmlToPdfManager
    {
        string GeneratePdf(string markup, string title, List<string> cssFiles, List<string> jsFiles);
    }
    public class HtmlToPdfManager : IHtmlToPdfManager
    {
        private readonly IExecuteProcess _executeProcess;
        private readonly IWFileWriter _fileWriter;

        public HtmlToPdfManager(IExecuteProcess executeProcess, IWFileWriter fileWriter)
        {
            _executeProcess = executeProcess;
            _fileWriter = fileWriter;
        }

        public string GeneratePdf(string markup, string title, List<string> cssFiles, List<string> jsFiles)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(markup);
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
                    var style = HtmlNode.CreateNode($"<script type='text/javascript' src='{jsLocation}'>");
                    doc.DocumentNode.AppendChild(style);
                }
            }

            var temp = _fileWriter.GetTempPath();
            var tempFileName = Guid.NewGuid().ToString("N");

            var tempFile = $"{temp}{tempFileName}";
            try
            {
                _fileWriter.WriteAllText($"{tempFile}.html", doc.DocumentNode.OuterHtml);
                _executeProcess.Execute(title, tempFile);
                
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
    }
}
