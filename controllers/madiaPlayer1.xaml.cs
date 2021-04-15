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
using Flight_Sim;

namespace Flight_Sim.controllers
{
    /// <summary>
    /// Interaction logic for madiaPlayer1.xaml
    /// </summary>
    /// 
    public partial class madiaPlayer1 : UserControl
    {
        mediaPlayer1VM vm;
        
        public madiaPlayer1()
        {
            
            InitializeComponent();
            this.vm = new mediaPlayer1VM(Single.SingleFlightSimM());
            Image b = new Image();
            b.Name = "play";
            DataContext = vm;
           


        }

        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            this.vm.VM_Play();
        }
        private void pauseButton_Click(object sender, RoutedEventArgs e)
        {
            this.vm.VM_Pause();
        }
        private void rightButton_Click(object sender, RoutedEventArgs e)
        {
            this.vm.VM_rightButton();
        }
        private void rightStopButton_Click(object sender, RoutedEventArgs e)
        {

            this.vm.VM_rightStopButton();
        }
        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            this.vm.VM_Stop();
        }
        private void leftButton_Click(object sender, RoutedEventArgs e)
        {
            this.vm.VM_leftButton();
        }
        private void leftStopButton_Click(object sender, RoutedEventArgs e)
        {
            this.vm.VM_leftStopButton();
        }



        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            /*double sliderVal = Convert.ToDouble(((Slider)sender).Value);
            this.vm.VM_changeTimeBySlider(sliderVal);
           // slider.Value = Single.SingleDataModel().CurrentLine;*/
        }

        private void SpeedField_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (((TextBox)sender).Text == "") {
                this.vm.SpeedVal = 10;
            }
            else {
                this.vm.SpeedVal = Double.Parse(((TextBox)sender).Text);
            }
        }




        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.vm.VM_close();
        }
    }
}
