namespace AttendanceMonitoringApi.Services
{
    public interface INotificationService
    {
        Task PostNotification(string uid, CancellationToken cancellationToken);
    }
}