using System;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using NationalInstruments.ModularInstruments;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.FiniteGenerationIQDevice
{
    public partial class MainForm : Form
    {
        NIRfsg _rfsgSession;
        const double frequencyReferenceRate = 10E6;

        public MainForm()
        {
            InitializeComponent();
            ConfigureRefClockComboBox();
            LoadRfsgDeviceNames();
        }

        private void LoadRfsgDeviceNames()
        {
            ModularInstrumentsSystem modularInstrumentsSystem = new ModularInstrumentsSystem("NI-Rfsg");
            foreach (DeviceInfo device in modularInstrumentsSystem.DeviceCollection)
                resourceNameComboBox.Items.Add(device.Name);
            if (modularInstrumentsSystem.DeviceCollection.Count > 0)
                resourceNameComboBox.SelectedIndex = 0;
        }

        private void ConfigureRefClockComboBox()
        {
            List<KeyValuePair<string, RfsgFrequencyReferenceSource>> refClockValueList = new List<KeyValuePair<string, RfsgFrequencyReferenceSource>>();
            refClockValueList.Add(new KeyValuePair<string, RfsgFrequencyReferenceSource>("OnboardClock", RfsgFrequencyReferenceSource.OnboardClock));
            refClockValueList.Add(new KeyValuePair<string, RfsgFrequencyReferenceSource>("RefIn", RfsgFrequencyReferenceSource.ReferenceIn));
            refClockValueList.Add(new KeyValuePair<string, RfsgFrequencyReferenceSource>("PXI_Clk", RfsgFrequencyReferenceSource.PxiClock));
            refClockValueList.Add(new KeyValuePair<string, RfsgFrequencyReferenceSource>("ClkIn", RfsgFrequencyReferenceSource.ClockIn));
            referenceClockComboBox.DisplayMember = "Key";
            referenceClockComboBox.ValueMember = "Value";
            referenceClockComboBox.DataSource = refClockValueList;
            referenceClockComboBox.SelectedIndex = 0;
        }

        #region UI Initial Value Config Section


        #endregion

        #region Program Functions

        void StartGeneration()
        {
            string resourceName;
            RfsgFrequencyReferenceSource referenceClockSource;
            double iqPortFrequency;
            double iqOutPortLevel;
            double iqRate;
            bool isWaveformRepeatCountFinite;
            int waveformRepeatCount;
            double signalBandwidth;
            int numberOfSamples;
            int waveformItr;
            double actualIQRate;
            int quantum;
            RfsgRFPowerLevelType powerLevelType;
            float[] iData, qData;
            try
            {
                // Read in all of the control values 
                resourceName = resourceNameComboBox.Text;
                referenceClockSource = referenceClockComboBox.Text;
                iqPortFrequency = (double)iqPortFrequencyNumeric.Value;
                iqOutPortLevel = (double)iqOutPortLevelNumeric.Value;
                iqRate = 50e6;
                waveformRepeatCount = (int)waveformRepeatCountNumeric.Value;
                isWaveformRepeatCountFinite = true;
                powerLevelType = RfsgRFPowerLevelType.PeakPower;

                errorTextBox.Text = "No error.";
                Application.DoEvents();

                // Initialize the NIRfsg session
                _rfsgSession = new NIRfsg(resourceName, true, false);

                // Subscribe to Rfsg warnings
                _rfsgSession.DriverOperation.Warning += new EventHandler<RfsgWarningEventArgs>(DriverOperation_Warning);

                // Configure Clock Source
                _rfsgSession.FrequencyReference.Configure(referenceClockSource, frequencyReferenceRate);

                // Configure IQOutPort CarrierFrequency, OutputPort and Level (Vpp)
                _rfsgSession.IQOutPort.CarrierFrequency = iqPortFrequency;
                _rfsgSession.Arb.OutputPort = RfsgOutputPort.IQOut;
                _rfsgSession.IQOutPort[""].Level = iqOutPortLevel;

                // Configure the generation mode
                _rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.ArbitraryWaveform;

                // Configure the power level type 
                _rfsgSession.RF.PowerLevelType = powerLevelType;

                // Configure the IQ rate of the waveforms 
                _rfsgSession.Arb.IQRate = iqRate;
                _rfsgSession.Arb.IsWaveformRepeatCountFinite = isWaveformRepeatCountFinite;
                _rfsgSession.Arb.WaveformRepeatCount = waveformRepeatCount;

                signalBandwidth = _rfsgSession.Arb.IQRate * 0.8;
                // Configure the signal bandwidth 
                _rfsgSession.Arb.SignalBandwidth = signalBandwidth;

                // Get the actual IQ rate
                actualIQRate = _rfsgSession.Arb.IQRate;

                // Generate and Write a DC signal to be upconverted 
                quantum = _rfsgSession.Arb.WaveformCapabilities.WaveformQuantum;
                numberOfSamples = RfsgCoerceToQuantum((int)(actualIQRate / 2), quantum);

                // Populate the GUI output for Actual Waveform Duration 
                waveformDurationTextBox.Text = (numberOfSamples / actualIQRate).ToString();

                iData = new float[numberOfSamples];
                qData = new float[numberOfSamples];
                for (waveformItr = 0; waveformItr < numberOfSamples; waveformItr++)
                {
                    iData[waveformItr] = 1.0F;
                    qData[waveformItr] = 0.0F;
                }

                _rfsgSession.Arb.WriteWaveform("waveform", iData, qData);

                // Initiate Generation 
                _rfsgSession.Initiate();

                // Start the status checking timer 
                EnableControls(false);

                // Activate stop button 
                stopButton.Focus();
            }
            catch (Exception ex)
            {
                ShowError("StartGeneration()", ex);
            }
        }

        void DriverOperation_Warning(object sender, RfsgWarningEventArgs e)
        {
            // Display the rfsg warning
            errorTextBox.Text = e.Message;
        }

        void StopGeneration()
        {
            // Stop the status checking timer 
            EnableControls(true);

            try
            {
                // Disable the output.  This sets the noise floor as low as possible.
                if (_rfsgSession != null)
                {
                    _rfsgSession.RF.OutputEnabled = false;

                    // Unsubscribe from warning events
                    _rfsgSession.DriverOperation.Warning -= DriverOperation_Warning;

                    // Close the RFSG NIRfsg session
                    _rfsgSession.Close();
                }
                _rfsgSession = null;
            }
            catch (Exception ex)
            {
                errorTextBox.Text = "Error in StopGeneration(): " + ex.Message;
            }
        }

        void CheckGeneration()
        {
            try
            {
                // Check the status of the RFSG 
                if (_rfsgSession.CheckGenerationStatus() == RfsgGenerationStatus.Complete)
                {
                    StopGeneration();
                }
            }
            catch (Exception ex)
            {
                ShowError("CheckGeneration()", ex);
            }
        }

        void ShowError(string functionName, Exception exception)
        {
            StopGeneration();
            errorTextBox.Text = "Error in " + functionName + ": " + exception.Message;
        }

        static int RfsgCoerceToQuantum(int numberOfSamples, int quantum)
        {
            double smallestNumberOfSamples;

            if (quantum <= 0)
            {
                return -1;
            }

            if (numberOfSamples >= quantum)
                smallestNumberOfSamples = numberOfSamples;
            else
                smallestNumberOfSamples = quantum;

            return (int)Math.Round(smallestNumberOfSamples / quantum) * quantum;
        }

        #endregion

        #region Form Events
        private void startButton_Click(object sender, System.EventArgs e)
        {
            StartGeneration();
        }

        private void stopButton_Click(object sender, System.EventArgs e)
        {
            StopGeneration();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopGeneration();
        }

        private void rfsgStatusTimer_Tick(object sender, System.EventArgs e)
        {
            CheckGeneration();
        }

        #endregion

        private void EnableControls(bool enabled)
        {
            startButton.Enabled = enabled;
            stopButton.Enabled = !enabled;
            resourceNameComboBox.Enabled = enabled;
            iqPortFrequencyNumeric.Enabled = enabled;
            iqOutPortLevelNumeric.Enabled = enabled;
            waveformRepeatCountNumeric.Enabled = enabled;
            rfsgStatusTimer.Enabled = !enabled;
            referenceClockComboBox.Enabled = enabled;

            Application.DoEvents();
        }
    }
}
