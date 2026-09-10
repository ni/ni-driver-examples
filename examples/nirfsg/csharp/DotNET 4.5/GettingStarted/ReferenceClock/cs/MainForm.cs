//==================================================================================================
// Title        : Reference Clock
// Description  : This program demonstrates the use of NIRfsg.FrequencyReference.Configure
//			to route the reference clock source during a simple sine wave 
//			generation. 
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

namespace NationalInstruments.Examples.ReferenceClock
{
    public partial class MainForm : Form
    {
        NIRfsg _rfsgSession;
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
            double frequency, power;
            RfsgFrequencyReferenceSource clockSource;
            try
            {
                // Read in all of the control values 
                resourceName = resourceNameComboBox.Text;
                frequency = (double)frequencyNumeric.Value;
                power = (double)powerLevelNumeric.Value;
                clockSource = clockSourceComboBox.SelectedValue as RfsgFrequencyReferenceSource ?? RfsgFrequencyReferenceSource.FromString(clockSourceComboBox.Text);

                errorTextBox.Text = "No error.";
                Application.DoEvents();

                // Initialize the NIRfsg session
                _rfsgSession = new NIRfsg(resourceName, true, false);

                // Subscribe to Rfsg warnings
                _rfsgSession.DriverOperation.Warning += new EventHandler<RfsgWarningEventArgs>(DriverOperation_Warning);

                // Configure the instrument 
                _rfsgSession.RF.Configure(frequency, power);
                _rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.ContinuousWave;
                _rfsgSession.FrequencyReference.Configure(clockSource, FrequencyReferenceRate);

                // Initiate Generation 
                _rfsgSession.Initiate();

                // Start the status checking timer 
                EnableControls(false);

                // Activate stop button 
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
            clockSourceComboBox.Enabled = enabled;
            rfsgStatusTimer.Enabled = !enabled;

            Application.DoEvents();
        }
    }
}
