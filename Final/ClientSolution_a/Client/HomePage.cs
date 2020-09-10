using Client.Model;
using Newtonsoft.Json;
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
    public partial class HomePage : Form
    {
        private HttpClient client;
        public HomePage()
        {
            InitializeComponent();
            client = Program.client;
            
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            
            TblGames tblGame = new TblGames { Date = DateTime.Now, Pid = Login.player.Id };
            HttpResponseMessage response = await client.PostAsJsonAsync("api/TblGames", tblGame);
            if (response.IsSuccessStatusCode)
            {
                string gameStr = await response.Content.ReadAsStringAsync();
                Game g = JsonConvert.DeserializeObject<Game>(gameStr);
                GameWindow gameWindow = new GameWindow(g);
                gameWindow.ShowDialog();
            }

            //TblGames tblGame = new TblGames { Date = DateTime.Now, Pid = 12 };
            //Game g = new Game(tblGame);
            //GameWindow gameWindow = new GameWindow(g);
            //gameWindow.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RestoreGameMenu restoreMenu = new RestoreGameMenu();
            restoreMenu.ShowDialog();
        }

        private void HomePage_Load(object sender, EventArgs e)
        {
            label3.Text = Login.player.Id.ToString();
            label4.Text = Login.player.Email;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
