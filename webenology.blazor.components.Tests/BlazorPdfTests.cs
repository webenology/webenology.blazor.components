//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection.Emit;
//using System.Text;
//using System.Threading.Tasks;

//using Bunit;
//using Bunit.TestDoubles;

//using Microsoft.JSInterop;

//using Moq;

//using PuppeteerSharp;

//using webenology.blazor.components.BlazorPdfComponent;

//using Xunit;

//namespace webenology.blazor.components.Tests
//{
//    public class BlazorPdfTests
//    {
//        private readonly Mock<IServiceProvider> _mockServiceProvider;
//        private readonly Mock<IHtmlToPdfManager> _mockHtmlToPdfManager;
//        private readonly BlazorPdf _sut;

//        public BlazorPdfTests()
//        {
//            _mockServiceProvider = new Mock<IServiceProvider>();
//            _mockHtmlToPdfManager = new Mock<IHtmlToPdfManager>();

//            _sut = new BlazorPdf(_mockServiceProvider.Object, _mockHtmlToPdfManager.Object);
//        }

//        [Fact]
//        public async Task it_should_render()
//        {
//            var mockJs = new Mock<IJSRuntime>();
//            var service = new WebTextInputJsHelper(mockJs.Object);
//            var markup = string.Empty;

//            _mockServiceProvider.Setup(x => x.GetService(typeof(IWebTextInputJsHelper))).Returns(service);
//            _mockHtmlToPdfManager.Setup(x => x.GeneratePdf(It.IsAny<string>(), "name", null, null, null, null, false, PdfOrHtml.Pdf))
//                .Returns(Task.FromResult("abi did this here"))
//                .Callback<string, string, List<string>, List<string>, PdfOptions, string, bool, PdfOrHtml>(
//                    delegate (string s, string s1, List<string> arg3, List<string> arg4,
//                        PdfOptions arg5, string arg6, bool arg7, PdfOrHtml arg8)
//                    { markup = s; });

//            var results = await _sut.GetBlazorInPdfBase64<WebTextInput>(x =>
//                    x.Add(y => y.Label, "ABC")
//                        .Add(y => y.HighlightOnFocus, false)
//                        .Add(y => y.InputType, WebInputType.text)
//                        .Add(y => y.PlaceHolder, "abc")
//                        .Add(y => y.Text, "ddd")
//                        .Add(y => y.Readonly, false)
//                , "name", null, null);

//            Assert.StartsWith("<div class=\"form-group\" b-m8a5m1i6uj><label class=\"form-label\" b-m8a5m1i6uj>ABC</label><div class=\"input-group\" b-m8a5m1i6uj><input type=\"text\" value=\"ddd\" blazor:oninput=\"1\" class=\"form-control\" placeholder=\"abc\" blazor:onfocus=\"2\" b-m8a5m1i6uj blazor:elementReference=", markup);
//            Assert.Equal("abi did this here", results);
//        }
//    }
//}
