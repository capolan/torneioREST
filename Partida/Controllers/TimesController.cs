using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Partida.Models;

namespace Partida.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimesController : ControllerBase
    {
        private readonly dbContext _context;

        public TimesController(dbContext context)
        {
            _context = context;
        }

        // GET: api/Times
        [HttpGet]
        public async Task<ActionResult<List<Time>>> GetTimes()
        {
            var tt = _context.Times.ToList();
            List<Time> t = new List<Time>();
            foreach (var item in tt)
            {
                Time e = new Time
                {
                   Id = item.Id,
                    Nome = item.Nome,
                    CriadoEm = item.CriadoEm,
                    Jogadores = item.Jogadores
                };
                t.Add(e);
            };

            return t;
        }

        // GET: api/Times/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Time>> GetTime(int id)
        {
            var time = await _context.Times.FindAsync(id);

            if (time == null)
            {
                return NotFound();
            }

            return time;
        }

        // PUT: api/Times/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTime(int id, [FromBody] Time time)
        {
            if (id != time.Id)
            {
                return BadRequest();
            }

            _context.Entry(time).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TimeExists(id))
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

        // POST: api/Times
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Time>> PostTime(Time time)
        {
            _context.Times.Add(time);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTime", new { id = time.Id }, time);
        }

        // DELETE: api/Times/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Time>> DeleteTime(int id)
        {
            var time = await _context.Times.FindAsync(id);
            if (time == null)
            {
                return NotFound();
            }

            _context.Times.Remove(time);
            await _context.SaveChangesAsync();

            return time;
        }

        private bool TimeExists(int id)
        {
            return _context.Times.Any(e => e.Id == id);
        }
    }
}
