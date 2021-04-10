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
using System.Xml;

namespace Flight_Sim.Model
{
    public class FlightSimM : IFlightSimM
    {
        private volatile int playRythm;
        private volatile string csvPath;
        private volatile string xmlPath;
        private Int32 port;
        private string serverPath;
        private Boolean stop;
        private int numOfCols;

        private int numberOfLines;
        private IDictionary<string, List<float>> table;
        private List<string> colNames;
        private FlightdataModel data;

        //Graphs
        private List<Point> points;


        public event PropertyChangedEventHandler PropertyChanged;
        //constructor
        public FlightSimM(string server, Int32 port)
        {
            this.serverPath = server;
            this.port = port;
            //default speed(X1)
            this.playRythm = 100;
            this.colNames = new List<string>();
            this.table = new Dictionary<string, List<float>>();
            //this.data = new FlightdataModel();
            this.stop = false;
            this.data = Single.SingleDataModel();
            this.points = new List<Point>();
        }

        public FlightdataModel GetFlightdata() { return data; }


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
        public void Play()
        {
           stop = false;
        }
       public void Stop()
        {
            this.data.CurrentLine = 0;
            stop = true;
        }
        public void Pause()
        {
            stop = true;
        }

        public string CsvPath
        {
            get
            {
                return this.csvPath;
            }
            set
            {
                if (this.csvPath != value)
                {
                    this.csvPath = value;

                }
            }
        }

        public string XmlPath
        {
            get
            {
                return this.xmlPath;
            }
            set
            {
                if (this.xmlPath != value)
                {
                    this.xmlPath = value;
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

        public int NumOfCols
        {
            get
            {
                return this.numOfCols;
            }
        }

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

        public FlightdataModel Data
        {
            get
            {
                return this.data;
            }
        }
    



        private void getColNames()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(this.xmlPath);
            XmlNode node = doc.DocumentElement.SelectSingleNode("/PropertyList/generic/output");
            foreach (XmlNode n in node)
            {
                if (n.Name.Equals("chunk"))
                {
                    string s1 = n.SelectSingleNode("name").InnerText;
                    if (colNames.Contains(s1))
                    {
                        colNames.Add(s1 + "2");
                    }
                    else
                    {
                        colNames.Add(s1);
                    }
                }
            }
            this.numOfCols = colNames.Count; 
            for (int i = 0; i < this.numOfCols; i++)
            {
                colNames[i] = colNames[i].Replace('-', '_');
                colNames[i] += "_p";
            }
        }


        /**
         *  Creating a Dictionary as a time Series.
         */
        private Dictionary<string, List<float>> CreateTable()
        {
            //Create a dicionary, with string as keys,and list of floats as values.
            var dic = new Dictionary<string, List<float>>();

            if (File.Exists(csvPath))
            {
                using (TextFieldParser parser = new TextFieldParser(csvPath))
                {
                    getColNames();
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    string[] fields;
                    //adding the rest of the data as value to associated with each key.
                    while (!parser.EndOfData)
                    {
                        //reading each line to a string array.
                        fields = parser.ReadFields();
                        //adding each value to its keys list.
                        for (int i = 0; i < dic.Count; i++)
                        {
                            dic[this.colNames[i]].Add(float.Parse(fields[i]));
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
        public void UpdateLine(string line)
        {
            float[] lineVal = Array.ConvertAll(line.Split(','), float.Parse);
             for (int i = 0; i < this.numOfCols ; i++)
              {
                 data.GetType().GetProperty(colNames[i]).SetValue(data, lineVal[i]);
              }
        }

        public void Connect()
        {
            string[] flightLines = File.ReadAllLines(csvPath);
            //number of line
            this.numberOfLines = flightLines.Length;
            for (int i = 0; i < numberOfLines; i++)
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
                new Thread(delegate ()
                {
                    while (true)
                    {
                        while (!stop)
                        {
                            //sending the lines 1 by 1 to the FG.
                            while (this.data.CurrentLine < numberOfLines)
                            {
                                //sending the line data as bytes to the fg.
                                Byte[] lineInBytes = System.Text.Encoding.ASCII.GetBytes(flightLines[this.data.CurrentLine]);
                                stream.Write(lineInBytes, 0, lineInBytes.Length);
                                //update the data class members.
                                UpdateLine(flightLines[this.data.CurrentLine]);
                                this.data.CurrentLine++;
                                Thread.Sleep(this.playRythm);
                            }
                            stop = true;
                        }
                        stream.Close();
                        client.Close();
                    }             
                }).Start();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("You have a null reference. Please try again with another file or check if the files aren't open.\n", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("Socket failed to open. Open the flightgear sim Please.\n", e);
            }
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}


