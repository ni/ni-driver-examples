//==================================================================================================
//
// Title:
//      Multiple Record Fetch More Than Available Memory
//
// Description:
//      This example demonstrates the multi-record and continuous acquisition capabilities of National 
//      Instruments digitizers.
//
//==================================================================================================

using System;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIScope;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.MultipleRecordFetchMoreThanAvailableMemory
{
    public partial class MainForm : Form
    {
        NIScope scopeSession;
        bool stop;

        public MainForm()
        {
            InitializeComponent();
            LoadScopeDeviceNames();
            ChangeControlState(true);
        }

        #region Mainform initial configuration
        void LoadScopeDeviceNames()
        {
            using (ModularInstrumentsSystem scopeDevices = new ModularInstrumentsSystem("NI-Scope"))
            {
                foreach (DeviceInfo device in scopeDevices.DeviceCollection)
                {
                    resourceNameComboBox.Items.Add(device.Name);
                }
            }
            if (resourceNameComboBox.Items.Count > 0)
            {
                resourceNameComboBox.SelectedIndex = 0;
            }
        }
        #endregion

        #region Mainform configuration values
        string ResourceName
        {
            get
            {
                return this.resourceNameComboBox.Text;
            }
        }

        string ChannelName
        {
            get
            {
                return this.channelNameTextBox.Text;
            }
        }

        double VerticalRange
        {
            get
            {
                return decimal.ToDouble(this.verticalRangeNumeric.Value);
            }
        }

        double SampleRateMin
        {
            get
            {
                return decimal.ToDouble(this.minSampleRateNumeric.Value);
            }
        }

        int RecordLengthMin
        {
            get
            {
                return decimal.ToInt32(this.minRecordLengthNumeric.Value);
            }
        }

        bool AllowMoreRecordsThanAvailableMem
        {
            get
            {
                return this.allowMoreRecsThanAvaiMemCheckBox.Checked;
            }
        }

        int NumberOfRecords
        {
            get
            {
                return decimal.ToInt32(this.numOfRecordNumeric.Value);
            }
        }

        string NumberOfRecordsFetched
        {
            set
            {
                this.numRecordsFetchedTextBox.Text = value;
            }
        }

        string NumberOfRecordsAcquired
        {
            set
            {
                this.numRecordsAcquiredTextBox.Text = value;
            }
        }
        #endregion

        void acquireButton_Click(object sender, System.EventArgs e)
        {
            MultiRecordFetch();
        }

        void stopButton_Click(object sender, System.EventArgs e)
        {
            StopFetch();
        }
        void mainForm_Closing(object sender, FormClosingEventArgs e)
        {
            CloseSession();
        }

        void InitializeSession()
        {
            scopeSession = new NIScope(ResourceName, false, false);
            scopeSession.DriverOperation.Warning += new EventHandler<ScopeWarningEventArgs>(DriverOperation_Warning);
        }

        void DriverOperation_Warning(object sender, ScopeWarningEventArgs e)
        {
            messageTextBox.Clear();
            MessageBox.Show(e.Text, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        void MultiRecordFetch()
        {
            stop = false;
            ChangeControlState(false);
            DisplayMessage("Acquisition is in progress...");

            AnalogWaveformCollection<double> waveforms = null;
            try
            {
                InitializeSession();
                scopeSession.Timing.MoreRecordsThanMemoryAllowed = AllowMoreRecordsThanAvailableMem;

                double offset = 0.0;
                double probeAttenuation = 1.0;
                scopeSession.Channels[ChannelName].Configure(VerticalRange, offset, ScopeVerticalCoupling.DC, probeAttenuation, true);

                double referencePosition = 50.0;
                bool enforceRealtime = true;
                scopeSession.Timing.ConfigureTiming(SampleRateMin, RecordLengthMin, referencePosition, NumberOfRecords, enforceRealtime);

                double triggerLevel = 0.0;
                ScopeTriggerSlope triggerSlope = ScopeTriggerSlope.Positive;
                ScopeTriggerCoupling triggerCoupling = ScopeTriggerCoupling.DC;
                ScopeTriggerSource triggerSource = ScopeTriggerSource.Channel0;
                PrecisionTimeSpan triggerHoldoff = PrecisionTimeSpan.Zero;
                PrecisionTimeSpan triggerDelay = PrecisionTimeSpan.Zero;
                scopeSession.Trigger.EdgeTrigger.Configure(triggerSource, triggerLevel, triggerSlope, triggerCoupling, triggerHoldoff, triggerDelay);

                scopeSession.Measurement.Initiate();

                long actualRecordLength = scopeSession.Acquisition.RecordLength;
                PrecisionTimeSpan timeout = new PrecisionTimeSpan(5.0);
                for (int record = 0; record < NumberOfRecords && !stop; record++)
                {
                    NumberOfRecordsAcquired = scopeSession.Measurement.RecordsDone.ToString();
                    NumberOfRecordsFetched = (record + 1).ToString();
                    scopeSession.Acquisition.NumberOfRecordsToFetch = 1;
                    scopeSession.Acquisition.RecordNumberToFetch = record;
                    waveforms = scopeSession.Channels[ChannelName].Measurement.FetchDouble(timeout, actualRecordLength, waveforms);
                    PlotWaveforms(sampledDataGridView, waveforms);
                }
                DisplayMessage("Acquisition successful!!!");
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
            finally
            {
                CloseSession();
                ChangeControlState(true);
            }
        }

        void ClearWaveforms()
        {
            sampledDataGridView.Columns.Clear();
        }

        static void PlotWaveforms(DataGridView dgv, AnalogWaveformCollection<double> waveforms)
        {
            int rowIndex, columnIndex;
            int lastCount = dgv.RowCount;

            SetupDataGridView(dgv, waveforms.Count);
            for (rowIndex = lastCount; rowIndex < lastCount + waveforms[0].SampleCount; rowIndex++)
            {
                columnIndex = 0;
                dgv.Rows.Add();
                dgv.Rows[rowIndex].Cells[columnIndex++].Value = (rowIndex + 1).ToString();
                foreach (AnalogWaveform<double> waveform in waveforms)
                {
                    dgv.Rows[rowIndex].Cells[columnIndex++].Value = waveform.Samples[rowIndex - lastCount].Value.ToString("E");
                }
                if (rowIndex % 1000 == 0)
                {
                    Application.DoEvents();
                }
            }
        }

        static void SetupDataGridView(DataGridView dgv, int numberOfWaveforms)
        {
            if (dgv.ColumnCount > 0)
                return;

            DataGridViewTextBoxColumn indexColumn = new DataGridViewTextBoxColumn();
            indexColumn.Width = 45;
            indexColumn.HeaderText = "Index";
            dgv.Columns.Add(indexColumn);

            for (int waveformIndex = 0; waveformIndex < numberOfWaveforms; ++waveformIndex)
            {
                DataGridViewTextBoxColumn waveformColumn = new DataGridViewTextBoxColumn();
                waveformColumn.Width = 125;
                waveformColumn.HeaderText = "Waveform " + waveformIndex;
                dgv.Columns.Add(waveformColumn);
            }
        }

        void StopFetch()
        {
            if (!stop)
            {
                DisplayMessage("Stop in progress...Fetched points are being plotted...");
                stop = true;
            }
        }

        void DisplayMessage(string message)
        {
            messageTextBox.Text = message;
            this.Refresh();
        }

        void ChangeControlState(bool isEnabled)
        {
            generalGroupBox.Enabled = isEnabled;
            acquireButton.Enabled = isEnabled;
            stopButton.Enabled = !isEnabled;
            if (!isEnabled)
            {
                ClearWaveforms();
            }
            this.Refresh();
        }

        void CloseSession()
        {
            if (scopeSession != null)
            {
                try
                {
                    scopeSession.Close();
                    scopeSession = null;
                }
                catch (Exception ex)
                {
                    ShowError(ex);
                    Application.Exit();
                }
            }
        }

        void ShowError(Exception ex)
        {
            messageTextBox.Clear();
            MessageBox.Show(ex.Message, ex.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}