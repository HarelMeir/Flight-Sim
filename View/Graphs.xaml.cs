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
        private string featureName;
        private OxyPlot.Wpf.PlotView fPlot;
        public Graphs()
        {
            InitializeComponent();
            this.gVM = new GraphsVm(Single.SingleDataModel(), featuresPlot);
            this.DataContext = gVM;
            
            //creating 42 buttons
            for (int i = 0; i < gVM.VM_colNames.Count; i++)
            {
                Button b = new Button();
                b.Click += Button_Click;
                b.Content = this.gVM.VM_colNames[i];
                b.Width = 400;
                this.fList.Children.Add(b);
            }
            fPlot = featuresPlot;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            featureName = (sender as Button).Content.ToString();
            this.gVM.VM_ChosenFeature = featureName;

            this.gVM.SetUpModel();
            this.gVM.LoadData(gVM.FeatureList);
            fPlot.InvalidatePlot(true);
        }
    }
}
