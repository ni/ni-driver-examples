//=============================================================================================================
//
// Title:
//      NI-DCPower Hardware-Timed Single Point
//
// Description:
//      This example demonstrates how to set up a hardware-timed Single Point operation.
//      The hardware is configured to source a voltage, wait for a specified delay, and
//      then take a measurement.  This example uses Single Point source mode.
//
//      Note: In this example the Output Function is set to DC Voltage. If you change the
//      Output Function to DC Current, you must use Current Level and Voltage Limit instead
//      of Voltage Level and Current Limit.
//
//  Suggested Devices:
//      PXI-4132
//      PXIe-4112, PXIe-4113, PXIe-4135, PXIe-4136, PXIe-4137, PXIe-4138, PXIe-4139, PXIe-4140, PXIe-4141, PXIe-4142, PXIe-4143, PXIe-4144, PXIe-4145, PXIe-4147, PXIe-4154, PXIe-4162, PXIe-4163
//
//=============================================================================================================

using System;
using System.Drawing;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIDCPower;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.HardwareTimedSinglePoint
{
    public partial class MainForm : Form
    {
        NIDCPower dcPowerSession;

        public MainForm()
        {
            InitializeComponent();
            LoadNIDCPowerDeviceNames();
        }

        #region MainForm initial configuration

        void LoadNIDCPowerDeviceNames()
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

        double VoltageLevelRange
        {
            get
            {
                return decimal.ToDouble(this.voltageLevelRangeNumeric.Value);
            }
        }

        double VoltageLevel1
        {
            get
            {
                return decimal.ToDouble(this.voltageLevel1Numeric.Value);
            }
        }

        double VoltageLevel2
        {
            get
            {
                return decimal.ToDouble(this.voltageLevel2Numeric.Value);
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

        PrecisionTimeSpan FetchTimeout
        {
            get
            {
                return new PrecisionTimeSpan(decimal.ToDouble(this.fetchTimeoutNumeric.Value));
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
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Voltage.VoltageLevel = VoltageLevel1;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Voltage.CurrentLimit = CurrentLimit;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Voltage.VoltageLevelRange = VoltageLevelRange;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Voltage.CurrentLimitRange = CurrentLimitRange;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.SourceDelay = SourceDelay;
                dcPowerSession.Measurement.Configuration.MeasureWhen = DCPowerMeasurementWhen.AutomaticallyAfterSourceComplete;

                dcPowerSession.Control.Initiate();

                // Fetch the first set of measurements.
                DCPowerFetchResult result = dcPowerSession.Measurement.Fetch(FullyQualifiedChannelName, FetchTimeout, 1);
                UpdateMeasurement(voltage1MeasurementsTextBox, current1MeasurementsTextBox, inCompliance1ButtonLed, result);

                // Dynamically reconfigure the voltage level. This is another source operation.
                // The device waits for the source delay after reprogramming the output and then
                // takes a measurement.
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Voltage.VoltageLevel = VoltageLevel2;

                // Fetch the new set of measurements.
                result = dcPowerSession.Measurement.Fetch(FullyQualifiedChannelName, FetchTimeout, 1);
                UpdateMeasurement(voltage2MeasurementsTextBox, current2MeasurementsTextBox, inCompliance2ButtonLed, result);

                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Output.Enabled = false;
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

        private static void UpdateMeasurement(TextBox voltageTextBox, TextBox currentTextBox, Button inComplianceButtonLed, DCPowerFetchResult newResult)
        {
            voltageTextBox.Text = newResult.VoltageMeasurements[0].ToString("E");
            currentTextBox.Text = newResult.CurrentMeasurements[0].ToString("E");

            inComplianceButtonLed.BackColor = newResult.InCompliance[0] ? Color.Red : SystemColors.Control;
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

        void InitializeDCPowerSession()
        {
            dcPowerSession = new NIDCPower(FullyQualifiedChannelName, false, String.Empty);
            dcPowerSession.DriverOperation.Warning += new EventHandler<DCPowerWarningEventArgs>(DCPowerDriverOperationWarning);
        }

        void DCPowerDriverOperationWarning(object sender, DCPowerWarningEventArgs e)
        {
            MessageBox.Show(e.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        static void ShowError(Exception ex)
        {
            MessageBox.Show(ex.Message, ex.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        void ChangeControlState(bool flag)
        {
            resourceNameAndChannelNameGroupBox.Enabled = flag;
            configurationGroupBox.Enabled = flag;
            voltageLevel1GroupBox.Enabled = flag;
            voltageLevel2GroupBox.Enabled = flag;
            startButton.Enabled = flag;
            resourceNameComboBox.Select();
            this.Refresh();
        }
    }
}