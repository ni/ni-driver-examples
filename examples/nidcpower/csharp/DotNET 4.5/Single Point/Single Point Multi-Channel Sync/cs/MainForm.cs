//=============================================================================================================
//
// Title:
//      NI-DCPower Single Point Multi-Channel Synchronization
//
// Description:
//      Demonstrates how to use triggers and events to synchronize multiple channels in
//      Single Point source mode.
//
//      Note: In this example the Output Function is set to DC Voltage. If you change the
//      Output Function to DC Current, you must use Current Level and Voltage Limit instead
//      of Voltage Level and Current Limit.
//
// Suggested Devices:
//      PXI 4132
//      PXIe-4112, PXIe-4113, PXIe-4135, PXIe-4136, PXIe-4137, PXIe-4138, PXIe-4139, PXIe-4140, PXIe-4141, PXIe-4142, PXIe-4143, PXIe-4144, PXIe-4145, PXIe-4147, PXIe-4154, PXIe-4162, PXIe-4163
//
//=============================================================================================================

using System;
using System.Linq;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIDCPower;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.SinglePointMultiChannelSync
{
    public partial class MainForm : Form
    {
        const int NumberOfSlaveDevices = 2;

        NIDCPower Session;

        public MainForm()
        {
            InitializeComponent();
            ConfigureSlavesConfigurationDataGridView();
            ConfigureSlavesMeasurementDataGridView();
        }

        #region MainForm initial configuration
        void ConfigureSlavesConfigurationDataGridView()
        {
            LoadDCPowerDeviceNames();
            for (int i = 0; i < NumberOfSlaveDevices; i++)
            {
                slavesConfigurationDataGridView.Rows.Add(
                    String.Format("Slave {0}", i),
                    masterConfigurationResourceNameComboBox.Text,
                    "0",
                    2.ToString("E"),
                    (0.02).ToString("E"));
            }
            slavesConfigurationDataGridView.CellEnter += new DataGridViewCellEventHandler(AllowSingleClickEditInDataGridView);
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

        void ConfigureSlavesMeasurementDataGridView()
        {
            for (int i = 0; i < NumberOfSlaveDevices; i++)
            {
                slavesMeasurementsDataGridView.Rows.Add(
                    String.Format("Slave {0}", i + 1),
                    0.ToString("E"),
                    0.ToString("E"));
            }
        }

        void LoadDCPowerDeviceNames()
        {
            using (ModularInstrumentsSystem dcPowerDevices = new ModularInstrumentsSystem("NI-DCPower"))
            {
                foreach (DeviceInfo device in dcPowerDevices.DeviceCollection)
                {
                    masterConfigurationResourceNameComboBox.Items.Add(device.Name);
                    ((DataGridViewComboBoxColumn)(slavesConfigurationDataGridView.Columns[1])).Items.Add(device.Name);
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

        double MasterVoltageLevel
        {
            get
            {
                return decimal.ToDouble(this.masterConfigurationVoltageLevelNumeric.Value);
            }
        }

        PrecisionTimeSpan MasterSourceDelay
        {
            get
            {
                return new PrecisionTimeSpan(decimal.ToDouble(this.masterConfigurationSourceDelayNumeric.Value));
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

            internal double VoltageLevel
            {
                get
                {
                    return Double.Parse(_thisForm.slavesConfigurationDataGridView.Rows[_index].Cells[3].Value.ToString());
                }
            }

            internal double CurrentLimit
            {
                get
                {
                    return Double.Parse(_thisForm.slavesConfigurationDataGridView.Rows[_index].Cells[4].Value.ToString());
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

                #region Configure the master device
                Session.Outputs[MasterFullyQualifiedChannelName].Source.Mode = DCPowerSourceMode.SinglePoint;
                Session.Outputs[MasterFullyQualifiedChannelName].Source.Output.Function = DCPowerSourceOutputFunction.DCVoltage;
                Session.Outputs[MasterFullyQualifiedChannelName].Source.Voltage.VoltageLevel = MasterVoltageLevel;
                Session.Outputs[MasterFullyQualifiedChannelName].Source.Voltage.CurrentLimit = MasterCurrentLimit;
                Session.Outputs[MasterFullyQualifiedChannelName].Source.SourceDelay = MasterSourceDelay;

                // Configure the Source trigger. On the master device, the Source trigger is
                // disabled (None). This means that the device will source without waiting for
                // a trigger.  When the device starts sourcing, it will export the Source trigger.
                Session.Outputs[MasterFullyQualifiedChannelName].Triggers.SourceTrigger.Disable();

                // Configure when to take measurements.  On the master device, take a measurement
                // as soon as the source unit completes (including any source delay).
                Session.Outputs[MasterFullyQualifiedChannelName].Measurement.MeasureWhen = DCPowerMeasurementWhen.AutomaticallyAfterSourceComplete;

                Session.Outputs[MasterFullyQualifiedChannelName].Control.Commit();
                #endregion

                #region Configure the slave devices
                string sourceTriggerInputTerminal = BuildFullyQualifiedTerminalName(Session, MasterResourceName, MasterChannelName, "SourceTrigger");
                string measureTriggerInputTerminal = BuildFullyQualifiedTerminalName(Session, MasterResourceName, MasterChannelName, "SourceCompleteEvent");

                var slaveFullyQualifiedResourceNames = String.Join(",", Slave.Select(s => s.FullyQualifiedChannelName));
                Session.Outputs[slaveFullyQualifiedResourceNames].Source.Mode = DCPowerSourceMode.SinglePoint;
                Session.Outputs[slaveFullyQualifiedResourceNames].Source.Output.Function = DCPowerSourceOutputFunction.DCVoltage;

                for (int i = 0; i < NumberOfSlaveDevices; i++)
                {
                    Session.Outputs[Slave[i].FullyQualifiedChannelName].Source.Voltage.VoltageLevel = Slave[i].VoltageLevel;
                    Session.Outputs[Slave[i].FullyQualifiedChannelName].Source.Voltage.CurrentLimit = Slave[i].CurrentLimit;
                }

                // Configure the Source Delay. On the slave device(s), set the delay to 30uS, so that the slave(s)
                // are ready to receive the next trigger from the master as quickly as possible.
                Session.Outputs[slaveFullyQualifiedResourceNames].Source.SourceDelay = new PrecisionTimeSpan(0.00003);

                // Configure the Source trigger.  On the slave device(s), the source trigger is the exported
                // Source trigger from the master device.
                Session.Outputs[slaveFullyQualifiedResourceNames].Triggers.SourceTrigger.DigitalEdge.Configure(sourceTriggerInputTerminal, DCPowerTriggerEdge.Rising);

                // Configure when to take measurements. On the slave device(s), take a measurement when the
                // source unit on the master device completes. This is accomplished by setting Measure When
                // to On Measure Trigger and by setting the input terminal of the Measure trigger to be the
                // master device's Source Complete event.
                Session.Outputs[slaveFullyQualifiedResourceNames].Measurement.MeasureWhen = DCPowerMeasurementWhen.AutomaticallyAfterSourceComplete;
                Session.Outputs[slaveFullyQualifiedResourceNames].Triggers.MeasureTrigger.DigitalEdge.Configure(measureTriggerInputTerminal, DCPowerTriggerEdge.Rising);

                Session.Outputs[slaveFullyQualifiedResourceNames].Control.Commit();

                // Initiate the slave device(s) and then initiate the master device to start generation
                // and acquisition. The order here is significant.

                // Initiate the slave device(s). The slave device(s) will be waiting for the Source Trigger.
                Session.Outputs[slaveFullyQualifiedResourceNames].Control.Initiate();

                #endregion

                // Initiate the master device. The master device has to be initiated after the slave device(s)
                // so that when it exports the Source trigger the slave device(s) are already waiting for
                // the Source trigger.
                Session.Outputs[MasterFullyQualifiedChannelName].Control.Initiate();

                // Fetch measurements from master device and update Mainform with the new values.
                DCPowerFetchResult result = Session.Measurement.Fetch(MasterFullyQualifiedChannelName, new PrecisionTimeSpan(1.0), 1);

                masterMeasurementsVoltageTextBox.Text = result.VoltageMeasurements[0].ToString("E");
                masterMeasurementsCurrentTextBox.Text = result.CurrentMeasurements[0].ToString("E");

                // Fetch measurements from slave device(s) and update Mainform with the new values.
                for (int i = 0; i < NumberOfSlaveDevices; i++)
                {
                    result = Session.Measurement.Fetch(Slave[i].FullyQualifiedChannelName, new PrecisionTimeSpan(10.0), 1);
                    slavesMeasurementsDataGridView.Rows[i].Cells[1].Value = result.VoltageMeasurements[0].ToString("E");
                    slavesMeasurementsDataGridView.Rows[i].Cells[2].Value = result.CurrentMeasurements[0].ToString("E");
                }

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

        static void ShowError(Exception ex)
        {
            MessageBox.Show(ex.Message, ex.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        void ChangeControlState(bool flag)
        {
            masterConfigurationGroupBox.Enabled = flag;
            slavesConfigurationGroupBox.Enabled = flag;
            startButton.Enabled = flag;
            masterConfigurationResourceNameComboBox.Select();
            this.Refresh();
        }
    }
}