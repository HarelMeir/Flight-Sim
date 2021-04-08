using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.ComponentModel;
using Microsoft.VisualBasic.FileIO;

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


        //members added after commit
        private int numberOfLines;
        private IDictionary<string, List<float>> table;
        private List<string> colNames;
        private int currentLine;
        private FlightdataModel data;


        //constructor
        public FlightSimM(string server, Int32 port)
        {
            this.serverPath = server;
            this.port = port;
            //default speed(X1)
            this.playRythm = 100;
            stop = false;
            this.colNames = new List<string>();
            this.table = new Dictionary<string, List<float>>();
            this.data = new FlightdataModel();
            this.currentLine = 1;
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
        //properties added after commit.
        public int NumberOfLines
        {
            get
            {
                return this.numberOfLines;
            }

            set
            {
                if (this.numberOfLines != value)
                {
                    this.numberOfLines = value;
                    NotifyPropertyChanged("NumberOfLines");
                }
            }
        }


        public IDictionary<string, List<float>> Table
        {
            get
            {
                return this.table;
            }

            set
            {
                if (this.table != value)
                {
                    this.table = value;
                    NotifyPropertyChanged("Table");
                }
            }
        }

        public List<string> ColNames
        {
            get
            {
                return this.ColNames;
            }
        }

        public int CurrentLine
        {
            get
            {
                return this.currentLine;
            }

            set
            {
                if (this.currentLine != value)
                {
                    this.currentLine = value;
                    NotifyPropertyChanged("CurrentLine");
                }
            }
        }



        /**
         *  Creating a Dictionary as a time Series.
         */
        private Dictionary<string, List<float>> CreateTable()
        {
            //Create a dicionary, with string as keys,and list of floats as values.
            var dic = new Dictionary<string, List<float>>();

            if (File.Exists(filePath))
            {
                using (TextFieldParser parser = new TextFieldParser(filePath))
                {
                    //using parser
                    //setting up colnames,and add them as keys.
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    //reading the first line - the featurs.
                    string[] columns = parser.ReadFields();
                    foreach (string col in columns)
                    {
                        this.colNames.Add((col + "_p").Replace("-", "_"));
                        dic.Add(col, new List<float>());
                    }
                    string[] fields;
                    //adding the rest of the data as value to associated with each key.
                    while (!parser.EndOfData)
                    {
                        //reading each line to a string array.
                        fields = parser.ReadFields();
                        //adding each value to its keys list.
                        for (int i = 0; i < dic.Count; i++)
                        {
                            dic[columns[i]].Add(float.Parse(fields[i]));
                        }
                    }
                }
                //setting up number of lines.
            }
            return dic;
        }
        /**
         *  The idea: 
         *  parse every line that we send to the FG,update FlightDataModel according to its properties,
         *  that are saved at (this.colNames). 
         *  And every time the dataModel properties being updated, they send notication the the different VM that listens.
         *  its probobly relevant from user Story 3+.
         *  im going to sleep. ill dream about you AVEV.
         * 
         */
        /*   public void ParseLine(string line)
           {
               for(int i = 0; i < colNames.Count; i++)
               {
                    
               }
           }*/

        public void Connect()
        {
            string[] flightLines = File.ReadAllLines(filePath);
            //number of line
            this.numberOfLines = flightLines.Length;
            for (int i = 1; i < numberOfLines; i++)
            {
                flightLines[i] += "\n";
            }
            //Creating a Dictionary for future use.
            this.table = CreateTable();

            try
            {
                //tcp client
                TcpClient client = new TcpClient(serverPath, port);
                NetworkStream stream = client.GetStream();

                //sending the lines 1 by 1 to the FG.
                new Thread(delegate ()
                {
                    while (!stop)
                    {
                        for (int i = 1; i < numberOfLines; i++)
                        {
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

