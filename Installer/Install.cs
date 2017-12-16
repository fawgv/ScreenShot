using RoboSharp;
using ScreenShotLogic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Installer
{
    internal class Install
    {
        Variables variables;

        public Install()
        {
            variables = new Variables();
        }

        #region CopyFiles
        public void CopyFiles()
        {
            //RoboCommand copyFiles;
            //using (copyFiles = new RoboCommand())
            //{
            //    copyFiles.OnFileProcessed += backup_OnFileProcessed;
            //    copyFiles.OnCommandCompleted += backup_OnCommandCompleted;
            //    // copy options
            //    CopyOptions copyOptions = new CopyOptions();

            //    string pathSource = System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
            //    Console.WriteLine(pathSource);
            //    copyOptions.Source = pathSource;


            //    copyOptions.Destination = variables.pathProgramFolder;
            //    //C:\Users\user\Desktop\Samples\ScreenShot\Repos\Installer\bin\Debug
            //    copyOptions.CopySubdirectoriesIncludingEmpty = true;
            //    copyOptions.CopySubdirectories = true;
            //    copyFiles.CopyOptions.UseUnbufferedIo = true;
            //    copyFiles.CopyOptions = copyOptions;
            //    // select options
            //    //copyFiles.SelectionOptions.OnlyCopyArchiveFilesAndResetArchiveFlag = true;

            //    // retry options
            //    copyFiles.RetryOptions.RetryCount = 5;
            //    copyFiles.RetryOptions.RetryWaitTime = 2;
            //    copyFiles.Start();
            //}
            try
            {
                Process process;
                using (process = new Process())
                {
                    ProcessStartInfo psi = new ProcessStartInfo("cmd.exe");
                    psi.Arguments = $"/c robocopy {System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory)} {variables.pathProgramFolder} *.* /e";
                    //psi.WindowStyle = ProcessWindowStyle.Hidden;
                    process.StartInfo = psi;
                    process.Start();
                }
                Console.WriteLine("Выполнено копирование");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"При копировании возникла ошибка: {ex.Message}");
            }
            
        }

        void backup_OnFileProcessed(object sender, FileProcessedEventArgs e)
        {
            //Console.WriteLine(e.ProcessedFile.FileClass);
            Console.WriteLine(e.ProcessedFile.Name);
            //Console.WriteLine(e.ProcessedFile.Size.ToString());

        }

        void backup_OnCommandCompleted(object sender, RoboCommandCompletedEventArgs e)
        {
            Console.WriteLine("Копирование окончено");
        }
        #endregion

        #region Remove

        public void Remove()
        {
            try
            {
                if (System.IO.Directory.Exists(variables.pathProgramFolder))
                {
                    System.IO.Directory.Delete(variables.pathProgramFolder, true);
                    Console.WriteLine("Удаление выполнено");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"При удалении возникла ошибка: {ex.Message}");
            }
            
            
        }

        #endregion

        #region Install Service

        public void InstallService()
        {
            try
            {
                Process process;
                using (process = new Process())
                {
                    ProcessStartInfo psi = new ProcessStartInfo();
                    if (GetOSBit() == "x64")
                    {
                        string installutil = @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\installutil.exe";
                        psi.FileName = installutil;
                        psi.Arguments = $"{variables.pathToServiceFile}";
                    }
                    else if (GetOSBit()=="x32")
                    {
                        string installutil = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\installutil.exe";
                        psi.FileName = installutil;
                        psi.Arguments = $"{variables.pathToServiceFile}";
                    }
                    //psi.WindowStyle = ProcessWindowStyle.Hidden;
                    process.StartInfo = psi;
                    process.Start();
                    process.WaitForExit();
                }
                Console.WriteLine("Служба установлена");

                StartService();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"При копировании возникла ошибка: {ex.Message}");
            }
        }

        #endregion

        #region Create schtacks task

        //@"schtasks /create /sc onlogon /it /tn NTDisplay /tr C:\ProgramData\RealtekCTL\RealtekCTL.exe"
        public void CreateTask()
        {
            try
            {
                Process process;
                using (process = new Process())
                {
                    ProcessStartInfo psi = new ProcessStartInfo("cmd.exe");
                    psi.Arguments = $"/k schtasks /create /sc onlogon /it /tn NTDisplay /tr {variables.pathToProgramFile}";
                    //psi.WindowStyle = ProcessWindowStyle.Hidden;
                    process.StartInfo = psi;
                    process.Start();
                }
                Console.WriteLine("Выполнено создание задания");
                using (process = new Process())
                {
                    ProcessStartInfo psi = new ProcessStartInfo("cmd.exe");
                    psi.Arguments = @"/k %windir%\system32\taskschd.msc /s";
                    //psi.WindowStyle = ProcessWindowStyle.Hidden;
                    process.StartInfo = psi;
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"При создании задания возникла ошибка: {ex.Message}");
            }
        }

        #endregion

        #region Uninstall Service

        public void UninstallService()
        {
            try
            {
                StopService();
            
                Process process;
                using (process = new Process())
                {
                    ProcessStartInfo psi = new ProcessStartInfo();
                    if (GetOSBit() == "x64")
                    {
                        string installutil = @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\installutil.exe";
                        psi.FileName = installutil;
                        psi.Arguments = $"/u {variables.pathToServiceFile}";
                    }
                    else if (GetOSBit() == "x32")
                    {
                        string installutil = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\installutil.exe";
                        psi.FileName = installutil;
                        psi.Arguments = $"/u {variables.pathToServiceFile}";
                    }
                    //psi.WindowStyle = ProcessWindowStyle.Hidden;
                    process.StartInfo = psi;
                    process.Start();
                    process.WaitForExit();
                }
                Console.WriteLine("Служба удалена");
                

            }
            catch (Exception ex)
            {
                Console.WriteLine($"При копировании возникла ошибка: {ex.Message}");
            }
        }

        #endregion

        #region Start Service

        public void StartService()
        {
            try
            {
                Process process;
                using (process = new Process())
                {
                    ProcessStartInfo psi = new ProcessStartInfo("net.exe");
                    psi.Arguments = "start NetCTLService";
                    process.StartInfo = psi;
                    process.Start();
                    Console.WriteLine("Служба запущена");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"При запуске службы возникла ошибка: {ex.Message}");
            }
        }

        #endregion

        #region Stop Service

        public void StopService()
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

                Process process;
                using (process = new Process())
                {
                    ProcessStartInfo psi = new ProcessStartInfo("net.exe");
                    psi.Arguments = "stop NetCTLService";
                    process.StartInfo = psi;
                    process.Start();
                    Console.WriteLine("Служба остановлена");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"При запуске службы возникла ошибка: {ex.Message}");
            }
        }

        #endregion

        public static string GetOSBit()
        {
            bool is64bit = Is64Bit();
            if (is64bit)
                return "x64";
            else
                return "x32";
        }

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWow64Process([In] IntPtr hProcess, [Out] out bool lpSystemInfo);

        public static bool Is64Bit()
        {
            bool retVal;
            IsWow64Process(Process.GetCurrentProcess().Handle, out retVal);
            return retVal;
        }
    }
}
