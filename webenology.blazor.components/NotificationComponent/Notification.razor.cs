using System.Collections.Generic;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace webenology.blazor.components.NotificationComponent
{
    public partial class Notification
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        public List<NotificationModel> Items = new();
        [Inject]
        private IToastrJsHelper js { get; set; }

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
            var toastType = "success";
            if (model.Type == NotificationType.Success)
            {
                toastType = "success";
            }
            else if (model.Type == NotificationType.Danger)
            {
                toastType = "error";
            }
            else if (model.Type == NotificationType.Warning)
            {
                toastType = "warning";
            }

            var timeoutInMilliseconds = (model.TimeoutInSeconds > 0 ? model.TimeoutInSeconds : 3) * 1000;

            var options = new
            {
                timeOut = timeoutInMilliseconds
            };

            js.ShowToast(toastType, model.Body, model.Header, options);
        }
    }
}
