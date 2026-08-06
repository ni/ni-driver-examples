//==================================================================================================
// Title        : Frequency Sweep
// Description  : This program demonstrates the use of niRFSG to generate a sine wave with a 
//			frequency sweep and specified output power. 
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

namespace NationalInstruments.Examples.FrequencySweep
{
    public partial class MainForm : Form
    {
        NIRfsg _rfsgSession;
        static double _stopFrequency, _currentFrequency, _frequencyIncrement;

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
            double power, startFrequency;
            int dwellTime;
            int numberOfSteps;
            try
            {
                // Read in all of the control values 
                resourceName = resourceNameComboBox.Text;
                power = (double)powerLevelNumeric.Value;
                startFrequency = (double)startFrequencyNumeric.Value;
                _stopFrequency = (double)stopFrequencyNumeric.Value;
                numberOfSteps = (int)numberStepsNumeric.Value;
                dwellTime = (int)(dwellTimeNumeric.Value * 1000);

                _currentFrequency = startFrequency;
                _frequencyIncrement = (_stopFrequency - startFrequency) / (numberOfSteps - 1);

                errorTextBox.Text = "No error.";
                Application.DoEvents();

                // Initialize the NIRfsg session
                _rfsgSession = new NIRfsg(resourceName, true, false);
                // Subscribe to Rfsg warnings
                _rfsgSession.DriverOperation.Warning += new EventHandler<RfsgWarningEventArgs>(DriverOperation_Warning);


                // Configure the instrument for power setting
                _rfsgSession.RF.PowerLevel = power;

                // Set the start frequency 
                _rfsgSession.RF.Frequency = _currentFrequency;
                actualCurrentFrequencyTextBox.Text = _currentFrequency.ToString();

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

        void DriverOperation_Warning(object sender, RfsgWarningEventArgs e)
        {
            // Display the rfsg warning
            errorTextBox.Text = e.Message;
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

        void SetNextSweepFrequency()
        {
            try
            {
                _currentFrequency += _frequencyIncrement;

                // If we are beyond the last frequency, it is time to stop. 
                if ((_frequencyIncrement >= 0 && _currentFrequency > _stopFrequency) || (_frequencyIncrement < 0 && _currentFrequency < _stopFrequency))
                {
                    StopGeneration();
                }
                else
                {
                    // Abort current generation 
                    _rfsgSession.Abort();

                    // Set the frequency on the fly 
                    _rfsgSession.RF.Frequency = _currentFrequency;

                    // Restart generation 
                    _rfsgSession.Initiate();

                    actualCurrentFrequencyTextBox.Text = _currentFrequency.ToString();
                    Application.DoEvents();
                }
            }
            catch (Exception ex)
            {
                ShowError("SetNextSweepFrequency()", ex);
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
            SetNextSweepFrequency();
        }
        #endregion

        private void EnableControls(bool enabled)
        {
            startButton.Enabled = enabled;
            stopButton.Enabled = !enabled;
            resourceNameComboBox.Enabled = enabled;
            powerLevelNumeric.Enabled = enabled;
            startFrequencyNumeric.Enabled = enabled;
            stopFrequencyNumeric.Enabled = enabled;
            numberStepsNumeric.Enabled = enabled;
            dwellTimeNumeric.Enabled = enabled;
            rfsgStatusTimer.Enabled = !enabled;

            Application.DoEvents();
        }
    }
}
