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
    public partial class Login : Form
    {
        private HttpClient client;

        public static Player player;



        public Login()
        {
            InitializeComponent();
            client = Program.client;
        }

        private void Login_Load(object sender, EventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {
           
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        //LOGIN BUTTON
        private async void button1_Click(object sender, EventArgs e)
        {
            
            string email = textBox1.Text;
            string password = textBox2.Text;
            string path = "api/TblPlayers/login/" + email + "/" + password;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<Player> players = await response.Content.ReadAsAsync<IEnumerable<Player>>();
                if(players.Count() == 1)
                {
                    player = players.First();
                    HomePage manueForm = new HomePage();
                    manueForm.ShowDialog();
                }

            }
            
            //if login details ok by the server
            //HomePage manueForm = new HomePage();
            //manueForm.ShowDialog();
        }

        private void Login_Load_1(object sender, EventArgs e)
        {

        }
    }
}
