using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Flight_Sim.cppToCSharp;

namespace Flight_Sim
{
    class UserStory9VM : INotifyPropertyChanged
    {
        private FlightdataModel model;

        public UserStory9VM(FlightdataModel flightdataModel)
        {
            this.model = flightdataModel;
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) { NotifyPropertyChanged("VM_" + e.PropertyName); };
        }



        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }


        public int VM_CurrentLine { 
            get 
            {
                NotifyPropertyChanged("VM_Opacity");
                return model.CurrentLine; 
            }
        }
        
        public int VM_Opacity { 
            get 
            {
                if (checkLineAnomaly())
                    return 1;
                return 0;
            }
        }

        // checks if the current line has an anomaly
        private bool checkLineAnomaly()
        {
            List<AnomalyReport> anomalyReports = Single.SingleDataModel().AnomalyReports;
            int currLine = Single.SingleFlightSimM().getCurrentLine();
            foreach (AnomalyReport anom in anomalyReports)
            {
                if (currLine == anom.ts)
                {
                    return true;
                }
            }
            return false;
        }


    }
}
