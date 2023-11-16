using System.Net;
using Microsoft.AspNetCore.Components;

namespace webenology.blazor.components.TabComponent
{
    public partial class Tabs : IDisposable
    {
        /// <summary>
        /// Tabs content
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }
        /// <summary>
        /// Custom CSS Style, default <typeparam name="TabStyle.WebenologyStyle"></typeparam>
        /// </summary>
        [Parameter]
        public TabStyle CssStyle { get; set; } = TabStyle.WebenologyStyle;
        [Inject]
        private NavigationManager nav { get; set; }
        /// <summary>
        /// Active Tab
        /// </summary>
        public Tab ActivePage;

        /// <summary>
        /// List of all tabs
        /// </summary>
        /// <example>new list</example>
        /// <example>new list 2</example>
        /// <example>new list 3</example>
        /// <code>List&lt;Tab&gt;</code>
        /// <returns>List&lt;Tab&gt;</returns>
        public List<Tab> TabPages = new();
        private string _oldUrl;
        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                nav.LocationChanged += Nav_LocationChanged;
                CheckCurrentUri(nav.Uri);
            }

            base.OnAfterRender(firstRender);
        }

        private void Nav_LocationChanged(object sender, Microsoft.AspNetCore.Components.Routing.LocationChangedEventArgs e)
        {
            CheckCurrentUri(e.Location);
        }

        private void CheckCurrentUri(string location)
        {
            var uri = new Uri(location);
            if (!string.IsNullOrEmpty(uri.Fragment) && uri.Fragment.StartsWith("#tab"))
            {
                var tabName = WebUtility.UrlDecode(uri.Fragment).Substring(1);
                int.TryParse(tabName.Split(":")[1], out var index);
                var tab = TabPages[index];
                if (tab != null && ActivePage != tab)
                {
                    ActivePage = tab;
                    StateHasChanged();
                }
            }
            else
            {
                ActivePage = TabPages[0];
                StateHasChanged();
            }
        }

        /// <summary>
        /// Add a new tab to list of tabs
        /// </summary>
        /// <param name="tab">Tab object</param>
        public void AddPage(Tab tab)
        {
            TabPages.Add(tab);
            StateHasChanged();
        }

        /// <summary>
        /// Active a tab
        /// </summary>
        /// <param name="tab">Tab object</param>
        public void ActivatePage(Tab tab)
        {
            var tabIndex = TabPages.IndexOf(tab);
            SetNewUrl(tabIndex);
            ActivePage = tab;
            StateHasChanged();
        }

        private void SetNewUrl(int tabIndex)
        {
            var uri = new Uri(nav.Uri);
            var splitUri = uri.OriginalString;
            if (!string.IsNullOrEmpty(uri.Fragment))
            {
                splitUri = splitUri.Replace(uri.Fragment, "");
            }
            var newTitle = WebUtility.UrlEncode($"tab:{tabIndex}");
            var newUrl = $"{splitUri}#{newTitle}";
            if (newUrl != _oldUrl)
            {
                _oldUrl = newUrl;
                nav.NavigateTo(newUrl);
            }
        }

        /// <summary>
        /// Dispose tabs when destroying component
        /// </summary>
        public void Dispose()
        {
            nav.LocationChanged -= Nav_LocationChanged;
        }
    }
}
