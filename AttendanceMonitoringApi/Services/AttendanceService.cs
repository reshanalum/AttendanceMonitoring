using AttendanceMonitoring;
using AttendanceMonitoring.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceMonitoringApi.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly ILogger<AttendanceService> _logger;
        private readonly AttendanceMonitoringContext _context;

        public AttendanceService(ILogger<AttendanceService> logger, AttendanceMonitoringContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task PostAttendance(string uid, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Posting attendance for student with id: {uid}");

            var student = await _context.Students.FirstOrDefaultAsync(c => c.RFID == uid, cancellationToken);

            if (student == null)
            {
                _logger.LogWarning($"No student found associated with: {uid}");
                return;
            }

            String? lastStatus = await _context.Attendances
                .Where(c => c.StudentLink.RFID == uid)
                .OrderByDescending(c => c.AttendanceId)
                .Select(c => c.Status)
                .FirstOrDefaultAsync(cancellationToken);

            string newStatus = "";
            if (lastStatus == null || lastStatus == "Left")
            {
                newStatus = "Arrived";
            }
            else
            {
                newStatus = "Left";
            }

            Attendance newAttendance = new()
            {
                DateTime = DateTime.Now,
                StudentLink = student,
                Status = newStatus
            };

            _context.Attendances.Add(newAttendance);
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Failed to save attendance for student {StudentId}", student.StudentId);
                throw;
            }

            _logger.LogInformation($"Attendance posted successfully! Student has {newStatus}");
        }
    }
}
