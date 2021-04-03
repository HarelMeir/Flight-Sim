using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;
using System.Threading;


namespace Flight_Sim
{
    class FlightSimVM : IFlightSimVM
    {
            
        //fields
        private IFlightSimM model;
        public event PropertyChangedEventHandler PropertyChanged;


        //Constructor
        public FlightSimVM()
        {
            this.model = new FlightSimM("localhost", 5400);
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
        }

        public string VM_FilePath
        {
            get
            {
                return this.model.FilePath;
            }
            set
            {
                if (this.VM_FilePath != value)
                {
                    this.model.FilePath = value;
                    NotifyPropertyChanged("VM_FilePath");
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

        public void Connect()
        {
            Console.WriteLine("this is Csv filepath----------------->" + VM_FilePath);
            if (VM_FilePath == null)
            {
                MessageBox.Show("An error occured. Make sure XML and CSV file are loaded.");
            }
            else
            {
                Console.WriteLine("GDFGDSFGDFSGDFGDSFGDFSGFDSGDFSGFDGFD");
                model.Connect();
            }
        }


        public void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

