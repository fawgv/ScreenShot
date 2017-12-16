using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenShotLogic
{
    public class ProviderScreenShot
    {
        public bool VerifyFolder(string path)
        {
            return Directory.Exists(path);
        }
    }
}
