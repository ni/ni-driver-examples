//=============================================================================================================
//
// Title:
//      NI-DCPower Pulse Current Sequence
//
// Description:
//      Demonstrates how to use the DCPower Pulse API to generate a Current Pulse
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

namespace NationalInstruments.Examples.PulseCurrentSequence
{
    public partial class MainForm : Form
    {
        NIDCPower dcPowerSession;
        private const int NumberOfStepsInCurrentSequence = 4;
        private const int NumberOfStepsInVoltageMeasurements = 10;
        private const int NumberOfStepsInSourceDelay = 4;
        private const int NumberOfStepsInCurrentMeasurements = 10;

        public MainForm()
        {
            InitializeComponent();
            LoadDCPowerDeviceNames();
            ConfigurePulseCurrentSeqDataGridView();
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

        void ConfigurePulseCurrentSeqDataGridView()
        {
            for (int i = 0; i < NumberOfStepsInCurrentSequence; i++)
            {
                pulseCurrentSequenceDataGridView.Rows.Add(
                    ((i + 1) / 10.0).ToString("E"));
            }
            pulseCurrentSequenceDataGridView.CellEnter += new DataGridViewCellEventHandler(AllowSingleClickEditInDataGridView);
        }

        void ConfigureSourceDelaysDataGridView()
        {
            for (int i = 0; i < NumberOfStepsInSourceDelay; i++)
            {
                sourceDelaysDataGridView.Rows.Add(
                    ((i + 5)/100000.0).ToString());
            }
            sourceDelaysDataGridView.CellEnter += new DataGridViewCellEventHandler(AllowSingleClickEditInDataGridView);
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

        double PulseCurrentLevelRange
        {
            get
            {
                return decimal.ToDouble(this.pulseCurrentLevelRangeNumeric.Value);
            }
        }

        double BiasCurrentLevel
        {
            get
            {
                return decimal.ToDouble(this.biasCurrentLevelNumeric.Value);
            }
        }

        double ApertureTime
        {
            get
            {
                return decimal.ToDouble(this.apertureTimeNumeric.Value);
            }
        }

        double PulseVoltageLimitRange
        {
            get
            {
                return decimal.ToDouble(this.pulseVoltageLimitRangeNumeric.Value);
            }
        }

        double PulseVoltageLimit
        {
            get
            {
                return decimal.ToDouble(this.pulseVoltageLimitNumeric.Value);
            }
        }

        double PulseBiasVoltageLimit
        {
            get
            {
                return decimal.ToDouble(this.biasVoltageLimitNumeric.Value);
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

        double[] PulseCurrentSequence
        {
            get
            {
                double[] sequence = new double[NumberOfStepsInCurrentSequence];
                for (int i = 0; i < NumberOfStepsInCurrentSequence; i++)
                {
                    sequence[i] = Double.Parse(pulseCurrentSequenceDataGridView.Rows[i].Cells[0].Value.ToString());
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
                    sequence[i] = new PrecisionTimeSpan(Double.Parse(sourceDelaysDataGridView.Rows[i].Cells[0].Value.ToString()));
                }
                return sequence;
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
                // Configure Session Parameters from UI.
                dcPowerSession.Source.Mode = DCPowerSourceMode.Sequence;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Output.Function = DCPowerSourceOutputFunction.PulseCurrent;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.SetSequence(PulseCurrentSequence, SourceDelay);
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.PulseCurrent.CurrentLevelRange = PulseCurrentLevelRange;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.PulseCurrent.BiasCurrentLevel = BiasCurrentLevel;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.PulseCurrent.VoltageLimit = PulseVoltageLimit;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.PulseCurrent.VoltageLimitRange = PulseVoltageLimitRange;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.PulseCurrent.BiasVoltageLimit = PulseBiasVoltageLimit;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.PulseOnTime = PulseOnTime;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.PulseOffTime = PulseOffTime;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.PulseBiasDelay = PulseBiasDelay;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Measurement.ConfigureApertureTime(ApertureTime, DCPowerMeasureApertureTimeUnits.Seconds);

                // Initiate the device to start generation and acquisition.
                dcPowerSession.Control.Initiate();

                // Fetch voltage and current.
                DCPowerFetchResult result = dcPowerSession.Measurement.Fetch(FullyQualifiedChannelName,new PrecisionTimeSpan(10.00), NumberOfStepsInCurrentSequence);
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
            inputSequenceGroupBox.Enabled = isEnabled;
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