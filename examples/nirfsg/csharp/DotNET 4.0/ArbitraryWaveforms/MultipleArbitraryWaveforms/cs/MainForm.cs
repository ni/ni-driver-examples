//==================================================================================================
// Title        : Multiple Arbitrary Waveforms
// Description  : This example demonstrates how to create and write multiple 
//			arbitrary waveforms in Arb Waveform mode.  This technique 
//			allows you to switch between waveforms without incurring the 
//			overhead of writing them.  The example creates and writes 
//			three waveforms and lets you select a waveform for generation. 
//			The three waveforms are: 
//
//			   + Double Side Band - two tones around the center frequency 
//			   + Lower Side Band  - one tone left of the center frequency 
//			   + Upper Side Band  - one tone right of the center frequency 
//
//			Each waveform is 100 samples long.  Since the sample clock 
//			rate is 100 MS/s, the frequency of the waveforms is 1 MHz; if 
//			viewed on a spectrum analyzer, the waveform peak(s) will be 
//			1 MHz away from the specified center frequency. 
//
//			Note: In order to run this example, the upconverter must be configured with 
//			an Arbitrary Waveform Generator. 
//			To do this, open Measurement & Automation Explorer, select the upconverter 
//			and click on properties.
//==================================================================================================

using System;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.MultipleArbitraryWaveforms
{
    public partial class MainForm : Form
    {
        NIRfsg _rfsgSession;
        const string DoubleSideband = "DoubleSideBand";
        const string LowerSideband = "LowerSideBand";
        const string UpperSideband = "UpperSideBand";

        public MainForm()
        {
            InitializeComponent();

            // Enable controls on startup
            EnableControls(true);

            LoadRfsgDeviceNames();

            ConfigureWaveformComboBox();
        }

        private void LoadRfsgDeviceNames()
        {
            ModularInstrumentsSystem modularInstrumentsSystem = new ModularInstrumentsSystem("NI-Rfsg");
            foreach (DeviceInfo device in modularInstrumentsSystem.DeviceCollection)
                resourceNameComboBox.Items.Add(device.Name);
            if (modularInstrumentsSystem.DeviceCollection.Count > 0)
                resourceNameComboBox.SelectedIndex = 0;
        }

        #region UI Initial Value Config Section

        private void ConfigureWaveformComboBox()
        {
            waveformComboBox.Items.AddRange(new string[] { DoubleSideband, LowerSideband, UpperSideband });
            waveformComboBox.SelectedIndex = 0;
        }

        #endregion

        #region Program Functions

        void StartGeneration()
        {
            string resourceName;
            int numberOfSamples = 100;
            double frequency;
            double frequencyOffset;
            double power;
            double iqRate;
            double actualIQRate;
            int waveform;
            double[] data, data90;
            try
            {
                // Read in all of the control values 
                resourceName = resourceNameComboBox.Text;
                frequency = (double)frequencyNumeric.Value;
                power = (double)powerLevelNumeric.Value;
                waveform = waveformComboBox.SelectedIndex;
                iqRate = (double)iqRateNumeric.Value;

                data = SinePattern(numberOfSamples, 1.0, 0.0, 1.0);
                data90 = SinePattern(numberOfSamples, 1.0, 90.0, 1.0);

                errorTextBox.Text = "No error.";
                Application.DoEvents();

                // Initialize the NIRfsg session
                _rfsgSession = new NIRfsg(resourceName, true, false);

                // Subscribe to Rfsg warnings
                _rfsgSession.DriverOperation.Warning += new EventHandler<RfsgWarningEventArgs>(DriverOperation_Warning);

                // Configure the instrument 
                _rfsgSession.RF.Configure(frequency, power);
                _rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.ArbitraryWaveform;
                _rfsgSession.Arb.IQRate = iqRate;

                // Get the actual IQ rate, and populate the GUI output 
                actualIQRate = _rfsgSession.Arb.IQRate;

                frequencyOffset = actualIQRate / numberOfSamples;
                actualIQRateTextBox.Text = actualIQRate.ToString();
                actualFrequencyOffsetTextBox.Text = frequencyOffset.ToString();

                // Configure the signal bandwidth to twice the baseband signal's
                //  maximum frequency deviation from 0 Hz. 
                _rfsgSession.Arb.SignalBandwidth = 2 * frequencyOffset;

                // Select the requested waveform by string name 
                switch (waveform)
                {
                    case 0:  // DoubleSideBand 
                        _rfsgSession.Arb.SelectedWaveform = DoubleSideband;
                        break;

                    case 1:  // LowerSideBand 
                        _rfsgSession.Arb.SelectedWaveform = LowerSideband;
                        break;

                    case 2:  // UpperSideBand 
                        _rfsgSession.Arb.SelectedWaveform = UpperSideband;
                        break;
                }

                // Configure three waveforms 
                _rfsgSession.Arb.WriteWaveform(DoubleSideband, data, data);
                _rfsgSession.Arb.WriteWaveform(LowerSideband, data, data90);
                _rfsgSession.Arb.WriteWaveform(UpperSideband, data90, data);

                // Initiate Generation 
                _rfsgSession.Initiate();

                // Start the status checking timer 
                EnableControls(false);

                // Activate stop and update buttons 
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
        
        static double[] SinePattern(int numberOfSamples, double amplitude, double phaseDegrees, double numberOfCycles)
        {
            double[] sineArray = new double[numberOfSamples];
            for (int i = 0; i < numberOfSamples; i++)
            {
                sineArray[i] = amplitude * Math.Sin(2 * Math.PI * i * numberOfCycles / numberOfSamples + Math.PI * phaseDegrees / 180);
            }
            return sineArray;
        }

        void CheckGeneration()
        {
            try
            {
                // Check the status of the RFSG 
                _rfsgSession.CheckGenerationStatus();
            }
            catch (Exception ex)
            {
                ShowError("CheckGeneration()", ex);
            }
        }

        void UpdateGeneration()
        {
            int waveform;
            try
            {
                // Stop the status checking timer 
                EnableControls(true);

                // Read in the waveform selection 
                waveform = waveformComboBox.SelectedIndex;

                // Abort generation 
                _rfsgSession.Abort();

                // Select the requested waveform by string name 
                switch (waveform)
                {
                    case 0:  // DoubleSideBand 
                        _rfsgSession.Arb.SelectedWaveform = DoubleSideband;
                        break;

                    case 1:  // LowerSideBand 
                        _rfsgSession.Arb.SelectedWaveform = LowerSideband;
                        break;

                    case 2:  // UpperSideBand 
                        _rfsgSession.Arb.SelectedWaveform = UpperSideband;
                        break;
                }

                // Initiate Generation 
                _rfsgSession.Initiate();

                // Start the status checking timer 
                EnableControls(false);
            }
            catch (Exception ex)
            {
                ShowError("UpdateGeneration()", ex);
            }
        }

        void StopGeneration()
        {
            // Stop the status checking timer 
            EnableControls(true);

            try
            {
                if (_rfsgSession != null)
                {
                    // Disable the output.  This sets the noise floor as low as possible.
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

        void ShowError(string functionName, Exception exception)
        {
            StopGeneration();
            errorTextBox.Text = "Error in " + functionName + ": " + exception.Message;
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

        private void updateButton_Click(object sender, System.EventArgs e)
        {
            UpdateGeneration();
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
            updateButton.Enabled = !enabled;
            resourceNameComboBox.Enabled = enabled;
            frequencyNumeric.Enabled = enabled;
            powerLevelNumeric.Enabled = enabled;
            iqRateNumeric.Enabled = enabled;
            rfsgStatusTimer.Enabled = !enabled;

            Application.DoEvents();
        }
    }
}
