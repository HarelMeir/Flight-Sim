using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Flight_Sim.User_Story_3
{
    public class JoystickControlViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private string imageName = "Joystick_images\\0_neutral_joystick.png";
        //private string imageName = "Joystick_images\\3_right_joystick.png";
        public string ImageName {
            get { return imageName; }
            set {
                imageName = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ImageName)));
            }
        }


        private int rudderSliderPos = 400;
        public int RudderSliderPos
        {
            get { return rudderSliderPos; }
            set
            {
                rudderSliderPos = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(RudderSliderPos)));
            }
        }
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
                throttleSliderPos = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ThrottleSliderPos)));
            }
        }
        public Thickness ThrottleMargin
        {
            get { return new Thickness(244, ThrottleSliderPos, 0, 0); }
        }



        private double lastAileron = 0;
        private double lastElevator = 0;
        private int vertical = 0;
        private int horizontal = 0;

        public void UpdateJoystickPosition()
        {
            
            //if (lastElevator == currentElevator && lastAileron == currentAileron)
            //      ImageName = "Joystick_images\\0_neutral_joystick.png";

            //if (lastElevator < currentElevator && lastAileron == currentAileron)
            //      ImageName = "Joystick_images\\1_up_joystick.png";

            //if (lastElevator < currentElevator && lastAileron < currentAileron)
            //      ImageName = "Joystick_images\\2_up-right_joystick.png";

            //if (lastElevator == currentElevator && lastAileron < currentAileron)
            //      ImageName = "Joystick_images\\3_right_joystick.png";

            //if (lastElevator > currentElevator && lastAileron < currentAileron)
            //      ImageName = "Joystick_images\\4_down-right_joystick.png";

            //if (lastElevator > currentElevator && lastAileron == currentAileron)
            //      ImageName = "Joystick_images\\5_down_joystick.png";

            //if (lastElevator > currentElevator && lastAileron > currentAileron)
            //      ImageName = "Joystick_images\\6_down-left_joystick.png";

            //if (lastElevator > currentElevator && lastAileron == currentAileron)
            //      ImageName = "Joystick_images\\7_left_joystick.png";

            //if (lastElevator > currentElevator && lastAileron < currentAileron)
            //      ImageName = "Joystick_images\\8_up-left_joystick.png";

        }

    }
}
