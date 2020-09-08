using Client.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class GameWindow : Form
    {
        private HttpClient client;
        private GamesDataContext db = new GamesDataContext();
        private BindingSource TblStepsBindingSource = new BindingSource();

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
        public GameWindow(Game g)
        {
            game = new Game(g.TblGame);
            InitializeComponent();
            GameWindow_Load();
        }
        private void GameWindow_Load()
        {
            TblStepsBindingSource.DataSource = db.TblSteps;

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
            if (dragging)
            {
                dragging = false;
                Point dropLocation = e.Location;
                Step step = CreateStep(dropLocation);

                if (step == null)
                {
                    textBox1.Text = "Unvalid Move!!!";
                }
                else
                {
                    //update game
                    game.PerformStep(step);
                    //add step to db
                    SaveStep(step);
                    //update panel
                    RefreshPanel();
                    //check end game
                    string endGame = game.CheckEndGame();
                    if (endGame != "")
                    {
                        EndGame(endGame);
                    }
                    
                    game.Turn = Game.Player.Server.ToString();
                    textBox1.Text = "Server turn...";
                    //post step to server
                    Step serverStep = getServerStep(step);
                    //update game
                    game.PerformStep(serverStep);
                    //add step to db
                    SaveStep(serverStep);
                    //check end game
                    endGame = game.CheckEndGame();
                    if(endGame != "")
                    {
                        EndGame(endGame);
                    }
                    else
                    {
                        game.Turn = Game.Player.Client.ToString();
                        textBox1.Text = "Your turn...";
                    }
                    
                }
                //update panel
                RefreshPanel();

            }
        }
        private Step getServerStep(Step step)
        {
            Step serverStep = PostStepToServerAsync(step).Result;
            if (serverStep == null)
            {
                serverStep = PostStepToServerAsync(step).Result;
                if (serverStep == null)
                {
                    throw new Exception("Couldnt retreive step from server");
                }
            }
            return serverStep;
            
        }
        private async Task<Step> PostStepToServerAsync(Step step)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/TblGames/step/"+ game.TblGame.Id, step);
            if (response.IsSuccessStatusCode)
            {
                Step serverStep = await response.Content.ReadAsAsync<Step>();
                return serverStep;
            }
            return null;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && game.Turn == Game.Player.Client.ToString())
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
                && location.Y < topLeft.Y + cellRadius
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
        private Step CreateStep(Point dropLocation)
        {
            Cell targetCell = GetCellFromLocation(dropLocation);

            if (targetCell == null)
                return null;

            List<Step> steps = game.AllStepOptionsForPlayer(Game.Player.Client.ToString());
            foreach(Step step in steps)
            {
                if(step.DstCellRow == targetCell.Row && step.DstCellCol == targetCell.Col)
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
        private void SaveStep(Step step)
        {
            TblStepsBindingSource.Add(step);
        }
        private void EndGame(string endGameStr)
        {
            if (endGameStr == Game.EndGame.ClientWinner.ToString())
            {
                textBox1.Text = "You Win!!!";
            }
            else if (endGameStr == Game.EndGame.ServerWinner.ToString())
            {
                textBox1.Text = "You Lost :(";
            }
            else if (endGameStr == Game.EndGame.Draw.ToString())
            {
                textBox1.Text = "Its a Draw";
            }
        }
    }
}
