using RoboSharp;
using ScreenShotLogic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Installer
{
    class Program
    {
        Install Install = new Install();

        Variables variables = new Variables();




        static void Main(string[] args)
        {
            Program program = new Program();
            foreach (var arg in args)
            {
                if (arg == "/?" || arg == "--help" || arg == "-h")
                {
                    Console.WriteLine("Установщик");
                    Console.WriteLine(@"Синтаксис: installer [параметр]");
                    Console.WriteLine("Параметры:");
                    Console.WriteLine("-c Скопировать файлы в исходный каталог");
                    Console.WriteLine("-r Удалить файлы из исходного каталога");
                    Console.WriteLine("-i Установить службу");
                    Console.WriteLine("-u Удалить службу");
                    Console.WriteLine("-start Запустить службу");
                    Console.WriteLine("-stop Остановить службу");
                    Console.WriteLine("-ct Создать задание");
                }

                if (arg == "-c")
                {

                    Install copy = new Install();
                    copy.CopyFiles();
                }

                if (arg == "-r")
                {
                    Install remove = new Install();
                    remove.Remove();
                }

                if (arg == "-i")
                {
                    Install installService = new Install();
                    installService.InstallService();

                }

                if (arg == "-u")
                {
                    Install uninstallService = new Install();
                    uninstallService.UninstallService();
                }

                if (arg=="-start")
                {
                    Install startService = new Install();
                    startService.StartService();
                }

                if (arg == "-stop")
                {
                    Install startService = new Install();
                    startService.StopService();
                }

                if (arg == "-ct")
                {
                    Install creatTask = new Install();
                    creatTask.CreateTask();
                }


            }

            Console.ReadKey();
        }

        
    }
}
