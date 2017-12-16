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
using System.Threading;
using System.Diagnostics;

namespace RealtekCTL
{
    public partial class MainForm : Form
    {
        ScreenShot screenShot;

        private System.Threading.Timer robocopyScreenShotsTimer;

        private Variables variables;

        public MainForm()
        {
            InitializeComponent();
            variables = new Variables();
            screenShot = new ScreenShot();
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                robocopyScreenShotsTimer = new System.Threading.Timer(new TimerCallback(RobocopyScreenShots_Tick), null, 0, variables.robocopyTimerInMillisec);

                screenShot.Start();
            }
            catch (Exception)
            {
            }
        }

        private void RobocopyScreenShots_Tick(object state)
        {
            Process killProc;
            using (killProc = new Process())
            {
                ProcessStartInfo psi = new ProcessStartInfo("cmd.exe");
                psi.Arguments = "/c taskkill /im robocopy* /f";
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                killProc.StartInfo = psi;
                killProc.Start();
                killProc.WaitForExit(10000);

            }
            Process process;

            using (process = new Process())
            {
                string curData = DateTime.Now.ToShortDateString();
                ProcessStartInfo psi = new ProcessStartInfo("cmd.exe");
                string fullPath = System.IO.Path.Combine(variables.pathToNasFolder, curData);
                psi.Arguments = $"/c robocopy {System.IO.Path.GetDirectoryName(variables.pathScreenShots)} {fullPath} /move";
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo = psi;
                process.Start();
            }
        }
    }
}
