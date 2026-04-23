//=============================================================================================================
//
// Title:
//     NI-DCPower Hardware-Timed Two-Channel Sweep
//
// Description:
//      This example demonstrates how to set up a hardware-timed two-channel nested
//      voltage sweep. Use this example to produce the characteristic curves of a FET transistor.
//      It can be easily adapted to test a BJT by performing a current sweep instead of
//      a voltage sweep. This example performs a hardware-timed sweep (with triggers
//      and events) using Sequence source mode.
//
//      Note: In this example the Output Function is set to DC Voltage. If you change the
//      Output Function to DC Current, you must use Current Level and Voltage Limit instead
//      of Voltage Level and Current Limit.
//
// Suggested Devices:
//      PXI-4132
//      PXIe-4112, PXIe-4113, PXIe-4135, PXIe-4136, PXIe-4137, PXIe-4138, PXIe-4139, PXIe-4140, PXIe-4141, PXIe-4142, PXIe-4143, PXIe-4144, PXIe-4145, PXIe-4147, PXIe-4154, PXIe-4162, PXIe-4163
//
//============================================================================================================

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIDCPower;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.HardwareTimedTwoChannelSweep
{
    public partial class MainForm : Form
    {
        NIDCPower session;
        Dictionary<string, DCPowerFetchResult> measurements = new Dictionary<string, DCPowerFetchResult>();

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
                device0SenseComboBox.Items.Add(item);
                device1SenseComboBox.Items.Add(item);
            }
            device0SenseComboBox.SelectedIndex = 0;
            device1SenseComboBox.SelectedIndex = 0;
        }

        void LoadDCPowerDeviceNames()
        {
            using (ModularInstrumentsSystem dcPowerDevices = new ModularInstrumentsSystem("NI-DCPower"))
            {
                foreach (DeviceInfo device in dcPowerDevices.DeviceCollection)
                {
                    device0ResourceNameComboBox.Items.Add(device.Name);
                    device1ResourceNameComboBox.Items.Add(device.Name);
                }
            }
            if (device0ResourceNameComboBox.Items.Count > 0)
            {
                device0ResourceNameComboBox.SelectedIndex = 0;
            }
            if (device1ResourceNameComboBox.Items.Count > 1)
            {
                device1ResourceNameComboBox.SelectedIndex = 1;
            }
        }
        #endregion

        #region MainForm configuration values
        internal string ResourceName0
        {
            get
            {
                return this.device0ResourceNameComboBox.Text;
            }
        }

        internal string ChannelName0
        {
            get
            {
                return this.device0ChannelNameComboBox.Text;
            }
        }

        string FullyQualifiedChannelName0
        {
            get
            {
                return $"{ResourceName0}/{ChannelName0}";
            }
        }

        internal int NumberOfPlots0
        {
            get
            {
                return decimal.ToInt32(this.device0NumberOfPlotsNumeric.Value);
            }
        }

        internal double CurrentLimit0
        {
            get
            {
                return decimal.ToDouble(this.device0CurrentLimitNumeric.Value);
            }
        }

        internal double VoltageLevelStart0
        {
            get
            {
                return decimal.ToDouble(this.device0VoltageLevelStartNumeric.Value);
            }
        }

        internal double VoltageLevelStop0
        {
            get
            {
                return decimal.ToDouble(this.device0VoltageLevelStopNumeric.Value);
            }
        }

        internal PrecisionTimeSpan SourceDelay0
        {
            get
            {
                return new PrecisionTimeSpan(decimal.ToDouble(this.device0SourceDelayNumeric.Value));
            }
        }

        internal DCPowerMeasurementSense Sense0
        {
            get
            {
                return (DCPowerMeasurementSense)this.device0SenseComboBox.SelectedItem;
            }
        }

        internal string ResourceName1
        {
            get
            {
                return this.device1ResourceNameComboBox.Text;
            }
        }

        internal string ChannelName1
        {
            get
            {
                return this.device1ChannelNameComboBox.Text;
            }
        }

        string FullyQualifiedChannelName1
        {
            get
            {
                return $"{ResourceName1}/{ChannelName1}";
            }
        }

        internal int NumberOfPoints1
        {
            get
            {
                return decimal.ToInt32(this.device1NumberOfPointsNumeric.Value);
            }
        }

        internal double CurrentLimit1
        {
            get
            {
                return decimal.ToDouble(this.device1CurrentLimitNumeric.Value);
            }
        }

        internal double VoltageLevelStart1
        {
            get
            {
                return decimal.ToDouble(this.device1VoltageLevelStartNumeric.Value);
            }
        }

        internal double VoltageLevelStop1
        {
            get
            {
                return decimal.ToDouble(this.device1VoltageLevelStopNumeric.Value);
            }
        }

        internal PrecisionTimeSpan SourceDelay1
        {
            get
            {
                return new PrecisionTimeSpan(decimal.ToDouble(this.device1SourceDelayNumeric.Value));
            }
        }

        internal DCPowerMeasurementSense Sense1
        {
            get
            {
                return (DCPowerMeasurementSense)this.device1SenseComboBox.SelectedItem;
            }
        }

        PrecisionTimeSpan Timeout
        {
            get
            {
                return new PrecisionTimeSpan(decimal.ToDouble(this.timeoutNumeric.Value));
            }
        }

        string SelectedPlotName
        {
            get
            {
                return this.plotNameComboBox.Text;
            }
        }
        #endregion

        void startButton_Click(object sender, EventArgs e)
        {
            ChangeControlState(false);
            measurements.Clear();
            measurementsDataGridView.Rows.Clear();
            Start();
            ChangeControlState(true);
        }

        void mainForm_Closing(object sender, FormClosingEventArgs e)
        {
            CloseSession();
        }

        void plotNameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayPlot(SelectedPlotName);
        }

        void Start()
        {
            //FullyQualifiedChannelName0 controls the gate voltage
            //FullyQualifiedChannelName1 controls the drain voltage

            try
            {
                InitializeDCPowerSession();

                session.Source.Mode = DCPowerSourceMode.Sequence;

                //"" means all channels in the session
                session.Outputs[""].Source.Output.Function = DCPowerSourceOutputFunction.DCVoltage;

                session.Outputs[FullyQualifiedChannelName0].Measurement.Sense = Sense0;
                session.Outputs[FullyQualifiedChannelName1].Measurement.Sense = Sense1;

                session.Outputs[""].Source.Voltage.VoltageLevelAutorange = DCPowerSourceVoltageLevelAutorange.On;
                session.Outputs[""].Source.Voltage.CurrentLimitAutorange = DCPowerSourceCurrentLimitAutorange.On;

                session.Outputs[FullyQualifiedChannelName0].Source.SourceDelay = SourceDelay0;
                session.Outputs[FullyQualifiedChannelName1].Source.SourceDelay = SourceDelay1;

                session.Outputs[FullyQualifiedChannelName0].Source.Voltage.CurrentLimit = CurrentLimit0;
                session.Outputs[FullyQualifiedChannelName1].Source.Voltage.CurrentLimit = CurrentLimit1;

                double[] voltageLevelsSequence0 = CreateVoltageLevelsSequence(VoltageLevelStart0, VoltageLevelStop0, NumberOfPlots0);
                double[] voltageLevelsSequence1 = CreateVoltageLevelsSequence(VoltageLevelStart1, VoltageLevelStop1, NumberOfPoints1);

                session.Outputs[FullyQualifiedChannelName0].Source.SetSequence(voltageLevelsSequence0);
                session.Outputs[FullyQualifiedChannelName1].Source.SetSequence(voltageLevelsSequence1);

                // The Source trigger for the gate device is the Sequence Iteration Complete event
                // of the drain device. This way the gate device will start sourcing the next gate
                // voltage when the drain completes an iteration of its sequence.
                string inputTerminal0 = BuildFullyQualifiedTerminalName(session, ResourceName1, ChannelName1, "SequenceIterationCompleteEvent");
                session.Outputs[FullyQualifiedChannelName0].Triggers.SourceTrigger.DigitalEdge.Configure(inputTerminal0, DCPowerTriggerEdge.Rising);

                // The Start trigger and the Sequence Advance trigger for the drain device come from the
                // Measure Complete event of the gate device. This way the drain device starts each
                // iteration of the sequence when the gate device finishes sourcing and measuring
                // the gate voltage.
                string inputTerminal1 = BuildFullyQualifiedTerminalName(session, ResourceName0, ChannelName0, "MeasureCompleteEvent");
                session.Outputs[FullyQualifiedChannelName1].Triggers.StartTrigger.DigitalEdge.Configure(inputTerminal1, DCPowerTriggerEdge.Rising);
                session.Outputs[FullyQualifiedChannelName1].Triggers.SequenceAdvanceTrigger.DigitalEdge.Configure(inputTerminal1, DCPowerTriggerEdge.Rising);

                // Configure the second device to loop over the same sequence as many times as there
                // are gate voltages.
                session.Outputs[FullyQualifiedChannelName1].Source.SequenceLoopCount = NumberOfPlots0;

                // Commit the settings to the devices so that the driver establishes all the routing
                // required for the configured triggers and events.
                session.Outputs[FullyQualifiedChannelName0].Control.Commit();
                session.Outputs[FullyQualifiedChannelName1].Control.Commit();

                // Initiate both devices. Initiate the drain device first so that it waits for the
                // Start trigger before the gate device sources its gate voltage.
                session.Outputs[FullyQualifiedChannelName1].Control.Initiate();
                session.Outputs[FullyQualifiedChannelName0].Control.Initiate();

                // Wait for the sequence engine to be done on the drain device. When the drain device's
                // sequence engine is done, the gate device engine must also be done due to the trigger
                // setup (the gate device completes the last step in its sequence just before the
                // drain device starts the last iteration of its sequence).
                session.Outputs[FullyQualifiedChannelName1].Events.SequenceEngineDoneEvent.WaitForEvent(Timeout);

                // Fetch the array of measured gate voltages from the gate device.
                PrecisionTimeSpan fetchTimeout = new PrecisionTimeSpan(1.0);
                DCPowerFetchResult measurements0, measurements1;
                measurements0 = session.Measurement.Fetch(FullyQualifiedChannelName0, Timeout, NumberOfPlots0);

                // For each gate voltage...
                for (int plotIndex = 0; plotIndex < NumberOfPlots0; plotIndex++)
                {
                    // Fetch the array of measured drain voltages and currents from the drain device for the iteration.
                    measurements1 = session.Measurement.Fetch(FullyQualifiedChannelName1, fetchTimeout, NumberOfPoints1);

                    // Store the results in dictionary.
                    string plotName = String.Format("Plot {0}: ch{1} v={2:0.00}", plotIndex, FullyQualifiedChannelName0, measurements0.VoltageMeasurements[plotIndex]);
                    measurements.Add(plotName, measurements1);
                }

                UpdateMeasurements();

                session.Outputs[""].Source.Output.Enabled = false;
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

        static double[] CreateVoltageLevelsSequence(double voltageLevelStart, double voltageLevelStop, int numberOfPoints)
        {
            double stepSize = 0.0;
            if (numberOfPoints > 1) // To avoid dividing by 0.
            {
                // Calculate step size.
                stepSize = ((voltageLevelStop - voltageLevelStart) / (numberOfPoints - 1));
            }
            double[] voltageLevelsSequence = new double[numberOfPoints];
            for (int pointIndex = 0; pointIndex < numberOfPoints; pointIndex++)
            {
                // Calculate the Voltage Level for this step.
                voltageLevelsSequence[pointIndex] = (stepSize * (double)pointIndex) + voltageLevelStart;
            }
            return voltageLevelsSequence;
        }

        void InitializeDCPowerSession()
        {
            string fullyQualifiedResourceNames = String.Join(",",FullyQualifiedChannelName0, FullyQualifiedChannelName1);
            session = new NIDCPower(fullyQualifiedResourceNames, false, String.Empty);
            session.DriverOperation.Warning += new EventHandler<DCPowerWarningEventArgs>(DCPowerDriverOperationWarning);
        }

        void DCPowerDriverOperationWarning(object sender, DCPowerWarningEventArgs e)
        {
            MessageBox.Show(e.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        void CloseSession()
        {
            try
            {
                if (session != null)
                {
                    session.Close();
                    session = null;
                }
            }
            catch (Exception ex)
            {
                ShowError(ex);
                Application.Exit();
            }
        }

        void UpdateMeasurements()
        {
            plotNameComboBox.Items.Clear();
            foreach (KeyValuePair<string, DCPowerFetchResult> measurement in measurements)
            {
                plotNameComboBox.Items.Add(measurement.Key);
            }
            if (plotNameComboBox.Items.Count > 0)
            {
                plotNameComboBox.SelectedIndex = 0;
            }
            DisplayPlot(SelectedPlotName);
        }

        void DisplayPlot(string plotName)
        {
            measurementsDataGridView.Rows.Clear();

            DCPowerFetchResult plot;
            if (measurements.TryGetValue(plotName, out plot))
            {
                for (int i = 0; i < plot.VoltageMeasurements.Length; i++)
                {
                    measurementsDataGridView.Rows.Add(
                        (i + 1).ToString(),
                        plot.VoltageMeasurements[i].ToString("E"),
                        plot.CurrentMeasurements[i].ToString("E"));
                }
            }
        }

        static void ShowError(Exception ex)
        {
            MessageBox.Show(ex.Message, ex.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        static string BuildFullyQualifiedTerminalName(NIDCPower dcPowerSession, string resourceName, string channelName, string localTerminalName)
        {
            string modelName = dcPowerSession.Instruments[resourceName].Identity.InstrumentModel;
            if (modelName.Equals("NI PXI-4132", StringComparison.OrdinalIgnoreCase))
            {
                return String.Format("/{0}/{1}", resourceName, localTerminalName);
            }
            else
            {
                return String.Format("/{0}/Engine{1}/{2}", resourceName, channelName, localTerminalName);
            }
        }

        void ChangeControlState(bool isEnabled)
        {
            this.device0GroupBox.Enabled = isEnabled;
            this.device1GroupBox.Enabled = isEnabled;
            this.timeoutGroupBox.Enabled = isEnabled;
            this.startButton.Enabled = isEnabled;
            this.device0ResourceNameComboBox.Select();
            this.Refresh();
        }
    }
}