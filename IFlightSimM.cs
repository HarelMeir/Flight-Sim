using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Flight_Sim
{
    interface IFlightSimM : INotifyPropertyChanged
    {
        //connection to FG properties.
        int PlaySpeed { get; set; }
        string FilePath { get; set; }
        Int32 Port { get; set; }
        string Server { get; set; }

        //Connecting method.
        void Connect();

    }
}

