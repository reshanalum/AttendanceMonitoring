using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AttendanceMonitoring;
using AttendanceMonitoring.Models;

namespace AttendanceMonitoringApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly AttendanceMonitoringContext _context;

        public NotificationsController(AttendanceMonitoringContext context)
        {
            _context = context;
        }

        // POST: api/Notifications
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<Notification>> PostNotification([FromForm] string uid)
        {
            Notification notification = new();
            notification.Message = uid;
            _context.Notifications.Add(notification);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (NotificationExists(notification.NotificationId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            //return CreatedAtAction("GetNotification", new { id = notification.NotificationId }, notification);
            return Ok();
        }

        private bool NotificationExists(int id)
        {
            return _context.Notifications.Any(e => e.NotificationId == id);
        }
    }
}
