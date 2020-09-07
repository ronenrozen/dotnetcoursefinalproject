﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Model;

namespace Server.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TblGamesController : ControllerBase
    {
        private readonly GameDataContext _context;
        private List<Game> games { get; set; }
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
        [HttpPost]
        public async Task<ActionResult<TblGames>> PostTblGames(TblGames tblGames)
        {
            _context.TblGames.Add(tblGames);
            await _context.SaveChangesAsync();
            Game g = new Game(tblGames);
            games.Add(g);
            return CreatedAtAction("GetTblGames", new { id = tblGames.Id }, g);
        }
        [Route("step/{gameId}")]
        [HttpPost]
        public ActionResult<TblGames> PostStep(int gameId, Step step)
        {
            Game cuurentGame = games.Find(g => g.TblGame.Id == gameId);
            updateCurrentGame(cuurentGame, step);
            Step serverStep = calculateServerStep(cuurentGame);
            return CreatedAtAction("serverStep", serverStep);
        }

        private Step calculateServerStep(Game cuurentGame)
        {
            throw new NotImplementedException();
        }

        private void updateCurrentGame(Game cuurentGame,Step step)
        {
            throw new NotImplementedException();
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
