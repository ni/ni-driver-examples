//==================================================================================================
// Title        : Making Connections On A Switch
// Description  : Use this example to learn how to connect and disconnect channels on a NI-Switch device.
//==================================================================================================
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using NationalInstruments;
using NationalInstruments.ModularInstruments.NISwitch;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.MakingConnectionsOnASwitch
{
    public partial class MainForm : Form
    {
        NISwitch switchSession;
        PrecisionTimeSpan maxTime = new PrecisionTimeSpan(5);
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
        #endregion UI Initial Value Config Section
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
        private string Channel1
        {
            get
            {
                return this.channel1TextBox.Text;
            }
        }
        private string Channel2
        {
            get
            {
                return this.channel2TextBox.Text;
            }
        }
        private string Channel3
        {
            get
            {
                return this.channel3TextBox.Text;
            }
        }
        private string Channel4
        {
            get
            {
                return this.channel4TextBox.Text;
            }
        }
        #endregion Program Properties

        #region FormEvents
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseSession();
        }
        private void connectButton_Click(object sender, EventArgs e)
        {
            ChangeControlState(false);
            try
            {
                CloseSession();

                //Open session to the switch module and sets topology
                InitializeSwitchSession();
                //Connect channel1 and channel2.    
                switchSession.Path.Connect(Channel1, Channel2);
                // Wait for any relay to activate and debounce.
                switchSession.Path.WaitForDebounce(maxTime);

                //Connect channel3 and channel4  
                switchSession.Path.Connect(Channel3, Channel4);
                // Wait for any relay to activate and debounce.
                switchSession.Path.WaitForDebounce(maxTime);
            }
            catch (System.Exception ex)
            {
                ShowError(ex.Message);
            }
            finally
            {
                ChangeControlState(true);
                //Close session to switch module.
                CloseSession();
            }
        }
        #endregion FormEvents
        #region Program Functions
        private void ChangeControlState(bool isEnabled)
        {
            this.connectButton.Enabled = isEnabled;
            this.resourceNameComboBox.Enabled = isEnabled;
            this.topologyNameComboBox.Enabled = isEnabled;
            this.channel1TextBox.Enabled = isEnabled;
            this.channel2TextBox.Enabled = isEnabled;
            this.channel3TextBox.Enabled = isEnabled;
            this.channel4TextBox.Enabled = isEnabled;
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
        #endregion Program Functions
    }
}
