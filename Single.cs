using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Sim
{
    static class Single
    {
        private static bool flag = true;
        private static FlightdataModel fdm;
        public static FlightdataModel Singleton()
        {
            if (flag) {
                fdm = new FlightdataModel();
                flag = false;
            }
            return fdm;
        }
    }
}
