using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Moq;

using webenology.blazor.components.BlazorPdfComponent;

using Xunit;

namespace webenology.blazor.components.Tests
{
    public class HtmlToPdfManagerTests
    {
        private readonly HtmlToPdfManager _sut;
        private readonly Mock<IExecuteProcess> _mockExecuteProcess;
        private readonly Mock<IWFileWriter> _mockFileWriter;

        public HtmlToPdfManagerTests()
        {
            _mockExecuteProcess = new Mock<IExecuteProcess>();
            _mockFileWriter = new Mock<IWFileWriter>();

            _sut = new HtmlToPdfManager(_mockFileWriter.Object, _mockExecuteProcess.Object);
        }

        [Fact]
        public async Task it_should_generate_pdf_from_html()
        {
            var markup = "<div>hello</div>";
            var title = "abi";
            var byteResults = new byte[2] { 0, 14 };

            _mockFileWriter.SetupSequence(x => x.Exists(It.IsAny<string>())).Returns(true).Returns(false);
            _mockFileWriter.Setup(x => x.GetTempPath()).Returns("abc");
            _mockFileWriter.Setup(x => x.ReadAllBytes(It.IsAny<string>())).Returns(byteResults);

            var results = await _sut.GeneratePdf(markup, title, null, null);

            Assert.Equal("AA4=", results);
            _mockFileWriter.Verify(x => x.GetTempPath(), Times.Once);
            _mockFileWriter.Verify(x=> x.ReadAllBytes(It.IsAny<string>()), Times.Once);
        }
    }
}
