//=============================================================================================================
//
// Title:
//      NI-DCPower Simultaneous Operation
//
// Description:
//      This example demonstrates how to simultaneously change the configuration on multiple
//      channels.  This example initializes a session, configures the Voltage Levels and
//      Current Limits on two channels, and updates the outputs of the device simultaneously
//      and then takes measurements on both channels.
//
//      Note: In this example the Output Function is set to DC Voltage. If you change the
//      Output Function to DC Current, you must use Current Level and Voltage Limit instead
//      of Voltage Level and Current Limit.
//
//  Suggested Devices:
//      PXI-4110, PXI-4130
//      PXIe-4112, PXIe-4113, PXIe-4140, PXIe-4141, PXIe-4142, PXIe-4143, PXIe-4144, PXIe-4145, PXIe-4147, PXIe-4154, PXIe-4162, PXIe-4163
//
//=============================================================================================================

using System;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIDCPower;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.SimultaneousOperation
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

        #region MainForm initial values
        string ResourceName
        {
            get
            {
                return this.resourceNameComboBox.Text;
            }
        }

        string FirstChannelName
        {
            get
            {
                return this.firstChannelNameTextBox.Text;
            }
        }

        string FirstFullyQualifiedChannelName
        {
            get
            {
                return $"{ResourceName}/{FirstChannelName}";
            }
        }

        double FirstChannelVoltageLevel
        {
            get
            {
                return decimal.ToDouble(this.firstChannelVoltagLevelNumeric.Value);
            }
        }

        double FirstChannelCurrentLimit
        {
            get
            {
                return decimal.ToDouble(this.firstChannelCurrentLimitNumeric.Value);
            }
        }

        string SecondChannelName
        {
            get
            {
                return this.secondChannelNameTextBox.Text;
            }
        }

        string SecondFullyQualifiedChannelName
        {
            get
            {
                return $"{ResourceName}/{SecondChannelName}";
            }
        }

        double SecondChannelVoltageLevel
        {
            get
            {
                return decimal.ToDouble(this.secondChannelVoltageLevelNumeric.Value);
            }
        }

        double SecondChannelCurrentLimit
        {
            get
            {
                return decimal.ToDouble(this.secondChannelCurrentLimitNumeric.Value);
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

                //"" means all channels in the session
                dcPowerSession.Outputs[""].Source.Output.Function = DCPowerSourceOutputFunction.DCVoltage;

                dcPowerSession.Outputs[FirstFullyQualifiedChannelName].Source.Voltage.VoltageLevel = FirstChannelVoltageLevel;
                dcPowerSession.Outputs[FirstFullyQualifiedChannelName].Source.Voltage.CurrentLimit = FirstChannelCurrentLimit;

                dcPowerSession.Outputs[SecondFullyQualifiedChannelName].Source.Voltage.VoltageLevel = SecondChannelVoltageLevel;
                dcPowerSession.Outputs[SecondFullyQualifiedChannelName].Source.Voltage.CurrentLimit = SecondChannelCurrentLimit;

                dcPowerSession.Control.Initiate();

                string fullyQualifiedChannelNames = String.Join(",", FirstFullyQualifiedChannelName, SecondFullyQualifiedChannelName);

                dcPowerSession.Events.SourceCompleteEvent.WaitForEvent( new PrecisionTimeSpan(5.0));
                DCPowerMeasureResult result = dcPowerSession.Measurement.Measure(fullyQualifiedChannelNames);

                UpdateMeasurements(result);
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

        void UpdateMeasurements(DCPowerMeasureResult newResult)
        {

            firstChaannelVoltageMeasurementTextBox.Text = newResult.VoltageMeasurements[0].ToString("E");
            firstChannelCurrentMeasurementTextBox.Text = newResult.CurrentMeasurements[0].ToString("E");
            secondChaannelVoltageMeasurementTextBox.Text = newResult.VoltageMeasurements[1].ToString("E");
            secondChannelCurrentMeasurementTextBox.Text = newResult.CurrentMeasurements[1].ToString("E");
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
            string fullyQualifiedResourceNames = String.Join(",", FirstFullyQualifiedChannelName, SecondFullyQualifiedChannelName);
            dcPowerSession = new NIDCPower(fullyQualifiedResourceNames, false, String.Empty);
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

        void ChangeControlState(bool isEnabled)
        {
            resourceNameGroupBox.Enabled = isEnabled;
            firstChannelGroupBox.Enabled = isEnabled;
            secondChannelGroupBox.Enabled = isEnabled;
            startButton.Enabled = isEnabled;
            resourceNameComboBox.Select();
            this.Refresh();
        }
    }
}