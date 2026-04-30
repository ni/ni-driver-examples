using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using NationalInstruments.SystemConfiguration;

namespace NationalInstruments.Examples.GenerateMAXReport
{
    public partial class MainWindow : Window
    {
        private ReportWorker worker;
        private ReportType reportType = ReportType.Html;
        private string reportTypeExtension = "html";

        public MainWindow()
        {
            InitializeComponent();
            worker = new ReportWorker();
            mainGrid.DataContext = worker;
        }

        private void OnBrowseClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileExplorer = new OpenFileDialog();
            fileExplorer.Title = "Choose a report directory and filename";
            fileExplorer.CheckFileExists = false;
            // Restrict file type of file dialog. For instance, the filter for XML is: "XML files (*.xml)|*.xml".
            fileExplorer.Filter = reportTypeExtension + " files (*." + reportTypeExtension + ")|*." + reportTypeExtension;
            fileExplorer.ShowDialog();
            filePathBox.Text = fileExplorer.FileName;
        }

        private void OnGenerateReportClick(object sender, RoutedEventArgs e)
        {
            worker.ReportType = reportType;

            if (string.IsNullOrEmpty(filePathBox.Text))
            {
                filePathBox.Text = System.IO.Path.GetTempPath() + "MAXReport." + reportTypeExtension;
            }

            worker.GenerateReport(passwordBox.Password);
        }

        private void ReportTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // A SelectionChanged event alone will not reflect the newly selected option.
            // We must explicitly obtain the text in ReportTypeBox for the new selection.
            string reportTypeText = (e.AddedItems[0] as ComboBoxItem).Content as string;

            if (reportTypeText == "XML")
            {
                reportType = ReportType.Xml;
                reportTypeExtension = "xml";
            }
            else if (reportTypeText == "Technical Support")
            {
                reportType = ReportType.TechnicalSupportZip;
                reportTypeExtension = "zip";
            }
            else
            {
                reportType = ReportType.Html;
                reportTypeExtension = "html";
            }
        }
    }
}
