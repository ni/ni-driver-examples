//=======================================================================================================
//
// Title:
//      External Clocking
//
// Description:
//      This example demonstrates the external clock feature of the digitizer. In this mode,
//      an external signal is used to run the instrument. This external signal must have
//      frequency of at least 30Mhz and at most 105Mhz.       
//
//======================================================================================================

using System;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIScope;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.ExternalClocking
{
    public partial class MainForm : Form
    {
        NIScope scopeSession;
        bool stop;

        public MainForm()
        {
            InitializeComponent();
            ConfigureTimebaseSourceComboBox();
            LoadScopeDeviceNames();
            ChangeControlState(true);
        }

        #region Mainform initial configuration
        void ConfigureTimebaseSourceComboBox()
        {
            timebaseSourceComboBox.Items.Add(ScopeSampleClockTimebaseSource.ClockIn);
            timebaseSourceComboBox.Items.Add(ScopeSampleClockTimebaseSource.PxiStar);
            timebaseSourceComboBox.Items.Add(ScopeSampleClockTimebaseSource.Pfi0);
            timebaseSourceComboBox.Items.Add(ScopeSampleClockTimebaseSource.Pfi1);
            timebaseSourceComboBox.SelectedIndex = 0;
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

        int RecordLengthMin
        {
            get
            {
                return decimal.ToInt32(this.recordLengthMinNumeric.Value);
            }
        }

        ScopeSampleClockTimebaseSource TimebaseSource
        {
            get
            {
                return (ScopeSampleClockTimebaseSource)this.timebaseSourceComboBox.SelectedItem;
            }
        }

        double TimebaseRate
        {
            get
            {
                return decimal.ToDouble(this.timebaseRateNumeric.Value);
            }
        }

        int TimebaseDivisor
        {
            get
            {
                return decimal.ToInt32(this.timebaseDivisorNumeric.Value);
            }
        }

        int TimebaseMultiplier
        {
            get
            {
                return decimal.ToInt32(this.timebaseMultiplierNumeric.Value);
            }
        }

        double ActualSampleRate
        {
            set
            {
                this.actualSampleRateTextBox.Text = value.ToString("E");
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
            StopAcquistion();
        }

        void StartAcquisition()
        {
            stop = false;
            ChangeControlState(false);
            DisplayMessage("Acquisition is in progress...");

            PrecisionTimeSpan timeout = new PrecisionTimeSpan(5.0);
            PrecisionTimeSpan holdOff = PrecisionTimeSpan.Zero;
            AnalogWaveformCollection<double> scaledWaveForms = null;
            try
            {
                InitializeSession();

                // Configure the vertical parameters.
                ScopeVerticalCoupling verticalCoupling = ScopeVerticalCoupling.DC;
                double probeAttenuation = 1.0;
                scopeSession.Channels[ChannelName].Configure(VerticalRange, VerticalOffset, verticalCoupling, probeAttenuation, true);

                // Configure the record length and reference position.
                double referencePosition = 50.0;
                scopeSession.Acquisition.NumberOfPointsMin = RecordLengthMin;
                scopeSession.Trigger.ReferenceTrigger.ReferencePosition = referencePosition;

                // Configure immediate triggering.
                scopeSession.Trigger.ConfigureTriggerImmediate();

                // Configure the external clocking attributes.
                scopeSession.Timing.SampleClockTimebaseSource = TimebaseSource;
                scopeSession.Timing.SampleClockTimebaseDivisor = TimebaseDivisor;
                scopeSession.Timing.SampleClockTimebaseRate = TimebaseRate;
                scopeSession.Timing.SampleClockTimebaseMultiplier = TimebaseMultiplier;

                // Query the coerced record length.
                long actualRecordLength = scopeSession.Acquisition.RecordLength;

                // // Query the actual sample rate.
                double actualSampRate = scopeSession.Acquisition.SampleRate;
                ActualSampleRate = actualSampRate;

                // Loop until the stop flag is set.  This example loops around initiate acquisition
                // and fetch, using the same configuration for every acquisition.  This is the 
                // most efficient method for acquiring multiple waveforms with the same configuration
                // parameters. (Although typically, you would scale the data at a later time.)
                while (!stop)
                {
                    scaledWaveForms = scopeSession.Channels[ChannelName].Measurement.Read(timeout, actualRecordLength, scaledWaveForms);
                    PlotWaveforms(scaledDataGridView, scaledWaveForms);
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

        void StopAcquistion()
        {
            if (!stop)
            {
                DisplayMessage("Stop in progress...Fetched points are being plotted...");
                stop = true;
            }
        }

        void ClearWaveforms()
        {
            scaledDataGridView.Columns.Clear();
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
            verticalAndHorizontalGroupBox.Enabled = isEnabled;
            externalClockGroupBox.Enabled = isEnabled;
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