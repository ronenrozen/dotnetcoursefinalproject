using System.Collections.Generic;

namespace FinalProjectDotNet.Model
{
    class Board
    {
       
        private const int boardRowsLen = 8;
        private const int boardColsLen = 4;

        private Cell[,] boardCellsMat;

        public Board()
        {
            // initiate new board
            boardCellsMat = CreateCleanBoardCellsMat(boardRowsLen, boardColsLen);
            AddPieces(ref boardCellsMat);
        }

        // create boardCellsMat without pieces
        public Cell[,] CreateCleanBoardCellsMat(int boardRowsLen, int boardColsLen)
        {
            Cell[,] boardCellsMat = new Cell[boardRowsLen, boardColsLen];

            for (int r = 0; r < boardRowsLen; r++)
                for (int c = 0; c < boardColsLen; c++)
                    boardCellsMat[r, c] = new Cell { Row = r, Col = c, State = (int)CellState.Black};
            return boardCellsMat;
        }

        // add pieces to an empty board
        public void AddPieces(ref Cell[,] boardCellsMat)
        {
            List<(int, int)> blackPiecesLocations = new List<(int, int)> { (0, 1), (0, 3), (1, 0), (1, 2) };
            List<(int, int)> whitePiecesLocations = new List<(int, int)> { (6, 1), (6, 3), (7, 0), (7, 2) };
            foreach((int,int) location in blackPiecesLocations)
            {
                boardCellsMat[location.Item1, location.Item2].State = (int)CellState.Black;
            }
            foreach ((int, int) location in whitePiecesLocations)
            {
                boardCellsMat[location.Item1, location.Item2].State = (int)CellState.White;
            }

        }
    
       // move piece, if not valid return False
       public bool MovePiece(ref Cell[,] boardCellsMat,(int,int) srcCellPosition, (int, int) dstCellPosition)
       {
            if (IsValidPosition(srcCellPosition, dstCellPosition))
            {
                Cell srcCell = boardCellsMat[srcCellPosition.Item1, srcCellPosition.Item2];
                Cell dstCell = boardCellsMat[dstCellPosition.Item1, dstCellPosition.Item2];

                boardCellsMat[srcCellPosition.Item1, srcCellPosition.Item2].State = (int)CellState.Empty; //src cell is now empty
                boardCellsMat[dstCellPosition.Item1, dstCellPosition.Item2].State = srcCell.State; //dst cell is now with a piece

                return true;
            }
            return false;
       }

        // check cell in bound
        public bool InBound((int, int) cellPosition)
        {
            return ((-1 < cellPosition.Item1 && cellPosition.Item1 < boardRowsLen) && (-1 < cellPosition.Item2 && cellPosition.Item2 < boardRowsLen));
        }

        // check positions are valid
        public bool IsValidPosition((int, int) srcCellPosition, (int, int) dstCellPosition)
        {
            // src cell is not empty
            bool srcCellNotEmpty = boardCellsMat[srcCellPosition.Item1, srcCellPosition.Item2].State != (int)CellState.Empty;

            // dst cell is empty
            bool dstCellIsEmpty = boardCellsMat[dstCellPosition.Item1, dstCellPosition.Item2].State == (int)CellState.Empty;

            bool inBound = InBound(srcCellPosition) && InBound(dstCellPosition);

            return srcCellNotEmpty && dstCellIsEmpty && inBound;
        }
    }     
}
