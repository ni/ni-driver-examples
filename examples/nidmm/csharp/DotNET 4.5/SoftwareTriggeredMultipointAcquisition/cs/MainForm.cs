
//==================================================================================================
// Title        : Software Triggered Multipoint Acquisition
// Copyright    : National Instruments 2011. All Rights Reserved.
// Description  : This application demonstrates how to configure a DMM for a triggered
//                multipoint acquisition, send a software trigger and fetch the acquired
//		          samples using the .NET class library.
//===================================================================================================

using System;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIDmm;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.SoftwareTriggeredMultiPointAquisition
{
    public partial class MainForm : Form
    {
        NIDmm sampleDmmSession;
        bool softwareTriggerButtonClicked = false;
        bool mainFormClosed = false;
        int sampleCount;

        public MainForm()
        {
            InitializeComponent();
            LoadDmmDeviceNames();
            LoadPowerlineFrequencyValues();
            LoadMeasurementModes();
            LoadResolutionValues();
            LoadTriggerSourceOptions();
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

        private void LoadTriggerSourceOptions()
        {
            string[] triggerSourceValues = { "Immediate", "External", "Software Trigger", "Ttl0", "Ttl1", "Ttl2", "Ttl3", "Ttl4", "Ttl5", "Ttl6", "Ttl7", "PxiStar", "LbrTrig1", "AuxTrig1" };
            triggerSourceComboBox.Items.AddRange(triggerSourceValues);
            triggerSourceComboBox.SelectedIndex = 2;
        }

        private void UpdateActualRange(NIDmm sampleDmmSession)
        {
            double actualRange = sampleDmmSession.Range;
            actualRangeTextBox.Text = String.Format("{0:0.000}", actualRange);
        }

        private void EnableControls(bool enabled)
        {
            resolutionValueComboBox.Enabled = enabled;
            measurementModeComboBox.Enabled = enabled;
            resourceNameComboBox.Enabled = enabled;
            rangeNumericUpDown.Enabled = enabled;
            powerlineFrequencyValueComboBox.Enabled = enabled;
            clearButton.Enabled = enabled;
            triggerDelayNumericUpDown.Enabled = enabled;
            triggerSourceComboBox.Enabled = enabled;
            numberOfMeasurementsNumericUpDown.Enabled = enabled;
            readButton.Enabled = enabled;
        }

        private void UpdateMeasurementDisplay(params double[] readingArray)
        {
            int point = readingDataGridView.Rows.Count;
            foreach (double reading in readingArray)
            {
                point++;
                readingDataGridView.Rows.Add(point, reading);
            }
        }

        private void Configure()
        {
            DmmMeasurementFunction measurementMode = (DmmMeasurementFunction)Enum.Parse(typeof(DmmMeasurementFunction), measurementModeComboBox.Text);
            DmmTriggerSource triggerSource = triggerSourceComboBox.Text;    // Using implicit converter of DmmTriggerSource to assign string to DmmTriggerSource
            double range = (double)rangeNumericUpDown.Value;
            double resolution = double.Parse(resolutionValueComboBox.Text);
            double powerlineFrequency = double.Parse(powerlineFrequencyValueComboBox.Text);
            double triggerDelay = (double)triggerDelayNumericUpDown.Value;
            sampleCount = (int)numberOfMeasurementsNumericUpDown.Value;
            // Configure Dmm session Measurement parameters
            sampleDmmSession.ConfigureMeasurementDigits(measurementMode, range, resolution);
            sampleDmmSession.Advanced.PowerlineFrequency = powerlineFrequency;
            // Configure Trigger
            sampleDmmSession.Trigger.Configure(triggerSource, PrecisionTimeSpan.FromSeconds(triggerDelay));
            sampleDmmSession.Trigger.MultiPoint.SampleCount = sampleCount;
        }

        private void readButton_Click(object sender, EventArgs e)
        {
            EnableControls(false);
            messageTextBox.Clear();
            Application.DoEvents();
            double[] readingsArray;
            try
            {
                // Create a Dmm Session
                sampleDmmSession = new NIDmm(resourceNameComboBox.Text, true, true);
                Configure();
                // Initiate Aquisition
                sampleDmmSession.Measurement.Initiate();
                UpdateActualRange(sampleDmmSession);
                if (sampleDmmSession.Trigger.Source.Equals(DmmTriggerSource.SoftwareTrigger))
                {
                    softwareTriggerButton.Enabled = true;
                    // Poll
                    while (!softwareTriggerButtonClicked && !mainFormClosed)
                        Application.DoEvents();
                    softwareTriggerButtonClicked = false;
                    softwareTriggerButton.Enabled = false;

                    // Send software trigger
                    sampleDmmSession.Measurement.SendSoftwareTrigger();
                }
                messageTextBox.Text = "Acquisition in progress...";
                Application.DoEvents();
                readingsArray = sampleDmmSession.Measurement.FetchMultiPoint(sampleCount);
                UpdateMeasurementDisplay(readingsArray);
                messageTextBox.Text = "Operation completed successfully.";
            }
            catch (Exception exception)
            {
                messageTextBox.Text = exception.Message;
            }
            finally
            {
                if (sampleDmmSession != null)
                {
                    sampleDmmSession.Close();
                }
                Application.DoEvents();
                EnableControls(true);
            }
        }

        private void softwareTriggerButton_Click(object sender, EventArgs e)
        {
            softwareTriggerButtonClicked = true;
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            readingDataGridView.Rows.Clear();
        }

        private void mainFormForm_Closing(object sender, FormClosingEventArgs e)
        {
            mainFormClosed = true;
        }
    }
}