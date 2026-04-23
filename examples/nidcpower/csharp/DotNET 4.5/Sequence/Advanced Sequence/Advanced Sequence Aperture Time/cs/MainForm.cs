//=============================================================================================================
//
// Title:
//      NI-DCPower Advanced Sequence Aperture Time
//
// Description:
//      Demonstrates how to use the Advanced Sequence functions to output a
// DC Voltage with different aperture times.  This example initializes a
// session, configures the Output Function, Source Delay, creates an advanced
// sequence with multiple aperture times, initiates generation, waits for a
// specified delay, and then measures the voltage and current output.
// This example uses Advanced Sequence mode.
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

namespace NationalInstruments.Examples.AdvancedSequenceApertureTime
{
    public partial class MainForm : Form
    {
        const int ApertureSequenceSize = 3;

        NIDCPower dcPowerSession;

        public MainForm()
        {
            InitializeComponent();
            LoadDCPowerDeviceNames();
            ConfigureApertureTimesDataGridView();
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

        void ConfigureApertureTimesDataGridView()
        {
            apertureTimesDataGridView.Rows.Add((16.00e-3).ToString("E"));
            apertureTimesDataGridView.Rows.Add((100.00e-3).ToString("E"));
            apertureTimesDataGridView.Rows.Add((100.00e-6).ToString("E"));
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

        double VoltageLevel
        {
            get
            {
                return Convert.ToDouble(voltageLevelNumeric.Value);
            }
        }

        PrecisionTimeSpan SourceDelay
        {
            get
            {
                return new PrecisionTimeSpan(Convert.ToDouble(this.sourceDelayNumeric.Value));
            }
        }

        double[] ApertureTime
        {
            get
            {
                double[] sequence = new double[ApertureSequenceSize];
                for (int i = 0; i < ApertureSequenceSize; i++)
                {
                    sequence[i] = Double.Parse(apertureTimesDataGridView.Rows[i].Cells[0].Value.ToString());
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

        void Start()
        {
            /* Specify the Advanced Sequence Attributes which can change per step. */
            DCPowerAdvancedSequenceProperty[] advancedSequenceProperties = {DCPowerAdvancedSequenceProperty.VoltageLevel, DCPowerAdvancedSequenceProperty.OutputFunction};
            try
            {

                InitializeDCPowerSession();

                /* Configure the Source mode to Sequence. */
                dcPowerSession.Source.Mode = DCPowerSourceMode.Sequence;

                /* Set the Output Function to DC Voltage.  If you change the Output
                Function to DC Current, you must use Current Level and Voltage Limit
                instead of Voltage Level and Current Limit. */
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Output.Function = DCPowerSourceOutputFunction.DCVoltage;

                /* Configure the source delay.  This is the amount of time the device
                waits after programming the output. The source operation is complete
                after this delay. */
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.SourceDelay = SourceDelay;

                /* Create the Advanced Sequence. */
                string advancedSequenceName = "MySeq";
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.AdvancedSequencing.CreateAdvancedSequence(advancedSequenceName, advancedSequenceProperties, true);

                /* Create new Advanced Sequence Step for every Advanced Sequence Attribute. */
                double voltageLevel = VoltageLevel;
                for (int i = 0; i < ApertureSequenceSize; i++)
                {
                    dcPowerSession.Outputs[FullyQualifiedChannelName].Source.AdvancedSequencing.CreateAdvancedSequenceStep(true);

                    /* Configure the Voltage Level. */
                    dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Voltage.VoltageLevel = voltageLevel++;

                    /* Configure the Aperture Time. */
                    dcPowerSession.Outputs[FullyQualifiedChannelName].Measurement.ApertureTime = ApertureTime[i];
                }

                /* Initiate the device to start generation and acquisition. */
                dcPowerSession.Control.Initiate();

                /* Wait for Sequence Engine Done event. */
                PrecisionTimeSpan timeout = new PrecisionTimeSpan(10.0);
                dcPowerSession.Outputs[FullyQualifiedChannelName].Events.SequenceEngineDoneEvent.WaitForEvent(timeout);

                /* Measure voltage. */
                DCPowerFetchResult result = dcPowerSession.Measurement.Fetch(FullyQualifiedChannelName, timeout, ApertureSequenceSize);

                DisplayMeasurements(result);

                /* Abort the session. */
                dcPowerSession.Control.Abort();

                /* Delete the Advanced Sequence. */
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.AdvancedSequencing.DeleteAdvancedSequence(advancedSequenceName);

                /* Reset to disable the output. */
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

        void DisplayMeasurements(DCPowerFetchResult result)
        {
            for (int i = 0; i < result.VoltageMeasurements.Length; i++)
            {
                voltageMeasurmentsDataGridView.Rows.Add(
                    result.VoltageMeasurements[i].ToString("E"));
            }
        }

        void mainForm_Closing(object sender, FormClosingEventArgs e)
        {
            CloseSession();
        }

        void ClearMeasurementsDataGridView()
        {
            voltageMeasurmentsDataGridView.Rows.Clear();
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