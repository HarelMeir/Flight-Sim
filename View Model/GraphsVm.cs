using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.ComponentModel;
using Flight_Sim;
using Flight_Sim.Model;

using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace Flight_Sim.View_Model 
{
    class GraphsVm : IFlightSimVM
    {
        private FlightdataModel data;

        private PlotModel plotModelFeatures, plotModelCor, plotModelReg;
        private Stopwatch clock;
        private long lastStopTime = 0;
        private OxyPlot.Wpf.PlotView pv_features, pvCor, pvReg;
        private List<float> featuresList, corrList, regList;

        public event PropertyChangedEventHandler PropertyChanged;
        
        public FlightdataModel Data
        {
            get
            {
                return this.data;
            }
        }
        public PlotModel VM_PlotModelFeatures
        {
            get { return this.plotModelFeatures; }
            set
            {
                if (VM_PlotModelFeatures != value)
                {
                    this.plotModelFeatures = value;
                    NotifyPropertyChanged("VM_PlotModelFeatures");
                }
            }
        }

        public PlotModel VM_PlotModelCor
        {
            get { return this.plotModelCor; }
            set
            {
                if (VM_PlotModelCor != value)
                {
                    this.plotModelCor = value;
                    NotifyPropertyChanged("VM_PlotModelCor");
                }
            }
        }

        public PlotModel VM_PlotModelReg
        {
            get { return this.plotModelReg; }
            set
            {
                if (VM_PlotModelReg != value)
                {
                    this.plotModelReg = value;
                    NotifyPropertyChanged("VM_PlotModelReg");
                }
            }
        }

        //list of features.
        public List<string> VM_colNames
        {
            get {  return this.data.ColNames; }
        }
        
        public int VM_CurrentLine
        {
            get { return this.data.CurrentLine; }
        }

        public string VM_ChosenFeature
        {
            get { return this.data.ChosenFeature; }
            set
            {
                if (this.data.ChosenFeature != value)
                {
                    this.data.ChosenFeature = value;
                    
                }
            }
        }

        public string VM_ChosenCorr
        {
            get {return  this.data.ChosenCorr; }
            set
            {
                if(VM_ChosenCorr != value)
                {
                    this.data.ChosenCorr = value;
                }
            }
        }

        public List<float> VM_ChosenValues
        {
            get { return this.data.ChosenValues; }
        }

        public List<float> FeatureList
        {
            get { return this.featuresList; }
            set
            {
                 if(this.featuresList != value)
                {
                    this.featuresList = value;
                }
            }
        }

        public List<float> CorrList
        {
            get { return this.corrList; }
            set
            {
                if(this.corrList != value)
                {
                    this.corrList = value;
                }
            }
        }

        public List<float> RegList
        {
            get { return this.regList; }
            set
            {
                if (this.regList != value)
                {
                    this.regList = value;
                }
            }
        }
        public GraphsVm(FlightdataModel data, OxyPlot.Wpf.PlotView pv_features, OxyPlot.Wpf.PlotView pvCorr, OxyPlot.Wpf.PlotView pvReg)
        {
            //initizlize dat model.
            this.data = data;
            //setting up the pv
            this.pv_features = pv_features;
            this.pvCor = pvCorr;
            this.pvReg = pvReg;

            //creating a stopwatch,to set up the time.
            this.clock = new Stopwatch();
            clock.Start();
            //initizling plotmodel and list.
            this.plotModelFeatures = new PlotModel();
            this.plotModelCor = new PlotModel();
            this.plotModelReg = new PlotModel();

            this.featuresList = new List<float>();
            this.corrList = new List<float>();

            //usual mvvm propertychanged setup.
            this.data.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);
        
            };

            //at every line property change - build the graph after 450 ms passed.
            this.data.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                //if currentline has been changed,and there more then 500 ms passed - set up the graph again for cleaner view.
                if (clock.ElapsedMilliseconds > 450 + lastStopTime &&e.PropertyName == "CurrentLine" 
                && VM_ChosenFeature != null && VM_ChosenCorr != null)
                {
                    //plot1
                    this.VM_PlotModelFeatures.Series.Clear();
                    FeatureList = this.data.FeatureChosenValues();
                    SetUpModel(VM_PlotModelFeatures);
                    LoadData(featuresList , VM_PlotModelFeatures);
                    this.pv_features.InvalidatePlot(true);

                    //plot2
                    Updatecorr(VM_ChosenFeature);
                    corrList = data.FeatureChosenCorrValues();
                    this.VM_PlotModelCor.Series.Clear();
                    SetUpModel(VM_PlotModelCor);
                    LoadData(corrList, VM_PlotModelCor);
                    this.pvCor.InvalidatePlot(true);

                    //plot3
                        this.VM_PlotModelReg.Series.Clear();
                    SetUpModel(VM_PlotModelReg);
                    LoadRegData(FeatureList, corrList, VM_PlotModelReg);
                    this.pvReg.InvalidatePlot(true);

                    this.lastStopTime = clock.ElapsedMilliseconds;
                }
            };
        }

        //set up the graph
        public void SetUpModel(PlotModel plotM)
        {
            plotM = new PlotModel()
            {
                Background = OxyColor.FromAColor(200, OxyColors.White),
                LegendOrientation = LegendOrientation.Horizontal,
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.TopRight,       
                LegendBorder = OxyColors.Black
            };

            var timeAxis = new LinearAxis() { Position = AxisPosition.Bottom, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, IntervalLength = 80, Title = "Time" };
            plotM.Axes.Add(timeAxis);
            var valueAxis = new LinearAxis() { Position = AxisPosition.Left, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = "Value" };
            plotM.Axes.Add(valueAxis);
        }

        //loading the data(points to the line)
        public void LoadData(List<float> values, PlotModel plotM)
        {
            int linenumber = VM_CurrentLine;
            LineSeries lineSer = new LineSeries()
            {
                StrokeThickness = 2,
                Color = OxyColors.Red,
            };
            for(int i = 0; i < linenumber; i++)
            {
                lineSer.Points.Add(new DataPoint(i, values[i]));            
            }
            plotM.Series.Add(lineSer);
        }

        //find the most corr feature to the selected one.
        public void Updatecorr(string fName)
        {
            this.data.FeatureChosenCorr(fName);
        }




        //load the reg_line scattered graph.
        public void LoadRegData(List<float> values1, List<float> values2, PlotModel plotM)
        {
            int lineNumber = VM_CurrentLine;
            Line regLine = Data.CorrLinReg(this.VM_ChosenFeature, this.VM_ChosenCorr);
            
            LineSeries lineSer = new LineSeries()
            {
                StrokeThickness = 2,
                Color = OxyColors.Black,
            };

            //adding a line from the max point to the smallest.
            lineSer.Points.Add(new DataPoint(values1.Max(), regLine.LineEx(values1.Max())));
            lineSer.Points.Add(new DataPoint(values1.Min(), regLine.LineEx(values1.Min())));
            

            ScatterSeries scatterP = new ScatterSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerFill = OxyColors.Black,
            };

            ScatterSeries last300P = new ScatterSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerFill = OxyColors.BlueViolet,
            };
           
            if (lineNumber > 300)
            {
                for (int i = 0; i < lineNumber - 300; i++)
                {
                    scatterP.Points.Add(new ScatterPoint(values1[i], values2[i], 1.5));
                }
                for (int j = lineNumber - 300; j < lineNumber; j++)
                {
                    last300P.Points.Add(new ScatterPoint(values1[j], values2[j], 2));
                }
            }
            else
            {
                for (int k = 0; k < lineNumber; k++)
                {
                    last300P.Points.Add(new ScatterPoint(values1[k], values2[k], 2));
                }
            }
            //adding the points to the plotmodel
            plotM.Series.Add(last300P);
            plotM.Series.Add(scatterP);
            plotM.Series.Add(lineSer);

        }

        // INotifyPropertyChanged
        public void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
