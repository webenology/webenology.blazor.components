using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using webenology.blazor.components;

namespace TestApp.Pages
{
    public partial class Index
    {
        private Notification _notification;
        private Modal _modal;
        private Modal _modal2;
        private Confirm _confirm;
        private bool _insideClick;
        private DateTime _dt = DateTime.Now;
        private string _text;
        private decimal _num = 0;
        private int _count = 0;

        private ConfirmStyle confirmStyle()
        {
            var c = ConfirmStyle.WebenologyStyle;
            c.BackdropCss = "none";
            return c;
        }

        private List<KeyValuePair<string, string>> items = new();

        private void AddNotification()
        {
            var success = new NotificationModel { Body = $"Count: {_count}", ShowTimeoutBar = true };

            _notification.AddNotification(success);

            _count++;

            var warning = new NotificationModel { Body = $"Count: {_count}", ShowTimeoutBar = true, Type = NotificationType.Warning };

            _notification.AddNotification(warning);

            _count++;

            var danger = new NotificationModel { Body = $"Count: {_count}", Header = "what what what", ShowTimeoutBar = true, Type = NotificationType.Danger, TimeoutInSeconds = 5 };

            _notification.AddNotification(danger);

            _count++;

        }

        protected override void OnInitialized()
        {
            for (var i = 0; i < 100; i++)
            {
                items.Add(new KeyValuePair<string, string>(i.ToString(), i.ToString()));
            }
            base.OnInitialized();
        }
    }
}
