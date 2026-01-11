using AttendanceMonitoring;
using AttendanceMonitoring.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceMonitoringApi.Services
{
    public interface IAttendanceService
    {
        Task PostAttendance(string uid, CancellationToken cancellationToken);
    }
}
