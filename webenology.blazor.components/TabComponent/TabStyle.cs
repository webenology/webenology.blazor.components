﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webenology.blazor.components.TabComponent
{
    public class TabStyle
    {
        public static TabStyle WebenologyStyle => new TabStyle
        {
            TabContainerCss = "",
            TabListCss = "nav nav-tabs",
            ActiveTabCss = "active",
            TabContentContainerCss = "tab-content",
            TabPaneCss = "tab-pane active",
            TabBodyCss = "panel-body",
            TabListItemCss = "nav-item",
            TabLinkCss = "nav-link"
        };

        public string TabContainerCss { get; set; }
        public string TabListCss { get; set; }
        public string ActiveTabCss { get; set; }
        public string TabContentContainerCss { get; set; }
        public string TabPaneCss { get; set; }
        public string TabBodyCss { get; set; }
        public string TabListItemCss { get; set; }
        public string TabLinkCss { get; set; }
    }
}
