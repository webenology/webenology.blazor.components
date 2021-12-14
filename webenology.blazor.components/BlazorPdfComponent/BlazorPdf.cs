using Bunit;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace webenology.blazor.components.BlazorPdfComponent
{
    public interface IBlazorPdf
    {
        string GetBlazorInPdfBase64<TValue>(Action<ComponentParameterCollectionBuilder<TValue>> cParams,
            string fileName, List<string> cssFiles, List<string> jsFiles) where TValue : IComponent;
    }
    public class BlazorPdf : IBlazorPdf
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHtmlToPdfManager _htmlToPdfManager;

        public BlazorPdf(IServiceProvider serviceProvider, IHtmlToPdfManager htmlToPdfManager)
        {
            _serviceProvider = serviceProvider;
            _htmlToPdfManager = htmlToPdfManager;
        }

        public string GetBlazorInPdfBase64<TValue>(Action<ComponentParameterCollectionBuilder<TValue>> cParams, string fileName, List<string> cssFiles, List<string> jsFiles) where TValue : IComponent
        {
            var ctx = new TestContext();
            ctx.Services.AddFallbackServiceProvider(_serviceProvider);

            var results = ctx.RenderComponent(cParams);

            var markup = results.Markup;

            return _htmlToPdfManager.GeneratePdf(markup, fileName, cssFiles, jsFiles);
        }
    }
}
