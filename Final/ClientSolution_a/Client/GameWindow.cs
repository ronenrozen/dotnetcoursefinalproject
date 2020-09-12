﻿using Client.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class GameWindow : Form
    {
        private HttpClient client;
        private static GamesDataContext db = new GamesDataContext();
        private static BindingSource TblStepsBindingSource = new BindingSource();
        public static BindingSource TblGamesBindingSource = new BindingSource();
        public static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Git\GitDotnet\Final\ClientSolution_a\Client\client_db.mdf;Integrated Security=True";

        //graphics details
        ////////////////////////////////////////
        //grid cells
        public static int cellSize;
        public static int cellRadius;
        //pieces
        public static int pieceSize;
        public static int pieceRadius;
        public static int pieceOffset;
        private Bitmap bitm;
        ////////////////////////////////////////

        //for dragg action
        private bool dragging = false;
        private Piece draggedPiece;
        public Cell draggSrcCell;
        public Point draggedPieceTopLeftLocation;

        private Game game;
        static string clientTurnStr = "Your Turn...";
        static string serverTurnStr = "Server Turn...";
        static string unvalidStepStr = "Unvalid Step! Try Again";
        static string drawStr = "Its a Draw, Game Ended";
        static string clientWinStr = "You Win!! Game Ended";
        static string clientLostStr = "You Lost ): Game Ended";
        static string emptyStr = "";
        static string topLabelStr = clientTurnStr;
        static string bottomLabelStr = "";
        bool gameEnded = false;
        string winner;

        public GameWindow(Game g)
        {
            game = new Game(g.TblGame);
            game.Turn = Game.Player.Client.ToString();
            InitializeComponent();
            topLabelStr = clientTurnStr;
            GameWindow_Load();
        }

        private void GameWindow_Load()
        {
            TblStepsBindingSource.DataSource = db.TblSteps;
            TblGamesBindingSource.DataSource = db.TblGames;

            client = Program.client;
            this.DoubleBuffered = true;
            bitm = new Bitmap(panel1.Width, panel1.Height);
            InitGraphicsDetails();
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            using (Graphics g = Graphics.FromImage(bitm))
            {
                DrawGrid(g);
                DrawPieces(g);
                label1.Text = topLabelStr;
                label2.Text = bottomLabelStr;
                e.Graphics.DrawImage(bitm, 0, 0);
            }
        }
        private void RefreshPanel()
        {
            using (Graphics g = Graphics.FromImage(bitm))
            {
                g.Clear(Color.White);
                panel1.Invalidate();
            }
        }
        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (dragging && !gameEnded)
            {
                //stop dragging and create step
                dragging = false;
                Point dropLocation = e.Location;
                Step clientStep = CreateValidMoveFromDragg(dropLocation);
                if (clientStep == null)
                {
                    bottomLabelStr = unvalidStepStr;
                    //update panel
                    RefreshPanel();
                    return;
                }

                // valid client step
                bottomLabelStr = emptyStr;
                //update panel
                RefreshPanel();
                //update game
                game.PerformStep(clientStep);
                //add step to db
                SaveStep(clientStep,Game.Player.Client.ToString());
                //update panel
                RefreshPanel();
                if (clientStep.EndGameResult != "")
                {
                    EndGame(clientStep.EndGameResult);
                }
               
                else
                {
                    // server turn
                    game.Turn = Game.Player.Server.ToString();
                    topLabelStr = serverTurnStr;
                    RefreshPanel();
                    //Thread.Sleep(2000); // to see server step
                    //post step to server and get server step
                    Step serverStep = getServerStep(clientStep);
                    //update game
                    game.PerformStep(serverStep);
                    //add step to db
                    SaveStep(serverStep,Game.Player.Server.ToString());
                    //update panel

                    RefreshPanel();
                    if (serverStep.EndGameResult != "")
                    {
                        EndGame(serverStep.EndGameResult);
                    }
                    topLabelStr = clientTurnStr;
                    RefreshPanel();
                    game.Turn = Game.Player.Client.ToString();
                }
            }
        }

        private Step getServerStep(Step step)
        {
            Step serverStep = PostStepToServerAsync(step);
            if (serverStep == null)
            {
                throw new Exception("Couldnt retreive step from server");
            }
            return serverStep;
        }
  
        private  Step PostStepToServerAsync(Step step)
        {

            string path = PreparePathForSTep(step);

            HttpResponseMessage response = client.GetAsync(path).Result;

            if (response.IsSuccessStatusCode)
            {
                Step serverStep = response.Content.ReadAsAsync<Step>().Result;
                return serverStep;
            }
            return null;
        }

        private string PreparePathForSTep(Step step)
        {
            return "api/TblGames/step/" + game.TblGame.Id + "/" + step.SrcCellCol + "/" + step.SrcCellRow + "/" + step.PieceToRemoveCol + "/" + step.PieceToRemoveRow + "/" + step.DstCellCol + "/" + step.DstCellRow;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && game.Turn == Game.Player.Client.ToString() && !gameEnded)
            {
                if (dragging) // middle of dragging action
                {
                    draggedPieceTopLeftLocation = new Point(e.Location.X - pieceRadius, e.Location.Y - pieceRadius);
                }
                else
                {
                    foreach (Piece piece in game.Pieces)
                    {
                        if (piece.Player == Game.Player.Client.ToString() && IsInPieceArea(piece,e.Location)) //start dragging
                        {
                            dragging = true;
                            draggedPiece = piece;
                            draggSrcCell = game.Cells[piece.Row, piece.Col];
                            draggedPieceTopLeftLocation = new Point(e.Location.X - pieceRadius, e.Location.Y - pieceRadius);
                            break;
                        }
                    }
                }
                RefreshPanel();
            }
        }
        private void InitGraphicsDetails()
        {
            cellSize = (panel1.Width) / Game.gridCols;
            cellRadius = cellSize / 2;
            pieceSize = (int)(cellSize * 0.75);
            pieceRadius = (int)(pieceSize / 2);
            pieceOffset = (cellSize - pieceSize) / 2;
        }
        private void DrawGrid(Graphics g)
        {
            foreach (Cell cell in game.Cells)
            {
                Point topLeft = CellTopLeftPoint(cell);
                Size size = new Size(cellSize, cellSize);
                Rectangle rec = new Rectangle(topLeft, size);
                g.DrawRectangle(new Pen(Color.Black, 1), rec);
            }
        }
        private void DrawPieces(Graphics g)
        {
            foreach (Piece piece in game.Pieces)
            {
                Point pieceTopLeft;
                if (dragging && piece == draggedPiece)
                {
                    pieceTopLeft = draggedPieceTopLeftLocation;
                }
                else
                {
                    pieceTopLeft = PieceTopLeftPoint(piece);
 
                }
                Size size = new Size(pieceSize, pieceSize);
                Rectangle pieceRac = new Rectangle(pieceTopLeft, size);
                Color pieceColor;
                if (piece.Player == Game.Player.Client.ToString())
                {
                    pieceColor = Color.FromName(game.ClientColor);
                }
                else
                {
                    pieceColor = Color.FromName(game.ServerColor);
                }
                g.FillEllipse(new SolidBrush(pieceColor), pieceRac);
            }
        }
        private Point CellTopLeftPoint(Cell cell)
        {
            return new Point(cell.Col * cellSize, cell.Row * cellSize);
        }
        private Point CellCenterPoint(Cell cell)
        {
            return new Point(cell.Col * cellSize + cellRadius, cell.Row * cellSize + cellRadius);
        }
        private Point PieceTopLeftPoint(Piece piece)
        {
            return new Point(piece.Col * cellSize + pieceOffset, piece.Row * cellSize+ pieceOffset);
        }
        private Point PieceCenterPoint(Piece piece)
        {
            return new Point(piece.Col * cellSize + cellRadius, piece.Row * cellSize + cellRadius);
        }
        private bool IsInCellArea(Cell cell, Point location)
        {
            Point topLeft = CellTopLeftPoint(cell);
            return
                location.X < topLeft.X + cellSize
                && location.X > topLeft.X
                && location.Y < topLeft.Y + cellSize
                && location.Y > topLeft.Y;
        }
        private Cell GetCellFromLocation(Point location)
        {
            foreach (Cell cell in game.Cells)
            {
                if (IsInCellArea(cell,location))
                {
                    return cell;
                }
            }
            return null;
        }

        private Step CreateValidMoveFromDragg(Point dropLocation)
        {
            Cell targetCell = GetCellFromLocation(dropLocation);

            if (targetCell == null)
                return null;
            
            Piece piece = game.GetPieceByRowCol((draggSrcCell.Row, draggSrcCell.Col));
            List<Step> stepOptions = new List<Step>();

            game.StepOptions(piece, ref stepOptions);
            
            if(stepOptions.Count == 0)
            {
                return null;
            }

            foreach(Step step in stepOptions)
            {
                if(step.DstCellRow == targetCell.Row && step.DstCellCol == targetCell.Col)
                {
                    return step;
                }
            }
            
            return null;
        }
       
        private Step CreateStepFromDragg(Point dropLocation)
        {
            Cell targetCell = GetCellFromLocation(dropLocation);
            List<Step> steps = game.AllStepOptionsForPlayer(Game.Player.Client.ToString());
           
            foreach(Step step in steps)
            {
                if(step.DstCellRow == targetCell.Row && step.DstCellCol == targetCell.Col &&
                    step.SrcCellRow == draggSrcCell.Row && step.SrcCellCol == draggSrcCell.Col)
                {
                    return step;
                }
            }

            return null;
        }

        private bool IsInPieceArea(Piece piece, Point location)
        {
            Point pieceCenterPoint = PieceCenterPoint(piece);
            double dis = Math.Sqrt(Math.Pow(location.X - pieceCenterPoint.X, 2) + Math.Pow(location.Y - pieceCenterPoint.Y, 2));
            return dis < pieceRadius;
        }
        private void SaveStep(Step step,string player)
        {
            string query = "INSERT INTO dbo.TblSteps (SrcCellRow,SrcCellCol,DstCellRow,DstCellCol,PieceToRemoveRow,PieceToRemoveCol,EndGameResult,Player,GameId) VALUES (@SrcCellRow,@SrcCellCol,@DstCellRow,@DstCellCol,@PieceToRemoveRow,@PieceToRemoveCol,@EndGameResult,@Player,@GameId)";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(query, connection);
          
            command.Parameters.Add("@SrcCellRow", System.Data.SqlDbType.Int);
            command.Parameters.Add("@SrcCellCol", System.Data.SqlDbType.Int);
            command.Parameters.Add("@DstCellRow", System.Data.SqlDbType.Int);
            command.Parameters.Add("@DstCellCol", System.Data.SqlDbType.Int);
            command.Parameters.Add("@PieceToRemoveRow", System.Data.SqlDbType.Int);
            command.Parameters.Add("@PieceToRemoveCol", System.Data.SqlDbType.Int);
            command.Parameters.Add("@EndGameResult", System.Data.SqlDbType.NChar);
            command.Parameters.Add("@Player", System.Data.SqlDbType.NChar);
            command.Parameters.Add("@GameId", System.Data.SqlDbType.Int);

            command.Parameters["@SrcCellRow"].Value = step.SrcCellRow;
            command.Parameters["@SrcCellCol"].Value = step.SrcCellCol;
            command.Parameters["@DstCellRow"].Value = step.DstCellRow;
            command.Parameters["@DstCellCol"].Value = step.DstCellCol;
            command.Parameters["@PieceToRemoveRow"].Value = step.PieceToRemoveRow;
            command.Parameters["@PieceToRemoveCol"].Value = step.PieceToRemoveCol;
            if(step.EndGameResult == "")
            {
                command.Parameters["@EndGameResult"].Value = "none";
            }
            else
            {
                command.Parameters["@EndGameResult"].Value = step.EndGameResult;
            }
            command.Parameters["@Player"].Value = player;
            command.Parameters["@GameId"].Value = game.TblGame.Id;

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

        }
        private void EndGame(string endGameStr)
        {
            if (endGameStr == Game.EndGame.ClientWinner.ToString())
            {
                topLabelStr = clientWinStr;
                winner = "Client";
            }
            else if (endGameStr == Game.EndGame.ServerWinner.ToString())
            {
                topLabelStr = clientLostStr;
                winner = "Server";
            }
            else if (endGameStr == Game.EndGame.Draw.ToString())
            {
                topLabelStr = drawStr;
                winner = "Draw";
            }

            
            RefreshPanel();
            gameEnded = true;
          
            //save game
            SaveGame();
        }

        private void SaveGame()
        {

            string query = "INSERT INTO dbo.TblGames (GameId,Date,Winner,PlayerId) VALUES (@GameId,@Date,@Winner,@PlayerId)";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.Add("@GameId", System.Data.SqlDbType.Int);
            command.Parameters.Add("@Date", System.Data.SqlDbType.DateTime);
            command.Parameters.Add("@Winner", System.Data.SqlDbType.NChar);
            command.Parameters.Add("@PlayerId", System.Data.SqlDbType.Int);

            command.Parameters["@GameId"].Value = game.TblGame.Id;
            command.Parameters["@Date"].Value = game.TblGame.Date;
            command.Parameters["@Winner"].Value = winner;
            command.Parameters["@PlayerId"].Value = game.TblGame.Pid;

            connection.Open();
            command.ExecuteNonQuery();

            connection.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void GameWindow_Load(object sender, EventArgs e)
        {

        }
    }
}
