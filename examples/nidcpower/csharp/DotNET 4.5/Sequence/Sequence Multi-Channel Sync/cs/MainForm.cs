//=============================================================================================================
//
// Title:
//      NI-DCPower Sequence Multi-Channel Synchronization
//
// Description:
//      This example demonstrates how to use triggers and events to synchronize multiple
//      channels in Sequence source mode.  Use this example to sequence multiple channels
//      in lock-step.
//
//      Note: In this example the Output Function is set to DC Voltage. If you change the
//      Output Function to DC Current, you must use Current Level and Voltage Limit instead
//      of Voltage Level and Current Limit.
//
// Suggested Devices:
//      PXI-4132
//      PXIe-4112, PXIe-4113, PXIe-4135, PXIe-4136, PXIe-4137, PXIe-4138, PXIe-4139, PXIe-4140, PXIe-4141, PXIe-4142, PXIe-4143, PXIe-4144, PXIe-4145, PXIe-4147, PXIe-4154, PXIe-4162, PXIe-4163
//
//=============================================================================================================

using System;
using System.Linq;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIDCPower;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.SequenceMultiChannelSync
{
    public partial class MainForm : Form
    {
        const int NumberOfSlaveDevices = 2;
        const int NumberOfStepsInMasterSequence = 3;
        const int NumberOfStepsInSlaveSequence = 3;

        NIDCPower Session;

        public MainForm()
        {
            InitializeComponent();
            ConfigureMasterSequenceDataGridView();
            ConfigureMasterMeasurementDataGridView();
            ConfigureSlavesConfigurationDataGridView();
            ConfigureSlavesSequenceDataGridView();
            ConfigureSlave0MeasurementsDataGridView();
            ConfigureSlave1MeasurementsDataGridView();
        }

        #region MainForm initial configuration
        void ConfigureMasterSequenceDataGridView()
        {
            for (int i = 0; i < NumberOfStepsInMasterSequence; i++)
            {
                masterSequenceDataGridView.Rows.Add(
                    (i + 1).ToString(),
                    i.ToString("E"));
            }
            masterSequenceDataGridView.CellEnter += new DataGridViewCellEventHandler(AllowSingleClickEditInDataGridView);
        }

        void ConfigureMasterMeasurementDataGridView()
        {
            for (int i = 0; i < NumberOfStepsInMasterSequence; i++)
            {
                masterMeasurementsDataGridView.Rows.Add(
                    (i + 1).ToString(),
                    0.ToString("E"),
                    0.ToString("E"));
            }
        }

        void ConfigureSlavesSequenceDataGridView()
        {
            for (int i = 0; i < NumberOfStepsInSlaveSequence; i++)
            {
                slavesSequenceDataGridView.Rows.Add(
                    (i + 1).ToString(),
                    i.ToString("E"),
                    i.ToString("E"));
            }
            slavesSequenceDataGridView.CellEnter += new DataGridViewCellEventHandler(AllowSingleClickEditInDataGridView);
        }

        void ConfigureSlavesConfigurationDataGridView()
        {
            LoadDCPowerDeviceNames();
            for (int i = 0; i < NumberOfSlaveDevices; i++)
            {
                slavesConfigurationDataGridView.Rows.Add(
                    String.Format("Slave {0}", i),
                    masterConfigurationResourceNameComboBox.Text,
                    "0",
                    (0.02).ToString("E"));
            }
            slavesConfigurationDataGridView.CellEnter += new DataGridViewCellEventHandler(AllowSingleClickEditInDataGridView);
        }

        void ConfigureSlave0MeasurementsDataGridView()
        {
            for (int i = 0; i < NumberOfStepsInSlaveSequence; i++)
            {
                slave0MeasurementsDataGridView.Rows.Add(
                    (i + 1).ToString(),
                    0.ToString("E"),
                    0.ToString("E"));
            }
        }

        void ConfigureSlave1MeasurementsDataGridView()
        {
            for (int i = 0; i < NumberOfStepsInSlaveSequence; i++)
            {
                slave1MeasurementsDataGridView.Rows.Add(
                    (i + 1).ToString(),
                    0.ToString("E"),
                    0.ToString("E"));
            }
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
                    masterConfigurationResourceNameComboBox.Items.Add(device.Name);
                    ((DataGridViewComboBoxColumn)(slavesConfigurationDataGridView.Columns["resourceNameColumn"])).Items.Add(device.Name);
                }
            }
            if (masterConfigurationResourceNameComboBox.Items.Count > 0)
            {
                masterConfigurationResourceNameComboBox.SelectedIndex = 0;
            }
        }
        #endregion

        #region MainForm configuration values
        string MasterResourceName
        {
            get
            {
                return this.masterConfigurationResourceNameComboBox.Text;
            }
        }

        string MasterChannelName
        {
            get
            {
                return this.masterConfigurationChannelNameTextBox.Text;
            }
        }

        string MasterFullyQualifiedChannelName
        {
            get
            {
                return $"{MasterResourceName}/{MasterChannelName}";
            }
        }

        double MasterCurrentLimit
        {
            get
            {
                return decimal.ToDouble(this.masterConfigurationCurrentLimitNumeric.Value);
            }
        }

        PrecisionTimeSpan MasterSourceDelay
        {
            get
            {
                return new PrecisionTimeSpan(decimal.ToDouble(this.masterSourceDelayNumeric.Value));
            }
        }

        PrecisionTimeSpan MasterMeasureCompleteEventDelay
        {
            get
            {
                return new PrecisionTimeSpan(decimal.ToDouble(this.masterMeasureCompleteEventDelayNumeric.Value));
            }
        }

        double[] MasterSequence
        {
            get
            {
                double[] sequence = new double[NumberOfStepsInMasterSequence];
                for (int i = 0; i < NumberOfStepsInMasterSequence; i++)
                {
                    sequence[i] = Double.Parse(masterSequenceDataGridView.Rows[i].Cells[1].Value.ToString());
                }
                return sequence;
            }
        }

        class SlaveConfiguration
        {
            int _index;
            MainForm _thisForm;

            internal SlaveConfiguration(MainForm form, int index)
            {
                _index = index;
                _thisForm = form;
            }

            internal string ResourceName
            {
                get
                {
                    return _thisForm.slavesConfigurationDataGridView.Rows[_index].Cells[1].Value.ToString();
                }
            }

            internal string ChannelName
            {
                get
                {
                    return _thisForm.slavesConfigurationDataGridView.Rows[_index].Cells[2].Value.ToString();
                }
            }

            internal string FullyQualifiedChannelName
            {
                get
                {
                    return $"{ResourceName}/{ChannelName}";
                }
            }

            internal double CurrentLimit
            {
                get
                {
                    return Double.Parse(_thisForm.slavesConfigurationDataGridView.Rows[_index].Cells[3].Value.ToString());
                }
            }

            internal double[] Sequence
            {
                get
                {
                    double[] sequence = new double[NumberOfStepsInSlaveSequence];
                    for (int i = 0; i < NumberOfStepsInSlaveSequence; i++)
                    {
                        sequence[i] = Double.Parse(_thisForm.slavesSequenceDataGridView.Rows[i].Cells[_index + 1].Value.ToString());
                    }
                    return sequence;
                }
            }
        }

        SlaveConfiguration[] _slave;
        SlaveConfiguration[] Slave
        {
            get
            {
                if (_slave == null)
                {
                    _slave = new SlaveConfiguration[NumberOfSlaveDevices];
                    for (int i = 0; i < NumberOfSlaveDevices; i++)
                    {
                        _slave[i] = new SlaveConfiguration(this, i);
                    }
                }
                return _slave;
            }
        }
        #endregion

        void startButton_Click(object sender, System.EventArgs e)
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
                InitializeDCPowerSessions();

                #region Configure master device.
                Session.Outputs[MasterFullyQualifiedChannelName].Source.Mode = DCPowerSourceMode.Sequence;
                Session.Outputs[MasterFullyQualifiedChannelName].Source.Output.Function = DCPowerSourceOutputFunction.DCVoltage;
                Session.Outputs[MasterFullyQualifiedChannelName].Source.SetSequence(MasterSequence);
                Session.Outputs[MasterFullyQualifiedChannelName].Source.Voltage.CurrentLimit = MasterCurrentLimit;

                // Configure the Source Delay on the master device. This delay has to be long enough for
                // all the devices to finish programming the output and to settle.
                Session.Outputs[MasterFullyQualifiedChannelName].Source.SourceDelay = MasterSourceDelay;

                // Configure the Source trigger. On the master device, the Source trigger is
                // disabled (None). This means that the device will source without waiting for
                // a trigger.  When the device starts sourcing, it will export the Source trigger.
                Session.Outputs[MasterFullyQualifiedChannelName].Triggers.SourceTrigger.Disable();

                // Configure when to take measurements.  On the master device, take a measurement
                // as soon as the source unit completes (including any source delay).

                Session.Outputs[MasterFullyQualifiedChannelName].Measurement.MeasureWhen = DCPowerMeasurementWhen.AutomaticallyAfterSourceComplete;

                // Configure the Measure Complete Event Delay on the master device. The master device has
                // to delay generating the Measure Complete event (which triggers the next step of the master's
                // sequence) until all slave device(s) have completed taking measurements. The amount of
                // time a slave takes to measure can vary based on its configuration and model.
                Session.Outputs[MasterFullyQualifiedChannelName].Events.MeasureCompleteEvent.Delay = MasterMeasureCompleteEventDelay;

                Session.Outputs[MasterFullyQualifiedChannelName].Control.Commit();
                #endregion

                #region Configure the slave devices
                string sourceTriggerInputTerminal = BuildFullyQualifiedTerminalName(Session, MasterResourceName, MasterChannelName, "SourceTrigger");
                string measureTriggerInputTerminal = BuildFullyQualifiedTerminalName(Session, MasterResourceName, MasterChannelName, "SourceCompleteEvent");

                var slaveFullyQualifiedResourceNames = String.Join(",", Slave.Select(s => s.FullyQualifiedChannelName));
                Session.Outputs[slaveFullyQualifiedResourceNames].Source.Mode = DCPowerSourceMode.Sequence;
                Session.Outputs[slaveFullyQualifiedResourceNames].Source.Output.Function = DCPowerSourceOutputFunction.DCVoltage;

                for (int i = 0; i < NumberOfSlaveDevices; i++)
                {
                    Session.Outputs[Slave[i].FullyQualifiedChannelName].Source.SetSequence(Slave[i].Sequence);
                    Session.Outputs[Slave[i].FullyQualifiedChannelName].Source.Voltage.CurrentLimit = Slave[i].CurrentLimit;
                }

                // Configure the Source Delay.  On the slave device(s), set the delay to 0, so that the slave(s)
                // are ready to receive the next trigger from the master as quickly as possible.
                Session.Outputs[slaveFullyQualifiedResourceNames].Source.SourceDelay = new PrecisionTimeSpan(0.00003);

                // Configure the Source trigger.  On the slave device(s), the source  trigger is the exported
                // Source trigger from the master device.
                Session.Outputs[slaveFullyQualifiedResourceNames].Triggers.SourceTrigger.DigitalEdge.Configure(sourceTriggerInputTerminal, DCPowerTriggerEdge.Rising);

                // On the slave device(s), take a measurement when the source unit on the master device completes.
                // This is accomplished by setting Measure When to On Measure Trigger and by setting the input
                // terminal of the Measure Trigger to be the master device's Source Complete event.
                Session.Outputs[slaveFullyQualifiedResourceNames].Measurement.MeasureWhen = DCPowerMeasurementWhen.OnMeasureTrigger;
                Session.Outputs[slaveFullyQualifiedResourceNames].Triggers.MeasureTrigger.DigitalEdge.Configure(measureTriggerInputTerminal, DCPowerTriggerEdge.Rising);

                Session.Outputs[slaveFullyQualifiedResourceNames].Control.Commit();

                // Initiate the slave device(s) and then initiate the master device to start generation
                // and acquisition. The order here is significant.

                // Initiate the slave device(s). The slave device(s) will
                // be waiting for the Source Trigger.
                Session.Outputs[slaveFullyQualifiedResourceNames].Control.Initiate();

                #endregion

                // Initiate the master device. The master device has to be initiated after the slave device(s)
                // so that when it exports the Source trigger the slave device(s) are already waiting for
                // the Source trigger.
                Session.Outputs[MasterFullyQualifiedChannelName].Control.Initiate();

                // Fetch measurements from master device and update Mainform with the new values.

                DCPowerFetchResult result = Session.Measurement.Fetch(MasterFullyQualifiedChannelName, new PrecisionTimeSpan(1), NumberOfStepsInMasterSequence);//Error 4154

                UpdateDataGridView(this.masterMeasurementsDataGridView, result);

                // Fetch measurements from slave device(s) and update MainForm with the new values.

                result = Session.Measurement.Fetch(Slave[0].FullyQualifiedChannelName, new PrecisionTimeSpan(10), NumberOfStepsInSlaveSequence);
                UpdateDataGridView(this.slave0MeasurementsDataGridView, result);
                result = Session.Measurement.Fetch(Slave[1].FullyQualifiedChannelName, new PrecisionTimeSpan(10), NumberOfStepsInSlaveSequence);
                UpdateDataGridView(this.slave1MeasurementsDataGridView, result);

                // Reset to disable the output.
                Session.Utility.Reset();

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

        void InitializeDCPowerSessions()
        {
            var slaveFullyQualifiedResourceNames = String.Join(",", Slave.Select(s => s.FullyQualifiedChannelName));
            string fullyQualifiedResourceNames = String.Join(",", MasterFullyQualifiedChannelName, slaveFullyQualifiedResourceNames); 
            Session = new NIDCPower(fullyQualifiedResourceNames, false, String.Empty);
            Session.DriverOperation.Warning += new EventHandler<DCPowerWarningEventArgs>(DCPowerDriverOperationWarning);
        }

        void DCPowerDriverOperationWarning(object sender, DCPowerWarningEventArgs e)
        {
            MessageBox.Show(e.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        void CloseSession()
        {
            try
            {
                if (Session != null)
                {
                    Session.Close();
                    Session = null;
                }
            }
            catch (Exception ex)
            {
                ShowError(ex);
                Application.Exit();
            }
        }

        static void UpdateDataGridView(DataGridView table, DCPowerFetchResult newResult)
        {
            for (int i = 0; i < newResult.VoltageMeasurements.Length; i++)
            {
                table.Rows[i].Cells[1].Value = newResult.VoltageMeasurements[i].ToString("E");
                table.Rows[i].Cells[2].Value = newResult.CurrentMeasurements[i].ToString("E");
            }
        }

        static void ShowError(Exception ex)
        {
            MessageBox.Show(ex.Message, ex.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        void ChangeControlState(bool isEnabled)
        {
            this.masterConfigurationGroupBox.Enabled = isEnabled;
            this.slavesConfigurationGroupBox.Enabled = isEnabled;
            this.slavesSequenceDataGridView.Enabled = isEnabled;
            this.startButton.Enabled = isEnabled;
            this.masterConfigurationResourceNameComboBox.Select();
            this.Refresh();
        }

        static string BuildFullyQualifiedTerminalName(NIDCPower session, string resourceName, string channelName, string localTerminalName)
        {
            string modelName = session.Instruments[resourceName].Identity.InstrumentModel;
            if (modelName.Equals("NI PXI-4132", StringComparison.OrdinalIgnoreCase))
            {
                return String.Format("/{0}/{1}", resourceName, localTerminalName);
            }
            else
            {
                return String.Format("/{0}/Engine{1}/{2}", resourceName, channelName, localTerminalName);
            }
        }
    }
}