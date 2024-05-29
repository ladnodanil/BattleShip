using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattleShip
{
    public partial class MenuForm : Form
    {
        private Player player;
        public MenuForm(string namePlaer)
        {
            InitializeComponent();
            label1.Text = "Игрок: " + namePlaer;
            player = new Player(namePlaer);
        }

        
        private void buttonPlayOnline_Click(object sender, EventArgs e)
        {
            MenuOnline menuOnline = new MenuOnline(player);
            menuOnline.Show();
            this.Hide();
        }

        private void buttonPlayWithPC_Click(object sender, EventArgs e)
        {
            PlaceForm placeForm = new PlaceForm(player);
            placeForm.Show();
            this.Hide();
        }
    }
}
