using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Model
{
    public class TblGames
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        [DisplayName("Player ID")]
        public int Pid { get; set; }
    }
}
