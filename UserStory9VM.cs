using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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


        public int VM_CurrentLine { get {
                NotifyPropertyChanged("VM_Opacity");
                return model.CurrentLine; }}
        
        public int VM_Opacity { 
            get 
            {
                if (model.CurrentLine % 2 == 0)
                    return 1;
                return 0;
            }
        }




    }
}
