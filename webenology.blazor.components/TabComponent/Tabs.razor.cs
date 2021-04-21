﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;

using Microsoft.AspNetCore.Components;

using webenology.blazor.components.TabComponent;

namespace webenology.blazor.components
{
    public partial class Tabs : IDisposable
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }
        [Parameter]
        public TabStyle CssStyle { get; set; } = TabStyle.WebenologyStyle;
        [Inject]
        private NavigationManager nav { get; set; }

        public Tab ActivePage;
        public List<Tab> TabPages = new();
        private string _oldUrl;
        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                nav.LocationChanged += Nav_LocationChanged;
            }

            base.OnAfterRender(firstRender);
        }

        private void Nav_LocationChanged(object sender, Microsoft.AspNetCore.Components.Routing.LocationChangedEventArgs e)
        {
            var uri = new Uri(e.Location);
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

        public void AddPage(Tab tab)
        {
            TabPages.Add(tab);
            StateHasChanged();
        }

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

        public void Dispose()
        {
            nav.LocationChanged -= Nav_LocationChanged;
        }
    }
}
