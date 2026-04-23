//==================================================================================================
//
// Title:
//      Digital Filtering

// Description:
//      The NI-SCOPE driver includes many measurements that may be performed on the 
//      acquired waveforms from your digitizer.  This example illustrates using digital 
//      filters to eliminate noise in specific frequency ranges of your signal.
//
//==================================================================================================

using System;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIScope;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.DigitalFiltering
{
    public partial class MainForm : Form
    {
        NIScope scopeSession;
        bool stop;

        public MainForm()
        {
            InitializeComponent();
            ConfigureFftFunctionComboBox();
            ConfigureFilterTypeComboBox();
            ConfigureFirWindowComboBox();
            ConfigureFilterComboBox();
            LoadScopeDeviceNames();
            ChangeControlState(true);
        }

        #region Mainform initial configuration
        void ConfigureFftFunctionComboBox()
        {
            fftFunctionComboBox.Items.Add(ScopeArrayMeasurementType.FftAmplitudeSpectrumVoltsRms);
            fftFunctionComboBox.Items.Add(ScopeArrayMeasurementType.FftAmplitudeSpectrumDB);
            fftFunctionComboBox.SelectedIndex = 1;
        }

        void ConfigureFilterTypeComboBox()
        {
            foreach (ScopeMeasurementFilterType enumValue in Enum.GetValues(typeof(ScopeMeasurementFilterType)))
            {
                filterTypeComboBox.Items.Add(enumValue);
            }
            filterTypeComboBox.SelectedIndex = 0;
        }

        void ConfigureFirWindowComboBox()
        {
            firWindowComboBox.Items.Add(ScopeMeasurementFirFilterWindow.None);
            firWindowComboBox.Items.Add(ScopeMeasurementFirFilterWindow.HanningWindow);
            firWindowComboBox.Items.Add(ScopeMeasurementFirFilterWindow.FlatTopWindow);
            firWindowComboBox.Items.Add(ScopeMeasurementFirFilterWindow.HammingWindow);
            firWindowComboBox.Items.Add(ScopeMeasurementFirFilterWindow.TriangleWindow);
            firWindowComboBox.Items.Add(ScopeMeasurementFirFilterWindow.BlackmanWindow);
            firWindowComboBox.SelectedIndex = 1;
        }

        void ConfigureFilterComboBox()
        {
            filterComboBox.Items.Add(ScopeArrayMeasurementType.NoMeasurement);
            filterComboBox.Items.Add(ScopeArrayMeasurementType.WindowedFirFilter);
            filterComboBox.Items.Add(ScopeArrayMeasurementType.BesselFilter);
            filterComboBox.Items.Add(ScopeArrayMeasurementType.ButterworthFilter);
            filterComboBox.Items.Add(ScopeArrayMeasurementType.ChebyshevFilter);
            filterComboBox.SelectedIndex = 4;
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

        ScopeArrayMeasurementType Filter
        {
            get
            {
                return (ScopeArrayMeasurementType)this.filterComboBox.SelectedItem;
            }
        }

        ScopeArrayMeasurementType FftFunction
        {
            get
            {
                return (ScopeArrayMeasurementType)this.fftFunctionComboBox.SelectedItem;
            }
        }

        ScopeMeasurementFilterType FilterType
        {
            get
            {
                return (ScopeMeasurementFilterType)this.filterTypeComboBox.SelectedItem;
            }
        }

        double CutoffFrequency
        {
            get
            {
                return decimal.ToDouble(this.lowOrHighPassCutoffFrequencyNumeric.Value);
            }
        }

        double CenterFrequency
        {
            get
            {
                return decimal.ToDouble(this.bandpassOrStopCenterFrequencyNumeric.Value);
            }
        }

        double BandWidth
        {
            get
            {
                return decimal.ToDouble(this.bandpassOrBandstopWidthNumeric.Value);
            }
        }

        int FirTaps
        {
            get
            {
                return decimal.ToInt32(this.firTapsNumeric.Value);
            }
        }

        ScopeMeasurementFirFilterWindow FirWindow
        {
            get
            {
                return (ScopeMeasurementFirFilterWindow)this.firWindowComboBox.SelectedItem;
            }
        }

        int IirOrder
        {
            get
            {
                return decimal.ToInt32(this.iirOrderNumeric.Value);
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

            AnalogWaveformCollection<double> filteredWaveforms = null;
            AnalogWaveformCollection<double> spectrumWaveforms = null;
            try
            {
                InitializeSession();
                while (!stop)
                {
                    scopeSession.Channels[ChannelName].Enabled = true;

                    // Configure the vertical parameters such as input range and coupling.
                    scopeSession.Channels[ChannelName].Range = 2;
                    scopeSession.Channels[ChannelName].Coupling = ScopeVerticalCoupling.DC;

                    // Configure the horizontal parameters such as sampling rate and number of samples to acquire.
                    scopeSession.Timing.ConfigureTiming(SampleRateMin, RecordLengthMin, 50, 1, true);

                    // Configure all the attributes associated with the digital filter.
                    ScopeChannelMeasurementFilter measurementFilter = scopeSession.Channels[ChannelName].Measurement.Filter;
                    measurementFilter.CutoffFrequency = CutoffFrequency;
                    measurementFilter.CenterFrequency = CenterFrequency;
                    measurementFilter.Taps = FirTaps;
                    measurementFilter.FirFilterWindow = FirWindow;
                    measurementFilter.Order = IirOrder;
                    measurementFilter.TransientPercent = 20;
                    measurementFilter.Ripple = 0.5;
                    measurementFilter.Type = FilterType;
                    measurementFilter.Width = BandWidth;

                    // Add the user requested filter process. The measurement library will apply this process step 
                    // to the acquired data before calculating any of the array measurements.
                    scopeSession.Channels[ChannelName].Measurement.AddWaveformProcessing(Filter);

                    // Initiate a new acquisition.
                    scopeSession.Measurement.Initiate();

                    // Show the filtered data.
                    filteredWaveforms = scopeSession.Channels[ChannelName].Measurement.FetchArrayMeasurement(new PrecisionTimeSpan(), ScopeArrayMeasurementType.NoMeasurement, filteredWaveforms);

                    // Apply an FFT to the filtered data and display the results.
                    spectrumWaveforms = scopeSession.Channels[ChannelName].Measurement.FetchArrayMeasurement(new PrecisionTimeSpan(), FftFunction, spectrumWaveforms);

                    // Clear the filter process.
                    scopeSession.Channels[ChannelName].Measurement.ClearWaveformProcessing();

                    PlotWaveforms(filteredWaveformDataGridView, filteredWaveforms);
                    PlotWaveforms(spectrumDataGridView, spectrumWaveforms);
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
            filteredWaveformDataGridView.Columns.Clear();
            spectrumDataGridView.Columns.Clear();
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
            timingParametersGroupBox.Enabled = isEnabled;
            functionsGroupBox.Enabled = isEnabled;
            filterParametersGroupBox.Enabled = isEnabled;
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