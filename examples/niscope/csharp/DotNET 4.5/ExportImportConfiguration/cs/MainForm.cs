//==================================================================================================
//
// Title:
//      NI-Scope Export/Import Configuration File
//
// Description:
//      This example demonstrates how to use the Export and Import Attribute Configuration File APIs.
//      Clicking Perform auto-setup will initialize a session; perform auto-setup; and acquire data once.
//
//      Clicking Export will prompt for a file to save the attribute configuration into; initialize a
//      session; configure those attributes to the session; and export those to file.
//
//      Clicking Import will prompt for a file to load the attribute configuration from; initialize a
//      session; and import the attribute configuration into the session.
//      
//      Clicking Acquire will initialize a session; configure that session with the attribute values
//      displayed on the UI; and acquire data once.
//
//==================================================================================================

using System;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIScope;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;
using System.IO;

namespace NationalInstruments.Examples.ExportImportConfiguration
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            LoadScopeDeviceNames();
            ConfigureVerticalCouplingComboBox();
            ConfigureInputImpedanceComboBox();
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

        double Range
        {
            get
            {
                return Convert.ToDouble(this.verticalRangeToolbox.Text);
            }
            set
            {
                this.verticalRangeToolbox.Text = value.ToString();
            }
        }

        double Offset
        {
            get
            {
                return Convert.ToDouble(this.verticalOffsetToolbox.Text);
            }
            set
            {
                this.verticalOffsetToolbox.Text = value.ToString();
            }
        }

        ScopeVerticalCoupling Coupling
        {
            get
            {
                return (ScopeVerticalCoupling)this.verticalCouplingComboBox.SelectedItem;
            }
            set
            {
                this.verticalCouplingComboBox.Text = value.ToString();
            }
        }

        double InputImpedance
        {
            get
            {
                return Convert.ToDouble(this.inputImpedanceComboBox.SelectedItem);
            }
            set
            {
                this.inputImpedanceComboBox.Text = value.ToString();
            }
        }

        double MinSampleRate
        {
            get
            {
                return Convert.ToDouble(this.minSampleRateTextBox.Text);
            }
            set
            {
                this.minSampleRateTextBox.Text = value.ToString();
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

        void acquireButton_Click(object sender, EventArgs e)
        {
            ConfigureAndAcquireData();
        }

        void ConfigureAndAcquireData()
        {
            ChangeControlState(false);

            try
            {
                 using (NIScope scopeSession = new NIScope(ResourceName, false, false))
                 {
                     scopeSession.DriverOperation.Warning += new EventHandler<ScopeWarningEventArgs>(DriverOperation_Warning);
                     Configure(scopeSession);
                     StartAcquisition(scopeSession);
                 }
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }

            ChangeControlState(true);
        }

        void Export_Click(object sender, EventArgs e)
        {
            ChangeControlState(false);

            OpenFileDialog fileDialog = GetConfigurationFileDialog();
            fileDialog.Title = "Select filename to export configuration to...";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                ExportConfiguration(fileDialog.FileName);
            }

            ChangeControlState(true);
        }

        void Import_Click(object sender, EventArgs e)
        {
            ChangeControlState(false);

            OpenFileDialog fileDialog = GetConfigurationFileDialog();
            fileDialog.Title = "Select filename to import configuration from...";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                ImportConfiguration(fileDialog.FileName);
            }

            ChangeControlState(true);
        }

        void ExportConfiguration(string filePath)
        {
            try
            {
                using (NIScope scopeSession = new NIScope(ResourceName, false, false))
                {
                    scopeSession.DriverOperation.Warning += new EventHandler<ScopeWarningEventArgs>(DriverOperation_Warning);

                    // Configure the session
                    Configure(scopeSession);

                    // Export the configuration to a file
                    scopeSession.Utility.ExportAttributeConfigurationFile(filePath);
                }
            }
            // Handle driver-specific exceptions before the general exception handler that follows.
            // An example of a driver-specific exception is Ivi.Driver.IviCDriverException.
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        void ImportConfiguration(string filePath)
        {
            try
            {
                using (NIScope scopeSession = new NIScope(ResourceName, false, false))
                {
                    scopeSession.DriverOperation.Warning += new EventHandler<ScopeWarningEventArgs>(DriverOperation_Warning);

                    // Import the configuration from a file
                    scopeSession.Utility.ImportAttributeConfigurationFile(filePath);

                    // Query the properties and display them on the UI
                    Query(scopeSession);
                }
            }
            // Handle driver-specific exceptions before the general exception handler that follows.
            // An example of a driver-specific exception is Ivi.Driver.IviCDriverException.
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        void AutoSetup_Click(object sender, EventArgs e)
        {
            ChangeControlState(false);

            try
            {
                using (NIScope scopeSession = new NIScope(ResourceName, false, false))
                {
                    scopeSession.DriverOperation.Warning += new EventHandler<ScopeWarningEventArgs>(DriverOperation_Warning);

                    // Perform Auto-setup
                    scopeSession.Measurement.AutoSetup();

                    // Query the properties and display them on the UI
                    Query(scopeSession);

                    // Acquire data
                    StartAcquisition(scopeSession);
                }
            }
            // Handle driver-specific exceptions before the general exception handler that follows.
            // An example of a driver-specific exception is Ivi.Driver.IviCDriverException.
            catch (Exception ex)
            {
                ShowError(ex);
            }

            ChangeControlState(true);
        }

        void DriverOperation_Warning(object sender, ScopeWarningEventArgs e)
        {
            MessageBox.Show(e.Text, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        void StartAcquisition(NIScope scopeSession)
        {
            PrecisionTimeSpan timeout = new PrecisionTimeSpan(5.0);
            AnalogWaveformCollection<double> waveforms = null;

            long recordLength = scopeSession.Acquisition.RecordLength;
            double sampleRate = scopeSession.Acquisition.SampleRate;

            waveforms = scopeSession.Channels[ChannelName].Measurement.Read(timeout, recordLength, waveforms);

            DisplayResults(recordLength, sampleRate);
            PlotWaveforms(sampledDataGridView, waveforms);
        }

        void Configure(NIScope scopeSession)
        {
            scopeSession.Channels[ChannelName].Range = Range;
            scopeSession.Channels[ChannelName].Coupling = Coupling;
            scopeSession.Channels[ChannelName].Offset = Offset;
            scopeSession.Channels[ChannelName].InputImpedance = InputImpedance;
            scopeSession.Acquisition.SampleRateMin = MinSampleRate;
        }

        void Query(NIScope scopeSession)
        {
            Range = scopeSession.Channels[ChannelName].Range;
            Coupling = scopeSession.Channels[ChannelName].Coupling;
            Offset = scopeSession.Channels[ChannelName].Offset;
            InputImpedance = scopeSession.Channels[ChannelName].InputImpedance;
            MinSampleRate = scopeSession.Acquisition.SampleRateMin;
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
            verticalCouplingComboBox.Enabled = isEnabled;
            verticalRangeToolbox.Enabled = isEnabled;
            inputImpedanceComboBox.Enabled = isEnabled;
            verticalOffsetToolbox.Enabled = isEnabled;
            minSampleRateTextBox.Enabled = isEnabled;
            this.Refresh();
        }

        OpenFileDialog GetConfigurationFileDialog()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            var userDir = Environment.GetEnvironmentVariable("userprofile");

            fileDialog.InitialDirectory = Path.Combine(userDir, "Desktop");
            fileDialog.Filter = "NI-Scope configuration files (*.niscopeconfig)|*.niscopeconfig|All files (*.*)|*.*";
            fileDialog.FilterIndex = 1;
            fileDialog.RestoreDirectory = true;
            fileDialog.CheckFileExists = false;

            return fileDialog;
        }

        static void ShowError(Exception ex)
        {
            MessageBox.Show(ex.Message, ex.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}