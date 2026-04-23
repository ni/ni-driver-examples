//=============================================================================================================
//
// Title:
//      IVI DCPower .NET application
//      
// Description:
//      This example is a IVI DCPower .NET application. This example uses only those
//      interfaces from the IVI class for DC Power Supplies to configure the DCPower device.
//
//============================================================================================================

using System;
using System.Windows.Forms;
using Ivi.DCPwr;

namespace NationalInstruments.Examples.IviDCPowerDotNetApplication
{
    public partial class MainForm : Form
    {
        IIviDCPwr iviDCPwrSession;

        public MainForm()
        {
            InitializeComponent();
            configureDCPowerGroupBox.Enabled = false;
        }

        #region MainForm initial configuration
        void ConfigureCurrentLimitComboBox()
        {
            foreach (CurrentLimitBehavior item in Enum.GetValues(typeof(CurrentLimitBehavior)))
            {
                currentLimitBehaviorComboBox.Items.Add(item);
            }
            currentLimitBehaviorComboBox.SelectedIndex = 0;
        }

        void ConfigureChannelNameComboBox()
        {
            foreach (IIviDCPwrOutput channel in iviDCPwrSession.Outputs)
            {
                channelNameComboBox.Items.Add(channel.Name);
            }
            if (channelNameComboBox.Items.Count > 0)
            {
                channelNameComboBox.SelectedIndex = 0;
            }
        }

        void ChangeControlToConfiguration()
        {
            ConfigureChannelNameComboBox();
            ConfigureCurrentLimitComboBox();
            initializeDCPowerGroupBox.Enabled = false;
            configureDCPowerGroupBox.Enabled = true;
            channelNameComboBox.Select();
            this.Refresh();
        }
        #endregion

        #region MainForm configuration values
        string LogicalName
        {
            get
            {
                return this.logicalNameTextBox.Text;
            }
        }

        string ChannelName
        {
            get
            {
                return this.channelNameComboBox.Text;
            }
        }

        bool IdQuery
        {
            get
            {
                return this.idQueryCheckBox.Checked;
            }
        }

        bool ResetDevice
        {
            get
            {
                return this.resetDeviceCheckBox.Checked;
            }
        }

        bool OvpEnabled
        {
            get
            {
                return this.outputEnabledCheckBox.Checked;
            }
        }

        bool OutputEnabled
        {
            get
            {
                return this.outputEnabledCheckBox.Checked;
            }
        }

        double VoltageLevel
        {
            get
            {
                return decimal.ToDouble(this.voltageLevelNumeric.Value);
            }
        }

        double OvpLimit
        {
            get
            {
                return decimal.ToDouble(this.ovpLimitNumeric.Value);
            }
        }

        double CurrentLimit
        {
            get
            {
                return decimal.ToDouble(this.currentLimitNumeric.Value);
            }
        }

        CurrentLimitBehavior CurrentLimitBehavior
        {
            get
            {
                return (CurrentLimitBehavior)this.currentLimitBehaviorComboBox.SelectedItem;
            }
        }
        #endregion

        void configureAndOutputButton_Click(object sender, System.EventArgs e)
        {
            ConfigureAndOutput();
        }

        void initializeButton_Click(object sender, EventArgs e)
        {
            Initialize();
        }

        void mainForm_Closing(object sender, FormClosingEventArgs e)
        {
            CloseSession();
        }

        void Initialize()
        {
            try
            {
                iviDCPwrSession = IviDCPwr.Create(LogicalName, IdQuery, ResetDevice);
                ChangeControlToConfiguration();
                ShowOperationSuccessful("Initialize()");
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        void ConfigureAndOutput()
        {
            try
            {
                // Disable the output to prevent accidental trip conditions.
                iviDCPwrSession.Outputs[ChannelName].Enabled = false;

                if (OutputEnabled)
                {
                    // Set voltage to 0.0 to prevent accidental trip conditions.
                    iviDCPwrSession.Outputs[ChannelName].VoltageLevel = 0.0;

                    iviDCPwrSession.Outputs[ChannelName].ConfigureCurrentLimit(CurrentLimitBehavior, CurrentLimit);
                    iviDCPwrSession.Outputs[ChannelName].ConfigureOvp(OvpEnabled, OvpLimit);
                    iviDCPwrSession.Outputs[ChannelName].VoltageLevel = VoltageLevel;
                    iviDCPwrSession.Outputs[ChannelName].Enabled = true;
                }

                ShowOperationSuccessful("ConfigureAndOutput()");
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        void CloseSession()
        {
            if (iviDCPwrSession != null)
            {
                try
                {
                    iviDCPwrSession.Close();
                    iviDCPwrSession = null;
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

        static void ShowOperationSuccessful(string operation)
        {
            MessageBox.Show(operation + " completed successfully.");
        }

    }
}