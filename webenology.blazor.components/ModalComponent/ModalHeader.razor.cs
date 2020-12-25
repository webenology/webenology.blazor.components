using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

namespace webenology.blazor.components
{
    public partial class ModalHeader
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }
        [Parameter]
        public bool HideClose { get; set; }
        [Parameter]
        public string Header { get; set; }
        [Parameter]
        public string SubHeader { get; set; }
        [CascadingParameter]
        public Modal Modal { get; set; }

        public async Task OnClose()
        {
            await Modal.CloseModal();
        }
    }
}
