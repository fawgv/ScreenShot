using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyProcess
{
    class Program
    {
        static void Main(string[] args)
        {
            var runningProcs = from proc in Process.GetProcesses(".") orderby proc.Id select proc;
            foreach (var item in runningProcs)
            {
                Console.WriteLine(item.ToString());
            }
            if (runningProcs.Count(p => p.ProcessName.Contains("ScreenShotSaver")) > 0)
            {
                Console.WriteLine("Есть!");
            }
            else
            {
                Console.WriteLine("Нет!");
            }
            Console.ReadLine();
        }
    }
}
