//=============================================================================================================
//
// Title:
//      NI-DCPower Hardware-Timed Voltage Sweep
//
// Description:
//      This example demonstrates how to use triggers and events to synchronize multiple
//      channels in Sequence source mode. Use this example to sequence multiple channels
//      in lock-step.
//
//      Note: In this example the Output Function is set to DC Voltage. If you change the
//      Output Function to DC Current, you must use Current Level and Voltage Limit instead
//      of Voltage Level and Current Limit.
//
//  Suggested Devices:
//      PXI-4132
//      PXIe-4112, PXIe-4113, PXIe-4135, PXIe-4136, PXIe-4137, PXIe-4138, PXIe-4139, PXIe-4140, PXIe-4141, PXIe-4142, PXIe-4143, PXIe-4144, PXIe-4145, PXIe-4147, PXIe-4154, PXIe-4162, PXIe-4163
//
//============================================================================================================

using System;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIDCPower;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.HardwareTimedVoltageSweep
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

        PrecisionTimeSpan Timeout
        {
            get
            {
                return new PrecisionTimeSpan(decimal.ToDouble(this.timeoutNumeric.Value));
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

                dcPowerSession.Source.Mode = DCPowerSourceMode.Sequence;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Output.Function = DCPowerSourceOutputFunction.DCVoltage;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Voltage.VoltageLevelAutorange = DCPowerSourceVoltageLevelAutorange.On;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Voltage.CurrentLimitAutorange = DCPowerSourceCurrentLimitAutorange.On;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.SourceDelay = SourceDelay;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Voltage.CurrentLimit = CurrentLimit;

                double[] voltageLevelsSequence = CreateVoltageLevelsSequence();
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.SetSequence(voltageLevelsSequence);

                dcPowerSession.Outputs[FullyQualifiedChannelName].Control.Initiate();
                dcPowerSession.Outputs[FullyQualifiedChannelName].Events.SequenceEngineDoneEvent.WaitForEvent(Timeout);

                DCPowerFetchResult result = dcPowerSession.Measurement.Fetch(FullyQualifiedChannelName, Timeout, NumberOfPoints);
                DisplayMeasurements(result);

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

        void ClearMeasurementsDataGridView()
        {
            measurementsDataGridView.Rows.Clear();
        }

        double[] CreateVoltageLevelsSequence()
        {
            double stepSize = 0.0;
            if (NumberOfPoints > 1) // To avoid dividing by 0.
            {
                // Calculate step size.
                stepSize = ((VoltageLevelStop - VoltageLevelStart) / (NumberOfPoints - 1));
            }
            double[] voltageLevelsSequence = new double[NumberOfPoints];
            for (int pointIndex = 0; pointIndex < NumberOfPoints; pointIndex++)
            {
                // Calculate the Voltage Level for this step.
                voltageLevelsSequence[pointIndex] = (stepSize * (double)pointIndex) + VoltageLevelStart;
            }
            return voltageLevelsSequence;
        }

        void DisplayMeasurements(DCPowerFetchResult result)
        {
            for (int i = 0; i < result.VoltageMeasurements.Length; i++)
            {
                measurementsDataGridView.Rows.Add(
                    (i + 1).ToString(),
                    result.VoltageMeasurements[i].ToString("E"),
                    result.CurrentMeasurements[i].ToString("E"));
            }
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
            resourceNameAndChannelNameGroupBox.Enabled = isEnabled;
            configurationGroupBox.Enabled = isEnabled;
            startButton.Enabled = isEnabled;
            resourceNameComboBox.Select();
            this.Refresh();
        }
    }
}