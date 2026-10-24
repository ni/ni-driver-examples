//==================================================================================================
// Title        : Script Trigger-Hardware Source
// Description  : This example demonstrates how to a generate signals based on a hardware script
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
using System.Linq;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.ScriptTriggerHardwareSource
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
                               "script triggersToggleWaveforms" + Environment.NewLine+
                               "   repeat forever" + Environment.NewLine+
                               "      repeat until scriptTrigger0" + Environment.NewLine+
                               "         generate negativeOffset" + Environment.NewLine+
                               "      end repeat" + Environment.NewLine+
                               "      repeat until scriptTrigger0" + Environment.NewLine+
                               "         generate positiveOffset" + Environment.NewLine+
                               "      end repeat" + Environment.NewLine+
                               "   end repeat" + Environment.NewLine+
                               "end script",

                               "script myScript" + Environment.NewLine +
                               "   Repeat forever" + Environment.NewLine +
                               "      Generate allZeros" + Environment.NewLine +
                               "      Clear scriptTrigger0" + Environment.NewLine +
                               "      Wait until scriptTrigger0" + Environment.NewLine +
                               "      Clear scriptTrigger1" + Environment.NewLine +
                               "      Repeat until scriptTrigger1" + Environment.NewLine +
                               "         Generate positiveOffset" + Environment.NewLine +
                               "      end repeat" + Environment.NewLine +
                               "      Clear scriptTrigger1" + Environment.NewLine +
                               "      Repeat until scriptTrigger1" + Environment.NewLine +
                               "         Generate negativeOffset" + Environment.NewLine +
                               "      end repeat" + Environment.NewLine +
                               "   end repeat" + Environment.NewLine +
                               "end script"
                           };

        public MainForm()
        {
            InitializeComponent();

            LoadRfsgDeviceNames();

            ConfigureTrigger2TypeComboBox();
            ConfigureTrigger1TypeComboBox();
            ConfigureTriggerSource2ComboBox();
            ConfigureTriggerSource1ComboBox();

            // force set the script
            scriptIndexNumeric.Value = 1;
            scriptIndexNumeric.Value = 0;
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

        private void ConfigureTrigger2TypeComboBox()
        {
            trigger2TypeComboBox.Items.Add(RfsgScriptTriggerType.None);
            trigger2TypeComboBox.Items.Add(RfsgScriptTriggerType.DigitalEdge);
            trigger2TypeComboBox.Items.Add(RfsgScriptTriggerType.DigitalLevel);
            trigger2TypeComboBox.SelectedIndex = 0;
        }

        private void ConfigureTrigger1TypeComboBox()
        {
            trigger1TypeComboBox.Items.Add(RfsgScriptTriggerType.None);
            trigger1TypeComboBox.Items.Add(RfsgScriptTriggerType.DigitalEdge);
            trigger1TypeComboBox.Items.Add(RfsgScriptTriggerType.DigitalLevel);
            trigger1TypeComboBox.SelectedIndex = 0;
        }

        private void ConfigureTriggerSource2ComboBox()
        {
            var triggerSource2ValueList = new List<DictionaryEntry>();
            triggerSource2ValueList.Add(new DictionaryEntry("PFI0", RfsgDigitalEdgeScriptTriggerSource.Pfi0));
            triggerSource2ValueList.Add(new DictionaryEntry("PFI1", RfsgDigitalEdgeScriptTriggerSource.Pfi1));
            triggerSource2ValueList.Add(new DictionaryEntry("PFI2", RfsgDigitalEdgeScriptTriggerSource.Pfi2));
            triggerSource2ValueList.Add(new DictionaryEntry("PFI3", RfsgDigitalEdgeScriptTriggerSource.Pfi3));
            triggerSource2ValueList.Add(new DictionaryEntry("PXI_Trig0", RfsgDigitalEdgeScriptTriggerSource.PxiTriggerLine0));
            triggerSource2ValueList.Add(new DictionaryEntry("PXI_Trig1", RfsgDigitalEdgeScriptTriggerSource.PxiTriggerLine1));
            triggerSource2ValueList.Add(new DictionaryEntry("PXI_Trig2", RfsgDigitalEdgeScriptTriggerSource.PxiTriggerLine2));
            triggerSource2ValueList.Add(new DictionaryEntry("PXI_Trig3", RfsgDigitalEdgeScriptTriggerSource.PxiTriggerLine3));
            triggerSource2ValueList.Add(new DictionaryEntry("PXI_Trig4", RfsgDigitalEdgeScriptTriggerSource.PxiTriggerLine4));
            triggerSource2ValueList.Add(new DictionaryEntry("PXI_Trig5", RfsgDigitalEdgeScriptTriggerSource.PxiTriggerLine5));
            triggerSource2ValueList.Add(new DictionaryEntry("PXI_Trig6", RfsgDigitalEdgeScriptTriggerSource.PxiTriggerLine6));
            triggerSource2ValueList.Add(new DictionaryEntry("PXI_Trig7", RfsgDigitalEdgeScriptTriggerSource.PxiTriggerLine7));
            triggerSource2ValueList.Add(new DictionaryEntry("PXI_STAR", RfsgDigitalEdgeScriptTriggerSource.PxiStarLine));

            triggerSource2ComboBox.DataSource = triggerSource2ValueList;
            triggerSource2ComboBox.DisplayMember = "Key";
            triggerSource2ComboBox.ValueMember = "Value";
            triggerSource2ComboBox.SelectedIndex = 1;

        }

        private void ConfigureTriggerSource1ComboBox()
        {
            var triggerSource1ValueList = new List<DictionaryEntry>();
            triggerSource1ValueList.Add(new DictionaryEntry("PFI0", RfsgDigitalEdgeScriptTriggerSource.Pfi0));
            triggerSource1ValueList.Add(new DictionaryEntry("PFI1", RfsgDigitalEdgeScriptTriggerSource.Pfi1));
            triggerSource1ValueList.Add(new DictionaryEntry("PFI2", RfsgDigitalEdgeScriptTriggerSource.Pfi2));
            triggerSource1ValueList.Add(new DictionaryEntry("PFI3", RfsgDigitalEdgeScriptTriggerSource.Pfi3));
            triggerSource1ValueList.Add(new DictionaryEntry("PXI_Trig0", RfsgDigitalEdgeScriptTriggerSource.PxiTriggerLine0));
            triggerSource1ValueList.Add(new DictionaryEntry("PXI_Trig1", RfsgDigitalEdgeScriptTriggerSource.PxiTriggerLine1));
            triggerSource1ValueList.Add(new DictionaryEntry("PXI_Trig2", RfsgDigitalEdgeScriptTriggerSource.PxiTriggerLine2));
            triggerSource1ValueList.Add(new DictionaryEntry("PXI_Trig3", RfsgDigitalEdgeScriptTriggerSource.PxiTriggerLine3));
            triggerSource1ValueList.Add(new DictionaryEntry("PXI_Trig4", RfsgDigitalEdgeScriptTriggerSource.PxiTriggerLine4));
            triggerSource1ValueList.Add(new DictionaryEntry("PXI_Trig5", RfsgDigitalEdgeScriptTriggerSource.PxiTriggerLine5));
            triggerSource1ValueList.Add(new DictionaryEntry("PXI_Trig6", RfsgDigitalEdgeScriptTriggerSource.PxiTriggerLine6));
            triggerSource1ValueList.Add(new DictionaryEntry("PXI_Trig7", RfsgDigitalEdgeScriptTriggerSource.PxiTriggerLine7));
            triggerSource1ValueList.Add(new DictionaryEntry("PXI_STAR", RfsgDigitalEdgeScriptTriggerSource.PxiStarLine));

            triggerSource1ComboBox.DataSource = triggerSource1ValueList;
            triggerSource1ComboBox.DisplayMember = "Key";
            triggerSource1ComboBox.ValueMember = "Value";
            triggerSource1ComboBox.SelectedIndex = 0;
        }

        #endregion

        #region Program Functions

        void StartGeneration()
        {
            string resourceName;
            int kNumberOfSamples = 100;
            int kNumberOfBlankSamples = 500;
            RfsgScriptTriggerType triggerType1;
            RfsgScriptTriggerType triggerType2;
            int scriptNum;
            double frequency;
            double power;
            double iqRate;
            double actualIQRate;
            double frequencyOffset;
            string triggerSource1;
            string triggerSource2;
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
                triggerType1 = (RfsgScriptTriggerType)trigger1TypeComboBox.SelectedItem;
                triggerType2 = (RfsgScriptTriggerType)trigger2TypeComboBox.SelectedItem;
                triggerSource1 = triggerSource1ComboBox.Text;
                triggerSource2 = triggerSource2ComboBox.Text;

                iData = SinePattern(kNumberOfSamples, Amplitude, IDataPhaseDegrees, NumberOfCycles);
                qData = SinePattern(kNumberOfSamples, Amplitude, QDataPhaseDegrees, NumberOfCycles);
                blankData = Enumerable.Repeat(0.0, kNumberOfBlankSamples).ToArray();

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

                // Configure the IQ rate of the waveforms 
                _rfsgSession.Arb.IQRate = iqRate;

                // Configure Pre-filter Gain to avoid overflow due to phase-
                //  discontinuous signals 
                _rfsgSession.Arb.PreFilterGain = ArbPreFilterGain;

                // Configure the power level type 
                _rfsgSession.RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower;

                // Configure scriptTrigger0 (refer to the script) 
                if (triggerType1 == RfsgScriptTriggerType.DigitalEdge)
                {
                    _rfsgSession.Triggers.ScriptTriggers[0].DigitalEdge.Configure(triggerSource1, RfsgTriggerEdge.RisingEdge);
                }
                else if (triggerType1 == RfsgScriptTriggerType.DigitalLevel)
                {
                    _rfsgSession.Triggers.ScriptTriggers[0].DigitalLevel.Configure(triggerSource1, RfsgTriggerLevel.ActiveHigh);
                }
                else
                {
                    _rfsgSession.Triggers.ScriptTriggers[0].Disable();
                }

                // Configure scriptTrigger1 (refer to the script) 
                if (triggerType2 == RfsgScriptTriggerType.DigitalEdge)
                {
                    _rfsgSession.Triggers.ScriptTriggers[1].DigitalEdge.Configure(triggerSource2, RfsgTriggerEdge.RisingEdge);
                }
                else if (triggerType2 == RfsgScriptTriggerType.DigitalLevel)
                {
                    _rfsgSession.Triggers.ScriptTriggers[1].DigitalLevel.Configure(triggerSource2, RfsgTriggerLevel.ActiveHigh);
                }
                else
                {
                    _rfsgSession.Triggers.ScriptTriggers[1].Disable();
                }

                // Get the actual IQ rate, and populate the GUI output 
                actualIQRate = _rfsgSession.Arb.IQRate;
                actualIQRateTextBox.Text = actualIQRate.ToString();
                frequencyOffset = actualIQRate / kNumberOfSamples;
                actualFrequencyOffsetTextBox.Text = frequencyOffset.ToString();

                // Configure the signal bandwidth 
                _rfsgSession.Arb.SignalBandwidth = frequencyOffset * 2;

                // Write the arb waveforms 
                _rfsgSession.Arb.WriteWaveform("negativeOffset", iData, qData);
                _rfsgSession.Arb.WriteWaveform("positiveOffset", qData, iData);
                _rfsgSession.Arb.WriteWaveform("allZeros", blankData, blankData);

                // Write the script 
                scriptNum = (int)scriptIndexNumeric.Value;
                _rfsgSession.Arb.Scripting.WriteScript(_scripts[scriptNum]);

                // Initiate Generation 
                _rfsgSession.Initiate();

                // Activate stop button 
                stopButton.Focus();

                // Start the status checking timer 
                EnableControls(false);
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

        void StopGeneration()
        {
            // Stop the status checking timer, and turn off the LED 
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
            trigger1TypeComboBox.Enabled = enabled;
            triggerSource1ComboBox.Enabled = enabled;
            trigger2TypeComboBox.Enabled = enabled;
            triggerSource2ComboBox.Enabled = enabled;
            scriptIndexNumeric.Enabled = enabled;

            // Start the status checking timer
            rfsgStatusTimer.Enabled = !enabled;

            Application.DoEvents();
        }
    }
}
