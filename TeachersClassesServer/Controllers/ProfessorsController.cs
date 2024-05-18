using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeachersClassesServer.DTO;
using TeachersClassesServer.Models;

namespace TeachersClassesServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessorsController : ControllerBase
    {
        private readonly ClassSourceContext _context;

        public ProfessorsController(ClassSourceContext context)
        {
            _context = context;
        }

        // GET: api/Professors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProfessorDto>>> GetProfessors()
        {
            //return await _context.Professors.ToListAsync();
            IQueryable<ProfessorDto> profqry = _context.Professors.Select(p => new ProfessorDto
            {
                ProfessorId = p.ProfessorId,
                ProfessorName = p.Professor1,
                CourseNum = p.CourseNum,
                Days = p.Days,
                Time = p.Time,
                Location = p.Location,
                Class = p.Class.Cnum

            }).Take(200); // limit to 200;
            return await profqry.ToListAsync();
        }

        // GET: api/Professors/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Professor>> GetProfessor(int id)
        {
            Professor? professor = await _context.Professors.FindAsync(id);

            if (professor == null)
            {
                return NotFound();
            }

            return professor;
        }

        // PUT: api/Professors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProfessor(int id, Professor professor)
        {
            if (id != professor.ProfessorId)
            {
                return BadRequest();
            }

            _context.Entry(professor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfessorExists(id))
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

        // POST: api/Professors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Professor>> PostProfessor(Professor professor)
        {
            _context.Professors.Add(professor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProfessor", new { id = professor.ProfessorId }, professor);
        }

        // DELETE: api/Professors/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfessor(int id)
        {
            var professor = await _context.Professors.FindAsync(id);
            if (professor == null)
            {
                return NotFound();
            }

            _context.Professors.Remove(professor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProfessorExists(int id)
        {
            return _context.Professors.Any(e => e.ProfessorId == id);
        }
    }
}
