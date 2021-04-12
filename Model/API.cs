using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Sim
{
    class API
    {
        [DllImport("Dll.dll")]
        public static extern int uploadCSV(string path);
        [DllImport("Dll.dll")]
        public static extern IntPtr createPoint(float x, float y);
        [DllImport("Dll.dll")]
        public static extern float getXPoint(IntPtr p);
        [DllImport("Dll.dll")]
        public static extern float getYPoint(IntPtr p);
        [DllImport("Dll.dll")]
        public static extern string mostCorr(string feature);
    }
}
