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
            List<Piece> pieces = new List<Piece>();
            int row, col;
            List<(int, int)> blackPiecesPositions = new List<(int, int)> { (0, 1), (1, 0), (2, 1), (3, 0) };
            for (int i = 0; i < blackPiecesPositions.Count; i++)
            {
                //black pieces
                row = blackPiecesPositions[i].Item1;
                col = blackPiecesPositions[i].Item2;
                Piece blackPiece = new Piece(i + 1, Color.Black.Name, row, col);
                pieces.Add(blackPiece);
            }
            List<(int, int)> bluePiecesPositions = new List<(int, int)> { (0, 7), (1, 6), (2, 7), (3, 6) };
            for (int i = 4; i < bluePiecesPositions.Count; i++)
            {
                //blue pieces
                row = bluePiecesPositions[i].Item1;
                col = bluePiecesPositions[i].Item2;
                Piece bluePiece = new Piece(i + 1, Color.Blue.Name, row, col);
                pieces.Add(bluePiece);
            }
            return pieces;
        }
    }
}
