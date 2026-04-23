//========================================================================================================
//
// Title:
//        OSP Quadrature Downconversion
//
// Description:
//      This example shows how to program a quadrature downconversion acquisition. Digitizers support only
//      certain number of defined sample rates with DDC processing enabled, so if the value chosen is not a
//      valid rate, it is rounded to the next higher rate, called the actual sample rate.
//         
//=========================================================================================================

using System;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIScope;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.OspQuadratureDownconversion
{
    public partial class MainForm : Form
    {
        NIScope scopeSession;
        bool stop;

        public MainForm()
        {
            InitializeComponent();
            ConfigureTriggerTypeComboBox();
            ConfigureInputImpedanceComboBox();
            LoadScopeDeviceNames();
            ChangeControlState(true);
        }

        #region Mainform initial configuration
        void ConfigureTriggerTypeComboBox()
        {
            triggerTypeComboBox.Items.Add(ScopeTriggerType.Edge);
            triggerTypeComboBox.Items.Add(ScopeTriggerType.Immediate);
            triggerTypeComboBox.SelectedIndex = 1;
        }

        void ConfigureInputImpedanceComboBox()
        {
            inputImpedanceComboBox.Items.Add(50);
            inputImpedanceComboBox.Items.Add(1000000);
            inputImpedanceComboBox.SelectedIndex = 0;
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

        PrecisionTimeSpan Timeout
        {
            get
            {
                return PrecisionTimeSpan.FromSeconds(decimal.ToDouble(this.timeoutNumeric.Value));
            }
        }

        int InputImpedance
        {
            get
            {
                return (int)this.inputImpedanceComboBox.SelectedItem;
            }
        }

        double VerticalRange
        {
            get
            {
                return decimal.ToDouble(this.verticalRangeNumeric.Value);
            }
        }

        double DigitalGain
        {
            get
            {
                return decimal.ToDouble(this.digitalGainNumeric.Value);
            }
        }

        double CenterFrequency
        {
            get
            {
                return decimal.ToDouble(this.centerFrequencyNumeric.Value);
            }
        }

        double PhaseI
        {
            get
            {
                return decimal.ToDouble(this.phaseINumeric.Value);
            }
        }

        double PhaseQ
        {
            get
            {
                return decimal.ToDouble(this.phaseQNumeric.Value);
            }
        }

        double SampleRateMin
        {
            get
            {
                return decimal.ToDouble(this.sampleRateMinNumeric.Value);
            }
        }

        string ActualSamplerate
        {
            set
            {
                this.actualSampleRateTextBox.Text = value;
            }
        }

        int RecordLengthMin
        {
            get
            {
                return decimal.ToInt32(this.recordLengthMinNumeric.Value);
            }
        }

        string ActualRecordLength
        {
            set
            {
                this.actualRecordLengthTextBox.Text = value;
            }
        }

        bool FractionalResampleEnabled
        {
            get
            {
                return this.fracResampleEnabledCheckBox.Checked;
            }
        }

        ScopeTriggerType TriggerType
        {
            get
            {
                return (ScopeTriggerType)this.triggerTypeComboBox.SelectedItem;
            }
        }

        double TriggerLevel
        {
            get
            {
                return decimal.ToDouble(this.triggerLevelNumeric.Value);
            }
        }

        double TriggerMinQuietTime
        {
            get
            {
                return decimal.ToDouble(this.triggerMinQuietTimeNumeric.Value);
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

        void stopButton_Click(object sender, EventArgs e)
        {
            StopAcquisition();
        }

        void StartAcquisition()
        {
            stop = false;
            ChangeControlState(false);
            DisplayMessage("Acquisition is in progress...");

            AnalogWaveformCollection<double> waveforms = null;
            try
            {
                InitializeSession();
                while (!stop)
                {
                    scopeSession.Channels[ChannelName].Range = VerticalRange;
                    scopeSession.Channels[ChannelName].Enabled = true;
                    scopeSession.Timing.FractionalResample.Enabled = FractionalResampleEnabled;

                    double inputFrequencyMaximum = -1.0;
                    scopeSession.Channels[ChannelName].ConfigureCharacteristics(InputImpedance, inputFrequencyMaximum);

                    double referencePosition = 50.0;
                    int numberOfRecords = 1;
                    bool enforceRealtime = true;
                    scopeSession.Timing.ConfigureTiming(SampleRateMin, RecordLengthMin, referencePosition, numberOfRecords, enforceRealtime);

                    scopeSession.Channels[ChannelName].OnboardSignalProcessing.Ddc.Enabled = true;
                    scopeSession.Channels[ChannelName].OnboardSignalProcessing.Ddc.FrequencyTranslationEnabled = true;
                    scopeSession.Channels[ChannelName].OnboardSignalProcessing.Ddc.CenterFrequency = CenterFrequency;
                    scopeSession.Channels[ChannelName].OnboardSignalProcessing.Ddc.FrequencyTranslationPhaseI = PhaseI;
                    scopeSession.Channels[ChannelName].OnboardSignalProcessing.Ddc.FrequencyTranslationPhaseQ = PhaseQ;

                    scopeSession.Acquisition.DdcDataProcessingMode = ScopeDdcDataProcessingMode.Complex;
                    scopeSession.Acquisition.OverflowErrorReportingMode = ScopeOverflowErrorReportingMode.Warning;
                    scopeSession.Trigger.ReferenceTrigger.DetectorLocation = ScopeReferenceTriggerDetectorLocation.DdcOutput;
                    scopeSession.Trigger.ReferenceTrigger.QuietTimeMin = PrecisionTimeSpan.FromSeconds(TriggerMinQuietTime);

                    if (TriggerType == ScopeTriggerType.Immediate)
                    {
                        scopeSession.Trigger.ConfigureTriggerImmediate();
                    }
                    else if (TriggerType == ScopeTriggerType.Edge)
                    {
                        ScopeTriggerSource triggerSource = ScopeTriggerSource.Channel0;
                        PrecisionTimeSpan triggerHoldoff = PrecisionTimeSpan.Zero;
                        PrecisionTimeSpan triggerDelay = PrecisionTimeSpan.Zero;
                        ScopeTriggerSlope triggerSlope = ScopeTriggerSlope.Positive;
                        ScopeTriggerCoupling triggerCoupling = ScopeTriggerCoupling.DC;
                        scopeSession.Trigger.EdgeTrigger.Configure(triggerSource, TriggerLevel, triggerSlope, triggerCoupling, triggerHoldoff, triggerDelay);
                    }

                    scopeSession.Measurement.FetchInterleavedIQData = false;
                    scopeSession.Measurement.Initiate();

                    long actualRecLength = scopeSession.Acquisition.RecordLength;
                    waveforms = scopeSession.Channels[ChannelName].Measurement.FetchDouble(Timeout, actualRecLength, waveforms);

                    PlotWaveforms(acquisitionDataGridView, waveforms);
                    ActualRecordLength = scopeSession.Acquisition.RecordLength.ToString();
                    ActualSamplerate = scopeSession.Acquisition.SampleRate.ToString("E");
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
            acquisitionDataGridView.Columns.Clear();
        }

        void StopAcquisition()
        {
            if (!stop)
            {
                DisplayMessage("Stop in progress...Fetched points are being plotted...");
                stop = true;
            }
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