//=============================================================================================================
//
// Title:
//       NI-DCPower Source DC Voltage
//
// Description:
//      This example demonstrates how to use the DC Voltage Output Function to force an output
//      voltage. This example initializes a session; configures the Output Function,
//      Autorange, Voltage Level and Current Limit; initiates generation; waits for a specified
//      delay; and measures the voltage and current output. This example uses Single Point
//      source mode.
//
//      Note: In this example the Output Function is set to  DC Voltage. If you change the
//      Output Function to DC Current, you must use Current Level and Voltage Limit instead
//      of Voltage Level and Current Limit.
//
//  Suggested Devices:
//      PXI-4110, PXI-4130, PXI-4132
//      PXIe-4112, PXIe-4113, PXIe-4135, PXIe-4136, PXIe-4137, PXIe-4138, PXIe-4139, PXIe-4140, PXIe-4141, PXIe-4142, PXIe-4143, PXIe-4144, PXIe-4145, PXIe-4147, PXIe-4154, PXIe-4162, PXIe-4163
//
//=============================================================================================================

using System;
using System.Drawing;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIDCPower;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.SourceDCVoltage
{
    public partial class MainForm : Form
    {
        NIDCPower dcPowerSession;

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
                return $"{ResourceName}/{ChannelName}";
            }
        }

        double VoltageLevel
        {
            get
            {
                return decimal.ToDouble(this.voltageLevelNumeric.Value);
            }
        }

        double VoltageLevelRange
        {
            get
            {
                return decimal.ToDouble(this.voltageLevelRangeNumeric.Value);
            }
        }

        double CurrentLimit
        {
            get
            {
                return decimal.ToDouble(this.currentLimitNumeric.Value);
            }
        }

        double CurrentLimitRange
        {
            get
            {
                return decimal.ToDouble(this.currentLimitRangeNumeric.Value);
            }
        }

        PrecisionTimeSpan SourceDelay
        {
            get
            {
                return new PrecisionTimeSpan(decimal.ToDouble(this.sourceDelayNumeric.Value));
            }
        }
        #endregion

        void startButton_Click(object sender, EventArgs e)
        {
            ChangeControlState(false);
            Start();
            ChangeControlState(true);
        }

        void mainForm_Closing(object sender, FormClosingEventArgs e)
        {
            CloseSession();
        }

        void Start()
        {
            try
            {
                InitializeDCPowerSession();

                dcPowerSession.Source.Mode = DCPowerSourceMode.SinglePoint;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Output.Function = DCPowerSourceOutputFunction.DCVoltage;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Voltage.VoltageLevel = VoltageLevel;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Voltage.CurrentLimit = CurrentLimit;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Voltage.VoltageLevelRange = VoltageLevelRange;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Voltage.CurrentLimitRange = CurrentLimitRange;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.SourceDelay = SourceDelay;

                dcPowerSession.Control.Initiate();
                dcPowerSession.Events.SourceCompleteEvent.WaitForEvent(new PrecisionTimeSpan(5.0));

                DCPowerMeasureResult result = dcPowerSession.Measurement.Measure(FullyQualifiedChannelName);
                bool inCompliance = dcPowerSession.Measurement.QueryInCompliance(FullyQualifiedChannelName);
                DisplayMeasurements(result.VoltageMeasurements[0], result.CurrentMeasurements[0], inCompliance);

                dcPowerSession.Utility.Reset();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
            finally
            {
                CloseSession();
            }
        }

        void InitializeDCPowerSession()
        {
            dcPowerSession = new NIDCPower(FullyQualifiedChannelName, false, String.Empty);
            dcPowerSession.DriverOperation.Warning += new EventHandler<DCPowerWarningEventArgs>(DCPowerDriverOperationWarning);
        }

        void DCPowerDriverOperationWarning(object sender, DCPowerWarningEventArgs e)
        {
            MessageBox.Show(e.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        void CloseSession()
        {
            if (dcPowerSession != null)
            {
                try
                {
                    dcPowerSession.Close();
                    dcPowerSession = null;
                }
                catch (Exception ex)
                {
                    ShowError(ex);
                    Application.Exit();
                }
            }
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
            this.resourceNameComboBox.Select();
            this.Refresh();
        }

    }
}