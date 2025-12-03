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
    public class AttendancesController : ControllerBase
    {
        private readonly AttendanceMonitoringContext _context;

        public AttendancesController(AttendanceMonitoringContext context)
        {
            _context = context;
        }

        // POST: api/Attendances
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<Attendance>> PostAttendanceByUID([FromForm] string uid)
        {
            var student = await _context.Students.FirstOrDefaultAsync(c => c.RFID == uid);

            if (student == null)
            {
                return NotFound("No student found!");
            }

            Attendance? oldAttendance = await _context.Attendances
                .Where(c => c.StudentLink.RFID == uid)
                .OrderByDescending(c => c.AttendanceId)
                .FirstOrDefaultAsync();

            string newStatus = "";
            if (oldAttendance == null || oldAttendance.Status == "Left")
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
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AttendanceExists(newAttendance.AttendanceId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            //return CreatedAtAction("GetAttendance", new { id = newAttendance.AttendanceId }, newAttendance);
            return Ok($"Attendance has been posted! Student has {newStatus}");
        }

        private bool AttendanceExists(int id)
        {
            return _context.Attendances.Any(e => e.AttendanceId == id);
        }
    }
}
