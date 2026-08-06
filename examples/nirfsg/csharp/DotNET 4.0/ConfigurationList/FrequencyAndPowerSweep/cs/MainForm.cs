//==================================================================================================
// Title        : FrequencyAndPowerSweep
// Description  : This example demonstrates how to create a Configuration List for sweeping frequency and/or power.                                                              
//==================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.FrequencyAndPowerSweep
{
    public partial class MainForm : Form
    {
        NIRfsg _rfsgSession;
        const int ArbPreFilterGain = -2;
        const double FrequencyReferenceRate = 10E6;

        public MainForm()
        {
            InitializeComponent();
            LoadRfsgDeviceNames();
            ConfigureReferenceClockSourceComboBox();
            ConfigureListTriggerSourceComboBox();
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

        private void ConfigureReferenceClockSourceComboBox()
        {
            var referenceSourceValueList = new List<DictionaryEntry>();
            referenceSourceValueList.Add(new DictionaryEntry("OnboardClock", RfsgFrequencyReferenceSource.OnboardClock));
            referenceSourceValueList.Add(new DictionaryEntry("RefIn", RfsgFrequencyReferenceSource.ReferenceIn));
            referenceSourceValueList.Add(new DictionaryEntry("PXI_CLK", RfsgFrequencyReferenceSource.PxiClock));
            referenceSourceValueList.Add(new DictionaryEntry("ClkIn", RfsgFrequencyReferenceSource.ClockIn));
            referenceClockSourceComboBox.DataSource = referenceSourceValueList;
            referenceClockSourceComboBox.DisplayMember = "Key";
            referenceClockSourceComboBox.ValueMember = "Value";
            referenceClockSourceComboBox.SelectedValue = RfsgFrequencyReferenceSource.OnboardClock;

        }

        private void ConfigureListTriggerSourceComboBox()
        {
            var referenceSourceValueList = new List<DictionaryEntry>();
            referenceSourceValueList.Add(new DictionaryEntry("TimerEvent", RfsgDigitalEdgeConfigurationListStepTriggerSource.TimerEvent));
            referenceSourceValueList.Add(new DictionaryEntry("PFI0", RfsgDigitalEdgeConfigurationListStepTriggerSource.Pfi0));
            referenceSourceValueList.Add(new DictionaryEntry("PFI1", RfsgDigitalEdgeConfigurationListStepTriggerSource.Pfi1));
            referenceSourceValueList.Add(new DictionaryEntry("PXI_Trig0", RfsgDigitalEdgeConfigurationListStepTriggerSource.PxiTriggerLine0));
            referenceSourceValueList.Add(new DictionaryEntry("PXI_Trig1", RfsgDigitalEdgeConfigurationListStepTriggerSource.PxiTriggerLine1));
            referenceSourceValueList.Add(new DictionaryEntry("PXI_Trig2", RfsgDigitalEdgeConfigurationListStepTriggerSource.PxiTriggerLine2));
            referenceSourceValueList.Add(new DictionaryEntry("PXI_Trig3", RfsgDigitalEdgeConfigurationListStepTriggerSource.PxiTriggerLine3));
            referenceSourceValueList.Add(new DictionaryEntry("PXI_Trig4", RfsgDigitalEdgeConfigurationListStepTriggerSource.PxiTriggerLine4));
            referenceSourceValueList.Add(new DictionaryEntry("PXI_Trig5", RfsgDigitalEdgeConfigurationListStepTriggerSource.PxiTriggerLine5));
            referenceSourceValueList.Add(new DictionaryEntry("PXI_Trig6", RfsgDigitalEdgeConfigurationListStepTriggerSource.PxiTriggerLine6));
            referenceSourceValueList.Add(new DictionaryEntry("PXI_Trig7", RfsgDigitalEdgeConfigurationListStepTriggerSource.PxiTriggerLine7));
            referenceSourceValueList.Add(new DictionaryEntry("Marker0Event", RfsgDigitalEdgeConfigurationListStepTriggerSource.Marker0Event));
            referenceSourceValueList.Add(new DictionaryEntry("Marker1Event", RfsgDigitalEdgeConfigurationListStepTriggerSource.Marker1Event));
            referenceSourceValueList.Add(new DictionaryEntry("Marker2Event", RfsgDigitalEdgeConfigurationListStepTriggerSource.Marker2Event));
            referenceSourceValueList.Add(new DictionaryEntry("Marker3Event", RfsgDigitalEdgeConfigurationListStepTriggerSource.Marker3Event));
            listTriggerSourceComboBox.DataSource = referenceSourceValueList;
            listTriggerSourceComboBox.DisplayMember = "Key";
            listTriggerSourceComboBox.ValueMember = "Value";
            listTriggerSourceComboBox.SelectedValue = RfsgDigitalEdgeConfigurationListStepTriggerSource.TimerEvent;
        }

        #endregion

        #region Program Functions

        void StartGeneration()
        {
            string resourceName;
            double startFrequency;
            double endFrequency;
            double startPowerLevel;
            double endPowerLevel;
            double dwellTime;
            int numberOfSteps;
            double frequencyForStep;
            double powerLevelForStep;
            double frequencyIncrement = 0;
            double powerLevelIncrement = 0;
            double frequencySettlingTime;
            int numberOfStepsIterator;

            RfsgConfigurationListProperties[] configurationListAttributes = { RfsgConfigurationListProperties.Frequency, 
                                                                                    RfsgConfigurationListProperties.PowerLevel };
            RfsgFrequencyReferenceSource refClockSource;
            RfsgDigitalEdgeConfigurationListStepTriggerSource listTriggerSource;
            try
            {
                // Read in all of the control values 
                resourceName = resourceNameComboBox.Text;
                startFrequency = (double)startFrequencyNumeric.Value;
                endFrequency = (double)endFrequencyNumeric.Value;
                startPowerLevel = (double)startPowerNumeric.Value;
                endPowerLevel = (double)stopPowerNumeric.Value;
                dwellTime = (double)dwellTimeNumeric.Value;
                numberOfSteps = (int)numberStepsNumeric.Value;
                frequencySettlingTime = (double)frequencySettlingsNumeric.Value;

                refClockSource = referenceClockSourceComboBox.SelectedValue as RfsgFrequencyReferenceSource ?? RfsgFrequencyReferenceSource.FromString(referenceClockSourceComboBox.Text);
                listTriggerSource = listTriggerSourceComboBox.SelectedValue as RfsgDigitalEdgeConfigurationListStepTriggerSource ?? RfsgDigitalEdgeConfigurationListStepTriggerSource.FromString(listTriggerSourceComboBox.Text);

                errorTextBox.Text = "No error.";
                Application.DoEvents();

                // Initialize the NIRfsg session
                _rfsgSession = new NIRfsg(resourceName, true, false);

                // Subscribe to Rfsg warnings
                _rfsgSession.DriverOperation.Warning += new EventHandler<RfsgWarningEventArgs>(DriverOperation_Warning);

                // Configure the reference clock source 
                _rfsgSession.FrequencyReference.Configure(refClockSource, FrequencyReferenceRate);

                // Configure the Frequency Settling Units to 'Seconds After I/O'. 
                // RFSG currrently only supports 'Seconds After I/O' when using the Configuration List feature. 
                _rfsgSession.RF.Advanced.FrequencySettlingUnits = RfsgRFFrequencySettlingUnits.TimeAfterIO;

                // Configure the Frequency Settling to 0 seconds to indicate that we don't want to wait for the frequency to settle in each step.
                // If the step is waiting for the frequency to settle, the device will not advance to the next configuration when a trigger is received. 
                _rfsgSession.RF.Advanced.FrequencySettlingTime = frequencySettlingTime;

                // Configure the trigger type to advance steps in the list 
                _rfsgSession.Triggers.ConfigurationListStepTrigger.TriggerType = RfsgConfigurationListStepTriggerType.DigitalEdge;

                // Configure the trigger source to advance steps in the list 
                _rfsgSession.Triggers.ConfigurationListStepTrigger.DigitalEdge.Source = listTriggerSource;

                // Configure the timer event interval 
                // Note: Only used when listTriggerSource is TimerEvent
                if (listTriggerSource.Equals(RfsgDigitalEdgeConfigurationListStepTriggerSource.TimerEvent))
                {
                    _rfsgSession.DeviceEvents.Timer.Interval = dwellTime;
                }

                // Create a Configuration List. Pass Frequency and Power Level in the 
                // Configuration List Attributes parameter to be able to configure 
                // Frequency and Power Level in each step that we create. Pass true to the 
                // Set As Active List parameter, this will set the Active Configuration 
                // List attribute to the name of the created Configuration List. Once the 
                // Active Configuration List is set, the Frequency or Power Level will be modified for this configuration list. 
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
                    // Create a Configuration List Step. Pass true to the Set As Active 
                    // Step parameter, this will set the Active Configuration List Step attribute to the created
                    // Configuration List Step index. Once the 
                    // Active Configuration List Step is set, the Frequency or Power Level will be modified for this
                    // Configuration List Step in the Configuration 
                    // List indicated by the Active Configuration List attribute. 
                    _rfsgSession.BasicConfigurationList.CreateStep(true);

                    // Configure the Frequency for this step
                    _rfsgSession.RF.Frequency = frequencyForStep;

                    // Configure the Power Level for this step
                    _rfsgSession.RF.PowerLevel = powerLevelForStep;

                    frequencyForStep += frequencyIncrement;
                    powerLevelForStep += powerLevelIncrement;
                }

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

                    // Close the RFSG session
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

        private void EnableControls(bool enabled)
        {
            rfsgStatusTimer.Enabled = !enabled;
            startButton.Enabled = enabled;
            stopButton.Enabled = !enabled;
            configurationListTriggerGroupBox.Enabled = enabled;
            resourceNameComboBox.Enabled = enabled;
            configurationListParametersGroupBox.Enabled = enabled;
            referenceClockSourceComboBox.Enabled = enabled;

            Application.DoEvents();
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
    }
}
