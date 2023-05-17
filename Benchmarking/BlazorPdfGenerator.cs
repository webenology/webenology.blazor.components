using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.JSInterop;
using Moq;
using PuppeteerSharp;
using TestApp.Pages;
using webenology.blazor.components;
using webenology.blazor.components.BlazorPdfComponent;
using webenology.blazor.ss.authentication;

namespace Benchmarking;

[SimpleJob(RuntimeMoniker.Net70)]
[MemoryDiagnoser]
[RPlotExporter]
public class BlazorPdfGenerator
{
    private BlazorPdf _blazorPdf;
    private readonly IServiceScopeFactory _serviceFactory;
    private IServiceScope _serviceScope;
    private readonly IHost _host;

    public BlazorPdfGenerator()
    {
        _host = Host.CreateDefaultBuilder(null)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureServices(services =>
                {
                    services.AddWebenologyAuth();
                    services.AddRazorPages();
                    services.AddServerSideBlazor();
                    services.AddWebenologyHelpers();
                    services.AddScoped<BlazorPdfGenerator>();
                });
            })
            .Build();
        
        _serviceFactory = _host.Services.GetService<IServiceScopeFactory>();
    }

    [GlobalSetup]
    public void Setup()
    {
        _serviceScope = _serviceFactory.CreateScope();
        var htmlToPdfManager = new HtmlToPdfManager(_serviceScope.ServiceProvider.GetService<IWFileWriter>(),
            _serviceScope.ServiceProvider.GetService<IExecuteProcess>());

        _blazorPdf = new BlazorPdf(_serviceScope.ServiceProvider, htmlToPdfManager);
    }

    [Benchmark]
    public async Task CreatePdf()
    {
        var css = new List<string>
        {
            "/css/bootstrap/bootstrap.min.css"
        };
        var baseUrl = "https://localhost:44348";
        var pdfOptions = new PdfOptions
        {
            PrintBackground = true,
            HeaderTemplate = "abc",
            FooterTemplate = "pageNumber of totalPages",
            Landscape = true,
        };
        var results =
            await _blazorPdf.GetBlazorInPdfBase64<Counter>(x => { }, "abc", css, null, pdfOptions, baseUrl, true);
    }
}