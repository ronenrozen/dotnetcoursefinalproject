using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model
{
    class Step
    {
        public int SrcCellCol { get; set; }
        public int SrcCellRow { get; set; }
        public int DstCellCol { get; set; }
        public int DstCellRow { get; set; }
        public string Type { get; set; }

        public int PieceId { get; set; }
        public int PieceToRemoveId { get; set; }

        public Step(int pieceId, int srcCellRow, int srcCellCol, int dstCellRow, int dstCellCol, string type, int pieceToRemoveId = -1)
        {
            SrcCellCol = srcCellCol;
            SrcCellRow = srcCellRow;
            DstCellCol = dstCellCol;
            DstCellRow = dstCellRow;
            Type = type;
            PieceToRemoveId = pieceToRemoveId;
            PieceId = pieceId;
        }
    }
}
