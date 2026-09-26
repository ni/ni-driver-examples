//==================================================================================================
// Title        : Simple Script
// Description  : This example demonstrates how to use scripts to dictate the behavior of the 
//			waveform generation. 
//
//			Note: In order to run this example, the upconverter must be configured with 
//			an Arbitrary Waveform Generator. 
//			To do this, open Measurement & Automation Explorer, select the upconverter 
//			and click on properties.
//==================================================================================================

using System;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.SimpleScript
{
    public partial class MainForm : Form
    {
        NIRfsg _rfsgSession;
        const double ArbPreFilterGain = -2;
        public MainForm()
        {
            InitializeComponent();

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

        #region UI Initial Value Config Section

        #endregion

        #region Program Functions

        void StartGeneration()
        {
            int kNumberOfSamples = 1000000;
            double kSamplesPerCycle = 10000.0;
            string resourceName;
            double frequency;
            double frequencyOffset;
            double power;
            double iqRate;
            RfsgRFPowerLevelType powerLevelType;
            string script;
            double actualIQRate;
            double[] iData;
            double[] qData;
            try
            {
                // Read in all of the control values 
                resourceName = resourceNameComboBox.Text;
                frequency = (double)frequencyNumeric.Value;
                power = (double)powerLevelNumeric.Value;
                iqRate = (double)iqRateNumeric.Value;
                script = scriptTextBox.Text;
                powerLevelType = RfsgRFPowerLevelType.PeakPower;

                iData = SinePattern(kNumberOfSamples, 1.0, 0.0, kSamplesPerCycle);
                qData = SinePattern(kNumberOfSamples, 1.0, 90.0, kSamplesPerCycle);

                errorTextBox.Text = "No error.";
                Application.DoEvents();

                // Initialize the NIRfsg session
                _rfsgSession = new NIRfsg(resourceName, true, false);

                // Subscribe to Rfsg warnings
                _rfsgSession.DriverOperation.Warning += new EventHandler<RfsgWarningEventArgs>(DriverOperation_Warning);

                // Configure the power level type 
                _rfsgSession.RF.PowerLevelType = powerLevelType;

                // Configure the instrument 
                _rfsgSession.RF.Configure(frequency, power);

                // Configure the generation mode to Script 
                _rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script;

                // Configure the IQ rate of the waveforms 
                _rfsgSession.Arb.IQRate = iqRate;

                // Configure Pre-filter Gain to avoid overflow due to phase-
                //  discontinuous signals 
                _rfsgSession.Arb.PreFilterGain = ArbPreFilterGain;

                // Get the actual IQ rate, and populate the GUI output 
                actualIQRate = _rfsgSession.Arb.IQRate;

                frequencyOffset = actualIQRate / (kNumberOfSamples / kSamplesPerCycle);

                actualIQRateTextBox.Text = actualIQRate.ToString();
                actualFrequencyOffsetTextBox.Text = frequencyOffset.ToString();

                // Configure the signal bandwidth to twice the maximum frequency
                //  deviation of all the waveforms in the script. 
                _rfsgSession.Arb.SignalBandwidth = 2 * frequencyOffset;

                // Write the two waveforms 
                _rfsgSession.Arb.WriteWaveform("negativeOffset", iData, qData);
                _rfsgSession.Arb.WriteWaveform("positiveOffset", qData, iData);

                // Write the script 
                _rfsgSession.Arb.Scripting.WriteScript(script);

                // Initiate Generation 
                _rfsgSession.Initiate();

                // Start the status checking timer 
                EnableControls(false);
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
            resourceNameComboBox.Enabled = enabled;
            scriptTextBox.Enabled = enabled;
            frequencyNumeric.Enabled = enabled;
            powerLevelNumeric.Enabled = enabled;
            iqRateNumeric.Enabled = enabled;
            rfsgStatusTimer.Enabled = !enabled;

            Application.DoEvents();
        }
    }
}
