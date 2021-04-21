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
    public class jogadorModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int? TimesId { get; set; }
        public static implicit operator jogadorModel(Jogadore v)
        {
            jogadorModel emp = new jogadorModel()
            {
                Id = v.Id,
                Nome = v.Nome,
                TimesId = v.TimesId
            };
            return emp;
        }


    }
    [Route("api/[controller]")]
    [ApiController]
    public class JogadoresController : ControllerBase
    {
        private readonly dbContext _context;

        public JogadoresController(dbContext context)
        {
            _context = context;
        }

        // GET: api/Jogadores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Jogadore>>> GetJogadores()
        {
            return await _context.Jogadores.ToListAsync();
        }

        // GET: api/Jogadores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Jogadore>> GetJogadore(int id)
        {
            var jogadore = await _context.Jogadores.FindAsync(id);

            if (jogadore == null)
            {
                return NotFound();
            }

            return jogadore;
        }

        // PUT: api/Jogadores/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJogadore(int id, Jogadore jogadore)
        {
            if (id != jogadore.Id)
            {
                return BadRequest();
            }

            _context.Entry(jogadore).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JogadoreExists(id))
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

        // POST: api/Jogadores
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostJogadore(Jogadore jogadore)
        {
            _context.Jogadores.Add(jogadore);
            await _context.SaveChangesAsync();
            ajustaJogadore(jogadore.TimesId);

            jogadorModel jm = jogadore;
            return Ok(jm);

      //      return jogadore;
        }

        // DELETE: api/Jogadores/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJogadore(int id)
        {
            var jogadore = await _context.Jogadores.FindAsync(id);
            if (jogadore == null)
            {
                return NotFound();
            }
            int? timeId = jogadore.TimesId;
            _context.Jogadores.Remove(jogadore);
            await _context.SaveChangesAsync();

            ajustaJogadore(timeId);

            return NoContent();
        }

        private bool JogadoreExists(int id)
        {
            return _context.Jogadores.Any(e => e.Id == id);
        }

        private async void ajustaJogadore(int? timeId)
        {
            if (timeId == null) return;
            try
            {
                int tot = _context.Jogadores.Where(s => s.TimesId == timeId).Count();
                Time time = _context.Times.Find(timeId);
                time.Jogadores = tot;
                _context.SaveChanges();

                // ajusta jogos
                // remove partida
                if (tot<=4)
                {
                    _context.Database.ExecuteSqlRaw($"DELETE from jogos where time1_id={time.Id} or time2_id={time.Id};");
                }
                // add partida
                if (tot >= 5 )
                {
                    var times = _context.Jogadores.Where(s => s.Id != timeId).GroupBy(s => s.TimesId)
                        .Select(n => new
                        {
                            timeId = n.Key,
                            qtde = n.Count()
                        }
                    );

                    foreach (var t in times.Where(s=>s.qtde>=5).ToList())
                    {
                        if (t.timeId == timeId) continue;
                        Jogo existeJogo = _context.Jogos.FirstOrDefault(s => (s.Time1Id == timeId || s.Time2Id == timeId) &&
                        (s.Time1Id == t.timeId ||s.Time2Id == t.timeId));
                        if (existeJogo == null)
                        {
                            Jogo jogo = new Jogo()
                            {
                                Time1Id = timeId,
                                Time2Id = t.timeId,
                                CriadoEm = DateTime.Now
                            };
                            _context.Jogos.Add(jogo);
                        }
                    }
                    await _context.SaveChangesAsync();
                }
            }
            catch
            {

            }

        }
    }
}
