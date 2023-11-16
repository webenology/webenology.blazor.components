using System.Data;

using Microsoft.AspNetCore.Components;

namespace webenology.blazor.components.TabComponent
{
    public partial class Tab
    {
        /// <summary>
        /// Tab component must be a child of the <typeparam name="Tabs">Tabs</typeparam> Component
        /// </summary>
        [CascadingParameter]
        public Tabs Tabs { get; set; }
        /// <summary>
        /// Tab Content
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }
        /// <summary>
        /// Title of the tab
        /// </summary>
        [Parameter]
        public string Title { get; set; }
        /// <summary>
        /// Badge text on tab
        /// </summary>
        [Parameter]
        public string Badge { get; set; }

        protected override void OnInitialized()
        {
            if (Tabs == null)
                throw new NoNullAllowedException("Tab must be a child of Tabs component.");

            Tabs.AddPage(this);
            Tabs.ActivePage ??= this;

            base.OnInitialized();
        }
    }
}
