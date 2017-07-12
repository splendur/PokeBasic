using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Imaging;
using PokeBasic.Entities;
using WindowScrape.Types;
using PokeBasic.Model;

namespace PokeBasic
{
    public partial class Form1 : Form
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        public Form1()
        {
            AllocConsole();
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(1050, 100);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DBHandler.InitializeSQLiteDB();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            var game = new Game(pictureBox1);
            var decision = game.makeDecision();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
    }
}
