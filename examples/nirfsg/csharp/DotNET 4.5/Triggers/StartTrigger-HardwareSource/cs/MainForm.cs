//==================================================================================================
// Title        : Start Trigger-Hardware Source
// Description  : This program demonstrates the use of niRFSG to generate a simple sine wave 
//			at a specified frequency and output gain when a hardware trigger is received. 
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

namespace NationalInstruments.Examples.StartTriggerHardwareSource
{
    public partial class MainForm : Form
    {
        NIRfsg _rfsgSession;

        public MainForm()
        {
            InitializeComponent();

            LoadRfsgDeviceNames();

            ConfigureTriggerTypeComboBox();
            ConfigureTriggerSourceComboBox();
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

        private void ConfigureTriggerTypeComboBox()
        {
            triggerTypeComboBox.Items.Add(RfsgStartTriggerType.None);
            triggerTypeComboBox.Items.Add(RfsgStartTriggerType.DigitalEdge);
            triggerTypeComboBox.SelectedIndex = 0;
        }

        private void ConfigureTriggerSourceComboBox()
        {
            var triggerSourceValueList = new List<DictionaryEntry>();
            triggerSourceValueList.Add(new DictionaryEntry("PFI0", RfsgDigitalEdgeStartTriggerSource.Pfi0));
            triggerSourceValueList.Add(new DictionaryEntry("PFI1", RfsgDigitalEdgeStartTriggerSource.Pfi1));
            triggerSourceValueList.Add(new DictionaryEntry("PFI2", RfsgDigitalEdgeStartTriggerSource.Pfi2));
            triggerSourceValueList.Add(new DictionaryEntry("PFI3", RfsgDigitalEdgeStartTriggerSource.Pfi3));
            triggerSourceValueList.Add(new DictionaryEntry("PXI_Trig0", RfsgDigitalEdgeStartTriggerSource.PxiTriggerLine0));
            triggerSourceValueList.Add(new DictionaryEntry("PXI_Trig1", RfsgDigitalEdgeStartTriggerSource.PxiTriggerLine1));
            triggerSourceValueList.Add(new DictionaryEntry("PXI_Trig2", RfsgDigitalEdgeStartTriggerSource.PxiTriggerLine2));
            triggerSourceValueList.Add(new DictionaryEntry("PXI_Trig3", RfsgDigitalEdgeStartTriggerSource.PxiTriggerLine3));
            triggerSourceValueList.Add(new DictionaryEntry("PXI_Trig4", RfsgDigitalEdgeStartTriggerSource.PxiTriggerLine4));
            triggerSourceValueList.Add(new DictionaryEntry("PXI_Trig5", RfsgDigitalEdgeStartTriggerSource.PxiTriggerLine5));
            triggerSourceValueList.Add(new DictionaryEntry("PXI_Trig6", RfsgDigitalEdgeStartTriggerSource.PxiTriggerLine6));
            triggerSourceValueList.Add(new DictionaryEntry("PXI_Trig7", RfsgDigitalEdgeStartTriggerSource.PxiTriggerLine7));
            triggerSourceValueList.Add(new DictionaryEntry("PXI_STAR", RfsgDigitalEdgeStartTriggerSource.PxiStarLine));
     
            triggerSourceComboBox.DataSource = triggerSourceValueList;
            triggerSourceComboBox.DisplayMember = "Key";
            triggerSourceComboBox.ValueMember = "Value";
            triggerSourceComboBox.SelectedValue = RfsgDigitalEdgeStartTriggerSource.Pfi0;            
        }

        #endregion

        #region Program Functions

        void StartGeneration()
        {
            string resourceName;
            double frequency, power;
            RfsgStartTriggerType triggerType;
            RfsgDigitalEdgeStartTriggerSource triggerSource;
            try
            {
                // Read in all of the control values 
                resourceName = resourceNameComboBox.Text;
                frequency = (double)frequencyNumeric.Value;
                power = (double)powerLevelNumeric.Value;
                triggerType = (RfsgStartTriggerType)triggerTypeComboBox.SelectedItem;
                triggerSource = triggerSourceComboBox.SelectedValue as RfsgDigitalEdgeStartTriggerSource ?? RfsgDigitalEdgeStartTriggerSource.FromString(triggerSourceComboBox.Text);

                errorTextBox.Text = "No error.";
                Application.DoEvents();

                // Initialize the NIRfsg session
                _rfsgSession = new NIRfsg(resourceName, true, false);

                // Subscribe to Rfsg warnings
                _rfsgSession.DriverOperation.Warning += new EventHandler<RfsgWarningEventArgs>(DriverOperation_Warning);

                // Configure the instrument 
                _rfsgSession.RF.Configure(frequency, power);

                if (triggerType == RfsgStartTriggerType.DigitalEdge)
                {
                    _rfsgSession.Triggers.StartTrigger.DigitalEdge.Configure(triggerSource, RfsgTriggerEdge.RisingEdge);
                }
                else
                {
                    _rfsgSession.Triggers.StartTrigger.Disable();
                }

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
            resourceNameComboBox.Enabled = enabled;
            frequencyNumeric.Enabled = enabled;
            powerLevelNumeric.Enabled = enabled;
            triggerTypeComboBox.Enabled = enabled;
            triggerSourceComboBox.Enabled = enabled;
            rfsgStatusTimer.Enabled = !enabled;

            Application.DoEvents();
        }
    }
}
