//=============================================================================================================
//
// Title:
//      NI-DCPower Advanced Sequence Output Function
//
// Description:
//      This example demonstrates how to use the Advanced Sequence functions to
// change the output function in a sequence.  This example initializes a
// session, configures the Source Delay, creates an Advanced Sequence, creates
// a step to set the configured Voltage Level in DC Voltage mode, creates a step
// to set the configured Current Level in DC Current mode, initiates generation,
// waits for a specified delay, and then measures the voltage and current output.
// This example uses Advanced Sequence source mode.
//
//  Suggested Device:
//      PXIe-4135, PXIe-4136, PXIe-4137, PXIe-4138, PXIe-4139, PXIe-4140, PXIe-4141, PXIe-4142, PXIe-4143, PXIe-4144, PXIe-4145, PXIe-4147, PXIe-4162, PXIe-4163
//
//============================================================================================================
using System;
using System.Windows.Forms;
using NationalInstruments;
using NationalInstruments.ModularInstruments.NIDCPower;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.AdvancedSequenceOutputFunction
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
                return Convert.ToInt32(this.stepsNumeric.Value);
            }
        }

        double CurrentLevelStart
        {
            get
            {
                return Convert.ToDouble(this.currentLevelStartNumeric.Value);
            }
        }

        double VoltageLevelStart
        {
            get
            {
                return Convert.ToDouble(this.voltageLevelStartNumeric.Value);
            }
        }

        double CurrentLevelStop
        {
            get
            {
                return Convert.ToDouble(this.currentLevelStopNumeric.Value);
            }
        }

        double VoltageLevelStop
        {
            get
            {
                return Convert.ToDouble(this.voltageLevelStopNumeric.Value);
            }
        }

        PrecisionTimeSpan SourceDelay
        {
            get
            {
                return new PrecisionTimeSpan(Convert.ToDouble(this.delayNumeric.Value));
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

        void Start()
        {
            /* Specify the Advanced Sequence Attributes which can change per step. */
            DCPowerAdvancedSequenceProperty[] advancedSequenceProperties = {  DCPowerAdvancedSequenceProperty.VoltageLevel, DCPowerAdvancedSequenceProperty.CurrentLevel, DCPowerAdvancedSequenceProperty.OutputFunction};

            try
            {

                InitializeDCPowerSession();

                /* Allocate enough memory for the sequence steps. */
                double[] voltageLevelsSequence = new double[NumberOfPoints];
                double[] currentLevelsSequence = new double[NumberOfPoints];

                /* Configure the Source mode to Sequence. */
                dcPowerSession.Source.Mode = DCPowerSourceMode.Sequence;

                /* Set the Source Delay.  This is the amount of time the device should
                wait after each sourcing step in the sequence. The device
                automatically takes a measurement after this delay. */
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.SourceDelay = SourceDelay;

                /* Set the Voltage Level Autorange and Current Level Autorange to On to
                   configure the Voltage Level and Current Level without having to
                   explicitly set the range. */
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Voltage.VoltageLevelAutorange = DCPowerSourceVoltageLevelAutorange.On;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Current.CurrentLevelAutorange = DCPowerSourceCurrentLevelAutorange.On;

                /* Create the Advanced Sequence. */
                string advancedSequenceName = "MySequence";
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.AdvancedSequencing.CreateAdvancedSequence(advancedSequenceName, advancedSequenceProperties, true);

                /* Create Voltage Setpoints. */
                /* Calculate the step size. */
                double stepSize;
                if (NumberOfPoints == 1)
                {
                    /* To avoid dividing by 0. */
                    stepSize = 0;
                }
                else
                {
                    stepSize = (VoltageLevelStop - VoltageLevelStart) / ((NumberOfPoints) - 1);
                }

                /* Create the sequence. */
                for (int pointIndex = 0; pointIndex < NumberOfPoints; ++pointIndex)
                {
                    /* Calculate the Voltage Level for this step. */
                    voltageLevelsSequence[pointIndex] = (stepSize * (double)pointIndex) + VoltageLevelStart;
                }

                /* Create Current Setpoints. */
                /* Calculate the step size. */
                if (NumberOfPoints == 1)
                {
                    /* To avoid dividing by 0. */
                    stepSize = 0;
                }
                else
                {
                    stepSize = (CurrentLevelStop - CurrentLevelStart) / ((NumberOfPoints) - 1);
                }

                /* Create the sequence. */
                for (int pointIndex = 0; pointIndex < NumberOfPoints; ++pointIndex)
                {
                    /* Calculate the Current Level for this step. */
                    currentLevelsSequence[pointIndex] = (stepSize * (double)pointIndex) + CurrentLevelStart;
                }

                /* Create new Advanced Sequence Step for every Current Level. */
                for (int i = 0; i < NumberOfPoints; i++)
                {
                    dcPowerSession.Outputs[FullyQualifiedChannelName].Source.AdvancedSequencing.CreateAdvancedSequenceStep(true);

                    /* Set the Output Function to DC Voltage.  */
                    dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Output.Function = DCPowerSourceOutputFunction.DCVoltage;

                    /* Configure the Voltage Level. */
                    dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Voltage.VoltageLevel = voltageLevelsSequence[i];
                }

                /* Create new Advanced Sequence Step for every Voltage Level. */
                for (int i = 0; i < NumberOfPoints; i++)
                {
                    dcPowerSession.Outputs[FullyQualifiedChannelName].Source.AdvancedSequencing.CreateAdvancedSequenceStep(true);

                    /* Set the Output Function to DC Current.  */
                    dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Output.Function = DCPowerSourceOutputFunction.DCCurrent;

                    /* Configure the Current Level. */
                    dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Current.CurrentLevel = currentLevelsSequence[i];
                }

                /* Initiate the device. */
                dcPowerSession.Control.Initiate();

                /* Wait for the sequence engine to be done. */
                PrecisionTimeSpan timeout = new PrecisionTimeSpan(10.0);
                dcPowerSession.Outputs[FullyQualifiedChannelName].Events.SequenceEngineDoneEvent.WaitForEvent(timeout);

                /* Fetch the arrays of measured voltages and currents from the
                   device. */
                DCPowerFetchResult result = dcPowerSession.Measurement.Fetch(FullyQualifiedChannelName, timeout, NumberOfPoints);

                DisplayMeasurements(result);

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

        void InitializeDCPowerSession()
        {
            dcPowerSession = new NIDCPower(FullyQualifiedChannelName, false, String.Empty);
            dcPowerSession.DriverOperation.Warning += new EventHandler<DCPowerWarningEventArgs>(DCPowerDriverOperationWarning);
        }

        void DCPowerDriverOperationWarning(object sender, DCPowerWarningEventArgs e)
        {
            MessageBox.Show(e.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        void mainForm_Closing(object sender, FormClosingEventArgs e)
        {
            CloseSession();
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
            configurationGroupBox.Enabled = isEnabled;
            startButton.Enabled = isEnabled;
            resourceNameComboBox.Select();
            this.Refresh();
        }

    }
}