using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjectDotNet.Model
{
    public enum CellState
    {
        Black, // black piece in cell
        White, // white piece in cell
        Empty  // no piece in cell
    }

    public class Cell
    {
     
        public int Row { get; set; } //count starts at top-left cell
        public int Col { get; set; } //count starts at top-left cell
        public int State { get; set; } // cell state (enum)

    }
}

