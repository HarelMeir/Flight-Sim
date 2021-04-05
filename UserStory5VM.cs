using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Flight_Sim
{
    class UserStory5VM : INotifyPropertyChanged
    {
        FlightdataModel model;
        public UserStory5VM(FlightdataModel fdm)
        {
            this.model = fdm;
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) { NotifyPropertyChanged("VM_" + e.PropertyName); };
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }




        private double altitude = 0;
        public string Altitude_Text { get { return "Aircraft altitude: " + VM_Altitude_ft; }}
        public double VM_Altitude_ft
        {
            get { return model.Altitude_ft; }
            set { altitude = value; }
        }




        private double airspeed = 0; 
        public string Airspeed_Text { get { return "Airspeed: " + VM_Airspeed; } }
        public double VM_Airspeed //////////might need to switch to indicated-speed-kt
        {
            get { return airspeed; }
            set { airspeed = value; }
        }



        //aircraft direction
        private double heading = 0;
        public string Heading_Text { get { return "Aircraft direction: " + VM_Heading_deg; } }
        public double VM_Heading_deg
        {
            get { return heading; }
            set { heading = value; }
        }




        private double roll = 0;
        public double VM_Roll_deg
        {
            get { return roll; }
            set { roll = value; }
        }

        private double pitch = 0;
        public double VM_Pitch_deg
        {
            get { return pitch; }
            set { pitch = value; }
        }
        private double yaw = 0;
        public double VM_Side_slip_deg
        {
            get { return yaw; }
            //set { yaw = value; }
            set { yaw = model.Side_slip_deg; } ////test to see how Inotfiy really works
        }

        public string RollPitchYaw_Text { get { return "Roll: " + VM_Roll_deg + "\nPitch: " + VM_Pitch_deg + "\nYaw: " + VM_Side_slip_deg; } }
    }
}
