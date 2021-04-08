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
        public string Altitude_Text { get { return "Aircraft altitude: " + VM_altitude_ft_p; }}
        public double VM_altitude_ft_p
        {
            get { return model.altitude_ft_p; } ////test to see which way to recieve the new value
            set { altitude = value; }
        }




        private double airspeed = 0; 
        public string Airspeed_Text { get { return "Airspeed: " + VM_airspeed_kt_p; } }
        public double VM_airspeed_kt_p //////////might need to switch to indicated-speed-kt https://lemida.biu.ac.il/mod/forum/discuss.php?d=670687
        {
            get { return airspeed; }
            set { airspeed = value; }
        }



        //aircraft direction
        private double heading = 0;
        public string Heading_Text { get { return "Aircraft direction: " + VM_heading_deg_p; } }
        public double VM_heading_deg_p
        {
            get { return heading; }
            set { heading = value; }
        }




        private double roll = 0;
        public double VM_roll_deg_p
        {
            get { return roll; }
            set { roll = value; }
        }

        private double pitch = 0;
        public double VM_pitch_deg_p
        {
            get { return pitch; }
            set { pitch = value; }
        }
        private double yaw = 0;
        public double VM_side_slip_deg_p
        {
            get { return yaw; }
            //set { yaw = value; }
            set { yaw = model.side_slip_deg_p; } ////test to see how Inotfiy really works
        }

        public string RollPitchYaw_Text { get { return "Roll: " + VM_roll_deg_p + "\nPitch: " + VM_pitch_deg_p + "\nYaw: " + VM_side_slip_deg_p; } }
    }
}
