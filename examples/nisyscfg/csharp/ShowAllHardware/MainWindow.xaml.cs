using System.Windows;
using System.Windows.Controls;

namespace NationalInstruments.Examples.ShowAllHardware
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            worker = new ShowHardwareWorker();
            mainGrid.DataContext = worker;
        }

        private ShowHardwareWorker worker;

        private void OnShowHardwareClick(object sender, RoutedEventArgs e)
        {
            worker.StartShowHardware(passwordBox.Password);
        }
    }
}
