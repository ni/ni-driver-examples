//=============================================================================================================
//
// Title:
//       Scope Synchronization Using TClock
//
// Description:
//       This application demonstrates how to synchronize two NI-SCOPE devices. The first scope device is 
//       triggered by an analog edge or is triggered immediately. The second scope device is synchronized 
//       with the first device. Data is fetched from both the devices and displayed on datagrids.
//
//=============================================================================================================

using System;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments;
using NationalInstruments.ModularInstruments.NIScope;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;
using NationalInstruments.ModularInstruments.SystemServices.TimingServices;

namespace NationalInstruments.Examples.ScopeSynchronizationUsingTClock
{
    public partial class MainForm : Form
    {
        NIScope scopeSession1;
        NIScope scopeSession2;
        TClock tClockSession;

        public MainForm()
        {
            InitializeComponent();
            ConfigureTriggerTypeComboBox();
            ConfigureTriggerSourceComboBox();
            LoadScopeDeviceNames();
            ChangeControlState(true);
        }

        #region Mainform initial configuration
        void ConfigureTriggerTypeComboBox()
        {
            triggerTypeComboBox.Items.Add(ScopeTriggerType.Edge);
            triggerTypeComboBox.Items.Add(ScopeTriggerType.Immediate);
            triggerTypeComboBox.SelectedIndex = 0;
        }

        void ConfigureTriggerSourceComboBox()
        {
            triggerSourceComboBox.Items.Add(ScopeTriggerSource.Channel0);
            triggerSourceComboBox.Items.Add(ScopeTriggerSource.Channel1);
            triggerSourceComboBox.Items.Add(ScopeTriggerSource.FromString("TRIG"));
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

        void LoadScopeDeviceNames()
        {
            using (ModularInstrumentsSystem scopeDevices = new ModularInstrumentsSystem("NI-Scope"))
            {
                foreach (DeviceInfo device in scopeDevices.DeviceCollection)
                {
                    resourceNameDevice1ComboBox.Items.Add(device.Name);
                    resourceNameDevice2ComboBox.Items.Add(device.Name);
                }
            }
            if (resourceNameDevice1ComboBox.Items.Count > 0)
            {
                resourceNameDevice1ComboBox.SelectedIndex = 0;
                resourceNameDevice2ComboBox.SelectedIndex = resourceNameDevice1ComboBox.Items.Count > 1 ? 1:0;
            }
        }
        #endregion

        #region Mainform configuration values
        string ResourceName1
        {
            get
            {
                return this.resourceNameDevice1ComboBox.Text;
            }
        }

        string ChannelName1
        {
            get
            {
                return this.channelNameDevice1TextBox.Text;
            }
        }

        string ResourceName2
        {
            get
            {
                return this.resourceNameDevice2ComboBox.Text;
            }
        }

        string ChannelName2
        {
            get
            {
                return this.channelNameDevice2TextBox.Text;
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
                return decimal.ToDouble(this.sampleRateMinNumeric.Value);
            }
        }

        int RecordLengthMin
        {
            get
            {
                return decimal.ToInt32(this.recordLengthMinNumeric.Value);
            }
        }

        double InputFrequencyMax
        {
            get
            {
                return decimal.ToDouble(this.maximumInputFrequencyNumeric.Value);
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
                return (ScopeTriggerSource)this.triggerSourceComboBox.SelectedItem;
            }
        }

        double TriggerLevel
        {
            get
            {
                return decimal.ToDouble(this.triggerLevelNumeric.Value);
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

        void IntializeSession()
        {
            scopeSession1 = new NIScope(ResourceName1, false, false);
            scopeSession1.DriverOperation.Warning += new EventHandler<ScopeWarningEventArgs>(DriverOperation_Warning);

            scopeSession2 = new NIScope(ResourceName2, false, false);
            scopeSession2.DriverOperation.Warning += new EventHandler<ScopeWarningEventArgs>(DriverOperation_Warning);

            ITClockSynchronizableDevice[] scopeSynchronizableDevices = new ITClockSynchronizableDevice[2] { scopeSession1, scopeSession2 };
            tClockSession = new TClock(scopeSynchronizableDevices);
        }

        void DriverOperation_Warning(object sender, ScopeWarningEventArgs e)
        {
            MessageBox.Show(e.Text, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        void StartAcquisition()
        {
            ChangeControlState(false);

            AnalogWaveformCollection<double> scope1Waveform = null;
            AnalogWaveformCollection<double> scope2Waveform = null;
            try
            {
                IntializeSession();

                // Configure Vertical and Horizontal parameters on both the devices.
                scopeSession1.Channels[ChannelName1].Enabled = true;
                scopeSession2.Channels[ChannelName2].Enabled = true;

                scopeSession1.Channels[ChannelName1].Range = VerticalRange;
                scopeSession2.Channels[ChannelName2].Range = VerticalRange;

                scopeSession1.Acquisition.SampleRateMin = SampleRateMin;
                scopeSession2.Acquisition.SampleRateMin = SampleRateMin;

                scopeSession1.Acquisition.NumberOfPointsMin = RecordLengthMin;
                scopeSession2.Acquisition.NumberOfPointsMin = RecordLengthMin;

                scopeSession1.Channels[ChannelName1].InputFrequencyMax = InputFrequencyMax;
                scopeSession2.Channels[ChannelName2].InputFrequencyMax = InputFrequencyMax;

                // NI-TClock does not support Random Interleaved Sampling ( RIS ).
                scopeSession1.Timing.EnforceRealtime = true;
                scopeSession2.Timing.EnforceRealtime = true;

                // Configure Triggering on the first NI-SCOPE device.
                scopeSession1.Trigger.Type = TriggerType;
                if (TriggerType == ScopeTriggerType.Edge)
                {
                    scopeSession1.Trigger.Level = TriggerLevel;
                    scopeSession1.Trigger.Source = TriggerSource;
                }

                tClockSession.ConfigureForHomogeneousTriggers();
                tClockSession.Synchronize();
                tClockSession.Initiate();

                long recordLength1 = scopeSession1.Acquisition.RecordLength;
                long recordLength2 = scopeSession2.Acquisition.RecordLength;

                PrecisionTimeSpan timeout = new PrecisionTimeSpan(5.0);
                scope1Waveform = scopeSession1.Channels[ChannelName1].Measurement.FetchDouble(timeout, recordLength1, scope1Waveform);
                scope2Waveform = scopeSession2.Channels[ChannelName2].Measurement.FetchDouble(timeout, recordLength2, scope2Waveform);

                PlotWaveforms(scope1DataGridView, scope1Waveform);
                PlotWaveforms(scope2DataGridView, scope2Waveform);
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
            scope1DataGridView.Columns.Clear();
            scope2DataGridView.Columns.Clear();
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
            scopeDevice1GroupBox.Enabled = isEnabled;
            scopeDevice2GroupBox.Enabled = isEnabled;
            commonConfigurationGroupBox.Enabled = isEnabled;
            triggeringGroupBox.Enabled = isEnabled;
            acquireButton.Enabled = isEnabled;
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
                if (scopeSession1 != null)
                {
                    scopeSession1.Close();
                    scopeSession1 = null;
                }
                if (scopeSession2 != null)
                {
                    scopeSession2.Close();
                    scopeSession2 = null;
                }
            }
            catch (Exception ex)
            {
                ShowError(ex);
                Application.Exit();
            }
            tClockSession = null;
        }

        static void ShowError(Exception ex)
        {
            MessageBox.Show(ex.Message, ex.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
