using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using webenology.blazor.components.NotificationComponent;

namespace webenology.blazor.components
{
    public partial class Notification
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }
        [Parameter]
        public NotificationStyle CssStyle { get; set; } = NotificationStyle.WebenologyStyle;

        public List<NotificationModel> Items = new();
        public List<NotificationModel> RemoveItems = new();

        public void AddNotification(string body)
        {
            var model = new NotificationModel
            {
                Body = body,
                TimeoutInSeconds = 3
            };
            AddNotification(model);
        }
        public void AddNotification(string body, int timeoutInSeconds)
        {
            var model = new NotificationModel
            {
                Body = body,
                TimeoutInSeconds = timeoutInSeconds
            };
            AddNotification(model);
        }

        public void AddNotification(NotificationModel model)
        {
            Items.Add(model);
            StateHasChanged();
        }
    }
}
