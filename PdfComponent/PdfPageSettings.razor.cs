using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

using SelectPdf;

using webenology.blazor.components.PdfComponent;
using webenology.blazor.components.PdfComponent.Models;

namespace webenology.blazor.components
{
    public partial class PdfPageSettings
    {
        [CascadingParameter] public PdfPageModel PdfPageModel { get; set; }
        [Parameter] public PdfSizeUnit? GlobalSizeUnit { get; set; }
        [Parameter]
        public RenderFragment ChildContent { get; set; }
        protected override void OnInitialized()
        {
            this.IfNullThrow(PdfPageModel);

            base.OnInitialized();
        }

        internal void AddOrientation(PageOrientation orientation)
        {
            PdfPageModel.PageSettingsModel.Orientation = (PdfPageOrientation)orientation;
        }

        internal void AddPageSize(float width, float height, PdfSizeUnit? sizeUnit)
        {
            var sU = sizeUnit ?? GlobalSizeUnit ?? PdfSizeUnit.Point;
            PdfPageModel.PageSettingsModel.PageSize = new PdfCustomPageSize(width.ToPoint(sU), height.ToPoint(sU));
        }
        
        internal void AddPageMargins(float left, float right, float top, float bottom, PdfSizeUnit? sizeUnit)
        {
            var sU = sizeUnit ?? GlobalSizeUnit ?? PdfSizeUnit.Point;
            PdfPageModel.PageSettingsModel.Margin = new PdfMargins(left.ToPoint(sU), right.ToPoint(sU), top.ToPoint(sU),
                bottom.ToPoint(sU));
        }
    }
}
