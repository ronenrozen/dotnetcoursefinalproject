using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FinalProjectDotNet.Model;

namespace FinalProjectDotNet.Data
{
    public class PlayerDataContext : DbContext
    {
        public PlayerDataContext (DbContextOptions<PlayerDataContext> options)
            : base(options)
        {
        }

        public DbSet<FinalProjectDotNet.Model.TblPlayers> TblPlayers { get; set; }
    }
}
