using AttendanceMonitoring;
using AttendanceMonitoring.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceMonitoringApi.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger<AttendanceService> _logger;
        private readonly AttendanceMonitoringContext _context;

        public NotificationService(ILogger<AttendanceService> logger, AttendanceMonitoringContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task PostNotification(string uid, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Posting notification...");

            Notification notification = new Notification();
            notification.Message = uid;

            _context.Notifications.Add(notification);
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Failed to save notification");
                throw;
            }

            _logger.LogInformation($"Notification posted successfully!");
        }
    }
}