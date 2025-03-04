﻿using System;
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
using Flight_Sim.cppToCSharp;

namespace Flight_Sim.Model
{
    public class FlightSimM : IFlightSimM
    {
        private volatile int playRythm;
        private volatile string csvPath;
        private volatile string xmlPath;
        private volatile string dllPath;
        private volatile string csvTrainPath;
        private Int32 port;
        private string serverPath;

        volatile Boolean stop;
        volatile public int sliderCurrent;
        private bool closeFlag;
        private int numOfCols;
        private int slider_al;
        int currentValSlider; //gets num between 0-100

        private int numberOfLines;
        private List<string> colDataNames;
        private FlightdataModel data;
        public event PropertyChangedEventHandler PropertyChanged;
        private ITimeSeriesAnomalyDetector aDetector;
        //Graphs



        //constructor
        public FlightSimM(string server, Int32 port)
        {
            this.serverPath = server;
            this.port = port;
            //default speed(X1)
            this.playRythm = 100;
            this.aDetector = new SimpleAnomalyDetector();
            this.colDataNames = new List<string>();
            this.stop = false;
            this.data = Single.SingleDataModel();
            this.closeFlag = false;
            aDetector = new SimpleAnomalyDetector();
            
            this.currentValSlider = 0;
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


        public int sliderVal
        {
            get
            {
                return currentValSlider;

            }
            set
            {
                if (this.currentValSlider != value)
                {
                    this.currentValSlider = value;
                    this.NotifyPropertyChanged("sliderVal");
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
        public int getCurrentLine()
        {
            return data.CurrentLine;
        }

        //change the rythem by the value we get
        public void changeRhythm(double newSpeedRhythm)
        {
            this.playRythm = Convert.ToInt32(100 /newSpeedRhythm);
        }

        public void ChangeTimeBySlider(double val)
        { 
            data.CurrentLine = Convert.ToInt32(val);
        }
        public void Close()
        {
            closeFlag = true;
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
        public string DLLPath
        {
            get
            {
                return this.dllPath;
            }
            set
            {
                if (this.dllPath != value)
                {
                    this.dllPath = value;
                }
            }
        }

        public string CSVTrainPath
        {
            get
            {
                return this.csvTrainPath;
            }
            set
            {
                if (this.csvTrainPath != value)
                {
                    this.csvTrainPath = value;
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

        public List<string> ColDataNames
        {
            get
            {
                return this.colDataNames;
            }
            set
            {
                if(this.colDataNames != value)
                {
                    this.colDataNames = value;
                }
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
                    if (colDataNames.Contains(s1))
                    {
                        data.ColNames.Add(s1 + "2");
                        colDataNames.Add(s1 + "2");
                    }
                    else
                    {
                        data.ColNames.Add(s1);
                        colDataNames.Add(s1);
                    }
                }
            }
            //initialize numOfCost field member
            this.numOfCols = colDataNames.Count; 
            for (int i = 0; i < this.numOfCols; i++)
            {
                this.data.ColNames[i] = data.ColNames[i].Replace('-', '_'); 
                colDataNames[i] = colDataNames[i].Replace('-', '_');
                colDataNames[i] += "_p";
            }
        }


        /***************************************************
         *  Creating a Dictionary as a time Series.
         ****************************************************/

        private Dictionary<string, List<float>> CreateTable()
        {
            //Create a dicionary, with string as keys,and list of floats as values.
            var dic = new Dictionary<string, List<float>>();

            if (File.Exists(csvPath))
            {
                using (TextFieldParser parser = new TextFieldParser(csvPath))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    string[] fields;
                    //adding the rest of the data as value to associated with each key.

                    foreach(string col in this.data.ColNames)
                    {
                        dic.Add(col, new List<float>());
                    }
                    while (!parser.EndOfData)
                    {
                        //reading each line to a string array.
                        fields = parser.ReadFields();
                        //adding each value to its keys list.
                        for (int i = 0; i < fields.Length; i++)
                        {
                            dic[this.data.ColNames[i]].Add(float.Parse(fields[i]));
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
         * 
         */
        public void UpdateLine(string line)
        {
            float[] lineVal = Array.ConvertAll(line.Split(','), float.Parse);
             for (int i = 0; i < this.numOfCols ; i++)
             {
                 data.GetType().GetProperty(colDataNames[i]).SetValue(data, lineVal[i]);             
             }
        }


        public void Connect()
        {
            string[] flightLines = File.ReadAllLines(csvPath);
            //number of line
            this.numberOfLines = flightLines.Length;
            //adding linefeed char in every single line.
            for (int i = 0; i < numberOfLines; i++)
            {
                flightLines[i] += "\n";
            }
            //Creating a Dictionary for future use.
            getColNames();
            this.data.Table = CreateTable();
            this.aDetector.LearnNormal(this.data.Table);
            this.data.CorrFeatures = this.aDetector.Cf;
            

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
                            for (; data.CurrentLine < numberOfLines; data.CurrentLine++)
                            {
                                double curr = (Convert.ToDouble(data.CurrentLine) / Convert.ToDouble(numberOfLines)) * 100.0;
                                this.currentValSlider = Convert.ToInt32(curr);

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
                                //update the data class members.
                                UpdateLine(flightLines[this.data.CurrentLine]);
                                this.data.CurrentLine++;
                                Thread.Sleep(this.playRythm);
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
                    //stream.Close();
                    //client.Close();
                    System.Environment.Exit(1); //in case of "close" - exit the window
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


