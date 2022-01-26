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

namespace webenology.blazor.components.BlazorPdfComponent
{
    public interface IExecuteProcess
    {
        Task GeneratePdf(string html, string tempFile, PdfOptions pdfOptions);
    }
    internal class ExecuteProcess : IExecuteProcess
    {

        public async Task GeneratePdf(string html, string tempFile, PdfOptions pdfOptions)
        {
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });
            await using var page = await browser.NewPageAsync();
            await page.SetContentAsync(html);

            await page.PdfAsync(tempFile, pdfOptions);

        }
    }
}
