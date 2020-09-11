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
    public class GamesPerPlayerModel : PageModel
    {
        private readonly GameDataContext _context;
        private readonly PlayerDataContext _Pcontext;

        public GamesPerPlayerModel(GameDataContext context, PlayerDataContext Pcontext)
        {
            _context = context;
            _Pcontext = Pcontext;
        }

        public IList<TblGames> TblGames { get; set; }
        public IList<TblPlayers> TblPlayers { get; set; }

        public List<PlayerToGame> playerToGames { get; set; }
        public static Dictionary<TblPlayers, List<TblGames>> map { get; set; }
        public List<TblGames> currentPlayerGames { get; set; }
        public List<String> playersList { get; set; }
        public async Task OnGetAsync()
        {
            TblGames = await _context.TblGames.ToListAsync();
            TblPlayers = await _Pcontext.TblPlayers.ToListAsync();
            playerToGames = new List<PlayerToGame>();
            currentPlayerGames = new List<TblGames>();
            playersList = TblPlayers.Select(p => p.Email).ToList();
            map = new Dictionary<TblPlayers, List<TblGames>>();
            foreach (TblPlayers player in TblPlayers)
            {
                int playerId = player.Id;
                int gameCount = 0;

                foreach (TblGames game in TblGames)
                {
                    if (game.Pid.Equals(playerId))
                    {
                        gameCount++;
                        addToMap(player, game);
                    }
                }
                playerToGames.Add(new PlayerToGame { Pid = playerId, Email = player.Email, GameCount = gameCount });
            }
        }

        private void addToMap(TblPlayers player, TblGames game)
        {
            List<TblGames> listValue;
            if (map.TryGetValue(player, out listValue))
            {
                listValue.Add(game);
            }
            else
            {
                map.Add(player, new List<TblGames> { game });
            }

        }

        [BindProperty]
        public string playerEmail { get; set; }

        public async void OnPostAsync()
        {
            TblPlayers = await _Pcontext.TblPlayers.ToListAsync();
            TblPlayers cPlayer = null;

            foreach (TblPlayers player in TblPlayers)
            {
                if (player.Email.Equals(playerEmail))
                {
                    cPlayer = player;
                }
            }
            if (cPlayer != null)
            {
                List<TblGames> playerGames;
                map.TryGetValue(cPlayer, out playerGames);
                currentPlayerGames = playerGames;
            }

        }
    }
}
