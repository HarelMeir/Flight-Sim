using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Flight_Sim.User_Story_3
{
    class JoystickControlViewModel : INotifyPropertyChanged
    {
        FlightdataModel fdm;
        public JoystickControlViewModel(FlightdataModel fdm) {
            this.fdm = fdm;
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private string imageName = "Joystick_images\\0_neutral_joystick.png";

        public string ImageName {
            get { return imageName; }
            set {
                if (imageName != value)
                {
                    imageName = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(ImageName)));
                }
            }
        }

        //public const Thickness t 
        private int rudderSliderPos = 400;
        public int RudderSliderPos
        {
            get { return rudderSliderPos; }
            set
            {
                if (rudderSliderPos != value)
                {
                    rudderSliderPos = value;
                    //not sure how this works yet so specifically setting throttle and rudder diferently to see which one works.
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(RudderSliderPos)));
                }
            }
        }
        //private const int rudder0
        public Thickness RudderMargin
        {
            get { return new Thickness(RudderSliderPos, 318, 0, 0); }
        }


        private int throttleSliderPos = 168;
        public int ThrottleSliderPos
        {
            get { return throttleSliderPos; }
            set
            {
                if (throttleSliderPos != value)
                {
                    throttleSliderPos = value;
                    //PropertyChanged(this, new PropertyChangedEventArgs(nameof(ThrottleSliderPos)));
                }
            }
        }
        public Thickness ThrottleMargin
        {
            get {
                //not sure how this works yet so specifically setting throttle and rudder diferently to see which one works.
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ThrottleMargin)));
                return new Thickness(244, ThrottleSliderPos, 0, 0); }
        }

        public void UpdateRudderPosition()
        {
            //if RUD < -0.9 -> RudderSliderPos = leftMargin + 1/10 * totalMarginLength

            //
        }

        private double lastAileron = 0;
        private double lastElevator = 0;

        public void UpdateJoystickPosition()
        {
            if (lastElevator == this.fdm.Elevator && lastAileron == this.fdm.Aileron)
                  ImageName = "Joystick_images\\0_neutral_joystick.png";

            if (lastElevator < this.fdm.Elevator && lastAileron == this.fdm.Aileron)
                  ImageName = "Joystick_images\\1_up_joystick.png";

            if (lastElevator < this.fdm.Elevator && lastAileron < this.fdm.Aileron)
                  ImageName = "Joystick_images\\2_up-right_joystick.png";

            if (lastElevator == this.fdm.Elevator && lastAileron < this.fdm.Aileron)
                  ImageName = "Joystick_images\\3_right_joystick.png";

            if (lastElevator > this.fdm.Elevator && lastAileron < this.fdm.Aileron)
                  ImageName = "Joystick_images\\4_down-right_joystick.png";

            if (lastElevator > this.fdm.Elevator && lastAileron == this.fdm.Aileron)
                  ImageName = "Joystick_images\\5_down_joystick.png";

            if (lastElevator > this.fdm.Elevator && lastAileron > this.fdm.Aileron)
                  ImageName = "Joystick_images\\6_down-left_joystick.png";

            if (lastElevator > this.fdm.Elevator && lastAileron == this.fdm.Aileron)
                  ImageName = "Joystick_images\\7_left_joystick.png";

            if (lastElevator > this.fdm.Elevator && lastAileron < this.fdm.Aileron)
                  ImageName = "Joystick_images\\8_up-left_joystick.png";

            lastAileron = this.fdm.Aileron;
            lastElevator = this.fdm.Elevator;
        }

    }
}
