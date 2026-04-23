//=============================================================================================================
//
// Title:
//      NI-DCPower Sink DC Current into Electronic Load
//
// Description:
//      Demonstrates how to use the DC Current Output Function to force a current into the electronic load,
//      and how to configure the electronic load with the Output Shorted, Conduction Voltage and Current Level
//      Slew Rate features.
//
// Suggested Device:
//      PXIe-4051
//
//=============================================================================================================

using System;
using System.Drawing;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIDCPower;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.SinkDCCurrentIntoElectronicLoad
{
    public partial class MainForm : Form
    {
        NIDCPower dcPowerSession;

        public MainForm()
        {
            InitializeComponent();
            ConfigureConductionVoltageModeComboBox();
            LoadDCPowerDeviceNames();
        }

        #region MainForm initial configuration
        void LoadDCPowerDeviceNames()
        {
            using (ModularInstrumentsSystem dcPowerDevices = new ModularInstrumentsSystem("NI-DCPower"))
            {
                foreach (DeviceInfo device in dcPowerDevices.DeviceCollection)
                {
                    resourceNameComboBox.Items.Add(device.Name + "/0");
                }
            }
            if (resourceNameComboBox.Items.Count > 0)
            {
                resourceNameComboBox.SelectedIndex = 0;
            }
        }

        void ConfigureConductionVoltageModeComboBox()
        {
            foreach (DCPowerConductionVoltageMode item in Enum.GetValues(typeof(DCPowerConductionVoltageMode)))
            {
                conductionVoltageModeComboBox.Items.Add(item);
            }
            conductionVoltageModeComboBox.SelectedIndex = 0;
        }
        #endregion

        #region MainForm configuration values

        string FullyQualifiedChannelName
        {
            get
            {
                return this.resourceNameComboBox.Text;
            }
        }

        DCPowerConductionVoltageMode ConductionVoltageMode
        {
            get
            {
                return (DCPowerConductionVoltageMode)this.conductionVoltageModeComboBox.SelectedItem;
            }
        }

        double CurrentLevel
        {
            get
            {
                return decimal.ToDouble(this.currentLevelNumeric.Value);
            }
        }

        double CurrentLevelRange
        {
            get
            {
                return decimal.ToDouble(this.currentLevelRangeNumeric.Value);
            }
        }

        double VoltageLimitRange
        {
            get
            {
                return decimal.ToDouble(this.voltageLimitRangeNumeric.Value);
            }
        }

        double ConductionVoltageOnThreshold
        {
            get
            {
                return decimal.ToDouble(this.conductionVoltageOnThresholdNumeric.Value);
            }
        }

        double ConductionVoltageOffThreshold
        {
            get
            {
                return decimal.ToDouble(this.conductionVoltageOffThresholdNumeric.Value);
            }
        }

        double CurrentLevelRisingSlewRate
        {
            get
            {
                return decimal.ToDouble(this.currentLevelRisingSlewRateNumeric.Value);
            }
        }

        double CurrentLevelFallingSlewRate
        {
            get
            {
                return decimal.ToDouble(this.currentLevelFallingSlewRateNumeric.Value);
            }
        }

        bool OutputShorted
        {
            get
            {
                return this.outputShortedCheckBox.Checked;
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
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Output.Function = DCPowerSourceOutputFunction.DCCurrent;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Current.CurrentLevel = CurrentLevel;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Current.CurrentLevelRange = CurrentLevelRange;

                // Note that the Voltage Limit property is not applicable for electronic loads and is not configured in this example.
                // If you change the Output Function, configure the appropriate level, limit and range properties corresponding to your
                // selected Output Function.
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Current.VoltageLimitRange = VoltageLimitRange;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.SourceDelay = SourceDelay;

                // Configure the Output Shorted property to specify whether to simulate a short circuit in the electronic load.
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.OutputShorted = OutputShorted;

                // If you are using the DC Current or Constant Power Output Functions, set the Conduction Voltage Mode to "Automatic" or
                // "Enabled" to enable Conduction Voltage. If you are using the DC Voltage or Constant Resistance Output Functions, set the
                // Conduction Voltage Mode to "Automatic" or "Disabled" to disable Conduction Voltage.
                // If Conduction Voltage is enabled, set the Conduction Voltage On Threshold to configure the electronic load to start
                // sinking current when the input voltage exceeds the configured threshold, and set the Conduction Voltage Off Threshold to
                // configure the electronic load to stop sinking current when the input voltage falls below the threshold.
                // If Conduction Voltage is disabled, the electronic load attempts to sink the desired level regardless of the input voltage.
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.ConductionVoltageMode = ConductionVoltageMode;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.ConductionVoltageOnThreshold = ConductionVoltageOnThreshold;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.ConductionVoltageOffThreshold = ConductionVoltageOffThreshold;

                // If you are using the DC Current Output Function, configure the Current Level Rising Slew Rate and Current Level Falling Slew
                // Rate, in amps per microsecond, to control the rising and falling current slew rates of the electronic load while sinking current.
                // When using Output Functions besides DC Current, these properties have no effect.
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Current.CurrentLevelRisingSlewRate = CurrentLevelRisingSlewRate;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Current.CurrentLevelFallingSlewRate = CurrentLevelFallingSlewRate;

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