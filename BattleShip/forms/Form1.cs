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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != null)
            {
                MenuForm menuForm = new MenuForm(textBox1.Text);
                menuForm.Show();
                this.Hide();

            }
            else
            {
                MessageBox.Show("Введите имя, чтобы продолжить играть");
            }
        }
    }
}
