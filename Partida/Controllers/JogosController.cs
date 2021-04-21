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
    public class JogosController : ControllerBase
    {
        private readonly dbContext _context;

        public JogosController(dbContext context)
        {
            _context = context;
        }

        // GET: api/Jogos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Jogo>>> GetJogos()
        {
            return await _context.Jogos.ToListAsync();
        }

        // GET: api/Jogos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Jogo>> GetJogo(int id)
        {
            var jogo = await _context.Jogos.FindAsync(id);

            if (jogo == null)
            {
                return NotFound();
            }

            return jogo;
        }

        // PUT: api/Jogos/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJogo(int id, Jogo jogo)
        {
            if (id != jogo.Id)
            {
                return BadRequest();
            }

            _context.Entry(jogo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JogoExists(id))
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

        // POST: api/Jogos
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Jogo>> PostJogo(Jogo jogo)
        {
            var times1 = _context.Times.ToList();
            var times2 = _context.Times.ToList();

            _context.Database.ExecuteSqlRaw("DELETE from jogos");
            foreach (var t1 in times1)
            {
                foreach (var t2 in times2)
                {
                    if (t1.Nome == t2.Nome) continue;
                    Jogo j = new Jogo()
                    {
                        Time1Id = t1.Id,
                        Time2Id = t2.Id
                    };
                    try
                    {
                        _context.Jogos.Add(j);
                //        _context.SaveChanges();
                    } catch
                    {
                    }
                }
            }
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetJogo", new { id = jogo.Id }, jogo);
        }

        // DELETE: api/Jogos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Jogo>> DeleteJogo(int id)
        {
            var jogo = await _context.Jogos.FindAsync(id);
            if (jogo == null)
            {
                return NotFound();
            }

            _context.Jogos.Remove(jogo);
            await _context.SaveChangesAsync();

            return jogo;
        }

        private bool JogoExists(int id)
        {
            return _context.Jogos.Any(e => e.Id == id);
        }
    }
}
