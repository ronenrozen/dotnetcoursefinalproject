using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FinalProjectDotNet.Data;
using FinalProjectDotNet.Model;

namespace FinalProjectDotNet.Pages.Players
{
    public class IndexModel : PageModel
    {
        private readonly FinalProjectDotNet.Data.PlayerDataContext _context;

        public IndexModel(FinalProjectDotNet.Data.PlayerDataContext context)
        {
            _context = context;
        }

        public IList<TblPlayers> TblPlayers { get;set; }

        public async Task OnGetAsync()
        {
            TblPlayers = await _context.TblPlayers.ToListAsync();
        }
    }
}
