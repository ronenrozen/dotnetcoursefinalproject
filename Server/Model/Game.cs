using FinalProjectDotNet.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Model
{
    public class Game
    {
        public List<Piece> Pieces { get; set; }
        public TblGames TblGame { get; set; }
        public Game(TblGames tblGame)
        {
            TblGame = tblGame;
            Pieces = CreatePieces();
        }


        private List<Piece> CreatePieces()
        {
            int blackPieceId = 1;
            int bluePieceId = 5;
            List<Piece> pieces = new List<Piece>();
            int row, col;
            List<(int, int)> blackPiecesPositions = new List<(int, int)> { (1, 0), (0, 1), (1, 2), (0, 3) };
            List<(int, int)> bluePiecesPositions = new List<(int, int)> { (7,0), (6, 1), (7, 2), (6, 3) };
            for (int i = 0; i < blackPiecesPositions.Count; i++)
            {
                //black pieces
                row = blackPiecesPositions[i].Item1;
                col = blackPiecesPositions[i].Item2;
                Piece blackPiece = new Piece(blackPieceId, Color.Black.Name, row, col);
                pieces.Add(blackPiece);

                //blue pieces
                row = bluePiecesPositions[i].Item1;
                col = bluePiecesPositions[i].Item2;
                Piece bluePiece = new Piece(bluePieceId, Color.Blue.Name, row, col);
                pieces.Add(bluePiece);

                blackPieceId++;
                bluePieceId++;
            }

            return pieces;
        }
    }
}
