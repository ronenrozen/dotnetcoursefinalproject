using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Server.Data;
using Server.Model;

namespace Server.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TblGamesController : ControllerBase
    {
        private readonly GameDataContext _context;
        private static List<Game> games = new List<Game>();
        public TblGamesController(GameDataContext context)
        {
            _context = context;
        }

        // GET: api/TblGames
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TblGames>>> GetTblGames()
        {
            return await _context.TblGames.ToListAsync();
        }

        // GET: api/TblGames/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TblGames>> GetTblGames(int id)
        {
            var tblGames = await _context.TblGames.FindAsync(id);

            if (tblGames == null)
            {
                return NotFound();
            }

            return tblGames;
        }

        // PUT: api/TblGames/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblGames(int id, TblGames tblGames)
        {
            if (id != tblGames.Id)
            {
                return BadRequest();
            }

            _context.Entry(tblGames).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblGamesExists(id))
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

        // POST: api/TblGames
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost] //get new game request and create game
        public async Task<ActionResult<String>> PostTblGames(TblGames tblGames)
        {
            _context.TblGames.Add(tblGames);
            await _context.SaveChangesAsync();
            Game g = new Game(tblGames);
            games.Add(g);
            String gameStr = JsonConvert.SerializeObject(g, Formatting.Indented);
            return CreatedAtAction("GetTblGames", new { id = tblGames.Id }, gameStr);
        }

        [Route("step/{gameId}")]
        [HttpPost] //get step from client and return server step
        public ActionResult<String> PostStep(int gameId, [FromBody] string jsonStep)
        {
            Game currentGame = games.Find(g => g.TblGame.Id == gameId);
            Step step = JsonConvert.DeserializeObject<Step>(jsonStep);
            currentGame.PerformStep(step);
            Step serverStep = currentGame.GetRandomStep(Game.Player.Server.ToString());
            currentGame.PerformStep(serverStep);
            String responseStep = JsonConvert.SerializeObject(serverStep, Formatting.Indented);
            return CreatedAtAction("serverStep", serverStep);
            }
        // DELETE: api/TblGames/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TblGames>> DeleteTblGames(int id)
        {
            var tblGames = await _context.TblGames.FindAsync(id);
            if (tblGames == null)
            {
                return NotFound();
            }

            _context.TblGames.Remove(tblGames);
            await _context.SaveChangesAsync();

            return tblGames;
        }

        private bool TblGamesExists(int id)
        {
            return _context.TblGames.Any(e => e.Id == id);
        }
    }
}
