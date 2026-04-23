//==================================================================================================
// Title        : PowerSweep(565xand5673)
// Description  : This program demonstrates the use of niRFSG to generate a sine wave with a 
//			      power sweep and specified output frequency for the 565x and 5673. 
//
//			      Note: 
//			      The 565x and 5673 power sweep is different from the standard example because the
//			      565x and 5673 has the ability to make changes on-the-fly.  The example no longer uses
//			      abort and initiate when the power changes and instead, it now waits until 
//			      the output is settled.  This ensures that the signal is constantly outputted.
//==================================================================================================

using System;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.PowerSweep565xand5673
{
    public partial class MainForm : Form
    {
        NIRfsg _rfsgSession;
        double _stopPower, _currentPower, _powerIncrement;
        const double FrequencyReferenceRate = 10E6;
        public MainForm()
        {
            InitializeComponent();

            LoadRfsgDeviceNames();

            ConfigureClockSourceComboBox();
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

        #endregion

        #region Program Functions

        void StartGeneration()
        {
            string resourceName;
            double frequency;
            double startPower;
            int dwellTime;
            int numberOfSteps;
            RfsgFrequencyReferenceSource clockSource;
            try
            {
                // Read in all of the control values 
                resourceName = resourceNameComboBox.Text;
                frequency = (double)frequencyNumeric.Value;
                startPower = (double)startPowerNumeric.Value;
                _stopPower = (double)stopPowerNumeric.Value;
                numberOfSteps = (int)numberStepsNumeric.Value;
                dwellTime = (int)(dwellTimeNumeric.Value * 1000);
                clockSource = clockSourceComboBox.SelectedValue as RfsgFrequencyReferenceSource ?? RfsgFrequencyReferenceSource.FromString(clockSourceComboBox.Text);

                _currentPower = startPower;
                _powerIncrement = (_stopPower - startPower) / (numberOfSteps - 1);

                errorTextBox.Text = "No error.";
                Application.DoEvents();

                // Initialize the NIRfsg session
                _rfsgSession = new NIRfsg(resourceName, true, false);

                // subscribe to the warning events
                _rfsgSession.DriverOperation.Warning += new EventHandler<RfsgWarningEventArgs>(DriverOperation_Warning);

                // Configure center frequency and power level 
                _rfsgSession.RF.Configure(frequency, _currentPower);

                actualCurrentPowerTextBox.Text = _currentPower.ToString();

                // Configure the generation mode (continuous wave) 
                _rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.ContinuousWave;

                // Configure the clock source 
                _rfsgSession.FrequencyReference.Configure(clockSource, FrequencyReferenceRate);

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
                    // Set the power on the fly 
                    _rfsgSession.RF.PowerLevel = _currentPower;
                    actualCurrentPowerTextBox.Text = _currentPower.ToString();

                    // Wait for the output to settle 
                    _rfsgSession.Utility.WaitUntilSettled(10000);
                }
            }
            catch (Exception ex)
            {
                ShowError("SetNextSweepPower()", ex);
            }
        }

        void DriverOperation_Warning(object sender, RfsgWarningEventArgs e)
        {
            // Display any Rfsg warnings
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
            frequencyNumeric.Enabled = enabled;
            startPowerNumeric.Enabled = enabled;
            stopPowerNumeric.Enabled = enabled;
            numberStepsNumeric.Enabled = enabled;
            dwellTimeNumeric.Enabled = enabled;
            clockSourceComboBox.Enabled = enabled;
            rfsgStatusTimer.Enabled = !enabled;

            Application.DoEvents();
        }
    }
}
