//==================================================================================================
//
// Title:
//      Basic Measurement Library
//
// Description:
//      This example illustrates fetching scalar measurements from NI-SCOPE, 
//      such as period and rise time calculations. Some common horizontal and vertical 
//      parameters are configured for the digitizer. 
//
//==================================================================================================

using System;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIScope;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.BasicMeasurementLibrary
{
    public partial class MainForm : Form
    {
        NIScope scopeSession;
        bool stop;

        public MainForm()
        {
            InitializeComponent();
            ConfigureTriggerTypeComboBox();
            ConfigureTriggerSlopeComboBox();
            ConfigureTriggerSourceComboBox();
            ConfigureTriggerCouplingComboBox();
            ConfigureScalarMeasurementComboBox();
            LoadScopeDeviceNames();
            ChangeControlState(true);
        }

        #region Mainform Initial Configuration
        void ConfigureTriggerTypeComboBox()
        {
            triggerTypeComboBox.Items.Add(ScopeTriggerType.Edge);
            triggerTypeComboBox.Items.Add(ScopeTriggerType.Immediate);
            triggerTypeComboBox.SelectedIndex = 1;
        }

        void ConfigureTriggerSlopeComboBox()
        {
            foreach (ScopeTriggerSlope value in Enum.GetValues(typeof(ScopeTriggerSlope)))
            {
                triggerSlopeComboBox.Items.Add(value);
            }
            triggerSlopeComboBox.SelectedIndex = 1;
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
            triggerSourceComboBox.SelectedIndex = 0;
        }

        void ConfigureTriggerCouplingComboBox()
        {
            foreach (ScopeTriggerCoupling value in Enum.GetValues(typeof(ScopeTriggerCoupling)))
            {
                triggerCouplingComboBox.Items.Add(value);
            }
            triggerCouplingComboBox.SelectedIndex = 1;
        }

        void ConfigureScalarMeasurementComboBox()
        {
            foreach (ScopeScalarMeasurementType value in Enum.GetValues(typeof(ScopeScalarMeasurementType)))
            {
                scalarMeasurementComboBox.Items.Add(value);
            }
            scalarMeasurementComboBox.SelectedIndex = 4;
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
                return decimal.ToInt32(this.minRecordLengthNumeric.Value);
            }
        }

        ScopeTriggerType TriggerType
        {
            get
            {
                return (ScopeTriggerType)this.triggerTypeComboBox.SelectedItem;
            }
        }

        PrecisionTimeSpan Timeout
        {
            get
            {
                return PrecisionTimeSpan.FromSeconds(decimal.ToDouble(this.maxTimeNumeric.Value));
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

        ScopeTriggerSlope TriggerSlope
        {
            get
            {
                return (ScopeTriggerSlope)this.triggerSlopeComboBox.SelectedItem;
            }
        }

        ScopeTriggerCoupling TriggerCoupling
        {
            get
            {
                return (ScopeTriggerCoupling)this.triggerCouplingComboBox.SelectedItem;
            }
        }

        ScopeScalarMeasurementType ScalarMeasurement
        {
            get
            {
                return (ScopeScalarMeasurementType)this.scalarMeasurementComboBox.SelectedItem;
            }
        }

        double LowReference
        {
            get
            {
                return Convert.ToDouble(this.lowReferenceNumeric.Value);
            }
        }

        double MiddleReference
        {
            get
            {
                return Convert.ToDouble(this.middleReferenceNumeric.Value);
            }
        }

        double HighReference
        {
            get
            {
                return Convert.ToDouble(this.highReferenceNumeric.Value);
            }
        }

        bool EnforceRealTime
        {
            get
            {
                return enforceRealTimeCheckBox.Checked;
            }
        }

        string ScalarResult
        {
            set
            {
                this.scalarResultTextBox.Text = value;
            }
        }

        string Mean
        {
            set
            {
                this.meanTextBox.Text = value;
            }
        }

        string StandardDeviation
        {
            set
            {
                this.standardDeviationTextBox.Text = value;
            }
        }

        string Minimum
        {
            set
            {
                this.minimumTextBox.Text = value;
            }
        }

        string Maximum
        {
            set
            {
                this.maximumTextBox.Text = value;
            }
        }

        string NumberInStatistics
        {
            set
            {
                this.numberInStatsTextBox.Text = value;
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

        void clearStatisticsButton_Click(object sender, EventArgs e)
        {
            ClearStatistics();
        }

        void ClearStatistics()
        {
            scopeSession.Channels[ChannelName].Measurement.ClearWaveformMeasurements(ScalarMeasurement);
        }

        void StartAcquisition()
        {
            stop = false;
            ChangeControlState(false);

            AnalogWaveformCollection<double> waveform = null;
            ScopeScalarMeasurementStatistics[] measurementStatistics = null;
            double[] data = null;
            try
            {
                InitializeSession();
                scopeSession.Acquisition.Type = ScopeAcquisitionType.Normal;

                // Configure the vertical parameters.
                ScopeVerticalCoupling verticalCoupling = ScopeVerticalCoupling.DC;
                double verticalOffset = 0.0;
                double probeAttenuation = 1.0;
                bool channelEnabled = true;
                scopeSession.Channels[ChannelName].Configure(VerticalRange, verticalOffset, verticalCoupling, probeAttenuation, channelEnabled);

                // Configure the horizontal parameters.
                double referencePosition = 50.0;
                int numberOfRecords = 1;
                scopeSession.Timing.ConfigureTiming(SampleRateMin, RecordLengthMin, referencePosition, numberOfRecords, EnforceRealTime);

                // Configure the trigger.
                if (TriggerType == ScopeTriggerType.Edge)
                {
                    PrecisionTimeSpan holdOff = PrecisionTimeSpan.Zero;
                    PrecisionTimeSpan delay = PrecisionTimeSpan.Zero;
                    scopeSession.Trigger.EdgeTrigger.Configure(TriggerSource, TriggerLevel, TriggerSlope, TriggerCoupling, holdOff, delay);
                }

                while (!stop)
                {
                    scopeSession.Measurement.Initiate();
                    scopeSession.Channels[ChannelName].Measurement.ReferenceLevel.Low = LowReference;
                    scopeSession.Channels[ChannelName].Measurement.ReferenceLevel.Mid = MiddleReference;
                    scopeSession.Channels[ChannelName].Measurement.ReferenceLevel.High = HighReference;

                    // Acquire data from the NI-Scope device
                    // Get measurement data and statistics.
                    waveform = scopeSession.Channels[ChannelName].Measurement.FetchDouble(Timeout, RecordLengthMin, waveform);
                    data = scopeSession.Channels[ChannelName].Measurement.FetchScalarMeasurement(Timeout, ScalarMeasurement);
                    measurementStatistics = scopeSession.Channels[ChannelName].Measurement.FetchScalarMeasurementStatistics(Timeout, ScalarMeasurement);

                    DisplayResults(data, measurementStatistics);
                }
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

        void DisplayResults(double[] data, ScopeScalarMeasurementStatistics[] measurementStatistics)
        {
            int currentChannelIndex = 0;
            ScalarResult = data[currentChannelIndex].ToString("E");
            Mean = measurementStatistics[currentChannelIndex].Mean.ToString("E");
            StandardDeviation = measurementStatistics[currentChannelIndex].StandardDeviation.ToString("E");
            Maximum = measurementStatistics[currentChannelIndex].Max.ToString("E");
            Minimum = measurementStatistics[currentChannelIndex].Min.ToString("E");
            NumberInStatistics = measurementStatistics[currentChannelIndex].StatisticsCount.ToString();
            Application.DoEvents();
        }

        void InitializeSession()
        {
            scopeSession = new NIScope(ResourceName, false, false);
            scopeSession.DriverOperation.Warning += new EventHandler<ScopeWarningEventArgs>(DriverOperation_Warning);
        }

        void DriverOperation_Warning(object sender, ScopeWarningEventArgs e)
        {
            MessageBox.Show(e.Text, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        void StopAcquisition()
        {
            if (!stop)
            {
                stop = true;
            }
        }

        void ChangeControlState(bool isEnabled)
        {
            generalGroupBox.Enabled = isEnabled;
            timingGroupBox.Enabled = isEnabled;
            triggeringGroupBox.Enabled = isEnabled;
            edgeTriggerGroupBox.Enabled = isEnabled;
            acquireButton.Enabled = isEnabled;
            clearStatisticsButton.Enabled = !isEnabled;
            stopButton.Enabled = !isEnabled;
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

        static void ShowError(Exception ex)
        {
            MessageBox.Show(ex.Message, ex.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}