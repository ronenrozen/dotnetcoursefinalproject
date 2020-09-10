using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Model
{
    public class PlayerToGame
    {
        [DisplayName("Player ID")]
        public int Pid { get; set; }

        public string Email { get; set; }

        [DisplayName("Games Count")]
        public int GameCount { get; set; }

    
    }
}
