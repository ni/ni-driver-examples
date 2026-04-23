//=============================================================================================================
//
// Title:
//      NI-DCPower Measure Step Response
//
// Description:
//      This example demonstrates how to measure while the output is changing.
//      This example can be used to configure the transient response and observe
//      how the settings change the output.
//
//      Note: In this example the Output Function is set to DC Voltage. If you change the
//      Output Function to DC Current, you must use Current Level and Voltage Limit instead
//      of Voltage Level and Current Limit.
//
// Suggested Devices:
//      PXIe-4135, PXIe-4136, PXIe-4137, PXIe-4138, PXIe-4139, PXIe-4140, PXIe-4141, PXIe-4142, PXIe-4143, PXIe-4144, PXIe-4145, PXIe-4147, PXIe-4162, PXIe-4163
//
//=============================================================================================================

using System;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIDCPower;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.MeasureStepResponse
{
    public partial class MainForm : Form
    {
        const int SequenceSize = 2;

        NIDCPower dcPowerSession;

        public MainForm()
        {
            InitializeComponent();
            ConfigureTransientResponseComboBox();
            ConfigureVoltageSetpointsDataGridView();
            LoadDCPowerDeviceNames();
        }

        #region MainForm initial configuration
        void ConfigureTransientResponseComboBox()
        {
            transientResponseComboBox.Items.Add(DCPowerSourceTransientResponse.Normal);
            transientResponseComboBox.Items.Add(DCPowerSourceTransientResponse.Fast);
            transientResponseComboBox.Items.Add(DCPowerSourceTransientResponse.Slow);
            transientResponseComboBox.SelectedIndex = 0;
        }

        void ConfigureVoltageSetpointsDataGridView()
        {
            for (int i = 0; i < SequenceSize; i++)
            {
                voltageSetPointsDataGridView.Rows.Add(
                    (i + 1).ToString(),
                    (i * 5.0).ToString("E"));
            }
            voltageSetPointsDataGridView.CellEnter += new DataGridViewCellEventHandler(AllowSingleClickEditInDataGridView);
        }

        void AllowSingleClickEditInDataGridView(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView target = sender as DataGridView;
            if (target != null)
            {
                target.BeginEdit(true);
                ComboBox cmb = target.EditingControl as ComboBox;
                if (cmb != null)
                {
                    cmb.DroppedDown = true;
                }
            }
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

        int MeasureRecordLength
        {
            get
            {
                return decimal.ToInt32(this.measureRecordLengthNumeric.Value);
            }
        }

        DCPowerSourceTransientResponse TransientResponse
        {
            get
            {
                return (DCPowerSourceTransientResponse)this.transientResponseComboBox.SelectedItem;
            }
        }

        double ApertureTime
        {
            get
            {
                return decimal.ToDouble(this.apertureTimeNumericUpDown.Value);
            }
        }

        double[] VoltageSetpoints
        {
            get
            {
                double[] sequence = new double[this.voltageSetPointsDataGridView.Rows.Count];
                for (int i = 0; i < sequence.Length; i++)
                {
                    sequence[i] = Double.Parse(this.voltageSetPointsDataGridView.Rows[i].Cells[1].Value.ToString());
                }
                return sequence;
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
                InitializeDCPowerSession(reset: true);

                dcPowerSession.Source.Mode = DCPowerSourceMode.Sequence;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Measurement.ConfigureApertureTime(ApertureTime, DCPowerMeasureApertureTimeUnits.Seconds);

                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Output.Function = DCPowerSourceOutputFunction.DCVoltage;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Voltage.CurrentLimit = CurrentLimit;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Voltage.VoltageLevelRange = VoltageLevelRange;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Voltage.CurrentLimitRange = CurrentLimitRange;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.TransientResponse = TransientResponse;

                // Configure the source delay to 0 in order to view the entire step response.
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.SourceDelay = new PrecisionTimeSpan(0.0);

                dcPowerSession.Outputs[FullyQualifiedChannelName].Measurement.RecordLength = MeasureRecordLength;
                dcPowerSession.Measurement.Configuration.MeasureWhen = DCPowerMeasurementWhen.AutomaticallyAfterSourceComplete;

                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.SetSequence(VoltageSetpoints);
                dcPowerSession.Control.Commit();

                // Read the measure record delta time to determine the length of time between each
                // measurement. The length of time between each measurement is used for graphing
                // later in the example and to compute number of measurements to fetch.
                double measureRecordDeltaTime = dcPowerSession.Outputs[FullyQualifiedChannelName].Measurement.RecordDeltaTime;

                dcPowerSession.Control.Initiate();

                // Fetch timeout must be at least (measure record delta time  measure record length number
                // of measure records). Multiply this by 2 to ensure that the timeout is large enough.
                PrecisionTimeSpan fetchTimeout = new PrecisionTimeSpan(measureRecordDeltaTime * MeasureRecordLength * SequenceSize * 2);
                DCPowerFetchResult result = dcPowerSession.Measurement.Fetch(FullyQualifiedChannelName, fetchTimeout, MeasureRecordLength * SequenceSize);
                UpdateMeasurements(result.VoltageMeasurements, result.CurrentMeasurements);

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

        void InitializeDCPowerSession(bool reset)
        {
            dcPowerSession = new NIDCPower(FullyQualifiedChannelName, reset, String.Empty);
            dcPowerSession.DriverOperation.Warning += new EventHandler<DCPowerWarningEventArgs>(DCPowerDriverOperationWarning);
        }

        void DCPowerDriverOperationWarning(object sender, DCPowerWarningEventArgs e)
        {
            MessageBox.Show(e.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        void UpdateMeasurements(double[] voltageMeasurements, double[] currentMeasurements)
        {
            for (int i = 0; i < voltageMeasurements.Length; i++)
            {
                measurementsDataGridView.Rows.Add(
                    (i + 1).ToString(),
                    voltageMeasurements[i].ToString("E"),
                    currentMeasurements[i].ToString("E"));
            }
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
            resourceNameAndChannelNameGroupBox.Enabled = isEnabled;
            startButton.Enabled = isEnabled;
            configurationGroupBox.Enabled = isEnabled;
            resourceNameComboBox.Select();
            this.Refresh();
        }
    }
}