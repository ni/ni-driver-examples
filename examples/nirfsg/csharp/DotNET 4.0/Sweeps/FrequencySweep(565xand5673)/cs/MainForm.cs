//==================================================================================================
// Title        : FrequencySweep(565xand5673)
// Description  : This program demonstrates the use of niRFSG to generate a sine wave with a 
//			frequency sweep and specified output power for the 565x and 5673. 
//
//			Note: 
//			The 565x and 5673 frequency sweep is different from the standard example because
//			the 565x and 5673 has the ability to make changes on-the-fly.  The example no longer
//			uses abort and initiate when the frequency changes and instead, it now waits until
//			the output is settled. This ensures that the signal is constantly outputted.
//==================================================================================================

using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Collections;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.FrequencySweep565xand5673
{
    public partial class MainForm : Form
    {
        NIRfsg _rfsgSession;
        double _stopFrequency, _currentFrequency, _frequencyIncrement;
        const double FrequencyReferenceRate = 10E6;

        public MainForm()
        {
            InitializeComponent();

            LoadRfsgDeviceNames();

            ConfigureClockSourceComboBox();
            ConfigureLoopBandwidthComboBox();
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

        private void ConfigureLoopBandwidthComboBox()
        {
            loopBandwidthComboBox.Items.AddRange(Enum.GetNames(typeof(RfsgLoopBandwidth)));
            loopBandwidthComboBox.SelectedIndex = 0;
        }

        #endregion

        #region Program Functions

        void StartGeneration()
        {
            string resourceName;
            double power, startFrequency;
            int numberOfSteps, dwellTime;
            RfsgFrequencyReferenceSource clockSource;
            RfsgLoopBandwidth loopBandwidth;
            try
            {
                // Read in all of the control values 
                resourceName = resourceNameComboBox.Text;
                power = (double)powerLevelNumeric.Value;
                startFrequency = (double)startFrequencyNumeric.Value;
                _stopFrequency = (double)stopFrequencyNumeric.Value;
                numberOfSteps = (int)numberStepsNumeric.Value;
                dwellTime = (int)(dwellTimeNumeric.Value * 1000);
                clockSource = clockSourceComboBox.SelectedValue as RfsgFrequencyReferenceSource ?? RfsgFrequencyReferenceSource.FromString(clockSourceComboBox.Text);
                loopBandwidth = (RfsgLoopBandwidth)Enum.Parse(typeof(RfsgLoopBandwidth), (string)loopBandwidthComboBox.SelectedItem);

                _currentFrequency = startFrequency;
                _frequencyIncrement = (_stopFrequency - startFrequency) / (numberOfSteps - 1);

                errorTextBox.Text = "No error.";
                Application.DoEvents();

                // Initialize the NIRfsg session
                _rfsgSession = new NIRfsg(resourceName, true, false);

                // Subscribe to Rfsg warnings
                _rfsgSession.DriverOperation.Warning += new EventHandler<RfsgWarningEventArgs>(DriverOperation_Warning);

                // Configure the power level and the starting frequency 
                _rfsgSession.RF.Configure(_currentFrequency, power);
                actualCurrentFrequencyTextBox.Text = _currentFrequency.ToString();

                // Configure generation mode (continuous wave) 
                _rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.ContinuousWave;

                // Configure the clock source 
                _rfsgSession.FrequencyReference.Configure(clockSource, FrequencyReferenceRate);

                // Configure the loop bandwidth 
                _rfsgSession.RF.LocalOscillator.LoopBandwidth = loopBandwidth;

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

        void SetNextSweepFrequency()
        {
            try
            {
                _currentFrequency += _frequencyIncrement;

                // If we are beyond the last frequency, it is time to stop. 
                if ((_frequencyIncrement >= 0 && _currentFrequency > _stopFrequency) ||
                    (_frequencyIncrement < 0 && _currentFrequency < _stopFrequency))
                {
                    StopGeneration();
                }
                else
                {
                    // Set the frequency on the fly 
                    _rfsgSession.RF.Frequency = _currentFrequency;

                    // Wait for the output to settle after making the change 
                    _rfsgSession.Utility.WaitUntilSettled(10000);

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
            clockSourceComboBox.Enabled = enabled;
            loopBandwidthComboBox.Enabled = enabled;
            rfsgStatusTimer.Enabled = !enabled;

            Application.DoEvents();
        }
    }
}
