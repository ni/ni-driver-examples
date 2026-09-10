//==================================================================================================
// Title        : Frequency And PowerSweep (TimerTriggered)
// Description  : This example demonstrates how to create a Configuration List and how to trigger 
//			steps using a timer. 
//			Set the Start and End to the same value to keep the Frequency or Power Level 
//			constant in the Configuration List. 
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

namespace NationalInstruments.Examples.FrequencyAndPowerSweepTimerTriggered
{
    public partial class MainForm : Form
    {
        NIRfsg _rfsgSession;
        const int ArbPreFilterGain = -2;
        const double FrequencyReferenceRate = 10E6;
        const int RfAdvancedFrequencySettlingTime = 0;
        public MainForm()
        {
            InitializeComponent();

            LoadRfsgDeviceNames();

            ConfigureClockSourceComboBox();
            ConfigureLoopBandwidthComboBox();
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

        private void ConfigureClockSourceComboBox()
        {
            var refSourceValueList = new List<DictionaryEntry>();
            refSourceValueList.Add(new DictionaryEntry("ClkIn", RfsgFrequencyReferenceSource.ClockIn));
            refSourceValueList.Add(new DictionaryEntry("OnboardClock", RfsgFrequencyReferenceSource.OnboardClock));
            refSourceValueList.Add(new DictionaryEntry("PXI_CLK", RfsgFrequencyReferenceSource.PxiClock));
            refSourceValueList.Add(new DictionaryEntry("RefIn", RfsgFrequencyReferenceSource.ReferenceIn));

            clockSourceComboBox.DataSource = refSourceValueList;
            clockSourceComboBox.DisplayMember = "Key";
            clockSourceComboBox.ValueMember = "Value";
            clockSourceComboBox.SelectedValue = RfsgFrequencyReferenceSource.OnboardClock;


        }

        private void ConfigureLoopBandwidthComboBox()
        {
            loopBandwidthComboBox.Items.AddRange(Enum.GetNames(typeof(RfsgLoopBandwidth)));
            loopBandwidthComboBox.SelectedIndex = 2;

        }
        #endregion

        #region Program Functions

        void StartGeneration()
        {
            string resourceName;
            double iqRate;
            double actualIQRate;
            double startFrequency;
            double endFrequency;
            double startPowerLevel;
            double endPowerLevel;
            double dwellTime;
            int numberOfSteps;
            RfsgLoopBandwidth loopBandwidth;
            string clockSource;
            string script;
            double frequencyForStep;
            double powerLevelForStep;
            double frequencyIncrement = 0;
            double powerLevelIncrement = 0;
            int waveformSize = 100000;
            int waveformIterator, numberOfStepsIterator;
            double[] iData;
            double[] qData;
            RfsgConfigurationListProperties[] configurationListAttributes = { RfsgConfigurationListProperties.Frequency, 
                                                                                RfsgConfigurationListProperties.PowerLevel };
            try
            {
                // Read in all of the control values 
                iqRate = (double)iqRateNumeric.Value;
                resourceName = resourceNameComboBox.Text;
                startFrequency = (double)startFrequencyNumeric.Value;
                endFrequency = (double)stopFrequencyNumeric.Value;
                startPowerLevel = (double)startPowerNumeric.Value;
                endPowerLevel = (double)stopPowerNumeric.Value;
                dwellTime = (double)dwellTimeNumeric.Value;
                numberOfSteps = (int)numberStepsNumeric.Value;
                loopBandwidth = (RfsgLoopBandwidth)Enum.Parse(typeof(RfsgLoopBandwidth), (string)loopBandwidthComboBox.SelectedItem);
                clockSource = (string)clockSourceComboBox.Text;
                script = scriptTextBox.Text;

                // Generate I and Q data
                iData = new double[waveformSize];
                qData = new double[waveformSize];
                for (waveformIterator = 0; waveformIterator < waveformSize; waveformIterator++)
                {
                    iData[waveformIterator] = 1.0;
                    qData[waveformIterator] = 0.0;
                }

                errorTextBox.Text = "No error.";
                Application.DoEvents();

                // Initialize the NIRfsg session
                _rfsgSession = new NIRfsg(resourceName, true, false);

                // Subscribe to Rfsg warnings
                _rfsgSession.DriverOperation.Warning += new EventHandler<RfsgWarningEventArgs>(DriverOperation_Warning);

                // Configure the generation mode to Script 
                _rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script;

                // Configure the power level type 
                _rfsgSession.RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower;

                // Configure the reference clock source 
                _rfsgSession.FrequencyReference.Configure(clockSource, FrequencyReferenceRate);

                // Configure Pre-filter Gain to avoid overflow due to phase-
                //  discontinuous signals 
                _rfsgSession.Arb.PreFilterGain = ArbPreFilterGain;

                // Configure the loop bandwidth 
                _rfsgSession.RF.LocalOscillator.LoopBandwidth = loopBandwidth;

                // Configure the IQ rate of the waveforms 
                _rfsgSession.Arb.IQRate = iqRate;

                // Get the actual IQ rate, and populate the GUI output 
                actualIQRate = _rfsgSession.Arb.IQRate;

                actualIQRateTextBox.Text = actualIQRate.ToString();

                //  Configure the Frequency Settling Units to 'Seconds After I/O'. 
                //   RFSG currrently only supports  'Seconds After I/O' when using the 
                //   Configuration List feature. 
                _rfsgSession.RF.Advanced.FrequencySettlingUnits = RfsgRFFrequencySettlingUnits.TimeAfterIO;

                // Configure the Frequency Settling to 0 seconds to indicate that we 
                //  don't want to wait for the frequency to settle in each step. If the 
                //  step is waiting for the frequency to settle, the device will not 
                //  advance to the next configuration when a trigger is received. 
                _rfsgSession.RF.Advanced.FrequencySettlingTime = RfAdvancedFrequencySettlingTime;

                // Configure the trigger type to advance steps in the list 
                _rfsgSession.Triggers.ConfigurationListStepTrigger.TriggerType = RfsgConfigurationListStepTriggerType.DigitalEdge;

                // Configure the trigger source to advance steps in the list 
                _rfsgSession.Triggers.ConfigurationListStepTrigger.DigitalEdge.Source = RfsgDigitalEdgeConfigurationListStepTriggerSource.TimerEvent;

                // Configure the timer event interval 
                _rfsgSession.DeviceEvents.Timer.Interval = dwellTime;

                //  Create a Configuration List. Pass Frequency and Power Level in the 
                //   Configuration List Attributes parameter to be able to configure 
                //   Frequency and Power Level in each step that we create. Pass true to
                //   the Set As Active List parameter, this will set the Active Configuration 
                //   List attribute to the name of the created Configuration List. Once the 
                //   Active Configuration List is set, the Frequency or Power Level will 
                //   be modified for this configuration list. 
                _rfsgSession.BasicConfigurationList.CreateConfigurationList("frequencyAndPowerLevelList", configurationListAttributes, true);

                frequencyForStep = startFrequency;
                powerLevelForStep = startPowerLevel;

                if (numberOfSteps > 1)
                {
                    frequencyIncrement = (endFrequency - startFrequency) / (numberOfSteps - 1);
                    powerLevelIncrement = (endPowerLevel - startPowerLevel) / (numberOfSteps - 1);
                }

                // Build the Configuration List 
                for (numberOfStepsIterator = 0; numberOfStepsIterator < numberOfSteps; numberOfStepsIterator++)
                {
                    //  Create a Configuration List Step. Pass true to the Set As Active 
                    // Step parameter, this will set the Active Configuration List Step 
                    // attribute to the created Configuration List Step index. Once the 
                    // Active Configuration List Step is set, the Frequency or Power Level 
                    // will be modified for this Configuration List Step in the Configuration 
                    // List indicated by the Active Configuration List attribute. 
                    _rfsgSession.BasicConfigurationList.CreateStep(true);

                    // Configure the Frequency for this step
                    _rfsgSession.RF.Frequency = frequencyForStep;

                    // Configure the Power Level for this step
                    _rfsgSession.RF.PowerLevel = powerLevelForStep;

                    frequencyForStep += frequencyIncrement;
                    powerLevelForStep += powerLevelIncrement;
                }

                // Write the waveform 
                _rfsgSession.Arb.WriteWaveform("continuousWaveformTone", iData, qData);

                // Write the script 
                _rfsgSession.Arb.Scripting.WriteScript(script);

                // Initiate Generation 
                _rfsgSession.Initiate();

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
                    // Abort the generation to delete the Configuration List 
                    _rfsgSession.Abort();

                    // Delete the Configuration List 
                    _rfsgSession.BasicConfigurationList.DeleteConfigurationList("frequencyAndPowerLevelList");

                    // Set the Active Configuration List to string.Empty 
                    _rfsgSession.BasicConfigurationList.ActiveList = string.Empty;

                    // Disable the output 
                    _rfsgSession.RF.OutputEnabled = false;

                    // When a Configuration List is stopped, NI-RFSG transitions to the Configuration State.  
                    //  The new value of the Output Enabled attribute is committed to apply the change to the hardware.
                    _rfsgSession.Utility.Commit();

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
            iqRateNumeric.Enabled = enabled;
            resourceNameComboBox.Enabled = enabled;
            rfsgStatusTimer.Enabled = !enabled;
            scriptTextBox.Enabled = enabled;
            startFrequencyNumeric.Enabled = enabled;
            stopFrequencyNumeric.Enabled = enabled;
            startPowerNumeric.Enabled = enabled;
            stopPowerNumeric.Enabled = enabled;
            dwellTimeNumeric.Enabled = enabled;
            numberStepsNumeric.Enabled = enabled;
            loopBandwidthComboBox.Enabled = enabled;
            clockSourceComboBox.Enabled = enabled;

            Application.DoEvents();
        }
    }
}
