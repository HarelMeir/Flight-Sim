﻿using System;
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

namespace Flight_Sim
{
    /// <summary>
    /// Interaction logic for UserStory5.xaml
    /// </summary>
    public partial class UserStory5 : UserControl
    {
        public UserStory5()
        {
            InitializeComponent();
            UserStory5VM vm = new UserStory5VM(new FlightdataModel());
            DataContext = vm;
        }
    }
}
