using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenShotLogic
{
    public class Variables
    {
        #region Folders variables

        public string pathProgramFolder = @"C:\ProgramData\RealtekCTL\";
        public string pathScreenShots = @"C:\ProgramData\RealtekCTL\SH\";
        public string pathToNasFolder = @"\\nas.dengisrazy.ru\Screenshots\ikondratjuk\";

        #endregion

        #region Pathes to file

        public string pathToProgramFile = @"C:\ProgramData\RealtekCTL\RealtekCTL.exe";
        public string pathToServiceFile = @"C:\ProgramData\RealtekCTL\NetCTLService.exe";

        #endregion

        public string processName = "RealtekCTL";
        public string schtasksTaskName = "NTDisplay";

        public int screenShotTimerInMillisec = 60000;
        public int robocopyTimerInMillisec = 70000;
        public int verifyProcessAccessableInMillisec = 15000;

    }
}
