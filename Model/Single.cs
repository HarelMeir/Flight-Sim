using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flight_Sim.Model;

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
        public static FlightSimM SingleFlightSimM()
        {
            if (MFlag) {
                fsm = new FlightSimM(server, port);
                MFlag = false;
            }
            return fsm;
        }

        private static string server;
        private static int port;
        public static void SetServer(string s, int p) {
            server = s;
            port = p;
        }
    }
}
