using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        public RestoreGameMenu()
        {
            InitializeComponent();
        }

        private void RestoreGameMenu_Load(object sender, EventArgs e)
        {
            TblGamesBindingSource.DataSource = db.TblGames;
            TblGamesDataGridView.DataSource = TblGamesBindingSource;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
