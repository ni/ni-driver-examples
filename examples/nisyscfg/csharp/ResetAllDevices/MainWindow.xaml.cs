using System.Windows;
using System.Windows.Controls;

namespace NationalInstruments.Examples.ResetAllDevices
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ResetAllDevicesWorker worker;

        public MainWindow()
        {
            InitializeComponent();
            worker = new ResetAllDevicesWorker();
            mainGrid.DataContext = worker;
        }

        private void OnResetDevicesClick(object sender, RoutedEventArgs e)
        {
            worker.GetAndResetDevices(PasswordBox.Password);
        }
    }
}
