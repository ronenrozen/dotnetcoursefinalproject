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
        public string Color { get; set; }

        public int Row { get; set; }

        public int Col { get; set; }

        public int Id { get; set; }

        public Piece(int id, string color, int row, int col)
        {
            Id = id;
            Color = color;
            Row = row;
            Col = col;
        }
    }
}
