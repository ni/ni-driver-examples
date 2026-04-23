//==================================================================================================
//
// Title:
//      Stream To Disk
//
// Description:
//      This application demonstrates the programmatic usage of .NET API for  NI-SCOPE. 
//      This application acquires data from a channel and saves it to a file on the local 
//      disk. The waveform stored in the file can be loaded onto a datagrid.
//
//==================================================================================================

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIScope;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.StreamToDisk
{
    public partial class MainForm : Form
    {
        NIScope scopeSession;

        public MainForm()
        {
            InitializeComponent();
            LoadScopeDeviceNames();
            ChangeControlState(true);
        }

        #region Mainform Initial Configuration
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
                return decimal.ToDouble(this.minSampleRateNumeric.Value);
            }
        }

        int RecordLengthMin
        {
            get
            {
                return decimal.ToInt32(this.recordLengthMinNumeric.Value);
            }
        }

        bool Acquire
        {
            get
            {
                return this.acquireRadioButton.Checked;
            }
        }

        string FilePath
        {
            get
            {
                return this.filePathTextBox.Text;
            }
        }
        #endregion

        void startButton_Click(object sender, System.EventArgs e)
        {
            BinaryAcquisition();
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

        void BinaryAcquisition()
        {
            ChangeControlState(false);

            AnalogWaveformCollection<double> sampledwaveforms = null;
            try
            {
                if (Acquire)
                {
                    InitializeSession();

                    // Configure the vertical parameters.
                    double verticalOffset = 0.0;
                    ScopeVerticalCoupling verticalCoupling = ScopeVerticalCoupling.DC;
                    double probeAttenuation = 1.0;
                    scopeSession.Channels[ChannelName].Configure(VerticalRange, verticalOffset, verticalCoupling, probeAttenuation, true);

                    // Configure the horizontal parameters.
                    double referencePosition = 50.0;
                    int numberOfRecords = 1;
                    bool enforceRealTime = true;
                    scopeSession.Timing.ConfigureTiming(SampleRateMin, RecordLengthMin, referencePosition, numberOfRecords, enforceRealTime);

                    scopeSession.Measurement.Initiate();
                    long recordLength = scopeSession.Acquisition.RecordLength;
                    PrecisionTimeSpan timeout = new PrecisionTimeSpan(5.0);
                    sampledwaveforms = scopeSession.Channels[ChannelName].Measurement.FetchDouble(timeout, recordLength, sampledwaveforms);
                    PlotWaveforms<double>(acquiredDataGridView, sampledwaveforms);

                    // Configure the folder location.
                    string directoryPath = @"C:\waveform";
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    FileInfo filePath = new FileInfo(Path.Combine(directoryPath, @"waveform.txt"));
                    if (!filePath.Exists)
                    {
                        filePath.Create().Close();
                    }
                    else
                    {
                        File.WriteAllText(filePath.ToString(), String.Empty);
                    }

                    using (FileStream fileStream = new FileStream(filePath.ToString(), FileMode.Create, FileAccess.ReadWrite))
                    {
                        BinaryFormatter binaryFormatter = new BinaryFormatter();
                        binaryFormatter.Serialize(fileStream, sampledwaveforms);
                    }
                }
                else
                {
                    using (FileStream fileStream = new FileStream(FilePath, FileMode.Open))
                    {
                        BinaryFormatter binaryFormatter = new BinaryFormatter();
                        AnalogWaveformCollection<double> records = binaryFormatter.Deserialize(fileStream) as AnalogWaveformCollection<double>;
                        PlotWaveforms<double>(recordDataGridView, records);
                    }
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

        void ClearWaveforms()
        {
            acquiredDataGridView.Columns.Clear();
            recordDataGridView.Columns.Clear();
        }

        static void PlotWaveforms<T>(DataGridView dgv, AnalogWaveformCollection<T> waveforms)
        {
            int rowindex, columnIndex;
            int lastCount = dgv.RowCount;

            SetupDataGridView(dgv, waveforms.Count);
            for (rowindex = lastCount; rowindex < lastCount + waveforms[0].SampleCount; rowindex++)
            {
                columnIndex = 0;
                dgv.Rows.Add();
                dgv.Rows[rowindex].Cells[columnIndex++].Value = (rowindex + 1).ToString();
                foreach (AnalogWaveform<T> waveform in waveforms)
                {
                    dgv.Rows[rowindex].Cells[columnIndex++].Value = waveform.Samples[rowindex - lastCount].Value.ToString();
                }
            }
        }

        static void SetupDataGridView(DataGridView dgv, int numberOfWaveforms)
        {
            if (dgv.Columns.Count > 0)
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

        void browseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            if (op.ShowDialog() == DialogResult.OK)
            {
                filePathTextBox.Text = op.FileName;
            }
        }

        void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            ChangeControlState(true);
        }

        void ChangeControlState(bool isEnabled)
        {
            if (isEnabled)
            {
                configurationGroupBox.Enabled = Acquire;
                filePathGroupBox.Enabled = !Acquire;
            }
            else
            {
                ClearWaveforms();
                configurationGroupBox.Enabled = false;
                filePathGroupBox.Enabled = false;
            }
            startButton.Enabled = isEnabled;
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

        static void ShowError(Exception ex)
        {
            MessageBox.Show(ex.Message, ex.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}