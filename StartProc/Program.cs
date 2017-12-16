using ScreenShotLogic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StartProc
{
    class Program
    {
        internal System.Threading.Timer copyTimer;

        Variables variables = new Variables();
        //private System.Threading.Timer screenShotTimer = new System.Threading.Timer(new TimerCallback(ScreenShotTimer_Tick), null, 0, 10000);

        static void Main(string[] args)
        {
            Program progr = new Program();

            progr.copyTimer = new System.Threading.Timer(new TimerCallback(progr.RobocopyScreenShots_Tick), null, 0, 10000);

            //ProcessStartInfo startInfo = new ProcessStartInfo(Properties.Resources.pathPSExec);
            //ProcessStartInfo startInfo = new ProcessStartInfo("cmd.exe");
            ////startInfo.Arguments = $" {Properties.Resources.pathRealtekCTL} -i -d -s";
            //startInfo.Arguments = "/c schtasks /run /tn ntdisplay";
            ////startInfo.UseShellExecute = true;
            //Process.Start(startInfo);
            Console.ReadKey();
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

        //void ScreenShotTimer_Tick(object state)
        //{
        //    Process proc;
        //    using (proc = new Process())
        //    {
        //        string curData = DateTime.Now.ToShortDateString();
        //        ProcessStartInfo psi = new ProcessStartInfo("cmd.exe");
        //        psi.Arguments = $"/c robocopy C:\\nasdengisrazyru \\\\nas.dengisrazy.ru\\it\\itsсrееn\\krat\\{curData} /e /move";
        //        psi.WindowStyle = ProcessWindowStyle.Hidden;
        //        Process.Start(psi);
        //    }


        //}
    }
}
