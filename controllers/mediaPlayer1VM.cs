using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Sim.controllers
{
    public class mediaPlayer1VM
    {
        FlightSimM model;
        public mediaPlayer1VM(FlightSimM model)
        {
            this.model = model;
        }

        public void VM_Play()
        {
            this.model.Play();
        }
        public void VM_Pause()
        {
            this.model.Pause();
        }
        /* public void VM_Stop()
         {
             this.model.Stop();
         }

         public void VM_rightButton()
         {
             this.model.rightButton();
         }
         public void VM_rightStopButton()
         {
             this.model.rightStopButton();
         }
         public void VM_leftButton()
         {
             this.model.leftButton();
         }
         public void VM_leftStopButton()
         {
             this.model.Play();
         }

         public void VM_changeTimeSlider()
         {
             this.model.Play();
         }*/

    }
}
