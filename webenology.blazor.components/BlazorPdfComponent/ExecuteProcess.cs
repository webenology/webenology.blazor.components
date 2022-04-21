using PuppeteerSharp;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace webenology.blazor.components.BlazorPdfComponent
{
    public interface IExecuteProcess
    {
        Task GeneratePdf(string html, string tempFile, PdfOptions pdfOptions);
    }
    public class ExecuteProcess : IExecuteProcess
    {
        private readonly ILogger<ExecuteProcess> _logger;

        public ExecuteProcess(ILogger<ExecuteProcess> logger)
        {
            _logger = logger;
        }

        public async Task GeneratePdf(string html, string tempFile, PdfOptions pdfOptions)
        {
            var results = await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
            
            _logger.LogDebug("pdf folder {0} and url {1}", results.FolderPath, results.Url);

            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
            });
            await using var page = await browser.NewPageAsync();
            await page.SetContentAsync(html);

            await page.PdfAsync(tempFile, pdfOptions);

        }
    }
}
