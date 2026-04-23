//=============================================================================================================
//
// Title:
//      NI-DCPower Pulse Voltage Sequence
//
// Description:
//      Demonstrates how to use the DCPower Pulse API to generate a Voltage Pulse
//      Sequence. This example initializes a session, configures the Source Mode
//      and the Output Function, configures the pulse parameters, initiates the
//      pulse output sequence, and takes measurements.
//
// Suggested Devices:
//      NI PXIe-4135, NI PXIe-4136, NI PXIe-4137, NI PXIe-4138, NI PXIe-4139
//
//=============================================================================================================

using System;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIDCPower;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.PulseVoltageSequence
{
    public partial class MainForm : Form
    {
        NIDCPower dcPowerSession;
        private const int NumberOfStepsInVoltageSequence = 3;
        private const int NumberOfStepsInSourceDelay = 3;
        private const int NumberOfStepsInVoltageMeasurements = 3;
        private const int NumberOfStepsInCurrentMeasurements = 3;
        public MainForm()
        {
            InitializeComponent();
            LoadDCPowerDeviceNames();
            ConfigurePulseVoltageSeqDataGridView();
            ConfigureSourceDelaysDataGridView();
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

        void ConfigurePulseVoltageSeqDataGridView()
        {
            for (int i = 0; i < NumberOfStepsInVoltageSequence; i++)
            {
                pulseVoltageSequenceDataGridView.Rows.Add(
                    (i + 1).ToString(),
                    i.ToString("E"),
                    i.ToString("E"));
            }
            pulseVoltageSequenceDataGridView.CellEnter += new DataGridViewCellEventHandler(AllowSingleClickEditInDataGridView);
        }

        void ConfigureSourceDelaysDataGridView()
        {
            for (int i = 0; i < NumberOfStepsInSourceDelay; i++)
            {
                sourceDelaysDataGridView.Rows.Add(
                    ((i + 5) / 100000.0).ToString(),
                    i.ToString("E"),
                    i.ToString("E"));
            }
            sourceDelaysDataGridView.CellEnter += new DataGridViewCellEventHandler(AllowSingleClickEditInDataGridView);
        }

        void ConfigureVoltageMeasurmentsDataGridView()
        {
            for (int i = 0; i < NumberOfStepsInVoltageMeasurements; i++)
            {
                voltageMeasurmentsDataGridView.Rows.Add(
                    (i + 1).ToString(),
                    i.ToString("E"),
                    i.ToString("E"));
            }
            voltageMeasurmentsDataGridView.CellEnter += new DataGridViewCellEventHandler(AllowSingleClickEditInDataGridView);
        }

        void ConfigureCurrentMeasurmentsDataGridView()
        {
            for (int i = 0; i < NumberOfStepsInCurrentMeasurements; i++)
            {
                currentMeasurementsDataGridView.Rows.Add(
                    (i + 1).ToString(),
                    i.ToString("E"),
                    i.ToString("E"));
            }
            currentMeasurementsDataGridView.CellEnter += new DataGridViewCellEventHandler(AllowSingleClickEditInDataGridView);
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

        double PulseVoltageLevelRange
        {
            get
            {
                return decimal.ToDouble(this.pulseVoltLevelRangeNumeric.Value);
            }
        }

        double BiasVoltageLevel
        {
            get
            {
                return decimal.ToDouble(this.biasVoltageLevelNumeric.Value);
            }
        }

        double ApertureTime
        {
            get
            {
                return decimal.ToDouble(this.apertureTimeNumeric.Value);
            }
        }

        double PulseCurrentLimitRange
        {
            get
            {
                return decimal.ToDouble(this.pulseCurLimitRangeNumeric.Value);
            }
        }

        double PulseCurrentLimit
        {
            get
            {
                return decimal.ToDouble(this.pulseCurrentLimitNumeric.Value);
            }
        }

        double PulseBiasCurrentLimit
        {
            get
            {
                return decimal.ToDouble(this.biasCurrentLimitNumeric.Value);
            }
        }

        double PulseOnTime
        {
            get
            {
                return decimal.ToDouble(this.pulseOnTimeNumeric.Value);
            }
        }

        double PulseOffTime
        {
            get
            {
                return decimal.ToDouble(this.pulseOffTimeNumeric.Value);
            }
        }

        PrecisionTimeSpan PulseBiasDelay
        {
            get
            {
                return new PrecisionTimeSpan(decimal.ToDouble(this.pulseBiasDelayNumeric.Value));
            }
        }

        double[] PulseVoltageSequence
        {
            get
            {
                double[] sequence = new double[NumberOfStepsInVoltageSequence];
                for (int i = 0; i < NumberOfStepsInVoltageSequence; i++)
                {
                    sequence[i] = Double.Parse(pulseVoltageSequenceDataGridView.Rows[i].Cells[0].Value.ToString());
                }
                return sequence;
            }
        }

        PrecisionTimeSpan[] SourceDelay
        {
            get
            {
                PrecisionTimeSpan[] sequence = new PrecisionTimeSpan[NumberOfStepsInSourceDelay];
                for (int i = 0; i < NumberOfStepsInSourceDelay; i++)
                {
                    sequence[i] =new PrecisionTimeSpan(Double.Parse(sourceDelaysDataGridView.Rows[i].Cells[0].Value.ToString()));
                }
                return sequence;
            }
        }

        #endregion

        private void startButton_Click(object sender, System.EventArgs e)
        {
            ChangeControlState(false);
            Start();
            ChangeControlState(true);
        }

        void mainForm_Closing(object sender, FormClosingEventArgs e)
        {
            CloseSession();
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

        void Start()
        {
            try
            {
                InitializeDCPowerSession(false);

                dcPowerSession.Source.Mode = DCPowerSourceMode.Sequence;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Output.Function = DCPowerSourceOutputFunction.PulseVoltage;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.SetSequence(PulseVoltageSequence, SourceDelay);
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.PulseVoltage.VoltageLevelRange = PulseVoltageLevelRange;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.PulseVoltage.BiasVoltageLevel = BiasVoltageLevel;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.PulseVoltage.CurrentLimit = PulseCurrentLimit;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.PulseVoltage.CurrentLimitRange = PulseCurrentLimitRange;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.PulseVoltage.BiasCurrentLimit = PulseBiasCurrentLimit;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.PulseOnTime = PulseOnTime;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.PulseOffTime = PulseOffTime;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.PulseBiasDelay = PulseBiasDelay;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Measurement.ConfigureApertureTime(ApertureTime, DCPowerMeasureApertureTimeUnits.Seconds);

                dcPowerSession.Control.Initiate();
                DCPowerFetchResult result = dcPowerSession.Measurement.Fetch(FullyQualifiedChannelName,new PrecisionTimeSpan(10), NumberOfStepsInVoltageSequence);
                UpdateDataGridView(this.voltageMeasurmentsDataGridView, this.currentMeasurementsDataGridView, result);
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

        private static void ShowError(Exception ex)
        {
            MessageBox.Show(ex.Message, ex.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        void ChangeControlState(bool isEnabled)
        {
            resourceAndChannelNameGroupBox.Enabled = isEnabled;
            configurationGroupBox.Enabled = isEnabled;
            inputSequenceGroupBox.Enabled = isEnabled;
            startButton.Enabled = isEnabled;
            resourceNameComboBox.Select();
            this.Refresh();
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

        static void UpdateDataGridView(DataGridView voltageTable, DataGridView currentTable, DCPowerFetchResult newResult)
        {
            voltageTable.Rows.Add(newResult.VoltageMeasurements.Length);
            currentTable.Rows.Add(newResult.VoltageMeasurements.Length);
            for (int i = 0; i < newResult.VoltageMeasurements.Length; i++)
            {
                voltageTable.Rows[i].Cells[0].Value = newResult.VoltageMeasurements[i].ToString("E");
                currentTable.Rows[i].Cells[0].Value = newResult.CurrentMeasurements[i].ToString("E");
            }
        }
    }
}