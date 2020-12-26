using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

using Microsoft.AspNetCore.Components;

using Timer = System.Timers.Timer;

namespace webenology.blazor.components
{
    public partial class NotificationItem
    {
        [CascadingParameter]
        private Notification _notification { get; set; }
        [Parameter]
        public NotificationModel Model { get; set; }

        private bool _isVisible = true;
        private bool _isHidden;
        private Timer _timer;
        private bool _isPaused;

        private string GetCss()
        {
            var css = new List<string> { _notification.CssStyle.NotificationItemCss };

            if (Model.Type == NotificationType.Success)
                css.Add(_notification.CssStyle.ItemSuccessCss);
            if (Model.Type == NotificationType.Warning)
                css.Add(_notification.CssStyle.ItemWarningCss);
            if (Model.Type == NotificationType.Danger)
                css.Add(_notification.CssStyle.ItemDangerCss);

            css.Add(_isVisible ? _notification.CssStyle.AnimateInCss : _notification.CssStyle.AnimateOutCss);

            return string.Join(" ", css);
        }

        private async Task Close()
        {
            _isVisible = false;
            await InvokeAsync(StateHasChanged);
            await Task.Run(() =>
             {
                 Thread.Sleep(300);
                 _isHidden = true;
                 InvokeAsync(StateHasChanged);
             });
        }

        protected override void OnInitialized()
        {
            this.IfNullThrow(_notification);
            this.IfNullThrow(Model);

            if (Model.TimeoutInSeconds <= 0)
                Model.TimeoutInSeconds = 3;
            
            _timer = new Timer { Interval = (double)Model.TimeoutInSeconds * 1000 };
            _timer.Elapsed += timer_Elapsed;
            _timer.Start();
            base.OnInitialized();
        }

        private async void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            await Close();
        }

        private void onPause()
        {
            _isPaused = true;
            _timer.Enabled = false;
            StateHasChanged();
        }

        private void onStart()
        {
            _isPaused = false;
            _timer.Enabled = true;
        }

        private string GetBarStyle()
        {
            if (_isPaused)
            {
                return "animation-name: none";
            }
            return $"animation-duration: {Model.TimeoutInSeconds}s";
        }
    }
}
