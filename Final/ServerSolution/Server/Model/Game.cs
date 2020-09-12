﻿using FinalProjectDotNet.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Model
{
    public class Game
    {
        static Random rnd = new Random();
        public static int gridRows = 8;
        public static int gridCols = 4;
        public List<(int, int)> blackPiecesRowCol = new List<(int, int)> { (1, 0), (0, 1), (1, 2), (0, 3) };
        public List<(int, int)> bluePiecesRowCol = new List<(int, int)> { (7, 0), (6, 1), (7, 2), (6, 3) };

        public enum EndGame
        {
            Draw,
            ClientWinner,
            ServerWinner
        }

        public enum StepType
        {
            MoveLeft,
            MoveRight,
            EatRight,
            EatLeft,
            CantMove,
            EndGame

        }

        public enum Player
        {
            Client,
            Server
        }

        public enum CellState
        {
            Client,
            Server,
            Empty
        }

        public List<Piece> Pieces { get; set; }
        public TblGames TblGame { get; set; }
        public Cell[,] Cells { get; set; }
        public string ServerColor { get; set; }
        public string ClientColor { get; set; }
        public string Turn { get; set; }
        public Game(TblGames tblGame)
        {
            TblGame = tblGame;
            Pieces = CreatePieces();
            Cells = CreateCells();
            ServerColor = Color.Black.Name;
            ClientColor = Color.Blue.Name;
        }
        public List<Piece> CreatePieces()
        {
            List<Piece> pieces = new List<Piece>();
            int row;
            int col;
            for (int i = 0; i < gridCols; i++)
            {
                //black pieces
                row = blackPiecesRowCol[i].Item1;
                col = blackPiecesRowCol[i].Item2;
                Piece blackPiece = new Piece(row, col, Player.Server.ToString());
                pieces.Add(blackPiece);

                //blue pieces
                row = bluePiecesRowCol[i].Item1;
                col = bluePiecesRowCol[i].Item2;
                Piece bluePiece = new Piece(row, col, Player.Client.ToString());
                pieces.Add(bluePiece);
            }

            return pieces;
        }
        public Cell[,] CreateCells()
        {
            Cell[,] cellsList = new Cell[gridRows, gridCols];
            for (int row = 0; row < gridRows; row++)
            {
                for (int col = 0; col < gridCols; col++)
                {
                    cellsList[row, col] = new Cell(row, col, CellState.Empty.ToString());
                }
            }

            foreach (Piece piece in Pieces)
            {
                cellsList[piece.Row, piece.Col].State = piece.Player.ToString();
            }

            return cellsList;
        }
        public void StepOptions(Piece piece, ref List<Step> stepOptions)
        {
            int srcCellRow = piece.Row;
            int srcCellCol = piece.Col;

            if (piece.Player == Player.Client.ToString())
            {
                //client
                (int, int) topRight = (srcCellRow - 1, srcCellCol + 1);
                (int, int) topRight2 = (srcCellRow - 2, srcCellCol + 2);
                (int, int) topLeft = (srcCellRow - 1, srcCellCol - 1);
                (int, int) topLeft2 = (srcCellRow - 2, srcCellCol - 2);

                //MoveRight
                string endGame = "";
                if (EmptyCell(topRight))
                {
                    if (topRight.Item1 == 0)
                    {
                        endGame = Game.EndGame.ClientWinner.ToString();
                    }
                    stepOptions.Add(new Step(srcCellRow, srcCellCol, topRight.Item1, topRight.Item2, -1, -1, endGame));
                }
                //MoveLeft
                endGame = "";
                if (EmptyCell(topLeft))
                {
                    if (topLeft.Item1 == 0)
                    {
                        endGame = Game.EndGame.ClientWinner.ToString();
                    }
                    stepOptions.Add(new Step(srcCellRow, srcCellCol, topLeft.Item1, topLeft.Item2, -1, -1, endGame));
                }
                //EatRight
                endGame = "";
                if (EmptyCell(topRight2) && PlayerInCell(Player.Server.ToString(), topRight))
                {
                    if (topRight2.Item1 == 0)
                    {
                        endGame = Game.EndGame.ClientWinner.ToString();
                    }

                    else
                    {
                        List<Piece> serverPieces = Pieces.FindAll(p => p.Player == Player.Server.ToString());
                        if (serverPieces.Count == 1 && serverPieces[0].Row == topRight.Item1 && serverPieces[0].Col == topRight.Item2)
                        {
                            endGame = Game.EndGame.ClientWinner.ToString();
                        }
                    }
                    stepOptions.Add(new Step(srcCellRow, srcCellCol, topRight2.Item1, topRight2.Item2, topRight.Item1, topRight.Item2, endGame));
                }
                //EatLeft
                endGame = "";
                if (EmptyCell(topLeft2) && PlayerInCell(Player.Server.ToString(), topLeft))
                {
                    if (topLeft2.Item1 == 0)
                    {
                        endGame = Game.EndGame.ClientWinner.ToString();
                    }

                    else
                    {
                        List<Piece> serverPieces = Pieces.FindAll(p => p.Player == Player.Server.ToString());
                        if (serverPieces.Count == 1 && serverPieces[0].Row == topLeft.Item1 && serverPieces[0].Col == topLeft.Item2)
                        {
                            endGame = Game.EndGame.ClientWinner.ToString();
                        }
                    }
                    stepOptions.Add(new Step(srcCellRow, srcCellCol, topLeft2.Item1, topLeft2.Item2, topLeft.Item1, topLeft.Item2, endGame));
                }
            }
            else
            {
                //server
                (int, int) downRight = (srcCellRow + 1, srcCellCol - 1);
                (int, int) downRight2 = (srcCellRow + 2, srcCellCol - 2);
                (int, int) downLeft = (srcCellRow + 1, srcCellCol + 1);
                (int, int) downLeft2 = (srcCellRow + 2, srcCellCol + 2);

                //MoveRight
                string endGame = "";
                if (EmptyCell(downRight))
                {
                    if (downRight.Item1 == 7)
                    {
                        endGame = Game.EndGame.ServerWinner.ToString();
                    }
                    stepOptions.Add(new Step(srcCellRow, srcCellCol, downRight.Item1, downRight.Item2, -1, -1, endGame));
                }
                //MoveLeft
                endGame = "";
                if (EmptyCell(downLeft))
                {
                    if (downLeft.Item1 == 7)
                    {
                        endGame = Game.EndGame.ServerWinner.ToString();
                    }
                    stepOptions.Add(new Step(srcCellRow, srcCellCol, downLeft.Item1, downLeft.Item2, -1, -1, endGame));
                }
                //EatRight
                endGame = "";
                if (EmptyCell(downRight2) && PlayerInCell(Player.Client.ToString(), downRight))
                {
                    if (downRight2.Item1 == 7)
                    {
                        endGame = Game.EndGame.ServerWinner.ToString();
                    }

                    else
                    {
                        List<Piece> clientPieces = Pieces.FindAll(p => p.Player == Player.Client.ToString());
                        if (clientPieces.Count == 1 && clientPieces[0].Row == downRight.Item1 && clientPieces[0].Col == downRight.Item2)
                        {
                            endGame = Game.EndGame.ServerWinner.ToString();
                        }
                    }
                    stepOptions.Add(new Step(srcCellRow, srcCellCol, downRight2.Item1, downRight2.Item2, downRight.Item1, downRight.Item2, endGame));
                }
                //EatLeft
                endGame = "";
                if (EmptyCell(downLeft2) && PlayerInCell(Player.Client.ToString(), downLeft))
                {
                    if (downLeft2.Item1 == 7)
                    {
                        endGame = Game.EndGame.ServerWinner.ToString();
                    }
                    else
                    {
                        List<Piece> clientPieces = Pieces.FindAll(p => p.Player == Player.Client.ToString());
                        if (clientPieces.Count == 1 && clientPieces[0].Row == downLeft.Item1 && clientPieces[0].Col == downLeft.Item2)
                        {
                            endGame = Game.EndGame.ServerWinner.ToString();
                        }
                    }
                    stepOptions.Add(new Step(srcCellRow, srcCellCol, downLeft2.Item1, downLeft2.Item2, downLeft.Item1, downLeft.Item2, endGame));
                }
            }

        }

        public bool PlayerInCell(string player, (int, int) rowCol)
        {
            return RowColInBound(rowCol) && Cells[rowCol.Item1, rowCol.Item2].State == player;
        }
        public bool EmptyCell((int, int) rowCol)
        {
            if (RowColInBound(rowCol) && Cells[rowCol.Item1, rowCol.Item2].State == CellState.Empty.ToString())
            {
                return true;
            }
            return false;
        }
        public bool RowColInBound((int, int) rowCol)
        {
            return -1 < rowCol.Item1 && rowCol.Item1 < gridRows && -1 < rowCol.Item2 && rowCol.Item2 < gridCols;
        }
        public List<Step> AllStepOptionsForPlayer(string player)
        {
            List<Step> stepOptions = new List<Step>();
            foreach (Piece piece in Pieces)
            {
                if (piece.Player == player)
                {
                    StepOptions(piece, ref stepOptions);
                }
            }

            return stepOptions;
        }

        public Piece GetPieceByRowCol((int, int) rowCol)
        {
            foreach (Piece piece in Pieces)
            {
                if (piece.Row == rowCol.Item1 && piece.Col == rowCol.Item2)
                {
                    return piece;
                }
            }

            return null;
        }
        public void PerformStep(Step step)
        {
            //update moved piece
            Piece piece = GetPieceByRowCol((step.SrcCellRow, step.SrcCellCol));
            piece.Row = step.DstCellRow;
            piece.Col = step.DstCellCol;

            //update src and dst cell
            Cells[step.SrcCellRow, step.SrcCellCol].State = CellState.Empty.ToString();
            Cells[step.DstCellRow, step.DstCellCol].State = piece.Player.ToString();

            //remove eaten piece and update her cell
            if (step.PieceToRemoveCol != -1 && step.PieceToRemoveRow != -1)
            {
                Cells[step.PieceToRemoveRow, step.PieceToRemoveCol].State = Game.CellState.Empty.ToString();
                Pieces.RemoveAll(p => p.Row == step.PieceToRemoveRow && p.Col == step.PieceToRemoveCol);
            }
        }
        public Step GetRandomStep(string player)
        {
            List<Step> steps = AllStepOptionsForPlayer(player);
            int index = rnd.Next(steps.Count);
            return steps[index];
        }
    }
}
