//==================================================================================================
// Title        : 5673 Synchronization (TClk,SharedLO)
// Description  : This example demonstrates how to synchronize multiple NI 5673 device that share a LO using NI-TClk. 
//                The master 5673 has a LO and AWG while the slaves should be configured to have an external LO through MAX.
//==================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;
using NationalInstruments.ModularInstruments.SystemServices.TimingServices;

namespace NationalInstruments.Examples.Synchronization5673TClockSharedLO
{
    public partial class MainForm : Form
    {
        NIRfsg _rfsgMasterSession;
        NIRfsg[] _rfsgSlaveSessions;
        int _numberOfSlaveResources;
        TClock _tClockSession;
        const double FrequencyReferenceRate = 10E6;

        public MainForm()
        {
            InitializeComponent();

            LoadMasterRfsgDeviceNames();
            ConfigureSlaveFrequencyReferenceOutputComboBox();
            ConfigureMasterFrequencyReferenceOutputComboBox();
            ConfigureSlaveFrequencyReferenceSourceComboBox();
            ConfigureMasterFrequencyReferenceSourceComboBox();
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
                slaveRfsgResourceNamesTextBox.Text = masterRfsgResourceNameComboBox.Text;
            }
        }

        private void ConfigureSlaveFrequencyReferenceOutputComboBox()
        {
            var freqSourceValueList = new List<DictionaryEntry>();
            freqSourceValueList.Add(new DictionaryEntry("ClkOut", RfsgFrequencyReferenceExportedOutputTerminal.ClockOut));
            freqSourceValueList.Add(new DictionaryEntry("Do Not Export", RfsgFrequencyReferenceExportedOutputTerminal.DoNotExport));
            freqSourceValueList.Add(new DictionaryEntry("RefOut", RfsgFrequencyReferenceExportedOutputTerminal.ReferenceOut));
            freqSourceValueList.Add(new DictionaryEntry("RefOut2", RfsgFrequencyReferenceExportedOutputTerminal.ReferenceOut2));

            slaveFrequencyReferenceOutputComboBox.DataSource = freqSourceValueList;
            slaveFrequencyReferenceOutputComboBox.DisplayMember = "Key";
            slaveFrequencyReferenceOutputComboBox.ValueMember = "Value";
            slaveFrequencyReferenceOutputComboBox.SelectedValue = RfsgFrequencyReferenceExportedOutputTerminal.ClockOut;
        }

        private void ConfigureMasterFrequencyReferenceOutputComboBox()
        {
            var freqSourceValueList = new List<DictionaryEntry>();
            freqSourceValueList.Add(new DictionaryEntry("ClkOut", RfsgFrequencyReferenceExportedOutputTerminal.ClockOut));
            freqSourceValueList.Add(new DictionaryEntry("Do Not Export", RfsgFrequencyReferenceExportedOutputTerminal.DoNotExport));
            freqSourceValueList.Add(new DictionaryEntry("RefOut",RfsgFrequencyReferenceExportedOutputTerminal.ReferenceOut));
            freqSourceValueList.Add(new DictionaryEntry("RefOut2", RfsgFrequencyReferenceExportedOutputTerminal.ReferenceOut2));

            masterFrequencyReferenceOutputComboBox.DataSource = freqSourceValueList;
            masterFrequencyReferenceOutputComboBox.DisplayMember = "Key";
            masterFrequencyReferenceOutputComboBox.ValueMember = "Value";
            masterFrequencyReferenceOutputComboBox.SelectedValue = RfsgFrequencyReferenceExportedOutputTerminal.ClockOut;
        }

        private void ConfigureSlaveFrequencyReferenceSourceComboBox()
        {
            var refSourceValueList = new List<DictionaryEntry>();
            refSourceValueList.Add(new DictionaryEntry("ClkIn", RfsgFrequencyReferenceSource.ClockIn));
            refSourceValueList.Add(new DictionaryEntry("OnboardClock", RfsgFrequencyReferenceSource.OnboardClock));
            refSourceValueList.Add(new DictionaryEntry("PXI_CLK", RfsgFrequencyReferenceSource.PxiClock));
            refSourceValueList.Add(new DictionaryEntry("RefIn", RfsgFrequencyReferenceSource.ReferenceIn));

            slaveFrequencyReferenceSourceComboBox.DataSource = refSourceValueList;
            slaveFrequencyReferenceSourceComboBox.DisplayMember = "Key";
            slaveFrequencyReferenceSourceComboBox.ValueMember = "Value";
            slaveFrequencyReferenceSourceComboBox.SelectedValue = RfsgFrequencyReferenceSource.ClockIn;
        }

        private void ConfigureMasterFrequencyReferenceSourceComboBox()
        {
            var refSourceValueList = new List<DictionaryEntry>();
            refSourceValueList.Add(new DictionaryEntry("ClkIn", RfsgFrequencyReferenceSource.ClockIn));
            refSourceValueList.Add(new DictionaryEntry("OnboardClock", RfsgFrequencyReferenceSource.OnboardClock));
            refSourceValueList.Add(new DictionaryEntry("PXI_CLK", RfsgFrequencyReferenceSource.PxiClock));
            refSourceValueList.Add(new DictionaryEntry("RefIn", RfsgFrequencyReferenceSource.ReferenceIn));

            masterFrequencyReferenceSourceComboBox.DataSource = refSourceValueList;
            masterFrequencyReferenceSourceComboBox.DisplayMember = "Key";
            masterFrequencyReferenceSourceComboBox.ValueMember = "Value";
            masterFrequencyReferenceSourceComboBox.SelectedValue = RfsgFrequencyReferenceSource.OnboardClock;
        }

        #endregion

        #region Program Functions

        void StartGeneration()
        {
            string masterResourceName;
            string[] slaveResourceNamesList;
            double frequency, power, loOutPower = 0.0;
            int index;
            RfsgFrequencyReferenceSource masterFrequencyReferenceSource;
            RfsgFrequencyReferenceExportedOutputTerminal masterFrequencyReferenceOutput;
            RfsgFrequencyReferenceSource slaveFrequencyReferenceSource;
            RfsgFrequencyReferenceExportedOutputTerminal slaveFrequencyReferenceOutput;
            ITClockSynchronizableDevice[] rfsgSynchronizableDevices;
            try
            {
                // Read in all the control values 
                masterResourceName = masterRfsgResourceNameComboBox.Text;
                slaveResourceNamesList = slaveRfsgResourceNamesTextBox.Text.Split(',');
                frequency = (double)frequencyNumeric.Value;
                power = (double)powerLevelNumeric.Value;
                masterFrequencyReferenceSource = masterFrequencyReferenceSourceComboBox.SelectedValue as RfsgFrequencyReferenceSource ?? RfsgFrequencyReferenceSource.FromString(masterFrequencyReferenceSourceComboBox.Text);
                masterFrequencyReferenceOutput = masterFrequencyReferenceOutputComboBox.SelectedValue as RfsgFrequencyReferenceExportedOutputTerminal ?? RfsgFrequencyReferenceExportedOutputTerminal.FromString(masterFrequencyReferenceOutputComboBox.Text);
                slaveFrequencyReferenceSource = slaveFrequencyReferenceSourceComboBox.SelectedValue as RfsgFrequencyReferenceSource ?? RfsgFrequencyReferenceSource.FromString(slaveFrequencyReferenceSourceComboBox.Text);
                slaveFrequencyReferenceOutput = slaveFrequencyReferenceOutputComboBox.SelectedValue as RfsgFrequencyReferenceExportedOutputTerminal ?? RfsgFrequencyReferenceExportedOutputTerminal.FromString(slaveFrequencyReferenceOutputComboBox.Text);

                errorTextBox.Text = "No error.";
                Application.DoEvents();

                // Configure the master RFSG
                // Open a NI-RFSG session 
                _rfsgMasterSession = new NIRfsg(masterResourceName, true, false);

                // Subscribe to Rfsg warnings
                _rfsgMasterSession.DriverOperation.Warning += new EventHandler<RfsgWarningEventArgs>(MasterRfsgDriverOperation_Warning);

                // Configure the frequency and output power level 
                _rfsgMasterSession.RF.Configure(frequency, power);

                // Configure the generation mode 
                _rfsgMasterSession.Arb.GenerationMode = RfsgWaveformGenerationMode.ContinuousWave;

                // Master Rfsg is the device that has the LO.
                // All other devices have an external LO.  The LO from the master
                // device is daisy-chained through each 5611.
                // Configure the reference clock source 
                _rfsgMasterSession.FrequencyReference.Configure(masterFrequencyReferenceSource, FrequencyReferenceRate);

                // Output the LO 
                _rfsgMasterSession.RF.LocalOscillator.LOOutEnabled = true;

                // Export the reference clock for other devices to use 
                _rfsgMasterSession.FrequencyReference.ExportedOutputTerminal = masterFrequencyReferenceOutput;

                // Read the LO out power for the next device in the chain 
                loOutPower = _rfsgMasterSession.RF.LocalOscillator.LOOutPower;

                // Configure all the slave devices now
                _numberOfSlaveResources = slaveResourceNamesList.Length;
                _rfsgSlaveSessions = new NIRfsg[_numberOfSlaveResources];

                rfsgSynchronizableDevices = new ITClockSynchronizableDevice[_numberOfSlaveResources + 1];
                rfsgSynchronizableDevices[0] = (ITClockSynchronizableDevice)_rfsgMasterSession;

                for (index = 0; index < _numberOfSlaveResources; index++)
                {
                    // Open a NI-RFSG session 
                    _rfsgSlaveSessions[index] = new NIRfsg(slaveResourceNamesList[index], true, false);

                    // Subscribe to Rfsg warnings
                    _rfsgSlaveSessions[index].DriverOperation.Warning += new EventHandler<RfsgWarningEventArgs>(SlaveRfsgDriverOperation_Warning);

                    // Configure the frequency and output power level 
                    _rfsgSlaveSessions[index].RF.Configure(frequency, power);

                    // Configure the generation mode 
                    _rfsgSlaveSessions[index].Arb.GenerationMode = RfsgWaveformGenerationMode.ContinuousWave;


                    // Configure the reference clock source 
                    _rfsgSlaveSessions[index].FrequencyReference.Configure(slaveFrequencyReferenceSource, FrequencyReferenceRate);

                    // Output the LO 
                    _rfsgSlaveSessions[index].RF.LocalOscillator.LOOutEnabled = (index != slaveResourceNamesList.Length - 1);

                    // Set the LO in power as the LO out power from the previous device in the daisy-chain -
                    _rfsgSlaveSessions[index].RF.LocalOscillator.LOInPower = loOutPower;

                    // Export the reference clock for other devices to use 
                    _rfsgSlaveSessions[index].FrequencyReference.ExportedOutputTerminal = slaveFrequencyReferenceOutput;

                    // Read the LO out power for the next device in the chain 
                    loOutPower = _rfsgSlaveSessions[index].RF.LocalOscillator.LOOutPower;

                    // Populate the synchronizable devices array to use for TClock
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
                }
                _rfsgSlaveSessions = null;

                // Close the Master session
                if (_rfsgMasterSession != null)
                {
                    // Disable the output 
                    _rfsgMasterSession.RF.OutputEnabled = false;

                    // Unsubscribe from warning events
                    _rfsgMasterSession.DriverOperation.Warning -= MasterRfsgDriverOperation_Warning;

                    // Close the NI-RFSG Master session 
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

        #endregion

        private void EnableControls(bool enabled)
        {
            startButton.Enabled = enabled;
            stopButton.Enabled = !enabled;
            masterRfsgResourceNameComboBox.Enabled = enabled;
            slaveRfsgResourceNamesTextBox.Enabled = enabled;
            frequencyNumeric.Enabled = enabled;
            powerLevelNumeric.Enabled = enabled;
            masterFrequencyReferenceSourceComboBox.Enabled = enabled;
            masterFrequencyReferenceOutputComboBox.Enabled = enabled;
            slaveFrequencyReferenceSourceComboBox.Enabled = enabled;
            slaveFrequencyReferenceOutputComboBox.Enabled = enabled;
            rfsgStatusTimer.Enabled = !enabled;

            Application.DoEvents();
        }
    }
}
