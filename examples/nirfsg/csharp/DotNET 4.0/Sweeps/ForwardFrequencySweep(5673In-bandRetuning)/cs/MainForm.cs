//==================================================================================================
// Title        : Forward Frequency Sweep (5673 In-band Retuning)
// Description  : This example demonstrates how to generate a frequency sweep 
//			at a specified power level.
//
//			Note: In order to run this example, you must have your 
//			NI PXIe-5673 configured to work together with a NI PXIe-5450(AWG) 
//			and a NI PXI-5652 (LO).
//			To do this, open the Measurements & Automation Explorer, 
//			select the NI PXIe-5611 and click on properties.
//==================================================================================================

using System;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.ForwardFrequencySweep5673InbandRetuning
{
    public partial class MainForm : Form
    {
        NIRfsg _rfsgSession;
        double _stopFrequency, _currentFrequency, _frequencyIncrement, _inBandSpan;
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
            loopBandwidthComboBox.SelectedIndex = 2;
        }

        #endregion

        #region Program Functions

        void StartGeneration()
        {
            string resourceName;
            double power;
            double startFrequency;
            int dwellTime;
            double upconverterCenterFrequency;
            RfsgLoopBandwidth upconverterLoopBandwidth;
            RfsgFrequencyReferenceSource clockSource;

            int numberOfSteps;
            try
            {
                // Read in all of the control values 
                resourceName = resourceNameComboBox.Text;
                clockSource = clockSourceComboBox.SelectedValue as RfsgFrequencyReferenceSource ?? RfsgFrequencyReferenceSource.FromString(clockSourceComboBox.Text);
                power = (double)powerLevelNumeric.Value;
                startFrequency = (double)startFrequencyNumeric.Value;
                _stopFrequency = (double)stopFrequencyNumeric.Value;
                _inBandSpan = (double)deviceBandwidthToUseNumeric.Value;
                upconverterLoopBandwidth = (RfsgLoopBandwidth)Enum.Parse(typeof(RfsgLoopBandwidth), (string)loopBandwidthComboBox.SelectedItem);
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

                // Configure the instrument for continuous wave generation
                _rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.ContinuousWave;

                // Configure the reference clock for the instrument
                _rfsgSession.FrequencyReference.Configure(clockSource, FrequencyReferenceRate);

                // Configure the instrument for power setting and start frequency
                _rfsgSession.RF.Configure(startFrequency, power);

                // Default signal bandwidth in Continuous Wave mode is 100
                upconverterCenterFrequency = ((_inBandSpan - 100) / 2) + startFrequency;

                // Set the upconverter center frequency and upconverter loop bandwidth-
                // By setting the upconverter center frequency property, the in-band
                // retuning feature is enabled.
                _rfsgSession.RF.Upconverter.CenterFrequency = upconverterCenterFrequency;
                _rfsgSession.RF.LocalOscillator.LoopBandwidth = upconverterLoopBandwidth;

                // Initiate Generation 
                _rfsgSession.Initiate();

                // Output Current Frequency 
                actualCurrentFrequencyTextBox.Text = startFrequency.ToString();

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
                    // checkWarn is omitted to ensure that the session is closed even in
                    // the case of an error occurring. 

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
                double upconverterCenterFrequency;

                // Obtain Current Upconverter Center Frequency 

                upconverterCenterFrequency = _rfsgSession.RF.Upconverter.CenterFrequency;

                _currentFrequency += _frequencyIncrement;

                // If we are beyond the last frequency, it is time to stop. 
                if (_currentFrequency > _stopFrequency)
                {
                    StopGeneration();
                }
                else
                {
                    if ((_currentFrequency - ((_inBandSpan - 100) / 2)) > upconverterCenterFrequency)
                    {
                        // Update Upconverter Center Frequency 
                        // Setting the upconverter center frequency will not change the LO immediately.
                        // This will happen when the frequency property changes.  Thus the upconverter
                        // center frequency should be set first.
                        upconverterCenterFrequency = _currentFrequency + ((_inBandSpan - 100) / 2);

                        _rfsgSession.RF.Upconverter.CenterFrequency = upconverterCenterFrequency;

                        // Set the frequency on the fly 
                        _rfsgSession.RF.Frequency = _currentFrequency;

                        _rfsgSession.Utility.WaitUntilSettled(10000);
                    }
                    else
                    {
                        // Set the frequency on the fly 
                        _rfsgSession.RF.Frequency = _currentFrequency;
                    }

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
        private void stopButton_Click(object sender, System.EventArgs e)
        {
            StopGeneration();
        }

        private void startButton_Click(object sender, System.EventArgs e)
        {
            StartGeneration();
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
            rfsgStatusTimer.Enabled = !enabled;
            startButton.Enabled = enabled;
            stopButton.Enabled = !enabled;
            resourceNameComboBox.Enabled = enabled;
            clockSourceComboBox.Enabled = enabled;
            powerLevelNumeric.Enabled = enabled;
            startFrequencyNumeric.Enabled = enabled;
            stopFrequencyNumeric.Enabled = enabled;
            deviceBandwidthToUseNumeric.Enabled = enabled;
            loopBandwidthComboBox.Enabled = enabled;
            numberStepsNumeric.Enabled = enabled;
            dwellTimeNumeric.Enabled = enabled;

            Application.DoEvents();
        }
    }
}
