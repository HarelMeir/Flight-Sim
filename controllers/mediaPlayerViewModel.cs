using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Sim.controllers
{
    class mediaPlayerViewModel
    {
        mediaPlayerModel model;
        bool VM_play;
        int VM_speedPlay;
        double VM_currentTime;
        
        mediaPlayerViewModel()
        {
            this.VM_play = false;
            this.VM_speedPlay = 1; //defaulf play speed
            this.VM_currentTime = 0.0; //default current time

        }
    }
}
