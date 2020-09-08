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
        public string State { get; set; }
        public Cell(int row, int col, string state)
        {
            Row = row;
            Col = col;
            State = state;
        }

    }
}
