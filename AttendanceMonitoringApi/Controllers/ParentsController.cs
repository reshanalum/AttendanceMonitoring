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

        // GET: api/Parents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Parent>>> GetParents()
        {
            return await _context.Parents.ToListAsync();
        }

        // GET: api/Parents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Parent>> GetParent(string id)
        {
            var parent = await _context.Parents.FindAsync(id);

            if (parent == null)
            {
                return NotFound();
            }

            return parent;
        }

        // GET: api/Parents/XX XX XX XX
        [HttpGet("/{uid}")]
        public async Task<ActionResult<Parent>> GetParentContactNumber(string uid)
        {
            Relationship relationship = await _context.Relationships
                .Include(c => c.StudentLink)
                .Include(c => c.ParentLink)
                .Where(c => c.StudentLink.UidRFID == uid)
                .FirstAsync();

            if (relationship == null)
            {
                return NotFound();
            }

            string phoneNumber = relationship.ParentLink.ContactList.First().PhoneNumber;

            if (phoneNumber == null)
            {
                return NotFound();
            }

            return Ok(phoneNumber);
        }

        // PUT: api/Parents/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParent(string id, Parent parent)
        {
            if (id != parent.ParentId)
            {
                return BadRequest();
            }

            _context.Entry(parent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Parents
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Parent>> PostParent(Parent parent)
        {
            _context.Parents.Add(parent);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ParentExists(parent.ParentId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetParent", new { id = parent.ParentId }, parent);
        }

        // DELETE: api/Parents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParent(string id)
        {
            var parent = await _context.Parents.FindAsync(id);
            if (parent == null)
            {
                return NotFound();
            }

            _context.Parents.Remove(parent);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ParentExists(string id)
        {
            return _context.Parents.Any(e => e.ParentId == id);
        }
    }
}
