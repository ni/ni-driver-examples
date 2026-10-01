//==================================================================================================
// Title        : Marker Events
// Description  : This example demonstrates how to generate marker events. Marker events are 
//			digital pulses that are generated on specific samples in the waveform. 
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

namespace NationalInstruments.Examples.MarkerEvents
{
    public partial class MainForm : Form
    {
        NIRfsg _rfsgSession;
        const double ArbSignalBandwidth = 1;
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
            exportTerminalValueList.Add(new DictionaryEntry("Do Not Export", RfsgMarkerEventExportedOutputTerminal.DoNotExport));
            exportTerminalValueList.Add(new DictionaryEntry("PFI0", RfsgMarkerEventExportedOutputTerminal.Pfi0));
            exportTerminalValueList.Add(new DictionaryEntry("PFI1", RfsgMarkerEventExportedOutputTerminal.Pfi1));
            exportTerminalValueList.Add(new DictionaryEntry("PFI4", RfsgMarkerEventExportedOutputTerminal.Pfi4));
            exportTerminalValueList.Add(new DictionaryEntry("PFI5", RfsgMarkerEventExportedOutputTerminal.Pfi5));
            exportTerminalValueList.Add(new DictionaryEntry("PXI_Trig0", RfsgMarkerEventExportedOutputTerminal.PxiTriggerLine0));
            exportTerminalValueList.Add(new DictionaryEntry("PXI_Trig1", RfsgMarkerEventExportedOutputTerminal.PxiTriggerLine1));
            exportTerminalValueList.Add(new DictionaryEntry("PXI_Trig2", RfsgMarkerEventExportedOutputTerminal.PxiTriggerLine2));
            exportTerminalValueList.Add(new DictionaryEntry("PXI_Trig3", RfsgMarkerEventExportedOutputTerminal.PxiTriggerLine3));
            exportTerminalValueList.Add(new DictionaryEntry("PXI_Trig4", RfsgMarkerEventExportedOutputTerminal.PxiTriggerLine4));
            exportTerminalValueList.Add(new DictionaryEntry("PXI_Trig5", RfsgMarkerEventExportedOutputTerminal.PxiTriggerLine5));
            exportTerminalValueList.Add(new DictionaryEntry("PXI_Trig6", RfsgMarkerEventExportedOutputTerminal.PxiTriggerLine6));

            exportTerminalComboBox.DataSource = exportTerminalValueList;
            exportTerminalComboBox.DisplayMember = "Key";
            exportTerminalComboBox.ValueMember = "Value";
            exportTerminalComboBox.SelectedValue = RfsgMarkerEventExportedOutputTerminal.DoNotExport;
        }

       
        #endregion

        #region Program Functions

        void StartGeneration()
        {
            string resourceName;
            int kNumberOfSamples = 64;
            double frequency;
            double power;
            double iqRate;
            RfsgRFPowerLevelType powerLevelType;
            string script;
            RfsgMarkerEventExportedOutputTerminal outputTerminal;
            double actualIQRate;
            double[] iData;
            double[] qData;
            int waveformItr;
            try
            {
                // Read in all of the control values 
                resourceName = resourceNameComboBox.Text;
                frequency = (double)frequencyNumeric.Value;
                power = (double)powerLevelNumeric.Value;
                iqRate = (double)iqRateNumeric.Value;
                script = scriptTextBox.Text;
                outputTerminal = exportTerminalComboBox.SelectedValue as RfsgMarkerEventExportedOutputTerminal ?? RfsgMarkerEventExportedOutputTerminal.FromString(exportTerminalComboBox.Text);
                powerLevelType = RfsgRFPowerLevelType.PeakPower;

                // Generate I and Q data 
                iData = new double[kNumberOfSamples];
                qData = new double[kNumberOfSamples];
                for (waveformItr = 0; waveformItr < kNumberOfSamples; waveformItr++)
                {
                    iData[waveformItr] = 1.0;
                    qData[waveformItr] = 0.0;
                }

                errorTextBox.Text = "No error.";
                Application.DoEvents();

                // Initialize the session 
                _rfsgSession = new NIRfsg(resourceName, true, false);

                // Subscribe to Rfsg warnings
                _rfsgSession.DriverOperation.Warning += new EventHandler<RfsgWarningEventArgs>(DriverOperation_Warning);

                // Configure the instrument 
                _rfsgSession.RF.Configure(frequency, power);

                // Configure the power level type 
                _rfsgSession.RF.PowerLevelType = powerLevelType;

                // Configure the generation mode to Script 
                _rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script;

                // Export the marker event to the desired output terminal 
                _rfsgSession.DeviceEvents.MarkerEvents[0].ExportedOutputTerminal = outputTerminal;

                // Configure the IQ rate of the waveforms 
                _rfsgSession.Arb.IQRate = iqRate;

                // Configure the signal bandwidth 
                _rfsgSession.Arb.SignalBandwidth = ArbSignalBandwidth;

                // Get the actual IQ rate, and populate the GUI output 
                actualIQRate = _rfsgSession.Arb.IQRate;
                actualIQRateTextBox.Text = actualIQRate.ToString();


                // Write the DC arb waveform 
                _rfsgSession.Arb.WriteWaveform("waveformWithMarkers", iData, qData);

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
            exportTerminalComboBox.Enabled = enabled;
            rfsgStatusTimer.Enabled = !enabled;

            Application.DoEvents();
        }
    }
}
