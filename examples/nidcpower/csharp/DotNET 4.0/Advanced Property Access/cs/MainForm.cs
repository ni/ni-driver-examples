//=============================================================================================================
//
// Title:
//      NI-DCPower Source DC Current Using Advanced Property Access 
//      
// Description:
//      This example demonstrates how to use the Advanced Property Access Service
//      in the NI-DCPower .NET API. This example configures the Output Function, Sense,
//      Autorange, Current Level, and Voltage Limit using Advanced Property Access Service.
//
//      Note: In this example the Output Function is set to DC Voltage. If you change the 
//      Output Function to DC Current, you must use Current Level and Voltage Limit instead
//      of Voltage Level and Current Limit.
//
//============================================================================================================

using System;
using System.Drawing;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments;
using NationalInstruments.ModularInstruments.NIDCPower;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.AdvancedPropertyAccess
{
    public partial class MainForm : Form
    {
        // Attribute values obtained from the C header files: ivi.h, nidcpower.h
        internal static class CDriverAttributeId
        {
            // NIDCPOWER_ATTR_SOURCE_MODE
            internal const long SourceMode = 1150054;

            // NIDCPOWER_ATTR_OUTPUT_FUNCTION
            internal const long OutputFunction = 1150008;

            // NIDCPOWER_ATTR_SENSE
            internal const long Sense = 1150013;

            // NIDCPOWER_ATTR_VOLTAGE_LEVEL
            internal const long VoltageLevel = 1250001;

            // NIDCPOWER_ATTR_VOLTAGE_LEVEL_RANGE
            internal const long VoltageLevelRange = 1150005;

            // NIDCPOWER_ATTR_CURRENT_LIMIT
            internal const long CurrentLimit = 1250005;

            // NIDCPOWER_ATTR_CURRENT_LIMIT_RANGE
            internal const long CurrentLimitRange = 1150004;
        }

        NIDCPower dcPowerSession;

        public MainForm()
        {
            InitializeComponent();
            ConfigureSenseComboBox();
            LoadDCPowerDeviceNames();
        }

        #region MainForm initial configuration
        void ConfigureSenseComboBox()
        {
            foreach (DCPowerMeasurementSense item in Enum.GetValues(typeof(DCPowerMeasurementSense)))
            {
                senseComboBox.Items.Add(item);
            }
            senseComboBox.SelectedIndex = 0;
        }

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

        DCPowerMeasurementSense Sense
        {
            get
            {
                return (DCPowerMeasurementSense)this.senseComboBox.SelectedItem;
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
                ConfigureSessionUsingAdvancedPropertyAccessService();

                dcPowerSession.Control.Initiate();
                dcPowerSession.Events.SourceCompleteEvent.WaitForEvent(new PrecisionTimeSpan(5.0));

                DCPowerMeasureResult result = dcPowerSession.Measurement.Measure(ChannelName);
                bool inCompliance = dcPowerSession.Measurement.QueryInCompliance(ChannelName);
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
            dcPowerSession = new NIDCPower(ResourceName, ChannelName, false);
            dcPowerSession.DriverOperation.Warning += new EventHandler<DCPowerWarningEventArgs>(DCPowerDriverOperationWarning);
        }

        void DCPowerDriverOperationWarning(object sender, DCPowerWarningEventArgs e)
        {
            MessageBox.Show(e.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        void ConfigureSessionUsingAdvancedPropertyAccessService()
        {
            AdvancedPropertyAccessService dcPowerAdvancedPropertyAccessService =
                (AdvancedPropertyAccessService)(dcPowerSession as IServiceProvider).GetService(typeof(AdvancedPropertyAccessService));

            dcPowerAdvancedPropertyAccessService.SetAttributeInt32(CDriverAttributeId.SourceMode, (int)DCPowerSourceMode.SinglePoint);
            dcPowerAdvancedPropertyAccessService.SetAttributeInt32(CDriverAttributeId.OutputFunction, ChannelName, (int)DCPowerSourceOutputFunction.DCVoltage);
            dcPowerAdvancedPropertyAccessService.SetAttributeInt32(CDriverAttributeId.Sense, ChannelName, (int)Sense);
            dcPowerAdvancedPropertyAccessService.SetAttributeDouble(CDriverAttributeId.VoltageLevel, ChannelName, VoltageLevel);
            dcPowerAdvancedPropertyAccessService.SetAttributeDouble(CDriverAttributeId.CurrentLimit, ChannelName, CurrentLimit);
            dcPowerAdvancedPropertyAccessService.SetAttributeDouble(CDriverAttributeId.VoltageLevelRange, ChannelName, VoltageLevelRange);
            dcPowerAdvancedPropertyAccessService.SetAttributeDouble(CDriverAttributeId.CurrentLimitRange, ChannelName, CurrentLimitRange);
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
            this.currentMeasurementsTextBox.Text = current.ToString("E");
            this.voltageMeasurementsTextBox.Text = voltage.ToString("E");
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