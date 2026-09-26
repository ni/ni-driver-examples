using System;
using System.Collections.Generic;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.SingleToneGenerationIQDevice
{
    public partial class MainForm : Form
    {
        NIRfsg _rfsgSession;
        const double frequencyReferenceRate = 10E6;

        public MainForm()
        {
            InitializeComponent();

            // Enable controls on startup
            EnableControls(true);
            ConfigureRefClockComboBox();
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

        private void ConfigureRefClockComboBox()
        {
            List<KeyValuePair<string, RfsgFrequencyReferenceSource>> refClockValueList = new List<KeyValuePair<string, RfsgFrequencyReferenceSource>>();
            refClockValueList.Add(new KeyValuePair<string, RfsgFrequencyReferenceSource>("OnboardClock", RfsgFrequencyReferenceSource.OnboardClock));
            refClockValueList.Add(new KeyValuePair<string, RfsgFrequencyReferenceSource>("RefIn", RfsgFrequencyReferenceSource.ReferenceIn));
            refClockValueList.Add(new KeyValuePair<string, RfsgFrequencyReferenceSource>("PXI_Clk", RfsgFrequencyReferenceSource.PxiClock));
            refClockValueList.Add(new KeyValuePair<string, RfsgFrequencyReferenceSource>("ClkIn", RfsgFrequencyReferenceSource.ClockIn));
            referenceClockComboBox.DisplayMember = "Key";
            referenceClockComboBox.ValueMember = "Value";
            referenceClockComboBox.DataSource = refClockValueList;
            referenceClockComboBox.SelectedIndex = 0;
        }

        #region Program Functions

        void StartGeneration()
        {
            string resourceName;
            double iqPortFrequency, iqOutPortLevel;
            RfsgFrequencyReferenceSource referenceClockSource;
            RfsgRFPowerLevelType powerLevelType;
            RfsgWaveformGenerationMode waveformGenerationMode;
            RfsgOutputPort outputPort;
            try
            {
                // Read in all the control values 
                resourceName = resourceNameComboBox.Text;
                referenceClockSource = referenceClockComboBox.Text;
                powerLevelType = RfsgRFPowerLevelType.PeakPower;
                waveformGenerationMode = RfsgWaveformGenerationMode.ContinuousWave;
                outputPort = RfsgOutputPort.IQOut;
                iqPortFrequency = (double)iqPortFrequencyNumeric.Value;
                iqOutPortLevel = (double)iqOutPortLevelNumeric.Value;

                errorTextBox.Text = "No error.";
                Application.DoEvents();

                // Initialize the NIRfsg session
                _rfsgSession = new NIRfsg(resourceName, true, false);
                _rfsgSession.FrequencyReference.Configure(referenceClockSource, frequencyReferenceRate);
                _rfsgSession.RF.PowerLevelType = powerLevelType;
                _rfsgSession.Arb.GenerationMode = waveformGenerationMode;

                _rfsgSession.Arb.OutputPort = outputPort;
                // Configure IQOutPort CarrierFrequency, OutputPort and Level (Vpp)
                _rfsgSession.IQOutPort.CarrierFrequency = iqPortFrequency;
                // Set channel name as I or Q for 5645R, empty string for 5820
                _rfsgSession.IQOutPort[""].Level = iqOutPortLevel;

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
            double iqPortFrequency, iqOutPortLevel;
            try
            {
                // Stop the status checking timer 
                EnableControls(true);

                // Read in all the control values 
                iqPortFrequency = (double)iqPortFrequencyNumeric.Value;
                iqOutPortLevel = (double)iqOutPortLevelNumeric.Value;

                // Abort generation 
                _rfsgSession.Abort();

                _rfsgSession.Arb.OutputPort = RfsgOutputPort.IQOut;
                // Configure IQOutPort CarrierFrequency, OutputPort and Level (Vpp)
                _rfsgSession.IQOutPort.CarrierFrequency = iqPortFrequency;
                // Set channel name as I or Q for 5645R, empty string for 5820
                _rfsgSession.IQOutPort[""].Level = iqOutPortLevel;
                
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
            referenceClockComboBox.Enabled = enabled;

            Application.DoEvents();
        }
    }
}
