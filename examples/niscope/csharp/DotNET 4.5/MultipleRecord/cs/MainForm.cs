//==================================================================================================
//
// Title:
//      Multi Record
//
// Description  : This example demonstrates the multi-record capabilities of National Instruments 
//                high-speed digitizers.  In a multi-record acquisition, each record is one 
//                waveform with at least "min record length" points as specified with the 
//                Configure Horizontal Timing function. 
//
//==================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIScope;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.MultipleRecord
{
    public partial class MainForm : Form
    {
        NIScope scopeSession;

        public MainForm()
        {
            InitializeComponent();
            ConfigurePlotRelativeToComboBox();
            LoadScopeDeviceNames();
            ChangeControlState(true);
        }

        #region Mainform initial configuration
        void ConfigurePlotRelativeToComboBox()
        {
            var plotRelativeToList = new List<DictionaryEntry>();
            plotRelativeToList.Add(new DictionaryEntry("Trigger", "0"));
            plotRelativeToList.Add(new DictionaryEntry("Absolute", "1"));
            plotRelativeToComboBox.DataSource = plotRelativeToList;
            plotRelativeToComboBox.ValueMember = "Value";
            plotRelativeToComboBox.DisplayMember = "Key";
            plotRelativeToComboBox.SelectedIndex = 0;
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

        int NumberOfRecords
        {
            get
            {
                return decimal.ToInt32(this.numOfRecordsNumeric.Value);
            }
        }

        double Range
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
        #endregion

        void acquireButton_Click(object sender, System.EventArgs e)
        {
            StartMultiRecordAcquisition();
        }

        void mainForm_Closing(object sender, FormClosingEventArgs e)
        {
            CloseSession();
        }

        void DriverOperation_Warning(object sender, ScopeWarningEventArgs e)
        {
            MessageBox.Show(e.Text, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        void InitializeSession()
        {
            scopeSession = new NIScope(ResourceName, false, false);
            scopeSession.DriverOperation.Warning += new EventHandler<ScopeWarningEventArgs>(DriverOperation_Warning);
        }

        void StartMultiRecordAcquisition()
        {
            ChangeControlState(false);

            AnalogWaveformCollection<double> waveforms = null;
            ScopeWaveformInfo[] waveformInfo;
            try
            {
                InitializeSession();

                double offset = 0.0;
                ScopeVerticalCoupling coupling = ScopeVerticalCoupling.DC;
                double probeAttenuation = 1.0;
                scopeSession.Channels[ChannelName].Configure(Range, offset, coupling, probeAttenuation, true);

                double referencePosition = 50.0;
                bool enforceRealtime = true;
                scopeSession.Timing.ConfigureTiming(SampleRateMin, RecordLengthMin, referencePosition, NumberOfRecords, enforceRealtime);

                double triggerLevel = 0.0;
                ScopeTriggerSlope triggerSlope = ScopeTriggerSlope.Positive;
                ScopeTriggerCoupling triggerCoupling = ScopeTriggerCoupling.DC;
                PrecisionTimeSpan triggerHoldoff = PrecisionTimeSpan.Zero;
                PrecisionTimeSpan triggerDelay = PrecisionTimeSpan.Zero;
                ScopeTriggerSource triggerSource = ScopeTriggerSource.Channel0;

                scopeSession.Trigger.EdgeTrigger.Configure(triggerSource, triggerLevel, triggerSlope, triggerCoupling, triggerHoldoff, triggerDelay);

                scopeSession.Measurement.Initiate();
               
                long recordLength = scopeSession.Acquisition.RecordLength;

                PrecisionTimeSpan timeout = new PrecisionTimeSpan(5.0);
                waveforms = scopeSession.Channels[ChannelName].Measurement.FetchDouble(timeout, recordLength, waveforms, out waveformInfo);
                PlotWaveforms(sampledDataGridView, waveforms);
            }
            catch (System.Exception ex)
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

            SetupDataGridView(dgv, waveforms.Count);
            for (rowIndex = 0; rowIndex < waveforms[0].SampleCount; rowIndex++)
            {
                columnIndex = 0;
                dgv.Rows.Add();
                dgv.Rows[rowIndex].Cells[columnIndex++].Value = (rowIndex + 1).ToString();
                foreach (AnalogWaveform<double> waveform in waveforms)
                {
                    dgv.Rows[rowIndex].Cells[columnIndex++].Value = waveform.Samples[rowIndex].Value.ToString("E");
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

        void ChangeControlState(bool isEnabled)
        {
            generalGroupBox.Enabled = isEnabled;
            verticalAndHorizontalGroupBox.Enabled = isEnabled;
            acquireButton.Enabled = isEnabled;
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

        static void ShowError(Exception ex)
        {
            MessageBox.Show(ex.Message, ex.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}

