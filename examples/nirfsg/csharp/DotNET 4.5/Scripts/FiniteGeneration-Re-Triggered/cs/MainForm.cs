//==================================================================================================
// Title        : Finite Generation-Re-Triggered
// Description  : This example demonstrates how to generate a finite waveform repeatedly, based 
//			on a trigger. The waveform generated is a sine tone at the center frequency. 
//			If the device is a 5672, the script is different from other RFSG devices because 
//			the device generates a tone during the wait command. 
//
//			Note: In order to run this example, the upconverter must be configured with 
//			an Arbitrary Waveform Generator. 
//			To do this, open Measurement & Automation Explorer, select the upconverter 
//			and click on properties.
//==================================================================================================

using System;
using System.Drawing;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.FiniteGenerationReTriggered
{
    public partial class MainForm : Form
    {
        NIRfsg _rfsgSession;
        const double ArbPreFilterGain = -2;
        const double ArbSignalBandwidth = 1;

        public MainForm()
        {
            InitializeComponent();

            // Enable controls on startup
            EnableControls(true);

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

        #region Program Functions

        void StartGeneration()
        {
            string resourceName;
            double frequency;
            double power;
            double iqRate;
            double waveformDuration;
            double actualIQRate;
            int numberOfSamples;
            int waveformIterator;
            int quantum;
            double[] iData, qData, blankData;
            try
            {
                string modelType;
                string[] script = {
                  @"script triggerFiniteGeneration
                     repeat forever
                        wait until scriptTrigger0
                        generate triggeredWaveform
                     end repeat
                  end script",
     
                 @"script triggerFiniteGeneration
                     repeat forever
                        repeat until scriptTrigger0
                           generate allZeroes
                        end repeat
                        generate triggeredWaveform
                     end repeat
                  end script"};

                // Read in all of the control values 
                resourceName = resourceNameComboBox.Text;
                frequency = (double)frequencyNumeric.Value;
                power = (double)powerLevelNumeric.Value;
                iqRate = (double)iqRateNumeric.Value;
                waveformDuration = (double)durationNumeric.Value;

                errorTextBox.Text = "No error.";
                Application.DoEvents();

                // Initialize the NIRfsg session
                _rfsgSession = new NIRfsg(resourceName, true, false);

                // Subscribe to Rfsg warnings
                _rfsgSession.DriverOperation.Warning += new EventHandler<RfsgWarningEventArgs>(DriverOperation_Warning);

                // Configure the instrument 
                _rfsgSession.RF.Configure(frequency, power);

                // Configure the generation mode to Script 
                _rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script;

                // Configure scriptTrigger0 (refer to the script) 
                _rfsgSession.Triggers.ScriptTriggers[0].ConfigureSoftwareTrigger();

                // Configure the signal bandwidth 
                _rfsgSession.Arb.SignalBandwidth = ArbSignalBandwidth;

                // Configure the power level type 
                _rfsgSession.RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower;

                // Configure the IQ rate of the waveforms 
                _rfsgSession.Arb.IQRate = iqRate;

                // Get the actual IQ rate, and populate the GUI output 
                actualIQRate = _rfsgSession.Arb.IQRate;
                actualIQRateTextBox.Text = actualIQRate.ToString();

                // Generate and Write a DC signal to be upconverted 
                quantum = _rfsgSession.Arb.WaveformCapabilities.WaveformQuantum;
                numberOfSamples = RfsgCoerceToQuantum((int)(actualIQRate * waveformDuration), quantum);

                actualDurationTextBox.Text = (numberOfSamples / actualIQRate).ToString();
                iData = new double[numberOfSamples];
                qData = new double[numberOfSamples];
                for (waveformIterator = 0; waveformIterator < numberOfSamples; waveformIterator++)
                {
                    iData[waveformIterator] = 1.0;
                    qData[waveformIterator] = 0.0;
                }

                _rfsgSession.Arb.WriteWaveform("triggeredWaveform", iData, qData);

                // Get the model number to determine which script to run 
                modelType = _rfsgSession.Identity.InstrumentModel;

                if (modelType.Equals("NI PXIe-5672", StringComparison.OrdinalIgnoreCase))
                {
                    blankData = new double[12];
                    for (waveformIterator = 0; waveformIterator < 12; waveformIterator++)
                    {
                        blankData[waveformIterator] = 0.0;
                    }

                    _rfsgSession.Arb.PreFilterGain = ArbPreFilterGain;

                    _rfsgSession.Arb.WriteWaveform("allZeroes", blankData, blankData);

                    _rfsgSession.Arb.Scripting.WriteScript(script[1]);
                }
                else
                {
                    _rfsgSession.Arb.Scripting.WriteScript(script[0]);
                }

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

        void StopGeneration()
        {
            // Stop the status checking timer, and turn off the LED 
            EnableControls(true);
            readyLed.BackColor = SystemColors.Control;
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
                readyLed.BackColor = Color.Lime;

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

        void SendSoftwareScriptTrigger0()
        {
            try
            {
                // Send the software trigger 
                _rfsgSession.Triggers.ScriptTriggers[0].SendSoftwareEdgeTrigger();
            }
            catch (Exception ex)
            {
                ShowError("SendSoftwareScriptTrigger0()", ex);
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

        private void triggerButton_Click(object sender, System.EventArgs e)
        {
            SendSoftwareScriptTrigger0();
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
            triggerButton.Enabled = !enabled;
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
