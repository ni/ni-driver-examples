//=============================================================================================================
//
// Title:
//      NI-DCPower Pulse Current
//
// Description:
//      Demonstrates how to use the DCPower Pulse API to generate a single Current
//      pulse.  This example initializes a session, configures the Source Mode and
//      the Output Function, configures the pulse parameters, initiates the pulse
//      output, and takes a measurement.
//
// Suggested Devices:
//      NI PXIe-4135, NI PXIe-4136, NI PXIe-4137, NI PXIe-4138, NI PXIe-4139
//
//=============================================================================================================


using System;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIDCPower;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.PulseCurrent
{
    public partial class MainForm : Form
    {
        NIDCPower dcPowerSession;
        public MainForm()
        {
            InitializeComponent();
            LoadDCPowerDeviceNames();
        }

        #region UI Initial Value Config Section
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

        double PulseCurrentLevel
        {
            get
            {
                return decimal.ToDouble(this.pulseCurrentLevelNumeric.Value);
            }
        }

        double PulseVoltageLimit
        {
            get
            {
                return decimal.ToDouble(this.pulseVoltageLimitNumeric.Value);
            }
        }

        double PulseOnTime
        {
            get
            {
                return decimal.ToDouble(this.pulseOnTimeNumeric.Value);
            }
        }

        PrecisionTimeSpan SourceDelay
        {
            get
            {
                return new PrecisionTimeSpan(decimal.ToDouble(this.sourceDelayNumeric.Value));
            }
        }

        double PulseCurrentLevelRange
        {
            get
            {
                return decimal.ToDouble(this.pulseCurrentLevelRangeNumeric.Value);
            }
        }

        double PulseVoltageLimitRange
        {
            get
            {
                return decimal.ToDouble(this.pulseVoltLimitRangeNumeric.Value);
            }
        }

        double PulseOffTime
        {
            get
            {
                return decimal.ToDouble(this.pulseOffTimeNumeric.Value);
            }
        }

        double ApertureTime
        {
            get
            {
                return decimal.ToDouble(this.apertureTimeNumeric.Value);
            }
        }

        double BiasCurrentLevel
        {
            get
            {
                return decimal.ToDouble(this.biasCurrentLevelNumeric.Value);
            }
        }

        double BiasVoltageLimit
        {
            get
            {
                return decimal.ToDouble(this.biasVoltageLimitNumeric.Value);
            }
        }

        PrecisionTimeSpan PulseBiasDelay
        {
            get
            {
                return new PrecisionTimeSpan(decimal.ToDouble(this.pulseBiasDelayNumeric.Value));
            }
        }

        double CurrentMeasurement
        {
            set
            {
                currentMeasurementTextBox.Text = value.ToString();
            }
        }

        double VoltageMeasurement
        {
            set
            {
                voltageMeasurementTextBox.Text = value.ToString();
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
              InitializeDCPowerSession(false);
                //Configure Session Parameters from UI.
                dcPowerSession.Source.Mode = DCPowerSourceMode.SinglePoint;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Output.Function = DCPowerSourceOutputFunction.PulseCurrent;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.PulseCurrent.CurrentLevel = PulseCurrentLevel;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.PulseCurrent.CurrentLevelRange = PulseCurrentLevelRange;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.PulseCurrent.BiasCurrentLevel = BiasCurrentLevel;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.PulseCurrent.VoltageLimit = PulseVoltageLimit;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.PulseCurrent.VoltageLimitRange = PulseVoltageLimitRange;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.PulseCurrent.BiasVoltageLimit = BiasVoltageLimit;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.PulseOnTime = PulseOnTime;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.PulseOffTime = PulseOffTime;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.PulseBiasDelay = PulseBiasDelay;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.SourceDelay = SourceDelay;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Measurement.ConfigureApertureTime(ApertureTime, DCPowerMeasureApertureTimeUnits.Seconds);
                //Initiate the device to start generation and acquisition.
                dcPowerSession.Control.Initiate();
                //Fetch voltage and current.
                DCPowerFetchResult result = dcPowerSession.Measurement.Fetch(FullyQualifiedChannelName,new PrecisionTimeSpan(10), 1);
                DisplayResult(result);
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

        private void DisplayResult(DCPowerFetchResult result)
        {
            VoltageMeasurement = result.VoltageMeasurements[0];
            CurrentMeasurement = result.CurrentMeasurements[0];
        }

        private void CloseSession()
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

        private static void ShowError(Exception ex)
        {
            MessageBox.Show(ex.Message, ex.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        void InitializeDCPowerSession(bool reset)
        {
            dcPowerSession = new NIDCPower(FullyQualifiedChannelName, reset, String.Empty);
            dcPowerSession.DriverOperation.Warning += new EventHandler<DCPowerWarningEventArgs>(DCPowerDriverOperationWarning);
        }
        void ChangeControlState(bool isEnabled)
        {
            resourceAndChannelNameGroupBox.Enabled = isEnabled;
            configurationGroupBox.Enabled = isEnabled;
            startButton.Enabled = isEnabled;
            resourceNameComboBox.Select();
            this.Refresh();
        }
        void DCPowerDriverOperationWarning(object sender, DCPowerWarningEventArgs e)
        {
            MessageBox.Show(e.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}