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
    public class ParentsController : ControllerBase
    {
        private readonly AttendanceMonitoringContext _context;

        public ParentsController(AttendanceMonitoringContext context)
        {
            _context = context;
        }

        // GET: api/Parents/XX XX XX XX
        [HttpGet("{uid}")]
        public async Task<ActionResult<String>> GetParentContactNumberAndStudent(string uid)
        {
            var student = await _context.Students.FirstOrDefaultAsync(c => c.RFID == uid);

            if (student == null) return NotFound("No student found!");

            Relationship? relationship = await _context.Relationships
                .Include(c => c.StudentLink)
                .Include(c => c.ParentLink)
                .Where(c => c.StudentLink.RFID == uid)
                .FirstOrDefaultAsync();

            Attendance? attendance = await _context.Attendances
                .Include(c => c.StudentLink)
                .Where(c => c.StudentLink.RFID == uid)
                .OrderByDescending(c => c.AttendanceId)
                .FirstOrDefaultAsync();

            if (relationship == null)
            {
                return NotFound("No relationship was found!");
            }

            string status = "";
            if (attendance == null)
            {
                status = "Arrived";
            }
            else
            {
                if (attendance.Status == "Arrived")
                {
                    status = "Left";
                }
                else
                {
                    status = "Arrived";
                }
            }

            string studentName = relationship.StudentLink.FirstName + " " + relationship.StudentLink.LastName;
            string parentName = relationship.ParentLink.FirstName + " " + relationship.ParentLink.LastName;

            string phoneNumber = await _context.Contacts
                .Include(c => c.ParentLink)
                .Where(c => c.ParentLink.ParentId == relationship.ParentId)
                .Select(c => c.PhoneNumber)
                .FirstAsync();


            if (phoneNumber == null)
            {
                return NotFound();
            }

            return phoneNumber + "\n" + studentName + "\n" + parentName + "\n" + status + "\n" + DateTime.Now;
        }
    }
}
