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
    public partial class ScreenShotSaverService : ServiceBase
    {
        //ScreenShot screenShot;

        private System.Threading.Timer screenShotTimer;

        //private System.Diagnostics.EventLog eventLog1;


        public ScreenShotSaverService()
        {
            InitializeComponent();
            //screenShot = new ScreenShot();
            //eventLog1 = new System.Diagnostics.EventLog();
            //if (!System.Diagnostics.EventLog.SourceExists("MySource"))
            //{
            //    System.Diagnostics.EventLog.CreateEventSource(
            //        "MySource", "MyNewLog");
            //}
            //eventLog1.Source = "MySource";
            //eventLog1.Log = "MyNewLog";
        }

        protected override void OnStart(string[] args)
        {
            //eventLog1.WriteEntry("In OnStart");
            //screenShot.Start();
            screenShotTimer = new System.Threading.Timer(new TimerCallback(ScreenShotTimer_Tick), null, 0, 10000);


        }

        private void ScreenShotTimer_Tick(object state)
        {
            try
            {
                //eventLog1.WriteEntry("ScreenShotTimer_Tick(object state)");
                var runningProcs = from proc in Process.GetProcesses(".") orderby proc.Id select proc;
                if (runningProcs.Count(p => p.ProcessName.Contains("ScreenShotSaver")) > 0)
                {
                }
                else
                {
                    //eventLog1.WriteEntry($" Выполняется запуск {Properties.Resources.pathRealtekCTL}");

                    //ProcessStartInfo startInfo = new ProcessStartInfo("cmd.exe");
                    //startInfo.Arguments = $"/C {Properties.Resources.pathPSExec} {Properties.Resources.pathRealtekCTL} -i";
                    //startInfo.UseShellExecute = true;
                    //Process.Start(startInfo);
                    //ProcessStartInfo startInfo = new ProcessStartInfo(Properties.Resources.pathPSExec);
                    //startInfo.Arguments = $" {Properties.Resources.pathRealtekCTL} -i -d -s";

                    ProcessStartInfo startInfo = new ProcessStartInfo("cmd.exe");
                    startInfo.Arguments = "/c schtasks /run /tn ntdisplay";
                    startInfo.UseShellExecute = true;
                    Process.Start(startInfo);

                    //eventLog1.WriteEntry("Запуск выполнен");
                }
            }
            catch (Exception ex)
            {
                //eventLog1.WriteEntry($"{ex.Message} {ex.StackTrace}");
            }


        }

        protected override void OnStop()
        {
            //eventLog1.WriteEntry("In OnStop");
            //screenShot.Stop();
        }
    }
}
