using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webenology.blazor.components.NotificationComponent
{
    public class NotificationStyle
    {
        public static NotificationStyle WebenologyStyle => new NotificationStyle
        {
            NotificationContainerCss = "notification-container",
            NotificationContainerFlipCss = "flip",
            NotificationItemCss = "notification-item",
            ItemSuccessCss = "success",
            ItemWarningCss = "warning",
            ItemDangerCss = "danger",
            ItemHeaderCss = "ni-header",
            ItemBodyCss = "ni-body",
            ItemBarCss = "ni-bar",
            ItemBarColorCss = "bar-color",
            AnimateInCss = "animate-in",
            AnimateOutCss = "animate-out"
        };

        public string NotificationContainerCss { get; set; }
        public string NotificationItemCss { get; set; }
        public string ItemSuccessCss { get; set; }
        public string ItemWarningCss { get; set; }
        public string ItemDangerCss { get; set; }
        public string ItemHeaderCss { get; set; }
        public string ItemBodyCss { get; set; }
        public string ItemBarCss { get; set; }
        public string ItemBarColorCss { get; set; }
        public string NotificationContainerFlipCss { get; set; }
        public string AnimateInCss { get; set; }
        public string AnimateOutCss { get; set; }
    }
}
