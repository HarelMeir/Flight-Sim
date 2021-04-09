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
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
                Update(propName);
            }
        }




        private double altitude = 0;
        //public string Altitude_Text { get { return "Aircraft altitude: " + VM_altitude_ft_p; }}
        public string Altitude_Text { get { return "Aircraft altitude: " + VM_altitude_ft_p; } }
        public double VM_altitude_ft_p
        {
            get { return model.altitude_ft_p; }
        }




        public string Airspeed_Text { get { return "Airspeed: " + VM_airspeed_kt_p; } }
        public double VM_airspeed_kt_p //////////might need to switch to indicated-speed-kt https://lemida.biu.ac.il/mod/forum/discuss.php?d=670687
        {
            get { return model.airspeed_kt_p; }
        }



        public string Heading_Text { get { return "Aircraft direction: " + VM_heading_deg_p; } }
        public double VM_heading_deg_p {  get { return model.heading_deg_p; } }




        public double VM_roll_deg_p { get { return model.roll_deg_p; } }

        public double VM_pitch_deg_p { get { return model.pitch_deg_p; } }

        public double VM_side_slip_deg_p { get { return model.side_slip_deg_p; } }

        public string RollPitchYaw_Text { get { return "Roll: " + VM_roll_deg_p + "\nPitch: " + VM_pitch_deg_p + "\nYaw: " + VM_side_slip_deg_p; } }



        private void Update(string propName)
        {
            if (propName == "VM_airspeed_kt_p")
                NotifyPropertyChanged("Airspeed_Text");
            if (propName == "VM_altitude_ft_p")
                NotifyPropertyChanged("Altitude_Text");
            if (propName == "VM_heading_deg_p")
                NotifyPropertyChanged("Heading_Text");
            if (propName == "VM_roll_deg_p" || propName == "VM_pitch_deg_p" || propName == "VM_side_slip_deg_p")
                NotifyPropertyChanged("RollPitchYaw_Text");
        }
    }
}
