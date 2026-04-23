//==================================================================================================
//
// Title:
//      Voltage Histogram
//
// Description:
//      This example illustrates using some of the advanced measurement library functions
//      to create a voltage histogram. Voltage histograms eliminate the voltage information from 
//      waveforms by sorting all points into bins based on their time from the trigger. 
//      The result is a histogram of counts versus voltage value that can be used to see 
//      statistical information about the amplitude of a signal. Consult the NI High-Speed
//      Digitizers Help for more information about voltage histograms.
//
//==================================================================================================

using System;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIScope;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.VoltageHistogram
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
            measurement2ComboBox.Items.AddRange(Enum.GetNames(typeof(ScopeScalarMeasurementType)));
            measurement2ComboBox.SelectedIndex = 39;
        }

        void ConfigureMeasurement1ComboBox()
        {
            measurement1ComboBox.Items.AddRange(Enum.GetNames(typeof(ScopeScalarMeasurementType)));
            measurement1ComboBox.SelectedIndex = 8;
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

        double HighVoltageLimit
        {
            get
            {
                return decimal.ToDouble(this.highVoltageLimitNumeric.Value);
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
                return (ScopeScalarMeasurementType)Enum.Parse(typeof(ScopeScalarMeasurementType), measurement1ComboBox.Text);
            }
        }

        ScopeScalarMeasurementType Measurement2
        {
            get
            {
                return (ScopeScalarMeasurementType)Enum.Parse(typeof(ScopeScalarMeasurementType), measurement2ComboBox.Text);
            }
        }

        bool ClearStats
        {
            get
            {
                return this.clearStatsCheckBox.Checked;
            }
        }

        double ScalarResult2
        {
            set
            {
                this.scalarResult2TextBox.Text = value.ToString();
            }
        }

        double ScalarResult1
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
            AnalogWaveformCollection<double> voltageHistogram = null;
            PrecisionTimeSpan ZeroTimeout = new PrecisionTimeSpan(0);
            PrecisionTimeSpan Timeout = new PrecisionTimeSpan(5);
            try
            {
                InitializeSession();
                while (!stop)
                {
                    // Configure the vertical parameters such as input range, offset, and coupling.
                    scopeSession.Channels[ChannelName].Range = 5;

                    // Configure the horizontal parameters such as sampling rate and number of samples to acquire.
                    scopeSession.Timing.ConfigureTiming(SampleRateMin, RecordLengthMin, 50, 1, true);

                    // Configure an analog edge trigger.
                    scopeSession.Trigger.EdgeTrigger.Configure(ScopeTriggerSource.Channel0, 0, ScopeTriggerSlope.Positive,
                        ScopeTriggerCoupling.DC, ZeroTimeout, ZeroTimeout);

                    // Configure the attributes used in the voltage histogram calculation.
                    scopeSession.Channels[ChannelName].Measurement.VoltageHistogram.HighVolts = HighVoltageLimit;
                    scopeSession.Channels[ChannelName].Measurement.VoltageHistogram.LowVolts = LowVoltageLimit;
                    scopeSession.Channels[ChannelName].Measurement.VoltageHistogram.Size = HistogramSize;

                    // Initiate a new acquisition with the configured settings, wait for this acquisition to complete,
                    // and fetch the data. Plot the data.
                    waveformFromScope = scopeSession.Channels[ChannelName].Measurement.Read(Timeout, -1, waveformFromScope);
                    PlotWaveforms(waveformFromScopeDataGridView, waveformFromScope);

                    // Calculate the array measurement, multi-acquisition voltage histogram, and plot the result.
                    voltageHistogram = scopeSession.Channels[ChannelName].Measurement.FetchArrayMeasurement(Timeout, 
                        ScopeArrayMeasurementType.MultipleAcquisitionVoltageHistogram, voltageHistogram);
                    PlotWaveforms(voltageHistogramDataGridView, voltageHistogram);

                    // Perform the requested scalar measurement.
                    ScalarResult2 = scopeSession.Channels[ChannelName].Measurement.FetchScalarMeasurement(Timeout, Measurement2)[0];
                    ScopeScalarMeasurementStatistics statistics = scopeSession.Channels[ChannelName].Measurement.
                        FetchScalarMeasurementStatistics(Timeout, Measurement1)[0];
                    ScalarResult1 = statistics.MeasurementResult;
                    Mean = statistics.Mean;

                    // If the user requests, clear any cached statistics.
                    if (ClearStats)
                    {
                        scopeSession.Channels[ChannelName].Measurement.ClearWaveformMeasurements();
                    }
                    Application.DoEvents();
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
            voltageHistogramDataGridView.Columns.Clear();
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
            resourceNameComboBox.Enabled = isEnabled;
            measurement1ComboBox.Enabled = isEnabled;
            measurement2ComboBox.Enabled = isEnabled;
            clearStatsCheckBox.Enabled = isEnabled;
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