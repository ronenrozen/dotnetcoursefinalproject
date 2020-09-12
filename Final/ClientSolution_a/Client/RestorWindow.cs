using Client.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class RestorWindow : Form
    {
        private static GamesDataContext db = new GamesDataContext();
        private static BindingSource TblStepsBindingSource = new BindingSource();
        public static BindingSource TblGamesBindingSource = new BindingSource();
        public static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Git\GitDotnet\Final\ClientSolution_a\Client\client_db.mdf;Integrated Security = True";

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
        public Cell draggSrcCell;
        public Point draggedPieceTopLeftLocation;

        private Game game;
        static string clientTurnStr = "Your Turn...";
        static string serverTurnStr = "Server Turn...";
        static string drawStr = "Its a Draw, Game Ended";
        static string clientWinStr = "You Win!! Game Ended";
        static string clientLostStr = "You Lost ): Game Ended";

        int delaySec = 1000;
        List<Step> steps;

        public RestorWindow(Game g)
        {
            TblStepsBindingSource.DataSource = db.TblSteps;
            TblGamesBindingSource.DataSource = db.TblGames;
            game = new Game(g.TblGame);
            game.Turn = Game.Player.Client.ToString();
            InitializeComponent();
            steps = new List<Step>();
            GetSteps();
            RestorWindow_Load();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            delaySec = 1000;
        }

        private void RestorWindow_Load()
        {
            this.DoubleBuffered = true;
            bitm = new Bitmap(panel1.Width, panel1.Height);
            InitGraphicsDetails();
            label1.Text = "";
        }

        private async void StartPerformSteps()
        {
            foreach(Step step in steps)
            {
                if(step.EndGameResult != "")
                {
                    //end game
                    if(step.EndGameResult == Game.EndGame.ClientWinner.ToString())
                    {
                        label1.Text = clientWinStr;
                    }
                    else if(step.EndGameResult == Game.EndGame.ServerWinner.ToString())
                    {
                        label1.Text = clientLostStr;
                    }
                    else
                    {
                        label1.Text = drawStr;
                    }
                }
                else
                {
                    if (game.GetPieceByRowCol((step.SrcCellRow, step.SrcCellCol)).Player == Game.Player.Client.ToString())
                    {
                        label1.Text = clientTurnStr;
                    }
                    else
                    {
                        label1.Text = serverTurnStr;
                    }

                }
                
                Invalidate();
                Update();
                game.PerformStep(step);
                RefreshPanel();
                await WaitingPeriod();
            }
        }

        private void RefreshPanel()
        {
            using (Graphics g = Graphics.FromImage(bitm))
            {
                g.Clear(Color.White);
                panel1.Invalidate();
                panel1.Update();
            }
        }

        private void GetSteps()
        {
            string query = $"SELECT * FROM dbo.TblSteps WHERE GameId = '{game.TblGame.Id}'";
            SqlConnection connection = new SqlConnection(GameWindow.connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);
            command.ExecuteNonQuery();
           
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int srcCellRow = (int)reader["SrcCellRow"];
                    int srcCellCol = (int)reader["SrcCellCol"];
                    int dstCellRow = (int)reader["DstCellRow"];
                    int dstCellCol = (int)reader["DstCellCol"];
                    int pieceToRemoveRow = (int)reader["PieceToRemoveRow"];
                    int pieceToRemoveCol = (int)reader["PieceToRemoveCol"];
                    string endGameResult = (string)reader["EndGameResult"];
                    if(endGameResult == "none")
                    {
                        endGameResult = "";
                    }
                    steps.Add(new Step(srcCellRow, srcCellCol, dstCellRow, dstCellCol, pieceToRemoveRow, pieceToRemoveCol, endGameResult));
                
                }
            }
            connection.Close();
        }

        private void InitGraphicsDetails()
        {
            cellSize = (panel1.Width) / Game.gridCols;
            cellRadius = cellSize / 2;
            pieceSize = (int)(cellSize * 0.75);
            pieceRadius = (int)(pieceSize / 2);
            pieceOffset = (cellSize - pieceSize) / 2;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            delaySec = 3000;
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
                pieceTopLeft = PieceTopLeftPoint(piece);
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

        private Point PieceTopLeftPoint(Piece piece)
        {
            return new Point(piece.Col * cellSize + pieceOffset, piece.Row * cellSize + pieceOffset);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            StartPerformSteps();
        }

        private void RestorWindow_Load(object sender, EventArgs e)
        {

        }
        private async Task<int> WaitingPeriod()
        {
            await Task.Delay(delaySec);
            return 1;
        }
    }
}
