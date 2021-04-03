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

namespace Flight_Sim.User_Story_3
{
    /// <summary>
    /// Interaction logic for JoystickControl.xaml
    /// </summary>

    
    public partial class JoystickControl : UserControl
    {
        public string ImageName;
        public JoystickControl()
        {
            ImageName = "0 - neutral joystick.png";
            InitializeComponent();
        }
    }
}
