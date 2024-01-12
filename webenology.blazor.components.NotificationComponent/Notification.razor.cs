using Microsoft.AspNetCore.Components;

namespace webenology.blazor.components.NotificationComponent
{
    public partial class Notification
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }
        [Parameter]
        public NotificationStyle CssStyle { get; set; } = NotificationStyle.WebenologyStyle;

        public List<NotificationModel> Items = new();

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

        public void RemoveNotification(NotificationModel model)
        {
            Items.Remove(model);
            InvokeAsync(StateHasChanged);
        }
    }
}
