using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;
using System.Threading;
using Flight_Sim.Model;



namespace Flight_Sim
{
    public class FlightSimVM : IFlightSimVM
    {

        //fields
        private FlightSimM model;
        public event PropertyChangedEventHandler PropertyChanged;

        //Constructor
        public FlightSimVM(FlightSimM model)
        {
            this.model = model;
            this.model.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }

        //Properties
        public int VM_PlaySpeed
        {
            get
            {
                return this.model.PlaySpeed;
            }
            set
            {
                if (this.VM_PlaySpeed != value)
                {
                    this.VM_PlaySpeed = value;
                    this.NotifyPropertyChanged("VM_PlaySpeed");
                }
            }
        }

        public string VM_CsvPath
        {
            get
            {
                return this.model.CsvPath;
            }
            set
            {
                if (this.VM_CsvPath != value)
                {
                    this.model.CsvPath = value;
                }
            }
        }

        public Int32 VM_Port
        {
            get
            {
                return this.model.Port;
            }
        }

        public string Server
        {
            get
            {
                return this.model.Server;
            }
        }

        public string VM_XmlPath
        {
            get
            {
                return model.XmlPath;
            }
            set
            {
                this.model.XmlPath = value;
            }
        }

        public string VM_DLLPath {
            get
            {
                return model.DLLPath;
            }
            set
            {
                this.model.DLLPath = value;
            }
        }

        public void VM_Play()
        {
            this.model.Play();
        }


        public void Connect()
        {
            if (VM_CsvPath == null)
            {
                MessageBox.Show("An error occured. Make sure XML and CSV file are loaded.");
            }
            else
            {
                this.model.Connect();
            }
        }


        public void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

