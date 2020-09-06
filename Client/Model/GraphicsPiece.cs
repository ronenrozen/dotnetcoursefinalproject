using Client.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class GraphicsPiece : Piece
    {
        public GraphicsPiece(int id, string color, int row, int col, Point center = new Point()) : base(id,color,row,col)
        {
            
            Center = center;
        }

        public Point Center { get; set; }

    }
}
