//==================================================================================================
//
// Title:
//      Advanced Measurement Library

// Description:
//      This example illustrates using some of the advanced measurement library functions 
//      such as waveform processing. The example calls Auto Setup once to configure the 
//      vertical and horizontal subsystems of the digitizer based on the input to all 
//      the channels.
//
//==================================================================================================

using System;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIScope;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.AdvancedMeasurementLibrary
{
    public partial class MainForm : Form
    {
        NIScope scopeSession;
        bool stop;

        public MainForm()
        {
            InitializeComponent();
            ConfigureArrayMeasurementComboBox();
            ConfigureProcessingStepComboBox();
            ConfigureFilterComboBox();
            LoadScopeDeviceNames();
            ChangeControlState(true);
        }

        #region Mainform initial configuration
        void ConfigureArrayMeasurementComboBox()
        {
            foreach (ScopeArrayMeasurementType enumValue in Enum.GetValues(typeof(ScopeArrayMeasurementType)))
            {
                arrayMeasurementComboBox.Items.Add(enumValue);
            }
            arrayMeasurementComboBox.SelectedItem = arrayMeasurementComboBox.Items[19];
        }

        void ConfigureProcessingStepComboBox()
        {
            foreach (ScopeArrayMeasurementType enumValue in Enum.GetValues(typeof(ScopeArrayMeasurementType)))
            {
                processingStepComboBox.Items.Add(enumValue);
            }
            processingStepComboBox.SelectedItem = processingStepComboBox.Items[18];
        }

        void ConfigureFilterComboBox()
        {
            foreach (ScopeMeasurementFilterType enumValue in Enum.GetValues(typeof(ScopeMeasurementFilterType)))
            {
                filterComboBox.Items.Add(enumValue);
            }
            filterComboBox.SelectedItem = filterComboBox.Items[0];
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
                return this.channelTextBox.Text;
            }
        }

        ScopeArrayMeasurementType ProcessStepMeasurement
        {
            get
            {
                return (ScopeArrayMeasurementType)this.processingStepComboBox.SelectedItem;
            }
        }

        ScopeArrayMeasurementType ArrayMeasurement
        {
            get
            {
                return (ScopeArrayMeasurementType)this.arrayMeasurementComboBox.SelectedItem;
            }
        }

        ScopeMeasurementFilterType FilterType
        {
            get
            {
                return (ScopeMeasurementFilterType)this.filterComboBox.SelectedItem;
            }
        }

        double CutoffFrequency
        {
            get
            {
                return decimal.ToDouble(this.cutoffFrequencyNumeric.Value);
            }
        }

        double CenterFrequency
        {
            get
            {
                return decimal.ToDouble(this.centerFrequencyNumeric.Value);
            }
        }

        double BandPassWidth
        {
            get
            {
                return decimal.ToDouble(this.bandpassWidthNumeric.Value);
            }
        }

        PrecisionTimeSpan Timeout
        {
            get
            {
                return PrecisionTimeSpan.FromSeconds(decimal.ToDouble(this.timeoutNumeric.Value));
            }
        }
        #endregion

        void acquireButton_Click(object sender, EventArgs e)
        {
            StartAcquisition();
        }

        void mainForm_Closing(object sender, FormClosingEventArgs e)
        {
            CloseSession();
        }

        void stopButton_Click(object sender, EventArgs e)
        {
            StopAcquistion();
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

        void StartAcquisition()
        {
            stop = false;
            ChangeControlState(false);
            DisplayMessage("Acquisition is in progress...");

            AnalogWaveformCollection<double> sampledwaveforms = null;
            AnalogWaveformCollection<double> measurementWaveforms = null;
            try
            {
                InitializeSession();
                scopeSession.Measurement.AutoSetup();
                while (!stop)
                {
                    scopeSession.Channels[ChannelName].Measurement.Filter.Type = FilterType;
                    scopeSession.Channels[ChannelName].Measurement.Filter.CutoffFrequency = CutoffFrequency;
                    scopeSession.Channels[ChannelName].Measurement.Filter.CenterFrequency = CenterFrequency;
                    scopeSession.Channels[ChannelName].Measurement.Filter.Width = BandPassWidth;

                    scopeSession.Channels[ChannelName].Measurement.AddWaveformProcessing(ProcessStepMeasurement);
                    long recordLength = scopeSession.Acquisition.RecordLength;
                    sampledwaveforms = scopeSession.Channels[ChannelName].Measurement.Read(Timeout, recordLength, sampledwaveforms);
                    measurementWaveforms = scopeSession.Channels[ChannelName].Measurement.FetchArrayMeasurement(Timeout, ArrayMeasurement, measurementWaveforms);
                    scopeSession.Channels[ChannelName].Measurement.ClearWaveformProcessing();

                    PlotWaveforms(sampledDataGridView, sampledwaveforms);
                    PlotWaveforms(measurementDataGridView, measurementWaveforms);
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
            measurementDataGridView.Columns.Clear();
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

        void StopAcquistion()
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