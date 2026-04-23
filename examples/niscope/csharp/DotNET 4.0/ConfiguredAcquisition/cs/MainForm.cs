//==================================================================================================
//
// Title:
//        Configured Acquisition
//
// Description:
//     This application demonstrates acquiring data from multiple channels of a high-speed 
//     digitizer device. The application allows you to configure various Horizontal and Vertical
//     parameters along with the Trigger parameters. The fetched data is plotted onto a datagrid.  
//
//=================================================================================================

using System;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIScope;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.ConfiguredAcquisition
{
    public partial class MainForm : Form
    {
        NIScope scopeSession;
        bool stop;

        public MainForm()
        {
            InitializeComponent();
            ConfigureAcquisitionTypeComboBox();
            ConfigureVerticalCouplingComboBox();
            ConfigureInputImpedanceComboBox();
            ConfigureTriggerTypeComboBox();
            ConfigureTriggerSourceComboBox();
            ConfigureTriggerCouplingComboBox();
            ConfigureTriggerSlopeComboBox();
            ConfigureWindowModeComboBox();
            LoadScopeDeviceNames();
            ChangeControlState(true);
        }

        #region Mainform initial configuration
        void ConfigureAcquisitionTypeComboBox()
        {
            acquisitionTypeComboBox.Items.Add(ScopeAcquisitionType.Normal);
            acquisitionTypeComboBox.Items.Add(ScopeAcquisitionType.FlexibleResolution);
            acquisitionTypeComboBox.SelectedIndex = 0;
        }

        void ConfigureVerticalCouplingComboBox()
        {
            foreach (var value in Enum.GetValues(typeof(ScopeVerticalCoupling)))
            {
                verticalCouplingComboBox.Items.Add(value);
            }
            verticalCouplingComboBox.SelectedIndex = 1;
        }

        void ConfigureInputImpedanceComboBox()
        {
            inputImpedanceComboBox.Items.Add(50);
            inputImpedanceComboBox.Items.Add(1000000);
            inputImpedanceComboBox.SelectedIndex = 1;
        }

        void ConfigureTriggerTypeComboBox()
        {
            triggerTypeComboBox.Items.Add(ScopeTriggerType.DigitalEdge);
            triggerTypeComboBox.Items.Add(ScopeTriggerType.Edge);
            triggerTypeComboBox.Items.Add(ScopeTriggerType.Hysteresis);
            triggerTypeComboBox.Items.Add(ScopeTriggerType.Immediate);
            triggerTypeComboBox.Items.Add(ScopeTriggerType.Window);
            triggerTypeComboBox.SelectedIndex = 3;
        }

        void ConfigureTriggerSourceComboBox()
        {
            triggerSourceComboBox.Items.Add(ScopeTriggerSource.Channel0);
            triggerSourceComboBox.Items.Add(ScopeTriggerSource.Channel1);
            triggerSourceComboBox.Items.Add(ScopeTriggerSource.Channel2);
            triggerSourceComboBox.Items.Add(ScopeTriggerSource.Channel3);
            triggerSourceComboBox.Items.Add(ScopeTriggerSource.Channel4);
            triggerSourceComboBox.Items.Add(ScopeTriggerSource.Channel5);
            triggerSourceComboBox.Items.Add(ScopeTriggerSource.Channel6);
            triggerSourceComboBox.Items.Add(ScopeTriggerSource.Channel7);
            triggerSourceComboBox.Items.Add(ScopeTriggerSource.External);
            triggerSourceComboBox.Items.Add(ScopeTriggerSource.Rtsi0);
            triggerSourceComboBox.Items.Add(ScopeTriggerSource.Rtsi1);
            triggerSourceComboBox.Items.Add(ScopeTriggerSource.Rtsi2);
            triggerSourceComboBox.Items.Add(ScopeTriggerSource.Rtsi3);
            triggerSourceComboBox.Items.Add(ScopeTriggerSource.Rtsi4);
            triggerSourceComboBox.Items.Add(ScopeTriggerSource.Rtsi5);
            triggerSourceComboBox.Items.Add(ScopeTriggerSource.Rtsi6);
            triggerSourceComboBox.Items.Add(ScopeTriggerSource.Pfi0);
            triggerSourceComboBox.Items.Add(ScopeTriggerSource.Pfi1);
            triggerSourceComboBox.Items.Add(ScopeTriggerSource.Pfi2);
            triggerSourceComboBox.Items.Add(ScopeTriggerSource.PxiStar);
            triggerSourceComboBox.SelectedIndex = 0;
        }

        void ConfigureTriggerCouplingComboBox()
        {
            foreach (var value in Enum.GetValues(typeof(ScopeTriggerCoupling)))
            {
                triggerCouplingComboBox.Items.Add(value);
            }
            triggerCouplingComboBox.SelectedIndex = 1;
        }

        void ConfigureTriggerSlopeComboBox()
        {
            foreach (var value in Enum.GetValues(typeof(ScopeTriggerSlope)))
            {
                triggerSlopeComboBox.Items.Add(value);
            }
            triggerSlopeComboBox.SelectedIndex = 1;
        }

        void ConfigureWindowModeComboBox()
        {
            foreach (var value in Enum.GetValues(typeof(ScopeWindowTriggerMode)))
            {
                windowModeComboBox.Items.Add(value);
            }
            windowModeComboBox.SelectedIndex = 0;
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

        ScopeAcquisitionType AcquisitionType
        {
            get
            {
                return (ScopeAcquisitionType)this.acquisitionTypeComboBox.SelectedItem;
            }
        }

        double Range
        {
            get
            {
                return decimal.ToDouble(this.verticalRangeNumeric.Value);
            }
        }

        double Offset
        {
            get
            {
                return decimal.ToDouble(this.verticalOffsetNumeric.Value);
            }
        }

        double ProbeAttenuation
        {
            get
            {
                return decimal.ToDouble(this.probeAttenuationNumeric.Value);
            }
        }

        ScopeVerticalCoupling Coupling
        {
            get
            {
                return (ScopeVerticalCoupling)this.verticalCouplingComboBox.SelectedItem;
            }
        }

        double SampleRateMin
        {
            get
            {
                return decimal.ToDouble(this.minSampleRateNumeric.Value);
            }
        }

        string SampleRate
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
                return decimal.ToInt32(this.minRecordLengthNumeric.Value);
            }
        }

        string RecordLength
        {
            set
            {
                this.actualRecordLengthTextBox.Text = value;
            }
        }

        int NumberOfRecords
        {
            get
            {
                return decimal.ToInt32(this.numRecordsNumeric.Value);
            }
        }

        double InputFrequencyMax
        {
            get
            {
                return decimal.ToDouble(this.maxInputFrequencyNumeric.Value);
            }
        }

        int InputImpedance
        {
            get
            {
                return (int)this.inputImpedanceComboBox.SelectedItem;
            }
        }

        ScopeTriggerType TriggerType
        {
            get
            {
                return (ScopeTriggerType)this.triggerTypeComboBox.SelectedItem;
            }
        }

        ScopeTriggerSource TriggerSource
        {
            get
            {
                return (ScopeTriggerSource)this.triggerSourceComboBox.SelectedItem.ToString();
            }
        }

        double ReferencePosition
        {
            get
            {
                return decimal.ToDouble(this.referencePositionNumeric.Value);
            }
        }

        PrecisionTimeSpan HoldOff
        {
            get
            {
                return PrecisionTimeSpan.FromSeconds(decimal.ToDouble(this.triggerHoldoffNumeric.Value));
            }
        }

        PrecisionTimeSpan Delay
        {
            get
            {
                return PrecisionTimeSpan.FromSeconds(decimal.ToDouble(this.triggerDelayNumeric.Value));
            }
        }

        double Level
        {
            get
            {
                return decimal.ToDouble(this.triggerLevelNumeric.Value);
            }
        }

        ScopeTriggerCoupling TriggerCoupling
        {
            get
            {
                return (ScopeTriggerCoupling)this.triggerCouplingComboBox.SelectedItem;
            }
        }

        ScopeTriggerSlope Slope
        {
            get
            {
                return (ScopeTriggerSlope)this.triggerSlopeComboBox.SelectedItem;
            }
        }

        double Hysteresis
        {
            get
            {
                return decimal.ToDouble(this.hysteresisNumeric.Value);
            }
        }

        ScopeWindowTriggerMode Mode
        {
            get
            {
                return (ScopeWindowTriggerMode)this.windowModeComboBox.SelectedItem;
            }
        }

        double Low
        {
            get
            {
                return decimal.ToDouble(this.lowLevelWindowNumeric.Value);
            }
        }

        double High
        {
            get
            {
                return decimal.ToDouble(this.highLevelWindowNumeric.Value);
            }
        }

        bool EnforceRealTime
        {
            get
            {
                return enforceRealtimecheckBox.Checked;
            }
        }

        bool EnableTimeInterleavedSampling
        {
            get
            {
                return enableTimeInterleavedSamplingCheckBox.Checked;
            }
        }
        #endregion

        void acquireButton_Click(object sender, System.EventArgs e)
        {
            ConfigureAndStartAcquisition();
        }

        void mainForm_closing(object obj, FormClosingEventArgs e)
        {
            CloseSession();
        }

        void stopButton_Click(object sender, System.EventArgs e)
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

        void ConfigureAndStartAcquisition()
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
                    scopeSession.Acquisition.Type = AcquisitionType;
                    scopeSession.Channels[ChannelName].Configure(Range, Offset, Coupling, ProbeAttenuation, true);
                    scopeSession.Channels[ChannelName].ConfigureCharacteristics(InputImpedance, InputFrequencyMax);
                    scopeSession.Timing.ConfigureTiming(SampleRateMin, RecordLengthMin, ReferencePosition, NumberOfRecords, EnforceRealTime);
                    scopeSession.Channels[ChannelName].EnableTimeInterleavedSampling = EnableTimeInterleavedSampling;

                    switch (TriggerType)
                    {
                        case ScopeTriggerType.Edge:
                            scopeSession.Trigger.EdgeTrigger.Configure(TriggerSource, Level, Slope, TriggerCoupling, HoldOff, Delay);
                            break;
                        case ScopeTriggerType.Hysteresis:
                            scopeSession.Trigger.ConfigureTriggerHysteresis(TriggerSource, Level, Hysteresis, Slope, TriggerCoupling, HoldOff, Delay);
                            break;
                        case ScopeTriggerType.Immediate:
                            scopeSession.Trigger.ConfigureTriggerImmediate();
                            break;
                        case ScopeTriggerType.DigitalEdge:
                            scopeSession.Trigger.ConfigureTriggerDigital(TriggerSource, Slope, HoldOff, Delay);
                            break;
                        case ScopeTriggerType.Window:
                            scopeSession.Trigger.ConfigureTriggerWindow(TriggerSource, Low, High, Mode, TriggerCoupling, HoldOff, Delay);
                            break;
                    }

                    scopeSession.Measurement.Initiate();
                    waveforms = scopeSession.Channels[ChannelName].Measurement.FetchDouble(Timeout, -1, waveforms);

                    PlotWaveforms(sampledDataGridView, waveforms);
                    SampleRate = scopeSession.Acquisition.SampleRate.ToString("E");
                    RecordLength = scopeSession.Acquisition.RecordLength.ToString();
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