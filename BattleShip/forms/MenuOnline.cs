using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattleShip
{
    public partial class MenuOnline : Form
    {
        private Socket clientSocket;
        private Player player;
        public MenuOnline(Player player)
        {
            this.player = player;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ip = maskedTextBox1.Text;
            int port = Convert.ToInt32(maskedTextBox2.Text);

            try
            {
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                clientSocket.Connect(IPAddress.Parse(ip), port);
                PlaceForm placeForm = new PlaceForm(player,clientSocket);
                placeForm.Show();
                this.Hide();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
