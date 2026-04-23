//==================================================================================================
//
// Title:
//      Getting Started
//
// Description:
//      This example opens a session to the NI-SCOPE driver by passing in the device's 
//      resource name to the Init function.  The resource name is a unique identifier 
//      for your National Instruments hardware product.  It can be found (and changed) 
//      by running Measurement & Automation Explorer (MAX). 
//
//==================================================================================================

using System;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIScope;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.GettingStarted
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

        #region Mainform Configuration values
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

        string RecordLength
        {
            set
            {
                this.actualRecordLengthTextBox.Text = value;
            }
        }

        string SampleRate
        {
            set
            {
                this.actualSampleRateTextBox.Text = value;
            }
        }
        #endregion

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
            MessageBox.Show(e.Text, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        void StartAcquisition()
        {
            ChangeControlState(false);

            PrecisionTimeSpan timeout = new PrecisionTimeSpan(5.0);
            AnalogWaveformCollection<double> waveforms = null;
            try
            {
                InitializeSession();
                scopeSession.Measurement.AutoSetup();

                long recordLength = scopeSession.Acquisition.RecordLength;
                double sampleRate = scopeSession.Acquisition.SampleRate;

                waveforms = scopeSession.Channels[ChannelName].Measurement.Read(timeout, recordLength, waveforms);

                DisplayResults(recordLength, sampleRate);
                PlotWaveforms(sampledDataGridView, waveforms);
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

            SetupDataGridView(dgv, waveforms.Count);
            for (rowIndex = 0; rowIndex < waveforms[0].SampleCount; rowIndex++)
            {
                columnIndex = 0;
                dgv.Rows.Add();
                dgv.Rows[rowIndex].Cells[columnIndex++].Value = (rowIndex + 1).ToString();
                foreach (AnalogWaveform<double> waveform in waveforms)
                {
                    dgv.Rows[rowIndex].Cells[columnIndex++].Value = waveform.Samples[rowIndex].Value.ToString("E");
                }
            }
        }

        void DisplayResults(long recordLength, double sampleRate)
        {
            RecordLength = recordLength.ToString();
            SampleRate = sampleRate.ToString("E");
        }

        static void SetupDataGridView(DataGridView dgv, int numberOfWaveforms)
        {
            if (dgv.ColumnCount > 0)
                return;

            DataGridViewTextBoxColumn indexColumn = new DataGridViewTextBoxColumn();
            indexColumn.Width = 60;
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
            acquireButton.Enabled = isEnabled;
            resourceNameComboBox.Enabled = isEnabled;
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

        static void ShowError(Exception ex)
        {
            MessageBox.Show(ex.Message, ex.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}