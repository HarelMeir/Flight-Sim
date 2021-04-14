using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
namespace Flight_Sim
{
    static class NativeMethods
    {
        //[DllImport("Kernel32.dll")]
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LoadLibrary(string dllToLoad);
        [DllImport("Kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);
        [DllImport("Kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hModule);
    }
}
