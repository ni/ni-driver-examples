//==================================================================================================
// Title        : Export Start Trigger
// Description  : This program demonstrates the use of niRFSG to generate a simple sine wave at
//			a specified frequency and output power and export a digital trigger at the start
//			of generation.
//
//			Note: In order to run this example, the upconverter must be configured with 
//			an Arbitrary Waveform Generator. 
//			To do this, open Measurement & Automation Explorer, select the upconverter 
//			and click on properties.
//==================================================================================================

using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Collections;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.ExportStartTrigger
{
    public partial class MainForm : Form
    {
        NIRfsg _rfsgSession;

        public MainForm()
        {
            InitializeComponent();

            LoadRfsgDeviceNames();

            ConfigureExportTerminalComboBox();
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

        private void ConfigureExportTerminalComboBox()
        {
            var exportTerminalValueList = new List<DictionaryEntry>();
            exportTerminalValueList.Add(new DictionaryEntry("Do Not Export", RfsgStartTriggerExportedOutputTerminal.DoNotExport));
            exportTerminalValueList.Add(new DictionaryEntry("PFI0", RfsgStartTriggerExportedOutputTerminal.Pfi0));
            exportTerminalValueList.Add(new DictionaryEntry("PFI1", RfsgStartTriggerExportedOutputTerminal.Pfi1));
            exportTerminalValueList.Add(new DictionaryEntry("PFI4", RfsgStartTriggerExportedOutputTerminal.Pfi4));
            exportTerminalValueList.Add(new DictionaryEntry("PFI5", RfsgStartTriggerExportedOutputTerminal.Pfi5));
            exportTerminalValueList.Add(new DictionaryEntry("PXI_Trig0", RfsgStartTriggerExportedOutputTerminal.PxiTriggerLine0));
            exportTerminalValueList.Add(new DictionaryEntry("PXI_Trig1", RfsgStartTriggerExportedOutputTerminal.PxiTriggerLine1));
            exportTerminalValueList.Add(new DictionaryEntry("PXI_Trig2", RfsgStartTriggerExportedOutputTerminal.PxiTriggerLine2));
            exportTerminalValueList.Add(new DictionaryEntry("PXI_Trig3", RfsgStartTriggerExportedOutputTerminal.PxiTriggerLine3));
            exportTerminalValueList.Add(new DictionaryEntry("PXI_Trig4", RfsgStartTriggerExportedOutputTerminal.PxiTriggerLine4));
            exportTerminalValueList.Add(new DictionaryEntry("PXI_Trig5", RfsgStartTriggerExportedOutputTerminal.PxiTriggerLine5));
            exportTerminalValueList.Add(new DictionaryEntry("PXI_Trig6", RfsgStartTriggerExportedOutputTerminal.PxiTriggerLine6));

            exportTerminalComboBox.DataSource = exportTerminalValueList;
            exportTerminalComboBox.DisplayMember = "Key";
            exportTerminalComboBox.ValueMember = "Value";
            exportTerminalComboBox.SelectedValue = RfsgStartTriggerExportedOutputTerminal.DoNotExport;

        }

        #endregion

        #region Program Functions

        void StartGeneration()
        {
            string resourceName;
            double frequency;
            double power;
            RfsgStartTriggerExportedOutputTerminal outputTerminal;
            try
            {
                // Read in all of the control values 
                resourceName = resourceNameComboBox.Text;
                frequency = (double)frequencyNumeric.Value;
                power = (double)powerLevelNumeric.Value;
                outputTerminal = exportTerminalComboBox.SelectedValue as RfsgStartTriggerExportedOutputTerminal ?? RfsgStartTriggerExportedOutputTerminal.FromString(exportTerminalComboBox.Text);

                errorTextBox.Text = "No error.";
                Application.DoEvents();
                // Initialize the NIRfsg session
                _rfsgSession = new NIRfsg(resourceName, true, false);

                // Subscribe to Rfsg warnings
                _rfsgSession.DriverOperation.Warning += new EventHandler<RfsgWarningEventArgs>(DriverOperation_Warning);

                // Configure the instrument 
                _rfsgSession.RF.Configure(frequency, power);

                // Configure the instrument to export trigger at start of generation 
                _rfsgSession.Triggers.StartTrigger.ExportedOutputTerminal = outputTerminal;

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
            frequencyNumeric.Enabled = enabled;
            powerLevelNumeric.Enabled = enabled;
            exportTerminalComboBox.Enabled = enabled;
            rfsgStatusTimer.Enabled = !enabled;

            Application.DoEvents();
        }
    }
}
