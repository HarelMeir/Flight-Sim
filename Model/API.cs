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
        /*[DllImport("Kernel32.dll")]
        public static extern IntPtr createPoint(float x, float y);
        [DllImport("Kernel32.dll")]
        public static extern float getXPoint(IntPtr p);
        [DllImport("Kernel32.dll")]
        public static extern float getYPoint(IntPtr p);
        [DllImport("Kernel32.dll")]
        public static extern string mostCorr(string feature);*/
    }
}
