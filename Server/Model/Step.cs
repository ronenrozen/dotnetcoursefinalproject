namespace Server.Model
{
    public class Step
    {
        public int SrcCellCol { get; set; }
        public int SrcCellRow { get; set; }
        public int DstCellCol { get; set; }
        public int DstCellRow { get; set; }
        public int PieceToRemoveRow { get; set; }
        public int PieceToRemoveCol { get; set; }

        public Step(int srcCellRow, int srcCellCol, int dstCellRow, int dstCellCol, int pieceToRemoveRow = -1, int pieceToRemoveCol = -1)
        {
            SrcCellCol = srcCellCol;
            SrcCellRow = srcCellRow;
            DstCellCol = dstCellCol;
            DstCellRow = dstCellRow;
            PieceToRemoveRow = pieceToRemoveRow;
            PieceToRemoveCol = pieceToRemoveCol;

        }
    }
}