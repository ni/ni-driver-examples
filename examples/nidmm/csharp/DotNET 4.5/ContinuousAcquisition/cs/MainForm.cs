
//==================================================================================================
// Title        : Continuous Acquisition
// Copyright    : National Instruments 2011. All Rights Reserved.
// Description  : The application demonstrates how to configure the DMM for multipoint acquisition
//                and take multipoint readings using the .NET class library.
//===================================================================================================

using System;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIDmm;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.ContinuousAcquisition
{
    public partial class MainForm : Form
    {
        NIDmm sampleDmmSession;
        private Boolean modeACEnabled = false;
        private const int MaxSamplesPerReading = 100000;
        private int samplesPerReading;
        private int totalNumberOfSamples = 0;
        private double averageReading, minReading = Double.PositiveInfinity, maxReading = Double.NegativeInfinity;
        private bool stopped;
        private double[] reading = new double[MaxSamplesPerReading];

        public MainForm()
        {
            stopped = false;
            InitializeComponent();
            LoadDmmDeviceNames();
            LoadPowerlineFrequencyValues();
            LoadMeasurementModes();
            LoadResolutionValues();
        }

        private void ResetValues()
        {
            minReading = Double.PositiveInfinity;
            maxReading = Double.NegativeInfinity;
            averageReading = 0;
            totalNumberOfSamples = 0;
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
            rangeTextBox.Enabled = enabled;
            samplesPerReadingNumericUpDown.Enabled = enabled;
            powerlineFrequencyValueComboBox.Enabled = enabled;
            acquireButton.Enabled = enabled;
            stopButton.Enabled = !enabled;
            if (modeACEnabled == true)
                acConfigurationGroupBox.Enabled = enabled;
        }

        private void UpdateActualRange()
        {
            double actualRange = sampleDmmSession.Range;
            actualRangeTextBox.Text = String.Format("{0:G5}", actualRange);
        }

        private void UpdateMeasurementDisplay(double[] reading, int numberOfMeasurements)
        {
            double sum = 0;
            for (int i = 0; i < numberOfMeasurements; i++)
            {
                minReading = (reading[i] < minReading) ? reading[i] : minReading;
                maxReading = (reading[i] > maxReading) ? reading[i] : maxReading;
                sum += reading[i];
            }
            averageReading = (sum + averageReading) / (numberOfMeasurements + 1);
            averageReadingTextBox.Text = String.Format("{0:G8}", averageReading);
            minReadingTextBox.Text = String.Format("{0:G8}", minReading);
            maxReadingTextBox.Text = String.Format("{0:G8}", maxReading);
            numberOfSamplesTextBox.Text = totalNumberOfSamples.ToString();
            UpdateActualRange();
        }

        private void ClearMeasurementDisplay()
        {
            minReadingTextBox.Clear();
            maxReadingTextBox.Clear();
            averageReadingTextBox.Clear();
            numberOfSamplesTextBox.Clear();
            actualRangeTextBox.Clear();
        }

        private void TakeMeasurement()
        {
            sampleDmmSession.Trigger.MultiPoint.SampleCount = samplesPerReading;
            sampleDmmSession.Measurement.MemoryOptimizedReadMultiPointAsync(samplesPerReading, reading, null);
        }

        private void Configure()
        {
            DmmMeasurementFunction measurementMode = (DmmMeasurementFunction)Enum.Parse(typeof(DmmMeasurementFunction), measurementModeComboBox.Text);
            double range = double.Parse(rangeTextBox.Text);
            double resolution = double.Parse(resolutionValueComboBox.Text);
            samplesPerReading = int.Parse(samplesPerReadingNumericUpDown.Text);
            double powerlineFrequency = double.Parse(powerlineFrequencyValueComboBox.Text);
            //Configure Dmm session Measurement parameters
            sampleDmmSession.ConfigureMeasurementDigits(measurementMode, range, resolution);
            //Configure Powerline Frequency
            sampleDmmSession.Advanced.PowerlineFrequency = powerlineFrequency;
            //Configure minimum and maximum AC frequency
            if (modeACEnabled == true)
            {
                sampleDmmSession.AC.FrequencyMin = double.Parse(minACFrequencyTextBox.Text);
                sampleDmmSession.AC.FrequencyMax = double.Parse(maxACFrequencyTextBox.Text);
            }
        }

        private void acquireButton_Click(object sender, EventArgs e)
        {
            stopped = false;
            EnableControls(false);
            ResetValues();
            messageTextBox.Clear();
            ClearMeasurementDisplay();
            Application.DoEvents();
            try
            {
                sampleDmmSession = new NIDmm(resourceNameComboBox.Text, true, true);
                sampleDmmSession.Measurement.ReadMultiPointCompleted += new EventHandler<DmmMeasurementEventArgs<double[]>>(Measurement_ReadMultipointCompleted);
                Configure();
                messageTextBox.Text = "Acquisition in progress...";
                TakeMeasurement();
            }
            catch (Exception exception)
            {
                DisplayMessageAndCloseSession(exception.Message);
            }
        }

        void Measurement_ReadMultipointCompleted(object sender, DmmMeasurementEventArgs<double[]> e)
        {
            if (e.Error != null)
            {
                DisplayMessageAndCloseSession(e.Error.Message);
                return;
            }
            totalNumberOfSamples += e.ActualNumberOfPoints;
            UpdateMeasurementDisplay(e.Reading, e.ActualNumberOfPoints);

            if (!stopped)
            {
                // Continue Acquisition
                TakeMeasurement();
            }
            else
            {
                DisplayMessageAndCloseSession("Finished Acquisition.");
            }
        }

        private void DisplayMessageAndCloseSession(string messageText)
        {
            if (sampleDmmSession != null && !sampleDmmSession.IsDisposed)
            {
                sampleDmmSession.Measurement.ReadMultiPointCompleted -= Measurement_ReadMultipointCompleted;
                sampleDmmSession.Close();
            }
            messageTextBox.Text = messageText;
            EnableControls(true);
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            stopped = true;
            messageTextBox.Text = "Waiting for completion of acquisition...";
            EnableControls(false);
            stopButton.Enabled = false;
            Application.DoEvents();
        }

        private void measurementModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (measurementModeComboBox.Text.Contains("AC"))
            {
                acConfigurationGroupBox.Enabled = true;
                modeACEnabled = true;
            }
            else
            {
                acConfigurationGroupBox.Enabled = false;
                modeACEnabled = false;
            }
        }
    }
}