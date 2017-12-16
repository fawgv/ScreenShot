using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScreenShotLogic;

namespace RealtekCTL
{
    public partial class MainForm : Form
    {
        ScreenShot screenShot;

        public MainForm()
        {
            InitializeComponent();
            screenShot = new ScreenShot();
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                screenShot.Start();
            }
            catch (Exception)
            {
            }
        }
    }
}
