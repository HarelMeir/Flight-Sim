using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flight_Sim.Model;

using System.Windows;
using System.Runtime.CompilerServices;
namespace Flight_Sim.controllers
{
    public class mediaPlayer1VM : INotifyPropertyChanged
    {
        FlightSimM model;
        double _speedVal;
        int _sliderVal;

        public event PropertyChangedEventHandler PropertyChanged;

        public mediaPlayer1VM(FlightSimM model)
        {
            this.model = model;
            this._sliderVal = 0;
        }
        public double SpeedVal
        {
            get{ return _speedVal; }
            set
            {
                if(_speedVal != value)
                {
                    _speedVal = value;
                    this.model.changeRhythm(value);
                }
            }
        }
        /*public int SliderVal
        {
            get { return _sliderVal; }
            set
            {
                    value = this.model.sliderCurrent;
                    _sliderVal = this.model.sliderCurrent;        
            }
        }*/

        public void VM_Play()
        {
            this.model.Play();
        }
        public void VM_Pause()
        {
            this.model.Pause();
        }
        public void VM_Stop()
         {
             this.model.Stop();
         }

        public void VM_rightButton()
        {
            this.model.RightButton();
        }
        public void VM_leftButton()
        {
            this.model.LeftButton();
        }

         public void VM_rightStopButton()
         {
             this.model.RightStopButton();
         }

         public void VM_leftStopButton()
         {
             this.model.LeftStopButton();
         }
        public void VM_changeTimeBySlider(double val)
        {
            this.model.ChangeTimeBySlider(val);
        }
        public void VM_close()
        {
            this.model.Close();
        }
            

        /*public void VM_changeTimeSlider()
        {
            this.model.Play();
        }*/

    }
}
