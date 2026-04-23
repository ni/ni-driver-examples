
//==================================================================================================
// Title        : Waveform Acquisition
// Copyright    : National Instruments 2011. All Rights Reserved.
// Description  : The application demonstrates how to configure a DMM for waveform acquisition, 
//                and Read waveform data using the .NET class library.
//===================================================================================================

using System;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIDmm;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;


namespace NationalInstruments.Examples.WaveformAcquisition
{
    public partial class MainForm : Form
    {
        NIDmm sampleDmmSession;

        int sampleCount;
        public MainForm()
        {
            InitializeComponent();
            LoadDmmDeviceNames();
            LoadMeasurementModes();
        }

        private void LoadDmmDeviceNames()
        {
            ModularInstrumentsSystem modularInstrumentsSystem = new ModularInstrumentsSystem("NI-DMM");
            foreach (DeviceInfo device in modularInstrumentsSystem.DeviceCollection)
                resourceNameComboBox.Items.Add(device.Name);
            if (modularInstrumentsSystem.DeviceCollection.Count > 0)
                resourceNameComboBox.SelectedIndex = 0;
        }

        private void LoadMeasurementModes()
        {
            acquisitionModeComboBox.Items.Add(DmmMeasurementFunction.WaveformVoltage);
            acquisitionModeComboBox.Items.Add(DmmMeasurementFunction.WaveformCurrent);
            acquisitionModeComboBox.SelectedIndex = 0;
        }

        private void UpdateActualRange(NIDmm sampleDmmSession)
        {
            double actualRange = sampleDmmSession.Range;
            actualRangeTextBox.Text = String.Format("{0:0.000}", actualRange);
        }

        private void EnableControls(bool enabled)
        {
            rateValueTextBox.Enabled = enabled;
            acquisitionModeComboBox.Enabled = enabled;
            resourceNameComboBox.Enabled = enabled;
            rangeNumericUpDown.Enabled = enabled;
            clearButton.Enabled = enabled;
            acquireButton.Enabled = enabled;
            numberOfSamplesNumericUpDown.Enabled = enabled;
        }

        private void UpdateMeasurementDisplay(AnalogWaveform<double> analogWaveform)
        {
            int point = readingDataGridView.Rows.Count;
            double[] readingsBuffer = analogWaveform.GetRawData();

            for (int i = 0; i < analogWaveform.SampleCount; i++)
            {
                point++;
                readingDataGridView.Rows.Add(point, readingsBuffer[i]);
            }
        }

        private void Configure()
        {
            //Get the Measurement Mode from the UI
            DmmMeasurementFunction measurementMode = (DmmMeasurementFunction)Enum.Parse(typeof(DmmMeasurementFunction), acquisitionModeComboBox.Text);
            double range = (double)rangeNumericUpDown.Value;
            double rate = double.Parse(rateValueTextBox.Text);
            sampleCount = (int)numberOfSamplesNumericUpDown.Value;
            //Configure Dmm session waveform acquisition parameters
            sampleDmmSession.ConfigureWaveformAcquisition(measurementMode, range, rate, sampleCount);
            double actualRange = sampleDmmSession.Range;
        }

        private void acquireButton_Click(object sender, EventArgs e)
        {
            EnableControls(false);
            messageTextBox.Text = "Acquiring data...";
            Application.DoEvents();
            AnalogWaveform<double> analogWaveform;
            try
            {
                sampleDmmSession = new NIDmm(resourceNameComboBox.Text, true, true);
                Configure();
                analogWaveform = sampleDmmSession.WaveformAcquisition.ReadWaveform(sampleCount, NationalInstruments.PrecisionTimeSpan.MaxValue);
                UpdateMeasurementDisplay(analogWaveform);
                messageTextBox.Text = "Operation completed successfully.";
                UpdateActualRange(sampleDmmSession);
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

        private void clearButton_Click(object sender, EventArgs e)
        {
            readingDataGridView.Rows.Clear();
        }
    }
}

