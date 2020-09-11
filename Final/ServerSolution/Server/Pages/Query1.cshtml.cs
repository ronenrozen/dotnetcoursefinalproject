using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProjectDotNet.Data;
using FinalProjectDotNet.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Model;

namespace Server.Pages
{
    public class Query1Model : PageModel
    {
        private readonly GameDataContext _context;
        private readonly PlayerDataContext _pcontext;

        public Query1Model(Server.Data.GameDataContext context, PlayerDataContext pcontext)
        {
            _context = context;
            _pcontext = pcontext;
        }

        public IList<TblGames> tblGames { get; set; }
        public IList<TblPlayers> TblPlayers { get; set; }
        public TblPlayers player { get; set; }
        public async Task OnGetAsync()
        {
            tblGames = await _context.TblGames.ToListAsync();
            tblGames = await _context.TblGames.ToListAsync();
        }

        public async Task OnPostAsync(String playerEmail)
        {
            if (playerEmail != null)
            {
                var p =
                    from tbPlayer in _pcontext.TblPlayers
                    where tbPlayer.Email == playerEmail
                    select tbPlayer;
                player = p.First();

                var x =
                    from g in _context.TblGames
                    where g.Pid == player.Id
                    orderby g.Id ascending
                    select g;
                tblGames = await x.ToListAsync();
            }
            else
            {
                tblGames = new List<TblGames>();
            }

        }
    }
}
