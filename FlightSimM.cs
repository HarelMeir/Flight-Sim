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
    public class FlightSimM : IFlightSimM
    {
        volatile private int playRythm;
        private volatile string filePath;
        private Int32 port;
        private string serverPath;
        public event PropertyChangedEventHandler PropertyChanged;
        volatile Boolean stop;
        volatile public int sliderCurrent;
        private bool closeFlag;


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
            //this.data = new FlightdataModel();
            this.data = Single.SingleDataModel();
            this.currentLine = 1;
            this.closeFlag = false;
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
            data.CurrentLine = 1;
            stop = true;
        }
        public void Pause()
        {
            stop = true;
        }

        public void RightButton()
        {
            data.CurrentLine = data.CurrentLine + 10;
        }
        public void LeftButton()
        {
            data.CurrentLine = data.CurrentLine - 10;
        }
        public void RightStopButton()
        {
            data.CurrentLine = numberOfLines - 1;
        }
        public void LeftStopButton()
        {
            data.CurrentLine = 0;
        }

        public void changeRhythm(double newSpeedRhythm)
        {
            this.playRythm = Convert.ToInt32(newSpeedRhythm);
        }
        public void ChangeTimeBySlider(double val)
        {
            double v = (val / 100) * numberOfLines;
            //sliderCurrent = Convert.ToInt32(val);
            data.CurrentLine = Convert.ToInt32(v);
        }
        public void Close()
        {
            closeFlag = true;
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
                    while (true)
                    {
                        while (!stop)
                        {
                            for (; data.CurrentLine < numberOfLines; data.CurrentLine++)
                            {
                                if(stop)
                                {
                                    break;
                                }
                                if (closeFlag)
                                {
                                    break;
                                }
                                Byte[] lineInBytes = System.Text.Encoding.ASCII.GetBytes(flightLines[data.CurrentLine]);

                                stream.Write(lineInBytes, 0, lineInBytes.Length);
                                Thread.Sleep(playRythm);
                               
                                
                            }
                            stop = true;
                            if (closeFlag)
                            {
                                break;
                            }
                        }
                        if (closeFlag)
                        {
                            break;
                        }
                    }
                    stream.Close();
                    client.Close();
                    System.Environment.Exit(1); //in case of "close" - exit the window
                }).Start();
                
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("You have a null reference. Please try again with another file or check if the files aren't open.\n", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("Socket failed to open. Open the flightgear sim.\n", e);
            }
            
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}




/* 
                            /////////////////////////////////// Ariel tests
                            data.CurrentLine = i;
                            if (i % 10 == 0)
                            { data.rudder_p = -1;
                                data.throttle1_p = (float)-1;
                                data.elevator_p = 1;
                            }
                            if (i % 10 == 1)
                            { data.rudder_p = (float)-0.8;
                               data.throttle1_p = (float)-0.8;
                                data.elevator_p = 2;
                                data.airleron_p = 1;
                            }
                            if (i % 10 == 2)
                            { data.rudder_p = (float)-0.6; ;
                                data.throttle1_p = (float)-0.6;
                                data.airleron_p = 2;
                            }
                            if (i % 10 == 3)
                            { data.rudder_p = (float)-0.4;
                                data.throttle1_p = (float)-0.4;
                                data.elevator_p = 0;
                                data.airleron_p = 3;
                            }
                            if (i % 10 == 4)
                            { data.rudder_p = (float)-0.2;
                                data.throttle1_p = (float)-0.2;
                                data.elevator_p = -1;
                            }
                            if (i % 10 == 5)
                            { data.rudder_p = (float)0;
                                data.throttle1_p = (float)0;
                                data.elevator_p = -2;
                                data.airleron_p = -2;
                            }
                            if (i % 10 == 6)
                            { data.rudder_p = (float)0.2;
                                data.throttle1_p = (float)0.2;
                                data.airleron_p = -3;
                            }
                            if (i % 10 == 7)
                            { data.rudder_p = (float)0.4;
                                data.throttle1_p = (float)0.4;
                                data.elevator_p = 0;
                                data.airleron_p = -4;
                            }
                            if (i % 10 == 8)
                            { data.rudder_p = (float)0.6;
                                data.throttle1_p = (float)0.8;
                            }
                            if (i % 10 == 9)
                            { data.rudder_p = 1;
                                data.throttle1_p = (float)1;
                            }
                            if (i % 3 == 0){
                                data.airspeed_kt_p = -2;
                                data.altitude_ft_p = -1;
                                data.heading_deg_p = 0;
                                data.roll_deg_p = 1;
                                data.pitch_deg_p = 2;
                                data.side_slip_deg_p = 3;
                            }
                            if (i % 3 == 1)
                            {
                                data.airspeed_kt_p = -1;
                                data.altitude_ft_p = 0;
                                data.heading_deg_p = 1;
                                data.roll_deg_p = 2;
                                data.pitch_deg_p = 3;
                                data.side_slip_deg_p = 4;
                            }
                            if (i % 3 == 2)
                            {
                                data.airspeed_kt_p = 0;
                                data.altitude_ft_p = 1;
                                data.heading_deg_p = 2;
                                data.roll_deg_p = 3;
                                data.pitch_deg_p = 4;
                                data.side_slip_deg_p = 5;
                            }/////////////////////////////
 */
