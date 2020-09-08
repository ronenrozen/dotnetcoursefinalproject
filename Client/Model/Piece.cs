using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model
{
    public class Piece
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public string Player { get; set; }
        public Piece(int row, int col, string player)
        {
            Row = row;
            Col = col;
            Player = player;
        }
    }
}
