namespace Przypominajka.Services
{
    public interface INotificationManagerService
    {
        event EventHandler NotificationReceived;
        void SendNotification(string title, string message);
        void ReceiveNotification(string title, string message);
    }
}
