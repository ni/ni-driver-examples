//==================================================================================================
// Title        : 5673 Synchronization (TClk,SharedLO,Script)
// Description  : This example demonstrates how to use NI-TClk to synchronize the start and script triggers of multiple NI 5673 devices      
//                that share a local oscillator (LO). The master NI 5673 has an LO and arbitrary waveform generator (AWG). Slave NI   
//                5673 devices should be configured in Measurement and Automation Explorer (MAX) to use an external LO.
//==================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;
using NationalInstruments.ModularInstruments.SystemServices.TimingServices;

namespace NationalInstruments.Examples.Synchronization5673TClockSharedLOScript
{
    public partial class MainForm : Form
    {
        NIRfsg _rfsgMasterSession;
        NIRfsg[] _rfsgSlaveSessions;
        int _numberOfSlaveResources;
        TClock _tClockSession;
        const double FrequencyReferenceRate = 10E6;
        const int NumberOfSamples = 50000;
        const int SamplesPerCycle = 5000;
        double _frequency, _power, _iqRate;
        double[] _offsetIData, _offsetQData, _iData, _qData;
        string _script;

        public MainForm()
        {
            InitializeComponent();
            LoadMasterRfsgDeviceNames();
            ConfigureSlaveReferenceClockOutputTerminalComboBox();
            ConfigureMasterReferenceClockOutputTerminalComboBox();
            ConfigureSlaveReferenceClockClockSourceComboBox();
            ConfigureMasterReferenceClockClockSourceComboBox();
        }

        #region UI Initial Value Config Section

        private void LoadMasterRfsgDeviceNames()
        {
            ModularInstrumentsSystem modularInstrumentsSystem = new ModularInstrumentsSystem("NI-Rfsg");
            foreach (DeviceInfo device in modularInstrumentsSystem.DeviceCollection)
                masterRfsgResourceNameComboBox.Items.Add(device.Name);
            if (modularInstrumentsSystem.DeviceCollection.Count > 0)
            {
                masterRfsgResourceNameComboBox.SelectedIndex = 0;
                slaveRfsgResourceNamesTextBox.Text = modularInstrumentsSystem.DeviceCollection[0].Name;
                if (modularInstrumentsSystem.DeviceCollection.Count > 1)
                {
                    slaveRfsgResourceNamesTextBox.Text = modularInstrumentsSystem.DeviceCollection[1].Name;
                }
            }
        }

        private void ConfigureSlaveReferenceClockOutputTerminalComboBox()
        {
            var referenceClockOutputTerminalValueList = new List<DictionaryEntry>();
            referenceClockOutputTerminalValueList.Add(new DictionaryEntry("ClkOut", RfsgFrequencyReferenceExportedOutputTerminal.ClockOut));
            referenceClockOutputTerminalValueList.Add(new DictionaryEntry("Do Not Export", RfsgFrequencyReferenceExportedOutputTerminal.DoNotExport));
            referenceClockOutputTerminalValueList.Add(new DictionaryEntry("RefOut", RfsgFrequencyReferenceExportedOutputTerminal.ReferenceOut));
            referenceClockOutputTerminalValueList.Add(new DictionaryEntry("RefOut2", RfsgFrequencyReferenceExportedOutputTerminal.ReferenceOut2));
            slaveReferenceClockOutputTerminalComboBox.DataSource = referenceClockOutputTerminalValueList;
            slaveReferenceClockOutputTerminalComboBox.DisplayMember = "Key";
            slaveReferenceClockOutputTerminalComboBox.ValueMember = "Value";
            slaveReferenceClockOutputTerminalComboBox.SelectedValue = RfsgFrequencyReferenceExportedOutputTerminal.ClockOut;
        }

        private void ConfigureMasterReferenceClockOutputTerminalComboBox()
        {
            var referenceClockOutputTerminalValueList = new List<DictionaryEntry>();
            referenceClockOutputTerminalValueList.Add(new DictionaryEntry("ClkOut", RfsgFrequencyReferenceExportedOutputTerminal.ClockOut));
            referenceClockOutputTerminalValueList.Add(new DictionaryEntry("Do Not Export", RfsgFrequencyReferenceExportedOutputTerminal.DoNotExport));
            referenceClockOutputTerminalValueList.Add(new DictionaryEntry("RefOut", RfsgFrequencyReferenceExportedOutputTerminal.ReferenceOut));
            referenceClockOutputTerminalValueList.Add(new DictionaryEntry("RefOut2", RfsgFrequencyReferenceExportedOutputTerminal.ReferenceOut2));
            masterReferenceClockOutputTerminalComboBox.DataSource = referenceClockOutputTerminalValueList;
            masterReferenceClockOutputTerminalComboBox.DisplayMember = "Key";
            masterReferenceClockOutputTerminalComboBox.ValueMember = "Value";
            masterReferenceClockOutputTerminalComboBox.SelectedValue = RfsgFrequencyReferenceExportedOutputTerminal.ClockOut;
        }

        private void ConfigureSlaveReferenceClockClockSourceComboBox()
        {
            var referenceourceValueList = new List<DictionaryEntry>();
            referenceourceValueList.Add(new DictionaryEntry("ClkIn", RfsgFrequencyReferenceSource.ClockIn));
            referenceourceValueList.Add(new DictionaryEntry("OnboardClock", RfsgFrequencyReferenceSource.OnboardClock));
            referenceourceValueList.Add(new DictionaryEntry("RefIn", RfsgFrequencyReferenceSource.ReferenceIn));
            referenceourceValueList.Add(new DictionaryEntry("PXI_CLK", RfsgFrequencyReferenceSource.PxiClock));
            slaveReferenceClockSourceComboBox.DataSource = referenceourceValueList;
            slaveReferenceClockSourceComboBox.DisplayMember = "Key";
            slaveReferenceClockSourceComboBox.ValueMember = "Value";
            slaveReferenceClockSourceComboBox.SelectedValue = RfsgFrequencyReferenceSource.ClockIn;
        }

        private void ConfigureMasterReferenceClockClockSourceComboBox()
        {
            var referenceourceValueList = new List<DictionaryEntry>();
            referenceourceValueList.Add(new DictionaryEntry("ClkIn", RfsgFrequencyReferenceSource.ClockIn));
            referenceourceValueList.Add(new DictionaryEntry("OnboardClock", RfsgFrequencyReferenceSource.OnboardClock));
            referenceourceValueList.Add(new DictionaryEntry("RefIn", RfsgFrequencyReferenceSource.ReferenceIn));
            referenceourceValueList.Add(new DictionaryEntry("PXI_CLK", RfsgFrequencyReferenceSource.PxiClock));
            masterReferenceClockSourceComboBox.DataSource = referenceourceValueList;
            masterReferenceClockSourceComboBox.DisplayMember = "Key";
            masterReferenceClockSourceComboBox.ValueMember = "Value";
            masterReferenceClockSourceComboBox.SelectedValue = RfsgFrequencyReferenceSource.OnboardClock;
        }

        #endregion

        #region Program Functions

        void StartGeneration()
        {
            string[] slaveResourceNamesList;
            double loOutPower = 0.0;
            int index;
            RfsgFrequencyReferenceSource masterReferenceClockSource;
            RfsgFrequencyReferenceExportedOutputTerminal masterReferenceClockOutputTerminal;
            RfsgFrequencyReferenceSource slaveReferenceClockSource;
            RfsgFrequencyReferenceExportedOutputTerminal slaveReferenceClockOutputTerminal;
            ITClockSynchronizableDevice[] rfsgSynchronizableDevices;
            try
            {
                // Read in all the control values 
                _frequency = (double)frequencyNumeric.Value;
                _power = (double)powerLevelNumeric.Value;
                _iqRate = (double)iqRateNumeric.Value;
                masterReferenceClockSource = masterReferenceClockSourceComboBox.SelectedValue as RfsgFrequencyReferenceSource ?? RfsgFrequencyReferenceSource.FromString(masterReferenceClockSourceComboBox.Text);
                masterReferenceClockOutputTerminal = masterReferenceClockOutputTerminalComboBox.SelectedValue as RfsgFrequencyReferenceExportedOutputTerminal ?? RfsgFrequencyReferenceExportedOutputTerminal.FromString(masterReferenceClockOutputTerminalComboBox.Text);
                slaveReferenceClockSource = slaveReferenceClockSourceComboBox.SelectedValue as RfsgFrequencyReferenceSource ?? RfsgFrequencyReferenceSource.FromString(slaveReferenceClockSourceComboBox.Text);
                slaveReferenceClockOutputTerminal = slaveReferenceClockOutputTerminalComboBox.SelectedValue as RfsgFrequencyReferenceExportedOutputTerminal ?? RfsgFrequencyReferenceExportedOutputTerminal.FromString(slaveReferenceClockOutputTerminalComboBox.Text);
                slaveResourceNamesList = slaveRfsgResourceNamesTextBox.Text.Split(',');
                _script = scriptRichTextBox.Text;

                _offsetIData = new double[NumberOfSamples];
                _offsetQData = new double[NumberOfSamples];
                _iData = new double[NumberOfSamples];
                _qData = new double[NumberOfSamples];
                for (index = 0; index < NumberOfSamples; index++)
                {
                    _iData[index] = 1.0;
                    _qData[index] = 0.0;
                }
                _offsetIData = SinePattern(NumberOfSamples, 1.0, 0.0, SamplesPerCycle);
                _offsetQData = SinePattern(NumberOfSamples, 1.0, 90.0, SamplesPerCycle);

                errorTextBox.Text = "No error.";
                Application.DoEvents();

                string currentResource = (string)masterRfsgResourceNameComboBox.Text;
                _rfsgMasterSession = new NIRfsg(currentResource, true, false);
                _rfsgMasterSession.DriverOperation.Warning += new EventHandler<RfsgWarningEventArgs>(MasterRfsgDriverOperation_Warning);
                ConfigureNIRfsgSession(_rfsgMasterSession);

                // Configure the reference clock source
                _rfsgMasterSession.FrequencyReference.Configure(masterReferenceClockSource, 10E6);

                // Output the LO 
                _rfsgMasterSession.RF.LocalOscillator.LOOutEnabled = true;

                // Export the reference clock for other devices to use
                _rfsgMasterSession.FrequencyReference.ExportedOutputTerminal = masterReferenceClockOutputTerminal;

                // Read the LO out power for the next device in the chain
                loOutPower = _rfsgMasterSession.RF.LocalOscillator.LOOutPower;

                // Configure the master device to receive the script trigger for all 
                // TClk will automatically synchronize and propegate the trigger to the other devices
                _rfsgMasterSession.Triggers.ScriptTriggers[0].ConfigureSoftwareTrigger();

                // Configure all slave devices
                _numberOfSlaveResources = slaveResourceNamesList.Length;
                _rfsgSlaveSessions = new NIRfsg[_numberOfSlaveResources];
                rfsgSynchronizableDevices = new ITClockSynchronizableDevice[_numberOfSlaveResources + 1];
                rfsgSynchronizableDevices[0] = (ITClockSynchronizableDevice)_rfsgMasterSession;
                for (index = 0; index < _numberOfSlaveResources; index++)
                {
                    currentResource = slaveResourceNamesList[index];
                    _rfsgSlaveSessions[index] = new NIRfsg(currentResource, true, false);
                    _rfsgSlaveSessions[index].DriverOperation.Warning += new EventHandler<RfsgWarningEventArgs>(SlaveRfsgDriverOperation_Warning);
                    ConfigureNIRfsgSession(_rfsgSlaveSessions[index]);

                    // Configure the reference clock source
                    _rfsgSlaveSessions[index].FrequencyReference.Configure(slaveReferenceClockSource, 10E6);

                    // Output the LO 
                    _rfsgSlaveSessions[index].RF.LocalOscillator.LOOutEnabled = (index != _numberOfSlaveResources - 1);

                    // Set the LO in power as the LO out power from the previous device in the daisy-chain
                    _rfsgSlaveSessions[index].RF.LocalOscillator.LOInPower = loOutPower;

                    // Export the reference clock for other devices to use
                    _rfsgSlaveSessions[index].FrequencyReference.ExportedOutputTerminal = slaveReferenceClockOutputTerminal;

                    // Read the LO out power for the next device in the chain
                    loOutPower = _rfsgSlaveSessions[index].RF.LocalOscillator.LOOutPower;

                    rfsgSynchronizableDevices[index + 1] = (ITClockSynchronizableDevice)_rfsgSlaveSessions[index];
                }

                // Configure the devices for homogeneous triggers 
                _tClockSession = new TClock(rfsgSynchronizableDevices);
                _tClockSession.ConfigureForHomogeneousTriggers();

                // Synchronize the generators 
                _tClockSession.Synchronize();

                // Initiate generation 
                _tClockSession.Initiate();

                // Start the status checking timer 
                EnableControls(false);
            }
            catch (Exception ex)
            {
                ShowError("StartGeneration()", ex);
            }
        }

        private void ConfigureNIRfsgSession(NIRfsg _rfsgSession)
        {
            _rfsgSession.RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower;
            _rfsgSession.RF.Configure(_frequency, _power);
            _rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script;
            _rfsgSession.Arb.IQRate = _iqRate;
            _rfsgSession.Arb.PreFilterGain = -2;
            _rfsgSession.Arb.WriteWaveform("NegativeOffset", _offsetIData, _offsetQData);
            _rfsgSession.Arb.WriteWaveform("PositiveOffset", _offsetQData, _offsetIData);
            _rfsgSession.Arb.WriteWaveform("NoOffset", _iData, _qData);
            _rfsgSession.Arb.Scripting.WriteScript(_script);
        }

        void CheckGeneration()
        {
            try
            {
                bool isDone;
                // Continue generation until the Stop button is pressed or there is a hardware error.
                // tClock.IsDone is used for status checking and is equivalent to _rfsgSession.CheckGenerationStatus
                isDone = _tClockSession.IsDone;
            }
            catch (Exception ex)
            {
                ShowError("CheckGeneration()", ex);
            }
        }

        void StopGeneration()
        {
            EnableControls(true);
            try
            {
                if (_rfsgSlaveSessions != null)
                {
                    for (int index = 0; index < _numberOfSlaveResources; index++)
                    {
                        if (_rfsgSlaveSessions[index] != null)
                        {
                            // Disable the output 
                            _rfsgSlaveSessions[index].RF.OutputEnabled = false;

                            // Unsubscribe from warning events
                            _rfsgSlaveSessions[index].DriverOperation.Warning -= SlaveRfsgDriverOperation_Warning;

                            // Close the NI-RFSG session 
                            _rfsgSlaveSessions[index].Close();
                            _rfsgSlaveSessions[index] = null;
                        }
                    }
                    _rfsgSlaveSessions = null;
                }
                if (_rfsgMasterSession != null)
                {
                    // Close the Master session
                    _rfsgMasterSession.RF.OutputEnabled = false;
                    _rfsgMasterSession.DriverOperation.Warning -= MasterRfsgDriverOperation_Warning;

                    // Close the NI-RFSG session 
                    _rfsgMasterSession.Close();
                    _rfsgMasterSession = null;
                }
            }
            catch (Exception ex)
            {
                errorTextBox.Text = "Error in StopGeneration(): " + ex.Message;
            }

            // Dispose the TClock session
            _tClockSession = null;
        }

        static double[] SinePattern(int NumberOfSamples, double amplitude, double phaseDegrees, double numberOfCycles)
        {
            double[] sineArray = new double[NumberOfSamples];
            for (int i = 0; i < NumberOfSamples; i++)
            {
                sineArray[i] = amplitude * Math.Sin(2 * Math.PI * i * numberOfCycles / NumberOfSamples + Math.PI * phaseDegrees / 180);
            }
            return sineArray;
        }

        private void EnableControls(bool enabled)
        {
            masterRfsgResourceNameComboBox.Enabled = enabled;
            slaveRfsgResourceNamesTextBox.Enabled = enabled;
            configurationGroupBox.Enabled = enabled;
            frequencyReferenceGroupBox.Enabled = enabled;
            scriptRichTextBox.Enabled = enabled;
            rfsgStatusTimer.Enabled = !enabled;
            startButton.Enabled = enabled;
            stopButton.Enabled = !enabled;
            softwareTriggerButton.Enabled = !enabled;
            Application.DoEvents();
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

        void SlaveRfsgDriverOperation_Warning(object sender, RfsgWarningEventArgs e)
        {
            // Display the rfsg warning
            errorTextBox.Text = "Slave Device:" + e.Message;
        }

        void MasterRfsgDriverOperation_Warning(object sender, RfsgWarningEventArgs e)
        {
            // Display the rfsg warning
            errorTextBox.Text = "Master Device:" + e.Message;
        }

        private void softwareTriggerButton_Click(object sender, EventArgs e)
        {
            _rfsgMasterSession.Triggers.ScriptTriggers[0].SendSoftwareEdgeTrigger();
        }
        #endregion
    }
}
