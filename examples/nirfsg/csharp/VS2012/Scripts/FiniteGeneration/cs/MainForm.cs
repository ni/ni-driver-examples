//==================================================================================================
// Title        : Finite Generation
// Description  : This example demonstrates how to generate a finite waveform.
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

namespace NationalInstruments.Examples.FiniteGeneration
{
    public partial class MainForm : Form
    {
        NIRfsg _rfsgSession;
        const double ArbSignalBandwidth = 1;
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
            string resourceName;
            double frequency;
            double power;
            double iqRate;
            double waveformDuration;
            int numberOfSamples;
            int waveformItr;
            double actualIQRate;
            int quantum;
            RfsgRFPowerLevelType powerLevelType;
            double[] iData, qData;
            try
            {
                string script = @"script finiteGeneration
                                    generate waveform
                                end script";

                // Read in all of the control values 
                resourceName = resourceNameComboBox.Text;
                frequency = (double)frequencyNumeric.Value;
                power = (double)powerLevelNumeric.Value;
                iqRate = (double)iqRateNumeric.Value;
                waveformDuration = (double)durationNumeric.Value;
                powerLevelType = RfsgRFPowerLevelType.PeakPower;

                errorTextBox.Text = "No error.";
                Application.DoEvents();

                // Initialize the NIRfsg session
                _rfsgSession = new NIRfsg(resourceName, true, false);

                // Subscribe to Rfsg warnings
                _rfsgSession.DriverOperation.Warning += new EventHandler<RfsgWarningEventArgs>(DriverOperation_Warning);
                
                // Configure the instrument 
                _rfsgSession.RF.Configure(frequency, power);

                // Configure the power level type 
                _rfsgSession.RF.PowerLevelType = powerLevelType;

                // Configure the generation mode to Script 
                _rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script;

                // Configure the IQ rate of the waveforms 
                _rfsgSession.Arb.IQRate = iqRate;

                // Configure the signal bandwidth 
                _rfsgSession.Arb.SignalBandwidth = ArbSignalBandwidth;

                // Get the actual IQ rate, and populate the GUI output 
                actualIQRate = _rfsgSession.Arb.IQRate;
                actualIQRateTextBox.Text = actualIQRate.ToString();

                // Generate and Write a DC signal to be upconverted 
                quantum = _rfsgSession.Arb.WaveformCapabilities.WaveformQuantum;
                numberOfSamples = RfsgCoerceToQuantum((int)(actualIQRate * waveformDuration), quantum);

                // Populate the GUI output for Actual Waveform Duration 
                actualDurationTextBox.Text = (numberOfSamples / actualIQRate).ToString();

                iData = new double[numberOfSamples];
                qData = new double[numberOfSamples];
                for (waveformItr = 0; waveformItr < numberOfSamples; waveformItr++)
                {
                    iData[waveformItr] = 1.0;
                    qData[waveformItr] = 0.0;
                }

                _rfsgSession.Arb.WriteWaveform("waveform", iData, qData);

                // Write the script 
                _rfsgSession.Arb.Scripting.WriteScript(script);

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
            frequencyNumeric.Enabled = enabled;
            powerLevelNumeric.Enabled = enabled;
            iqRateNumeric.Enabled = enabled;
            durationNumeric.Enabled = enabled;
            rfsgStatusTimer.Enabled = !enabled;

            Application.DoEvents();
        }
    }
}
