//=====================================================================================================
//
// Title:
//        Flexible Resolution
//
// Description:
//      This example configures a flexible resolution acquisition with the Configure Acquisition function.
//      The vertical and horizontal parameters are configured as always.  The sampling rate used in this 
//      example will be coerced to a sampling rate supported in flexible resolution.  This example also 
//      displays the effective number of bits at each sampling rate.
//
//==================================================================================================

using System;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIScope;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.FlexibleResolution
{
    public partial class MainForm : Form
    {
        readonly string ChannelName = "0";

        NIScope scopeSession;
        bool stop;

        public MainForm()
        {
            InitializeComponent();
            ConfigureTriggerTypeComboBox();
            LoadScopeDeviceNames();
            ChangeControlState(true);
        }

        #region Mainform initial configuration
        void ConfigureTriggerTypeComboBox()
        {
            triggerTypeComboBox.Items.Add(ScopeTriggerType.Immediate);
            triggerTypeComboBox.Items.Add(ScopeTriggerType.Edge);
            triggerTypeComboBox.SelectedIndex = 0;
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

        double SampleRateMin
        {
            get
            {
                return decimal.ToDouble(this.minSampleRateNumeric.Value);
            }
        }

        string ActualSamplerate
        {
            set
            {
                this.actualSampleRateTextBox.Text = value;
            }
        }

        string Resolution
        {
            set
            {
                this.resolutionTextBox.Text = value;
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
            StopAcquisition();
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

            AnalogWaveformCollection<double> waveforms = null;
            ScopeWaveformInfo[] waveformInfo;
            AnalogWaveformCollection<double> freqwaveforms = null;
            ScopeWaveformInfo[] freqwaveformInfo;

            try
            {
                InitializeSession();
                scopeSession.Channels[ChannelName].Measurement.AddWaveformProcessing(ScopeArrayMeasurementType.HanningWindow);

                while (!stop)
                {
                    scopeSession.Acquisition.Type = ScopeAcquisitionType.FlexibleResolution;

                    // Configure the vertical parameters.
                    double offset = 0.0;
                    double probeAttenuation = 1.0;
                    ScopeVerticalCoupling coupling = ScopeVerticalCoupling.DC;
                    scopeSession.Channels[ScopeTriggerSource.Channel0].Configure(VerticalRange, offset, coupling, probeAttenuation, true);

                    // Configure the horizontal parameters.
                    double referencePosition = 50.0;
                    Int32 numberOfRecords = 1;
                    Boolean enforceRealTime = false;
                    scopeSession.Timing.ConfigureTiming(SampleRateMin, RecordLengthMin, referencePosition, numberOfRecords, enforceRealTime);

                    if (TriggerType == ScopeTriggerType.Edge)
                    {
                        double triggerLevel = 0.0;
                        ScopeTriggerSlope triggerSlope = ScopeTriggerSlope.Positive;
                        ScopeTriggerCoupling triggerCoupling = ScopeTriggerCoupling.DC;
                        ScopeTriggerSource triggerSource = ScopeTriggerSource.Channel0;
                        PrecisionTimeSpan triggerHoldoff = PrecisionTimeSpan.Zero;
                        PrecisionTimeSpan triggerDelay = PrecisionTimeSpan.Zero;
                        scopeSession.Trigger.EdgeTrigger.Configure(triggerSource, triggerLevel, triggerSlope, triggerCoupling, triggerHoldoff, triggerDelay);
                    }
                    else
                    {
                        scopeSession.Trigger.ConfigureTriggerImmediate();
                    }

                    PrecisionTimeSpan timeout = new PrecisionTimeSpan(5.0);

                    scopeSession.Measurement.Initiate();
                    long actualRecordLength = scopeSession.Acquisition.RecordLength;
                    waveforms = scopeSession.Channels[ScopeTriggerSource.Channel0].Measurement.FetchDouble(timeout, actualRecordLength, waveforms, out waveformInfo);
                    freqwaveforms = scopeSession.Channels[ScopeTriggerSource.Channel0].Measurement.FetchArrayMeasurement(timeout, ScopeArrayMeasurementType.FftAmplitudeSpectrumDB, freqwaveforms, out freqwaveformInfo);

                    ActualSamplerate = scopeSession.Acquisition.SampleRate.ToString();
                    Resolution = scopeSession.Acquisition.Resolution.ToString();

                    PlotWaveforms(sampledDataGridView, waveforms);
                    PlotWaveforms(measurementDataGridView, freqwaveforms);
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
            this.Refresh();
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

        void StopAcquisition()
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