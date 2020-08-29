using FinalProjectDotNet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Model
{
    public class Game
    {
        private Board board { get; set; }

        public Game ()
        {
            // Initate new game
            board = new Board();
        }

    }
}
