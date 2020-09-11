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
        private readonly PlayerDataContext _context;
        public string playerEmail { get; set; }

        public GamesPerPlayerModel( PlayerDataContext context)
        {
            _context = context;
        }

        public IList<TblPlayers> TblPlayers { get; set; }

        public async Task OnGetAsync()
        {
           
            TblPlayers = await _context.TblPlayers.ToListAsync();
        }


       
     
    }
}
