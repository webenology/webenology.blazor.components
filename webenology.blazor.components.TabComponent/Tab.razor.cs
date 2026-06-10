using System.Data;

using Microsoft.AspNetCore.Components;

namespace webenology.blazor.components.TabComponent
{
    public partial class Tab : IDisposable
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
        /// Optional stable identity for the tab, used in the URL fragment and for programmatic
        /// activation. Falls back to <see cref="Title"/> when not set. Provide an Id when the
        /// title is dynamic (e.g. flips between "Original" and "Audited") so links stay stable.
        /// </summary>
        [Parameter]
        public string Id { get; set; }
        /// <summary>
        /// Badge text on tab
        /// </summary>
        [Parameter]
        public string Badge { get; set; }

        /// <summary>The identity used for URL fragments and lookups: <see cref="Id"/> when present, otherwise <see cref="Title"/>.</summary>
        internal string Key => !string.IsNullOrEmpty(Id) ? Id : Title;

        protected override void OnInitialized()
        {
            if (Tabs == null)
                throw new NoNullAllowedException("Tab must be a child of Tabs component.");

            Tabs.AddPage(this);
            Tabs.ActivePage ??= this;

            base.OnInitialized();
        }

        /// <summary>
        /// Unregisters from the parent when a conditionally-rendered tab leaves the render tree —
        /// without this, stale tabs keep their header forever and ActivePage can point at a tab
        /// that no longer renders any content.
        /// </summary>
        public void Dispose()
        {
            Tabs?.RemovePage(this);
        }
    }
}
