using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
//Graphs
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using Flight_Sim.View_Model;
using System.Collections;
using System.Collections.ObjectModel;

namespace Flight_Sim.View
{
    /// <summary>
    /// Interaction logic for Graphs.xaml
    /// </summary>
    public partial class Graphs : UserControl
    {
        private GraphsVm gVM;
        private string featureName, corrName;
        private OxyPlot.Wpf.PlotView fPlot, cPlot, rPlot;
        public Graphs()
        {
            InitializeComponent();
            this.gVM = new GraphsVm(Single.SingleDataModel(), featuresPlot, corrPlot, regPlot);
            this.DataContext = gVM;
            
            //creating 42 buttons
            for (int i = 0; i < gVM.VM_colNames.Count; i++)
            {
                Button b = new Button();
                b.Click += Button_Click;
                b.Content = this.gVM.VM_colNames[i];
                b.Width = 250;
                b.Background = Brushes.White;
                this.fList.Children.Add(b);
            }
            fPlot = featuresPlot;
            cPlot = corrPlot;
            rPlot = regPlot;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
            //convert the current feature name of the button to string
            featureName = (sender as Button).Content.ToString();
            //set the chosenFeature property of the vm to it.
            this.gVM.VM_ChosenFeature = featureName;
            //setting up its PlotModel
            this.gVM.VM_PlotModelFeatures.Series.Clear();
            this.gVM.SetUpModel(this.gVM.VM_PlotModelFeatures);
            //setting up the lines
            this.gVM.LoadData(gVM.Data.FeatureChosenValues(), this.gVM.VM_PlotModelFeatures);
            //"refreshing" the graph.
            fPlot.InvalidatePlot(true);

            //again for Corrlation graph.
            this.gVM.Updatecorr(featureName);
            this.gVM.VM_PlotModelCor.Series.Clear();
            this.gVM.SetUpModel(this.gVM.VM_PlotModelCor);
            this.gVM.LoadData(gVM.Data.FeatureChosenCorrValues(), this.gVM.VM_PlotModelCor);
            this.corrPlot.InvalidatePlot(true);

            //and for regLine graph
            this.gVM.VM_PlotModelReg.Series.Clear();
            this.gVM.SetUpModel(this.gVM.VM_PlotModelReg);
            this.gVM.LoadRegData(gVM.Data.FeatureChosenValues(), gVM.Data.FeatureChosenCorrValues(), gVM.VM_PlotModelReg);
            this.rPlot.InvalidatePlot(true);

        }
    }
}
