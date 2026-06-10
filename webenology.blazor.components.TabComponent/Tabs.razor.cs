using System.Net;
using System.Text;

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
            if (!string.IsNullOrEmpty(uri.Fragment) && uri.Fragment.StartsWith("#tab:"))
            {
                var slug = WebUtility.UrlDecode(uri.Fragment).Substring("#tab:".Length);
                // First slug match wins. Tabs sharing the same Title with no distinguishing Id are
                // ambiguous — give the conflicting tab an Id if that ever matters.
                var tab = TabPages.FirstOrDefault(t => Slugify(t.Key) == slug);
                if (tab != null && ActivePage != tab)
                {
                    ActivePage = tab;
                    StateHasChanged();
                }
            }
            else
            {
                if (TabPages.Any())
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
        /// Unregister a tab (called when a conditionally-rendered Tab disposes). If the active tab
        /// is the one leaving, activation falls back to the first remaining tab.
        /// </summary>
        /// <param name="tab">Tab object</param>
        public void RemovePage(Tab tab)
        {
            TabPages.Remove(tab);
            if (ActivePage == tab)
                ActivePage = TabPages.FirstOrDefault();
            try
            {
                StateHasChanged();
            }
            catch
            {
                // The whole Tabs component may be tearing down at the same time (its children
                // dispose with it) — rendering a disposed component throws; safe to ignore.
            }
        }

        /// <summary>
        /// Activate a tab
        /// </summary>
        /// <param name="tab">Tab object</param>
        public void ActivatePage(Tab tab)
        {
            SetNewUrl(tab);
            ActivePage = tab;
            StateHasChanged();
        }

        /// <summary>
        /// Activate a tab by its key — the Tab's Id when set, otherwise its Title. Matching is
        /// done on the slugified key so "Site Access", "site-access", and an Id of "site-access"
        /// all resolve to the same tab. No-op when no tab matches.
        /// </summary>
        /// <param name="key">Tab Id or Title</param>
        public void ActivatePage(string key)
        {
            var slug = Slugify(key);
            var tab = TabPages.FirstOrDefault(t => Slugify(t.Key) == slug);
            if (tab != null)
                ActivatePage(tab);
        }

        private void SetNewUrl(Tab tab)
        {
            var uri = new Uri(nav.Uri);
            var splitUri = uri.OriginalString;
            if (!string.IsNullOrEmpty(uri.Fragment))
            {
                splitUri = splitUri.Replace(uri.Fragment, "");
            }
            var newUrl = $"{splitUri}#tab:{Slugify(tab.Key)}";
            if (newUrl != _oldUrl)
            {
                _oldUrl = newUrl;
                nav.NavigateTo(newUrl);
            }
        }

        /// <summary>
        /// Lowercase, alphanumerics kept, everything else collapsed to single hyphens:
        /// "Site Access" → "site-access", "Refersion Affiliate" → "refersion-affiliate".
        /// Keeps the URL fragment readable and stable across cosmetic title tweaks like
        /// added spaces or punctuation.
        /// </summary>
        internal static string Slugify(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;

            var sb = new StringBuilder(text.Length);
            var lastWasHyphen = true; // true so leading separators never emit a hyphen
            foreach (var c in text)
            {
                if (char.IsLetterOrDigit(c))
                {
                    sb.Append(char.ToLowerInvariant(c));
                    lastWasHyphen = false;
                }
                else if (!lastWasHyphen)
                {
                    sb.Append('-');
                    lastWasHyphen = true;
                }
            }
            // Trim a trailing hyphen left by ending punctuation/space.
            if (sb.Length > 0 && sb[^1] == '-')
                sb.Length--;
            return sb.ToString();
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
