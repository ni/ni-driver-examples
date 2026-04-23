//==================================================================================================
//
// Title:
//      Time Histogram
//
// Description:
//      This example illustrates using some of the advanced measurement library functions
//      to create a time histogram. Time histograms eliminate the voltage information from 
//      waveforms by sorting all points into bins based on their time from the trigger. 
//      The result is a histogram of counts versus time that can show statistical 
//      information, such as pulse width jitter. Consult the NI High-Speed Digitizers Help 
//      for more information about time histograms.
//
//==================================================================================================

using System;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIScope;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.TimeHistogram
{
    public partial class MainForm : Form
    {
        NIScope scopeSession;
        bool stop;

        public MainForm()
        {
            InitializeComponent();
            ConfigureMeasurement2ComboBox();
            ConfigureMeasurement1ComboBox();
            LoadScopeDeviceNames();
            ChangeControlState(true);
        }

        #region MainForm initial configuration
        void ConfigureMeasurement2ComboBox()
        {
            foreach (ScopeScalarMeasurementType value in Enum.GetValues(typeof(ScopeScalarMeasurementType)))
            {
                measurement2ComboBox.Items.Add(value);
            }
            measurement2ComboBox.SelectedIndex = 51;
        }

        void ConfigureMeasurement1ComboBox()
        {
            foreach (ScopeScalarMeasurementType value in Enum.GetValues(typeof(ScopeScalarMeasurementType)))
            {
                measurement1ComboBox.Items.Add(value);
            }
            measurement1ComboBox.SelectedIndex = 12;
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

        int RecordLengthMin
        {
            get
            {
                return decimal.ToInt32(this.recordLengthMinNumeric.Value);
            }
        }

        double SampleRateMin
        {
            get
            {
                return decimal.ToDouble(this.sampleRateMinNumeric.Value);
            }
        }

        double ReferencePosition
        {
            get
            {
                return decimal.ToDouble(this.triggerReferencePositionNumeric.Value);
            }
        }

        PrecisionTimeSpan HighTimeLimit
        {
            get
            {
                return new PrecisionTimeSpan(decimal.ToDouble(this.highTimeLimitNumeric.Value));
            }
        }

        double HighVoltageLimit
        {
            get
            {
                return decimal.ToDouble(this.highVoltageLimitNumeric.Value);
            }
        }

        PrecisionTimeSpan LowTimeLimit
        {
            get
            {
                return new PrecisionTimeSpan(decimal.ToDouble(this.lowTimeLimitNumeric.Value));
            }
        }

        double LowVoltageLimit
        {
            get
            {
                return decimal.ToDouble(this.lowVoltageLimitNumeric.Value);
            }
        }

        int HistogramSize
        {
            get
            {
                return decimal.ToInt32(this.histogramSizeNumeric.Value);
            }
        }

        ScopeScalarMeasurementType Measurement1
        {
            get
            {
                return (ScopeScalarMeasurementType)this.measurement1ComboBox.SelectedItem;
            }
        }

        ScopeScalarMeasurementType Measurement2
        {
            get
            {
                return (ScopeScalarMeasurementType)this.measurement2ComboBox.SelectedItem;
            }
        }

        bool ClearStats
        {
            get
            {
                return this.clearStatsCheckBox.Checked;
            }
        }

        double ScalarResult
        {
            set
            {
                this.scalarResult2TextBox.Text = value.ToString();
            }
        }

        double StatisticsScalarResult
        {
            set
            {
                this.scalarResult1TextBox.Text = value.ToString();
            }
        }

        double Mean
        {
            set
            {
                this.meanTextBox.Text = value.ToString();
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

            AnalogWaveformCollection<double> waveformFromScope = null;
            AnalogWaveformCollection<double> timeHistogram = null;
            PrecisionTimeSpan ZeroTimeout = new PrecisionTimeSpan(0);
            PrecisionTimeSpan Timeout = new PrecisionTimeSpan(5);
            try
            {
                InitializeSession();
                while (!stop)
                {
                    // Configure the vertical parameters.
                    scopeSession.Channels[ChannelName].Configure(5.0, 0.0, ScopeVerticalCoupling.DC, 1.0, true);

                    // Configure the horizontal parameters.
                    scopeSession.Timing.ConfigureTiming(SampleRateMin, RecordLengthMin, ReferencePosition, 1, true);

                    // Configure an analog edge trigger.
                    scopeSession.Trigger.EdgeTrigger.Configure(ScopeTriggerSource.Channel0, 0, ScopeTriggerSlope.Positive, ScopeTriggerCoupling.DC, ZeroTimeout, ZeroTimeout);

                    // Configure the properties used in the time histogram calculation.
                    scopeSession.Channels[ChannelName].Measurement.TimeHistogram.HighTime = HighTimeLimit;
                    scopeSession.Channels[ChannelName].Measurement.TimeHistogram.HighVolts = HighVoltageLimit;
                    scopeSession.Channels[ChannelName].Measurement.TimeHistogram.LowTime = LowTimeLimit;
                    scopeSession.Channels[ChannelName].Measurement.TimeHistogram.LowVolts = LowVoltageLimit;
                    scopeSession.Channels[ChannelName].Measurement.TimeHistogram.Size = HistogramSize;

                    // Initiate a new acquisition with the configured settings,
                    // wait for this acquisition to complete, and fetch the data. Plot the data.
                    waveformFromScope = scopeSession.Channels[ChannelName].Measurement.Read(Timeout, -1, waveformFromScope);
                    PlotWaveforms(waveformFromScopeDataGridView, waveformFromScope);

                    // Calculate the array measurement, multi-acquisition time histogram, and plot the result.
                    timeHistogram = scopeSession.Channels[ChannelName].Measurement.FetchArrayMeasurement(Timeout, ScopeArrayMeasurementType.MultipleAcquisitionTimeHistogram, timeHistogram);
                    PlotWaveforms(timeHistogramDataGridView, timeHistogram);

                    // Perform the requested scalar measurement.
                    ScalarResult = scopeSession.Channels[ChannelName].Measurement.FetchScalarMeasurement(Timeout, Measurement2)[0];
                    ScopeScalarMeasurementStatistics statistics = scopeSession.Channels[ChannelName].Measurement.FetchScalarMeasurementStatistics(Timeout, Measurement1)[0];
                    StatisticsScalarResult = statistics.MeasurementResult;
                    Mean = statistics.Mean;

                    // If the user requests, clear any cached statistics.
                    if (ClearStats)
                    {
                        scopeSession.Channels[ChannelName].Measurement.ClearWaveformMeasurements();
                    }
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
            waveformFromScopeDataGridView.Columns.Clear();
            timeHistogramDataGridView.Columns.Clear();
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

            for (int columnIndex = 0; columnIndex < numberOfWaveforms; ++columnIndex)
            {
                DataGridViewTextBoxColumn waveformColumn = new DataGridViewTextBoxColumn();
                waveformColumn.Width = 125;
                waveformColumn.HeaderText = "Waveform " + columnIndex;
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
            resourceNameComboBox.Enabled = isEnabled;
            channelNameLabel.Enabled = isEnabled;
            scalarMeasurementsGroupBox.Enabled = isEnabled;
            histogramParametersGroupBox.Enabled = isEnabled;
            timingParametersGroupBox.Enabled = isEnabled;
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