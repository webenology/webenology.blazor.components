using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

using webenology.blazor.components.PdfComponent.Models;

namespace webenology.blazor.components
{
    public partial class PdfPageOrientationModel
    {
        [CascadingParameter]
        private PdfPageSettings _pdfPageSettings { get; set; }

        [Parameter] public PageOrientation Orientation { get; set; } = PageOrientation.Portrait;

        protected override void OnInitialized()
        {
            this.IfNullThrow(_pdfPageSettings);
            _pdfPageSettings.AddOrientation(Orientation);
            base.OnInitialized();
        }
    }
}
