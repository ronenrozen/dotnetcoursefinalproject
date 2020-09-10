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
    public class DetailsModel : PageModel
    {
        private readonly FinalProjectDotNet.Data.PlayerDataContext _context;

        public DetailsModel(FinalProjectDotNet.Data.PlayerDataContext context)
        {
            _context = context;
        }

        public TblPlayers TblPlayers { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TblPlayers = await _context.TblPlayers.FirstOrDefaultAsync(m => m.Id == id);

            if (TblPlayers == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
