namespace webenology.blazor.components.NotificationComponent
{
    public class NotificationModel
    {
        public int TimeoutInSeconds { get; set; }
        public string Header { get; set; }
        public string Body { get; set; }
        public string Icon { get; set; }
        public NotificationType Type { get;set; }
        
    }

    public enum NotificationType
    {
        Success,
        Warning,
        Danger
    }
}
