//==================================================================================================
// Title        : Script Trigger-Software Source
// Description  : This example demonstrates how to a generate signals based on a software script 
//			trigger. The behavior of the signals is dictated by a script. This example has 
//			two scripts.  The first alternates between two signals when the specified software 
//			script trigger is received. The second script shows nested triggering.  It waits 
//			for a trigger for it to start generating a waveform.  A separate trigger will 
//			generate a second waveform. 
//
//			Note: In order to run this example, the upconverter must be configured with 
//			an Arbitrary Waveform Generator. 
//			To do this, open Measurement & Automation Explorer, select the upconverter 
//			and click on properties.
//==================================================================================================

using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.ScriptTriggerSoftwareSource
{
    public partial class MainForm : Form
    {
        NIRfsg _rfsgSession;
        const double Amplitude = 1.0;
        const double NumberOfCycles = 1.0;
        const double ArbPreFilterGain = -2;
        const double IDataPhaseDegrees = 0.0;
        const double QDataPhaseDegrees = 90.0;
        string[] _scripts = {

                               "script triggersToggleWaveforms" + Environment.NewLine +
                               "   repeat forever" + Environment.NewLine +
                               "      repeat until scriptTrigger0" + Environment.NewLine +
                               "         generate negativeOffset" + Environment.NewLine +
                               "      end repeat" + Environment.NewLine +
                               "      repeat until scriptTrigger0" + Environment.NewLine +
                               "         generate positiveOffset" + Environment.NewLine +
                               "      end repeat " + Environment.NewLine +
                               "   end repeat " + Environment.NewLine +
                               "end script",

                               "script myScript" + Environment.NewLine +
                               "  Repeat forever" + Environment.NewLine +
                               "     Generate allZeros" + Environment.NewLine +   
                               "     Clear scriptTrigger0" + Environment.NewLine +
                               "     Wait until scriptTrigger0" + Environment.NewLine +
                               "     Clear scriptTrigger1 " + Environment.NewLine +
                               "    Repeat until scriptTrigger1" + Environment.NewLine +
                               "        Generate positiveOffset" + Environment.NewLine +     
                               "     end repeat" + Environment.NewLine +
                               "     Clear scriptTrigger1" + Environment.NewLine +
                               "     Repeat until scriptTrigger1" + Environment.NewLine +
                               "        Generate negativeOffset" + Environment.NewLine +
                               "     end repeat" + Environment.NewLine + 
                               "  end repeat" + Environment.NewLine +
                               "end script"
                           };

        public MainForm()
        {
            InitializeComponent();

            LoadRfsgDeviceNames();

            // force set the script
            scriptIndexNumeric.Value = 1;
            scriptIndexNumeric.Value = 0;

            EnableControls(true);
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
            int numberOfSamples = 100;
            int numberOfBlankSamples = 500;
            int scriptNumber;
            double frequency;
            double power;
            double iqRate;
            double actualIQRate;
            double frequencyOffset;
            double[] iData;
            double[] qData;
            double[] blankData;
            try
            {
                // Read in all of the control values 
                resourceName = resourceNameComboBox.Text;
                frequency = (double)frequencyNumeric.Value;
                power = (double)powerLevelNumeric.Value;
                iqRate = (double)iqRateNumeric.Value;

                iData = SinePattern(numberOfSamples, Amplitude, IDataPhaseDegrees, NumberOfCycles);
                qData = SinePattern(numberOfSamples, Amplitude, QDataPhaseDegrees, NumberOfCycles);
                blankData = Enumerable.Repeat(0.0, numberOfBlankSamples).ToArray();

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

                // Configure scriptTrigger0 and scriptTrigger1 
                _rfsgSession.Triggers.ScriptTriggers[0].ConfigureSoftwareTrigger();

                _rfsgSession.Triggers.ScriptTriggers[1].ConfigureSoftwareTrigger();

                // Configure the IQ rate of the waveforms 
                _rfsgSession.Arb.IQRate = iqRate;

                // Configure Pre-filter Gain to avoid overflow due to phase-
                //  discontinuous signals 
                _rfsgSession.Arb.PreFilterGain = -2;

                // Configure the power level type 
                _rfsgSession.RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower;

                // Get the actual IQ rate, and populate the GUI output 
                actualIQRate = _rfsgSession.Arb.IQRate;
                actualIQRateTextBox.Text = actualIQRate.ToString();
                frequencyOffset = actualIQRate / numberOfSamples;
                actualFrequencyOffsetTextBox.Text = frequencyOffset.ToString();

                // Configure the signal bandwidth 
                _rfsgSession.Arb.SignalBandwidth = frequencyOffset * 2;

                // Write the arb waveforms 
                _rfsgSession.Arb.WriteWaveform("negativeOffset", iData, qData);
                _rfsgSession.Arb.WriteWaveform("positiveOffset", qData, iData);

                _rfsgSession.Arb.WriteWaveform("allZeros", blankData, blankData);

                // Write the script 
                scriptNumber = (int)scriptIndexNumeric.Value;
                _rfsgSession.Arb.Scripting.WriteScript(_scripts[scriptNumber]);

                // Initiate Generation 
                _rfsgSession.Initiate();

                stopButton.Focus();

                // Start the status checking timer 
                EnableControls(false);
            }
            catch (Exception ex)
            {
                ShowError("StartGeneration()", ex);
            }
        }

        private static double[] SinePattern(int numberOfSamples, double amplitude, double phaseDegrees, double numberOfCycles)
        {
            double[] sineArray = new double[numberOfSamples];
            for (int i = 0; i < numberOfSamples; i++)
            {
                sineArray[i] = amplitude * Math.Sin(2 * Math.PI * i * numberOfCycles / numberOfSamples + Math.PI * phaseDegrees / 180);
            }
            return sineArray;
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

        void SendSoftwareScriptTrigger1()
        {
            try
            {
                // Send the software trigger 
                _rfsgSession.Triggers.ScriptTriggers[1].SendSoftwareEdgeTrigger();
            }
            catch (Exception ex)
            {
                ShowError("SendSoftwareScriptTrigger1()", ex);
            }
        }

        void ShowError(string functionName, Exception exception)
        {
            StopGeneration();
            errorTextBox.Text = "Error in " + functionName + ": " + exception.Message;
        }

        #endregion

        #region Form Events
        private void scriptIndexNumeric_ValueChanged(object sender, System.EventArgs e)
        {
            int scriptNum;
            scriptNum = (int)scriptIndexNumeric.Value;
            if (scriptNum > 1)
                scriptNum = 1;
            scriptTextBox.Text = _scripts[scriptNum];
        }

        private void startButton_Click(object sender, System.EventArgs e)
        {
            StartGeneration();
        }

        private void stopButton_Click(object sender, System.EventArgs e)
        {
            StopGeneration();
        }

        private void trigger2Button_Click(object sender, System.EventArgs e)
        {
            SendSoftwareScriptTrigger1();
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
            resourceNameComboBox.Enabled = enabled;
            frequencyNumeric.Enabled = enabled;
            powerLevelNumeric.Enabled = enabled;
            iqRateNumeric.Enabled = enabled;
            scriptIndexNumeric.Enabled = enabled;

            // Start the status checking timer
            rfsgStatusTimer.Enabled = !enabled;

            triggerButton.Enabled = !enabled;
            trigger2Button.Enabled = !enabled && ((int)scriptIndexNumeric.Value == 1);

            Application.DoEvents();
        }
    }
}
