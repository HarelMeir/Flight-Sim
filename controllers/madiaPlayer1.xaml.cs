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

namespace Flight_Sim.controllers
{
    /// <summary>
    /// Interaction logic for madiaPlayer1.xaml
    /// </summary>
    /// 
    public partial class madiaPlayer1 : UserControl
    {
        public madiaPlayer1()
        {
            mediaPlayerViewModel vm;
            InitializeComponent();
            Image b = new Image();
            b.Name = "play";

        }

        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            // playFlag = true;
            Console.WriteLine("adassdsadfjlkjkl");
        }
        private void pauseButton_Click(object sender, RoutedEventArgs e)
        {
            // playFlag = false;
            Console.WriteLine("adassdsadfjlkjkl");
        }
        private void rightButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("adassdsadfjlkjkl");
        }
        private void rightStopButton_Click(object sender, RoutedEventArgs e)
        {

            Console.WriteLine("adassdsadfjlkjkl");
        }
        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("adassdsadfjlkjkl");
        }
        private void leftButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("adassdsadfjlkjkl");
        }
        private void leftStop_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("adassdsadfjlkjkl");
        }
        private void leftPauseButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("adassdsadfjlkjkl");
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }
}
