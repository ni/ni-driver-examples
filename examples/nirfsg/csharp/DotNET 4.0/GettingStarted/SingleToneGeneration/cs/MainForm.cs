//==================================================================================================
// Title        : Single Tone Generation
// Description  : This program demonstrates the use of niRFSG to generate a simple sine wave 
//			at a specified frequency and output gain.  The niRFSG.Abort function is used 
//			to quickly change from one configuration to another. 
//
//			Note: In order to run this example, the upconverter must be configured with 
//			an Arbitrary Waveform Generator. 
//			To do this, open Measurement & Automation Explorer, select the upconverter 
//			and click on properties.
//==================================================================================================

using System;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.SingleToneGeneration
{
    public partial class MainForm : Form
    {
        NIRfsg _rfsgSession;

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
            double frequency, power;
            try
            {
                // Read in all the control values 
                resourceName = resourceNameComboBox.Text;
                frequency = (double)frequencyNumeric.Value;
                power = (double)powerLevelNumeric.Value;

                errorTextBox.Text = "No error.";
                Application.DoEvents();

                // Initialize the NIRfsg session
                _rfsgSession = new NIRfsg(resourceName, true, false);

                // Subscribe to Rfsg warnings
                _rfsgSession.DriverOperation.Warning += new EventHandler<RfsgWarningEventArgs>(DriverOperation_Warning);

                // Configure the instrument 
                _rfsgSession.RF.Configure(frequency, power);

                // Initiate Generation 
                _rfsgSession.Initiate();

                // Disable all controls
                EnableControls(false);
            }
            catch (Exception ex)
            {
                ShowError("StartGeneration()", ex);
            }
        }

        void StopGeneration()
        {
            EnableControls(true);
            updateButton.Enabled = false;

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
                _rfsgSession.CheckGenerationStatus();
            }
            catch (Exception ex)
            {
                ShowError("CheckGeneration()", ex);
            }
        }

        void UpdateGeneration()
        {
            double frequency, power;
            try
            {
                // Stop the status checking timer 
                EnableControls(true);

                // Read in all the control values 
                frequency = (double)frequencyNumeric.Value;
                power = (double)powerLevelNumeric.Value;

                // Abort generation 
                _rfsgSession.Abort();

                // Configure the instrument 
                _rfsgSession.RF.Configure(frequency, power);

                // Initiate Generation 
                _rfsgSession.Initiate();

                // Start the status checking timer 
                EnableControls(false);

            }
            catch (Exception ex)
            {
                ShowError("UpdateGeneration()", ex);
            }
        }

        void ShowError(string functionName, Exception exception)
        {
            StopGeneration();
            errorTextBox.Text = "Error in " + functionName + ": " + exception.Message;
        }

        #endregion

        void DriverOperation_Warning(object sender, RfsgWarningEventArgs e)
        {
            // Display the rfsg warning
            errorTextBox.Text = e.Message;
        }

        #region Form Events
        private void updateButton_Click(object sender, System.EventArgs e)
        {
            UpdateGeneration();
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
            updateButton.Enabled = !enabled;
            stopButton.Enabled = !enabled;
            resourceNameComboBox.Enabled = enabled;
            // Start the status checking timer
            rfsgStatusTimer.Enabled = !enabled;
            
            Application.DoEvents();
        }
    }
}
