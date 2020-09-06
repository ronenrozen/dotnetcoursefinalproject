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

        //graphics details
        ////////////////////////////////////////
        //grid cells
        public static int gridRows = 8;
        public static int gridCols = 4;
        public static int cellSize;
        public static int cellRadius;
        //pieces
        public static int pieceSize;
        public static int pieceRadius;
        public static int pieceOffset;
        private Bitmap bitm;
        ////////////////////////////////////////
        
        Cell[,] cells = new Cell[gridRows, gridCols];
        List<GraphicsPiece> gPieces = new List<GraphicsPiece>();

        enum StepType
        {
            MoveLeft,
            MoveRight,
            EatRight,
            EatLeft
        }

        //for dragg action
        private bool dragging = false;
        private GraphicsPiece draggedPiece;
        public Cell draggSrcCell;
        private (int, int) mouseOffsetFromPieceCenter;
        
        private bool clientTurn = false;
        private Game game;
        public GameWindow(Game g)
        {
            game = g;
            List<Piece> pieces = g.Pieces;
            foreach (Piece piece in pieces)
            {
                gPieces.Add(new GraphicsPiece(piece.Id,piece.Color,piece.Row,piece.Col));
            }
            InitializeComponent();
            GameWindow_Load();
        }
        private void GameWindow_Load()
        {
            client = Program.client;
            this.DoubleBuffered = true;
            bitm = new Bitmap(panel1.Width, panel1.Height);
            InitGraphicsDetails();
            CreateCells();
            //send new game request to server
            //get pieces and gameId
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
                Point dropLocation = draggedPiece.Center;
                Step step = CreateStep(dropLocation);

                if (step == null)
                {
                    //dropLocation out of bound or unvalid
                    draggedPiece.Center = draggSrcCell.Center;
                    //update panel
                    RefreshPanel();
                }
                else
                {
                    //update piece
                    UpdatePiecesByStep(step);
                    //add step to db
                    clientTurn = false;
                    //update panel
                    RefreshPanel();
                    //post step to server
                    Step serverStep = getServerStep(step);
                    //update piece
                    UpdatePiecesByStep(serverStep);
                    //add step to db
                    clientTurn = true;
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
            if (e.Button == MouseButtons.Left && clientTurn == true)
            {
                if (dragging) // middle of dragging action
                {
                    draggedPiece.Center = new Point(e.Location.X + mouseOffsetFromPieceCenter.Item1, e.Location.Y + mouseOffsetFromPieceCenter.Item2);
                    RefreshPanel();
                }
                else
                {
                    foreach (GraphicsPiece piece in gPieces)
                    {
                        if (piece.Color == Color.Blue.Name && IsInPieceArea(piece,e.Location)) //start dragging
                        {
                            dragging = true;
                            draggedPiece = piece;
                            draggSrcCell = GetCellFromLocation(draggedPiece.Center);
                            mouseOffsetFromPieceCenter = (e.Location.X - draggedPiece.Center.X, e.Location.Y - draggedPiece.Center.Y);
                            break;
                        }
                    }

                }
            }
        }
        private void UpdatePiecesByStep(Step step)
        {
            GraphicsPiece piece = gPieces.Find(p => p.Id == step.PieceId);
            piece.Row = step.DstCellRow;
            piece.Col = step.DstCellCol;
            piece.Center = new Point((step.DstCellCol * cellSize) + cellRadius, (step.DstCellRow * cellSize) + cellRadius);
            if (step.Type == StepType.EatLeft.ToString() || step.Type == StepType.EatRight.ToString())
            {
                gPieces.Where(p => p.Id != step.PieceToRemoveId).ToList();
            }
        }
        private void CreateCells()
        {
            for (int row = 0; row < gridRows; row++)
            {
                int cellY = row * cellSize;
                for (int col = 0; col < gridCols; col++)
                {
                    int cellX = col * cellSize;
                    Point topLeft = new Point(cellX, cellY);
                    Size size = new Size(cellSize, cellSize);
                    Rectangle rec = new Rectangle(topLeft, size);
                    Point center = new Point(rec.Location.X + cellSize / 2, rec.Location.Y + cellSize / 2);
                    cells[row, col] = new Cell(row, col);
                    cells[row, col].Center = center;
                    cells[row, col].Rec = rec;
                }
            }
        }
        private void InitGraphicsDetails()
        {
            cellSize = (panel1.Width) / gridCols;
            cellRadius = cellSize / 2;
            pieceSize = (int)(cellSize * 0.75);
            pieceRadius = (int)(pieceSize / 2);
            pieceOffset = (cellSize - pieceSize) / 2;
        }
        private void DrawGrid(Graphics g)
        {
            foreach (Cell cell in cells)
            {
                g.DrawRectangle(new Pen(Color.Black, 1), cell.Rec);
            }
        }
        private void DrawOnePiece(GraphicsPiece piece, Graphics g)
        {
            Point pieceTopLeft = new Point(piece.Center.X - pieceRadius, piece.Center.Y - pieceRadius);
            Size size = new Size(pieceSize, pieceSize);
            Rectangle pieceRac = new Rectangle(pieceTopLeft, size);
            g.FillEllipse(new SolidBrush(Color.FromName(piece.Color)), pieceRac);
        }
        private void DrawPieces(Graphics g)
        {
            foreach (GraphicsPiece piece in gPieces)
            {
                DrawOnePiece(piece, g);
            }
        }
        private void CreatePiecesFromList(List<Piece> PiecesFromServer)
        {
            foreach (GraphicsPiece piece in PiecesFromServer)
            {
                GraphicsPiece newPiece = new GraphicsPiece(piece.Id, piece.Color, piece.Row, piece.Col);
                newPiece.Center = new Point((piece.Col * cellSize) + cellRadius, (piece.Row * cellSize) + cellRadius);
                gPieces.Add(new GraphicsPiece(piece.Id, piece.Color, piece.Row, piece.Col));
            }
        }
        private int PieceIdFromCell(Cell cell)
        {
            foreach (Piece piece in gPieces)
            {
                if (piece.Row == cell.Row && piece.Col == cell.Col)
                {
                    return piece.Id;
                }
            }
            return -1;
        }
        private string PieceColorFromCell(Cell cell)
        {
            int pieceId = PieceIdFromCell(cell);
            if (pieceId == -1)
            {
                return null;
            }
            else
            {
                return gPieces.Find(p => p.Id == pieceId).Color.ToString();
            }
        }
        private bool IsInCellArea(Cell cell, Point location)
        {
            return
                location.X < cell.Center.X + cellRadius
                && location.X > cell.Center.X - cellRadius
                && location.Y < cell.Center.Y + cellRadius
                && location.Y > cell.Center.Y - cellRadius;
        }
        private Cell GetCellFromLocation(Point location)
        {
            foreach (Cell cell in cells)
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

            int pieceInTargetCellId = PieceIdFromCell(targetCell);
            if (pieceInTargetCellId != -1)
                return null;

            //move forword right
            if (targetCell.Row == draggSrcCell.Row - 1 &&
                targetCell.Col == draggSrcCell.Col + 1)
            {
                return new Step(draggedPiece.Id, draggSrcCell.Row, draggSrcCell.Col, targetCell.Row, targetCell.Col, StepType.MoveRight.ToString());
            }

            //move forword left
            if (targetCell.Row == draggSrcCell.Row - 1 &&
               targetCell.Col == draggSrcCell.Col - 1)
            {
                return new Step(draggedPiece.Id, draggSrcCell.Row, draggSrcCell.Col, targetCell.Row, targetCell.Col, StepType.MoveLeft.ToString());
            }

            //eat right
            if (
                targetCell.Row == draggSrcCell.Row - 2 &&
                targetCell.Col == draggSrcCell.Col + 2 &&
                PieceColorFromCell(cells[draggSrcCell.Row - 1, draggSrcCell.Col + 1]) == Color.Black.ToString())
            {
                return new Step(draggedPiece.Id, draggSrcCell.Row, draggSrcCell.Col, targetCell.Row, targetCell.Col, StepType.EatRight.ToString(), PieceIdFromCell(cells[draggSrcCell.Row - 1, draggSrcCell.Col + 1]));
            }

            //eat left
            if (targetCell.Row == draggSrcCell.Row - 2 &&
                targetCell.Col == draggSrcCell.Col - 2 &&
                 PieceColorFromCell(cells[draggSrcCell.Row - 1, draggSrcCell.Col - 1]) == Color.Black.ToString())
            {
                return new Step(draggedPiece.Id, draggSrcCell.Row, draggSrcCell.Col, targetCell.Row, targetCell.Col, StepType.EatLeft.ToString(), PieceIdFromCell(cells[draggSrcCell.Row - 1, draggSrcCell.Col - 1]));
            }


            return null;
        }
        private bool IsInPieceArea(GraphicsPiece piece, Point location)
        {
            double dis = Math.Sqrt(Math.Pow(location.X - piece.Center.X, 2) + Math.Pow(location.Y - piece.Center.Y, 2));
            return dis < pieceRadius;
        }

        private void GameWindow_Load_1(object sender, EventArgs e)
        {

        }
    }
}
