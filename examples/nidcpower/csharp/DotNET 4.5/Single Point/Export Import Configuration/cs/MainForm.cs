//=============================================================================================================
//
// Title:
//       NI-DCPower Export/Import Configuration File
//
// Description:
//      This example demonstrates how to use the Export and Import Attribute Configuration File APIs.
//      Clicking Export will prompt for a file to save the attribute configuration into; initialize a session;
//      configure the Output Function, Voltage Level, and Current Limit; initiate generation; wait for a
//      specified delay; measure the voltage and current output; and export those attributes to file.
//      Clicking Import will prompt for a file to load the attribute configuration from; initialize a session;
//      import the attribute configuration into the session; initiate generation; wait for a specified delay;
//      measure the voltage and current output.
//
//      This example uses Single Point source mode.
//
//      Note: In this example the Output Function is set to  DC Voltage. If you change the
//      Output Function to DC Current, you must use Current Level and Voltage Limit instead
//      of Voltage Level and Current Limit.
//
//  Suggested Devices:
//      PXI-4132
//      PXIe-4112, PXIe-4113, PXIe-4135, PXIe-4136, PXIe-4137, PXIe-4138, PXIe-4139, PXIe-4140, PXIe-4141, PXIe-4142, PXIe-4143, PXIe-4144, PXIe-4145, PXIe-4154
//
//=============================================================================================================

using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIDCPower;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.ExportImportConfiguration
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            LoadDCPowerDeviceNames();
        }

        #region MainForm initial configuration

        void LoadDCPowerDeviceNames()
        {
            using (ModularInstrumentsSystem dcPowerDevices = new ModularInstrumentsSystem("NI-DCPower"))
            {
                foreach (DeviceInfo device in dcPowerDevices.DeviceCollection)
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

        string FullyQualifiedChannelName
        {
            get
            {
                return $"{ ResourceName}/{ChannelName}";
            }
        }

        double VoltageLevel
        {
            get
            {
                return decimal.ToDouble(this.voltageLevelNumeric.Value);
            }
            set
            {
                this.voltageLevelNumeric.Value = (decimal)value;
            }
        }

        double VoltageLevelRange
        {
            get
            {
                return decimal.ToDouble(this.voltageLevelRangeNumeric.Value);
            }
            set
            {
                this.voltageLevelRangeNumeric.Value = (decimal)value;
            }
        }

        double CurrentLimit
        {
            get
            {
                return decimal.ToDouble(this.currentLimitNumeric.Value);
            }
            set
            {
                this.currentLimitNumeric.Value = (decimal)value;
            }
        }

        double CurrentLimitRange
        {
            get
            {
                return decimal.ToDouble(this.currentLimitRangeNumeric.Value);
            }
            set
            {
                this.currentLimitRangeNumeric.Value = (decimal)value;
            }
        }

        PrecisionTimeSpan SourceDelay
        {
            get
            {
                return new PrecisionTimeSpan(decimal.ToDouble(this.sourceDelayNumeric.Value));
            }
            set
            {
                this.sourceDelayNumeric.Value = (decimal)value.FractionalSeconds;
            }
        }
        #endregion

        void startButton_Click(object sender, EventArgs e)
        {
            ChangeControlState(false);
            Start();
            ChangeControlState(true);
        }

        void Export_Click(object sender, EventArgs e)
        {
            ChangeControlState(false);

            var fileDialog = GetConfigurationFileDialog();
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

            var fileDialog = GetConfigurationFileDialog();
            fileDialog.Title = "Select filename to import configuration from...";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                ImportConfiguration(fileDialog.FileName);
            }

            ChangeControlState(true);
        }

        void Start()
        {
            // 1.  Initialize a session to the device.
            // 2.  Configure session based on UI controls.
            // 3.  Initiate sourcing and measure.
            // 4.  Reset to disable the output.
            // 5.  Cleanup of session is handled by "using".
            try
            {
                using (var dcPowerSession = new NIDCPower(FullyQualifiedChannelName, false, String.Empty))
                {
                    dcPowerSession.DriverOperation.Warning += DCPowerDriverOperationWarning;

                    Configure(dcPowerSession);
                    Measure(dcPowerSession);

                    dcPowerSession.Utility.Reset();
                }
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        void ExportConfiguration(string path)
        {
            // 1.  Initialize a session to the device.
            // 2.  Configure session based on UI controls.
            // 3.  Initiate sourcing and measure.
            // 4.  Export configuration to file.
            // 5.  Reset to disable the output.
            // 6.  Cleanup of session is handled by "using".
            try
            {
                using (var dcPowerSession = new NIDCPower(FullyQualifiedChannelName, false, String.Empty))
                {
                    dcPowerSession.DriverOperation.Warning += DCPowerDriverOperationWarning;

                    Configure(dcPowerSession);
                    Measure(dcPowerSession);

                    dcPowerSession.Utility.ExportAttributeConfigurationFile(path);
                    dcPowerSession.Utility.Reset();
                }
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        void ImportConfiguration(string path)
        {
            // 1.  Initialize a session to the device.
            // 2.  Import configuration from file and write session properties to the UI.
            // 3.  Initiate sourcing and measure.
            // 4.  Reset to disable the output.
            // 5.  Cleanup of session is handled by "using".
            try
            {
                using (var dcPowerSession = new NIDCPower(FullyQualifiedChannelName, false, String.Empty))
                {
                    dcPowerSession.DriverOperation.Warning += DCPowerDriverOperationWarning;

                    dcPowerSession.Utility.ImportAttributeConfigurationFile(path);
                    VoltageLevel = dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Voltage.VoltageLevel;
                    CurrentLimit = dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Voltage.CurrentLimit;
                    VoltageLevelRange = dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Voltage.VoltageLevelRange;
                    CurrentLimitRange = dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Voltage.CurrentLimitRange;
                    SourceDelay = dcPowerSession.Outputs[FullyQualifiedChannelName].Source.SourceDelay;

                    Measure(dcPowerSession);

                    dcPowerSession.Utility.Reset();
                }
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        void DCPowerDriverOperationWarning(object sender, DCPowerWarningEventArgs e)
        {
            MessageBox.Show(e.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        void Configure(NIDCPower dcPowerSession)
        {
            // 1.  Configure the Source mode to Single Point.
            // 2.  Set the Output Function to DC Voltage.
            //     If you change the Output Function to DC Current, you must use
            //     Current Level and Voltage Limit instead of Voltage Level and Current Limit.
            // 3.  Configure the Voltage Level.
            //     This property must be used instead of Voltage Limit because the
            //     Output Function is DC Voltage.
            // 4.  Configure the Current Limit.
            //     This property must be used instead of Current Level because
            //     the Output Function is DC Voltage.
            // 5.  Configure the Voltage Level Range.
            // 6.  Configure the Current Limit Range.
            // 7.  Configure the Source Delay.
            dcPowerSession.Source.Mode = DCPowerSourceMode.SinglePoint;
            dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Output.Function = DCPowerSourceOutputFunction.DCVoltage;
            dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Voltage.VoltageLevel = VoltageLevel;
            dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Voltage.CurrentLimit = CurrentLimit;
            dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Voltage.VoltageLevelRange = VoltageLevelRange;
            dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Voltage.CurrentLimitRange = CurrentLimitRange;
            dcPowerSession.Outputs[FullyQualifiedChannelName].Source.SourceDelay = SourceDelay;
        }

        void Measure(NIDCPower dcPowerSession)
        {
            // 1.  Initiate the device to start generation and acquisition.
            // 2.  Wait for output to settle.
            // 3.  Measure the voltage and current.
            // 4.  Determine if the output is in compliance and update the indicator.
            dcPowerSession.Control.Initiate();
            dcPowerSession.Events.SourceCompleteEvent.WaitForEvent(new PrecisionTimeSpan(5.0));

            double voltageMeasurement, currentMeasurement;
            bool inCompliance;

            if (dcPowerSession.Measurement.Configuration.MeasureWhen == DCPowerMeasurementWhen.OnDemand)
            {
                DCPowerMeasureResult result = dcPowerSession.Measurement.Measure(FullyQualifiedChannelName);
                voltageMeasurement = result.VoltageMeasurements[0];
                currentMeasurement = result.CurrentMeasurements[0];
                inCompliance = dcPowerSession.Measurement.QueryInCompliance(FullyQualifiedChannelName);
            }
            else
            {
                if (dcPowerSession.Measurement.Configuration.MeasureWhen == DCPowerMeasurementWhen.OnMeasureTrigger)
                {
                    dcPowerSession.Triggers.MeasureTrigger.SendSoftwareEdgeTrigger();
                }

                DCPowerFetchResult result = dcPowerSession.Measurement.Fetch(FullyQualifiedChannelName, PrecisionTimeSpan.FromSeconds(10), 1);
                voltageMeasurement = result.VoltageMeasurements[0];
                currentMeasurement = result.CurrentMeasurements[0];
                inCompliance = result.InCompliance[0];
            }

            DisplayMeasurements(voltageMeasurement, currentMeasurement, inCompliance);
        }

        void DisplayMeasurements(double voltage, double current, bool inCompliance)
        {
            this.currentMeasurementTextBox.Text = current.ToString("E");
            this.voltageMeasurementTextBox.Text = voltage.ToString("E");
            this.inComplianceButtonLed.BackColor = inCompliance ? Color.Red : SystemColors.Control;
        }

        static void ShowError(Exception ex)
        {
            MessageBox.Show(ex.Message, ex.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        void ChangeControlState(bool isEnabled)
        {
            this.resourceNameAndChannelNameGroupBox.Enabled = isEnabled;
            this.configurationGroupBox.Enabled = isEnabled;
            this.startButton.Enabled = isEnabled;
            this.exportButton.Enabled = isEnabled;
            this.importButton.Enabled = isEnabled;
            this.resourceNameComboBox.Select();
            this.Refresh();
        }

        OpenFileDialog GetConfigurationFileDialog()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            var userDir = Environment.GetEnvironmentVariable("userprofile");

            fileDialog.InitialDirectory = Path.Combine(userDir, "Desktop");
            fileDialog.Filter = "NI-DCPower configuration files (*.nidcpowerconfig)|*.nidcpowerconfig|All files (*.*)|*.*";
            fileDialog.FilterIndex = 1;
            fileDialog.RestoreDirectory = true;
            fileDialog.CheckFileExists = false;

            return fileDialog;
        }
    }
}