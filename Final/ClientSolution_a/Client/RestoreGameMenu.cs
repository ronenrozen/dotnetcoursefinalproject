using Client.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class RestoreGameMenu : Form
    {
        private GamesDataContext db = new GamesDataContext();
        private BindingSource TblGamesBindingSource = new BindingSource();
        public static DataGridViewRow selectedRow = null;
        public static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\rrozen\source\repos\FinalProjectDotNet\dotnetcoursefinalproject\Final\ClientSolution_a\Client\client_db.mdf;Integrated Security=True";
        public int thePlayerId;

        public RestoreGameMenu(int playerId)
        {
            thePlayerId = playerId;
            InitializeComponent();
        }

        private void RestoreGameMenu_Load(object sender, EventArgs e)
        {
            TblGamesBindingSource.DataSource = db.TblGames;
            TblGamesDataGridView.DataSource = TblGamesBindingSource;
        }

        private void SelectAllGames()
        {

            string query = $"SELECT * FROM dbo.TblGames WHERE PlayerId = {thePlayerId}";
            SqlConnection connection = new SqlConnection(GameWindow.connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);
            command.ExecuteNonQuery();

            SqlDataReader reader = command.ExecuteReader();

            TblGamesDataGridView.DataSource = reader;
            connection.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // get selected game row
            selectedRow = TblGamesDataGridView.SelectedRows[0];
            int gameId = (int)selectedRow.Cells["GameId"].Value;
            DateTime date = (DateTime)selectedRow.Cells["Date"].Value;
            int playerId = Login.player.Id;
            Game g = new Game(new TblGames { Id = gameId, Date = date, Pid = playerId });
            RestorWindow restorWindow = new RestorWindow(g);
            restorWindow.ShowDialog();
        }
    }
}
