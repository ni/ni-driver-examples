//==================================================================================================
// Title        : Analog Bus Sharing Enable NI2815
// Description  :  Use this example to learn how to enable sharing and connect of analog bus channels of two different devices. The example shows how to configure NI-Switch to facilitate enabling of analog bus channels, Reserve specific channels for routing , Connect and Disconnect the channels and Reset the two Devices.  
//==================================================================================================
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NISwitch;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;


namespace NationalInstruments.Examples.AnalogBusSharingEnableNI2815
{
    public partial class MainForm : Form
    {
        NISwitch switchSession1;
        NISwitch switchSession2;
        List<string> analogBusChannels = new List<string> { "ab0", "ab1", "ab2", "ab3" };
        List<string> rowChannels = new List<string> { "card1r0", "card1r1", "card1r2", "card1r3" };
        PrecisionTimeSpan maximumTime = new PrecisionTimeSpan(5);

        public MainForm()
        {
            InitializeComponent();
            LoadSwitchDeviceNames();
            LoadTopology();
            LoadAnalogBusChannels();
            LoadColumnChannels();
            ToggleConnectDisconnectButton(true);

        }

        #region UI Initial Value Config Section

        private void ToggleConnectDisconnectButton(bool state)
        {
            connectButton.Enabled = state;
            disconnectButton.Enabled = !state;
        }

        private void LoadAnalogBusChannels()
        {
            foreach (string channel in analogBusChannels)
            {
                analogBusChannelsComboBox.Items.Add(channel);
            }
            analogBusChannelsComboBox.SelectedIndex = 0;
        }

        private void LoadColumnChannels()
        {
            const char columnChannelPrefix = 'c';
            string channelPrefix;
            for (int i = 0; i < 86; i++)
            {
                channelPrefix = columnChannelPrefix + "" + i;
                columnChannel1ComboBox.Items.Add(channelPrefix);
                columnChannel2ComboBox.Items.Add(channelPrefix);
            }
            columnChannel1ComboBox.SelectedIndex = 0;
            columnChannel2ComboBox.SelectedIndex = 0;
        }

        private void LoadTopology()
        {
            Type myType = typeof(SwitchDeviceTopology);
            PropertyInfo[] properties = myType.GetProperties();

            foreach (PropertyInfo prop in properties)
            {
                topologyName1ComboBox.Items.Add(prop.GetValue(myType, null).ToString());
                topologyName2ComboBox.Items.Add(prop.GetValue(myType, null).ToString());
            }
            topologyName1ComboBox.SelectedIndex = 0;
            topologyName2ComboBox.SelectedIndex = 0;
        }

        private void LoadSwitchDeviceNames()
        {
            ModularInstrumentsSystem modularInstrumentsSystem = new ModularInstrumentsSystem("NI-SWITCH");
            foreach (DeviceInfo device in modularInstrumentsSystem.DeviceCollection)
            {
                resourceName1ComboBox.Items.Add(device.Name);
                resourceName2ComboBox.Items.Add(device.Name);
            }
            if (modularInstrumentsSystem.DeviceCollection.Count > 0)
            {
                resourceName1ComboBox.SelectedIndex = 0;
                resourceName2ComboBox.SelectedIndex = 0;
            }
        }

        #endregion UI Initial Value Config Section

        #region Program Properties

        private string ResourceName1
        {
            get
            {
                return this.resourceName1ComboBox.Text;
            }
        }

        private string ResourceName2
        {
            get
            {
                return this.resourceName2ComboBox.Text;
            }
        }

        private string TopologyName1
        {
            get
            {
                return this.topologyName1ComboBox.SelectedItem.ToString();
            }
        }

        private string TopologyName2
        {
            get
            {
                return this.topologyName2ComboBox.SelectedItem.ToString();
            }
        }

        private string ColumnChannel1
        {
            get
            {
                return this.columnChannel1ComboBox.Text;
            }
        }

        private string ColumnChannel2
        {
            get
            {
                return this.columnChannel2ComboBox.Text;
            }
        }

        private string AnalogBusChannel
        {
            get
            {
                return this.analogBusChannelsComboBox.SelectedItem.ToString();
            }
        }

        #endregion Program Properties

        #region FormEvents

        private void connectButton_Click(object sender, EventArgs e)
        {
            //Disable all controls till connect is performed
            ChangeControlState(false);
            try
            {
                CloseSession(switchSession1);
                CloseSession(switchSession2);

                //Open session to two switch modules and set topology
                switchSession1 = InitializeSwitchSession(ResourceName1, TopologyName1);
                switchSession2 = InitializeSwitchSession(ResourceName2, TopologyName2);

                //Mark analog bus channels as Analog Bus Sharing Enable
                foreach (string channel in analogBusChannels)
                {
                    switchSession1.Channels[channel].AnalogBusSharingEnable = true;
                    switchSession2.Channels[channel].AnalogBusSharingEnable = true;
                }
                //Mark rows as Reserved For Routing.
                foreach (string channel in rowChannels)
                {
                    switchSession1.Channels[channel].IsConfigurationChannel = true;
                    switchSession2.Channels[channel].IsConfigurationChannel = true;
                }

                //Connect specified channels in Device 1.
                switchSession1.Path.Connect(ColumnChannel1, AnalogBusChannel);
                //Wait for relay(s) to activate and debounce.
                switchSession1.Path.WaitForDebounce(maximumTime);

                //Connect specified channels in Device 2.
                switchSession2.Path.Connect(ColumnChannel2, AnalogBusChannel);
                //Wait for relay(s) to activate and debounce.
                switchSession2.Path.WaitForDebounce(maximumTime);

                //Enable Disconnect when Connect is performed.
                ToggleConnectDisconnectButton(false);

            }
            catch (System.Exception ex)
            {
                ShowError(ex.Message);
                CloseSession(switchSession1);
                CloseSession(switchSession2);
                ChangeControlState(true);
                ToggleConnectDisconnectButton(true);
            }

        }

        private void disconnectButton_Click(object sender, EventArgs e)
        {
            try
            {
                //Disconnect specified channels.
                switchSession1.Path.Disconnect(ColumnChannel1, AnalogBusChannel);
                switchSession2.Path.Disconnect(ColumnChannel2, AnalogBusChannel);

                //Reset the switch modules to disable analog bus sharing.
                switchSession1.Utility.Reset();
                switchSession2.Utility.Reset();

                ToggleConnectDisconnectButton(true);
            }
            catch (System.Exception ex)
            {
                ShowError(ex.Message);
            }
            finally
            {
                //Close session to switch module.
                CloseSession(switchSession1);
                CloseSession(switchSession2);
                ChangeControlState(true);
            }
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseSession(switchSession1);
            CloseSession(switchSession2);
        }

        #endregion FormEvents

        #region Program Functions

        private void ChangeControlState(bool isEnabled)
        {
            bool connectButtonState = this.connectButton.Enabled;
            this.connectButton.Enabled = isEnabled;
            this.disconnectButton.Enabled = isEnabled;
            this.resourceName1ComboBox.Enabled = isEnabled;
            this.resourceName2ComboBox.Enabled = isEnabled;
            this.topologyName1ComboBox.Enabled = isEnabled;
            this.topologyName2ComboBox.Enabled = isEnabled;
            this.columnChannel1ComboBox.Enabled = isEnabled;
            this.columnChannel2ComboBox.Enabled = isEnabled;
            this.analogBusChannelsComboBox.Enabled = isEnabled;
            if (isEnabled)
            {
                this.connectButton.Enabled = connectButtonState;
                this.disconnectButton.Enabled = !connectButtonState;
            }
        }

        private static void ShowError(string message)
        {
            if (string.IsNullOrEmpty(message))
                message = "Unexpected Error";
            MessageBox.Show(message, "Error"); ;
        }

        private NISwitch InitializeSwitchSession(string resourceName, string topologyName)
        {
            NISwitch switchSession;
            switchSession = new NISwitch(resourceName, topologyName, false, true);
            switchSession.DriverOperation.Warning += new System.EventHandler<SwitchWarningEventArgs>(DriverOperationWarning);
            return switchSession;
        }

        private void DriverOperationWarning(object sender, SwitchWarningEventArgs e)
        {
            MessageBox.Show(e.ToString(), "Warning");
        }

        private static void CloseSession(NISwitch switchSession)
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
