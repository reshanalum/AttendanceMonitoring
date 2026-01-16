using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AttendanceMonitoring;
using AttendanceMonitoring.Models;
using AttendanceMonitoringApi.Services;

namespace AttendanceMonitoringApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParentsController : ControllerBase
    {
        private readonly AttendanceMonitoringContext _context;
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly IServiceScopeFactory _scopeFactory;

        public ParentsController(AttendanceMonitoringContext context, IBackgroundTaskQueue taskQeue, IServiceScopeFactory scopeFactory)
        {
            _context = context;
            _taskQueue = taskQeue;
            _scopeFactory = scopeFactory;
        }

        // GET: api/Parents/XX XX XX XX
        [HttpGet("{uid}")]
        public async Task<ActionResult<String>> GetParentContactNumberAndStudent(string uid)
        {
            var data = await _context.Relationships
                .Where(r => r.StudentLink.RFID == uid)
                .Select(r => new 
                    {
                    StudentId = r.StudentLink.StudentId,
                    StudentName = r.StudentLink.FirstName + " " + r.StudentLink.LastName,
                    RFID = r.StudentLink.RFID,
                    ParentName = r.ParentLink.FirstName + " " + r.ParentLink.LastName,
                    PhoneNumber = r.ParentLink.ContactList.Where(c => c.PhoneNumber != null).Select(c => c.PhoneNumber).FirstOrDefault()
            })
            .FirstOrDefaultAsync();

            QueueNotificationTask(uid);

            if (data == null) return NotFound("No student or relationship found!");

            var lastStatus = await _context.Attendances
                .Where(a => a.StudentLink.StudentId == data.StudentId)
                .OrderByDescending(a => a.AttendanceId)
                .Select(a => a.Status)
                .FirstOrDefaultAsync();

            string status = "";
            if (lastStatus == null || lastStatus == "Left")
            {
                status = "Arrived";
            }
            else
            {
                status = "Left";
            }

            QueueAttendanceTask(uid);

            return data.PhoneNumber + "\n" + data.StudentName + "\n" + data.ParentName + "\n" + status + "\n" + DateTime.UtcNow;
        }

        private void QueueAttendanceTask(string rfid)
        {
            _taskQueue.QueueBackgroundWorkItem(async token =>
            {
                using var scope = _scopeFactory.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<IAttendanceService>();
                await service.PostAttendance(rfid, token);
            });
        }

        private void QueueNotificationTask(string uid)
        {
            _taskQueue.QueueBackgroundWorkItem(async token =>
            {
                using var scope = _scopeFactory.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<INotificationService>();
                await service.PostNotification(uid, token);
            });
        }
    }
}
