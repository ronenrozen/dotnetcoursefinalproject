using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model
{
    public class Cell
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public Rectangle Rec { get; set; }
        public Point Center { get; set; }
        public string State { get; set; }
        public Cell(int row, int col)
        {
            Row = row;
            Col = col;
        }
    }
}
