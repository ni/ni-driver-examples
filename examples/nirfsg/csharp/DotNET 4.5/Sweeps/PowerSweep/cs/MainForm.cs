//==================================================================================================
// Title        : Power Sweep
// Description  : This program demonstrates the use of niRFSG to generate a sine wave with a 
//			power sweep and specified output frequency. 
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

namespace NationalInstruments.Examples.PowerSweep
{
    public partial class MainForm : Form
    {
        NIRfsg _rfsgSession;
        double _stopPower, _currentPower, _powerIncrement;

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
            string resourceName;
            double frequency;
            double startPower;
            double maxPower;
            int dwellTime;
            int numberOfSteps;
            bool holdAttenuators;
            try
            {
                // Read in all of the control values 
                resourceName = resourceNameComboBox.Text;
                frequency = (double)frequencyNumeric.Value;
                startPower = (double)startPowerNumeric.Value;
                _stopPower = (double)stopPowerNumeric.Value;
                numberOfSteps = (int)numberStepsNumeric.Value;
                dwellTime = (int)(dwellTimeNumeric.Value * 1000);
                holdAttenuators = holdAttenuatorsCheckBox.Checked;

                _currentPower = startPower;
                _powerIncrement = (_stopPower - startPower) / (numberOfSteps - 1);
                if (startPower < _stopPower)
                    maxPower = _stopPower;
                else
                    maxPower = startPower;

                errorTextBox.Text = "No error.";
                Application.DoEvents();

                // Initialize the NIRfsg session
                _rfsgSession = new NIRfsg(resourceName, true, false);
                _rfsgSession.DriverOperation.Warning += new EventHandler<RfsgWarningEventArgs>(DriverOperation_Warning);

                // Configure center frequency and power level 
                _rfsgSession.RF.Configure(frequency, _currentPower);

                actualCurrentPowerTextBox.Text = _currentPower.ToString();

                // Configure attenuator hold mode attributes 
                if (holdAttenuators)
                {
                    _rfsgSession.RF.Advanced.AttenuatorHoldEnabled = holdAttenuators;
                    _rfsgSession.RF.Advanced.AttenuatorHoldMaximumPower = maxPower;
                }

                // Initiate Generation 
                _rfsgSession.Initiate();

                // Start the frequency sweep timer 
                rfsgStatusTimer.Interval = dwellTime;
                EnableControls(false);
                stopButton.Focus();
            }
            catch (Exception ex)
            {
                ShowError("StartGeneration()", ex);
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

        void SetNextSweepPower()
        {
            try
            {
                _currentPower += _powerIncrement;

                // If we are beyond the last power, it is time to stop. 
                if ((_powerIncrement >= 0 && _currentPower > _stopPower) || (_powerIncrement < 0 && _currentPower < _stopPower))
                {
                    StopGeneration();
                }
                else
                {
                    // Abort current generation 
                    _rfsgSession.Abort();

                    // Set the power on the fly 
                    _rfsgSession.RF.PowerLevel = _currentPower;
                    actualCurrentPowerTextBox.Text = _currentPower.ToString();

                    // Restart generation 
                    _rfsgSession.Initiate();
                }
            }
            catch (Exception ex)
            {
                ShowError("SetNextSweepPower()", ex);
            }
        }

        void DriverOperation_Warning(object sender, RfsgWarningEventArgs e)
        {
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
            SetNextSweepPower();
        }
        #endregion

        private void EnableControls(bool enabled)
        {
            startButton.Enabled = enabled;
            stopButton.Enabled = !enabled;
            resourceNameComboBox.Enabled = enabled;
            holdAttenuatorsCheckBox.Enabled = enabled;
            frequencyNumeric.Enabled = enabled;
            startPowerNumeric.Enabled = enabled;
            stopPowerNumeric.Enabled = enabled;
            numberStepsNumeric.Enabled = enabled;
            dwellTimeNumeric.Enabled = enabled;
            rfsgStatusTimer.Enabled = !enabled;

            Application.DoEvents();
        }
    }
}
