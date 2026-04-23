//=============================================================================================================
//
// Title:
//        Binary Acquisition
//
// Description:
//      The application demonstrates how to obtain data from the a NI-SCOPE device in binary format.
//      You can specify the binary data size to be 8, 16, or 32. The program then displays the
//      fetched binary data before scaling and the actual data after scaling.
//
//=================================================================================================

using System;
using System.Text;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIScope;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.BinaryAcquisition
{
    public partial class MainForm : Form
    {
        NIScope scopeSession;
        bool stop;

        public MainForm()
        {
            InitializeComponent();
            ConfigureTriggerTypeComboBox();
            ConfigureBinaryDataSizeComboBox();
            LoadScopeDeviceNames();
            ChangeControlState(true);
        }

        #region Mainform Initial Configuration
        void ConfigureTriggerTypeComboBox()
        {
            triggerTypeComboBox.Items.Add(ScopeTriggerType.Edge);
            triggerTypeComboBox.Items.Add(ScopeTriggerType.Immediate);
            triggerTypeComboBox.SelectedIndex = 0;
        }

        void ConfigureBinaryDataSizeComboBox()
        {
            binaryDataSizeComboBox.Items.Add(8);
            binaryDataSizeComboBox.Items.Add(16);
            binaryDataSizeComboBox.Items.Add(32);
            binaryDataSizeComboBox.SelectedIndex = 0;
        }

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

        ScopeTriggerType TriggerType
        {
            get
            {
                return (ScopeTriggerType)this.triggerTypeComboBox.SelectedItem;
            }
        }

        double VerticalRange
        {
            get
            {
                return decimal.ToDouble(this.verticalRangeNumeric.Value);
            }
        }

        double VerticalOffset
        {
            get
            {
                return decimal.ToDouble(this.verticalOffsetNumeric.Value);
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

        int BinaryDataSize
        {
            get
            {
                return (int)this.binaryDataSizeComboBox.SelectedItem;
            }
        }

        string Offset
        {
            set
            {
                this.scaleOffsetTextBox.Text = value;
            }
        }

        string GainFactor
        {
            set
            {
                this.scaleGainFactorTextBox.Text = value;
            }
        }
        #endregion

        void acquireButton_Click(object sender, System.EventArgs e)
        {
            StartAcquisition();
        }

        void MainForm_Closing(object sender, FormClosingEventArgs e)
        {
            CloseSession();
        }

        void stopButton_Click(object sender, System.EventArgs e)
        {
            StopAcquisition();
        }

        void StartAcquisition()
        {
            stop = false;
            ChangeControlState(false);
            DisplayMessage("Acquisition is in progress...");

            AnalogWaveformCollection<byte> byteWaveforms = null;
            AnalogWaveformCollection<short> shortWaveforms = null;
            AnalogWaveformCollection<int> intWaveforms = null;
            ScopeWaveformInfo[] waveformInfo = null;
            try
            {
                InitializeSession();

                // Configure the vertical parameters.
                ScopeVerticalCoupling coupling = ScopeVerticalCoupling.DC;
                double probeAttenuation = 1.0;
                scopeSession.Channels[ChannelName].Configure(VerticalRange, VerticalOffset, coupling, probeAttenuation, true);

                // Configure the horizontal parameters.
                double referencePosition = 50.0;
                int numberOfRecords = 1;
                bool enforceRealtime = true;
                scopeSession.Timing.ConfigureTiming(SampleRateMin, RecordLengthMin, referencePosition, numberOfRecords, enforceRealtime);

                // Configure the trigger.
                if (TriggerType == ScopeTriggerType.Edge)
                {
                    ScopeTriggerSource source = ScopeTriggerSource.Channel0;
                    double level = 0.0;
                    ScopeTriggerSlope slope = ScopeTriggerSlope.Positive;
                    ScopeTriggerCoupling triggerCoupling = ScopeTriggerCoupling.DC;
                    PrecisionTimeSpan holdOff = PrecisionTimeSpan.Zero;
                    PrecisionTimeSpan delay = PrecisionTimeSpan.Zero;
                    scopeSession.Trigger.EdgeTrigger.Configure(source, level, slope, triggerCoupling, holdOff, delay);
                }
                else
                {
                    scopeSession.Trigger.ConfigureTriggerImmediate();
                }

                PrecisionTimeSpan timeout = new PrecisionTimeSpan(5.0);
                long recordLength = scopeSession.Acquisition.RecordLength;
                while (!stop)
                {
                    scopeSession.Measurement.Initiate();
                    switch (BinaryDataSize)
                    {
                        case 8: byteWaveforms = scopeSession.Channels[ChannelName].Measurement.FetchByte(timeout, recordLength, byteWaveforms, out waveformInfo);
                            PlotWaveforms(byteWaveforms);
                            break;
                        case 16: shortWaveforms = scopeSession.Channels[ChannelName].Measurement.FetchInt16(timeout, recordLength, shortWaveforms, out waveformInfo);
                            PlotWaveforms(shortWaveforms);
                            break;
                        case 32: intWaveforms = scopeSession.Channels[ChannelName].Measurement.FetchInt32(timeout, recordLength, intWaveforms, out waveformInfo);
                            PlotWaveforms(intWaveforms);
                            break;
                    }
                    UpdateResults(waveformInfo);
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

        void UpdateResults(ScopeWaveformInfo[] waveformInfo)
        {
            StringBuilder offsetTextBuilder = new StringBuilder();
            StringBuilder gainFactorTextBuilder = new StringBuilder();
            for (int ii = 0; ii < waveformInfo.Length; ii++)
            {
                StringBuilder prefixTextBuilder = new StringBuilder();
                if (ii > 0)
                {
                    prefixTextBuilder.Append("; ");
                }
                prefixTextBuilder.Append("Waveform " + ii + ": ");
                offsetTextBuilder.Append(prefixTextBuilder + waveformInfo[ii].Offset.ToString("E"));
                gainFactorTextBuilder.Append(prefixTextBuilder + waveformInfo[ii].Gain.ToString("E"));
            }
            Offset = offsetTextBuilder.ToString();
            GainFactor = gainFactorTextBuilder.ToString();
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

        void StopAcquisition()
        {
            if (!stop)
            {
                DisplayMessage("Stop in progress...Fetched points are being plotted...");
                stop = true;
            }
        }

        void ClearWaveforms()
        {
            binaryDataGridView.Columns.Clear();
            scaledDataGridView.Columns.Clear();
        }

        void PlotWaveforms<T>(AnalogWaveformCollection<T> waveforms)
        {
            int rowIndex, columnIndex;
            int lastCount = binaryDataGridView.RowCount;

            SetupDataGridView(binaryDataGridView, waveforms.Count);
            SetupDataGridView(scaledDataGridView, waveforms.Count);

            double[][] scaledRecords = new double[waveforms.Count][];
            for (int i = 0; i < waveforms.Count; i++)
            {
                scaledRecords[i] = waveforms[i].GetScaledData();
            }

            for (rowIndex = lastCount; rowIndex < lastCount + waveforms[0].SampleCount; rowIndex++)
            {
                columnIndex = 0;
                binaryDataGridView.Rows.Add();
                scaledDataGridView.Rows.Add();

                binaryDataGridView.Rows[rowIndex].Cells[columnIndex].Value = (rowIndex + 1).ToString();
                scaledDataGridView.Rows[rowIndex].Cells[columnIndex].Value = (rowIndex + 1).ToString();
                columnIndex++;

                foreach (AnalogWaveform<T> waveform in waveforms)
                {
                    binaryDataGridView.Rows[rowIndex].Cells[columnIndex].Value = waveform.Samples[rowIndex - lastCount].Value.ToString();
                    scaledDataGridView.Rows[rowIndex].Cells[columnIndex].Value = scaledRecords[columnIndex - 1][rowIndex - lastCount].ToString("E");
                    columnIndex++;
                }

                if (rowIndex % 100 == 0)
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

        void DisplayMessage(string message)
        {
            messageTextBox.Text = message;
            this.Refresh();
        }

        void ChangeControlState(bool isEnabled)
        {
            generalGroupBox.Enabled = isEnabled;
            horizontalGroupBox.Enabled = isEnabled;
            verticalGroupBox.Enabled = isEnabled;
            triggerGroupBox.Enabled = isEnabled;
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
            try
            {
                if (scopeSession != null)
                {
                    scopeSession.Close();
                    scopeSession = null;
                }
            }
            catch (Exception ex)
            {
                ShowError(ex);
                Application.Exit();
            }
        }

        void ShowError(Exception ex)
        {
            messageTextBox.Clear();
            MessageBox.Show(ex.Message, ex.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}