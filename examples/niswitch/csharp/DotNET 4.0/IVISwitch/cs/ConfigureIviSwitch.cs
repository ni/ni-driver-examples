using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Ivi.Swtch;
using System.Resources;
using System.Reflection;
namespace NationalInstruments.Examples.IviSwitch
{
    public partial class ConfigureIviSwitch : Form
    {
        private Ivi.Swtch.IIviSwtch switchSession;
        int numberOfChannels;


        public ConfigureIviSwitch()
        {
            InitializeComponent();
        }

        public ConfigureIviSwitch(Ivi.Swtch.IIviSwtch switchSession)
        {
            this.switchSession = switchSession;
            InitializeComponent();
            LoadActions();
            LoadChannels();
        }

        private void LoadChannels()
        {
            List<IIviSwtchChannel> x = switchSession.Channels.ToList();
            List<string> channelList1 = new List<string>();
            List<string> channelList2 = new List<string>();
            numberOfChannels = switchSession.Channels.Count;


            for (int i = 0; i < numberOfChannels; i++)
            {
                channelList1.Add(x[i].Name);
                channelList2.Add(x[i].Name);
            }
            // channel1ComboBox.DisplayMember = "Key";
            // channel1ComboBox.ValueMember = "Value";
            channel1ComboBox.DataSource = channelList1;
            channel1ComboBox.SelectedIndex = 10;
            //channel2ComboBox.DisplayMember = "Key";
            //channel2ComboBox.ValueMember = "Value";
            channel2ComboBox.DataSource = channelList2;
            channel2ComboBox.SelectedIndex = 0;
            // throw new NotImplementedException();
        }

        private void LoadActions()
        {
            List<KeyValuePair<string, int>> actionsList = new List<KeyValuePair<string, int>>();
            actionsList.Add(new KeyValuePair<string, int>("Connect Channels", 0));
            actionsList.Add(new KeyValuePair<string, int>("Disconnect Specific Channels", 1));
            actionsList.Add(new KeyValuePair<string, int>("Disconnect All Channels", 2));
            actionsList.Add(new KeyValuePair<string, int>("Get Switch Path", 3));

            selectActionComboBox.DisplayMember = "Key";
            selectActionComboBox.ValueMember = "Value";
            selectActionComboBox.DataSource = actionsList;
            selectActionComboBox.SelectedIndex = 0;

        }

        private void runPanelButton_Click(object sender, EventArgs e)
        {
            PathCapability canConnect;
            string[] switchPath;
            int selectedEvent = Action;
            ResourceManager resourceMnger = new ResourceManager("NationalInstruments.Examples.IviSwitch.Properties.Resources", Assembly.GetExecutingAssembly());

            try
            {

                switch (selectedEvent)
                {
                    case 0:
                        canConnect = switchSession.Path.CanConnect(Channel1, Channel2);

                        if (canConnect == PathCapability.Available)
                        {
                            switchSession.Path.Connect(Channel1, Channel2);
                            switchPath = switchSession.Path.GetPath(Channel1, Channel2);
                            switchPathTextBox.Text = string.Join(",", switchPath);
                            canConnectRichTextBox.Text = resourceMnger.GetString("Available");

                        }
                        else
                            switchPathTextBox.Text = string.Empty;
                        if (canConnect == PathCapability.Exists)
                        {
                            canConnectRichTextBox.Text = resourceMnger.GetString("Exists");

                        }
                        if (canConnect == PathCapability.ResourceInUse)
                        {
                            canConnectRichTextBox.Text = resourceMnger.GetString("InUse");

                        }
                        if (canConnect == PathCapability.Unsupported)
                        {
                            canConnectRichTextBox.Text = resourceMnger.GetString("Unsupported");

                        }
                        if (canConnect == PathCapability.SourceConflict)
                        {
                            canConnectRichTextBox.Text = resourceMnger.GetString("Conflict");

                        }
                        break;

                    case 1:
                        switchSession.Path.Disconnect(Channel1, Channel2);
                        switchPathTextBox.Text = string.Empty;
                        canConnectRichTextBox.Text = string.Empty;

                        break;

                    case 2:
                        switchSession.Path.DisconnectAll();
                        switchPathTextBox.Text = string.Empty;
                        canConnectRichTextBox.Text = string.Empty;

                        break;
                    case 3:
                        switchPath = switchSession.Path.GetPath(Channel1, Channel2);
                        switchPathTextBox.Text = string.Join(",", switchPath);
                        canConnectRichTextBox.Text = string.Empty;

                        break;
                }

                resultActionPanel.BackColor = System.Drawing.Color.LimeGreen;
            }
            catch (Exception ex)
            {
                resultActionPanel.BackColor = System.Drawing.Color.DarkOliveGreen;
                ShowError(ex.Message);
                CloseSession();
            }
        }

        public int Action
        {
            get
            {
                return (int)selectActionComboBox.SelectedValue;
            }
        }

        public string Channel1
        {
            get
            {
                return (string)channel1ComboBox.SelectedItem;
            }
        }
        public string Channel2
        {
            get
            {
                return (string)channel2ComboBox.SelectedItem;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseSession();
        }
        private void closeButton_Click(object sender, EventArgs e)
        {
            CloseSession();
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
                finally
                {
                    this.Close();
                }
            }
        }
        private static void ShowError(string message)
        {
            if (string.IsNullOrEmpty(message))
                message = "Unexpected Error";
            MessageBox.Show(message, "Error"); ;
        }
    }
}
