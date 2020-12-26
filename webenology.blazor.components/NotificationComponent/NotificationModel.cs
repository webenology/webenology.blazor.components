namespace webenology.blazor.components
{
    public class NotificationModel
    {
        public double TimeoutInSeconds { get; set; }
        public string Header { get; set; }
        public string Body { get; set; }
        public string Icon { get; set; }
        public NotificationType Type { get; set; }
        public bool ShowTimeoutBar { get; set; }
    }

    public enum NotificationType
    {
        Success,
        Warning,
        Danger
    }
}
