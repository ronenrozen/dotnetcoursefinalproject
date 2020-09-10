using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Model;

namespace Server.Data
{
    public class GameDataContext : DbContext
    {
        public GameDataContext (DbContextOptions<GameDataContext> options)
            : base(options)
        {
        }

        public DbSet<Server.Model.TblGames> TblGames { get; set; }
    }
}
