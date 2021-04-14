using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Sim
{
    static class API
    {
        [DllImport("Kernel32.dll")]
        public static extern int useDetector(string trainPath, string testPath);
    }
}
