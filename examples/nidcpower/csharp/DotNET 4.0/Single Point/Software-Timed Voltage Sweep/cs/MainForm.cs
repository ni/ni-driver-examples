//=============================================================================================================
//
// Title:
//      NI-DCPower Software-Timed Voltage Sweep
//
// Description:
//      This example demonstrates how to sweep the voltage on a single channel.
//      This example performs a software-timed sweep using Single Point source mode.
//      Do not use this example if your device supports Sequence source mode;
//      use the hardware-timed example instead.
//
//      Note: In this example the Output Function is set to  DC Voltage. If you change the 
//      Output Function to DC Current, you must use Current Level and Voltage Limit instead
//      of Voltage Level and Current Limit
//
//  Suggested Devices:
//      PXI-4110, PXI-4130
//      PXIe-4112, PXIe-4113, PXIe-4135, PXIe-4136, PXIe-4137, PXIe-4138, PXIe-4139, PXIe-4140, PXIe-4141, PXIe-4142, PXIe-4143, PXIe-4144, PXIe-4145, PXIe-4147, PXIe-4154, PXIe-4162, PXIe-4163
//
//=============================================================================================================

using System;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIDCPower;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;
using System.Threading;

namespace NationalInstruments.Examples.SoftwareTimedVoltageSweep
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

        #region MainForm configuration values
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

        int NumberOfPoints
        {
            get
            {
                return decimal.ToInt32(this.numberOfPointsNumeric.Value);
            }
        }

        double VoltageLevelStart
        {
            get
            {
                return decimal.ToDouble(this.voltageLevelStartNumeric.Value);
            }
        }

        double VoltageLevelStop
        {
            get
            {
                return decimal.ToDouble(this.voltageLevelStopNumeric.Value);
            }
        }

        double CurrentLimit
        {
            get
            {
                return decimal.ToDouble(this.currentLimitNumeric.Value);
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
            ClearMeasurementsDataGridView();
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
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Voltage.VoltageLevelAutorange = DCPowerSourceVoltageLevelAutorange.On;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Voltage.CurrentLimitAutorange = DCPowerSourceCurrentLimitAutorange.On;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Voltage.CurrentLimit = CurrentLimit;

                dcPowerSession.Control.Initiate();

                // Calculate the step size.
                double stepSize = 0;
                if (NumberOfPoints > 1) // Avoid dividing by 0.
                {
                    stepSize = ((VoltageLevelStop - VoltageLevelStart) / (NumberOfPoints - 1));
                }

                // For each step..
                for (int pointIndex = 0; pointIndex < NumberOfPoints; pointIndex++)
                {
                    // Calculate the Voltage Level for this step.
                    double voltageLevel = (stepSize * (double)pointIndex) + VoltageLevelStart;

                    dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Voltage.VoltageLevel = voltageLevel;

                    // Delay after sourcing so that the output can settle.
                    Thread.Sleep((int)SourceDelay.TotalMilliseconds);

                    DCPowerMeasureResult result = dcPowerSession.Measurement.Measure(FullyQualifiedChannelName);
                    UpdateMeasurements(result);
                }

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

        void InitializeDCPowerSession()
        {
            dcPowerSession = new NIDCPower(FullyQualifiedChannelName, false, String.Empty);
            dcPowerSession.DriverOperation.Warning += new EventHandler<DCPowerWarningEventArgs>(DCPowerDriverOperationWarning);
        }

        void DCPowerDriverOperationWarning(object sender, DCPowerWarningEventArgs e)
        {
            MessageBox.Show(e.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        void UpdateMeasurements(DCPowerMeasureResult result)
        {
            measurementsDataGridView.Rows.Add(
                (measurementsDataGridView.Rows.Count + 1).ToString(),
                result.VoltageMeasurements[0].ToString("E"),
                result.CurrentMeasurements[0].ToString("E"));
        }

        void ClearMeasurementsDataGridView()
        {
            measurementsDataGridView.Rows.Clear();
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