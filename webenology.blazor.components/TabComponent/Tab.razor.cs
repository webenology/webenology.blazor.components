using System;

using Microsoft.AspNetCore.Components;

namespace webenology.blazor.components
{
    public partial class Tab
    {
        [CascadingParameter]
        public Tabs Tabs { get; set; }
        [Parameter]
        public RenderFragment ChildContent { get; set; }
        [Parameter]
        public string Title { get; set; }
        [Parameter]
        public string Badge { get;set; }

        protected override void OnInitialized()
        {
            this.IfNullThrow(Tabs);

            Tabs.AddPage(this);
            Tabs.ActivePage ??= this;

            base.OnInitialized();
        }
    }
}
