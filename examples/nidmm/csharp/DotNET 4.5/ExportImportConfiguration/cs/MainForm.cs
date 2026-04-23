
//==================================================================================================
// Title        : NI-DMM Export/Import Configuration File
// Description  : This example demonstrates how to use the Export and Import Attribute Configuration File APIs.
//                Clicking Export will prompt for a file to save the attribute configuration into; initialize a session;
//                configure the measurement function, range, digits of resolution and power line frequency; and export
//                those attributes to file.
//
//                Clicking Import will prompt for a file to load the attribute configuration from; initialize a session;
//                import the attribute configuration into the session; and display those attributes on the window.
//
//                Clicking Read will take a single measurement with the configuration displayed on the window.
//
//===================================================================================================

using System;
using System.IO;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIDmm;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.ExportImportConfiguration
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            LoadDmmDeviceNames();
            LoadPowerlineFrequencyValues();
            LoadMeasurementModes();
            LoadResolutionValues();
        }

        private void LoadDmmDeviceNames()
        {
            ModularInstrumentsSystem modularInstrumentsSystem = new ModularInstrumentsSystem("NI-DMM");
            foreach (DeviceInfo device in modularInstrumentsSystem.DeviceCollection)
            {
                resourceNameComboBox.Items.Add(device.Name);
            }

            if (modularInstrumentsSystem.DeviceCollection.Count > 0)
            {
                resourceNameComboBox.SelectedIndex = 0;
            }
        }

        private void LoadPowerlineFrequencyValues()
        {
            double[] powerlineFrequencyValues = { 50, 60 };
            foreach (double powerLineFrequencyValue in powerlineFrequencyValues)
            {
                powerlineFrequencyValueComboBox.Items.Add(powerLineFrequencyValue);
            }

            powerlineFrequencyValueComboBox.SelectedIndex = 1;
        }

        private void LoadMeasurementModes()
        {
            measurementModeComboBox.Items.AddRange(Enum.GetNames(typeof(DmmMeasurementFunction)));
            measurementModeComboBox.Items.Remove(DmmMeasurementFunction.WaveformCurrent.ToString());
            measurementModeComboBox.Items.Remove(DmmMeasurementFunction.WaveformVoltage.ToString());
            measurementModeComboBox.SelectedIndex = 0;
        }

        private void LoadResolutionValues()
        {
            double[] resolutionValues = { 3.5, 4.5, 5.5, 6.5, 7.5 };
            foreach (double resolutionValue in resolutionValues)
            {
                resolutionValueComboBox.Items.Add(resolutionValue);
            }

            resolutionValueComboBox.SelectedIndex = 3;
        }

        private void EnableControls(bool enabled)
        {
            resolutionValueComboBox.Enabled = enabled;
            measurementModeComboBox.Enabled = enabled;
            resourceNameComboBox.Enabled = enabled;
            rangeNumericUpDown.Enabled = enabled;
            readButton.Enabled = enabled;
            powerlineFrequencyValueComboBox.Enabled = enabled;
        }

        private void Configure(NIDmm dmmSession)
        {
            double range = (double)rangeNumericUpDown.Value;
            double resolution = double.Parse(resolutionValueComboBox.Text);
            double powerlineFrequency = double.Parse(powerlineFrequencyValueComboBox.Text);

            //Get the Measurement Mode from the UI
            DmmMeasurementFunction measurementMode;
            if (!Enum.TryParse(measurementModeComboBox.Text, out measurementMode))
            {
                measurementMode = DmmMeasurementFunction.ACVolts;
            }

            dmmSession.ConfigureMeasurementDigits(measurementMode, range, resolution);
            dmmSession.Advanced.PowerlineFrequency = powerlineFrequency;
        }

        private void UpdateActualRange(NIDmm dmmSession)
        {
            double actualRange = dmmSession.AutoRangeValue;
            actualRangTextBox.Text = actualRange.ToString();
        }

        private void readButton_Click(object sender, EventArgs e)
        {
            EnableControls(false);
            messageTextBox.Clear();
            Application.DoEvents();

            try
            {
                using (NIDmm dmmSession = new NIDmm(resourceNameComboBox.Text, true, true))
                {
                    // Configure the session
                    Configure(dmmSession);
                    // Obtain the reading
                    double reading = dmmSession.Measurement.Read();
                    // Update the actual range
                    UpdateActualRange(dmmSession);
                    // Display the reading
                    measurementTextBox.Text = reading.ToString();
                    messageTextBox.Text = "Operation completed successfully.";
                }
            }
            // Handle driver-specific exceptions before the general exception handler that follows.
            // An example of a driver-specific exception is Ivi.Driver.IviCDriverException.
            catch (Exception exception)
            {
                // Error handling here
                messageTextBox.Text = exception.Message;
            }

            Application.DoEvents();
            EnableControls(true);
        }

        void ExportConfiguration(string path)
        {
            try
            {
                using (NIDmm dmmSession = new NIDmm(resourceNameComboBox.Text, true, true))
                {
                    // Configure the session
                    Configure(dmmSession);

                    // Export the configuration to the specified file
                    dmmSession.DriverUtility.ExportAttributeConfigurationFile(path);

                    messageTextBox.Text = "Export operation completed successfully.";
                }
            }
            // Handle driver-specific exceptions before the general exception handler that follows.
            // An example of a driver-specific exception is Ivi.Driver.IviCDriverException.
            catch (Exception exception)
            {
                // Error handling here
                messageTextBox.Text = exception.Message;
            }
        }

        void ImportConfiguration(string path)
        {
            try
            {
                using (NIDmm dmmSession = new NIDmm(resourceNameComboBox.Text, true, true))
                {
                    // Import the configuration from the specified file
                    dmmSession.DriverUtility.ImportAttributeConfigurationFile(path);

                    // Display the imported configuration on the UI
                    measurementModeComboBox.Text = dmmSession.MeasurementFunction.ToString();
                    rangeNumericUpDown.Value = (decimal)dmmSession.Range;
                    resolutionValueComboBox.Text = dmmSession.DigitsResolution.ToString();
                    powerlineFrequencyValueComboBox.Text = dmmSession.Advanced.PowerlineFrequency.ToString();

                    messageTextBox.Text = "Import operation completed successfully.";
                }
            }
            // Handle driver-specific exceptions before the general exception handler that follows.
            // An example of a driver-specific exception is Ivi.Driver.IviCDriverException.
            catch (Exception exception)
            {
                // Error handling here
                messageTextBox.Text = exception.Message;
            }
        }

        void Export_Click(object sender, EventArgs e)
        {
            EnableControls(false);
            messageTextBox.Clear();
            Application.DoEvents();

            OpenFileDialog fileDialog = GetConfigurationFileDialog();
            fileDialog.Title = "Select filename to export configuration to...";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                ExportConfiguration(fileDialog.FileName);
            }

            Application.DoEvents();
            EnableControls(true);
        }

        void Import_Click(object sender, EventArgs e)
        {
            EnableControls(false);
            messageTextBox.Clear();
            Application.DoEvents();

            OpenFileDialog fileDialog = GetConfigurationFileDialog();
            fileDialog.Title = "Select filename to import configuration from...";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                ImportConfiguration(fileDialog.FileName);
            }

            Application.DoEvents();
            EnableControls(true);
        }

        OpenFileDialog GetConfigurationFileDialog()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            string userDir = Environment.GetEnvironmentVariable("userprofile");

            fileDialog.InitialDirectory = Path.Combine(userDir, "Desktop");
            fileDialog.Filter = "NI-DMM configuration files (*.nidmmconfig)|*.nidmmconfig|All files (*.*)|*.*";
            fileDialog.FilterIndex = 1;
            fileDialog.RestoreDirectory = true;
            fileDialog.CheckFileExists = false;

            return fileDialog;
        }
   }
}