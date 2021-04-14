using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Flight_Sim.View;

namespace Flight_Sim
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FlightSimVM vm;
        public MainWindow()
        {
            Single.SetServer("localhost", 5400);
            vm = new FlightSimVM(Single.SingleFlightSimM());
            //vm = new FlightSimVM(new FlightSimM("localhost", 5400));
            DataContext = vm;
            InitializeComponent();
        }
        private void confirm_Button_Click(object sender, RoutedEventArgs e)
        {
            string path_csv = user_Csv_Path.Text;
            string path_xml = user_XML_Path.Text;
            string path_csvTrain = user_csvTrain_Path.Text;
            string path_dll = user_DLL_Path.Text;

            if (!File.Exists(path_csv) && !File.Exists(path_xml))
            {
                error_msg_csv.Visibility = Visibility.Visible;
                error_msg_xml.Visibility = Visibility.Visible;
                user_Csv_Path.Text = "";
                user_XML_Path.Text = "";
            }
            else if (!File.Exists(path_csv) && File.Exists(path_xml))
            {
                error_msg_csv.Visibility = Visibility.Visible;
                error_msg_xml.Visibility = Visibility.Hidden;
                user_Csv_Path.Text = "";
            }
            else if (File.Exists(path_csv) && !File.Exists(path_xml))
            {
                error_msg_csv.Visibility = Visibility.Hidden;
                error_msg_xml.Visibility = Visibility.Visible;
                user_XML_Path.Text = "";
            }
            else if (File.Exists(path_csv) && File.Exists(path_xml))
            {
                error_msg_csv.Visibility = Visibility.Hidden;
                error_msg_xml.Visibility = Visibility.Hidden;


                vm.Connect();
                 FlightSimApp fsa = new FlightSimApp();
                fsa.Show();
                GraphTest g = new GraphTest();
                g.Show();
                this.Close();
                // Thread.Sleep(500);



             

            }
        }

        private void CSV_Button_Click(object sender, RoutedEventArgs e)
        {

            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();

            // Launch OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = openFileDlg.ShowDialog();
            // Get the selected file name and display in a TextBox.
            // Load content of file in a TextBlock
            if (result == true)
            {
                this.vm.VM_CsvPath = openFileDlg.FileName;
                user_Csv_Path.Text = openFileDlg.FileName;
            }
        }

        private void XML_Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();

            // Launch OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = openFileDlg.ShowDialog();
            // Get the selected file name and display in a TextBox.
            // Load content of file in a TextBlock
            if (result == true)
            {
                user_XML_Path.Text = openFileDlg.FileName;
                this.vm.VM_XmlPath = openFileDlg.FileName;
            }
        }

        private void DLL_Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();

            // Launch OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = openFileDlg.ShowDialog();
            // Get the selected file name and display in a TextBox.
            // Load content of file in a TextBlock
            if (result == true)
            {
                user_DLL_Path.Text = openFileDlg.FileName;
                this.vm.VM_DLLPath = openFileDlg.FileName;
            }
        }

        private void CSVTrain_Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();

            // Launch OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = openFileDlg.ShowDialog();
            // Get the selected file name and display in a TextBox.
            // Load content of file in a TextBlock
            if (result == true)
            {
                user_csvTrain_Path.Text = openFileDlg.FileName;
                this.vm.VM_CSVTrainPath = openFileDlg.FileName;
            }
        }
    }
}
