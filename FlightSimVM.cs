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
    public class FlightSimVM : IFlightSimVM
    {

        //fields
        private FlightSimM model;
        public event PropertyChangedEventHandler PropertyChanged;
        private Thread fgThread;

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
               /* if (this.PlaySpeed != value)
                {
                    this.PlaySpeed = value;
                    this.NotifyPropertyChanged("PlaySpeed");
                }*/
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
                if(this.fgThread == null || !this.fgThread.IsAlive)
                {
                    Console.WriteLine("GFSGDSFGFSDGDSFGDFSGDSFGDSFGDFSGDSFGDSFGDSF");
                    this.fgThread = new Thread(delegate ()
                    {
                        this.model.Connect();
                    });
                    this.fgThread.Start();
                }

            }
        }


        public void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

