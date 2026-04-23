//==================================================================================================
// Title  : Controlling An Individual Relay
// Description  : Use this example to learn how to control individual relay of a switch device .The example shows how to open and close a relay of a NI-Switch device. 
//==================================================================================================
using System;
using System.Reflection;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NISwitch;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.ControllingAnIndividualRelay
{
    public partial class MainForm : Form
    {
        NISwitch switchSession;
        PrecisionTimeSpan maximumTime = new PrecisionTimeSpan(5);

        public MainForm()
        {
            InitializeComponent();
            LoadSwitchDeviceNames();
            LoadTopology();
        }

        #region UI Initial Value Config Section

        private void LoadTopology()
        {
            Type myType = typeof(SwitchDeviceTopology);
            PropertyInfo[] properties = myType.GetProperties();

            foreach (PropertyInfo prop in properties)
            {
                topologyNameComboBox.Items.Add(prop.GetValue(myType, null).ToString());
            }
            topologyNameComboBox.SelectedIndex = 0;
        }

        private void LoadSwitchDeviceNames()
        {
            ModularInstrumentsSystem modularInstrumentsSystem = new ModularInstrumentsSystem("NI-SWITCH");
            foreach (DeviceInfo device in modularInstrumentsSystem.DeviceCollection)
            {
                resourceNameComboBox.Items.Add(device.Name);
            }
            if (modularInstrumentsSystem.DeviceCollection.Count > 0)
            {
                resourceNameComboBox.SelectedIndex = 0;
            }
        }

        #endregion

        #region Program Properties

        private string ResourceName
        {
            get
            {
                return this.resourceNameComboBox.Text;
            }
        }

        private string TopologyName
        {
            get
            {
                return this.topologyNameComboBox.SelectedItem.ToString();
            }
        }

        private string RelayName
        {
            get
            {
                return this.relayNameTextBox.Text;
            }
        }

        #endregion

        #region FormEvents

        private void openRelayButton_Click(object sender, EventArgs e)
        {
            ChangeControlState(false);
            try
            {
                ChangeRelayPosition(SwitchRelayAction.OpenRelay);
            }
            catch (System.Exception ex)
            {
                ShowError(ex.Message);
            }
            finally
            {
                //Close session to switch module.
                CloseSession();
                ChangeControlState(true);
            }
        }

        private void ChangeRelayPosition(SwitchRelayAction position)
        {
            CloseSession();
            //Open session to the switch module and sets topology
            InitializeSwitchSession();
            // Open the relay.
            switchSession.RelayOperations.RelayControl(RelayName, position);
            // Wait for the relay to activate and debounce.
            switchSession.Path.WaitForDebounce(maximumTime);
        }

        private void closeRelayButton_Click(object sender, EventArgs e)
        {
            ChangeControlState(false);
            try
            {
                ChangeRelayPosition(SwitchRelayAction.CloseRelay);
            }
            catch (System.Exception ex)
            {
                ShowError(ex.Message);
            }
            finally
            {
                //Close session to switch module.
                CloseSession();
                ChangeControlState(true);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseSession();
        }
        #endregion

        #region Program Functions

        private void ChangeControlState(bool isEnabled)
        {
            this.openRelayButton.Enabled = isEnabled;
            this.closeRelayButton.Enabled = isEnabled;
            this.resourceNameComboBox.Enabled = isEnabled;
            this.topologyNameComboBox.Enabled = isEnabled;
            this.relayNameTextBox.Enabled = isEnabled;
        }

        private static void ShowError(string message)
        {
            if (string.IsNullOrEmpty(message))
                message = "Unexpected Error";
            MessageBox.Show(message, "Error"); ;
        }

        private void InitializeSwitchSession()
        {
            switchSession = new NISwitch(ResourceName, TopologyName, false, true);
            switchSession.DriverOperation.Warning += new System.EventHandler<SwitchWarningEventArgs>(DriverOperationWarning);
        }

        private void DriverOperationWarning(object sender, SwitchWarningEventArgs e)
        {
            MessageBox.Show(e.ToString(), "Warning");
        }

        private void CloseSession()
        {
            if (switchSession != null)
            {
                try
                {
                    switchSession.Close();
                    switchSession = null;
                }
                catch (System.Exception ex)
                {
                    ShowError("Unable to Close Session, Reset the device.\n" + "Error : " + ex.Message);
                    Application.Exit();
                }
            }
        }

        #endregion
    }
}
