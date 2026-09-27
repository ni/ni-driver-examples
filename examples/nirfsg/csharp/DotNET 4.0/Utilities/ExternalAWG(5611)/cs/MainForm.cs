//==================================================================================================
// Title        : External AWG (5611)
// Description  : This example demonstrates how to use the 5611 with             
//			an external AWG.
//==================================================================================================

using System;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.ExternalAWG5611
{
    public partial class MainForm : Form
    {
        NIRfsg _rfsgSession;
        const double FrequencyReferenceRate = 10E6;
        public MainForm()
        {
            InitializeComponent();

            LoadRfsgDeviceNames();

            ConfigureLOSwitchComboBox();
            ConfigureFrequencyReferenceSourceComboBox();
            ConfigureOutputTerminalComboBox();
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

        private void ConfigureLOSwitchComboBox()
        {
            loSwitchComboBox.Items.AddRange(new string[] { "Associated LO", "External LO" });
            loSwitchComboBox.SelectedIndex = 0;
        }

        private void ConfigureFrequencyReferenceSourceComboBox()
        {
            var refSourceValueList = new List<DictionaryEntry>();
            refSourceValueList.Add(new DictionaryEntry("ClkIn", RfsgFrequencyReferenceSource.ClockIn));
            refSourceValueList.Add(new DictionaryEntry("OnboardClock", RfsgFrequencyReferenceSource.OnboardClock));
            refSourceValueList.Add(new DictionaryEntry("PXI_CLK", RfsgFrequencyReferenceSource.PxiClock));
            refSourceValueList.Add(new DictionaryEntry("RefIn", RfsgFrequencyReferenceSource.ReferenceIn));

            frequencyReferenceSourceComboBox.DataSource = refSourceValueList;
            frequencyReferenceSourceComboBox.DisplayMember = "Key";
            frequencyReferenceSourceComboBox.ValueMember = "Value";
            frequencyReferenceSourceComboBox.SelectedValue = RfsgFrequencyReferenceSource.OnboardClock;
        }

        private void ConfigureOutputTerminalComboBox()
        {
            var outputTerminalValueList = new List<DictionaryEntry>();
            outputTerminalValueList.Add(new DictionaryEntry("ClkOut", RfsgFrequencyReferenceExportedOutputTerminal.ClockOut));
            outputTerminalValueList.Add(new DictionaryEntry("Do Not Export", RfsgFrequencyReferenceExportedOutputTerminal.DoNotExport));
            outputTerminalValueList.Add(new DictionaryEntry("RefOut", RfsgFrequencyReferenceExportedOutputTerminal.ReferenceOut));
            outputTerminalValueList.Add(new DictionaryEntry("RefOut2", RfsgFrequencyReferenceExportedOutputTerminal.ReferenceOut2));

            outputTerminalComboBox.DataSource = outputTerminalValueList;
            outputTerminalComboBox.DisplayMember = "Key";
            outputTerminalComboBox.ValueMember = "Value";
            outputTerminalComboBox.SelectedValue = RfsgFrequencyReferenceExportedOutputTerminal.DoNotExport;
        }

        #endregion

        #region Program Functions

        void StartGeneration()
        {
            string optionsString;
            string resourceName;
            double centerFrequency;
            double gain;
            RfsgFrequencyReferenceSource refClockSource;
            RfsgFrequencyReferenceExportedOutputTerminal outputTerminal;
            int externalLO;
            try
            {
                // Read in all the control values 
                resourceName = resourceNameComboBox.Text;
                centerFrequency = (double)centerFrequencyNumeric.Value;
                gain = (double)powerLevelNumeric.Value;
                externalLO = loSwitchComboBox.SelectedIndex;

                errorTextBox.Text = "No error.";
                Application.DoEvents();

                if (externalLO == 0)
                {
                    refClockSource = frequencyReferenceSourceComboBox.SelectedValue as RfsgFrequencyReferenceSource ?? RfsgFrequencyReferenceSource.FromString(frequencyReferenceSourceComboBox.Text);
                    outputTerminal = outputTerminalComboBox.SelectedValue as RfsgFrequencyReferenceExportedOutputTerminal ?? RfsgFrequencyReferenceExportedOutputTerminal.FromString(outputTerminalComboBox.Text);

                    optionsString = "DriverSetup=awg:<External>";

                    // Initialize the NIRfsg session
                    _rfsgSession = new NIRfsg(resourceName, true, false, optionsString);

                    // Subscribe to Rfsg warnings
                    _rfsgSession.DriverOperation.Warning += new EventHandler<RfsgWarningEventArgs>(DriverOperation_Warning);

                    _rfsgSession.FrequencyReference.Configure(refClockSource, FrequencyReferenceRate);

                    _rfsgSession.FrequencyReference.ExportedOutputTerminal = outputTerminal;
                }
                else  // Using external LO 
                {
                    optionsString = "DriverSetup=awg:<External>;lo:<External>";

                    // Initialize the NIRfsg session
                    _rfsgSession = new NIRfsg(resourceName, true, false, optionsString);

                    // Subscribe to Rfsg warnings
                    _rfsgSession.DriverOperation.Warning += new EventHandler<RfsgWarningEventArgs>(DriverOperation_Warning);
                }

                // Initiate Generation 
                _rfsgSession.Initiate();

                // Basic configuration 
                _rfsgSession.RF.Upconverter.CenterFrequency = centerFrequency;
                _rfsgSession.RF.Upconverter.Gain = gain;

                UpdateActualIQImpairments();

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
            // Activate all stopped controls 
            EnableControls(true);

            try
            {
                if (_rfsgSession != null)
                {
                    // Unsubscribe from warning events
                    _rfsgSession.DriverOperation.Warning -= DriverOperation_Warning;
                    // Close NIRfsg session
                    _rfsgSession.Close();
                    _rfsgSession = null;
                }
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
                _rfsgSession.CheckGenerationStatus();
            }
            catch (Exception ex)
            {
                ShowError("CheckGeneration()", ex);
            }
        }

        void UpdateActualIQImpairments()
        {
            double iOffset;
            double qOffset;
            double gainImbalance;
            double skew;
            double actualGain;

            try
            {
                _rfsgSession.Utility.WaitUntilSettled(10000);

                // Retrieve IQ impairment information 

                iOffset = _rfsgSession.IQImpairments.IOffset;

                qOffset = _rfsgSession.IQImpairments.QOffset;

                gainImbalance = _rfsgSession.IQImpairments.GainImbalance;

                skew = _rfsgSession.IQImpairments.Skew;

                actualGain = _rfsgSession.RF.Upconverter.Gain;

                // Output Impairments 
                actualIOffsetTextBox.Text = iOffset.ToString();
                actualQOffsetTextBox.Text = qOffset.ToString();
                actualGainImbalanceTextBox.Text = gainImbalance.ToString();
                actualSkewTextBox.Text = skew.ToString();
                actualGainTextBox.Text = actualGain.ToString();
            }
            catch (Exception ex)
            {
                ShowError("UpdateActualIQImpairments()", ex);
            }
        }

        void ShowError(string functionName, Exception exception)
        {
            StopGeneration();
            errorTextBox.Text = "Error in " + functionName + ": " + exception.Message;
        }

        #endregion

        #region Form Events

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopGeneration();
        }

        private void stopButton_Click(object sender, System.EventArgs e)
        {
            StopGeneration();
        }

        private void startButton_Click(object sender, System.EventArgs e)
        {
            StartGeneration();
        }

        private void rfsgStatusTimer_Tick(object sender, System.EventArgs e)
        {
            CheckGeneration();
        }

        private void loSwitchComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int externalLO;
            externalLO = loSwitchComboBox.SelectedIndex;
            frequencyReferenceSourceLabel.Visible = (externalLO == 0);
            frequencyReferenceSourceComboBox.Visible = (externalLO == 0);
            outputTerminalLabel.Visible = (externalLO == 0);
            outputTerminalComboBox.Visible = (externalLO == 0);
        }

        #endregion

        private void EnableControls(bool enabled)
        {
            startButton.Enabled = enabled;
            stopButton.Enabled = !enabled;
            resourceNameComboBox.Enabled = enabled;
            centerFrequencyNumeric.Enabled = enabled;
            powerLevelNumeric.Enabled = enabled;
            frequencyReferenceSourceComboBox.Enabled = enabled;
            outputTerminalComboBox.Enabled = enabled;
            loSwitchComboBox.Enabled = enabled;
            // Start the status checking timer
            rfsgStatusTimer.Enabled = !enabled;

            Application.DoEvents();
        }
    }
}
