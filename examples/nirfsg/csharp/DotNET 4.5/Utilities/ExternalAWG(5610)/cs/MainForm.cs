//==================================================================================================
// Title        : External AWG (5610)
// Description  : This example demonstrates how to use NI-RFSG in Upconverter
//			Only Mode. In this mode, the driver operates the NI PXI-5610
//			using NI-RFSG without having an AWG.
//==================================================================================================

using System;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.ExternalAWG5610
{
    public partial class MainForm : Form
    {
        NIRfsg _rfsgSession;
        const int RfsgMaxSettlingTime = 10000;

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

        #region Program Functions

        void StartGeneration()
        {
            string optionsString = "DriverSetup = UpconverterOnly:1";
            string resourceName;
            double frequency;
            double gain;
            double bandwidth;
            double arbCarrierFrequency;
            double actualFrequency;
            double actualGain;
            try
            {
                // Read in all the control values 
                resourceName = resourceNameComboBox.Text;
                frequency = (double)frequencyNumeric.Value;
                gain = (double)gainNumeric.Value;
                bandwidth = (double)bandwidthNumeric.Value;
                arbCarrierFrequency = (double)arbCarrierNumeric.Value;

                errorTextBox.Text = "No error.";

                // Initialize the NIRfsg session
                _rfsgSession = new NIRfsg(resourceName, true, false, optionsString);

                // Subscribe to Rfsg warnings
                _rfsgSession.DriverOperation.Warning += new EventHandler<RfsgWarningEventArgs>(DriverOperation_Warning);

                // Basic configuration 
                _rfsgSession.RF.Upconverter.CenterFrequency = frequency;
                _rfsgSession.RF.Upconverter.Gain = gain;
                _rfsgSession.Arb.SignalBandwidth = bandwidth;
                _rfsgSession.Arb.CarrierFrequency = arbCarrierFrequency;

                // Commit settings 
                _rfsgSession.Utility.Commit();
                _rfsgSession.Utility.WaitUntilSettled(RfsgMaxSettlingTime);

                // Retrieve some information 
                actualFrequency = _rfsgSession.RF.Upconverter.CenterFrequency;
                actualGain = _rfsgSession.RF.Upconverter.Gain;

                actualFrequencyTextBox.Text = actualFrequency.ToString();
                actualGainTextBox.Text = actualGain.ToString();

                // Unsubscribe from warning events
                _rfsgSession.DriverOperation.Warning -= DriverOperation_Warning;

                // Close NIRfsg session
                _rfsgSession.Close();
            }
            catch (Exception ex)
            {
                ShowError("StartGeneration()", ex);
            }
        }

        void StopGeneration()
        {
            try
            {
                // Close NIRfsg session
                if (_rfsgSession != null)
                {
                    _rfsgSession.Close();
                }
                _rfsgSession = null;
            }
            catch (Exception ex)
            {
                errorTextBox.Text = "Error in StopGeneration(): " + ex.Message;
            }
        }
        
        void DriverOperation_Warning(object sender, RfsgWarningEventArgs e)
        {
            // Display the rfsg warning
            errorTextBox.Text = e.Message;
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

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopGeneration();
        }

        #endregion
    }
}
