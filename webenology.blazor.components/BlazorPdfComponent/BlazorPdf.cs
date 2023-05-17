using Bunit;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using PuppeteerSharp;

namespace webenology.blazor.components.BlazorPdfComponent
{
    public interface IBlazorPdf
    {
        Task<string> GetBlazorInPdfBase64<TValue>(Action<ComponentParameterCollectionBuilder<TValue>> cParams,
            string fileName, List<string> cssFiles, List<string> jsFiles, PdfOptions pdfOptions = null, string baseUrl = "", bool useBaseUrl = false,
            PdfOrHtml pdfOrHtml = PdfOrHtml.Pdf, string waitForElement = "", int waitForElementTimeoutMs = 1000) where TValue : IComponent;
    }
    public class BlazorPdf : IBlazorPdf, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHtmlToPdfManager _htmlToPdfManager;

        public BlazorPdf(IServiceProvider serviceProvider, IHtmlToPdfManager htmlToPdfManager)
        {
            _serviceProvider = serviceProvider;
            _htmlToPdfManager = htmlToPdfManager;
        }

        /// <summary>
        /// Get pdf back from a Blazor Component
        /// </summary>
        /// <typeparam name="TValue">Blazor Component Type</typeparam>
        /// <param name="cParams">Component Parameters</param>
        /// <param name="fileName">Title</param>
        /// <param name="cssFiles">CSS files location</param>
        /// <param name="jsFiles">JS files locations</param>
        /// <param name="pdfOptions">PDF Options</param>
        /// <param name="baseUrl">Base URL to locate all static files</param>
        /// <param name="useBaseUrl">Use explicit base url</param>
        /// <param name="pdfOrHtml">Return as PDF or HTML</param>
        /// <param name="waitForElement">Wait until this element is visible</param>
        /// <param name="waitForElementTimeoutMs">Max wait for element timeout</param>
        /// <returns></returns>
        public async Task<string> GetBlazorInPdfBase64<TValue>(
            Action<ComponentParameterCollectionBuilder<TValue>> cParams, string fileName, List<string> cssFiles,
            List<string> jsFiles, PdfOptions pdfOptions = null, string baseUrl = "", bool useBaseUrl = false,
            PdfOrHtml pdfOrHtml = PdfOrHtml.Pdf, string waitForElement = "", int waitForElementTimeoutMs = 1000) where TValue : IComponent
        {
            using (var ctx = new TestContext())
            {
                ctx.Services.AddFallbackServiceProvider(_serviceProvider);

                using (var results = ctx.RenderComponent(cParams))
                {
                    if (!string.IsNullOrEmpty(waitForElement))
                        results.WaitForElement(waitForElement, TimeSpan.FromMilliseconds(waitForElementTimeoutMs));
                
                    return await _htmlToPdfManager.GeneratePdf(results.Markup, fileName, cssFiles, jsFiles, pdfOptions, baseUrl, useBaseUrl, pdfOrHtml);
                }
            }
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
