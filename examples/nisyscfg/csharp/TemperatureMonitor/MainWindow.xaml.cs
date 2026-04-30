using System.Windows;
using System.Windows.Controls;

namespace NationalInstruments.Examples.BoardTemperatureMonitor
{
    public partial class MainWindow : Window
    {
        private BoardTemperatureMonitorWorker worker;

        public MainWindow()
        {
            InitializeComponent();
            worker = new BoardTemperatureMonitorWorker();
            mainGrid.DataContext = worker;
            TemperatureLimitBox.Text = "50";
            TemperatureSlider.Value = 50;
        }

        private void OnRunAuditClick(object sender, RoutedEventArgs e)
        {
            worker.StartTemperatureMonitor(passwordBox.Password);
        }

        private void OnStopButtonClick(object sender, RoutedEventArgs e)
        {
            worker.StopMonitor = true;
        }
    }
}
