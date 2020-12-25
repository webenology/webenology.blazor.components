using System.Collections.Generic;

using Microsoft.AspNetCore.Components;

namespace webenology.blazor.components.TabComponent
{
    public partial class Tabs
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        public Tab ActivePage;
        public List<Tab> TabPages = new();

        public void AddPage(Tab tab)
        {
            TabPages.Add(tab);
            StateHasChanged();
        }

        public void ActivatePage(Tab tab)
        {
            ActivePage = tab;
            StateHasChanged();
        }

    }
}
