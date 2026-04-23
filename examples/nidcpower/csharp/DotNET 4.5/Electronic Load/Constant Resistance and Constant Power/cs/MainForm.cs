//=============================================================================================================
//
// Title:
//       NI-DCPower Constant Resistance and Constant Power
//
// Description:
//      This example demonstrates how to use the Constant Resistance Output Function to force a
//      resistance level on the electronic load and how to use the Constant Power Output Function
//      to force a power level on the electronic load.
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

namespace NationalInstruments.Examples.ConstantResistanceAndConstantPower
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
                    resourceNameComboBox.Items.Add(device.Name + "/0");
                }
            }
            if (resourceNameComboBox.Items.Count > 0)
            {
                resourceNameComboBox.SelectedIndex = 0;
            }
        }
        #endregion

        #region Mainform configuration values
        string FullyQualifiedChannelName
        {
            get
            {
                return this.resourceNameComboBox.Text;
            }
        }

        double ConstantResistanceLevel
        {
            get
            {
                return decimal.ToDouble(this.constantResistanceLevelNumeric.Value);
            }
        }

        double ConstantResistanceLevelRange
        {
            get
            {
                return decimal.ToDouble(this.constantResistanceLevelRangeNumeric.Value);
            }
        }

        double ConstantResistanceCurrentLimit
        {
            get
            {
                return decimal.ToDouble(this.constantResistanceCurrentLimitNumeric.Value);
            }
        }

        double ConstantPowerLevel
        {
            get
            {
                return decimal.ToDouble(this.constantPowerLevelNumeric.Value);
            }
        }

        double ConstantPowerLevelRange
        {
            get
            {
                return decimal.ToDouble(this.constantPowerLevelRangeNumeric.Value);
            }
        }

        double ConstantPowerCurrentLimit
        {
            get
            {
                return decimal.ToDouble(this.constantPowerCurrentLimitNumeric.Value);
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
                if (this.constantResistanceConstantPowerTabControl.SelectedTab == constantResistanceTab)
                {
                    dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Output.Function = DCPowerSourceOutputFunction.ConstantResistance;
                    dcPowerSession.Outputs[FullyQualifiedChannelName].Source.ConstantResistance.Level = ConstantResistanceLevel;
                    dcPowerSession.Outputs[FullyQualifiedChannelName].Source.ConstantResistance.CurrentLimit = ConstantResistanceCurrentLimit;
                    dcPowerSession.Outputs[FullyQualifiedChannelName].Source.ConstantResistance.LevelRange = ConstantResistanceLevelRange;
                }
                else if (this.constantResistanceConstantPowerTabControl.SelectedTab == constantPowerTab)
                {
                    dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Output.Function = DCPowerSourceOutputFunction.ConstantPower;
                    dcPowerSession.Outputs[FullyQualifiedChannelName].Source.ConstantPower.Level = ConstantPowerLevel;
                    dcPowerSession.Outputs[FullyQualifiedChannelName].Source.ConstantPower.CurrentLimit = ConstantPowerCurrentLimit;
                    dcPowerSession.Outputs[FullyQualifiedChannelName].Source.ConstantPower.LevelRange = ConstantPowerLevelRange;
                }

                // Configure the Source Delay to allow for sufficient startup delay for the input to reach the desired sinking level.
                // When starting from a 0 A or Off state, the electronic load requires additional startup delay before the input
                // begins to sink the desired level. The default Source Delay in this example takes this startup delay into account.
                // In cases where the electronic load is already sinking, less settling time may be needed.
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.SourceDelay = SourceDelay;

                dcPowerSession.Control.Initiate();
                dcPowerSession.Events.SourceCompleteEvent.WaitForEvent(new PrecisionTimeSpan(10.0));

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

        double calculateResistanceMeasurement(double voltage, double current)
        {
            return voltage / current;
        }

        double calculatePowerMeasurement(double voltage, double current)
        {
            return voltage * current;
        }

        void DisplayMeasurements(double voltage, double current, bool inCompliance)
        {
            this.currentMeasurementTextBox.Text = current.ToString("E");
            this.voltageMeasurementTextBox.Text = voltage.ToString("E");
            this.resistanceMeasurementTextBox.Text = calculateResistanceMeasurement(voltage, current).ToString("E");
            this.powerMeasurementTextBox.Text = calculatePowerMeasurement(voltage, current).ToString("E");
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