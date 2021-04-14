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
        double _speedVal = 1;

        private string time;
        private double minutes;
        private double seconds;
        private int currentLine;


        public event PropertyChangedEventHandler PropertyChanged;

        public mediaPlayer1VM(FlightSimM model)
        {
            this.model = model;
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) { NotifyPropertyChanged("VM_" + e.PropertyName); };
            model.Data.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) { NotifyPropertyChanged("VM_" + e.PropertyName); };
            this.time = "00:00";
            this.minutes = 0.0;
            this.seconds = 0.0;
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
         public int numberOfLines
        {
            get { return this.model.NumberOfLines; }
        }

        
        public int VM_sliderVal
        {
            get {
                return model.sliderVal;
            }
            set
            {
                   /* if (model.sliderVal != value)
                {
                    model.sliderVal = value;
                    
                    this.NotifyPropertyChanged("VM_sliderVal");
                }8*/
                   // value = this.model.sliderCurrent;
                   // _sliderVal = this.model.sliderCurrent;        
            }
        }

        public int VM_CurrentLine
        {
            get
            {
                currentLine = model.Data.CurrentLine;
                this.NotifyPropertyChanged("VM_time");
                return currentLine;
                
            }
            set
            {
                model.Data.CurrentLine = value;
                this.NotifyPropertyChanged("sliderVal");
                
                if (value < numberOfLines)
                {
                    VM_Play();
                }

            }
        }
        public int VM_NumberOfLines
        {
            get
            {
                return model.NumberOfLines;

            }
            set
            {
           
            }
        }
        
        public string VM_time
        {
            get
            {
                seconds = ((double)currentLine / 10) % 59;
                minutes = Math.Floor(((double)currentLine / 10) / 59);
                if (seconds < 10)
                {
                    time = Convert.ToInt32(minutes) + ":0" + Convert.ToInt32(seconds);
                    return time;
                }
                time = Convert.ToInt32(minutes) + ":" + Convert.ToInt32(seconds);
                return time;
            }
            set
            {
                
            }
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

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
