//==================================================================================================
//
// Title:
//      Windowing
//
// Description:
//      This example illustrates using some of the advanced measurement library functions 
//      do a windowed, FFT measurement. Windowing is a technique to reduce the 
//      spectral leakage in FFT measurements.
//
//==================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIScope;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.Windowing
{
    public partial class MainForm : Form
    {
        NIScope scopeSession;
        bool stop;

        public MainForm()
        {
            InitializeComponent();
            ConfigureWindowComboBox();
            ConfigureFftFunctionComboBox();
            LoadScopeDeviceNames();
            ChangeControlState(true);
        }

        #region MainForm initial configuration
        void ConfigureWindowComboBox()
        {
            windowComboBox.Items.Add(ScopeArrayMeasurementType.NoMeasurement);
            windowComboBox.Items.Add(ScopeArrayMeasurementType.HanningWindow);
            windowComboBox.Items.Add(ScopeArrayMeasurementType.FlatTopWindow);
            windowComboBox.Items.Add(ScopeArrayMeasurementType.HammingWindow);
            windowComboBox.Items.Add(ScopeArrayMeasurementType.TriangleWindow);
            windowComboBox.Items.Add(ScopeArrayMeasurementType.BlackmanWindow);
            windowComboBox.SelectedIndex = 1;
        }

        void ConfigureFftFunctionComboBox()
        {
            fftFunctionComboBox.Items.Add(ScopeArrayMeasurementType.FftAmplitudeSpectrumVoltsRms);
            fftFunctionComboBox.Items.Add(ScopeArrayMeasurementType.FftAmplitudeSpectrumDB);
            fftFunctionComboBox.SelectedIndex = 1;
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

        ScopeArrayMeasurementType FftFunction
        {
            get
            {
                return (ScopeArrayMeasurementType)this.fftFunctionComboBox.SelectedItem;
            }
        }

        ScopeArrayMeasurementType Window
        {
            get
            {
                return (ScopeArrayMeasurementType)this.windowComboBox.SelectedItem;
            }
        }

        bool AverageSpectrum
        {
            get
            {
                return this.averageSpectrumCheckBox.Checked;
            }
        }

        bool ClearAveraging
        {
            get
            {
                return this.clearAveragingCheckBox.Checked;
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
            // Open a session to the digitizer.
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
            AnalogWaveformCollection<double> spectrumWaveforms = null;
            try
            {
                InitializeSession();
                while (!stop)
                {
                    // Configure the vertical parameters such as input range, offset, and coupling.
                    scopeSession.Channels[ChannelName].Range = 5;

                    // Configure the horizontal parameters such as sampling rate and number of samples to acquire.
                    double referencePosition = 50.0;
                    int numberOfRecords = 1;
                    bool enforceRealtime = true;
                    scopeSession.Timing.ConfigureTiming(SampleRateMin, RecordLengthMin, referencePosition, numberOfRecords, enforceRealtime);

                    // Initiate a new acquisition with the configured settings, wait for 
                    // this acquisition to complete, and fetch the data. Plot the data.
                    waveformFromScope = scopeSession.Channels[ChannelName].Measurement.Read(PrecisionTimeSpan.MaxValue, -1, waveformFromScope);

                    // Add the user-requested Window processing step.
                    scopeSession.Channels[ChannelName].Measurement.AddWaveformProcessing(Window);

                    // Add the user-requested FFT function processing step. This processing step will be applied
                    // after the Window processing step has been applied to the acquired data.
                    scopeSession.Channels[ChannelName].Measurement.AddWaveformProcessing(FftFunction);

                    // Calculate and display the running average of the waveform. The driver keeps a cached running
                    // average of the waveform. Every time this function is called the driver adds the current 
                    // processed data to the running average. Clear stats clears the cached data.
                    ScopeArrayMeasurementType arrayMeasurementType = AverageSpectrum ? ScopeArrayMeasurementType.MultipleAcquisitionAverage : ScopeArrayMeasurementType.NoMeasurement;
                    spectrumWaveforms = scopeSession.Channels[ChannelName].Measurement.FetchArrayMeasurement(new PrecisionTimeSpan(), arrayMeasurementType, spectrumWaveforms);

                    // If the user requests, clear any cached statistics.
                    if (ClearAveraging || !AverageSpectrum)
                    {
                        scopeSession.Channels[ChannelName].Measurement.ClearWaveformMeasurements();
                    }

                    // Clear all processes.
                    scopeSession.Channels[ChannelName].Measurement.ClearWaveformProcessing();

                    PlotWaveforms(waveformFromScopeDataGridView, waveformFromScope);
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
            waveformFromScopeDataGridView.Columns.Clear();
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
            channelTextBox.Enabled = isEnabled;
            scalarMeasurementsGroupBox.Enabled = isEnabled;
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