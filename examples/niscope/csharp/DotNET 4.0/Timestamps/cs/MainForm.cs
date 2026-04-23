
//==================================================================================================
//
// Title:
//      Timestamps
//
// Description:
//      This example demonstrates the timestamping ability of some National Instruments
//      digitizers by creating a histogram of the time between triggers in a multi-record
//      acquisition. It includes code to create a histogram of the time between triggers.
//
//==================================================================================================

using System;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIScope;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.Timestamps
{
    public partial class MainForm : Form
    {
        NIScope scopeSession;

        public MainForm()
        {
            InitializeComponent();
            ConfiguretriggertypeComboBox();
            ConfiguretriggersourceComboBox();
            LoadScopeDeviceNames();
            ChangeControlState(true);
        }

        #region MainForm initial configuration
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

        void ConfiguretriggertypeComboBox()
        {
            triggerTypeComboBox.Items.Add(ScopeTriggerType.Edge);
            triggerTypeComboBox.Items.Add(ScopeTriggerType.DigitalEdge);
            triggerTypeComboBox.SelectedIndex = 0;
        }

        void ConfiguretriggersourceComboBox()
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

        double SampleRateMin
        {
            get
            {
                return decimal.ToDouble(this.sampleRateMinNumeric.Value);
            }
        }

        double VerticalRange
        {
            get
            {
                return decimal.ToDouble(this.verticalRangeNumeric.Value);
            }
        }

        int NumberOfRecords
        {
            get
            {
                return decimal.ToInt32(this.numberOfRecordsNumeric.Value);
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

        PrecisionTimeSpan HoldOff
        {
            get
            {
                return PrecisionTimeSpan.FromSeconds(decimal.ToDouble(this.triggerHoldoffNumeric.Value));
            }
        }

        string MeanTime
        {
            set
            {
                this.meanTimeTextBox.Text = value;
            }
        }

        string StandardDeviation
        {
            set
            {
                this.standardDeviationTextBox.Text = value;
            }
        }

        string MeanFrequency
        {
            set
            {
                this.meanfrequencyTextBox.Text = value;
            }
        }
        #endregion

        void acquireButton_Click(object sender, EventArgs e)
        {
            Timestamp();
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
            MessageBox.Show(e.Text, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        void Timestamp()
        {
            ChangeControlState(false);

            try
            {
                InitializeSession();

                // Configure the vertical parameters.
                double verticalOffset = 0.0;
                ScopeVerticalCoupling verticalCoupling = ScopeVerticalCoupling.DC;
                double probeAttenuation = 1.0;
                scopeSession.Channels[ChannelName].Configure(VerticalRange, verticalOffset, verticalCoupling, probeAttenuation, true);

                // Configure the horizontal parameters, with the specified number of records and 1 point only.
                int recordLengthMin = 1;
                double referencePosition = 50.0;
                bool enforceRealtime = true;
                scopeSession.Timing.ConfigureTiming(SampleRateMin, recordLengthMin, referencePosition, NumberOfRecords, enforceRealtime);

                // Configure the trigger.
                double triggerLevel = 0.0;
                ScopeTriggerSlope triggerSlope = ScopeTriggerSlope.Positive;
                PrecisionTimeSpan triggerDelay = PrecisionTimeSpan.Zero;
                ScopeTriggerCoupling triggerCoupling = ScopeTriggerCoupling.DC;
                switch (TriggerType)
                {
                    case ScopeTriggerType.Edge:
                        scopeSession.Trigger.EdgeTrigger.Configure(TriggerSource, triggerLevel, triggerSlope, triggerCoupling, HoldOff, triggerDelay);
                        break;

                    case ScopeTriggerType.DigitalEdge:
                        scopeSession.Trigger.ConfigureTriggerDigital(TriggerSource, triggerSlope, HoldOff, triggerDelay);
                        break;
                }

                // Fetch only one record at a time.
                scopeSession.Acquisition.NumberOfRecordsToFetch = 1;

                scopeSession.Measurement.Initiate();

                ScopeWaveformInfo[] waveformInfo;
                PrecisionTimeSpan timeOut = new PrecisionTimeSpan(5.0);
                double[] triggerIntervals = new double[NumberOfRecords];
                double previousTriggerTime = 0.0;
                for (int recordNumber = 0; recordNumber < NumberOfRecords; recordNumber++)
                {
                    // Fetch only one record at a time.
                    scopeSession.Acquisition.RecordNumberToFetch = recordNumber;

                    // Fetch the timestamps, no data.
                    scopeSession.Channels[ChannelName].Measurement.FetchInt32(timeOut, 0, null, out waveformInfo);

                    double triggerTime = waveformInfo[0].AbsoluteInitialX - waveformInfo[0].RelativeInitialX;
                    triggerIntervals[recordNumber] = triggerTime - previousTriggerTime;
                    previousTriggerTime = triggerTime;
                }

                int numberOfBins = 100;
                double mean, standardDeviation;
                double[] histogramXValue;
                int[] histogramYValue;
                Histogram(triggerIntervals, numberOfBins, out mean, out standardDeviation, out histogramXValue, out histogramYValue);

                DisplayOutput(numberOfBins, histogramXValue, histogramYValue, mean, standardDeviation);
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

        void DisplayOutput(int numberOfBins, double[] histogramXValue, int[] histogramYValue, double mean, double standardDeviation)
        {
            histogramDataGridView.ColumnHeadersVisible = true;
            for (int rowIndex = 0; rowIndex < numberOfBins; rowIndex++)
            {
                histogramDataGridView.Rows.Add();
                histogramDataGridView.Rows[rowIndex].Cells[0].Value = (rowIndex + 1).ToString();
                histogramDataGridView.Rows[rowIndex].Cells[1].Value = histogramXValue[rowIndex].ToString("E");
                histogramDataGridView.Rows[rowIndex].Cells[2].Value = histogramYValue[rowIndex].ToString();
            }
            MeanTime = mean.ToString("E");
            StandardDeviation = standardDeviation.ToString("E");
            MeanFrequency = (1 / mean).ToString("E");
        }

        void ChangeControlState(bool isEnabled)
        {
            generalGroupBox.Enabled = isEnabled;
            acquireButton.Enabled = isEnabled;
            triggerGroupBox.Enabled = isEnabled;
            if (!isEnabled)
            {
                histogramDataGridView.Rows.Clear();
                histogramDataGridView.ColumnHeadersVisible = false;
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

        void Histogram(double[] triggerIntervals, int numberOfBins, out double mean, out double standardDeviation, out double[] histogramXValue, out int[] histogramYValue)
        {
            double max = triggerIntervals[1];
            double min = triggerIntervals[1];

            double counterMean = 0.0;
            double counterStandardDeviation = 0.0;

            // We ignore the first timestamp because it is not a valid reference.
            for (int i = 1; i < triggerIntervals.Length; i++)
            {
                max = Math.Max(triggerIntervals[i], max);
                min = Math.Min(triggerIntervals[i], min);
                counterMean += triggerIntervals[i];
            }

            // Calculate the mean -- sum{X[i]}/n
            mean = counterMean / (triggerIntervals.Length - 1);

            // Ignore timestamp 0.
            for (int i = 1; i < triggerIntervals.Length; i++)
            {
                counterStandardDeviation += Math.Pow(triggerIntervals[i] - mean, 2);
            }

            // Calculate the stdev -- sqrt(sum{ (X[i] - mean)^2 }/n )
            standardDeviation = Math.Sqrt(counterStandardDeviation / (triggerIntervals.Length - 1));

            // Get the width of each bin.
            double dx = (max - min) / numberOfBins;

            histogramXValue = new double[numberOfBins];
            histogramYValue = new int[numberOfBins];

            // Add the counts to each bin according to the timestamps.
            int y;
            for (int i = 1; i < NumberOfRecords; i++)
            {
                y = (int)((triggerIntervals[i] - min) / dx);
                if (y == numberOfBins)
                    y--;
                histogramYValue[y]++;
            }

            // Form a corresponding x axis.
            for (int i = 0; i < numberOfBins; i++)
            {
                histogramXValue[i] = i * dx + min;
            }
        }

        void ShowError(Exception ex)
        {
            MessageBox.Show(ex.Message, ex.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}