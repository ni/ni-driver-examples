//==================================================================================================
// Title        : Arbitrary Waveform Generation
// Description  : This example demonstrates how to generate an arbitrary waveform. The example 
//			lets you choose which waveform to create and download among three options: 
//
//			 + Double Side Band --> two tones around the center frequency. 
//			 + Lower Side Band  --> one tone to the left of the center frequency. 
//			 + Upper Side Band  --> one tone to the right of the center frequency. 
//
//			All waveforms have 4,000 samples, which are sampled at 100 MS/s by the device. 
//			Therefore, the frequency of waveforms generated is 25 kHz; if seen in a 
//			spectrum analyzer the power will show 25 kHz away from the specified center 
//			frequency. 
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

namespace NationalInstruments.Examples.ArbitraryWaveformGeneration
{
    public partial class MainForm : Form
    {
        NIRfsg _rfsgSession;

        public MainForm()
        {
            InitializeComponent();

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
            waveformComboBox.Items.AddRange(new string[] { "DoubleSideBand", "LowerSideBand", "UpperSideBand" });
            waveformComboBox.SelectedIndex = 0;
        }

        #endregion

        #region Program Functions

        void StartGeneration()
        {
            string resourceName;
            int kNumberOfSamples = 100;
            double frequency;
            double frequencyOffset;
            double power;
            int waveform;
            double iqRate;
            double actualIQRate;
            double[] iData;
            double[] qData;
            try
            {
                // Read in all of the control values 
                resourceName = resourceNameComboBox.Text;
                frequency = (double)frequencyNumeric.Value;
                power = (double)powerLevelNumeric.Value;
                waveform = waveformComboBox.SelectedIndex;
                iqRate = (double)iqRateNumeric.Value;

                iData = new double[kNumberOfSamples];
                qData = new double[kNumberOfSamples];

                switch (waveform)
                {
                    case 0:  // Double Side Band 
                        iData = SinePattern(kNumberOfSamples, 1.0, 0.0, 1.0);
                        qData = SinePattern(kNumberOfSamples, 1.0, 0.0, 1.0);
                        break;

                    case 1:  // Lower Side Band 
                        iData = SinePattern(kNumberOfSamples, 1.0, 0.0, 1.0);
                        qData = SinePattern(kNumberOfSamples, 1.0, 90.0, 1.0);
                        break;

                    case 2:  // Upper Side Band 
                        iData = SinePattern(kNumberOfSamples, 1.0, 90.0, 1.0);
                        qData = SinePattern(kNumberOfSamples, 1.0, 0.0, 1.0);
                        break;
                }

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
                frequencyOffset = actualIQRate / kNumberOfSamples;

                actualIQRateTextBox.Text = actualIQRate.ToString();
                actualFrequencyOffsetTextBox.Text = frequencyOffset.ToString();

                // Configure the signal bandwidth to twice the baseband signal's maximum frequency deviation from 0 Hz. 
                _rfsgSession.Arb.SignalBandwidth = 2 * frequencyOffset;

                // Write the arb waveform 
                _rfsgSession.Arb.WriteWaveform(string.Empty, iData, qData);

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

        static double[] SinePattern(int kNumberOfSamples, double amplitude, double phaseDegrees, double numberOfCycles)
        {
            double[] sineArray = new double[kNumberOfSamples];
            for (int i = 0; i < kNumberOfSamples; i++)
            {
                sineArray[i] = amplitude * Math.Sin(2 * Math.PI * i * numberOfCycles / kNumberOfSamples + Math.PI * phaseDegrees / 180);
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

            // Start the status checking timer
            rfsgStatusTimer.Enabled = !enabled;

            resourceNameComboBox.Enabled = enabled;
            frequencyNumeric.Enabled = enabled;
            powerLevelNumeric.Enabled = enabled;
            iqRateNumeric.Enabled = enabled;
            waveformComboBox.Enabled = enabled;

            Application.DoEvents();
        }
    }
}
