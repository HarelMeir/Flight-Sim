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

        FlightdataModel model;
        public JoystickControlViewModel(FlightdataModel fdm)
        {
            this.model = fdm;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private string imageName = "Joystick_images\\0_neutral_joystick.png";

        public string ImageName {
            get { return imageName; }
            set {
                if (imageName != value)
                {
                    imageName = value;
                    NotifyPropertyChanged(nameof(ImageName));
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
                    NotifyPropertyChanged(nameof(RudderSliderPos));
                }
            }
        }
        //private const int rudderStartPos
        private const int rudderLeft = 310;
        private const int rudderWidth = 210;
        private Thickness defaultThickness = new Thickness(400, 318, 0, 0);
       
        public Thickness RudderMargin
        {
            get { return new Thickness(RudderSliderPos, defaultThickness.Top, defaultThickness.Right, defaultThickness.Bottom);}
        }


        private const int throttleHeight = 226;
        private const int throttleTop = 71;
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
                //PropertyChanged(this, new PropertyChangedEventArgs(nameof(ThrottleMargin)));
                return new Thickness(244, ThrottleSliderPos, 0, 0); }
        }
        public void UpdateRudderPosition()
        {
            for (int i = 0; i <= 10; i++) {
                if (model.Rudder <= -1 + (0.2 * i) && model.Rudder >= -1 + (0.2 * (i + 1))) 
                    RudderSliderPos = rudderLeft + i * (1 / 10) * rudderWidth;
            }  
        }
        public void UpdateThrottlePosition()
        {
            for (int i = 0; i <= 10; i++)
            {
                if (model.Throttle1 <= -1 + (0.2 * i) && model.Throttle1 >= -1 + (0.2 * (i + 1)))
                    ThrottleSliderPos = throttleTop + i * (1 / 10) * throttleHeight;
            }
        }


        private double lastAileron = 0;
        private double lastElevator = 0;
        public void UpdateJoystickPosition()
        {
            if (lastElevator == this.model.Elevator && lastAileron == this.model.Aileron)
                ImageName = "Joystick_images\\0_neutral_joystick.png";

            if (lastElevator < this.model.Elevator && lastAileron == this.model.Aileron)
                ImageName = "Joystick_images\\1_up_joystick.png";

            if (lastElevator < this.model.Elevator && lastAileron < this.model.Aileron)
                ImageName = "Joystick_images\\2_up-right_joystick.png";

            if (lastElevator == this.model.Elevator && lastAileron < this.model.Aileron)
                ImageName = "Joystick_images\\3_right_joystick.png";

            if (lastElevator > this.model.Elevator && lastAileron < this.model.Aileron)
                ImageName = "Joystick_images\\4_down-right_joystick.png";

            if (lastElevator > this.model.Elevator && lastAileron == this.model.Aileron)
                ImageName = "Joystick_images\\5_down_joystick.png";

            if (lastElevator > this.model.Elevator && lastAileron > this.model.Aileron)
                ImageName = "Joystick_images\\6_down-left_joystick.png";

            if (lastElevator > this.model.Elevator && lastAileron == this.model.Aileron)
                ImageName = "Joystick_images\\7_left_joystick.png";

            if (lastElevator > this.model.Elevator && lastAileron < this.model.Aileron)
                ImageName = "Joystick_images\\8_up-left_joystick.png";

            lastAileron = this.model.Aileron;
            lastElevator = this.model.Elevator;
        }

    }
}
