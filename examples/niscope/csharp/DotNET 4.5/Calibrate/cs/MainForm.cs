//=========================================================================================================
//
// Title:
//      Calibrate
//
// Description:
//      This example performs a self-calibration of your digitizer.  Disconnect or disable 
//      any AC input signals before starting self-calibration.  AC or varying signals can,
//      in some cases, cause self-calibration to fail or compromise the accuracy of the
//      calibration.
//
//=========================================================================================================

using System;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIScope;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.Calibrate
{
    public partial class MainForm : Form
    {
        NIScope scopeSession;

        public MainForm()
        {
            InitializeComponent();
            ConfigureOptionComboBox();
            LoadScopeDeviceNames();
            ChangeControlState(true);
        }

        #region Mainform Initial Configuration
        void ConfigureOptionComboBox()
        {
            foreach (ScopeSelfCalibrationOption value in Enum.GetValues(typeof(ScopeSelfCalibrationOption)))
            {
                optionComboBox.Items.Add(value);
            }
            optionComboBox.SelectedIndex = 0;
        }

        void LoadScopeDeviceNames()
        {
            using (ModularInstrumentsSystem scopeDevices = new ModularInstrumentsSystem("NI-Scope"))
            {
                foreach (DeviceInfo device in scopeDevices.DeviceCollection)
                {
                    resourceNameComboBox.Items.Add(device.Name);
                }
            }
            if (resourceNameComboBox.Items.Count > 0)
            {
                resourceNameComboBox.SelectedIndex = 0;
            }
        }
        #endregion

        #region Mainform Configuration values
        string ResourceName
        {
            get
            {
                return this.resourceNameComboBox.Text;
            }
        }

        ScopeSelfCalibrationOption Option
        {
            get
            {
                return (ScopeSelfCalibrationOption)this.optionComboBox.SelectedItem;
            }
        }
        #endregion

        void calibrateButton_Click(object sender, System.EventArgs e)
        {
            SelfCalibrate();
        }

        void mainForm_Closing(object sender, FormClosingEventArgs e)
        {
            CloseSession();
        }

        void DriverOperation_Warning(object sender, ScopeWarningEventArgs e)
        {
            MessageBox.Show(e.Text, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        void InitializeSession()
        {
            scopeSession = new NIScope(ResourceName, false, false);
            scopeSession.DriverOperation.Warning += new EventHandler<ScopeWarningEventArgs>(DriverOperation_Warning);
        }

        void SelfCalibrate()
        {
            ChangeControlState(false);
            DisplayMessage("Calibration in Progress (May take a couple of mintues)...");

            try
            {
                InitializeSession();
                scopeSession.Calibration.Self.SelfCalibrate(Option);
                DisplayMessage("Calibration successful!");
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
            finally
            {
                CloseSession();
                ChangeControlState(true);
            }
        }

        void DisplayMessage(string message)
        {
            messageTextBox.Text = message;
            this.Refresh();
        }

        void ChangeControlState(bool isEnabled)
        {
            resourceNameComboBox.Enabled = isEnabled;
            optoinGroupBox.Enabled = isEnabled;
            this.Refresh();
        }

        void CloseSession()
        {
            if (scopeSession != null)
            {
                try
                {
                    scopeSession.Close();
                    scopeSession = null;
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
    }
}