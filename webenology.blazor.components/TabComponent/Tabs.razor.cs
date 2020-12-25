using System.Collections.Generic;

using Microsoft.AspNetCore.Components;

using webenology.blazor.components.TabComponent;

namespace webenology.blazor.components
{
    public partial class Tabs
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }
        [Parameter]
        public TabStyle Style { get; set; } = TabStyle.WebenologyStyle;

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
