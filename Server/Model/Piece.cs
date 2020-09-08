using System;
using System.Drawing;

namespace Server.Model
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