using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.ComponentModel;

namespace Flight_Sim
{
    class FlightSimM : IFlightSimM
    {
        private int playRythm;
        private volatile string filePath;
        private Int32 port;
        private string serverPath;
        public event PropertyChangedEventHandler PropertyChanged;
        volatile Boolean stop;


        //constructor
        public FlightSimM(string server, Int32 port)
        {
            this.serverPath = server;
            this.port = port;
            stop = false;
        }

        //Properties
        public int PlaySpeed
        {
            get
            {
                return this.playRythm;
            }
            set
            {
                if (this.playRythm != value)
                {
                    this.playRythm = value;
                    this.NotifyPropertyChanged("PlaySpeed");
                }

            }
        }

        public string FilePath
        {
            get
            {
                return this.filePath;
            }
            set
            {
                if (this.filePath != value)
                {
                    this.filePath = value;

                }
            }
        }

        public Int32 Port
        {
            get
            {
                return this.port;
            }
            set
            {
                if (this.port != value)
                {
                    this.port = value;
                    NotifyPropertyChanged("Port");
                }
            }
        }

        public string Server
        {
            get
            {
                return this.serverPath;
            }
            set
            {
                if (this.serverPath != value)
                {
                    this.serverPath = value;
                    NotifyPropertyChanged("Server");
                }
            }
        }


        public void Connect()
        {
            string[] flightLines = File.ReadAllLines(filePath);
            try
            {
                TcpClient client = new TcpClient(serverPath, port);
                NetworkStream stream = client.GetStream();
                int numOfLines = flightLines.Length;
                this.playRythm = 100;


                new Thread(delegate ()
                {
                    while (!stop)
                    {

                        for (int i = 1; i < numOfLines; i++)
                        {
                            flightLines[i] += "\n";
                            Byte[] lineInBytes = System.Text.Encoding.ASCII.GetBytes(flightLines[i]);
                            stream.Write(lineInBytes, 0, lineInBytes.Length);
                            Thread.Sleep(playRythm);
                        }
                        stop = true;
                    }

                    stream.Close();
                    client.Close();
                }).Start();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("You have a null reference. plz try again with another file. Or check if the files arent open.", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("socket failed to open,open the flightgear sim", e);
            }
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

