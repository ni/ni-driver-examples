/*******************************************************************************
*
* Example program:
*   RFSA Synchronization T-Clock
*
* Category:
*   NI-RFSA
*
* Description:
*   Use this example to learn the basics of TClk synchronization for RF vector
	signal analyzers. This example shows how to handle multiple sessions of
	NI-RFSA devices and pass them to NI-TClk synchronization functions. It uses
	NI-TClk to make all the NI-RFSA devices start acquiring data at the same time.


* Instructions for running:
*   1. Configure both RFSA device in the MAX for the program to run.
*
*	2. Configure the Reference Level, Carrier Frequency and IQ Rate in the UI.
*
*   3. Configure the Trigger Setting in Trigger Slope and Trigger Level.
*
*	4. Configure the Clock for both Master and Slave.
*
*   5. Select the Acquire Button in the UI to start the acquisition.
*
*   6. The data is displayed in the DataGrid.
*
* I/O Connections Overview:
*   Make sure your signal input terminals match the Physical Channel I/O
*   Controls.  If you have a PXI chassis, ensure that it has been properly
*   identified in MAX.
*
*******************************************************************************/
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIRfsa;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;
using NationalInstruments.ModularInstruments.SystemServices.TimingServices;


namespace NationalInstruments.Examples.RFSASynchronizationTClock5840
{
    public partial class MainForm : Form
    {
        private NIRfsa[] sessions;
        private double loFrequency;
        private DataGridView[] dataGridResults = new DataGridView[2];

        public MainForm()
        {
            InitializeComponent();
            ConfigureRefClockMasterComboBox();
            ConfigureRefClockExpMasterComboBox();
            ConfigureRefClockSlaveComboBox();
            ConfigureRefClockExpSlaveComboBox();
            LoadRfsaDeviceNames();

            dataGridResults[0] = this.dataGridViewResultsId0;
            dataGridResults[1] = this.dataGridViewResultsId1;

        }

        #region UI Initial Value Config Section

        private void ConfigureRefClockMasterComboBox()
        {
            List<KeyValuePair<string, RfsaReferenceClockSource>> refClockMasterValueList = new List<KeyValuePair<string, RfsaReferenceClockSource>>();
            refClockMasterValueList.Add(new KeyValuePair<string, RfsaReferenceClockSource>("OnboardClock", RfsaReferenceClockSource.OnboardClock));
            refClockMasterValueList.Add(new KeyValuePair<string, RfsaReferenceClockSource>("RefIn", RfsaReferenceClockSource.ReferenceIn));
            refClockMasterValueList.Add(new KeyValuePair<string, RfsaReferenceClockSource>("PXI_Clk", RfsaReferenceClockSource.PxiClock));
            refClockMasterValueList.Add(new KeyValuePair<string, RfsaReferenceClockSource>("ClkIn", RfsaReferenceClockSource.ClockIn));
            referenceClockMasterComboBox.DisplayMember = "Key";
            referenceClockMasterComboBox.ValueMember = "Value";
            referenceClockMasterComboBox.DataSource = refClockMasterValueList;
            referenceClockMasterComboBox.SelectedIndex = 0;
        }



        private void ConfigureRefClockExpMasterComboBox()
        {
            List<KeyValuePair<string, RfsaExportReferenceClockExportedOutputTerminal>> refClockExpMasterValueList = new List<KeyValuePair<string, RfsaExportReferenceClockExportedOutputTerminal>>();
            refClockExpMasterValueList.Add(new KeyValuePair<string, RfsaExportReferenceClockExportedOutputTerminal>("RefOut", RfsaExportReferenceClockExportedOutputTerminal.ReferenceOut));
            refClockExpMasterValueList.Add(new KeyValuePair<string, RfsaExportReferenceClockExportedOutputTerminal>("RefOut2", RfsaExportReferenceClockExportedOutputTerminal.ReferenceOut2));
            refClockExpMasterValueList.Add(new KeyValuePair<string, RfsaExportReferenceClockExportedOutputTerminal>("ClkOut", RfsaExportReferenceClockExportedOutputTerminal.ClockOut));
            refClockExpMasterValueList.Add(new KeyValuePair<string, RfsaExportReferenceClockExportedOutputTerminal>("None", RfsaExportReferenceClockExportedOutputTerminal.None));
            referenceClockExportMasterComboBox.DisplayMember = "Key";
            referenceClockExportMasterComboBox.ValueMember = "Value";
            referenceClockExportMasterComboBox.DataSource = refClockExpMasterValueList;
            referenceClockExportMasterComboBox.SelectedIndex = 2;
        }



        private void ConfigureRefClockSlaveComboBox()
        {
            List<KeyValuePair<string, RfsaReferenceClockSource>> refClockSlaveValueList = new List<KeyValuePair<string, RfsaReferenceClockSource>>();
            refClockSlaveValueList.Add(new KeyValuePair<string, RfsaReferenceClockSource>("OnboardClock", RfsaReferenceClockSource.OnboardClock));
            refClockSlaveValueList.Add(new KeyValuePair<string, RfsaReferenceClockSource>("RefIn", RfsaReferenceClockSource.ReferenceIn));
            refClockSlaveValueList.Add(new KeyValuePair<string, RfsaReferenceClockSource>("PXI_Clk", RfsaReferenceClockSource.PxiClock));
            refClockSlaveValueList.Add(new KeyValuePair<string, RfsaReferenceClockSource>("ClkIn", RfsaReferenceClockSource.ClockIn));
            referenceClockSlaveComboBox.DisplayMember = "Key";
            referenceClockSlaveComboBox.ValueMember = "Value";
            referenceClockSlaveComboBox.DataSource = refClockSlaveValueList;
            referenceClockSlaveComboBox.SelectedIndex = 3;
        }



        private void ConfigureRefClockExpSlaveComboBox()
        {
            List<KeyValuePair<string, RfsaExportReferenceClockExportedOutputTerminal>> refClockExpSlaveValueList = new List<KeyValuePair<string, RfsaExportReferenceClockExportedOutputTerminal>>();
            refClockExpSlaveValueList.Add(new KeyValuePair<string, RfsaExportReferenceClockExportedOutputTerminal>("RefOut", RfsaExportReferenceClockExportedOutputTerminal.ReferenceOut));
            refClockExpSlaveValueList.Add(new KeyValuePair<string, RfsaExportReferenceClockExportedOutputTerminal>("RefOut2", RfsaExportReferenceClockExportedOutputTerminal.ReferenceOut2));
            refClockExpSlaveValueList.Add(new KeyValuePair<string, RfsaExportReferenceClockExportedOutputTerminal>("ClkOut", RfsaExportReferenceClockExportedOutputTerminal.ClockOut));
            refClockExpSlaveValueList.Add(new KeyValuePair<string, RfsaExportReferenceClockExportedOutputTerminal>("None", RfsaExportReferenceClockExportedOutputTerminal.None));
            referenceClockExportSlaveComboBox.DisplayMember = "Key";
            referenceClockExportSlaveComboBox.ValueMember = "Value";
            referenceClockExportSlaveComboBox.DataSource = refClockExpSlaveValueList;
            referenceClockExportSlaveComboBox.SelectedIndex = 2;
        }

        #endregion

        private void LoadRfsaDeviceNames()
        {
            ModularInstrumentsSystem modularInstrumentsSystem = new ModularInstrumentsSystem("NI-RFSA");
            foreach (DeviceInfo device in modularInstrumentsSystem.DeviceCollection)
            {
                resourceNameMasterComboBox.Items.Add(device.Name);
                resourceNameSlaveComboBox.Items.Add(device.Name);
            }
            if (modularInstrumentsSystem.DeviceCollection.Count > 0)
            {
                resourceNameMasterComboBox.SelectedIndex = 0;
                resourceNameSlaveComboBox.SelectedIndex = 0;
            }
        }


        private int NumberOfSession
        {
            get
            {
                return decimal.ToInt32(numberOfDevicesNumeric.Value);
            }
        }

        private string MasterResourceName
        {
            get
            {
                return resourceNameMasterComboBox.Text;
            }
        }

        private string SlaveResourceName
        {
            get
            {
                return resourceNameSlaveComboBox.Text;
            }
        }

        private double IQRate
        {
            get
            {
                return decimal.ToDouble(this.iqRateNumeric.Value);
            }
        }

        private double ReferenceLevel
        {
            get
            {
                return decimal.ToDouble(this.referenceLevelNumeric.Value);
            }
        }

        private double CarrierFrequency
        {
            get
            {
                return decimal.ToDouble(this.carrierFrequencyNumeric.Value);
            }
        }

        private int NumberOfSamples
        {
            get
            {
                return decimal.ToInt32(this.samplesPerRecordNumeric.Value);
            }
        }

        private RfsaReferenceClockSource MasterReferenceClockSource
        {
            get
            {
                return this.referenceClockMasterComboBox.SelectedValue as RfsaReferenceClockSource ?? RfsaReferenceClockSource.FromString(this.referenceClockMasterComboBox.Text);
            }
        }

        private RfsaReferenceClockSource SlaveReferenceClockSource
        {
            get
            {
                return this.referenceClockSlaveComboBox.SelectedValue as RfsaReferenceClockSource ?? RfsaReferenceClockSource.FromString(this.referenceClockSlaveComboBox.Text);
            }
        }

        private RfsaExportReferenceClockExportedOutputTerminal MasterRefClockExport
        {
            get
            {
                return this.referenceClockExportMasterComboBox.SelectedValue as RfsaExportReferenceClockExportedOutputTerminal ?? RfsaExportReferenceClockExportedOutputTerminal.FromString(this.referenceClockExportMasterComboBox.Text); ;
            }
        }

        private RfsaExportReferenceClockExportedOutputTerminal SlaveRefClockExport
        {
            get
            {
                return this.referenceClockExportSlaveComboBox.SelectedValue as RfsaExportReferenceClockExportedOutputTerminal ?? RfsaExportReferenceClockExportedOutputTerminal.FromString(this.referenceClockExportSlaveComboBox.Text); ;
            }
        }

        private void acquireButton_Click(object sender, System.EventArgs e)
        {
            ChangeControlState(false);
            try
            {
                ConfigureSynchronizeAcquire();
            }
            catch (System.Exception ex)
            {
                ShowError(ex.Message);
                for (int i = 0; i < NumberOfSession; i++)
                {
                    CloseRfsaSession(sessions[i]);
                }
            }
            finally
            {
                ChangeControlState(true);
            }
        }


        private void ChangeControlState(bool isEnabled)
        {
            this.acquireButton.Enabled = isEnabled;
            this.resourceNameMasterComboBox.Enabled = isEnabled;
            this.resourceNameSlaveComboBox.Enabled = isEnabled;
            this.referenceLevelNumeric.Enabled = isEnabled;
            this.carrierFrequencyNumeric.Enabled = isEnabled;
            this.iqRateNumeric.Enabled = isEnabled;
            this.samplesPerRecordNumeric.Enabled = isEnabled;
            this.referenceClockMasterComboBox.Enabled = isEnabled;
            this.referenceClockExportMasterComboBox.Enabled = isEnabled;
            this.referenceClockSlaveComboBox.Enabled = isEnabled;
            this.referenceClockExportSlaveComboBox.Enabled = isEnabled;
        }

        private void ConfigureSynchronizeAcquire()
        {

            PrepareDeviceSessions();
            for (int i = 0; i < NumberOfSession; i++)
            {
                IntializeRfsaSession(i);
                ConfigureForIQ(i);
                ConfigureRefClock(i);
            }

            TClock tClockSession = new TClock(sessions[0], sessions[1]);
            PrecisionTimeSpan minTime = new PrecisionTimeSpan(0);
            tClockSession.ConfigureForHomogeneousTriggers();
            tClockSession.Synchronize(minTime);
            tClockSession.Initiate();

            for (int i = 0; i < NumberOfSession; i++)
            {
                FetchIQData(i);
            }
            for (int i = 0; i < NumberOfSession; i++)
            {
                CloseRfsaSession(sessions[i]);
            }
        }

        private void FetchIQData(int i)
        {
            RfsaWaveformInfo wfmInfo;
            ComplexDouble[] data;
            PrecisionTimeSpan timespan = new PrecisionTimeSpan(10.0);
            data = sessions[i].Acquisition.IQ.FetchIQSingleRecordComplex<ComplexDouble>(0, NumberOfSamples, timespan, out wfmInfo);
            this.dataGridResults[i].DataSource = data;
            this.Refresh();
        }

        private void ConfigureRefClock(int sessionIndex)
        {
            RfsaReferenceClockSource clockSource;
            RfsaExportReferenceClockExportedOutputTerminal clockExport;
            string _model;
            double _referenceClockRate;
            if (sessionIndex == 0)
            {
                clockSource = MasterReferenceClockSource;
                clockExport = MasterRefClockExport;
            }
            else
            {
                clockSource = SlaveReferenceClockSource;
                clockExport = SlaveRefClockExport;
            }

            sessions[sessionIndex].Configuration.ReferenceClock.Source = clockSource;
            sessions[sessionIndex].Configuration.ReferenceClock.Export.OutputTerminal = clockExport;

        }

        private void ConfigureForIQ(int sessionIndex)
        {
            sessions[sessionIndex].Configuration.Vertical.ReferenceLevel = ReferenceLevel;
            sessions[sessionIndex].Configuration.AcquisitionType = RfsaAcquisitionType.IQ;
            sessions[sessionIndex].Configuration.IQ.CarrierFrequency = CarrierFrequency;
            sessions[sessionIndex].Configuration.IQ.NumberOfSamples = NumberOfSamples;
            sessions[sessionIndex].Configuration.IQ.NumberOfSamplesIsFinite = true;
            sessions[sessionIndex].Configuration.IQ.IQRate = IQRate;
        }

        private void IntializeRfsaSession(int sessionIndex)
        {
            CloseRfsaSession(sessions[sessionIndex]);

            if (sessionIndex == 0)
            {
                sessions[sessionIndex] = new NIRfsa(MasterResourceName, true, false);
                sessions[sessionIndex].DriverOperation.Warning += new System.EventHandler<RfsaWarningEventArgs>(DriverOperationWarning);
            }
            else
            {
                sessions[sessionIndex] = new NIRfsa(SlaveResourceName, true, false);
                sessions[sessionIndex].DriverOperation.Warning += new System.EventHandler<RfsaWarningEventArgs>(DriverOperationWarning);
            }

        }

        private void DriverOperationWarning(object sender, RfsaWarningEventArgs e)
        {
            MessageBox.Show(e.Warning.ToString(), "Warning");
        }

        private static void CloseRfsaSession(NIRfsa rfsaSession)
        {
            if (rfsaSession != null)
            {
                try
                {
                    rfsaSession.Close();
                    rfsaSession = null;
                }
                catch (Exception ex)
                {
                    ShowError("Unable to Close Session, Reset the device.\n" + "Error : " + ex.Message);
                    Application.Exit();
                }
            }
        }

        private void PrepareDeviceSessions()
        {
            sessions = new NIRfsa[NumberOfSession];

            for (int i = 0; i < NumberOfSession; i++)
            {
                sessions[i] = null;
            }
        }

        private static void ShowError(string message)
        {
            if (string.IsNullOrEmpty(message))
                message = "Unexpected Error";
            MessageBox.Show(message, "Error"); ;
        }
    }

}