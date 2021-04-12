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

        private PlotModel plotModel;
        private Stopwatch clock;
        private long lastStopTime = 0;
        private OxyPlot.Wpf.PlotView pv_features;
        private List<float> featuresList;

        public event PropertyChangedEventHandler PropertyChanged;
        

        public PlotModel VM_PlotModel
        {
            get { return plotModel; }
            set
            {
                if (VM_PlotModel != value)
                {
                    this.plotModel = value;
                    NotifyPropertyChanged("VM_PlotModel");
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
        public GraphsVm(FlightdataModel data, OxyPlot.Wpf.PlotView pv_features)
        {
            //initizlize dat model.
            this.data = data;
            this.pv_features = pv_features;
            this.clock = new Stopwatch();
            clock.Start();
            this.plotModel = new PlotModel();
            this.featuresList = new List<float>();

            this.data.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);
        
            };

            //at every line property change - build the graph after 450 ms passed.
            this.data.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == "CurrentLine" && VM_ChosenFeature != null)
                {
                    if (clock.ElapsedMilliseconds > 450 + lastStopTime)
                    {
                        VM_PlotModel.Series.Clear();
                        SetUpModel();
                        LoadData(featuresList);
                        this.pv_features.InvalidatePlot(true);
                        this.lastStopTime = clock.ElapsedMilliseconds;
                    }
                }
            };
        }

        //set up the graph
        public void SetUpModel()
        {
            VM_PlotModel.LegendTitle = "Feature Graph";
            VM_PlotModel.LegendOrientation = LegendOrientation.Horizontal;
            VM_PlotModel.LegendPlacement = LegendPlacement.Outside;
            VM_PlotModel.LegendPosition = LegendPosition.TopRight;
            VM_PlotModel.Background = OxyColor.FromAColor(200, OxyColors.White);
            VM_PlotModel.LegendBorder = OxyColors.Black;

            var timeAxis = new LinearAxis() { Position = AxisPosition.Bottom, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, IntervalLength = 80, Title = "Time" };
            VM_PlotModel.Axes.Add(timeAxis);
            var valueAxis = new LinearAxis() { Position = AxisPosition.Left, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = "Value" };
            VM_PlotModel.Axes.Add(valueAxis);

        }

        //loading the data(points to the line)
        public void LoadData(List<float> featureValues)
        {
            this.featuresList = this.data.FeatureChosenValues();
            int linenumber = VM_CurrentLine;
            LineSeries lineSer = new LineSeries()
            {
                StrokeThickness = 2,
                Color = OxyColors.Red,
            };
            for(int i = 0; i < linenumber; i++)
            {
                lineSer.Points.Add(new DataPoint(i, this.featuresList[i]));            
            }
            VM_PlotModel.Series.Add(lineSer);
        }

        // INotifyPropertyChanged
        public void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
