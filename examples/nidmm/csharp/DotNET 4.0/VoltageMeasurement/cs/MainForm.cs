
//==================================================================================================
// Title        : Voltage Measurement
// Copyright    : National Instruments 2011. All Rights Reserved.
// Description  : The application demonstrates how to make measurements using the .NET class 
//                library. The application configures the DMM for DC Voltage measurement, 
//                acquires a reading and displays the aquired reading to the user.
//===================================================================================================

using System;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIDmm;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.VoltageMeasurement
{
    public partial class MainForm : Form
    {
        NIDmm sampleDmmSession;

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
                resourceNameComboBox.Items.Add(device.Name);
            if (modularInstrumentsSystem.DeviceCollection.Count > 0)
                resourceNameComboBox.SelectedIndex = 0;
        }

        private void LoadPowerlineFrequencyValues()
        {
            double[] powerlineFrequencyValues = { 50, 60 };
            for (int i = 0; i < powerlineFrequencyValues.Length; i++)
                powerlineFrequencyValueComboBox.Items.Add(powerlineFrequencyValues[i]);
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
            for (int i = 0; i < resolutionValues.Length; i++)
                resolutionValueComboBox.Items.Add(resolutionValues[i]);
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

        private void Configure()
        {
            double range = (double)rangeNumericUpDown.Value;
            double resolution = double.Parse(resolutionValueComboBox.Text);
            double powerlineFrequency = double.Parse(powerlineFrequencyValueComboBox.Text);
            //Get the Measurement Mode from the UI
            DmmMeasurementFunction measurementMode = DmmMeasurementFunction.ACVolts;
            measurementMode = (DmmMeasurementFunction)Enum.Parse(typeof(DmmMeasurementFunction), measurementModeComboBox.Text);
            //Configure Dmm session Measurement parameters
            sampleDmmSession.ConfigureMeasurementDigits(measurementMode, range, resolution);
            sampleDmmSession.Advanced.PowerlineFrequency = powerlineFrequency;
        }

        private void UpdateActualRange(NIDmm sampleDmmSession)
        {
            double actualRange = sampleDmmSession.AutoRangeValue;
            actualRangTextBox.Text = actualRange.ToString();
        }

        private void readButton_Click(object sender, EventArgs e)
        {
            EnableControls(false);
            messageTextBox.Clear();
            Application.DoEvents();
            try
            {
                double reading;
                // Create a Dmm Session
                sampleDmmSession = new NIDmm(resourceNameComboBox.Text, true, true);
                Configure();
                // Obtain the reading
                reading = sampleDmmSession.Measurement.Read();
                // Update the actual range
                UpdateActualRange(sampleDmmSession);
                // Display the reading
                measurementTextBox.Text = reading.ToString();
                messageTextBox.Text = "Operation completed successfully.";
            }
            catch (Exception exception)
            {
                messageTextBox.Text = exception.Message;
            }
            finally
            {
                if (sampleDmmSession != null)
                    sampleDmmSession.Close();
                Application.DoEvents();
                EnableControls(true);
            }
        }
    }
}