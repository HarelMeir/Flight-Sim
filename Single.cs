using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Sim
{
    static class Single
    {
        private static bool dataFlag = true;
        private static FlightdataModel fdm;
        public static FlightdataModel SingleDataModel()
        {
            if (dataFlag) {
                fdm = new FlightdataModel();
                dataFlag = false;
            }
            return fdm;
        }


        private static bool MFlag = true;
        private static FlightSimM fsm;
        public static FlightSimM SingleFlightSimM(string server, int port)
        {
            if (MFlag) {
                fsm = new FlightSimM(server, port);
                MFlag = false;
            }
            return fsm;
        }
    }
}
