using System.Windows;
using System.Windows.Controls;

namespace NationalInstruments.Examples.CalibrationAudit
{
    public partial class MainWindow : Window
    {
        private CalibrationAuditWorker worker;

        public MainWindow()
        {
            InitializeComponent();
            worker = new CalibrationAuditWorker();
            mainGrid.DataContext = worker;
        }

        private void OnRunAuditClick(object sender, RoutedEventArgs e)
        {
            worker.StartRunAudit(passwordBox.Password);
        }
    }
}
