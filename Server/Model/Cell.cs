using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjectDotNet.Model
{
    public class Cell
    {

        public int Row { get; set; }
        public int Col { get; set; }
        public string State { get; set; }
        public Cell(int row, int col,string state)
        {
            Row = row;
            Col = col;
            State = state;
        }

    }
}

