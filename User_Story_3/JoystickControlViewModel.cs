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
        public JoystickControlViewModel(FlightdataModel fdm) {
            this.model = fdm;
            rudderSliderPos = RudderMargin.Left;
            throttleSliderPos = ThrottleMargin.Top;
            sliderJumps = 2 / sliderAccuracy;
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) { NotifyPropertyChanged("VM_" + e.PropertyName); };
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName) {
            if (this.PropertyChanged != null) {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));

            }
        }


        /*
        //keep track of csv line number and update joystick when there's a change
        private int lineNumber = 0;
        public int VM_LineNumber { get { return lineNumber; } //////might not be called lineNumber when it's added.
            set { 
                if (lineNumber != value) {
                    lineNumber = value;
                    UpdateJoystickPosition();
                }
            }
        }*/
        //keep track of csv line number and update joystick when there's a change
        private int lineNumber = 0;
        public int VM_NumberOfLines
        {
            get { return lineNumber; } //////might not be called lineNumber when it's added.
            set
            {
                if (lineNumber != value)
                {
                    lineNumber = value;
                    UpdateJoystickPosition();
                }
            }
        }




        //Binding for Joystick image
        private string imageName = "Joystick_images\\0_neutral_joystick.png";
        public string ImageName {
            get { return imageName; }
            set {
                if (imageName != value) {
                    imageName = value;
                    NotifyPropertyChanged(nameof(ImageName));
                }
            }
        }



        //how many chunks to divide the slider into
        private int sliderAccuracy = 10;
        private int sliderJumps;

        //Rudder definitions and functions to move the slider
        private double rudderSliderPos;
        public double VM_rudder_p
        {
            get { return rudderSliderPos; }
            set {
                if (rudderSliderPos != value) {
                    rudderSliderPos = value;
                    UpdateRudderPosition();
                }
            }
        }

        public Thickness RudderMargin { get { return new Thickness(310, 321, 0, 0); } }
        public int RudderWidth { get{ return 210; } }
        public Thickness RudderSliderMargin { get { return new Thickness(VM_rudder_p, RudderMargin.Top - 4, RudderMargin.Right, RudderMargin.Bottom);}}
        public void UpdateRudderPosition() {
            for (int i = 0; i <= sliderAccuracy; i++) {
                if (model.rudder_p <= -1 + (sliderJumps * i) && model.rudder_p>= -1 + (sliderJumps * (i + 1)))
                    VM_rudder_p = RudderMargin.Left + i * (1 / sliderAccuracy) * RudderWidth;
            }
        }




        //Throttle definitions and functions to move the slider
        private double throttleSliderPos;
        public double VM_throttle1_p
        {
            get { return throttleSliderPos; }
            set {
                if (throttleSliderPos != value) {
                    throttleSliderPos = value;
                    UpdateThrottle1Position();
                }
            }
        }

        public Thickness ThrottleMargin { get { return new Thickness(248, 71, 0, 0); }}
        public Thickness ThrottleSliderMargin { get { return new Thickness(ThrottleMargin.Left - 4, VM_throttle1_p, ThrottleMargin.Right, ThrottleMargin.Bottom); }}
        public double ThrottleHeight { get { return 226; } }
        public void UpdateThrottle1Position() {
            for (int i = 0; i <= sliderAccuracy; i++) {
                if (model.throttle1_p <= -1 + (sliderJumps * i) && model.throttle1_p >= -1 + (sliderJumps * (i + 1)))
                    VM_throttle1_p = ThrottleMargin.Bottom - i * (1 / sliderAccuracy) * ThrottleHeight;
            }
        }




        //Joystick logic for changing positions
        private double lastAileron = 0;
        private double lastElevator = 0;
        public void UpdateJoystickPosition()
        {
            if (lastElevator == this.model.elevator_p && lastAileron == this.model.airleron_p)
                ImageName = "Joystick_images\\0_neutral_joystick.png";

            if (lastElevator < this.model.elevator_p && lastAileron == this.model.airleron_p)
                ImageName = "Joystick_images\\1_up_joystick.png";

            if (lastElevator < this.model.elevator_p && lastAileron < this.model.airleron_p)
                ImageName = "Joystick_images\\2_up-right_joystick.png";

            if (lastElevator == this.model.elevator_p && lastAileron < this.model.airleron_p)
                ImageName = "Joystick_images\\3_right_joystick.png";

            if (lastElevator > this.model.elevator_p && lastAileron < this.model.airleron_p)
                ImageName = "Joystick_images\\4_down-right_joystick.png";

            if (lastElevator > this.model.elevator_p && lastAileron == this.model.airleron_p)
                ImageName = "Joystick_images\\5_down_joystick.png";

            if (lastElevator > this.model.elevator_p && lastAileron > this.model.airleron_p)
                ImageName = "Joystick_images\\6_down-left_joystick.png";

            if (lastElevator > this.model.elevator_p && lastAileron == this.model.airleron_p)
                ImageName = "Joystick_images\\7_left_joystick.png";

            if (lastElevator > this.model.elevator_p && lastAileron < this.model.airleron_p)
                ImageName = "Joystick_images\\8_up-left_joystick.png";

            lastAileron = this.model.airleron_p;
            lastElevator = this.model.elevator_p;
        }

    }
}
