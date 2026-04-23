//=============================================================================================================
//
// Title:
//      NI-DCPower Software-Timed Two-Channel Voltage Sweep
//
// Description:
//      This example demonstrates how to set up a software-timed two-channel nested voltage
//      sweep and display the results in a graph (IV Curve).  Use this example to produce
//      the characteristic curves of a FET transistor.  It can be easily adapted to test
//      a BJT by performing a current sweep instead of a voltage sweep. This example
//      performs a software-timed sweep using Single Point source mode. Do not use this
//      example if your device supports Sequence source mode; use the hardware-timed
//      example instead.
//
//      Note: In this example the Output Function is set to DC Voltage. If you change the
//      Output Function to DC Current, you must use Current Level and Voltage Limit instead
//      of Voltage Level and Current Limit.
//
// Suggested Devices:
//      PXI-4110, PXI 4130
//      PXIe-4112, PXIe-4113, PXIe-4140, PXIe-4141, PXIe-4142, PXIe-4143, PXIe-4144, PXIe-4145, PXIe-4147, PXIe-4154, PXIe-4162, PXIe-4163
//
//=============================================================================================================

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIDCPower;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.SoftwareTimedTwoChannelSweep
{
    public partial class MainForm : Form
    {
        NIDCPower session;
        Dictionary<string, double[,]> measurements = new Dictionary<string, double[,]>();

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
                channel1SenseComboBox.Items.Add(item);
                channel2SenseComboBox.Items.Add(item);
            }
            channel1SenseComboBox.SelectedIndex = 0;
            channel2SenseComboBox.SelectedIndex = 0;
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

        string Channel1Name
        {
            get
            {
                return this.channel1NameTextBox.Text;
            }
        }

        string FullyQualifiedChannel1Name
        {
            get
            {
                return $"{ResourceName}/{Channel1Name}";
            }
        }

        int Channel1NumberOfPlots
        {
            get
            {
                return decimal.ToInt32(this.channel1NumberOfPlotsNumeric.Value);
            }
        }

        double Channel1VoltageLevelStart
        {
            get
            {
                return decimal.ToDouble(this.channel1VoltageLevelStartNumeric.Value);
            }
        }

        double Channel1VoltageLevelStop
        {
            get
            {
                return decimal.ToDouble(this.channel1VoltageLevelStopNumeric.Value);
            }
        }

        double Channel1CurrentLimit
        {
            get
            {
                return decimal.ToDouble(this.channel1CurrentLimitNumeric.Value);
            }
        }

        DCPowerMeasurementSense Channel1Sense
        {
            get
            {
                return (DCPowerMeasurementSense)this.channel1SenseComboBox.SelectedItem;
            }
        }

        PrecisionTimeSpan Channel1SourceDelay
        {
            get
            {
                return new PrecisionTimeSpan(decimal.ToDouble(this.channel1SourceDelayNumeric.Value));
            }
        }

        string Channel2Name
        {
            get
            {
                return this.channel2NameTextBox.Text;
            }
        }

        string FullyQualifiedChannel2Name
        {
            get
            {
                return $"{ResourceName}/{Channel2Name}";
            }
        }

        int Channel2NumberOfPoints
        {
            get
            {
                return decimal.ToInt32(this.channel2NumberOfPointsNumeric.Value);
            }
        }

        double Channel2VoltageLevelStart
        {
            get
            {
                return decimal.ToDouble(this.channel2VoltageLevelStartNumeric.Value);
            }
        }

        double Channel2VoltageLevelStop
        {
            get
            {
                return decimal.ToDouble(this.channel2VoltageLevelStopNumeric.Value);
            }
        }

        double Channel2CurrentLimit
        {
            get
            {
                return decimal.ToDouble(this.channel2CurrentLimitNumeric.Value);
            }
        }

        DCPowerMeasurementSense Channel2Sense
        {
            get
            {
                return (DCPowerMeasurementSense)this.channel2SenseComboBox.SelectedItem;
            }
        }

        PrecisionTimeSpan Channel2SourceDelay
        {
            get
            {
                return new PrecisionTimeSpan(decimal.ToDouble(this.channel2SourceDelayNumeric.Value));
            }
        }

        string SelectedPlotName
        {
            get
            {
                return this.selectedPlotNameComboBox.Text;
            }
        }
        #endregion

        #region Program functions
        void Start()
        {
        try
            {
                InitializeDCPowerSession();

                session.Source.Mode = DCPowerSourceMode.SinglePoint;

                //"" means all channels in the session
                session.Outputs[""].Source.Output.Function = DCPowerSourceOutputFunction.DCVoltage;

                session.Outputs[FullyQualifiedChannel1Name].Measurement.Sense = Channel1Sense;
                session.Outputs[FullyQualifiedChannel2Name].Measurement.Sense = Channel2Sense;

                session.Outputs[""].Source.Voltage.VoltageLevelAutorange = DCPowerSourceVoltageLevelAutorange.On;
                session.Outputs[""].Source.Voltage.CurrentLimitAutorange = DCPowerSourceCurrentLimitAutorange.On;

                session.Outputs[FullyQualifiedChannel1Name].Source.Voltage.CurrentLimit = Channel1CurrentLimit;
                session.Outputs[FullyQualifiedChannel2Name].Source.Voltage.CurrentLimit = Channel2CurrentLimit;

                session.Control.Initiate();

                double channel1StepSize = CalculateStepSize(Channel1VoltageLevelStart, Channel1VoltageLevelStop, Channel1NumberOfPlots);
                double channel2StepSize = CalculateStepSize(Channel2VoltageLevelStart, Channel2VoltageLevelStop, Channel2NumberOfPoints);

                // Nested Sweep with 2 channels.
                for (int plotIndex = 0; plotIndex < Channel1NumberOfPlots; plotIndex++)
                {
                    // Calculate the Voltage Level for this step.
                    double channel1VoltageLevel = (channel1StepSize * plotIndex) + Channel1VoltageLevelStart;
                    session.Outputs[FullyQualifiedChannel1Name].Source.Voltage.VoltageLevel = channel1VoltageLevel;

                    session.Outputs[FullyQualifiedChannel1Name].Events.SourceCompleteEvent.WaitForEvent(new PrecisionTimeSpan(Channel1SourceDelay.Milliseconds));

                    // Construct the plot name.
                    DCPowerMeasureResult measureResult = session.Measurement.Measure(FullyQualifiedChannel1Name);

                    string plotName = String.Format("Plot {0}: ch{1} v={2:0.00}", plotIndex, FullyQualifiedChannel1Name, measureResult.VoltageMeasurements[0]);

                    double[,] plot = new double[Channel2NumberOfPoints, 2];
                    for (int pointIndex = 0; pointIndex < Channel2NumberOfPoints; pointIndex++)
                    {
                        // Calculate the Voltage Level for this step.
                        double channel2VoltageLevel = (channel2StepSize * pointIndex) + Channel2VoltageLevelStart;
                        session.Outputs[FullyQualifiedChannel2Name].Source.Voltage.VoltageLevel = channel2VoltageLevel;

                        session.Outputs[FullyQualifiedChannel2Name].Events.SourceCompleteEvent.WaitForEvent(new PrecisionTimeSpan(Channel2SourceDelay.Milliseconds));

                        // Measure the Current and Voltage.
                        DCPowerMeasureResult result = session.Measurement.Measure(FullyQualifiedChannel2Name);
                        plot[pointIndex, 0] = result.VoltageMeasurements[0];
                        plot[pointIndex, 1] = result.CurrentMeasurements[0];
                    }

                    // Store the plotName and corresponding measurements in dictionary.
                    measurements.Add(plotName, plot);
                }

                UpdateMeasurements();

                session.Utility.Reset();
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
        #endregion

        void startButton_Click(object sender, EventArgs e)
        {
            ChangeControlState(false);
            measurements.Clear();
            selectedPlotNameComboBox.Items.Clear();
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

        void InitializeDCPowerSession()
        {
            string fullyQualifiedResourceNames = String.Join(",", FullyQualifiedChannel1Name, FullyQualifiedChannel2Name);
            session = new NIDCPower(fullyQualifiedResourceNames, false, String.Empty);
            session.DriverOperation.Warning += new EventHandler<DCPowerWarningEventArgs>(DCPowerDriverOperationWarning);
        }

        void DCPowerDriverOperationWarning(object sender, DCPowerWarningEventArgs e)
        {
            MessageBox.Show(e.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        static double CalculateStepSize(double voltageLevelStart, double voltageLevelStop, int numberOfPoints)
        {
            double stepSize = 0.0;
            if (numberOfPoints > 1) // To avoid dividing by 0.
            {
                stepSize = ((voltageLevelStop - voltageLevelStart) / (numberOfPoints - 1));
            }
            return stepSize;
        }

        void UpdateMeasurements()
        {
            foreach (KeyValuePair<string, double[,]> m in measurements)
            {
                selectedPlotNameComboBox.Items.Add(m.Key);
            }
            if (selectedPlotNameComboBox.Items.Count > 0)
            {
                selectedPlotNameComboBox.SelectedIndex = 0;
            }
            DisplayPlot(SelectedPlotName);
        }

        void DisplayPlot(string plotName)
        {
            measurementsDataGridView.Rows.Clear();

            double[,] plot;
            if (measurements.TryGetValue(plotName, out plot))
            {
                for (int i = 0; i < plot.GetLength(0); i++)
                {
                    measurementsDataGridView.Rows.Add(
                        (i + 1).ToString(),
                        plot[i, 0].ToString("E"),
                        plot[i, 1].ToString("E"));
                }
            }
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

        static void ShowError(Exception ex)
        {
            MessageBox.Show(ex.Message, ex.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        void ChangeControlState(bool flag)
        {
            channel1GroupBox.Enabled = flag;
            channel2GroupBox.Enabled = flag;
            resourceNameGroupBox.Enabled = flag;
            startButton.Enabled = flag;
            resourceNameComboBox.Select();
            this.Refresh();
        }
    }
}