using System;
using Microsoft.AspNetCore.Components;

namespace webenology.blazor.components.TabComponent
{
    public partial class Tab
    {
        [CascadingParameter]
        public Tabs Tabs { get; set; }
        [Parameter]
        public RenderFragment ChildContent { get; set; }
        [Parameter]
        public string Title { get; set; }

        protected override void OnInitialized()
        {
            if (Tabs == null)
                throw new ArgumentNullException($"S2Tab can only be part of S2Tabs control");

            Tabs.AddPage(this);
            Tabs.ActivePage ??= this;

            base.OnInitialized();
        }
    }
}
